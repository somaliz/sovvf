﻿//-----------------------------------------------------------------------
// <copyright file="GetComposizioneMezzi.cs" company="CNVVF">
// Copyright (C) 2017 - CNVVF
//
// This file is part of SOVVF.
// SOVVF is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// SOVVF is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/.
// </copyright>
//-----------------------------------------------------------------------
using Newtonsoft.Json;
using SO115App.API.Models.Classi.Condivise;
using SO115App.API.Models.Classi.Marker;
using SO115App.API.Models.Servizi.Infrastruttura.GestioneSoccorso.Mezzi;
using SO115App.FakePersistenceJSon.GestioneIntervento;
using SO115App.Models.Classi.ListaMezziInServizio;
using SO115App.Models.Classi.Utility;
using SO115App.Models.Servizi.Infrastruttura.GestioneSoccorso;
using SO115App.Models.Servizi.Infrastruttura.InfoRichiesta;
using SO115App.Models.Servizi.Infrastruttura.SistemiEsterni.Gac;
using System.Collections.Generic;

namespace SO115App.FakePersistenceJSon.GestioneMezzi
{
    /// <summary>
    ///   La classe recupera i dati di tutti i mezzi utilizzabili dal gac e i dati delle richieste
    ///   associate ai mezzi e restistuisce una lista di mezzi in servizio completa di tutte le
    ///   informazioni (squadre sul mezzo, richiesta a cui il mezzo e associato e il mezzo stesso)
    /// </summary>
    public class GetListaMezzi : IGetListaMezzi
    {
        private readonly IGetInfoRichiesta _getInfoRichiesta;
        private readonly IGetMezziUtilizzabili _getMezziUtilizzabili;
        private readonly IGetRichiestaById _getRichiestaById;

        public GetListaMezzi(IGetInfoRichiesta getInfoRichiesta, IGetMezziUtilizzabili getMezziUtilizzabili, IGetRichiestaById getRichiestaById)
        {
            _getInfoRichiesta = getInfoRichiesta;
            _getMezziUtilizzabili = getMezziUtilizzabili;
            _getRichiestaById = getRichiestaById;
        }

        /// <summary>
        ///   il metodo restituisce una lista di mezzi in servizio associati ad una determinata sede
        /// </summary>
        /// <param name="codiceSede">il codice sede</param>
        /// <returns>Lista di MezziInServizio</returns>
        public List<MezzoInServizio> Get(string codiceSede)
        {
            var listaMezzoInServizio = new List<MezzoInServizio>();

            var listaCodiciSede = new List<string>
            {
                codiceSede
            };
            var codiceSedeIniziali = codiceSede.Substring(0, 2);

            var mezzi = _getMezziUtilizzabili.Get(listaCodiciSede, "", "");

            foreach (var mezzo in mezzi
                .FindAll(x => x.Distaccamento.Codice
                .StartsWith(codiceSedeIniziali)))
            {
                var mezzoMarker = new MezzoMarker()
                {
                    Mezzo = mezzo,
                    InfoRichiesta = _getInfoRichiesta.GetInfoRichiestaFromIdRichiestaMezzo(mezzo.IdRichiesta)
                };
                var mezzoInServizio = new MezzoInServizio()
                {
                    Mezzo = mezzoMarker
                };

                if (mezzo.IdRichiesta != null)
                {
                    var richiesta = _getRichiestaById.Get(mezzo.IdRichiesta);
                    if (richiesta != null)
                    {
                        foreach (var partenza in richiesta.Partenze)
                        {
                            if (partenza.Partenza.Mezzo.Codice == mezzo.Codice)
                            {
                                mezzoInServizio.Squadre = partenza.Partenza.Squadre;
                            }
                        }
                    }
                }
                else
                {
                    mezzoInServizio.Squadre = null;
                }

                listaMezzoInServizio.Add(mezzoInServizio);
            }

            return listaMezzoInServizio;
        }
    }
}
