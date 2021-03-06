﻿//-----------------------------------------------------------------------
// <copyright file="AddRuoliUtenteCommand.cs" company="CNVVF">
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
using SO115App.API.Models.Classi.Autenticazione;
using System.Collections.Generic;

namespace SO115App.Models.Servizi.CQRS.Commands.GestioneUtenti.AddRuoliUtente
{
    /// <summary>
    ///   Modello per l'aggiunta dei ruoli ad un utente
    /// </summary>
    public class AddRuoliUtenteCommand
    {
        /// <summary>
        ///   il codice fiscale dell'utente
        /// </summary>
        public string CodFiscale { get; set; }

        /// <summary>
        ///   la lista dei ruoli da aggiungere
        /// </summary>
        public List<Role> Ruoli { get; set; }

        public string CodiceSede { get; set; }
    }
}
