﻿//-----------------------------------------------------------------------
// <copyright file="InSede.cs" company="CNVVF">
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
using System;
using System.Diagnostics.CodeAnalysis;
using Modello.Classi.Soccorso.Eventi.Eccezioni;
using Modello.Classi.Soccorso.Eventi.Partenze;

namespace Modello.Classi.Soccorso.Mezzi.StatiMezzo
{
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly", Justification = "https://stackoverflow.com/questions/37189518/stylecop-warning-sa1126prefixcallscorrectly-on-name-of-class")]

    /// <summary>
    ///   Presente presso la sede di servizio
    /// </summary>
    public class InSede : AbstractStatoMezzo
    {
        /// <summary>
        ///   Codice identificativo dello stato
        /// </summary>
        public override string Codice
        {
            get
            {
                return nameof(InSede);
            }
        }

        /// <summary>
        ///   In questo stato il mezzo risulta disponibile per l'assegnazione
        /// </summary>
        public override bool Disponibile
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        ///   In questo stato il mezzo non risulta assegnato alla richiesta
        /// </summary>
        public override bool AssegnatoARichiesta
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Nello stato <see cref="InSede"/> non può essere gestito l'evento <see cref="ArrivoSulPosto"/>  
        /// </summary>
        /// <param name="arrivoSulPosto">Il visitor</param>
        /// <returns>Nulla perché solleva un'eccezione</returns>
        public override IStatoMezzo AcceptVisitor(ArrivoSulPosto arrivoSulPosto)
        {
            throw new WorkflowException();
        }

        /// <summary>
        /// Nello stato <see cref="InSede"/> non può essere gestito l'evento <see cref="VaInFuoriServizio"/>  
        /// </summary>
        /// <param name="vaInFuoriServizio">Il visitor</param>
        /// <returns>Nulla perché solleva un'eccezione</returns>
        public override IStatoMezzo AcceptVisitor(VaInFuoriServizio vaInFuoriServizio)
        {
            throw new WorkflowException();
        }

        /// <summary>
        /// Nello stato <see cref="InSede"/> non può essere gestito l'evento <see cref="UscitaPartenza"/>  
        /// </summary>
        /// <param name="uscitaPartenza">Il visitor</param>
        /// <returns>Nulla perché solleva un'eccezione</returns>
        public override IStatoMezzo AcceptVisitor(UscitaPartenza uscitaPartenza)
        {
            throw new WorkflowException();
        }

        /// <summary>
        /// Nello stato <see cref="InSede"/> non può essere gestito l'evento <see cref="Revoca"/>  
        /// </summary>
        /// <param name="revoca">Il visitor</param>
        /// <returns>Nulla perché solleva un'eccezione</returns>
        public override IStatoMezzo AcceptVisitor(Revoca revoca)
        {
            throw new WorkflowException();
        }

        /// <summary>
        /// Nello stato <see cref="InSede"/> non può essere gestito l'evento <see cref="PartenzaInRientro"/>  
        /// </summary>
        /// <param name="partenzaInRientro">Il visitor</param>
        /// <returns>Nulla perché solleva un'eccezione</returns>
        public override IStatoMezzo AcceptVisitor(PartenzaInRientro partenzaInRientro)
        {
            throw new WorkflowException();
        }

        /// <summary>
        /// Nello stato <see cref="InSede"/> non può essere gestito l'evento <see cref="PartenzaRientrata"/>  
        /// </summary>
        /// <param name="partenzaRientrata">Il visitor</param>
        /// <returns>Nulla perché solleva un'eccezione</returns>
        public override IStatoMezzo AcceptVisitor(PartenzaRientrata partenzaRientrata)
        {
            throw new WorkflowException();
        }

        /// <summary>
        /// Nello stato <see cref="InSede"/> l'evento <see cref="ComposizionePartenze"/> produce la transizione nello stato <see cref="Assegnato"/>. 
        /// </summary>
        /// <param name="composizionePartenze">Il visitor</param>
        /// <returns>Lo stato <see cref="Assegnato"/></returns>
        public override IStatoMezzo AcceptVisitor(ComposizionePartenze composizionePartenze)
        {
            return new Assegnato();
        }
    }
}
