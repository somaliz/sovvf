import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ViewInterfaceButton, ViewInterfaceComposizione } from '../../../shared/interface/view.interface';

@Component({
    selector: 'app-filterbar',
    templateUrl: './filterbar.component.html',
    styleUrls: ['./filterbar.component.css']
})
export class FilterbarComponent {

    @Input() compPartenzaState: ViewInterfaceComposizione;
    @Input() colorButton: ViewInterfaceButton;
    @Output() buttonSwitchView = new EventEmitter<object>();
    @Output() buttonCompPartenzaMode = new EventEmitter<string>();

    compPartenzaSwitch(event: string) {
        this.buttonCompPartenzaMode.emit(event);
    }

    chiamata(value: boolean) {
        this.buttonSwitchView.emit({event: 'chiamata', chiamata: value});
    }

    buttonView(event: string) {
        let method = '';
        switch (event) {
            case 'normale':
                method = 'normale';
                break;
            case 'soloMappa':
                method = 'soloMappa';
                break;
            case 'soloRichieste':
                method = 'soloRichieste';
                break;
        }
        this.buttonSwitchView.emit({event: method});
    }

}