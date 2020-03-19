﻿using Microsoft.Extensions.Configuration;
using Persistence.MongoDB;
using SimpleInjector;
using SO115App.API.Models.Servizi.Infrastruttura.GestioneSoccorso;
using SO115App.Models.Servizi.Infrastruttura.Box;
using SO115App.Models.Servizi.Infrastruttura.GestioneSoccorso;
using SO115App.Persistence.MongoDB;
using SO115App.Persistence.MongoDB.GestioneInterventi;
using SO115App.Persistence.MongoDB.GestioneMezzi;
using SO115App.Persistence.MongoDB.Marker;
using SO115App.SignalR.Sender.GestioneSchedeContatto;

namespace SO115App.CompositionRoot
{
    internal static class PersistenceServicesConfigurator_MongoDB
    {
        internal static void Configure(Container container, IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("DatabaseSettings").GetSection("ConnectionString").Value;
            var databaseName = configuration.GetSection("DatabaseSettings").GetSection("DatabaseName").Value;

            container.Register<DbContext>(() =>
                new DbContext(connectionString, databaseName), Lifestyle.Singleton);

            #region Gestione richiesta di assistenza

            container.Register<ISaveRichiestaAssistenza, SaveRichiesta>();
            container.Register<IUpDateRichiestaAssistenza, UpDateRichiesta>();

            container.Register<IGetRichiestaById, GetRichiesta>();
            container.Register<IGetListaSintesi, GetRichiesta>();

            container.Register<Models.Servizi.Infrastruttura.GestioneSoccorso.GestioneTipologie.IGetTipologieByCodice,
                Persistence.MongoDB.GestioneInterventi.GestioneTipologie.GetTipologieByCodice>();
            container.Register<IGetSintesiRichiestaAssistenzaByCodice, GetRichiesta>();

            container.Register<SO115App.Models.Servizi.Infrastruttura.GetListaEventi.IGetListaEventi, GetListaEventiByCodiceRichiesta>();

            #endregion Gestione richiesta di assistenza

            #region BOX

            container.Register<IGetBoxRichieste, GetBoxRichieste>();

            #endregion BOX

            #region MARKER

            container.Register<
                SO115App.Models.Servizi.Infrastruttura.Marker.IGetRichiesteMarker,
                SO115App.Persistence.MongoDB.Marker.GetRichiesteMarker>();

            container.Register<
                SO115App.Models.Servizi.Infrastruttura.Marker.IGetCentroMappaMarker,
                SO115App.Persistence.MongoDB.Marker.GetCentroMappa>();

            container.Register<
                SO115App.Models.Servizi.Infrastruttura.Marker.IGetSediMarker,
                SO115App.Persistence.MongoDB.Marker.GetSediMarker>();

            #endregion MARKER

            #region Gestione Sedi

            container.Register<Models.Servizi.Infrastruttura.SistemiEsterni.Distaccamenti.IGetCoordinateByCodSede,
                               Persistence.MongoDB.GestioneSedi.GetCoordinateByCodSede>();

            container.Register<Models.Servizi.Infrastruttura.SistemiEsterni.Competenze.IGetCompetenzeByCoordinateIntervento,
                               Persistence.MongoDB.GestioneSedi.GetcompetenzeByCoordinateIntervento>();

            #endregion

            #region GestioneChiamataInCorso

            container.Register<Models.Servizi.Infrastruttura.Marker.IAddChiamataInCorso,
                SO115App.Persistence.MongoDB.Marker.AddChiamataInCorso>();
            container.Register<Models.Servizi.Infrastruttura.Marker.IDeleteChiamataInCorso,
                SO115App.Persistence.MongoDB.Marker.DeleteChiamataInCorso>();
            container.Register<Models.Servizi.Infrastruttura.Marker.IUpDateChiamataInCorso,
                SO115App.Persistence.MongoDB.Marker.UpdateChiamataInCorso>();
            container.Register<
                SO115App.Models.Servizi.Infrastruttura.Marker.IGetChiamateInCorso,
                SO115App.Persistence.MongoDB.Marker.GetChiamateInCorso>();

            #endregion GestioneChiamataInCorso

            #region StatoMezzo

            container.Register<
                Models.Servizi.Infrastruttura.Composizione.ISetMezzoPrenotato,
                SetMezzoPrenotato>();
            container.Register<
                Models.Servizi.Infrastruttura.Composizione.IGetStatoMezzi,
                GetStatoMezzoByCodice>();
            container.Register<
                Models.Servizi.Infrastruttura.GestioneSoccorso.Mezzi.ISetStatoOperativoMezzo, 
                SetStatoOperativoMezzo>();

            #endregion StatoMezzo

            #region Schede Contatto

            container.Register<
                    Models.Servizi.Infrastruttura.SistemiEsterni.Nue.IMergeSchedeContatto,
                    UpDateSchedeContatto>();

            container.Register<Models.Servizi.Infrastruttura.SistemiEsterni.Nue.IUndoSchedeContattoMerge,
                Persistence.MongoDB.GestioneSchedeContatto.UndoSchedeContattoMerge>();

            #endregion Schede Contatto


            #region Gestione Utenti e Ruoli

            container.Register<
                Models.Servizi.Infrastruttura.GestioneUtenti.IDeleteUtente, 
                Persistence.MongoDB.GestioneUtenti.GestioneUtente.DeleteUtente>();
            container.Register<
                Models.Servizi.Infrastruttura.GestioneUtenti.IAddUtente, 
                Persistence.MongoDB.GestioneUtenti.GestioneUtente.AddUtente>();
            container.Register<
                Models.Servizi.Infrastruttura.SistemiEsterni.Personale.IGetUtenti, 
                Persistence.MongoDB.GestioneUtenti.GestioneUtente.GetUtenti>();
            container.Register<
                Models.Servizi.Infrastruttura.GestioneUtenti.GestioneRuolo.IDeleteRuolo, 
                Persistence.MongoDB.GestioneUtenti.GestioneRuoli.DeleteRuolo>();
            container.Register<
                Models.Servizi.Infrastruttura.GestioneUtenti.GestioneRuolo.IAddRuoli, 
                Persistence.MongoDB.GestioneUtenti.GestioneRuoli.AddRuoli>();
            container.Register<
                Models.Servizi.Infrastruttura.GestioneUtenti.VerificaUtente.IFindUserByUsername, 
                Persistence.MongoDB.GestioneUtenti.GestioneUtente.FindUserByUsername>();
            container.Register<
                Models.Servizi.Infrastruttura.GestioneUtenti.IGetUtenteById, 
                Persistence.MongoDB.GestioneUtenti.GestioneUtente.GetUtenteById>();
            container.Register<
                Models.Servizi.Infrastruttura.GestioneUtenti.VerificaUtente.IVerificaLogIn, 
                Persistence.MongoDB.GestioneUtenti.GestioneUtente.VerificaLogIn>();
            container.Register<
                Models.Servizi.Infrastruttura.GestioneUtenti.GetUtenti.IGetUtentiByCodiciSedi, 
                Persistence.MongoDB.GestioneUtenti.GestioneUtente.GetUtentiByCodiciSede>();
            container.Register<
                Models.Servizi.Infrastruttura.GestioneUtenti.GetUtenti.IGetUtenteByCF, 
                Persistence.MongoDB.GestioneUtenti.GestioneUtente.GetUtenteByCF>();
            container.Register<
                Models.Servizi.Infrastruttura.Autenticazione.IGetAutorizzazioni, 
                Persistence.MongoDB.GestioneUtenti.GestioneRuoli.GetAutorizzazioni>();

            #endregion Gestione Utenti e Ruoli

            #region Utility
            container.Register<
                SO115App.Models.Servizi.Infrastruttura.NavBar.IGetNavbar,
                SO115App.FakePersistenceJSon.Navbar.GetNavbar>();

            container.Register<
                SO115App.Models.Servizi.Infrastruttura.Turni.IGetTurno,
                SO115App.FakePersistence.JSon.Turni.GetTurno>();
            
            container.Register<
                SO115App.Models.Servizi.Infrastruttura.Turni.IUpdateTurni,
                SO115App.FakePersistence.JSon.Turni.UpdateTurni>();

            container.Register<
                SO115App.Models.Servizi.Infrastruttura.InfoRichiesta.IGetInfoRichiesta,
                FakePersistence.JSon.Utility.GetInfoRichiesta>();

            container.Register<
                SO115App.Models.Servizi.Infrastruttura.GetFiltri.IGetFiltri,
                SO115App.Persistence.MongoDB.GestioneFiltri.GetFiltri>();

            container.Register<
                SO115App.Models.Servizi.Infrastruttura.GestioneSoccorso.GenerazioneCodiciRichiesta.IGeneraCodiceRichiesta,
                SO115App.Persistence.MongoDB.GestioneInterventi.GeneraCodiceRichiesta>();

            #endregion
        }
    }
}
