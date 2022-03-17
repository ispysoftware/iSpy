using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using iSpyApplication.Utilities;
using System.Net;

namespace iSpyApplication.Server
{
    class X509
    {
        private static X509Certificate _sslCertificate;

        public static X509Certificate SslCertificate => _sslCertificate;

        public static bool SslEnabled => MainForm.Conf.SSLEnabled && _sslCertificate != null;

        public static string LoadCertificate(string fileName)
        {
            try
            {
                _sslCertificate = X509Certificate.CreateFromCertFile(fileName);
                Logger.LogMessage("Loaded SSL Certificate: " + _sslCertificate.ToString(false),"X509");
                return "OK";
            }
            catch (Exception ex)
            {
                Logger.LogException(ex,"X509");
                return ex.Message;
            }
        }

        public static void CreateCertificate(string ip)
        {
            try
            {
                var cr = new CertificateRequest(new X500DistinguishedName("cn=ispy-local"), RSA.Create(), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                SubjectAlternativeNameBuilder subjectAlternativeNameBuilder = new SubjectAlternativeNameBuilder();
                subjectAlternativeNameBuilder.AddIpAddress(IPAddress.Parse(ip));
                var locals = MainForm.AddressListIPv4;
                foreach(var lip in locals)
                    subjectAlternativeNameBuilder.AddIpAddress(lip);

                cr.CertificateExtensions.Add(subjectAlternativeNameBuilder.Build());
                string pw = "123abc!!D";
                var cert = cr.CreateSelfSigned(DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddYears(+1));
                var cert2 = new X509Certificate2(cert.Export(X509ContentType.Pfx, pw), pw);
                X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadWrite);
                
                X509Certificate2Collection col = store.Certificates.Find(X509FindType.FindBySubjectName, "ispy-local", false);
                if (col.Count>0)
                    store.RemoveRange(col);

                store.Add(cert2);
                store.Close();
                _sslCertificate = cert2;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "X509 Create");
            }
        }
    }
}
