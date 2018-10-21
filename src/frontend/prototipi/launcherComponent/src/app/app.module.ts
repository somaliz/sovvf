import {BrowserModule} from '@angular/platform-browser';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {NgModule} from '@angular/core';
import {PipeModule} from './shared/pipes/pipe.module';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {FormsModule} from '@angular/forms';
import * as Shared from './shared/';
import {AppComponent} from './app.component';
import {environment} from '../environments/environment';
import {HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http';
import {ReactiveFormsModule} from '@angular/forms';
import {routing} from './app.routing';
import {BasicAuthInterceptor, ErrorInterceptor} from './auth/_helpers';
import {HomeComponent} from './auth/home';
import {LoginComponent} from './auth/login';
// backend fake per login
import {fakeBackendProvider} from './auth/_helpers';
// start maps-container
import {MapsComponent} from './maps/maps.component';
import {AgmComponent} from './maps/agm/agm.component';
import {AgmContentComponent} from './maps/agm/agm-content.component';
import {AgmJsMarkerClustererModule} from '@agm/js-marker-clusterer';
import {AgmCoreModule} from '@agm/core';
import {AgmSnazzyInfoWindowModule} from '@agm/snazzy-info-window';
import {InfoWindowComponent} from './maps/maps-ui/info-window/info-window.component';
import {MapsFiltroComponent} from './maps/maps-ui/filtro/filtro.component';
import {CambioSedeModalComponent} from './maps/maps-ui/info-window/cambio-sede-modal/cambio-sede-modal.component';
import {CambioSedeModalNavComponent} from './navbar/cambio-sede-modal-nav/cambio-sede-modal-nav.component';


// end maps-container
// start rigaElenco
import {RichiesteComponent} from './richieste/richieste.component';
import {ListaRichiesteComponent} from './richieste/lista-richieste/lista-richieste.component';
import {SintesiRichiestaComponent} from './richieste/lista-richieste/sintesi-richiesta/sintesi-richiesta.component';
import {RichiestaFissataComponent} from './richieste/lista-richieste/richiesta-fissata/richiesta-fissata.component';
import {NavTestComponent} from './richieste/lista-richieste-test/nav-test/nav-test.component';
import {RicercaRichiesteComponent} from './richieste/ricerca-richieste/ricerca-richieste.component';
import {FiltriRichiesteComponent} from './richieste/filtri-richieste/filtri-richieste.component';
import {FiltroComponent} from './richieste/filtri-richieste/filtro/filtro.component';
import {FilterPipeModule} from 'ngx-filter-pipe';
import {NgxPaginationModule} from 'ngx-pagination';
import {ScrollEventModule} from 'ngx-scroll-event';
// end rigaElenco
// start eventiRichiesta
import {EventiRichiestaComponent} from './eventi-richiesta/eventi-richiesta.component';
import {EventoRichiestaComponent} from './eventi-richiesta/evento-richiesta/evento-richiesta.component';
import {ListaEventiRichiestaComponent} from './eventi-richiesta/lista-eventi-richiesta/lista-eventi-richiesta.component';
// end eventiRichiesta
// start boxes
import {BoxFunzionariComponent} from './boxes/info-aggregate/box-funzionari/box-funzionari.component';
import {InfoAggregateComponent} from './boxes/info-aggregate/info-aggregate.component';
import {InfoAggregateService} from './boxes/service/boxes-services/info-aggregate.service';
import {InfoAggregateServiceFake} from './boxes/service/boxes-services/info-aggregate.service.fake';
import {BoxInterventiComponent} from './boxes/info-aggregate/box-interventi/box-interventi.component';
import {BoxMezziComponent} from './boxes/info-aggregate/box-mezzi/box-mezzi.component';
import {BoxMeteoComponent} from './boxes/info-aggregate/box-meteo/box-meteo.component';
// end boxes
// start sidebar
import {SidebarModule, Sidebar} from 'ng-sidebar';
import {NavbarComponent} from './navbar/navbar.component';
// end sidebar
// start navbar
import {UnitaOperativaService} from './navbar/navbar-service/unita-operativa-service/unita-operativa.service';
// end navbar

import {RichiesteMarkerService} from './dispatcher/data/service/maps-service/richieste-marker/richieste-marker.service';
import {RichiesteMarkerServiceFake} from './dispatcher/data/service/maps-service/richieste-marker/richieste-marker.service.fake';
import {SediMarkerService} from './dispatcher/data/service/maps-service/sedi-marker/sedi-marker.service';
import {SediMarkerServiceFake} from './dispatcher/data/service/maps-service/sedi-marker/sedi-marker.service.fake';
import {MezziMarkerService} from './dispatcher/data/service/maps-service/mezzi-marker/mezzi-marker.service';
import {MezziMarkerServiceFake} from './dispatcher/data/service/maps-service/mezzi-marker/mezzi-marker.service.fake';
import {RichiesteService} from './dispatcher/data/service/lista-richieste-service/richieste.service';
import {RichiesteServiceFake} from './dispatcher/data/service/lista-richieste-service/richieste.service.fake';

@NgModule({
    declarations: [
        AppComponent,
        RichiesteComponent,
        RichiestaFissataComponent,
        ListaRichiesteComponent,
        SintesiRichiestaComponent,
        NavTestComponent,
        RicercaRichiesteComponent,
        FiltriRichiesteComponent,
        FiltroComponent,
        // start import of eventi
        EventiRichiestaComponent,
        EventoRichiestaComponent,
        ListaEventiRichiestaComponent,
        // end import of eventi
        // start import of Shared Declarations
        [
            Shared.DebounceClickDirective,
            Shared.DebounceKeyUpDirective,
            Shared.CompetenzaComponent,
            Shared.ComponenteComponent,
            Shared.MezzoComponent
        ],
        // end import of Shared Declarations
        // start import maps
        MapsComponent,
        AgmComponent,
        AgmContentComponent,
        MapsFiltroComponent,
        InfoWindowComponent,
        CambioSedeModalComponent,
        CambioSedeModalNavComponent,
        // end import maps
        BoxFunzionariComponent,
        InfoAggregateComponent,
        BoxInterventiComponent,
        BoxMezziComponent,
        BoxMeteoComponent,
        NavbarComponent,
        HomeComponent,
        LoginComponent
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        ReactiveFormsModule,
        HttpClientModule,
        routing,
        NgbModule,
        PipeModule.forRoot(),
        AgmCoreModule.forRoot({
            apiKey: environment.apiUrl.maps.agm.key
        }),
        AgmJsMarkerClustererModule,
        FormsModule,
        NgxPaginationModule,
        FilterPipeModule,
        SidebarModule.forRoot(),
        ScrollEventModule,
        AgmSnazzyInfoWindowModule
    ],
    entryComponents: [CambioSedeModalComponent, CambioSedeModalNavComponent, EventiRichiestaComponent],
    providers: [
        {provide: InfoAggregateService, useClass: InfoAggregateServiceFake},
        {provide: HTTP_INTERCEPTORS, useClass: BasicAuthInterceptor, multi: true},
        {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true},
        // backend fake provider per login
        fakeBackendProvider,
        // Unità Operativa
        UnitaOperativaService,
        {provide: RichiesteMarkerService, useClass: RichiesteMarkerServiceFake},
        {provide: SediMarkerService, useClass: SediMarkerServiceFake},
        {provide: RichiesteService, useClass: RichiesteServiceFake},
        {provide: MezziMarkerService, useClass: MezziMarkerServiceFake}
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}
