export class GetFiltriComposizione {
    static readonly type = '[FiltriComposizione] Get Filtri';
}

export class SetFiltriComposizione {
    static readonly type = '[FiltriComposizione] Set Filtri';

    constructor(public filtri: any) {
    }
}

export class AddFiltroSelezionatoComposizione {
    static readonly type = '[FilterBarComposizione] Add Filtro Selezionato';

    constructor(public filtro: any, public tipo: string) {
    }
}

export class RemoveFiltroSelezionatoComposizione {
    static readonly type = '[FilterBarComposizione] Remove Filtro Selezionato';

    constructor(public filtro: any, public tipo: string) {
    }
}

export class RemoveFiltriSelezionatiComposizione {
    static readonly type = '[FilterBarComposizione] Remove Filtri Selezionati';

    constructor(public tipo: string) {
    }
}