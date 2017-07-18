import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { FormschedacontattoComponent } from './formschedacontatto/formschedacontatto.component';
import { FriendlyDatePipe } from './shared/pipes/friendly-date.pipe';
import { FriendlyHourPipe } from './shared/pipes/friendly-hour.pipe';
import { ListaSchedeService } from "app/lista-schede/lista-schede.service";
import { ListaSchedeService_FakeJson } from "app/lista-schede/lista-schede-fake-json.service";
import { ListaSchedeComponent } from "app/lista-schede/lista-schede.component";
import { SintesiSchedaComponent } from './sintesi-scheda/sintesi-scheda.component';

import { AppRoutingModule } from './app-routing.module';

@NgModule({
  declarations: [
    AppComponent,
    FormschedacontattoComponent,
    FriendlyDatePipe,
    FriendlyHourPipe,
    ListaSchedeComponent,
    SintesiSchedaComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    NgbModule.forRoot()
  ],
  providers: [
    { provide: ListaSchedeService, useClass: ListaSchedeService_FakeJson },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }



