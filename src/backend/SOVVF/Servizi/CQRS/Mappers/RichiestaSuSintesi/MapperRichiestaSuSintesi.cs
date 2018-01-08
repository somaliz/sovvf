﻿//-----------------------------------------------------------------------
// <copyright file="MapperRichiestaSuSintesi.cs" company="CNVVF">
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
using System.Linq;
using Modello.Classi.Soccorso;
using Modello.Servizi.CQRS.Queries.GestioneSoccorso.Shared.SintesiRichiestaAssistenza;
using Modello.Servizi.Infrastruttura.Organigramma;

namespace Modello.Servizi.CQRS.Mappers.RichiestaSuSintesi
{
    /// <summary>
    ///   Implementazione del servizio di mapping di <see cref="RichiestaAssistenza" /> sul DTO <see cref="SintesiRichiesta" />.
    /// </summary>
    internal class MapperRichiestaSuSintesi : IMapperRichiestaSuSintesi
    {
        /// <summary>
        ///   Istanza del servizio
        /// </summary>
        private readonly IGetUnitaOperativaPerCodice getUnitaOperativaPerCodice;

        /// <summary>
        ///   Istanza del servizio
        /// </summary>
        private readonly MapperMezzoSuSintesi mapperMezzoSuSintesi;

        /// <summary>
        ///   Costruttore della classe
        /// </summary>
        /// <param name="getUnitaOperativaPerCodice">
        ///   L'istanza del servizio di risoluzione unità operativa a partire dal codice
        /// </param>
        /// <param name="mapperMezzoSuSintesi">
        ///   L'istanza del servizio di mapping di un mezzo sulla sintesi
        /// </param>
        public MapperRichiestaSuSintesi(
            IGetUnitaOperativaPerCodice getUnitaOperativaPerCodice,
            MapperMezzoSuSintesi mapperMezzoSuSintesi)
        {
            this.getUnitaOperativaPerCodice = getUnitaOperativaPerCodice;
            this.mapperMezzoSuSintesi = mapperMezzoSuSintesi;
        }

        /// <summary>
        ///   Esegue il mapping di <see cref="RichiestaAssistenza" /> sul DTO <see cref="SintesiRichiesta" />.
        /// </summary>
        /// <param name="richiesta">La richiesta da mappare</param>
        /// <returns>Il DTO risultante dal mapping</returns>
        public SintesiRichiesta Map(RichiestaAssistenza richiesta)
        {
#warning Sarebbe conveniente usare la libreria AutoMapper per garantire la copertura completa
            return new SintesiRichiesta()
            {
                Id = richiesta.Id,
                Codice = richiesta.Codice,
                Rilevante = richiesta.Rilevante,
                IstanteRicezioneRichiesta = richiesta.IstanteRicezioneRichiesta,
                IstantePrimaAssegnazione = richiesta.IstantePrimaAssegnazione,
                Presidiato = richiesta.Presidiato,
                PrioritaRichiesta = richiesta.PrioritaRichiesta,
                Tipologie = richiesta.Tipologie.Select(t => t.Descrizione).ToArray(),
                Descrizione = richiesta.Descrizione,
                Richiedente = richiesta.Richiedente,
                NumeroRichiedente = richiesta.NumeroRichiedente,
                DescrizioneLocalita = richiesta.Indirizzo,
                DescrizioneCompetenze = richiesta.CodiciUOCompetenza.Select(cod => this.getUnitaOperativaPerCodice.Get(cod).Nome).ToArray(),
                NoteLocalita = richiesta.NoteLocalita,
                ZoneEmergenza = richiesta.ZoneEmergenza,
                IstantePresaInCarico = richiesta.IstantePresaInCarico,
                CodiceSchedaNue = richiesta.CodiceSchedaNue,
                StatoFonogrammaRichiesta = richiesta.StatoInvioFonogramma.Codice,
                DescrizioneStatoFonogramma = richiesta.StatoInvioFonogramma.Descrizione,
                ComplessitaRichiesta = richiesta.Complessita.Codice,
                DescrizioneComplessita = richiesta.Complessita.Descrizione,
                Mezzi = richiesta.MezziCoinvolti.Select(mc => this.mapperMezzoSuSintesi.Map(mc.Key, mc.Value)).ToArray()
            };
        }
    }
}