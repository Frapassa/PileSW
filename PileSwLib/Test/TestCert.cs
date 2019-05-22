using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PileSwLib.UtilTest;
using PileSwLib.Tipi;
namespace PileSwLib.Test
{
    public static class TestCert
    {

        public static void GetSrv(List<CTipoCfgCrt> TestCert, List<Dati> ListaDati)
        {
            foreach (var test in TestCert)
            {
              
                if (GestCertificate.FindCertificate(test.StoreName,System.Security.Cryptography.X509Certificates.StoreLocation.LocalMachine,test.PropQuale,test.PropValore))
                    AggiungiCert(test.StoreName, test.PropValore, test.PropQuale, test.Tipo, ListaDati);
            }
        }
        private static void AggiungiCert(string StoreName, string PropValore, string PropQuale, char tipo, List<Dati> ListaDati)
        {
            Dati riga = new Dati();
            riga.Tipo = tipo;
            riga.DisplayName = PropValore;
            riga.DisplayVersion = PropQuale;
            riga.GUID = StoreName;
            ListaDati.Add(riga);
        }
    }
}
