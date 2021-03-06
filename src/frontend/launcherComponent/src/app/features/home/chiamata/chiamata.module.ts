import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgSelectModule } from '@ng-select/ng-select';
import { FormsModule } from '@angular/forms';
import { GooglePlaceModule } from 'ngx-google-places-autocomplete';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ClipboardModule } from 'ngx-clipboard';
import { ReactiveFormsModule } from '@angular/forms';
import { ChiamataComponent } from './chiamata.component';
import { SchedaTelefonataComponent } from './scheda-telefonata/scheda-telefonata.component';
import { ChiamataService } from '../../../core/service/chiamata-service/chiamata.service';
import { ChiamataServiceFake } from '../../../core/service/chiamata-service/chiamata.service.fake';
import { NgxsModule } from '@ngxs/store';
import { SchedaTelefonataState } from '../store/states/chiamata/scheda-telefonata.state';
import { ClipboardState } from '../store/states/chiamata/clipboard.state';
import { ConfirmModalComponent } from '../../../shared';
import { UiSwitchModule } from 'ngx-ui-switch';
import { SharedModule } from 'src/app/shared/shared.module';
import { environment } from '../../../../environments/environment';
import { NgxsFormPluginModule } from '@ngxs/form-plugin';

@NgModule({
    imports: [
        CommonModule,
        NgSelectModule,
        FormsModule,
        GooglePlaceModule,
        NgbModule,
        ClipboardModule,
        ReactiveFormsModule,
        SharedModule,
        UiSwitchModule.forRoot(null),
        NgxsModule.forFeature([
            SchedaTelefonataState,
            ClipboardState
        ]),
        NgxsFormPluginModule
    ],
    declarations: [
        ChiamataComponent,
        SchedaTelefonataComponent
    ],
    exports: [
        ChiamataComponent
    ],
    entryComponents: [ConfirmModalComponent],
    providers: [
        environment.fakeProvider ? { provide: ChiamataService, useClass: ChiamataServiceFake } : ChiamataService
    ]
})
export class ChiamataModule {
}
