
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PileSwLib.UtilTest
{
    public  class Registri : UtilTestLib
    {


    public static  RegistryKey BaseKey(string chiave,out string baseKey,out string keyb,out string NSubVal)
        {
            RegistryKey key = Registry.ClassesRoot;

            string[] s=chiave.Split('\\');
            string keyRoot = s[0];
            string chiaveT = "";
            for (int i = 1; i < s.Length; i++)
            {
                chiaveT += s[i] + "\\";
            }
            baseKey = keyRoot;
            keyb = chiaveT;
            NSubVal = "Key:0 Val:0";
            if (keyRoot == Registry.ClassesRoot.Name)
                key = Registry.ClassesRoot;
            if (keyRoot == Registry.CurrentConfig.Name)
                key = Registry.CurrentConfig;
            if (keyRoot == Registry.CurrentUser.Name)
                key = Registry.CurrentUser;
            if (keyRoot == Registry.LocalMachine.Name)
                key = Registry.LocalMachine;
            if (keyRoot == Registry.PerformanceData.Name)
                key = Registry.PerformanceData;
            if (keyRoot == Registry.Users.Name)
                key = Registry.Users;
            RegistryKey KeyF = key.OpenSubKey(chiaveT);
            if (KeyF != null) {
                NSubVal = "Key:";
                NSubVal +=Convert.ToString(KeyF.SubKeyCount);
                NSubVal += " Val:";
                NSubVal += Convert.ToString(KeyF.ValueCount);
            }
            return KeyF;
        }
 
    }


}

