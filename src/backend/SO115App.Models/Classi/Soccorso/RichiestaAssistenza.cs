﻿//-----------------------------------------------------------------------
// <copyright file="RichiestaAssistenza.cs" company="CNVVF">
// Copyright (C) 2017 - CNVVF
//
// This file is part of SOVVF.
// SOVVF is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// SOVVF is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using SO115App.API.Models.Classi.Autenticazione;
using SO115App.API.Models.Classi.Composizione;
using SO115App.API.Models.Classi.Condivise;
using SO115App.API.Models.Classi.Persistenza;
using SO115App.API.Models.Classi.Soccorso.Eventi;
using SO115App.API.Models.Classi.Soccorso.Eventi.Fonogramma;
using SO115App.API.Models.Classi.Soccorso.Eventi.Partenze;
using SO115App.API.Models.Classi.Soccorso.Eventi.Segnalazioni;
using SO115App.API.Models.Classi.Soccorso.Mezzi.StatiMezzo;
using SO115App.API.Models.Classi.Soccorso.StatiRichiesta;
using SO115App.API.Models.Classi.Utenti;
using SO115App.Models.Classi.Condivise;
using SO115App.Models.Classi.Soccorso;
using SO115App.Models.Classi.Utility;

namespace SO115App.API.Models.Classi.Soccorso
{
    /// <summary>
    ///   Questa classe modella la generica richiesta di assistenza inoltrata ai VVF. La richiesta
    ///   può non avere seguito oppure generare un intervento da parte dei VVF. Esempi di istanze
    ///   sono: richiesta per un incendio in un'abitazione, richiesta per porta bloccata, richiesta
    ///   per vigilanza in occasione di una manifestazione, richiesta per partecipazione di una
    ///   delegazione VVF ad un convegno. Non è un'istanza di richiesta il terremoto, che essendo un
    ///   macro evento calamitoso darà luogo a più richieste di assistenza.
    /// </summary>
    public class RichiestaAssistenza : Entity
    {
        /// <summary>
        ///   Contiene la lista degli eventi considerati di interesse per la richiesta.
        /// </summary>
        private readonly List<Evento> _eventi;

        /// <summary>
        ///   Costruisce una nuova istanza di <see cref="RichiestaAssistenza" />
        /// </summary>
        [JsonConstructor]
        public RichiestaAssistenza()
        {
            this._eventi = new List<Evento>();
            this.Tipologie = new List<Tipologia>();
            this.Tags = new HashSet<string>();
            this.ListaUtentiInLavorazione = new List<AttivitaUtente>();
            this.ListaUtentiPresaInCarico = new List<AttivitaUtente>();
            //this.ListaPartenze = new List<Partenza>();
        }

        /// <summary>
        ///   E' la priorità della richiesta. Questa informazione è di ausilio nell'accodamento delle richieste.
        /// </summary>
        /// <remarks>
        ///   Al momento non è sensato assegnare una priorità a richieste del tipo vigilanza. Questo
        ///   aspetto deve essere valutato successivamente.
        /// </remarks>
        public enum Priorita
        {
            /// <summary>
            ///   Livello 1
            /// </summary>
            Bassissima = 1,

            /// <summary>
            ///   Livello 2
            /// </summary>
            Bassa,

            /// <summary>
            ///   Livello 3
            /// </summary>
            Media,

            /// <summary>
            ///   Livello 4
            /// </summary>
            Alta,

            /// <summary>
            ///   Livello 5
            /// </summary>
            Altissima
        }

        internal void SincronizzaStatoRichiesta(string stato, IStatoRichiesta statoRichiesta, string id, string motivazione)
        {
            if (stato == Costanti.RichiestaChiusa && !(statoRichiesta is Chiusa))
            {
                new ChiusuraRichiesta(motivazione, this, DateTime.UtcNow, id);
            }
            else if (stato.Equals(Costanti.RichiestaRiaperta) && !(statoRichiesta is Riaperta))
            {
                new RiaperturaRichiesta(this, DateTime.UtcNow, id);
            }
            else if (stato.Equals(Costanti.RichiestaAssegnata) && !(statoRichiesta is Assegnata))
            {
                new AssegnataRichiesta(this, DateTime.UtcNow, id);
            }
            else if (stato.Equals(Costanti.RichiestaPresidiata) && !(statoRichiesta is Presidiata))
            {
                new RichiestaPresidiata(this, DateTime.UtcNow, id);
            }
        }

