﻿//-----------------------------------------------------------------------
// <copyright file="ServicesConfigurator.cs" company="CNVVF">
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
using SimpleInjector;
using SO115App.FakePersistence.JSon.GestioneIntervento;
using SO115App.FakePersistenceJSon.Box;
using SO115App.FakePersistenceJSon.Composizione;
using SO115App.FakePersistenceJSon.GestioneMezzi;
using SO115App.FakePersistenceJSon.Marker;

namespace SO115App.CompositionRoot
{
    internal static class ServicesConfigurator
    {
        internal static void Configure(Container container)
        {

            container.Register<
                SO115App.Models.Servizi.Infrastruttura.Box.IGetBoxMezzi,
                GetBoxMezzi>();
            container.Register<
              SO115App.Models.Servizi.Infrastruttura.Box.IGetBoxPersonale,
              ExternalAPI.Fake.Box.GetBoxPersonale>();

            container.Register<
                SO115App.Models.Servizi.Infrastruttura.GetFiltri.IGetFiltri,
                FakePersistenceJSon.Filtri.GetFiltri>();
            container.Register<
                SO115App.Models.Servizi.Infrastruttura.Marker.IGetMezziMarker,
                GetMezziMarker>();
            container.Register<
                SO115App.Models.Servizi.Infrastruttura.Marker.IGetRichiesteMarker,
                FakePersistenceJSon.Marker.GetRichiesteMarker>();
            container.Register<
                SO115App.Models.Servizi.Infrastruttura.Marker.IGetSediMarker,
                FakePersistenceJSon.Marker.GetSediMarker>();
            container.Register<
                SO115App.Models.Servizi.Infrastruttura.Marker.IGetCentroMappaMarker,
                FakePersistenceJSon.Marker.GetCentroMappa>();

            container.Register<
                SO115App.Models.Servizi.Infrastruttura.NavBar.IGetNavbar,
                SO115App.FakePersistenceJSon.Navbar.GetNavbar>();

            container.Register<
                SO115App.Models.Servizi.Infrastruttura.Marker.IAddChiamataInCorso,
                FakePersistenceJSon.Marker.AddChiamateInCorso>();
            container.Register<
                SO115App.Models.Servizi.Infrastruttura.Marker.IGetChiamateInCorso,
                FakePersistenceJSon.Marker.GetChiamateInCorso>();
            container.Register<
                SO115App.Models.Servizi.Infrastruttura.Marker.IDeleteChiamataInCorso,
                FakePersistenceJSon.Marker.DelChiamataInCorso>();
            container.Register<
                SO115App.Models.Servizi.Infrastruttura.Marker.IUpDateChiamataInCorso,
                FakePersistenceJSon.Marker.UpDateChiamateInCorso>();
            container.Register<
                SO115App.Models.Servizi.Infrastruttura.GetComposizioneSquadre.IGetComposizioneSquadre,
                SO115App.ExternalAPI.Fake.ImportOracle.SquadreMapper.GetComposizioneSquadre>();

            container.Register<
                SO115App.Models.Servizi.Infrastruttura.GetComposizioneMezzi.IGetComposizioneMezzi,
                SO115App.ExternalAPI.Fake.ImportOracle.MezziMapper.GetComposizioneMezzi>();

            container.Register<
                SO115App.Models.Servizi.Infrastruttura.GetPreAccoppiati.IGetPreAccoppiati,
                SO115App.ExternalAPI.Fake.Servizi.Preaccoppiati.GetPreAccoppiati>();
            container.Register<
                SO115App.Models.Servizi.Infrastruttura.GetListaEventi.IGetListaEventi,
                FakePersistenceJSon.ListaEventi.GetListaEventi>();

            container.Register<
                SO115App.API.Models.Servizi.Infrastruttura.GestioneSoccorso.IGetIdByCodice,
                FakePersistenceJSon.GestioneIntervento.GetIdByCodice>();
            container.Register<
                Models.Servizi.Infrastruttura.Composizione.ISetMezzoPrenotato,
                FakePersistence.JSon.Composizione.SetMezzoPrenotato>();
            container.Register<Models.Servizi.Infrastruttura.Composizione.IGetStatoMezzi,
                FakePersistence.JSon.GestioneMezzi.GetStatoMezzi>();
            container.Register<
                SO115App.Models.Servizi.Infrastruttura.Composizione.IUpdateConfermaPartenze,
            UpdateConfermaPartenze>();
            container.Register<
                SO115App.Models.Servizi.Infrastruttura.Composizione.IUpdateStatoPartenze,
            UpdateStatoPartenza>();
            container.Register<
                SO115App.API.Models.Servizi.Infrastruttura.GestioneSoccorso.Mezzi.IGetMezzoByCodice,
                SO115App.FakePersistence.JSon.GestioneMezzi.GetMezzoById>();

            container.Register<
                SO115App.Models.Servizi.Infrastruttura.GestioneSoccorso.GenerazioneCodiciRichiesta.IGeneraCodiceRichiesta,
                SO115App.FakePersistence.JSon.Utility.GeneraCodiceRichiesta>();
            container.Register<
                Models.Servizi.Infrastruttura.SistemiEsterni.IdentityManagement.IGetDistaccamentoByCodiceSede,
                FakePersistence.JSon.Distaccamenti.GetDistaccamentoByCodiceSede>();
            container.Register<
                SO115App.Models.Servizi.Infrastruttura.GestioneUtenti.IGetUtenteById,
                SO115App.FakePersistence.JSon.GestioneUtenti.GetUtenteById>();
            // da trasportare su un altro file di configurazione
            container.Register<Models.Servizi.Infrastruttura.GestioneUtenti.VerificaUtente.IFindUserByUsername, FakePersistence.JSon.GestioneUtenti.FindByUsername>();
            container.Register<Models.Servizi.Infrastruttura.GestioneUtenti.IGetUtenteById, FakePersistence.JSon.GestioneUtenti.GetUtenteById>();
            container.Register<Models.Servizi.Infrastruttura.GestioneUtenti.VerificaUtente.IVerificaLogIn, FakePersistence.JSon.GestioneUtenti.VerificaLogIn>();

            container.Register<
                SO115App.API.Models.Servizi.Infrastruttura.GestioneSoccorso.Mezzi.IGetListaMezzi,
                GetListaMezzi>();
            container.Register<
            SO115App.Models.Servizi.Infrastruttura.Turni.IUpdateTurni,
            SO115App.FakePersistence.JSon.Turni.UpdateTurni>();
            container.Register<
            SO115App.Models.Servizi.Infrastruttura.Turni.IGetTurno,
            SO115App.FakePersistence.JSon.Turni.GetTurno>();
            container.Register<
    SO115App.Models.Servizi.Infrastruttura.InfoRichiesta.IGetInfoRichiesta,
    FakePersistence.JSon.Utility.GetInfoRichiesta>();
            container.Register<
                Models.Servizi.Infrastruttura.GestioneSoccorso.Mezzi.ISetStatoOperativoMezzo,
                FakePersistence.JSon.GestioneMezzi.SetStatoOperativoMezzo>();

            container.Register<
            SO115App.Models.Servizi.Infrastruttura.Notification.GestioneChiamata.INotifyInserimentoChiamata,
            SO115App.SignalR.Sender.GestioneChiamata.NotificationInserimentoChiamata>();
            container.Register<
            SO115App.Models.Servizi.Infrastruttura.Notification.GestioneChiamata.INotifyUpDateChiamata,
            SO115App.SignalR.Sender.GestioneChiamata.NotificationUpDateChiamata>();
            container.Register<
            SO115App.Models.Servizi.Infrastruttura.Notification.GestioneChiamateInCorso.INotificationAddChiamataInCorso,
            SO115App.SignalR.Sender.GestioneChiamateInCorso.NotificationAddChiamataInCorso>();
            container.Register<
            SO115App.Models.Servizi.Infrastruttura.Notification.GestioneChiamateInCorso.INotificationDeleteChiamataInCorso,
            SO115App.SignalR.Sender.GestioneChiamateInCorso.NotificationDeleteChiamataInCorso>();
            container.Register<
            SO115App.Models.Servizi.Infrastruttura.Notification.GestioneChiamateInCorso.INotificationUpDateChiamataInCorso,
            SO115App.SignalR.Sender.GestioneChiamateInCorso.NotificationUpDateChiamataInCorso>();
            container.Register<
            SO115App.Models.Servizi.Infrastruttura.Notification.ComposizionePartenza.MezzoPrenotato.INotificationAddPrenotazioneMezzo,
            SignalR.Sender.ComposizionePartenza.GestioneMezzoPrenotato.NotificationAddPrenotazioneMezzo>();
            container.Register<
            SO115App.Models.Servizi.Infrastruttura.Notification.ComposizionePartenza.INotificationConfermaPartenze,
            SO115App.SignalR.Sender.ComposizionePartenza.NotificationConfermaPartenze>();
            container.Register<
           SO115App.Models.Servizi.Infrastruttura.Notification.GestioneIntervento.INotifyMessaInLavorazioneRichiesta,
           SO115App.SignalR.Sender.GestioneIntervento.NotificationInserInLavorazione>();
            container.Register<
           SO115App.Models.Servizi.Infrastruttura.Notification.GestioneIntervento.INotifyDeleteInLavorazioneRichiesta,
           SO115App.SignalR.Sender.GestioneIntervento.NotificationDeleteInLavorazione>();
            container.Register<
           SO115App.Models.Servizi.Infrastruttura.Notification.GestioneIntervento.INotifyDeletePresaInCaricoRichiesta,
           SO115App.SignalR.Sender.GestioneIntervento.NotificationDeletePresaInCarico>();
            container.Register<
           SO115App.Models.Servizi.Infrastruttura.Notification.GestioneIntervento.INotifyPresaInCaricoRichiesta,
           SO115App.SignalR.Sender.GestioneIntervento.NotificationPresaInCarico>();
            container.Register<
           SO115App.Models.Servizi.Infrastruttura.Notification.GestionePartenza.INotifyAggiornaStatoMezzo,
           SO115App.SignalR.Sender.GestionePartenza.NotificationAggiornaStatoMezzo>();
            container.Register<
            SO115App.Models.Servizi.Infrastruttura.Notification.GestioneIntervento.INotifyUpDateStatoRichiesta,
            SO115App.SignalR.Sender.GestioneIntervento.NotificationUpDateStato>();
            container.Register<SO115App.Models.Servizi.Infrastruttura.Notification.GestioneSchedeContatto.INotificationSetSchedaGestita,
                SO115App.SignalR.Sender.GestioneSchedeContatto.NotificationSetSchedaGestita>();
            container.Register<Models.Servizi.Infrastruttura.Notification.GestioneUtenti.INotifyAddUtente, SignalR.Sender.GestioneUtenti.NotificationAddUtente>();
            container.Register<Models.Servizi.Infrastruttura.Notification.GestioneUtenti.INotifyDeleteUtente, SignalR.Sender.GestioneUtenti.NotificationDeleteUtente>();
            container.Register<Models.Servizi.Infrastruttura.Notification.GestioneUtenti.GestioneRuoli.INotifyAddRuoli, SignalR.Sender.GestioneRuoli.NotificationAddRuoli>();
            container.Register<Models.Servizi.Infrastruttura.Notification.GestioneUtenti.GestioneRuoli.INotifyDeleteRuolo, SignalR.Sender.GestioneRuoli.NotificationDeleteRuolo>();

            container.Register<
                API.Models.Servizi.Infrastruttura.Organigramma.IGetUnitaOperativaPerCodice,
                API.Models.Servizi.Infrastruttura.Organigramma.Implementazioni.GetUnitaOperativaPerCodice>();
            container.Register<
                API.Models.Servizi.Infrastruttura.Autenticazione.IGetOperatoreAutenticato,
                API.SOVVF.FakeImplementations.Modello.Autenticazione.GetOperatoreAutenticato>();
            container.Register<
                API.Models.Servizi.Infrastruttura.Organigramma.IGetUnitaOperativaRadice,
                API.SOVVF.FakeImplementations.Modello.Organigramma.GetUnitaOperativaRadice_CON_Direzioni_ComLazio>();
            container.Register<
                API.Models.Servizi.Infrastruttura.Organigramma.IGetUnitaOperativeVisibiliPerSoccorso,
                API.SOVVF.FakeImplementations.Modello.Organigramma.GetUnitaOperativeVisibiliPerSoccorso_OperatoreDirezioneRegionaleLazio>();
            container.Register<
                API.Models.Servizi.Infrastruttura.GestioneSoccorso.Mezzi.IGetSituazioneMezzi,
                API.SOVVF.FakeImplementations.Modello.GestioneSoccorso.Mezzi.GetSituazioneMezzi_RandomFake>();
            container.Register<
                API.Models.Servizi.Infrastruttura.Anagrafiche.IGetTipoInterventoByCodice,
                API.SOVVF.FakeImplementations.Modello.Infrastruttura.Anagrafiche.GetTipoInterventoByCodice_Fake>();
            container.Register<
                API.Models.Servizi.CQRS.Commands.GestioneSoccorso.SelezioneSquadra.CommandDTO.ITestAndSetSelezioneDisponibilitaSquadra,
                API.SOVVF.FakeImplementations.Modello.GestioneSoccorso.Risorse.TestAndSetSelezioneDisponibilitaSquadra_Fake>();
            container.Register<
                API.Models.Classi.Soccorso.Risorse.ITestAndSetSelezioneDisponibilitaMezzo,
                API.SOVVF.FakeImplementations.Modello.GestioneSoccorso.Risorse.TestAndSetSelezioneDisponibilitaMezzo_Fake>();
            container.Register<
                API.Models.Servizi.CQRS.Commands.GestioneSoccorso.DeselezionaSquadra.CommandDTO.ITestAndSetDeselezioneDisponibilitaSquadra,
                API.SOVVF.FakeImplementations.Modello.GestioneSoccorso.Risorse.TestAndSetDeselezioneDisponibilitaSquadra_DoNothing>();
            container.Register<
                API.Models.Servizi.CQRS.Commands.GestioneSoccorso.DeselezionaMezzo.CommandDTO.ITestAndSetDeselezioneDisponibilitaMezzo,
                API.SOVVF.FakeImplementations.Modello.GestioneSoccorso.Risorse.TestAndSetDeselezioneDisponibilitaMezzo_DoNothing>();
            container.Register<
                API.Models.Servizi.Infrastruttura.GestioneSoccorso.IGetRichiestaAssistenzaById,
                API.SOVVF.FakeImplementations.Modello.GestioneSoccorso.GetRichiestaAssistenzaById_Fake>();
            container.Register<
                API.Models.Servizi.Infrastruttura.GestioneSoccorso.RicercaRichiesteAssistenza.ICercaRichiesteAssistenza,
                API.Models.Classi.Soccorso.CercaRichiesteAssistenza_Empty>();
            container.Register<
                API.Models.Servizi.Infrastruttura.GestioneSoccorso.Mezzi.IGetNumeroMezziSoccorsoOraInServizio,
                API.SOVVF.FakeImplementations.Modello.GestioneSoccorso.Mezzi.GetNumeroMezziSoccorsoOraInServizio_Fake>();
            container.Register<
                API.Models.Servizi.Infrastruttura.GestioneSoccorso.IGetNumeroSquadreSoccorsoOraInServizio,
                API.SOVVF.FakeImplementations.Modello.GestioneSoccorso.Mezzi.GetNumeroSquadreSoccorsoOraInServizio_Fake>();
            container.Register<
                API.Models.Servizi.Infrastruttura.GestioneSoccorso.CompetenzeTerritoriali.IGetCompetenzeByPunto,
                API.SOVVF.FakeImplementations.Modello.Infrastruttura.CompetenzeTerritoriali.GetCompetenzeByPunto_Fake_Hardcoded>();
        }
    }
}
