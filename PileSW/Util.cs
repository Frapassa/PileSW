using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace PileSW
{
    class Util
    {

        /// <summary>
        /// Data una versione ottieni il seriale come long a 64bit
        /// </summary>
        /// <param name="major">ver major</param>
        /// <param name="minor">ver minor</param>
        /// <param name="build">build in formato YYGGG yy anno ggg giorno anno</param>
        /// <returns>Seriale della versione</returns>
        public static Int64 VerToSerial(int major, int minor, int build, int Revision)
        {
            Int64 ret = 0;
            Int64 appo = 0;
            try
            {
                appo = Convert.ToInt64(Revision);
                ret = ret + appo;
                appo = Convert.ToInt64(build); // AAGGG  anno e giorni del anno (0..365)
                ret = ret + appo * 10000;
                appo = Convert.ToInt64(minor); //Max 2 cifre
                ret = ret + appo * 1000000;
                appo = Convert.ToInt64(major);
                ret = ret + appo * 100000000;
            }
            catch (Exception)
            {

                ret = 0;
            }


            return ret;
        }
        #region Read Save Setting non usati
        /*
                
        public static KeyValueConfigurationCollection Settings()
        {
            var appSettings = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = appSettings.AppSettings.Settings;
            return settings;

        }
        public static string ReadSetting(string key)
        {
            string result = "";
            
            var appSettings = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = appSettings.AppSettings.Settings;
            if ( settings==null)
                return "Not Found";
            try
            {
              
                result = settings[key].Value ?? "Not Found";
                // result = appSettings[key] ?? "Not Found";
                Console.WriteLine(result);
            }
            catch (ConfigurationErrorsException ex)
            {
                
               log.Error(ex);
            }
                catch (NullReferenceException ex1)
            { 
                    log.Error(ex1); }
            finally
            {

            }
            return result;
        }

        public static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
             
                
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException ex)
            {
                log.Error(ex);
            }
        }*/
        #endregion

    }
}