        /// <summary>
        ///   Questo metodo analizza la richiesta, ne desume il tipo di rilevanza e aggiunge gli
        ///   eventi necessari a sincronizzare la rilevanza corrente con quella richiesta nei
        ///   parametri di ingresso.
        /// </summary>
        /// <param name="rilevanzaGrave">
        ///   Indica se la richiesta deve impostata a rilevante per gravità
        /// </param>
        /// <param name="rilevanzaStArCu">
        ///   Indica se la richiesta deve impostata a rilevante per edifici Storico/Artistico/Culturali
        /// </param>
        /// <param name="fonte">La fonte dell'evento</param>
        /// <param name="motivazione">La motivazione dell'aggiornamento della rilevanza</param>
        internal void SincronizzaRilevanza(bool rilevanzaGrave, bool rilevanzaStArCu, string fonte, string motivazione, DateTime istateRichiesta)
        {
            var ultimoEventoRilevanza = (MarcaRilevante)this.Eventi.LastOrDefault(e => e is MarcaRilevante);

            var rilevanzaGraveCorrente =
                ultimoEventoRilevanza != null &&
                (ultimoEventoRilevanza as MarcaRilevante).PerGravita;

            var rilevanzaStArCuCorrente =
                ultimoEventoRilevanza != null &&
                (ultimoEventoRilevanza as MarcaRilevante).PerEdificioStArCu;

            if ((rilevanzaGraveCorrente ^ rilevanzaGrave) || (rilevanzaStArCuCorrente ^ rilevanzaStArCu))
            {
                this.AddEvento(new MarcaRilevante(this, istateRichiesta, fonte, motivazione, rilevanzaGrave, rilevanzaStArCu));
            }
        }

        /// <summary>
        ///   <para>
        ///     Il codice intervento è un codice parlante che si utilizza per identificare
        ///     univocamente una richiesta. Si può per esempio utilizzare nelle comunicazioni verbali
        ///     o annotare su un foglietto.
        ///   </para>
        ///   <para>
        ///     Il codice deve essere progettato con l'obiettivo di garantire i seguenti requisiti:
        ///     * facilità rinumerazione dei vecchi interventi;
        ///     * leggibilità del codice (per es. facilità di annotazione);
        ///     * facilità di comunicarlo verbalmente.
        ///   </para>
        ///   <para>
        ///     Si stabilisce di utilizzare un sistema del tipo 223.117.949, ossia un numero
        ///     progressivo raggruppato in terzine. Il codice è comunque di tipo stringa.
        ///   </para>
        ///   <para>
        ///     Il criterio di mapping dei codici dei vecchi interventi SO115 sarà del tipo:
        ///     RM1.700.700 (RM= sigla provincia, 1.7=anno e 00700=numero intervento)
        ///   </para>
        ///   <para>
        ///     Gli interventi importati da SO115 verranno mappati su una maschera del tipo:
        ///     RM1.700.000 in cui
        ///     - RM è il codice della provincia attualmente usato,
        ///     - 17 sono le ultime due cifre dell'anno dell'intervento
        ///     - le restanti cifre sono l'attuale numero intervento (senza progressivo) per un
        ///     totale di 100.000 interventi mappabili per ogni anno (mai raggiunto).
        ///   </para>
        ///   <para>
        ///     Gli operativi del CNVVF spingono per avere un codice parlante, del tipo "RM.12345".
        ///     In questa modalità di composizione, il codice reca informazioni geografiche che
        ///     potrebbero variare durante il corso di evasione dell'intervento, rendendo il codice
        ///     fuorviante. Tuttavia per il momento si decide di usare questa metodologia.
        ///   </para>
        /// </summary>
        public string Codice { get; set; }

