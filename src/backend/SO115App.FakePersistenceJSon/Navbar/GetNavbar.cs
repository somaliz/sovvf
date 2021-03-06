﻿//-----------------------------------------------------------------------
// <copyright file="GetNavbar.cs" company="CNVVF">
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
using System.IO;
using Newtonsoft.Json;
using SO115App.Models.Servizi.Infrastruttura.NavBar;

namespace SO115App.FakePersistenceJSon.Navbar
{
    public class GetNavbar : IGetNavbar
    {
        public API.Models.Classi.NavBar.Navbar Get()
        {
            string filepath = "Fake/Navbar.json";
            string json;
            using (StreamReader r = new StreamReader(filepath))
            {
                json = r.ReadToEnd();
            }

            API.Models.Classi.NavBar.Navbar navbars = JsonConvert.DeserializeObject<API.Models.Classi.NavBar.Navbar>(json);

            return navbars;
        }
    }
}
