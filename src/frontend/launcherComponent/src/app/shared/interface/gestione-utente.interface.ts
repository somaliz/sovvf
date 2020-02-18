import { Role } from '../model/utente.model';
import { Sede } from '../model/sede.model';

export interface GestioneUtente {
    /**
     * id utente
     */
    id: string;
    /**
     * nominativo
     */
    nominativo: string;
    /**
     * ruolo utente
     */
    ruolo: Role;
    /**
     * sede relativa al ruolo utente
     */
    sede: Sede;
}