import { Component, EventEmitter, Input, isDevMode, OnDestroy, OnInit, Output } from '@angular/core';
import { SintesiRichiesta } from '../../../shared/model/sintesi-richiesta.model';
import { CenterService } from '../maps/service/center-service/center-service.service';
import { CentroMappa } from '../maps/maps-model/centro-mappa.model';
import { Subject, Subscription } from 'rxjs';
import { MarkerService } from '../maps/service/marker-service/marker-service.service';
import { Store } from '@ngxs/store';
import { AllFalseBoxRichieste, AllTrueBoxMezzi, ReducerBoxClick, ResetAllBoxes } from '../boxes/store';
import { MezzoComposizione } from './interface/mezzo-composizione-interface';
import { SquadraComposizione } from './interface/squadra-composizione-interface';
import { BoxPartenza } from './interface/box-partenza-interface';
import { Composizione } from '../../../shared/enum/composizione.enum';
import { AppFeatures } from '../../../shared/enum/app-features.enum';
import { CompPartenzaService } from '../../../core/service/comp-partenza-service/comp-partenza.service';
import { StatoRichiesta } from '../../../shared/enum/stato-richiesta.enum';

@Component({
    selector: 'app-composizione-partenza',
    templateUrl: './composizione-partenza.component.html',
    styleUrls: ['./composizione-partenza.component.css']
})
export class ComposizionePartenzaComponent implements OnInit, OnDestroy {
    @Input() richiesta: SintesiRichiesta;
    @Input() compPartenzaMode: string;
    @Output() statoPartenza = new EventEmitter<string>();
    dismissPartenzaSubject: Subject<boolean> = new Subject<boolean>();
    Composizione = Composizione;

    subscription = new Subscription();
    mezziComposizione: MezzoComposizione[];
    squadreComposizione: SquadraComposizione[];
    preAccoppiati: BoxPartenza[];

    centroMappa: CentroMappa;

    statoPrecedente: any;

    constructor(private store: Store,
                private centerService: CenterService,
                private markerS: MarkerService,
                private compPartenzaService: CompPartenzaService) {

        // Prendo i mezzi da visualizzare nella lista
        this.subscription.add(
            this.compPartenzaService.getMezziComposizione().subscribe((mezziComp: MezzoComposizione[]) => {
                this.mezziComposizione = mezziComp;
            })
        );

        // Prendo le squadre da visualizzare nella lista
        this.subscription.add(
            this.compPartenzaService.getSquadre().subscribe((squadreComp: SquadraComposizione[]) => {
                this.squadreComposizione = squadreComp;
            })
        );

        // Restituisce i PreAccoppiati
        this.subscription.add(
            this.compPartenzaService.getPreAccoppiati().subscribe((preAccoppiati: BoxPartenza[]) => {
                this.preAccoppiati = preAccoppiati;
            })
        );
    }

    ngOnInit() {
        this.centroMappa = this.centerService.centroMappaIniziale;
        if (this.richiesta) {
            this.statoPrecedente = this.store.snapshot();
            this.store.dispatch(new AllFalseBoxRichieste());
            this.store.dispatch(new AllTrueBoxMezzi());
            this.store.dispatch(new ReducerBoxClick('richieste', wipeStatoRichiesta(this.richiesta.stato)));
        } else {
            this.dismissPartenza();
        }
        isDevMode() && console.log('Componente Composizione creato');
    }

    ngOnDestroy() {
        isDevMode() && console.log('Componente Composizione distrutto');
    }

    dismissPartenza(): void {
        this.centerService.sendCentro(this.centroMappa);
        this.dismissPartenzaSubject.next(true);
        this.markerS.noAction();
        this.store.reset(this.statoPrecedente);
        this.statoPartenza.emit(AppFeatures.Default);
    }

    cardClasses(r: SintesiRichiesta) {
        return {
            'status_chiamata': r.stato === StatoRichiesta.Chiamata,
            'status_presidiato': r.stato === StatoRichiesta.Presidiata,
            'status_assegnato': r.stato === StatoRichiesta.Assegnata,
            'status_sospeso': r.stato === StatoRichiesta.Sospesa,
            'status_chiuso': r.stato === StatoRichiesta.Chiusa,
        };
    }

    getCentroMappa(event: CentroMappa): void {
        this.centroMappa = event;
    }

}

export function wipeStatoRichiesta(stato: StatoRichiesta): string {
    const stati: [StatoRichiesta, string][] = [
        [StatoRichiesta.Chiamata, 'chiamate'],
        [StatoRichiesta.Sospesa, 'sospesi'],
        [StatoRichiesta.Assegnata, 'assegnati'],
        [StatoRichiesta.Presidiata, 'presidiati'],
        [StatoRichiesta.Chiusa, 'chiusi']
    ];
    const mapTipoStato: Map<StatoRichiesta, string> = new Map(stati);

    return mapTipoStato.get(stato);
}