        /// <summary>
        ///   E' il codice generato nel momento in cui una chiamata viene gestita
        /// </summary>
        /// <remarks>
        ///   A differenza del codice questo campo non è giornaliero ma annuale, per questo nel
        ///   codice non sono presenti il giorno il mese e l'anno
        /// </remarks>
        public string CodiceRichiesta { get; set; }

        /// <summary>
        ///   E' il codice dell'unità operativa competente per l'intervento
        /// </summary>
        /// <remarks>
        ///   La competenza di un intervento può cambiare durante la vita della richiesta. Il cambio
        ///   è documentato da un apposito evento della richiesta.
        /// </remarks>
        public string CodiceUnitaOperativaCompetente { get; set; }

        /// <summary>
        ///   E' il set dei codici delle unità operative che sono chiamate a supporto.
        /// </summary>
        /// <remarks>
        ///   Le unità allertate possono partecipare alla gestione dell'intervento limitatamente a
        ///   determinati specifici casi d'uso (per es. composizione partenza, apposizione nota
        ///   intervento, ecc.)
        /// </remarks>
        public ISet<string> CodiciUnitaOperativeAllertate { get; set; }

        /// <summary>
        ///   Espone la lista degli eventi considerati di interesse per la richiesta.
        /// </summary>
        public IReadOnlyList<Evento> Eventi
        {
            get
            {
                return this._eventi;
            }
        }

        /// <summary>
        ///   Operatore che ha generato la segnalazione
        /// </summary>
        public Utente Operatore { get; set; }

        /// <summary>
        ///   Indica se la richiesta è sospesa
        /// </summary>
        /// <remarks>
        ///   La richiesta è sospesa se, prima del termine della sua evasione, tutte le risorse le
        ///   sono state sottratte e dirottate presso altro intervento
        /// </remarks>
        public bool Sospesa
        {
            get
            {
                return this.StatoRichiesta is Sospesa;
            }
        }

        /// <summary>
        ///   Indica se la richiesta è in attesa
        /// </summary>
        /// <remarks>La richiesta è in attesa se non le è stata ancora assegnata alcuna partenza</remarks>
        public bool InAttesa
        {
            get
            {
                return this.StatoRichiesta is InAttesa;
            }
        }

        /// <summary>
        ///   Restituisce l'elenco degli stati dei mezzi coinvolti nella Richiesta di Assistenza
        /// </summary>
        public IDictionary<string, IStatoMezzo> MezziCoinvolti
        {
            get
            {
                var ListaComposizioni = this.Eventi
                    .Where(e => e is Classi.Soccorso.Eventi.Partenze.ComposizionePartenze)
                    .Select(e => e as Classi.Soccorso.Eventi.Partenze.ComposizionePartenze)
                    .ToList();

                //return ListaComposizioni.Select(x => x.Partenza).ToList();

                var d = new Dictionary<string, IStatoMezzo>();

                //var eventiPartenza = this.eventi
                //    .Where(e => e is IPartenza)
                //    .Select(e => (IPartenza)e);

                var eventiPartenzaRaggruppatiPerMezzo = ListaComposizioni.SelectMany
                    (
                        e => e.Partenza.Mezzo.Codice,
                        (evento, codiceMezzo) => new
                        {
                            Codice = codiceMezzo,
                            Evento = evento
                        }
                    )
                    .GroupBy(el => el.Codice, el => el.Evento);

                foreach (var gruppoEventiPartenza in eventiPartenzaRaggruppatiPerMezzo)
                {
                    var codice = gruppoEventiPartenza.Key;
                    var eventi = gruppoEventiPartenza.AsEnumerable();
                    var processoreStato = new ProcessoreStato();
                    processoreStato.ProcessaEventi(eventi);
                    var stato = processoreStato.Stato;

                    d[codice.ToString()] = stato;
                }

                // Se la richiesta è chiusa, i mezzi devono essere stati tutti liberati.
                Debug.Assert(!this.Chiusa || d.Values.All(stato => stato.Disponibile), "La richiesta è chiusa ma contiene mezzi non liberi");

                return d;
            }
        }

        /// <summary>
        ///   Restituisce l'elenco delle squadre coinvolte nella Richiesta di Assistenza
        /// </summary>
        public IEnumerable<SquadraCoinvolta> SquadreCoinvolte
        {
            get;
            //set;
        }

