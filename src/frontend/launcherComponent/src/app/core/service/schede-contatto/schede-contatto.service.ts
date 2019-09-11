import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SchedaContatto } from 'src/app/shared/interface/scheda-contatto.interface';
import { HttpClient } from '@angular/common/http';
import { retry, catchError } from 'rxjs/operators';
import { handleError } from 'src/app/shared/helper/handleError';
import { environment } from 'src/environments/environment.prod';

const API_SCHEDE_CONTATTO = environment.apiUrl.schedeContatto;

@Injectable({
    providedIn: 'root'
})
export class SchedeContattoService {

    constructor(private http: HttpClient) {
    }

    getSchedeContatto(): Observable<SchedaContatto[]> {
        return this.http.get<SchedaContatto[]>(`${API_SCHEDE_CONTATTO}/GetLista`).pipe(
            retry(3),
            catchError(handleError)
        );
    }
}
