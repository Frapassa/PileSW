using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace PileSwLib.Tipi
{
    public class Dati
    {

        public string GUID;
        public string DisplayName;
        public string DisplayVersion;
        public char Tipo;
        public string Note;
        /*
        public UInt16 AssignmentType;
        public String Caption;
        public String Description;
        public String HelpLink;
        public String HelpTelephone;
     
        public String InstallDate;
        public DateTime InstallDate2;
        public String InstallLocation;
        public String InstallSource;
        public Int16 InstallState;
        public String Language;
        public String LocalPackage;
        public String PackageCache;
        public String PackageCode;
        public String PackageName;
        public String ProductID;
        public String RegCompany;
        public String RegOwner;
        public String SKUNumber;
        public String Transforms;
        public String URLInfoAbout;
        public String URLUpdateInfo;
        public String Vendor;
        public UInt32 WordCount;

        */

        public bool Ok = true;
        public bool OkV = true;
    }
    public class Dati2
    {

        public string Guid;
        public string DisplayName;
        public string DisplayVersion;
        public string Guid2;
        public string DisplayName2;
        public string DisplayVersion2;
        public char Tipo;

    }
     [Serializable]
    public class FileDati
    {
        private  List<Dati> items = new List<Dati>();
        private string versione;
        private DateTime data;
        private string file;

        [XmlElement("Versione")]
        public string Versione { get { return versione; } set { versione = value; } }

        [XmlElement("Data")]
        public DateTime Data { get { return data; } set { data=value; } }
        [XmlElement("File")]
        public string File { get { return file; } set { file=value; } }  
     //  [XmlElement("Dati")]
         [XmlArray("ListaDati")]
        public List<Dati> Items { get { return items; } }



        public FileDati()
        {
            this.items = new List<Dati>();
             versione = "";
            data = DateTime.Now;
            file = "";
        }
        public FileDati(List<Dati> listaDati,string fileName)
        {
            this.items = listaDati;
            versione = "1.01";
            data = DateTime.Now;
            file = fileName;
        }
    }
}
