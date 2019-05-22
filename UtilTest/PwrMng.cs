using MngPower;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PileSwLib.UtilTest
{
    public  class TPwrMng : UtilTestLib
    {
   
       

        public static Dictionary<string,uint> GetActPwrCfg( )
        {
            Dictionary<string, uint> ret = new Dictionary<string, uint>();
            IPowerScheme scheme;
            if (Environment.OSVersion.Version.Major > 5) scheme = new PowerSchemeVista();
            else scheme = new PowerSchemeXP();
            ;
            uint secHyber = scheme.Active.Policy.mach.DozeS4TimeoutAc; //secondi 0=disabilitato    
            uint secMonitorStandBy = scheme.Active.Policy.user.VideoTimeoutAc;
            uint secDiskStandBy = scheme.Active.Policy.user.SpindownTimeoutAc;         
            uint secIdleTimeout = scheme.Active.Policy.user.IdleTimeoutAc;           
            uint secDimTimeout = scheme.Active.DimTimeout;
            
            uint BrightnessNumericUpDown = scheme.Active.Brightness;
            uint DimBrightnessNumericUpDown = scheme.Active.DimBrightness;

         
            ret.Add("Time Hybernation", secHyber);
            ret.Add("Monitor Standby", secMonitorStandBy);
            ret.Add("Disk Standby", secDiskStandBy);
            ret.Add("Standby", secIdleTimeout);
            ret.Add("Dim illuminat.", secDimTimeout);

            //non usati
            ret.Add("Brightness Numeric UpDown", BrightnessNumericUpDown);
            ret.Add("Dim Brightness Numeric UpDown", DimBrightnessNumericUpDown);
            return ret;
        }

    }
}
