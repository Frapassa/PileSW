using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace PileSwLib.UtilTest
{
    public  class Servizi : UtilTestLib
    {
           public struct servizio
            {
              public string DisplayName               ;
              public string ServiceName               ;
              public string Status;
            }
        //Lista Servizi
        public static List<servizio> GetService()
        {
 
            ServiceController[] services = ServiceController.GetServices();
            StringBuilder sss = new StringBuilder();
            List<servizio> servizi = new List<servizio>();
            foreach (var item in services)
            {
                servizio srv = new servizio();
               srv.DisplayName= item.DisplayName;
               srv.ServiceName = item.ServiceName;
               srv.Status = item.Status.ToString();
               servizi.Add(srv);

            }
            return servizi;
            //  return CreateList("Select * from Win32_Service");
            // return CreateList("Select * from Win32_SoftwareElement");
        }
        // dati statici
        public static bool DoesServiceExist(string serviceName,out string DisplayName, out System.ServiceProcess.ServiceControllerStatus Stato)
        {
            ServiceController[] services = ServiceController.GetServices();
            Stato = ServiceControllerStatus.Running;
            DisplayName = "";
            var service = services.FirstOrDefault(s => s.ServiceName.ToLower() == serviceName.ToLower());
            if (service != null)
            {
            Stato = service.Status;
            DisplayName = service.DisplayName;}

            return service != null;
        }

    }


}
