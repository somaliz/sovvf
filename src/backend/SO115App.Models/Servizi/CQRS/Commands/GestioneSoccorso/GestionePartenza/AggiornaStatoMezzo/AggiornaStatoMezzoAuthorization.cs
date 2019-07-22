﻿//-----------------------------------------------------------------------
// <copyright file="AddInterventoAuthorization.cs" company="CNVVF">
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
using System.Collections.Generic;
using System.Security.Principal;
using CQRS.Authorization;
using CQRS.Commands.Authorizers;
using SO115App.API.Models.Classi.Autenticazione;
using SO115App.Models.Classi.Utility;

namespace DomainModel.CQRS.Commands.GestrionePartenza.AggiornaStatoMezzo
{
    public class AggiornaStatoMezzoAuthorization : ICommandAuthorizer<AggiornaStatoMezzoCommand>
    {
        private readonly IPrincipal currentUser;
        private readonly Costanti costanti;

        public AggiornaStatoMezzoAuthorization(IPrincipal currentUser)
        {
            this.currentUser = currentUser;
        }

        public IEnumerable<AuthorizationResult> Authorize(AggiornaStatoMezzoCommand command)
        {
            string username = this.currentUser.Identity.Name;

            if (this.currentUser.Identity.IsAuthenticated)
            {
                Utente user = Utente.FindUserByUsername(username);
                if (user == null)
                    yield return new AuthorizationResult(costanti.UtenteNonAutorizzato);
            }
            else
                yield return new AuthorizationResult(costanti.UtenteNonAutorizzato);
        }
    }
}