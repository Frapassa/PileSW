using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Drawing;
namespace PileSwLib.Tipi
{
    public  struct cc
    {
        public char ch;
        public Color co;
        public cc(char ch, Color co)
        {
            this.ch = ch;
            this.co = co;
        }
    }

    public static class ETipo
    {
 
        public static cc Programmi = new cc('P', Color.MediumAquamarine); 
        public static cc BuildVer = new cc('B',Color.Beige); 
        public static cc antiVirus = new cc('V',Color.Orange); 
        public static cc KitPT = new cc('K',Color.Yellow); 
        public static cc Service = new cc('S',Color.Moccasin); 
        public static cc Registro = new cc('R',Color.LightBlue); 
        public static cc Certificati = new cc('C',Color.PaleTurquoise); 
        public static cc Alimentazione = new cc('A',Color.Salmon); 
        public static cc FileEx = new cc('F',Color.Violet);
        public static cc Utenti = new cc('U', Color.FloralWhite);
        public static cc WMIQuery = new cc('W', Color.BlanchedAlmond); 
        public static cc Altro = new cc('X', Color.LightGray);     

        public struct TipoVal
        {
            public string tipo;
            public cc val;
             public TipoVal(String tipo,cc val)
            {
                this.tipo = tipo;
                this.val = val;
            }
        }
        public static Dictionary<char, Color> TipoCol()
        {
            Dictionary<char, Color> ret = new Dictionary<char, Color>();
            foreach (var prop in typeof(ETipo).GetFields())
            {
                cc v=(cc)prop.GetValue(typeof(ETipo));
                ret.Add(v.ch,v.co );
            }
            return ret;
        }
        public static Dictionary<string, cc> Tipi()
        {
            Dictionary<string, cc> ret = new Dictionary<string, cc>();
            foreach (var prop in typeof(ETipo).GetFields())
            {
                ret.Add(prop.Name, (cc)prop.GetValue(typeof(ETipo)));
            }
            return ret;
        }
    }

    public class CTipoCfgSrv
    {
        public char   Tipo;
        public string Nome;
        public override string ToString()
        {
            StringBuilder sb=new StringBuilder();            
            sb.Append( Tipo);
            sb.Append(" | ");
            sb.Append( Nome);
            sb.AppendLine();
            return sb.ToString();
        }
    }
    public class CTipoCfgReg
    {
        public char Tipo;
        public string Nome;
        public string Key;
        public string Value;
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Tipo);
            sb.Append(" | ");
            sb.Append(Nome);
            sb.Append(" | ");
            sb.Append(Key);
            sb.Append(" | ");
            sb.Append(Value);
            sb.AppendLine();
            return sb.ToString();
        }
    }
    public class CTipoCfgCrt
    {
        public char Tipo;
        public string StoreName;
        public string PropQuale;
        public string PropValore;
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Tipo);
            sb.Append(" | ");
            sb.Append(StoreName);
            sb.Append(" | ");
            sb.Append(PropQuale);
            sb.Append(" | ");
            sb.Append(PropValore);
            sb.AppendLine();
            return sb.ToString();
        }
    }
    public class CTipoCfgFile
    {
        public char Tipo;
        public string Path;
        public bool IsFile;       
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Tipo);
            sb.Append(" | ");
            sb.Append(Path);
            sb.Append(" | ");
            sb.Append(IsFile);
            sb.AppendLine();
            return sb.ToString();
        }
    }
    public class CTipoCfgWMIQ
    {
        public char Tipo;
        public string Nome;
        public string Query;
     //   public string Result;
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Tipo);
            sb.Append(" | ");
            sb.Append(Nome);
            sb.Append(" | ");
            sb.Append(Query);
       //     sb.Append(" | ");
       //     sb.Append(Result);
            sb.AppendLine();
            return sb.ToString();
        }
    }
}
