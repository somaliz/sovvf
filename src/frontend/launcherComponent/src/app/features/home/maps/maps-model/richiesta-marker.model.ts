import {Localita} from '../../../../shared/model/localita.model';
import {Tipologia} from '../../../../shared/model/tipologia.model';
import { StatoRichiesta } from '../../../../shared/enum/stato-richiesta.enum';
import { IconUrl } from '../maps-interface/icon-url-interface';

export class RichiestaMarker {
    constructor(
        /**
         * id
         */
        public id: string,
        /**
         * E' il codice della chiamata
         */
        public codice: string,
        /**
         * E' il codice della richiesta
         */
        public codiceRichiesta: string,
        /**
         * La stringa dell'indirizzo e le relative coordinate
         */
        public localita: Localita,
        /**
         * Id del tipo tipologia
         */
        public tipologia: Tipologia[],
        /**
         * Contiene la descrizione della label da mostrare
         */
        public label: string,
        /**
         * Determina la priorità del marcatore per scegliere il colore del marcatore
         * Es: Grave -> Marcatore rosso
         */
        public priorita: number,
        /**
         * Determina lo stato della richiesta
         */
        public stato: StatoRichiesta,
        /**
         * Determina se la richiesta è rilevante
         */
        public rilevanza: boolean,
        /**
         * Determina se la richiesta è rilevante Storica Artistica e Culturale
         */
        public rilevanzaStArCu: boolean,
        /**
         * l'url del marker richiesta
         */
        public iconUrl?: IconUrl
    ) {
    }

}
