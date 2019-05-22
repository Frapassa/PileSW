using PileSwLib.Tipi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace PileSwLib.Test
{
    public static class TestFile
    {
        public static void GetFileEx(List<CTipoCfgFile> TestFile, List<Dati> ListaDati)
        {
            foreach (CTipoCfgFile item in TestFile)
            {
                string edir = (item.IsFile ? "File" : "Cartella");
                string ret="?";
                bool esiste = false;
                if (item.IsFile)
                {
                    esiste = File.Exists(item.Path);
                    ret = (esiste ? "Esiste" : "Non Esiste");
                }
                else
                {
                    esiste = Directory.Exists(item.Path);
                    if (esiste)
                    {
                        if (IsDirectoryEmpty(item.Path))
                            ret="Esiste, vuota";
                        else
                            ret="Esiste, non vuota";
                    }
                    else
                        ret = "Non Esiste";
                }
                AggiungiFileEx(item.Path, ret, edir, 'F', ListaDati);
            }
        }
        private static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }
        private static void AggiungiFileEx(string Path, string Esiste, string EDir, char tipo, List<Dati> ListaDati)
        {
            Dati riga = new Dati();
            riga.Tipo = tipo;
            riga.DisplayName = Path;
            riga.DisplayVersion = Esiste;
            riga.GUID = EDir;
            ListaDati.Add(riga);
        }
    }
}
