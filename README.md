
# Scripts Eplan

---

In lavorazione

---

## Funzione

Script **C#** per gestione e automatizzazione di processi *Eplan*.

### Indice

- Contenuto Progetto
    - Salvataggi.cs
    - Livelli.cs
    - Lingue.cs
- Procedura d'installazione
- Contatti

## Contenuto Progetto

All'interno del progetto sono presenti dei file `.cs` che contengono i vari script necessari.
Di seguito una panoramica file per file.

### Salvataggi.cs

- Contiene 3 azioni
  1. Impostare Cartella - Serve per settare la cartella in cui verrà salvato il file di backup.
  2. Salvataggio Rapido - Salva solo i dati necessari a ritirare su il progetto. Consigliato per progetti molto grandi.
  3. Salvataggio Completo - Salva tutti i dati di un progetto.
- Necessita la creazione di un file `cfg.txt` all'interno della cartella: `C:\Program Files\EPLAN\Common`
  > **NOTA:** Il file deve essere creato a mano precedentemente per problemi relativi ai permessi di scrittura. Il contenuto può anche essere vuoto.
  >       Per cambiare la cartella in cui salvare il file `cfg.txt`, basta modificare il valore della variabile string cfg all'interno del codice alla riga 5.
  >       `string cfg = @"C:\Program Files\EPLAN\Common\cfg.txt";`. *Ricordarsi di inserire anche il nome del file e l'estenzione*

### 

## Procedura d'installazione

### Installazione

Per caricare gli Script all'interno di Eplan, andare in: `Programmi di servizio->Script->Carica...`.
Una volta caricato lo script, le relative azioni saranno disponibili all'interno del Menù *Programmi di Servizio*
  > **NOTA:** Lo script verrà ricaricaricato ad ogni nuovo avvio di Eplan o quando verrà riutilizzato il campo *Carica...* , perciò è consigliato caricarlo nella directory in cui si vorrà lasciare e di non spostarlo. Il consiglio è di utilizzare sempre `C:\Program Files\EPLAN\Common` per praticità

### Disinstallazione

Per rimuovere lo script da Eplan, basterà selezionare la voce `Programmi di servizio->Script->Scarica...`

### Contatti
