using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PileSwLib.UtilTest;
using PileSwLib.Tipi;
namespace PileSwLib.Test
{
    public static class TestServizi
    {

        public static void GetSrv(List<CTipoCfgSrv> TestSrvs, List<Dati> ListaDati) 
        {
            foreach (var test in TestSrvs)
            {
                System.ServiceProcess.ServiceControllerStatus Stato;
                string DisplayName;
                if (Servizi.DoesServiceExist(test.Nome, out DisplayName, out Stato))
                {
                    AggiungiServ(DisplayName, test.Nome, Stato.ToString(), test.Tipo, ListaDati);
                }
                
            }
        }
    

 
        private static void AggiungiServ(string DisplayName, string Name, string Status, char tipo, List<Dati> ListaDati)
        {
            Dati riga = new Dati();
            riga.Tipo = tipo;
            riga.DisplayName = DisplayName;
            riga.DisplayVersion = Status;
            riga.GUID =Name;

            ListaDati.Add(riga);
        }
    }


}
