import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { routing } from './scheda-telefonata.routing';
import { FormChiamataComponent } from './form-chiamata/form-chiamata.component';
import { MultiselectDropdownModule } from 'angular-2-dropdown-multiselect';
import { AgmCoreModule } from "@agm/core";
import { AutoCompleteModule } from 'primeng/primeng';
import { RicercaTipologieService } from "./ricerca-tipologie/ricerca-tipologie.service";
import { GrowlModule } from "primeng/components/growl/growl";
import { TooltipModule } from "primeng/primeng";
import { RicercaService } from "app/pages/schedaTelefonata/ricerca/ricerca.service";
import { ChipsModule } from 'primeng/primeng';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    routing,
    HttpModule,
    ReactiveFormsModule,
    MultiselectDropdownModule,
    AutoCompleteModule,
    AgmCoreModule.forRoot({
      apiKey: "AIzaSyCUyaLim6v4CB_eo-oJmeDlvPCQxwHha70",
      libraries: ["places"]
    }),
    GrowlModule,
    TooltipModule,
    ChipsModule
  ],
  declarations: [
    AppComponent,
    FormChiamataComponent
  ],
  providers: [ RicercaTipologieService, RicercaService ],
})
export class NewModule { }