        //public List<Partenza> ListaPartenze { get; set; }

        /// <summary>
        ///   Indica l'istante di chiusura della richiesta, impostato dall'evento <see cref="ChiusuraRichiesta" />
        /// </summary>
        public DateTime? IstanteChiusura { get; internal set; }

        /// <summary>
        ///   E' la lista ordinata (per importanza decrescente) delle tipologie di soccorso.
        /// </summary>
        /// <remarks>
        ///   Per es. è la lista { valanga, soccorso a persona, ricerca disperso, messa in sicurezza
        ///   } in un sinistro simile al Rigopiano
        /// </remarks>
        public virtual List<Tipologia> Tipologie { get; set; }

        /// <summary>
        ///   La località dove è avvenuto il fatto
        /// </summary>
        public virtual Localita Localita { get; set; }

        /// <summary>
        ///   Il turno nel quale viene presa la chiamata
        /// </summary>
        public Turno TurnoInserimentoChiamata { get; set; }

        /// <summary>
        ///   Il turno nel quale viene lavorato l'intervento
        /// </summary>
        public Turno TurnoIntervento { get; set; }

        /// <summary>
        ///   Indica se il terreno è uno tra Boschi/Campi/Sterpaglie e ne indica i mq.
        /// </summary>
        public List<TipologiaTerreno> TipoTerreno { get; set; }

        /// <summary>
        ///   Lista degli enti intervenuti (Es. ACEA)
        /// </summary>
        public List<EntiIntervenuti> ListaEntiIntervenuti { get; set; }

        /// <summary>
        ///   Lista degli enti al quale è stata passata la richiesta (Es. ACEA)
        /// </summary>
        public List<EntiIntervenuti> ListaEntiPresaInCarico { get; set; }

        /// <summary>
        ///   Se l'intervento è su un obiettivo ritenuto rilevante (Es. Colosseo) si seleziona da
        ///   interfaccia e si registra il codice
        /// </summary>
        public ObiettivoSensibile ObiettivoSensibile { get; set; }

        /// <summary>
        ///   E' la descrizione della richiesta, che ne sintetizza le caratteristiche principali.
        /// </summary>
        public virtual string Descrizione { get; set; }

        /// <summary>
        ///   E' la zona di emergenza a cui la richiesta è legata
        /// </summary>
        public virtual string[] ZoneEmergenza { get; set; }

        /// <summary>
        ///   Sono i tags legati alla richiesta di assistenza
        /// </summary>
        public ISet<string> Tags { get; set; }

        /// <summary>
        ///   Indica se la richiesta è aperta
        /// </summary>
        public bool Chiusa
        {
            get
            {
                return this.IstanteChiusura.HasValue;
            }
        }

        /// <summary>
        ///   Indica se la richiesta è chiusa
        /// </summary>
        public bool Aperta
        {
            get
            {
                return !this.Chiusa;
            }
        }

        /// <summary>
        ///   Indica se il luogo del sinistro è presidiato
        /// </summary>
        public virtual bool Presidiata
        {
            get
            {
                var ElencoPresidiate = this.Eventi
                    .Where(e => e is Classi.Soccorso.Eventi.RichiestaPresidiata)
                    .Select(e => e as Classi.Soccorso.Eventi.RichiestaPresidiata)
                    .ToList();

                if (ElencoPresidiate.Count > 0)
                    return true;

                return false;
            }
        }

#warning realizzare i metodi che restituiscono "n.richieste evase" e "n.mezzi intervenuti (RientratoInSede)" e utilizzarli per gli indicatori di soccorso

        /// <summary>
        ///   Restituisce la priorità della richiesta. Se non è esplicitamente impostata è uguale a <see cref="Priorita.Media" />.
        /// </summary>
        public virtual Priorita PrioritaRichiesta
        {
            get
            {
                var eventoAssegnazionePriorita = this._eventi
                    .Where(e => e is AssegnazionePriorita)
                    .LastOrDefault() as AssegnazionePriorita;

                return eventoAssegnazionePriorita != null ? eventoAssegnazionePriorita.Priorita : RichiestaAssistenza.Priorita.Media;
            }
        }

