using System;
using System.Security.Cryptography.X509Certificates;
using iSpyApplication.Utilities;

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
                Logger.LogMessageToFile("Loaded SSL Certificate: " + _sslCertificate.ToString(false),"X509");
                return "OK";
            }
            catch (Exception ex)
            {
                Logger.LogExceptionToFile(ex,"X509");
                return ex.Message;
            }
        }
    }
}
