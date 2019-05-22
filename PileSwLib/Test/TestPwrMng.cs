
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PileSwLib.Tipi;
using PileSwLib.UtilTest;
namespace PileSwLib.Test
{
    public static class TestPwrMng
    {
   


        public static void GetPwr(char tipo, List<Dati> ListaDati)
        {
            Dictionary<string, uint> pd = TPwrMng.GetActPwrCfg();

            foreach (var item in pd)
            {
                AggiungiPwr(item.Key, item.Value, tipo, ListaDati);
            }
          /*  AggiungiPwr("Time Hybernation", secHyber, tipo, ListaDati);
            AggiungiPwr("Monitor Standby", secMonitorStandBy, tipo, ListaDati);
            AggiungiPwr("Disk Standby", secDiskStandBy, tipo, ListaDati);
            AggiungiPwr("Standby", secIdleTimeout, tipo, ListaDati);
            AggiungiPwr("Dim illuminat.", secDimTimeout, tipo, ListaDati);*/
        }
        private static void AggiungiPwr(string Prop, uint Sec, char tipo, List<Dati> ListaDati)
        {
            Dati riga = new Dati();
            riga.Tipo = tipo;
            riga.DisplayName = Convert.ToString(Sec);
            riga.DisplayVersion ="secondi" ;
            riga.GUID = Prop;
            ListaDati.Add(riga);
        }
    }
}
