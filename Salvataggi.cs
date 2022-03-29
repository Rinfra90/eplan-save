using System.IO;

public class Salvataggi
{
	static string cfg = @"C:\Program Files\EPLAN\Common\cfg.txt";
	Form1 form1 = new Form1();
		
	//Funzione per leggere dal file cfg
    static string leggi()
    {
        string read;
        StreamReader sr = new StreamReader(cfg);
        read = sr.ReadLine();
        sr.Close();
        if (read == null)
        {
            return @"C:\BackupEplan";
        }
        else
        {
            return read;
        }
    }

    //Funzione per scrivere sul file cfg
    static void scrivi(string text)
    {
        StreamWriter sw = new StreamWriter(cfg,false);
        sw.WriteLine(text);
        sw.Close();
    }

	public class Form1 : Form
	{
		Button b1 = new Button();
		TextBox tb1 = new TextBox();

		//Inizializzazione del Form
		public void Init ()
		{
			//Lettura percorso da file
			this.tb1.Text = Salvataggi.leggi();
			//Form
			this.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Settaggio Cartella";		
			this.Height = 200;
			this.Width = 350;
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			//Labels
			Label descr1 = new Label();
			descr1.Text = "L'attuale cartella è: ";
			TextBox path = new TextBox();
			path.ReadOnly = true;
			path.BorderStyle = 0;
			path.BackColor = this.BackColor;
			path.Text = Salvataggi.leggi();
			Label descr2 = new Label();
			descr2.Text = "Inserisci il nuovo percorso: ";
			//Textbox
			this.tb1.AcceptsReturn = false;
			this.tb1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			this.tb1.AutoCompleteSource = AutoCompleteSource.FileSystemDirectories;
			this.tb1.Name = "Cartella di salvataggio";
			this.tb1.Width = 200;
			//Pulsante OK
			this.b1.Text = "OK";
			this.b1.NotifyDefault(true);
			this.b1.Click += new System.EventHandler(this.Premuto);
			//Configurazione grafica del Form
			//Testi Labels
			FlowLayoutPanel labels = new FlowLayoutPanel();
			labels.FlowDirection = FlowDirection.LeftToRight;
			labels.Width = 300;
			labels.Height = 25;
			labels.Controls.Add(descr1);
			labels.Controls.Add(path);
			//Testi settaggio nuova cartella
			FlowLayoutPanel newSet = new FlowLayoutPanel();
			newSet.FlowDirection = FlowDirection.LeftToRight;
			newSet.Width = 300;
			newSet.Height = 50;
			newSet.Controls.Add(descr2);
			newSet.Controls.Add(tb1);
			//Setto il corpo principale
			FlowLayoutPanel body = new FlowLayoutPanel();
			body.FlowDirection = FlowDirection.TopDown;
			body.Width = 300;
			body.Height = 200;
			body.Controls.Add(labels);
			body.Controls.Add(newSet);
			body.Controls.Add(b1);
			//Aggiungo i contenitori al Form e setto il pulsante per quando si preme Invio
			this.AcceptButton = b1;
			this.Controls.Add(body);
		}

        //Funzione richiamata quando si preme il tasto OK del Form
		private void Premuto(object sender, EventArgs e)
		{
			Salvataggi.scrivi(this.tb1.Text);
			this.Close();
			return;
		}
	}

	//Funzione per settare il contesto da richiamare su Eplan
	private ActionCallingContext context(
							string backupAmount, 
							string type="PROJECT",
							string destinationPath="C:\\BackupEplan", 
							string archieveName="Backup", 
							string splitSize="0.0", 
							string backupMedia="DISK", 
							string backupMethod="BACKUP"
							)
	{
		ActionCallingContext backupContext = new ActionCallingContext();
		backupContext .AddParameter("TYPE",type);
		backupContext .AddParameter("DESTINATIONPATH",destinationPath);
		backupContext .AddParameter("ARCHIVENAME",archieveName);
		backupContext .AddParameter("SPLITSIZE",splitSize);
		backupContext .AddParameter("BACKUPMEDIA",backupMedia);
		backupContext .AddParameter("BACKUPAMOUNT",backupAmount);
		backupContext .AddParameter("BACKUPMETHOD",backupMethod);
		return backupContext;
	}
	
	//Azione per impostare la nuova cartella dove salvare il Backup
	[DeclareAction("SetFolder")]
	public void setfold()
	{
		//Inizializzo il Form
		this.form1.Init();
		//Mostro il Form
		this.form1.ShowDialog();
		return;
	}
	
	//Funzione che salva solo i dati necessari
	[DeclareAction("SaveFast")]
	public void saveFast()
	{
		ActionCallingContext backupContext = new ActionCallingContext();
		string folder = Salvataggi.leggi();
		backupContext = context(backupAmount: "BACKUPAMOUNT_MIN", destinationPath: folder);
		new CommandLineInterpreter().Execute("backup", backupContext);
		MessageBox.Show("Operazione eseguita","Progetto salvato",MessageBoxButtons.OK, MessageBoxIcon.Information);
		return;
	}
	
	//Funzione che fa un backup completo
	[DeclareAction("SaveAll")]
	public void saveAll()
	{
		ActionCallingContext backupContext = new ActionCallingContext();
		string folder = Salvataggi.leggi();
		backupContext = context(backupAmount: "BACKUPAMOUNT_ALL", destinationPath: folder);
		new CommandLineInterpreter().Execute("backup", backupContext);
		MessageBox.Show("Operazione eseguita","Progetto salvato",MessageBoxButtons.OK, MessageBoxIcon.Information);
		return;
	}
	
	//Dichiarazione delle azioni che verranno richiamate in Programmi di Servizio
	[DeclareMenu]
	public void MenuFunction()
	{
		Eplan.EplApi.Gui.Menu oMenu = new Eplan.EplApi.Gui.Menu();
		oMenu.AddMenuItem("Imposta Cartella","SetFolder");
		oMenu.AddMenuItem("Salva Progetto","SaveFast");
		oMenu.AddMenuItem("Salva Progetto in maniera completa","SaveAll");
	}
}