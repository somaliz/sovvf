﻿//-----------------------------------------------------------------------
// <copyright file="SchedeNueMarkerQueryHandler.cs" company="CNVVF">
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
using CQRS.Queries;
using SO115App.Models.Servizi.Infrastruttura.SistemiEsterni.Nue;
using System.Linq;

namespace SO115App.API.Models.Servizi.CQRS.Queries.Marker.SchedeNueMarker
{
    public class SchedeNueMarkerQueryHandler : IQueryHandler<SchedeNueMarkerQuery, SchedeNueMarkerResult>
    {
        private readonly IGetSchedeContattoBySpatialArea _iGetSchedeNueMarker;

        /// <summary>
        ///   Costruttore della classe
        /// </summary>
        public SchedeNueMarkerQueryHandler(IGetSchedeContattoBySpatialArea iGetSchedeNueMarker)
        {
            this._iGetSchedeNueMarker = iGetSchedeNueMarker;
        }

        /// <summary>
        ///   Metodo di esecuzione della query
        /// </summary>
        /// <param name="query">Il DTO di ingresso della query</param>
        /// <returns>Il DTO di uscita della query</returns>
        public SchedeNueMarkerResult Handle(SchedeNueMarkerQuery query)
        {
            var SchedeMarker = _iGetSchedeNueMarker.SchedeContattoBySpatialArea(query.Filtro.BottomLeft.Latitudine, query.Filtro.BottomLeft.Longitudine, query.Filtro.TopRight.Latitudine, query.Filtro.TopRight.Longitudine);

            var gruppi = SchedeMarker.GroupBy(x => x.Localita.Coordinate).ToList();

            return new SchedeNueMarkerResult()
            {
                ListaSchedeMarker = SchedeMarker,
                ListaGruppiSchedeMarker = gruppi
            };
        }
    }
}
