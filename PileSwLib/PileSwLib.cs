using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using log4net;
using System.Management;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PileSwLib.Test;
using PileSwLib.Tipi;
using PileSwLib.UtilTest;

namespace PileSwLib
{
    /// <summary>
    /// Classe base per gestire le pile SW.
    /// </summary>
    public class PileSwLib
    {
        private static bool useWMI = true;

        private static List<CTipoCfgSrv> TestCfgSrvs = new List<CTipoCfgSrv>();
        private static List<CTipoCfgReg> TestCfgRegs = new List<CTipoCfgReg>();
        private static List<CTipoCfgCrt> TestCfgCrts = new List<CTipoCfgCrt>();
        private static List<CTipoCfgFile> TestCfgFiles = new List<CTipoCfgFile>();
        private static List<CTipoCfgWMIQ> TestCfgWMIQ = new List<CTipoCfgWMIQ>();
        
        
        public static ILog log;
        /// <summary>
        ///  Setta il log per scrivere le info in caso di errori o debug
        /// </summary>
        /// <param name="Log">Oggetto Log di Log4Net</param>
        public static void SetLog(ILog Log)
        {
            log = Log;
        }
        public static bool GetProperties { get { return false; } }

        /// <summary>
        /// Ritorna la lista dei registri che devono essere controllati
        /// </summary>
        public static List<CTipoCfgReg> GetTestCfgRegs { get { return TestCfgRegs; } }
        /// <summary>
        /// Carica le tre stringhe salvate nei parametri e trasformale nelle 3 liste corrispondenti
        /// </summary>
        /// <param name="_SettingsJSON">Stringa dei cfg</param>

        public static void SetServiceConfig(string _SettingsJSON)
        {
            string s = _SettingsJSON;// _SettingsJSON.Replace(@"\", @"\\"); //Commentati perchè avviene nella classe SwPileCfg che carica il file
            string r = _SettingsJSON;// _SettingsJSON.Replace(@"\", @"\\");
            string c = _SettingsJSON;// _SettingsJSON.Replace(@"\", @"\\");

            try
            {
                ETipo.Tipi();
                TestCfgSrvs = JsonConvert.DeserializeObject<List<CTipoCfgSrv>>(s);
                TestCfgSrvs.RemoveAll(x=>(x.Tipo!=ETipo.Service.ch));
                TestCfgRegs = JsonConvert.DeserializeObject<List<CTipoCfgReg>>(r);
                TestCfgRegs.RemoveAll(x => (x.Tipo != ETipo.Registro.ch));
                TestCfgCrts = JsonConvert.DeserializeObject<List<CTipoCfgCrt>>(c);
                TestCfgCrts.RemoveAll(x => (x.Tipo != ETipo.Certificati.ch));
                TestCfgFiles = JsonConvert.DeserializeObject<List<CTipoCfgFile>>(c);
                TestCfgFiles.RemoveAll(x => (x.Tipo != ETipo.FileEx.ch));
                TestCfgWMIQ = JsonConvert.DeserializeObject<List<CTipoCfgWMIQ>>(c);
                TestCfgWMIQ.RemoveAll(x => (x.Tipo != ETipo.WMIQuery.ch));
               
                
                //Sostituzione dei valori speciali nei registri
                for (int i = 0; i < TestCfgRegs.Count; i++)
			    {
			         CTipoCfgReg item=TestCfgRegs[i];
                     item.Key = UtilRegistry.SostSpecialReg(item.Key);
			    }
              
            }
            catch (Exception Ex)
            {
                log.Error(Ex);

            }

        }

