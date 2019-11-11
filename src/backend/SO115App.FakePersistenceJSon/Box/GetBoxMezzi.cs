﻿//-----------------------------------------------------------------------
// <copyright file="GetBoxMezzi.cs" company="CNVVF">
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

using SO115App.API.Models.Classi.Boxes;
using SO115App.Models.Classi.Utility;
using SO115App.Models.Servizi.Infrastruttura.Box;
using SO115App.Models.Servizi.Infrastruttura.SistemiEsterni.Gac;
using System.Collections.Generic;
using System.Linq;

namespace SO115App.FakePersistenceJSon.Box
{
    /// <summary>
    ///   La classe GetBoxMezzi recupera i mezzi utilizzabili dal servizio esterno Gac e aggiorna il
    ///   box dei mezzi
    /// </summary>
    public class GetBoxMezzi : IGetBoxMezzi
    {
        private readonly IGetMezziUtilizzabili _getMezziUtilizzabili;

        public GetBoxMezzi(IGetMezziUtilizzabili getMezziUtilizzabili)
        {
            _getMezziUtilizzabili = getMezziUtilizzabili;
        }

        /// <summary>
        ///   Il metodo accetta in firma il codice sede e recupera i mezzi utilizzabili dal servizio
        ///   esterno Gac e aggiorna i contatori dei mezzi
        /// </summary>
        /// <param name="codiceSede">il codice sede</param>
        /// <returns>BoxMezzi</returns>
        public BoxMezzi Get(string codiceSede)
        {
            var mezzi = new BoxMezzi();
            var listaCodici = new List<string>
            {
                codiceSede
            };

            var listaMezzi = _getMezziUtilizzabili.Get(listaCodici, "", "");

            mezzi.InSede = listaMezzi.Where(x => x.Stato == Costanti.MezzoInSede)
                .Select(x => x.Stato)
                .Count();
            mezzi.InViaggio = listaMezzi.Where(x => x.Stato == Costanti.MezzoInViaggio)
                .Select(x => x.Stato)
                .Count();
            mezzi.InRientro = listaMezzi.Where(x => x.Stato == Costanti.MezzoInRientro)
                .Select(x => x.Stato)
                .Count();
            mezzi.SulPosto = listaMezzi.Where(x => x.Stato == Costanti.MezzoSulPosto)
                .Select(x => x.Stato)
                .Count();
            mezzi.Istituto = listaMezzi.Where(x => x.Stato == Costanti.MezzoIstituto)
                .Select(x => x.Stato)
                .Count();
            mezzi.InServizio = mezzi.InSede + mezzi.InRientro + mezzi.SulPosto + mezzi.Istituto + mezzi.InViaggio;

            return mezzi;
        }
    }
}
