using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PileSwLib;
using PileSwLib.UtilTest;
using log4net;
using System.Diagnostics;
using System.Reflection;
using PileSwLib.Tipi;
using PTLib.CFG;
using OutlookStyleControls;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace PileSW
{

    public partial class frmMain : Form
    {
        private string fileChm = "";
        public string CHMFile { get { return fileChm; } }
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private static readonly ILog log = LogManager.GetLogger(typeof (Program)) ;
        public List<Dati> ListaDati = new List<Dati>();
        public List<Dati> ListaDatiC = new List<Dati>();
        public ParameterDictionary Parameter;
        public ListNameRegEx NomiRegEx;
        private ExcelPackage _package;
        StringBuilder s2 = new StringBuilder();
        string FileDaAprire = "";
        BackgroundWorker bw = new BackgroundWorker();

        int TDiff = 0;
        int TSKit = 0;
        int TSCam = 0;

        private void ReadFile()
        {
            string filename;
                
            OpenFileDialog sdf = new OpenFileDialog();
            sdf.Filter = "Pile File|*.PileSW|XML File|*.xml|TXT File|*.*";
            if (sdf.ShowDialog() != DialogResult.OK)
                return;
            filename = sdf.FileName;
            ListaDatiC = PileSwLib.PileSwLib.ReadFile(filename);
            FillGrid1(dgw2, ListaDatiC);
            SetTitle(filename);
        }
        private void ReadFile(string filename)
        {
            ListaDatiC = PileSwLib.PileSwLib.ReadFile(filename);
            FillGrid1(dgw2, ListaDatiC);
            SetTitle(filename);
        }
        private void SetTitle(string filename)
        {
            string s = "";
            string[] parti = this.Text.Split('-');
            parti[3] = " " + Path.GetFileName(filename);
            s = parti[0] + "-" + parti[1] + "-" + parti[2] + "-" + parti[3];
            this.Text = s; 
        }
        private void WriteFile()
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<Dati>));
            string filename;
            SaveFileDialog sdf = new SaveFileDialog();
            sdf.Filter = "Pile File|*.PileSW|XML File|*.xml|TXT File|*.*";
            if (sdf.ShowDialog() != DialogResult.OK)
                return;
            filename = sdf.FileName;
            PileSwLib.PileSwLib.WriteFile(filename, ListaDati);
        }

        private void SaveExcel()
        {
            int r = 0;
            _package = new ExcelPackage(new MemoryStream());

            var ws1 = _package.Workbook.Worksheets.Add("Delta");
            r = 2;

            ws1.Cells[r, 1].Value = "Tipo"; ws1.Cells[r, 1].Style.Font.Bold = true;
            ws1.Cells[r, 2].Value = "Name"; ws1.Cells[r, 2].Style.Font.Bold = true;
            ws1.Cells[r, 3].Value = "Display Name"; ws1.Cells[r, 3].Style.Font.Bold = true;
            ws1.Cells[r, 4].Value = "Display Version"; ws1.Cells[r, 4].Style.Font.Bold = true;
            ws1.Cells[r, 5].Value = "Name"; ws1.Cells[r, 5].Style.Font.Bold = true;
            ws1.Cells[r, 6].Value = "Display Name"; ws1.Cells[r, 6].Style.Font.Bold = true;
            ws1.Cells[r, 7].Value = "Display Version"; ws1.Cells[r, 7].Style.Font.Bold = true;

            r++;

            for (int i = 0; i < dgvDelta.Rows.Count; i++)
            {
                DataGridViewRow item = dgvDelta.Rows[i];
                for (int t = 1; t < 8; t++)
                {
                    string ss = (item.Cells[t].Value != null ? item.Cells[t].Value.ToString() : " ");
                    ws1.Cells[r, t].Value = ss;

                    ws1.Cells[r, t].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                }
                ws1.Cells[r, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws1.Cells[r, 1].Style.Fill.BackgroundColor.SetColor(item.Cells[0].Style.BackColor);
                r++;
            }
            r--;
            (ws1.Column(1)).Width = 3;
            (ws1.Column(2)).Width = 11;
            (ws1.Column(3)).Width = 33;
            (ws1.Column(4)).Width = 14.5;
            (ws1.Column(5)).Width = 11;
            (ws1.Column(6)).Width = 33;
            (ws1.Column(7)).Width = 14.5;
            //   ws1.Cells["G1:G" + r.ToString()].AutoFitColumns();
            ws1.Cells["B1:D1"].Merge = true;
            ws1.Cells["E1:G1"].Merge = true;
            ws1.Cells[1, 2].Value = "Kit"; ws1.Cells[1, 2].Style.Font.Bold = true; ws1.Cells[1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws1.Cells[1, 5].Value = "Campione"; ws1.Cells[1, 5].Style.Font.Bold = true; ws1.Cells[1, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws1.Cells["A1:G" + r.ToString()].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
            ws1.Cells["B1:D" + r.ToString()].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
            ws1.Cells["E1:G" + r.ToString()].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
            _package.Workbook.Calculate();
            var ws = _package.Workbook.Worksheets.Add("Riassunto");
            r = 3;
            (ws.Column(1)).Width = 25;
            ws.Cells[r, 1].Value = "Test eseguito il"; ws.Cells[r, 2].Value = DateTime.Now.ToString(); ws.Cells[r, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; r++;
            ws.Cells[r, 1].Value = "Righe solo presenti in Kit:"; ws.Cells[r, 2].Value = TSKit; ws.Cells[r, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; r++;
            ws.Cells[r, 1].Value = "Righe solo presenti in Campione:"; ws.Cells[r, 2].Value = TSCam; ws.Cells[r, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; r++;
            ws.Cells[r, 1].Value = "Righe differenti in versione:"; ws.Cells[r, 2].Value = TDiff; ws.Cells[r, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; r++;
            //-------------------
            SaveFileDialog saveFileDialog_SaveExcel = new SaveFileDialog();
            saveFileDialog_SaveExcel.Filter = "Excel files (*.xlsx)|*.xlsx";
            var dialogResult = saveFileDialog_SaveExcel.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                _package.SaveAs(new FileInfo(saveFileDialog_SaveExcel.FileName));
                //proviamo a lanciarlo
                OpenExcel(saveFileDialog_SaveExcel.FileName);
            }
            _package = null;
        }

        void SetChmFile()
        { //  fileChm
            var data = Properties.Resources.Manuale_SwPile;
            fileChm = Path.GetTempFileName() + ".chm";
            using (var stream = new FileStream(fileChm, FileMode.Create))
            {
                stream.Write(data, 0, data.Count() - 1);
                stream.Flush();
            }


        }


        public frmMain(string[] args)
        {

            Parameter = new ParameterDictionary();
            NomiRegEx = new ListNameRegEx();

            InitializeComponent();

            Logger.Setup(Parameter.GetBool("LogOn"));
            PileSwLib.PileSwLib.OnUpdatePosition += SwPileUtil_OnUpdatePosition;

            //Set logs da fare subito
            PileSwLib.UtilTest.UtilTestLib.SetLog(log); //SwPileLib
            PileSwLib.PileSwLib.SetLog(log);  //SwPileLibUtil
            //--------------------------
            PileSwLib.PileSwLib.UseWMI = false;// Properties.Settings.Default.UseWMI;
            PileSwLib.PileSwLib.SetServiceConfig(Parameter.StrCfg);

            //Start Work
            bw.DoWork += bw_DoWork;
            if (bw.IsBusy != true)
            {
                bw.RunWorkerAsync();
            }

            toolStrip1_Resize(this, new EventArgs());

            log.Warn("Started at " + DateTime.Now.ToString() + " " + SystemInformation.UserName);

            SetChmFile();
            if (args.Count() > 0)
                FileDaAprire = args[0];
        }
        // In background per partire subito senza blocchi
        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            AssemblyName l1 = Assembly.GetExecutingAssembly().GetName();
            string share = Parameter.GetString("Share");
            try
            {
                // Get the file version for the pilesw.
                FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(Path.Combine( share,@"PileSW.exe"));
                Int64 localeVer = GetSerial(l1);
                Int64 remoteVer = Util.VerToSerial(myFileVersionInfo.FileMajorPart, myFileVersionInfo.FileMinorPart, myFileVersionInfo.FileBuildPart, myFileVersionInfo.FilePrivatePart);
                if (localeVer < remoteVer)
                {
                    MessageBox.Show(@"Nuova versione disponinile in "+share, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                if (localeVer > remoteVer)
                {
                    MessageBox.Show(@"Nuova versione da caricare in remoto", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                e.Result = false;
            }
            e.Result = true;
        }
        Int64 GetSerial(AssemblyName l)
        {
            return Util.VerToSerial(l.Version.Major, l.Version.Minor, l.Version.Build, l.Version.Revision);
        }
        string GetVer(AssemblyName l)
        {
            return l.Version.Major.ToString().PadLeft(2, '0') + "." + l.Version.Minor.ToString().PadLeft(2, '0') + "." + l.Version.Build.ToString().PadLeft(2, '0') + "." + l.Version.Revision.ToString().PadLeft(4, '0');
        }

        void SwPileUtil_OnUpdatePosition(PileSwLib.ProgressEventArgs e)
        {
            this.Delta.Text = e.Status;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WriteFile();
        }


        private void FillGrid1b(OutlookGrid dgw, List<Dati> Lista)
        {
            dgw.BindData(null, null);
            dgw.Rows.Clear();
            dgw.Columns.Clear();
            // dgw.Columns.Add("TC", "");
            
            //dgw.Columns.Add(new DataGridViewTextBoxColumn() {HeaderText="Tipo",Name="Tipo", CellTemplate = new MyDataGridViewTextBoxCell() });
            dgw.Columns.Add("Tipo", "Tipo");
            dgw.Columns.Add("Name", "Name");
            dgw.Columns.Add("DisplayName", "Display Name");
            dgw.Columns.Add("DisplayVersion", "Display Version");
            dgw.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //dgw.Columns["TC"].Width = 10; dgw.Columns["TC"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgw.Columns["Tipo"].Width = 40; dgw.Columns["Tipo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None; dgw.Columns["Tipo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgw.Columns["Tipo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgw.Columns["Name"].FillWeight = 27;
            dgw.Columns["DisplayName"].FillWeight = 39;
            dgw.Columns["DisplayVersion"].FillWeight = 27;

            foreach (Dati riga in Lista)
            {
                dgw.Rows.Add();
                dgw.Rows[dgw.RowCount - 1].Cells["Tipo"].Value = riga.Tipo;
                dgw.Rows[dgw.RowCount - 1].Cells["Name"].Value = riga.GUID;
                dgw.Rows[dgw.RowCount - 1].Cells["DisplayName"].Value = riga.DisplayName;
                dgw.Rows[dgw.RowCount - 1].Cells["DisplayVersion"].Value = riga.DisplayVersion;
            }

        }
        
        private void FillGrid1(DataGridView dgw, List<Dati> Lista)
        {

            dgw.Rows.Clear();
            dgw.Columns.Clear();
            // dgw.Columns.Add("TC", "");
            dgw.Columns.Add("Tipo", "Tipo");
            dgw.Columns.Add("Name", "Name");
            dgw.Columns.Add("DisplayName", "Display Name");
            dgw.Columns.Add("DisplayVersion", "Display Version");
            dgw.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //dgw.Columns["TC"].Width = 10; dgw.Columns["TC"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgw.Columns["Tipo"].Width = 40; dgw.Columns["Tipo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None; dgw.Columns["Tipo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgw.Columns["Tipo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgw.Columns["Name"].FillWeight = 27;
            dgw.Columns["DisplayName"].FillWeight = 39;
            dgw.Columns["DisplayVersion"].FillWeight = 27;

            foreach (Dati riga in Lista)
            {
                dgw.Rows.Add();
                dgw.Rows[dgw.RowCount - 1].Cells["Tipo"].Value = riga.Tipo;
                dgw.Rows[dgw.RowCount - 1].Cells["Name"].Value = riga.GUID;
                dgw.Rows[dgw.RowCount - 1].Cells["DisplayName"].Value = riga.DisplayName;
                dgw.Rows[dgw.RowCount - 1].Cells["DisplayVersion"].Value = riga.DisplayVersion;
            }

        }
        private void SetGridDelta()
        {
            DataGridView dgw = dgvDelta;

            dgw.Rows.Clear();
            dgw.Columns.Clear();
            dgw.Columns.Add("Quale", "<>");
            dgw.Columns.Add("Tipo", "Tipo");
            dgw.Columns.Add("Name", "Name");
            dgw.Columns.Add("DisplayName", "Display Name");
            dgw.Columns.Add("DisplayVersion", "Display Version");
            dgw.Columns.Add("Name2", "Name2");
            dgw.Columns.Add("DisplayName2", "Display Name2");
            dgw.Columns.Add("DisplayVersion2", "Display Version2");

            dgw.Columns["Quale"].Width = 35; dgw.Columns["Quale"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None; dgw.Columns["Quale"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            //           dgw.Columns["Quale"].FillWeight = 7; dgw.Columns["Quale"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //            dgw.Columns["Tipo"].FillWeight = 7; ; dgw.Columns["Tipo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns["Tipo"].Width = 40; dgw.Columns["Tipo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None; dgw.Columns["Tipo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgw.Columns["Tipo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgw.Columns["Name"].FillWeight = 27; ; dgw.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns["DisplayName"].FillWeight = 39; ; dgw.Columns["DisplayName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns["DisplayVersion"].FillWeight = 27; ; dgw.Columns["DisplayVersion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns["Name2"].FillWeight = 27; ; dgw.Columns["Name2"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns["DisplayName2"].FillWeight = 39; ; dgw.Columns["DisplayName2"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns["DisplayVersion2"].FillWeight = 27; ; dgw.Columns["DisplayVersion2"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


        }
        private int FillGridDelta2(string Dir, Dati2 riga)
        {
            DataGridView dgw = dgvDelta;


            dgw.Rows.Add();
            dgw.Rows[dgw.RowCount - 1].Cells["Quale"].Value = Dir;
            dgw.Rows[dgw.RowCount - 1].Cells["Tipo"].Value = riga.Tipo;
            dgw.Rows[dgw.RowCount - 1].Cells["Name"].Value = riga.Guid;
            dgw.Rows[dgw.RowCount - 1].Cells["DisplayName"].Value = riga.DisplayName;
            dgw.Rows[dgw.RowCount - 1].Cells["DisplayVersion"].Value = riga.DisplayVersion;
            dgw.Rows[dgw.RowCount - 1].Cells["Name2"].Value = riga.Guid2;
            dgw.Rows[dgw.RowCount - 1].Cells["DisplayName2"].Value = riga.DisplayName2;
            dgw.Rows[dgw.RowCount - 1].Cells["DisplayVersion2"].Value = riga.DisplayVersion2;
            return dgw.RowCount - 1;

        }

        private void TestaLista()
        {
            int Diver = 0; //0 non trovato 1 trovato ver diff 2 trovato ok
            SetGridDelta();
            foreach (Dati riga in ListaDati)
            {
                Dati2 d = new Dati2();
                Diver = 0;
                // Attenzione cerco in ListaDatiC il corrispondente
                foreach (Dati rigaC in ListaDatiC)
                {

                    // if (riga.DisplayName == rigaC.DisplayName)
                    if ((riga.GUID == rigaC.GUID) && (riga.DisplayName == rigaC.DisplayName))
                    {
                        string DisplayVersionNorm = (riga.DisplayVersion != null ? riga.DisplayVersion : "<null>");
                        string DisplayVersionCNorm = (rigaC.DisplayVersion != null ? rigaC.DisplayVersion : "<null>");
                        if (DisplayVersionCNorm.ToUpperInvariant() != DisplayVersionNorm.ToUpperInvariant())
                        {
                            //Diversa ver
                            d.Tipo = rigaC.Tipo;
                            d.Guid2 = rigaC.GUID;
                            d.DisplayName2 = rigaC.DisplayName;
                            d.DisplayVersion2 = rigaC.DisplayVersion;
                            Diver = 1;

                        }
                        else
                            Diver = 2;
                        break;
                    }
                }
                if (Diver < 2)
                {
                    d.Tipo = riga.Tipo;
                    d.Guid = riga.GUID;
                    d.DisplayName = riga.DisplayName;
                    d.DisplayVersion = riga.DisplayVersion;
                    Diver = 1;
                    FillGridDelta2("", d);
                }
            }
            foreach (Dati riga in ListaDatiC)
            {
                Dati2 d = new Dati2();
                Diver = 0;
                // Attenzione cerco in ListaDatiC il corrispondente
                // Attenzione cerco in ListaDatiC il corrispondente
                foreach (Dati rigaC in ListaDati)
                {
                    if (riga.DisplayName == rigaC.DisplayName)
                    {
                        if (rigaC.DisplayVersion != riga.DisplayVersion)
                        {                        
                            Diver = 1;
                        }
                        else
                            Diver = 2;
                        break;
                    }
                }
                if (Diver == 0)
                {
                    d.Tipo = riga.Tipo;
                    d.Guid2 = riga.GUID;
                    d.DisplayName2 = riga.DisplayName;
                    d.DisplayVersion2 = riga.DisplayVersion;
                    FillGridDelta2("", d);
                    Diver = 1;
                }
            }

            for (int i = 0; i < dgvDelta.Rows.Count; i++)
            {
                DataGridViewRow item = dgvDelta.Rows[i];
                string DisplayName = (item.Cells[3].Value != null ? item.Cells[3].Value.ToString() : ""); ;
                string DisplayName2 = (item.Cells[6].Value != null ? item.Cells[6].Value.ToString() : ""); ;

                if (DisplayName != "" && DisplayName2 != "")
                {
                    TDiff++;
                    item.Cells[0].Style.BackColor = Color.Silver;
                }
                else if (DisplayName != "")
                {
                    TSKit++;
                    item.Cells[0].Style.BackColor = Color.Red;
                }
                else if (DisplayName2 != "")
                {
                    TSCam++;
                    item.Cells[0].Style.BackColor = Color.Green;
                }
            }
            StringBuilder s = new StringBuilder();
            s.Append("Ver. Diff:");
            s.Append(TDiff);
            s.AppendLine();
            s.Append(" Solo Kit:");
            s.Append(TSKit);
            s.AppendLine();
            s.Append("Solo Cam.:");
            s.Append(TSCam);
            dgw.Text = s.ToString();
            tabControl1.SelectedTab = tabPage3;
        }
        static void OpenExcel(string file)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "EXCEL.EXE";
            startInfo.Arguments = file;

            try
            {
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {

                log.Error("errore su start excel", ex);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button4_Click_1(object sender, EventArgs e)
        {
            ListaDati = PileSwLib.PileSwLib.GenerateList(MenuEsclListaAct.Checked, escludiTestCestinoToolStripMenuItem.Checked);
            FillGrid1(dgw, ListaDati);
            FillGrid1(dgw, ListaDati);
            tabControl1.SelectedTab = tabPage1;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ReadFile();
            tabControl1.SelectedTab = tabPage2;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            TestaLista();

        }
        private void Form1_Load(object sender, EventArgs e)
        {

            InfoLib AppListLibInf = new InfoLib();
            this.Text += " - JSON:" + Parameter["Versione"] + " - SW:" + GetVer(Assembly.GetExecutingAssembly().GetName())+" - ";
            if (FileDaAprire != "")
            {
                ReadFile(FileDaAprire);
                tabControl1.SelectedTab = tabPage2;
                FileDaAprire = "";
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            AboutBox frmAbout = new AboutBox();
            frmAbout.ShowDialog(this);


        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Alt == true && e.Control == true && e.KeyCode.ToString() == "S")
                toolStripButton1.Enabled = true;

        }

        private void button7_Click(object sender, EventArgs e)
        {
            SaveExcel();
        }

        private void listaHWToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = PileSwLib.PileSwLib.GetHWList();
            FRMShowTxt frm = new FRMShowTxt("Lista HW", s);
            frm.ShowDialog(this);
        }

        private void patchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = PileSwLib.PileSwLib.GetHotFix();
            FRMShowTxt frm = new FRMShowTxt("Hot Fix", s);
            frm.ShowDialog(this);
        }

        private void listaServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {

            List<PileSwLib.UtilTest.Servizi.servizio> servizi = PileSwLib.UtilTest.Servizi.GetService();
            frmServizi frm = new frmServizi(servizi);

            frm.ShowDialog(this);
        }
        private void eseguiWMIQueryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s2 = "";
            frmInputBox InputBox = new frmInputBox("Inserisci Query WMI:", "Input query WMI");
            if (InputBox.ShowDialog(this) == DialogResult.Cancel)
                return;
            s2 = InputBox.txtInput;
            string s = PileSwLib.PileSwLib.GetCustom(s2);
            FRMShowTxt frm = new FRMShowTxt("Personalizzata", s);
            frm.ShowDialog(this);

        }
        private void toolStrip1_Resize(object sender, EventArgs e)
        {
            Delta.Width = toolStrip1.Width - toolStripButton1.Bounds.Right - 8;
            Point j = new Point(toolStripButton1.Bounds.Right + 4, Delta.Bounds.Location.Y);

        }

        private void copiaDaAttualeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Dati> l = new List<Dati>();
            if (dgw.SelectedRows == null)
                return;
            foreach (DataGridViewRow item in dgw.SelectedRows)
            {
                try
                {
                    Dati iDatiObj = new Dati();

                    iDatiObj.Tipo = item.Cells["Tipo"].Value.ToString()[0];
                    iDatiObj.DisplayName = (string)item.Cells["DisplayName"].Value ?? " ";
                    iDatiObj.GUID = (string)item.Cells["Name"].Value ?? " ";
                    iDatiObj.DisplayVersion = (string)item.Cells["DisplayVersion"].Value ?? " ";
                    l.Add(iDatiObj);

                }
                catch (Exception ex)
                {

                    log.Error(ex);
                }


            }

            copy(l, true);
        }

        private void copiaDeltaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Dati> l = new List<Dati>();
            int r, rmax, rmin;
            rmin = dgvDelta.Rows.Count - 1;
            rmax = 0;
            foreach (DataGridViewCell item in dgvDelta.SelectedCells)
            {

                r = item.RowIndex;
                if (rmin > r)
                    rmin = r;
                if (rmax < r)
                    rmax = r;
            }
            for (int i = rmin; i <= rmax; i++)
            {

                DataGridViewRow item = dgvDelta.Rows[i];

                Dati iDatiObj = new Dati();


                iDatiObj.Tipo = item.Cells[1].Value.ToString()[0];
                iDatiObj.DisplayName = (string)item.Cells[2].Value ?? " ";
                iDatiObj.GUID = item.Cells[3].Value.ToString();
                iDatiObj.DisplayVersion = item.Cells[4].Value.ToString();
                if (iDatiObj.Tipo != 0)
                {
                    l.Add(iDatiObj);
                }

            }
            copy(l, false);
        }
        private void copy(List<Dati> Lista, bool revers)
        {
            string s = "";

            try
            {

                if (revers)
                    Lista.Reverse(); // gira la lista
                s = PileSwLib.PileSwLib.WriteToString(Lista);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ;
            }
            Clipboard.SetText(s);
            // string s = SwPileUtil.WriteToString(ListaDati);
            //  List<Dati> l = SwPileUtil.ReadFromString(s);
        }

        private void listaCertificatiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCertificati certificati = new frmCertificati();
            certificati.ShowDialog(this);
        }

        private void listaRegistriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRegs registro = new frmRegs(log);
            registro.ShowDialog(this);
        }

        private void f1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, fileChm);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Pulisci il file temp
            try
            {

                File.Delete(fileChm);
            }
            catch (IOException ex1)
            {
                e.Cancel = true;
                log.Error(ex1);
            }
            catch (Exception ex)
            {
                log.Error(ex);


            }
        }

        private void visualizzaCFGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmConfigAct formCfcAct;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("------ Parametri -------");
            sb.AppendLine(Parameter.ToString());
            sb.AppendLine("---- Configurazione ----");
            sb.AppendLine(PileSwLib.PileSwLib.ToString());

            formCfcAct = new frmConfigAct(sb.ToString());
            formCfcAct.ShowDialog(this);
        }

        private void dgw_Paint(object sender, PaintEventArgs e)
        {
            if (dgw.Rows.Count < 1)
                return;
            Dictionary<char, Color> TtoC = ETipo.TipoCol();
            foreach (DataGridViewRow item in this.dgw.Rows)
            {
                char t = (char)item.Cells["Tipo"].Value;
                if (TtoC.ContainsKey(t) && TtoC[t] != Color.Transparent)
                {
                    item.Cells["Tipo"].Style.BackColor = TtoC[t];
                }
            }
        }

        private void dgw2_Paint(object sender, PaintEventArgs e)
        {
            if (dgw2.Rows.Count < 1)
                return;
            Dictionary<char, Color> TtoC = ETipo.TipoCol();
            foreach (DataGridViewRow item in this.dgw2.Rows)
            {
                char t = (char)item.Cells["Tipo"].Value;
                if (TtoC.ContainsKey(t) && TtoC[t] != Color.Transparent)
                {
                    item.Cells["Tipo"].Style.BackColor = TtoC[t];
                }
            }
        }

        private void dgvDelta_Paint(object sender, PaintEventArgs e)
        {
            if (dgvDelta.Rows.Count < 1)
                return;
            Dictionary<char, Color> TtoC = ETipo.TipoCol();
            foreach (DataGridViewRow item in this.dgvDelta.Rows)
            {
                char t = (char)item.Cells["Tipo"].Value;
                if (TtoC.ContainsKey(t) && TtoC[t] != Color.Transparent)
                {
                    item.Cells["Tipo"].Style.BackColor = TtoC[t];
                }
            }
        }

        private void dgw2_DragDrop(object sender, DragEventArgs e)
        {

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            ReadFile(files[0]);
            tabControl1.SelectedTab = tabPage2;
            FileDaAprire = "";
        }

        private void dgw2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void testNomePCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCheckName frmname = new frmCheckName(NomiRegEx);
            frmname.ShowDialog(this);
        }
        int prevColIndex;
        ListSortDirection prevSortDirection;
        private void dgw__CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 && e.ColumnIndex >= 0)
            {
                ListSortDirection direction = ListSortDirection.Ascending;
                if (e.ColumnIndex == prevColIndex) // reverse sort order
                    direction = prevSortDirection == ListSortDirection.Descending ? ListSortDirection.Ascending : ListSortDirection.Descending;

                // remember the column that was clicked and in which direction is ordered
                prevColIndex = e.ColumnIndex;
                prevSortDirection = direction;
                // set the column to be grouped
                dgw.GroupTemplate.Column = dgw.Columns[e.ColumnIndex];
                dgw.Sort(dgw.Columns[e.ColumnIndex], direction);
            }
            else
            {
               //
            }
        }

        private void dgw__Paint(object sender, PaintEventArgs e)
        {
            
            if (dgw.Rows.Count < 1)
                return;
            
            Dictionary<char, Color> TtoC = ETipo.TipoCol();
            foreach (DataGridViewRow item in this.dgw.Rows)
            {
                char t = (char)item.Cells["Tipo"].Value;
                if (TtoC.ContainsKey(t) && TtoC[t] != Color.Transparent)
                {
                    item.Cells["Tipo"].Style.BackColor = TtoC[t];
                }
            }
            
        }



    }
}
