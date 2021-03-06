import { Utente } from '../../../shared/model/utente.model';

export class SignalRHubConnesso {
    static readonly type = '[signalR] Hub Connesso';
}

export class SignalRHubDisconnesso {
    static readonly type = '[signalR] Hub Disconnesso';
}

export class SetConnectionId {
    static readonly type = '[signalR] Set Connection ID';

    constructor(public connectionId: string) {
    }
}

export class SetUtenteSignalR {
    static readonly type = '[signalR] Set utente SignalR';

    constructor(public codiciSede: string[]) {
    }
}

export class ClearUtenteSignalR {
    static readonly type = '[Utente] Clear utente SignalR';

    constructor(public codiciSede: string[]) {
    }
}

export class LogoffUtenteSignalR {
    static readonly type = '[Utente] Logoff utente SignalR';

    constructor(public utente: Utente) {
    }
}

export class SetCodiceSede {
    static readonly type = '[signalR] Set Codice Sede';

    constructor(public codiciSede: string[]) {
    }
}

export class ClearCodiceSede {
    static readonly type = '[signalR] Clear Codice Sede';
}

export class SetIdUtente {
    static readonly type = '[signalR] Set ID Utente';

    constructor(public idUtente: string) {
    }
}

export class ClearIdUtente {
    static readonly type = '[signalR] Clear ID Utente';
}
