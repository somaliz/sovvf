﻿using Microsoft.Extensions.Configuration;
using SO115App.API.Models.Classi.Condivise;
using SO115App.Models.Classi.Condivise;
using SO115App.Models.Servizi.Infrastruttura.SistemiEsterni.Gac;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using SO115App.Models.Servizi.Infrastruttura.SistemiEsterni.Distaccamenti;
using SO115App.Models.Servizi.Infrastruttura.Composizione;
using Newtonsoft.Json;
using System.IO;
using SO115App.ExternalAPI.Fake.Classi.DTOFake;
using SO115App.ExternalAPI.Fake.Classi.Gac;
using SO115App.Models.Classi.Utility;
using SO115App.Models.Servizi.Infrastruttura.GeoFleet;
using SO115App.Models.Servizi.Infrastruttura.SistemiEsterni.ServizioSede;
using SO115App.API.Models.Classi.Organigramma;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace SO115App.ExternalAPI.Fake.GestioneMezzi
{
    public class GetMezziUtilizzabili : IGetMezziUtilizzabili

    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly IGetStatoMezzi _getStatoMezzi;
        private readonly IGetDistaccamentoByCodiceSedeUC _getDistaccamentoByCodiceSedeUC;
        private readonly IGetPosizioneByCodiceMezzo _getPosizioneByCodiceMezzo;
        private readonly IGetAlberaturaUnitaOperative _getAlberaturaUnitaOperative;
        private readonly IMemoryCache _memoryCache;

        public GetMezziUtilizzabili(HttpClient client, IConfiguration configuration, IGetStatoMezzi GetStatoMezzi,
            IGetDistaccamentoByCodiceSedeUC GetDistaccamentoByCodiceSedeUC, IGetPosizioneByCodiceMezzo getPosizioneByCodiceMezzo,
            IGetAlberaturaUnitaOperative getAlberaturaUnitaOperative,
            IMemoryCache memoryCache)
        {
            _client = client;
            _configuration = configuration;
            _getStatoMezzi = GetStatoMezzi;
            _getDistaccamentoByCodiceSedeUC = GetDistaccamentoByCodiceSedeUC;
            _getPosizioneByCodiceMezzo = getPosizioneByCodiceMezzo;
            _getAlberaturaUnitaOperative = getAlberaturaUnitaOperative;
            _memoryCache = memoryCache;
        }

        public async Task<List<Mezzo>> Get(List<string> sedi, string genereMezzo = null, string codiceMezzo = null)
        {
            var ListaCodiciSedi = new List<string>();

            var listaSediAlberate = _getAlberaturaUnitaOperative.ListaSediAlberata();
            var pinNodi = new List<PinNodo>();

            foreach (var sede in sedi)
            {
                pinNodi.Add(new PinNodo(sede, true));
            }

            foreach (var figlio in listaSediAlberate.GetSottoAlbero(pinNodi))
            {
                var codice = figlio.Codice;
                string codiceE = "";
                codiceE = ListaCodiciSedi.Find(x => x.Equals(codice));
                if (string.IsNullOrEmpty(codiceE))
                {
                    ListaCodiciSedi.Add(codice);
                }
            }

            var ListAnagraficaMezzo = new List<AnagraficaMezzo>();
            var ListaMezzi = new List<Mezzo>();

            foreach (string CodSede in ListaCodiciSedi)
            {
                List<Mezzo> listaMezziBySede = new List<Mezzo>();
                string nomeCache = "M_" + CodSede.Replace(".", "");
                if (!_memoryCache.TryGetValue(nomeCache, out listaMezziBySede))
                {
                    #region LEGGO DA API ESTERNA

                    //_client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("test");
                    //var response = await _client.GetAsync($"{_configuration.GetSection("DataFakeImplementation").GetSection("UrlAPIMezzi").Value}/GetListaMezziByCodComando?CodComando={CodSede}").ConfigureAwait(false);
                    //response.EnsureSuccessStatusCode();
                    //using HttpContent content = response.Content;
                    //var data = await content.ReadAsStringAsync().ConfigureAwait(false);
                    //var ListaMezziSede = JsonConvert.DeserializeObject<List<MezzoFake>>(data);

                    #endregion LEGGO DA API ESTERNA

                    #region LEGGO DA JSON FAKE

                    var filepath = Costanti.ListaMezzi;
                    string json;
                    using (var r = new StreamReader(filepath))
                    {
                        json = r.ReadToEnd();
                    }
                    var listaMezzi = JsonConvert.DeserializeObject<List<MezzoFake>>(json);
                    listaMezzi.FindAll(x => x.Sede.Equals(CodSede)).ToList();
                    var ListaMezziSede = listaMezzi.FindAll(x => x.Sede.Equals(CodSede)).ToList();

                    #endregion LEGGO DA JSON FAKE

                    List<Mezzo> listaMezziBySedeAppo = new List<Mezzo>();
                    foreach (MezzoFake mezzoFake in ListaMezziSede)
                    {
                        if (!mezzoFake.CodDestinazione.Equals("CMOB"))
                        {
                            var anagraficaMezzo = GetAnagraficaMezzoByTarga(mezzoFake.Targa).Result;

                            var mezzo = MapMezzo(anagraficaMezzo, mezzoFake);
                            listaMezziBySedeAppo.Add(mezzo);
                            ListaMezzi.Add(mezzo);
                        }
                    }

                    var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(8));
                    _memoryCache.Set(nomeCache, listaMezziBySedeAppo, cacheEntryOptions);
                }
                else
                {
                    ListaMezzi.AddRange(listaMezziBySede);
                }
            }

            return GetListaMezziConStatoAggiornat(ListaMezzi);
        }

        private List<Mezzo> GetListaMezziConStatoAggiornat(List<Mezzo> listaMezzi)
        {
            foreach (Mezzo mezzo in listaMezzi)
            {
                var ListaStatoOperativoMezzo = _getStatoMezzi.Get(mezzo.Distaccamento.Codice, mezzo.Codice);
                if (ListaStatoOperativoMezzo.Count > 0)
                {
                    mezzo.Stato = ListaStatoOperativoMezzo.Find(x => x.CodiceMezzo.Equals(mezzo.Codice)).StatoOperativo;
                }
            }

            return listaMezzi;
        }

        private Mezzo MapMezzo(AnagraficaMezzo anagraficaMezzo, MezzoFake mezzoFake)
        {
            var distaccamento = new Distaccamento();
            var coordinate = new Coordinate(0, 0);

            distaccamento = _getDistaccamentoByCodiceSedeUC.Get(mezzoFake.Sede).Result;
            var sede = new Sede(mezzoFake.Sede, distaccamento.DescDistaccamento, distaccamento.Indirizzo, distaccamento.Coordinate, "", "", "", "", "");

            var coordinateMezzo = _getPosizioneByCodiceMezzo.Get(anagraficaMezzo.GenereMezzo.CodiceTipo + "." + anagraficaMezzo.Targa).Result;
            if (coordinateMezzo != null)
            {
                coordinate = new Coordinate(coordinateMezzo.Localizzazione.Lat, coordinateMezzo.Localizzazione.Lon);
            }
            else
            {
                coordinate = new Coordinate(sede.Coordinate.Latitudine, sede.Coordinate.Longitudine);
            }

            Mezzo mezzo = new Mezzo(anagraficaMezzo.GenereMezzo.CodiceTipo + "." + anagraficaMezzo.Targa,
                anagraficaMezzo.Targa,
                anagraficaMezzo.GenereMezzo.Codice,
                GetStatoOperativoMezzo(anagraficaMezzo.Sede.Id, anagraficaMezzo.GenereMezzo.CodiceTipo + "." + anagraficaMezzo.Targa, mezzoFake.Stato),
               mezzoFake.CodDestinazione, sede, coordinate)
            {
                DescrizioneAppartenenza = mezzoFake.DescDestinazione,
            };

            return mezzo;
        }

        private async Task<AnagraficaMezzo> GetAnagraficaMezzoByTarga(string targaMezzo)
        {
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("test");
            var response = await _client.GetAsync($"{_configuration.GetSection("UrlExternalApi").GetSection("MezziApidipvvf").Value}?searchKey={targaMezzo}").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            using HttpContent contentMezzo = response.Content;
            var data = await contentMezzo.ReadAsStringAsync().ConfigureAwait(false);

            var listaAnagraficaMezzo = new List<AnagraficaMezzo>();
            listaAnagraficaMezzo = JsonConvert.DeserializeObject<List<AnagraficaMezzo>>(data);
            var anagraficaMezzo = listaAnagraficaMezzo.Find(x => x.Targa.Equals(targaMezzo));
            return anagraficaMezzo;
        }

        private string GetStatoOperativoMezzo(string codiceSedeDistaccamento, string codiceMezzo, string StatoMezzoOra)
        {
            string stato;
            if (StatoMezzoOra.Equals("I"))
            {
                stato = Costanti.MezzoSulPosto;
            }
            else
            {
                var ListaStatoOperativoMezzo = _getStatoMezzi.Get(codiceSedeDistaccamento, codiceMezzo);
                if (ListaStatoOperativoMezzo.Count == 0)
                {
                    switch (StatoMezzoOra)
                    {
                        case "D": stato = Costanti.MezzoInSede; break;
                        case "R": stato = Costanti.MezzoInRientro; break;
                        case "O": stato = Costanti.MezzoOperativoPreaccoppiato; break;
                        case "A": stato = Costanti.MezzoAssegnatoPreaccoppiato; break;
                        default: stato = Costanti.MezzoStatoSconosciuto; break;
                    }
                }
                else
                {
                    stato = ListaStatoOperativoMezzo.Find(x => x.CodiceMezzo.Equals(codiceMezzo)).StatoOperativo;
                }
            }
            return stato;
        }
    }
}
