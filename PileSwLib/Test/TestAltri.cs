
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PileSwLib.UtilTest;
using PileSwLib.Tipi;

namespace PileSwLib.Test
{
    public class TestAltri
    {
        public static void GetCestino(List<Dati> ListaDati)
        {

            
            Dati riga = new Dati();
            riga.Tipo = 'X';
            
            riga.DisplayVersion = "-";
            riga.GUID = "Cestino";
            int itemsCount= Altro.GetCestino();
            if ((itemsCount ) > 0)
                riga.DisplayName = "Cestino con " + Convert.ToString(itemsCount) + " oggetti";
            else
                riga.DisplayName ="Cestino vuoto";; 
            ListaDati.Add(riga);
        }
    }
}
