using PileSwLib.Tipi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PileSwLib;
namespace PileSwLib.Test
{
    class TestWMIQuery
    {
        public static void GetWMIQuery(List<CTipoCfgWMIQ> TestWMIQ, List<Dati> ListaDati)
        {
            string result;
            foreach (CTipoCfgWMIQ item in TestWMIQ)
            {
                // item.Result=SwPileUtil.GetCustom(item.Query);
          
                result = PileSwLib.GetOnlyVal(item.Query);
                AggiungiWMQU(item.Nome, item.Query, result, 'W', ListaDati);
            }
        }

        private static void AggiungiWMQU(string Name, string Query, string Result, char tipo, List<Dati> ListaDati)
        {
            Dati riga = new Dati();
            
            riga.Tipo = tipo;
            riga.DisplayName = Query;
            riga.DisplayVersion = Result;
            riga.GUID = Name;
            ListaDati.Add(riga);
        }
    }
}