        /// <summary>
        ///   Restituisce l'istante della Richiesta di Assistenza
        /// </summary>
        public virtual DateTime? IstanteRicezioneRichiesta
        {
            get
            {
                var eventoSegnalazione = this._eventi
                    .Where(e => e is Segnalazione)
                    .FirstOrDefault() as Segnalazione;

                if (eventoSegnalazione == null)
                {
                    return null;
                }
                else
                {
                    return eventoSegnalazione.Istante;
                }
            }
        }

        /// <summary>
        ///   Restituisce l'istante della prima presa in carico da parte di un operatore,
        ///   identificato dall'apposito evento.
        /// </summary>
        public virtual DateTime? IstantePresaInCarico
        {
            get
            {
                var eventoPresaInCarico = this._eventi
                    .Where(e => e is InizioPresaInCarico)
                    .FirstOrDefault() as InizioPresaInCarico;

                if (eventoPresaInCarico == null)
                {
                    return null;
                }
                else
                {
                    return eventoPresaInCarico.Istante;
                }
            }
        }

        ///<summary>
        ///
        /// Restituisce la lista di Utenti che hanno in lavorazione la richiesta
        ///
        /// </summary>
        public List<AttivitaUtente> ListaUtentiInLavorazione { get; set; }

        ///<summary>
        ///
        /// Restituisce la lista di Utenti che hanno preso in carico la richiesta
        ///
        /// </summary>
        public List<AttivitaUtente> ListaUtentiPresaInCarico { get; set; }

        /// <summary>
        ///   Restituisce l'istante della prima assegnazione di una risorsa squadra/mezzo alla <see cref="RichiestaAssistenza" />.
        /// </summary>
        public virtual DateTime? IstantePrimaAssegnazione
        {
            get
            {
                var eventoAssegnazione = this._eventi
                    .Where(e => e is ComposizionePartenze)
                    .FirstOrDefault() as ComposizionePartenze;

                if (eventoAssegnazione == null)
                {
                    return null;
                }
                else
                {
                    return eventoAssegnazione.Istante;
                }
            }
        }

        /// <summary>
        ///   Indica se la richiesta è rilevante per qualche motivo
        /// </summary>
        public bool Rilevante
        {
            get
            {
                var ultimoEventoRilevanza = (MarcaRilevante)this._eventi
                    .Where(e => e is MarcaRilevante)
                    .LastOrDefault();

                if (ultimoEventoRilevanza != null)
                    return ultimoEventoRilevanza.PerGravita || ultimoEventoRilevanza.PerEdificioStArCu;

                return false;
            }
        }

        /// <summary>
        ///   Indica se la richiesta è rilevante per gravità
        /// </summary>
        public bool RilevanteGrave
        {
            get
            {
                var ultimoEventoRilevanza = (MarcaRilevante)this._eventi
                    .Where(e => e is MarcaRilevante)
                    .LastOrDefault();

                if (ultimoEventoRilevanza != null)
                    return ultimoEventoRilevanza.PerGravita;

                return false;
            }
        }

        /// <summary>
        ///   Indica se la richiesta è rilevante per edificio Storico/Artistico/Culturale
        /// </summary>
        public bool RilevanteStArCu
        {
            get
            {
                var ultimoEventoRilevanza = (MarcaRilevante)this._eventi
                    .Where(e => e is MarcaRilevante)
                    .LastOrDefault();

                if (ultimoEventoRilevanza != null)
                    return ultimoEventoRilevanza.PerEdificioStArCu;

                return false;
            }
        }

        /// <summary>
        ///   Restituisce lo stato della Richiesta
        /// </summary>
        public IStatoRichiesta StatoRichiesta
        {
            get
            {
                var eventoChiusura = this._eventi
                    .Where(e => e is ChiusuraRichiesta);

                if (this.Chiusa)
                {
                    return new Chiusa();
                }

                if (this.Presidiata)
                    return new Presidiata();

                var composizionePartenza = this.Partenze;
                //var elencoMezziCoinvolti = this.MezziCoinvolti;
                if (!composizionePartenza.Any())
                {
                    return new InAttesa();
                }
                else
                {
                    if (composizionePartenza.Any(e => e.Partenza.Mezzo.Stato == "In Viaggio"))
                    {
                        return new Assegnata();
                    }
                    else
                    {
                        return new Sospesa();
                    }
                }
            }
        }

