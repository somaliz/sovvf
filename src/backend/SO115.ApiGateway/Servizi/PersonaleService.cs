﻿using Newtonsoft.Json;
using SO115App.ApiGateway.Classi;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SO115App.ApiGateway.Servizi
{
    public class PersonaleService
    {
        private HttpClient client = new HttpClient();

        public async Task<List<SquadreNelTurno>> GetSquadreNelTurno(string codiceSede, string codiceTurno)
        {
            List<SquadreNelTurno> ListaSquadreTurno = new List<SquadreNelTurno>();
            var response = await client.GetStringAsync(string.Format(Costanti.ServiziSquadreUrl + "/GetSquadreNelTurno/codiceSede={0}&codiceTurno={1}", codiceSede, codiceTurno));
            List<SquadreNelTurno> ListTurno = JsonConvert.DeserializeObject<List<SquadreNelTurno>>(response);

            foreach (var turno in ListTurno)
            {
                foreach (var squadra in turno.ListaSquadre)
                {
                    List<Componente> ListaComponenti = new List<Componente>();
                    squadra.ListaComponenti = new List<Componente>();
                    var responseComponenti = await client.GetStringAsync(string.Format(Costanti.ServiziComponentiUrl + "/codiceSede={0}&codiceSquadra={1}&codiceTurno={2}", codiceSede, squadra.Codice, turno.Codice));
                    ListaComponenti = JsonConvert.DeserializeObject<List<Componente>>(responseComponenti);
                    squadra.ListaComponenti = ListaComponenti;
                }
            }

            return null;
        }

        public async Task<List<SquadreNelTurno>> GetSquadreBySede(string codiceSede)
        {
            List<SquadreNelTurno> ListaSquadreTurno = new List<SquadreNelTurno>();
            var response = await client.GetStringAsync(string.Format(Costanti.ServiziSquadreUrl + "/GetSquadreBySede/codiceSede={0}", codiceSede));
            var ListTurno = JsonConvert.DeserializeObject<List<SquadreNelTurno>>(response);

            foreach (var turno in ListTurno)
            {
                foreach (var squadra in turno.ListaSquadre)
                {
                    List<Componente> ListaComponenti = new List<Componente>();
                    squadra.ListaComponenti = new List<Componente>();
                    var responseComponenti = await client.GetStringAsync(string.Format(Costanti.ServiziComponentiUrl + "/codiceSede={0}&codiceSquadra={1}&codiceTurno={2}", codiceSede, squadra.Codice, turno.Codice));
                    ListaComponenti = JsonConvert.DeserializeObject<List<Componente>>(responseComponenti);
                    squadra.ListaComponenti = ListaComponenti;
                }
            }

            return null;
        }
    }
}
