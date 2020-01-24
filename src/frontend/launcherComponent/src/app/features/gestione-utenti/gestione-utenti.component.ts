import { Component, OnInit } from '@angular/core';
import { Utente } from 'src/app/shared/model/utente.model';
import { Observable } from 'rxjs';
import { Select, Store } from '@ngxs/store';
import { SetRicercaUtenti } from './store/actions/ricerca-utenti/ricerca-utenti.actons';
import { UtenteState } from '../navbar/store/states/operatore/utente.state';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AddUtente, GetUtentiGestione, OpenModalRemoveUtente, UpdateUtenteGestione } from './store/actions/gestione-utenti/gestione-utenti.actions';
import { GetRuoli } from './store/actions/ruoli/ruoli.actions';
import { RuoliState } from './store/states/ruoli/ruoli.state';
import { GestioneUtentiState } from './store/states/gestione-utenti/gestione-utenti.state';
import { GestioneUtente } from '../../shared/interface/gestione-utente.interface';
import { RicercaUtentiState } from './store/states/ricerca-utenti/ricerca-utenti.state';
import { PaginationState } from '../../shared/store/states/pagination/pagination.state';
import { LoadingState } from '../../shared/store/states/loading/loading.state';
import { GetPermessi } from './store/actions/permessi/permessi.actions';
import { AggiungiUtenteModalComponent } from './aggiungi-utente-modal/aggiungi-utente-modal.component';

@Component({
    selector: 'app-gestione-utenti',
    templateUrl: './gestione-utenti.component.html',
    styleUrls: ['./gestione-utenti.component.css']
})
export class GestioneUtentiComponent implements OnInit {

    @Select(UtenteState.utente) user$: Observable<Utente>;
    @Select(GestioneUtentiState.listaUtenti) listaUtenti$: Observable<GestioneUtente[]>;
    @Select(RuoliState.ruoli) ruoli$: Observable<Array<any>>;
    @Select(RicercaUtentiState.ricerca) ricerca$: Observable<any>;
    @Select(PaginationState.limit) pageSize$: Observable<number>;
    @Select(PaginationState.totalItems) totalItems$: Observable<number>;
    @Select(PaginationState.page) page$: Observable<number>;
    @Select(LoadingState.loading) loading$: Observable<boolean>;

    constructor(public modalService: NgbModal,
                private store: Store) {
        this.getUtentiGestione();
        this.getRuoli();
        this.getPermessi();
        this.getRicerca();
    }

    onRicercaUtenti(ricerca: any) {
        this.store.dispatch(new SetRicercaUtenti(ricerca));
    }

    ngOnInit(): void {
    }

    onAddUtente() {
        const aggiungiUtenteModal = this.modalService.open(AggiungiUtenteModalComponent, { backdropClass: 'light-blue-backdrop', centered: true, size: 'lg' });
        aggiungiUtenteModal.result.then(
            (risultatoModal) => {
                if (risultatoModal[0] === 'ok') {
                    this.store.dispatch(new AddUtente(risultatoModal[1]));
                }
                // console.log('Modal chiusa con val ->', val);
            },
            (err) => console.error('Modal chiusa senza bottoni. Err ->', err)
        );
        // TODO: DEBUG
        // console.warn('add utente modal');
    }

    onUpdateUtente(utenteGestione: any) {
        this.store.dispatch(new UpdateUtenteGestione(utenteGestione.utente.id_utente));
        // TODO: DEBUG
        // const utente = event.utente.nome + ' ' + event.utente.cognome;
        // const ruolo = event.ruoli;
        // const sede = event.sede.descrizione;
        // console.warn(utente + ' è diventato ' + ruolo + ' nel ' + sede);
    }

    onRemoveUtente(id: any) {
        this.store.dispatch(new OpenModalRemoveUtente(id));
        // TODO: DEBUG
        // const utente = event.utente.nome + ' ' + event.utente.cognome;
        // const ruolo = event.ruoli;
        // const sede = event.sede.descrizione;
        // console.warn('remove utente modal (' + id + ')');
    }

    getUtentiGestione() {
        this.store.dispatch(new GetUtentiGestione());
    }

    getRuoli() {
        this.store.dispatch(new GetRuoli());
    }

    getPermessi() {
        this.store.dispatch(new GetPermessi());
    }

    getRicerca() {
        this.ricerca$.subscribe(() => {
            this.store.dispatch(new GetUtentiGestione());
        });
    }
}
