﻿//-----------------------------------------------------------------------
// <copyright file="IPartenza.cs" company="CNVVF">
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
using Modello.Classi.Soccorso.Mezzi.StatiMezzo;

namespace Modello.Classi.Soccorso.Eventi.Partenze
{
    /// <summary>
    ///   Indica che è un evento legato ad una partenza
    /// </summary>
    public interface IPartenza : IEvento
    {
        /// <summary>
        ///   Restituisce i codici dei mezzi coinvolti in questo evento
        /// </summary>
        string[] CodiciMezzo { get; }

        /// <summary>
        ///   Restituisce lo stato che il mezzo assume a seguito del verificarsi dell'evento
        /// </summary>
        /// <returns>Lo stato del mezzo</returns>
        IStatoMezzo GetStatoMezzo();
    }
}