         public   static string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in TestCfgSrvs)
	        {
                sb.Append(item.ToString());
	        }
            foreach (var item in TestCfgRegs)
            {
                sb.Append(item.ToString());
            }
            foreach (var item in TestCfgCrts)
            {
                sb.Append(item.ToString());
            }
            foreach (var item in TestCfgFiles)
            {
                sb.Append(item.ToString());
            }
            foreach (var item in TestCfgWMIQ)
            {
                sb.Append(item.ToString());
            }
            return sb.ToString();
        }
   
        /// <summary>
        /// Se true viene usate le query WMI per scopire il SW installato non usare.
        /// </summary>
        public static bool UseWMI { get { return useWMI; } set { useWMI = value; } }
        private static string Utf16ToUtf8(string utf16String)
        {
            // Get UTF16 bytes and convert UTF16 bytes to UTF8 bytes
            byte[] utf16Bytes = Encoding.Unicode.GetBytes(utf16String);
            byte[] utf8Bytes = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, utf16Bytes);

            // Return UTF8 bytes as ANSI string
            return Encoding.Default.GetString(utf8Bytes);
        }
        private static string Utf16ToAscii(string utf16String)
        {
            // Get UTF16 bytes and convert UTF16 bytes to UTF8 bytes
            byte[] utf16Bytes = Encoding.Unicode.GetBytes(utf16String);
            byte[] utf8Bytes = Encoding.Convert(Encoding.Unicode, Encoding.ASCII, utf16Bytes);

            // Return UTF8 bytes as ANSI string
            return Encoding.Default.GetString(utf8Bytes);
        }
        /// <summary>
        /// Leggi la configurazione del computer e genera la corrispondente lista
        /// inserendo programmi e cestino
        /// </summary>
        /// <returns>La lista attuake</returns>
        public static List<Dati> GenerateList()
        {
            return GenerateListReg(false, false);
        }
        /// <summary>
        /// Leggi la configurazione del computer e genera la corrispondente lista
        /// </summary>
        /// <param name="EsclProg">Se true escludi la rivelazione dei programmi</param>
        /// <param name="EsclCestino">Se true escludi la rivelazione del cestino</param>
        /// <returns>La lista attuale</returns>
        public static List<Dati> GenerateList(bool EsclProg, bool EsclCestino)
        {

            /*      if (UseWMI)
                      return GenerateListNew(log);
                  else*/
            return GenerateListReg(EsclProg, EsclCestino);

        }
        public delegate void StatusUpdateHandler(ProgressEventArgs e);
        public static event StatusUpdateHandler OnUpdatePosition;
        private static void UpdateStatus(string msg)
        {

            // Make sure someone is listening to event
            if (OnUpdatePosition == null) return;

            ProgressEventArgs args = new ProgressEventArgs(msg);
            OnUpdatePosition(args);
        }

        private static string CreateListVal(string Query)
        {
            ManagementObjectSearcher mosd = new ManagementObjectSearcher(Query);
            ManagementObjectCollection MOCD = mosd.Get();

            StringBuilder sss = new StringBuilder();
            int i = 0;
            foreach (ManagementObject mo in MOCD)
            {
                i++;

                foreach (var item in mo.Properties)
                {

                    sss.Append(item.Value);
                    sss.AppendLine();

                }

                ;
            }
            return sss.ToString();
        }
        private static string CreateList(string Query)
        {

            ManagementObjectSearcher mosd = new ManagementObjectSearcher(Query);
            ManagementObjectCollection MOCD = mosd.Get();
          
            UpdateStatus("Creating List ...");
            StringBuilder sss = new StringBuilder();
            int i = 0;
            foreach (ManagementObject mo in MOCD)
            {
                i++;
                sss.Append("------------\t-- Item n°");
                sss.Append(Convert.ToString(i).PadLeft(5, '0')); 
                sss.AppendLine(" --\t----------------");
                foreach (var item in mo.Properties)
                {
                    sss.Append(item.Type);
                    sss.Append("\t");
                    sss.Append(item.Name);
                    sss.Append("\t");
                    sss.Append(item.Value);

                    sss.AppendLine();

                }
               
                ;
            }
            UpdateStatus("Created List");
            return sss.ToString();
        }
        public static string GetHotFix()
        {
            return CreateList("SELECT * FROM Win32_QuickFixEngineering");
        }
        public static string GetHWList()
        {
            return CreateList("SELECT * FROM CIM_LogicalDevice");

        }
     //   internal static string WMI_Query(string query)
        internal static string GetOnlyVal(string Query)
        {
            return CreateListVal(Query);

        }
        public static string GetCustom(string Query)
        {
            return CreateList(Query);

        }
        private static List<Dati> GenerateListReg(bool EsclProgrammi, bool EsclCestino)
        {
            List<Dati> ListaDati = new List<Dati>();
            StringBuilder s2 = new StringBuilder();
            ListaDati.Clear();
            if (!EsclProgrammi)
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\MICROSOFT\WINDOWS\CurrentVersion\Uninstall");
                if (key != null) //alcune macchine salvano qui
                {
                    GetDatiFromReg(key, ListaDati, s2);
                    log.Debug("C1 " + ListaDati.Count);
                }

                key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\MICROSOFT\WINDOWS\CurrentVersion\Uninstall");
                if (key != null) //alcune macchine salvano qui
                {
                    GetDatiFromReg(key, ListaDati, s2);
                    log.Debug("C2 " + ListaDati.Count);

                }

                key = Registry.LocalMachine.OpenSubKey(@"HKU\USER-SID-HERE\Software\Microsoft\Windows\CurrentVersion\Uninstall");
                if (key != null) //alcune macchine salvano qui
                {
                    GetDatiFromReg(key, ListaDati, s2);
                    log.Debug("C3 " + ListaDati.Count);
                }
                if (key != null)
                    key.Close();
            }
            GetDatiExt(ListaDati, EsclCestino);
            return ListaDati;

        }
        private static void GetDatiFromReg(RegistryKey key, List<Dati> ListaDati, StringBuilder s2)
        {
            string[] j = key.GetSubKeyNames();
            //Console.WriteLine(key.GetValue("ProductID").ToString());

            Dati riga = new Dati();
            foreach (string item in j)
            {

                RegistryKey h = key.OpenSubKey(item);
                if (h == null)
                    continue;
                // string[] F = h.GetValueNames();
                var n = h.GetValue("DisplayName");
                var v = h.GetValue("DisplayVersion");
                if (n != null)
                {

                    if (!UpgradeOffice(item, (string)n))
                    {
                        riga = new Dati();
                        if (n != null)
                            riga.DisplayName = Utf16ToUtf8((string)n).Normalize();
                        else
                            s2.Append("\t");
                        if (v != null)
                            riga.DisplayVersion = ((string)v).Normalize();
                        else
                            s2.Append("\t");
                        riga.GUID = item.Normalize();
                        riga.Tipo = 'P';
                        ListaDati.Add(riga);
                    }
                    h.Close();
                }
            }

        }
        private static bool UpgradeOffice(string item, string displayname)
        {
            bool ret = false;

            int n = item.IndexOf("}_", 0);
            int n1 = item.IndexOf("_{", 0);
            if (n > -1 && n1 > -1)
                ret = true;
            return ret;
        }

        public static void GetDatiExt(List<Dati> ListaDati, bool EsclCestino)
        {

            //Importa i dati che non sono programmi e sono fissi
            importDati.GetDatiKit('K', ListaDati);
            importDati.GetDatiPatchSO('B', ListaDati);
            importDati.GetDatiPatchOffice('B', ListaDati);
            importDati.GetDatiPatchCA('B', ListaDati);
            importDati.GetDatiAVEngine('V', ListaDati);
            importDati.GetDatiUtentiDir('U', ListaDati);
            // 
            //Alimentazione
            TestPwrMng.GetPwr('A', ListaDati);
            //Altro Registro e Servizi
            if (TestCfgSrvs != null)
                TestServizi.GetSrv(TestCfgSrvs, ListaDati);
            if (TestCfgRegs != null)
                TestRegistri.GetReg(TestCfgRegs, ListaDati);
            if (TestCfgCrts != null)
                TestCert.GetSrv(TestCfgCrts, ListaDati);
            if (TestCfgFiles != null)
                TestFile.GetFileEx(TestCfgFiles, ListaDati);
            if (TestCfgWMIQ != null)
                TestWMIQuery.GetWMIQuery(TestCfgWMIQ, ListaDati);
            if (!EsclCestino)
                TestAltri.GetCestino(ListaDati);
        }

        private static List<Dati> ReadFileOld(string filename)
        {
            List<Dati> ListaDatiC = new List<Dati>();
            XmlSerializer ser = new XmlSerializer(typeof(List<Dati>));
            TextReader reader = new StreamReader(filename);
            ListaDatiC = (List<Dati>)ser.Deserialize(reader);
            reader.Close();
            return ListaDatiC;
        }
        public static List<Dati> ReadFile(string filename)
        {
            List<Dati> ListaDatiC = new List<Dati>();
            FileDati FileD = new FileDati();
            try
            {

                XmlSerializer ser = new XmlSerializer(typeof(FileDati));
                TextReader reader = new StreamReader(filename);
                FileD = (FileDati)ser.Deserialize(reader);
                reader.Close();
                ListaDatiC = FileD.Items;
            }
            catch (InvalidOperationException Ex1)
            {
                ListaDatiC = ReadFileOld(filename);
                log.Error(Ex1);
            }
            catch (Exception Ex)
            {

                log.Error(Ex);


            }

            return ListaDatiC;
        }
        public static void WriteFile(string filename, List<Dati> ListaDati)
        {
            XmlSerializer ser = new XmlSerializer(typeof(FileDati));
            FileDati Dati = new FileDati(ListaDati, filename);
            TextWriter writer = new StreamWriter(filename);
            ser.Serialize(writer, Dati);
            writer.Close();
        }
        public static string WriteToString(List<Dati> ListaDati)
        {
            StringWriter writer = new StringWriter();
            XmlSerializer ser = new XmlSerializer(typeof(FileDati));
            FileDati Dati = new FileDati(ListaDati, "<mem>");

            ser.Serialize(writer, Dati);

            writer.Close();

            string xmlData = writer.ToString();


            return xmlData;
        }
        public static List<Dati> ReadFromString(string dati)
        {
            List<Dati> ListaDatiC = new List<Dati>();
            FileDati FileD = new FileDati();

            XmlSerializer ser = new XmlSerializer(typeof(FileDati));
            StringReader reader = new StringReader(dati);
            FileD = (FileDati)ser.Deserialize(reader);
            reader.Close();
            ListaDatiC = FileD.Items;
            return ListaDatiC;
        }
    }
    public class ProgressEventArgs : EventArgs
    {
        private string _status;

        public string Status { get { return _status; } }

        public ProgressEventArgs(string status)
        {
            _status = status;
        }
    }
}
