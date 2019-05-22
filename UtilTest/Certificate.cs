using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;

namespace PileSwLib.UtilTest
{
    public struct DatiCert
    {
             public string Name;
             public string Issuer;
             public string Subject;
             public int Version;
             public DateTime ValidFrom;
             public DateTime ValidUntil;
             public string SerialNumber;
             public string SignatureAlgorithm;
             public string Thumbprint;
             public string ToString;
             public string ToStringLong;
    }
    public class GestCertificate : UtilTestLib
    {

        public static List<DatiCert> PrintCertificates(string name, StoreLocation location)
        {
 
            X509Store store = new X509Store(name, location);
             return PrintCertificates( store,  location);

        }

         public static bool FindCertificate(StoreName name, StoreLocation location, string quale, string test)
         {

             X509Store store = new X509Store(name, location);
             return FindCertificate( store,  location,  quale,  test);
         }
         public static bool FindCertificate(string name, StoreLocation location, string quale, string test)
         {

             X509Store store = new X509Store(name, location);
             return FindCertificate(store, location, quale, test);

         }

        // Metodi interni ---------------------------------------------------------------------------------
         private static List<DatiCert> PrintCertificates(X509Store store, StoreLocation location)
        {
            List<DatiCert> certs = new List<DatiCert>();
           // X509Store store = new X509Store(name, location);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                foreach (X509Certificate2 certificate in store.Certificates)
                {
                    certs.Add( PrintCertificateInfo(certificate));
                }
            }
            catch (Exception ex)
            {

                log.Error(ex);
            }
            finally
            {
                store.Close();
            }
            return certs;
        }

         private static DatiCert PrintCertificateInfo(X509Certificate2 certificate)
         {
             DatiCert cert;
             cert.Name = certificate.FriendlyName;
             cert.Issuer = certificate.IssuerName.Name;
             cert.Subject = certificate.SubjectName.Name;
             cert.Version =  certificate.Version;
             cert.ValidFrom = certificate.NotBefore;
             cert.ValidUntil=certificate.NotAfter;
             cert.SerialNumber = certificate.SerialNumber;
             cert.SignatureAlgorithm = certificate.SignatureAlgorithm.FriendlyName;
             cert.Thumbprint = certificate.Thumbprint;
             cert.ToString = certificate.ToString(false);
             cert.ToStringLong = certificate.ToString(true);
             return cert;
         }

        //------------------------ find name
         private static bool FindCertificate(X509Store store, StoreLocation location, string quale, string test)
         {
             string s = "";
             // X509Store store = new X509Store(name, location);
             try
             {
                 store.Open(OpenFlags.ReadOnly);
                 foreach (X509Certificate2 certificate in store.Certificates)
                 {
                     if (TestCertificate( certificate,  quale,  test))
                     {
                         return true;
                   
                     }
                 }
             }
             catch (Exception ex)
             {
                 log.Error(ex);
             }
             finally
             {
                 store.Close();
             }
             return false;
         }

         private static bool TestCertificate(X509Certificate2 certificate, string quale, string test)
         {
             bool ret = false;
             if (quale == "FriendlyName" && certificate.FriendlyName == test) ret = true;
             if (quale == "IssuerName" && certificate.IssuerName.Name == test) ret = true;
             if (quale == "SubjectName" && certificate.SubjectName.Name == test) ret = true;
             if (quale == "Version" && Convert.ToString(certificate.Version) == test) ret = true;
             if (quale == "NotBefore" && Convert.ToString(certificate.NotBefore) == test) ret = true;
             if (quale == "NotAfter" && Convert.ToString(certificate.NotAfter) == test) ret = true;
             if (quale == "SerialNumber" && certificate.SerialNumber == test) ret = true;
             if (quale == "SignatureAlgorithm" && certificate.SignatureAlgorithm.FriendlyName == test) ret = true;
             if (quale == "Thumbprint" && certificate.Thumbprint == test) ret = true;

             return ret;
         }
    }
}
