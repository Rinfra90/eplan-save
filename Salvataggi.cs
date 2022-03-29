using System.IO;

public class Salvataggi
{
	string cfg = @"C:\Program Files\EPLAN\Common\cfg.txt";
	Form form1 = new Form();
	Button b1 = new Button();
	TextBox tb1 = new TextBox();

	//Funzione per leggere dal file cfg
	private string leggi()
	{
		string read;
		StreamReader sr = new StreamReader(this.cfg);
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
	private void scrivi(string text)
	{
		StreamWriter sw = new StreamWriter(this.cfg,false);
		sw.WriteLine(text);
		sw.Close();
		this.form1.Close();
	}
	
	//Funzione richiamata quando si preme il tasto OK del Form
	private void Premuto(object sender, EventArgs e)
	{
		scrivi(this.tb1.Text);
		this.form1.Close();
		return;
	}
		
	//Inizializzazione del Form
	private void Init ()
	{
		//Lettura percorso da file
		this.tb1.Text = leggi();
		//Form
		this.form1.StartPosition = FormStartPosition.CenterScreen;
		this.form1.Text = "Settaggio Cartella";		
		this.form1.Height = 200;
		this.form1.Width = 350;
		this.form1.FormBorderStyle = FormBorderStyle.FixedDialog;
		this.form1.MaximizeBox = false;
		this.form1.MinimizeBox = false;
		//Labels
		Label descr1 = new Label();
		descr1.Text = "L'attuale cartella è: ";
		TextBox path = new TextBox();
		path.ReadOnly = true;
		path.BorderStyle = 0;
		path.BackColor = this.form1.BackColor;
		path.Text = leggi();
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
		this.form1.AcceptButton = b1;
		this.form1.Controls.Add(body);
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
		this.Init();
		//Mostro il Form
		this.form1.ShowDialog();
		this.form1.Close();
		return;
	}
	
	//Funzione che salva solo i dati necessari
	[DeclareAction("SaveFast")]
	public void saveFast()
	{
		ActionCallingContext backupContext = new ActionCallingContext();
		string folder = leggi();
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
		string folder = leggi();
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