using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PileSwLib;
using PileSwLib.Tipi;
using System.IO;
namespace PileSwLib
{
    public static class importDati
    {       //---------------- Metodi Pubblici ---------------------------------------
          public static void GetDatiKit(char Codice ,List<Dati> ListaDati)
        {
            var n = new object();
            string sh = "";
            string s = "ddd"; string s1 = "ddd";
            StringBuilder sb = new StringBuilder();
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\wOW6432nODE\SWPosteItaliane\KIT");
            bool bit32 = false;
            if (key == null)
            {
                key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\SWPosteItaliane\KIT");
                bit32 = true;
            }

            if (key != null)
            {
                n = null;
                if (bit32 == true)
                    sh = "32 BIT";
                else
                    sh = "64 BIT";
                sb.AppendLine("-- " + sh + " Key --");


                s = "DATAINST"; sb.Append(s + " = "); n = key.GetValue(s); sb.Append(n); sb.AppendLine(); Aggiungi("KIT POSTE 1 " + sh, s, n, Codice, ListaDati); n = null; //k

                s = "NOMEKIT"; sb.Append(s + " = "); n = key.GetValue(s); sb.Append(n); sb.AppendLine(); Aggiungi("KIT POSTE 2 " + sh, s, n, Codice, ListaDati); n = null;

                s = "NOMEMACCHINA"; sb.Append(s + " = "); n = key.GetValue(s); sb.Append(n); sb.AppendLine(); Aggiungi("KIT POSTE 3 " + sh, s, n, Codice, ListaDati); n = null;
                s = "VERSIONEKIT"; sb.Append(s + " = "); n = key.GetValue(s); sb.Append(n); sb.AppendLine(); Aggiungi("KIT POSTE 4 " + sh, s, n, Codice, ListaDati); n = null;
                s = "TIPOPDL"; sb.Append(s + " = "); n = key.GetValue(s); sb.Append(n); sb.AppendLine(); Aggiungi("KIT POSTE 5 " + sh, s, n, Codice, ListaDati); n = null;
                s = "WIMKIT"; sb.Append(s + " = "); n = key.GetValue(s); sb.Append(n); sb.AppendLine(); Aggiungi("KIT POSTE 6 " + sh, s, n, Codice, ListaDati); n = null;
                s = "NOTE"; sb.Append(s + " = "); n = key.GetValue(s); sb.Append(n); sb.AppendLine(); Aggiungi("KIT POSTE 7 " + sh, s, n, Codice, ListaDati); n = null;

                key.Close();
                sb.AppendLine();
            }
          
        }
          public static void GetDatiPatchOffice(char Codice, List<Dati> ListaDati)
          {
              var n = new object();
              string sh = "";
              string s = "ddd"; string s1 = "ddd";
              StringBuilder sb = new StringBuilder();      
           
              RegistryKey key  = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\ComputerAssociates\UPMRollupPatch\Office");
              if (key != null)
              {
                  sb.AppendLine("-- ComputerAssociates OFFICE --");
                  string[] j = key.GetSubKeyNames();
                  foreach (string item in j)
                  {
                      s = @"UPMRollupPatch\Office\" + item;

                      sb.AppendLine(s);
                      sb.Append(s);
                      sb.Append("\\Install Date=");
                      RegistryKey h = key.OpenSubKey(item);
                      var g = h.GetValue("Install Date");
                      sb.Append(g);
                      sb.AppendLine();
                    //  Aggiungi("CA OFFICE ", "D " + s, g, Codice, ListaDati);//b
                      sb.Append(s);
                      sb.Append("\\Version=");
                      g = h.GetValue("Version");
                      sb.Append(g);
                      h.Close();
                      sb.AppendLine();
                      Aggiungi("PATCH OFFICE", "V " + s, g, Codice, ListaDati);
                  }
                  key.Close();
                  sb.AppendLine();
              }
   
          }
          public static void GetDatiPatchSO(char Codice, List<Dati> ListaDati)
          {
              var n = new object();
              string sh = "";
              string s = "ddd"; string s1 = "ddd";
              StringBuilder sb = new StringBuilder();

              RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\ComputerAssociates\UPMRollupPatch\OS");
              if (key != null)
              {
                  sb.AppendLine("-- ComputerAssociates OS --");

                  string[] j1 = key.GetSubKeyNames();
                  foreach (string item in j1)
                  {
                      s = @"UPMRollupPatch\OS\" + item;
                      sb.AppendLine(s);
                      sb.Append(s);
                      sb.Append("\\Install Date=");
                      RegistryKey h = key.OpenSubKey(item);
                      var g = h.GetValue("Install Date");
                      sb.Append(g);
                      sb.AppendLine();
                      // Aggiungi("CA O.S.", "D " + s, g, Codice, ListaDati);
                      sb.Append(s);
                      sb.Append("\\Version=");
                      g = h.GetValue("Version");
                      sb.Append(g);
                      Aggiungi("PATCH O.S.", "V " + s, g, Codice, ListaDati); //b
                      h.Close();
                      sb.AppendLine();
                  }
                  //    [HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\McAfee\AVEngine]
                  key.Close();
                  sb.AppendLine();
                
              }

          }
          public static void GetDatiPatchCA(char Codice, List<Dati> ListaDati)
          {
            string nomePath = @"C:\CA\DSM\REPLACED";

            if (!Directory.Exists(nomePath))
                {
                    Aggiungi("PATCH CA", "DSM\\REPLACE ", "Non esiste", Codice, ListaDati); //b
                return;
            }
                   
           var paths = Directory.EnumerateDirectories(nomePath, "*.*", SearchOption.TopDirectoryOnly);
            foreach (var path in paths)
            {

                string[] p = path.Split('\\');
                Aggiungi("PATCH CA", "DSM\\REPLACE " ,p[p.Length-1], Codice,  ListaDati); //b
            }
        }
          public static void GetDatiAVEngine(char Codice, List<Dati> ListaDati)
          {
              var n = new object();
              string sh = "";
              string s = "ddd"; string s1 = "ddd";
              StringBuilder sb = new StringBuilder();

              RegistryKey key =  Registry.LocalMachine.OpenSubKey(@"SOFTWARE\McAfee\AVEngine");
              if (key == null)
                  key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\McAfee\AVEngine");
              if (key != null)
              {
                  sb.AppendLine("-- McAfee Engine --");

                  s = "AVDatDate"; sb.Append(s + " = "); n = key.GetValue(s); sb.Append(n); sb.AppendLine();
                  Aggiungi("McAfee Engine 1 ", s, n, Codice, ListaDati); n = null;
                  s = "AVDatVersion"; sb.Append(s + " = "); n = key.GetValue(s); sb.Append(n); sb.AppendLine(); Aggiungi("McAfee Engine 2 ", s, n, Codice, ListaDati); n = null;
                  sb.AppendLine();
              }
              // return sb.ToString();
          }
          public static void GetDatiUtentiDir(char Codice, List<Dati> ListaDati)
          {
              string s = "ddd"; string s1 = "ddd";
              StringBuilder sb = new StringBuilder();
              var localUsers = new List<string>();
              var k = new DirectoryInfo(@"C:\Users");
         
              foreach (DirectoryInfo item in k.EnumerateDirectories())
              {
                  s = item.Name;
                  if (s != "All Users" &&
                  s != "Public" &&
                  s != "Default User" &&
                  s != "Default")
                      Aggiungi("Utente", s, "", Codice, ListaDati); ;
              }
          }
          //---------------- Metodi Privati ---------------------------------------
          private static void Aggiungi(string Item, object name, object vers, char tipo, List<Dati> ListaDati)
          {
              Dati riga = new Dati();
              StringBuilder s1 = new StringBuilder();
              if (name != null)
                  s1.Append(name);
              else
                  s1.Append("");
              riga.DisplayName = s1.ToString(); s1 = new StringBuilder();
              if (vers != null)
                  s1.Append(vers);
              else
                  s1.Append("");
              riga.DisplayVersion = s1.ToString(); ;
              riga.GUID = Item;
              riga.Tipo = tipo;
              ListaDati.Add(riga);
          }
    }
}
