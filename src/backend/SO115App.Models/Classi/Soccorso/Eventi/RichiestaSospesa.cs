﻿//-----------------------------------------------------------------------
// <copyright file="RichiestaSospesa.cs" company="CNVVF">
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
using System;

namespace SO115App.API.Models.Classi.Soccorso.Eventi
{
    /// <summary>
    ///   Questo evento indica che è la richiesta assistenza è stata sospesa
    /// </summary>
    public class RichiestaSospesa : Evento
    {
        /// <summary>
        ///   Costruttore della classe. A seguito della chiamata, la richiesta risulta sospesa.
        /// </summary>
        ///<param name="motivazione">E' la motivazione della sospensione</param>
        /// <param name="richiesta">La richiesta alla quale l'evento deve essere aggiunto</param>
        /// <param name="istante">E' l'istante in cui si verifica l'evento</param>
        /// <param name="codiceFonte">E' la fonte informativa dell'evento</param>
        public RichiestaSospesa(string motivazione,
            RichiestaAssistenza richiesta,
            DateTime istante,
            string codiceFonte) : base(richiesta, istante, codiceFonte, "RichiestaSospesa")
        {
            richiesta.IstanteChiusura = null;
            this.Motivazione = motivazione;
        }

        [JsonConstructor]
        public RichiestaSospesa(
            string codice,
            DateTime istante,
            string codiceFonte) : base(istante, codiceFonte, codice, "RichiestaSospesa")
        {
        }

        /// <summary>
        ///   Indica la motivazione della sospensione
        /// </summary>
        public string Motivazione { get; set; }
    }
}
