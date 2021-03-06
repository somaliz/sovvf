﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using SO115App.API.Models.Classi.Composizione;
using SO115App.API.Models.Classi.Condivise;
using SO115App.API.Models.Servizi.CQRS.Queries.GestioneSoccorso.Composizione.ComposizioneMezzi;
using SO115App.Models.Servizi.Infrastruttura.Composizione;
using SO115App.Models.Servizi.Infrastruttura.GestioneSoccorso;
using SO115App.Models.Servizi.Infrastruttura.GetComposizioneMezzi;
using SO115App.Models.Servizi.Infrastruttura.GetFiltri;
using SO115App.Models.Servizi.Infrastruttura.SistemiEsterni.Gac;
using SO115App.Models.Servizi.Infrastruttura.SistemiEsterni.Squadre;
using SO115App.Persistence.MongoDB.GestioneMezzi;

namespace SO115App.ExternalAPI.Fake.Composizione
{
    public class GetComposizioneMezzi : IGetComposizioneMezzi
    {
        private readonly IGetStatoMezzi _getMezziPrenotati;
        private readonly OrdinamentoMezzi _ordinamentoMezzi;
        private readonly IGetMezziUtilizzabili _getMezziUtilizzabili;
        private readonly IGetListaSquadre _getSquadre;
        private readonly IGetFiltri _getFiltri;

        public GetComposizioneMezzi(IGetStatoMezzi getMezziPrenotati, OrdinamentoMezzi ordinamentoMezzi, IGetMezziUtilizzabili getMezziUtilizzabili,
            IGetListaSquadre getSquadre, IGetFiltri getFiltri)

        {
            _getMezziUtilizzabili = getMezziUtilizzabili;
            _getSquadre = getSquadre;
            _getMezziPrenotati = getMezziPrenotati;
            _ordinamentoMezzi = ordinamentoMezzi;
            _getFiltri = getFiltri;
        }

        public List<ComposizioneMezzi> Get(ComposizioneMezziQuery query)
        {
            List<string> ListaSedi = new List<string>();
            ListaSedi.Add(query.CodiceSede);
            List<Mezzo> ListaMezzi = _getMezziUtilizzabili.Get(ListaSedi).Result;

            var composizioneMezzi = GeneraListaComposizioneMezzi(ListaMezzi);
            string[] generiMezzi;
            string[] statiMezzi;
            string codiceDistaccamento;

            foreach (var composizione in composizioneMezzi)
            {
                composizione.IndiceOrdinamento = _ordinamentoMezzi.GetIndiceOrdinamento(query.Filtro.IdRichiesta, composizione, composizione.Mezzo.IdRichiesta);
                composizione.Id = composizione.Mezzo.Codice;

                if (composizione.IstanteScadenzaSelezione < DateTime.Now)
                {
                    composizione.IstanteScadenzaSelezione = null;
                }
            }

            var composizioneMezziPrenotati = GetComposizioneMezziPrenotati(composizioneMezzi, query.CodiceSede);

            return composizioneMezziPrenotati.OrderByDescending(x => x.IndiceOrdinamento).ToList();
            
        }

        private List<ComposizioneMezzi> GetComposizioneMezziPrenotati(List<ComposizioneMezzi> composizioneMezzi, string codiceSede)
        {
            var mezziPrenotati = _getMezziPrenotati.Get(codiceSede);
            foreach (var composizione in composizioneMezzi)
            {
                if (mezziPrenotati.Find(x => x.CodiceMezzo.Equals(composizione.Mezzo.Codice)) != null)
                {
                    composizione.IstanteScadenzaSelezione = mezziPrenotati.Find(x => x.CodiceMezzo.Equals(composizione.Mezzo.Codice)).IstanteScadenzaSelezione;

                    if (composizione.Mezzo.Stato.Equals("In Sede"))
                    {
                        composizione.Mezzo.Stato = mezziPrenotati.Find(x => x.CodiceMezzo.Equals(composizione.Mezzo.Codice)).StatoOperativo;
                    }
                }
            }
            return composizioneMezzi;
        }

        private static List<ComposizioneMezzi> GeneraListaComposizioneMezzi(IEnumerable<Mezzo> listaMezzi)
        {
            var random = new Random();

            return (from mezzo in listaMezzi
                    let kmGen = random.Next(1, 60).ToString()
                    let tempoPer = Convert.ToDouble(kmGen.Replace(".", ",")) / 1.75
                    select new ComposizioneMezzi()
                    {
                        Id = mezzo.Codice,
                        Mezzo = mezzo,
                        Km = kmGen,
                        TempoPercorrenza = Math.Round(tempoPer, 2).ToString(CultureInfo.InvariantCulture),
                    }).ToList();
        }
    }
}
