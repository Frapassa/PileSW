using Microsoft.Win32;
using PileSwLib.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PileSwLib
{
    public class UtilRegistry
    {
        // Funzione principlae che sostuisce i pezzi speciali del reg con quelli reali
       public static string SostSpecialReg(string reg)
        {
            string ret = reg;
           //Se è senza caratteri speciali esci
            if (!reg.Contains('%'))
                return ret;
            string[] pezzo = reg.Split('%');
           // Per ora 1 solo pezzo, quindi  speciale + stringa normale
         
           if (pezzo.Count()!=3)
               return ret;
           StringBuilder sb = new StringBuilder();
          
           pezzo[1] = pezzo[1].ToLowerInvariant().ToString();
            switch (pezzo[1])
            {
                case "java6":
                    sb.Append(f_java6());
                    break;
                default:
                    sb.Append( "?" + pezzo[0] + "?");
                    break;
            }
            sb.Append(pezzo[2]);
            ret = sb.ToString();
            return ret;
        }
        //Funzioni private che calcolano il vero reg
       static string f_java6()
       {
           RegistryKey keyb;
           string ret = "NoKey1";
           string baseKey, keys, NSubVal;
           string chiave = @"HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Runtime Environment";
           keyb = TestRegistri.BaseKey(chiave, out baseKey, out keys, out  NSubVal);
           if (keyb == null)
           {
               chiave = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\JavaSoft\Java Runtime Environment";
               keyb = TestRegistri.BaseKey(chiave, out baseKey, out keys, out  NSubVal);
           }
           if (keyb != null)
           {
               string k3 = "";
               if (keyb.GetValue("Java6FamilyVersion") != null)
                   k3 = keyb.GetValue("Java6FamilyVersion").ToString();
               else
                   return "NoKey2";
              // k3 = k3 + @"\MSI";
              // RegistryKey keyj = keyb.OpenSubKey(k3);
               ret = chiave+"\\" + k3 ;

           }
           return ret;

       }
    }
}