        /// <summary>
        ///   Restituisce le sole istanze della classe Telefonata presenti all'interno della lista
        ///   degli eventi.
        /// </summary>
        public IList<Telefonata> Telefonate
        {
            get
            {
                return this._eventi
                    .Where(e => e is Telefonata)
                    .Select(e => e as Telefonata)
                    .ToList();
            }
        }

        public IList<ComposizionePartenze> Partenze
        {
            get
            {
                var ListaComposizioni = this.Eventi
                    .Where(e => e is Classi.Soccorso.Eventi.Partenze.ComposizionePartenze)
                    .Select(e => e as Classi.Soccorso.Eventi.Partenze.ComposizionePartenze)
                    .ToList();

                return ListaComposizioni;
            }
        }

        /// <summary>
        ///   Il richiedente della richiesta.
        /// </summary>
        public virtual Richiedente Richiedente { get; set; }

        /// <summary>
        ///   E' il codice delle unità operative di prima, seconda, terza... competenza, in ordine di preferenza.
        /// </summary>
        public virtual string[] CodiciUOCompetenza { get; set; }

        /// <summary>
        ///   Lista delle Competenze della richiesta
        /// </summary>
        public virtual List<Sede> Competenze { get; set; }

        /// <summary>
        ///   Codice della scheda Nue, estratto dalla prima telefonata che è legata ad una scheda
        ///   contatto del Nue.
        /// </summary>
        public virtual string CodiceSchedaNue
        {
            get
            {
                var primoCodiceNue = this._eventi
                    .Where(e => e is Telefonata)
                    .Select(e => e as Telefonata)
                    .Select(e => e.CodiceSchedaContatto)
                    .Where(cod => !string.IsNullOrWhiteSpace(cod))
                    .FirstOrDefault();

                return primoCodiceNue;
            }
        }

        /// <summary>
        ///   Calcola lo stato di invio del fonogramma per la richiesta, in base all'ultimo evento
        ///   fonogramma presente nella richiesta.
        /// </summary>
        public virtual Fonogramma.IStatoFonogramma StatoInvioFonogramma
        {
            get
            {
                var ultimoEventoFonogramma = this._eventi
                    .Where(e => e is IFonogramma)
                    .LastOrDefault();

                if (ultimoEventoFonogramma is FonogrammaInviato)
                {
                    return new Fonogramma.Inviato();
                }

                if (ultimoEventoFonogramma is InviareFonogramma)
                {
                    return new Fonogramma.DaInviare();
                }

                return new Fonogramma.NonNecessario();
            }
        }

        /// <summary>
        ///   Modella la complessità legata ad una <see cref="RichiestaAssistenza" />. Le soglie sono
        ///   le seguenti: 0 - 20: bassa; 21 - 60: media; maggiore di 61: alta;
        /// </summary>
        public virtual Complessita.IComplessita Complessita
        {
            get
            {
                if (this._eventi.Count <= 20)
                {
                    return new Complessita.Bassa();
                }

                if (this._eventi.Count <= 60)
                {
                    return new Complessita.Media();
                }

                return new Complessita.Alta();
            }
        }

        /// <summary>
        ///   Aggiunge un evento alla lista degli eventi. L'evento deve essersi verificato in un
        ///   istante superiore a quello dell'ultimo evento in lista. In caso contrario il metodo
        ///   solleva un'eccezione.
        /// </summary>
        /// <param name="evento">L'evento da aggiungere</param>
        public void AddEvento(Evento evento)
        {
            if (this._eventi.Any() && this._eventi.Last().Istante > evento.Istante)
            {
                throw new InvalidOperationException("Impossibile aggiungere un evento ad una richiesta che ne ha già uno più recente.");
            }

            this._eventi.Add(evento);
        }
    }
}
