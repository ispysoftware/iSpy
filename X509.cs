using System;
using System.Security.Cryptography.X509Certificates;
//using CERTENROLLLib;
using iSpyApplication.Utilities;

namespace iSpyApplication
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
                Logger.LogMessage("Loaded SSL Certificate: " + _sslCertificate.ToString(false));
                return "OK";
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ex.Message;
            }
        }

        //public static void CreateSelfSignedCertificate(string subjectName)
        //{
        //    // create DN for subject and issuer
        //    var dn = new CX500DistinguishedName();
        //    dn.Encode("CN=" + subjectName);

        //    // create a new private key for the certificate
        //    CX509PrivateKey privateKey = new CX509PrivateKey
        //                                 {
        //                                     ProviderName =
        //                                         "Microsoft Base Cryptographic Provider v1.0",
        //                                     MachineContext = true,
        //                                     Length = 2048,
        //                                     KeySpec = X509KeySpec.XCN_AT_SIGNATURE,
        //                                     ExportPolicy =
        //                                         X509PrivateKeyExportFlags.XCN_NCRYPT_ALLOW_PLAINTEXT_EXPORT_FLAG
        //                                 };
        //    // use is not limited
        //    privateKey.Create();

        //    // Use the stronger SHA512 hashing algorithm
        //    var hashobj = new CObjectId();
        //    hashobj.InitializeFromAlgorithmName(ObjectIdGroupId.XCN_CRYPT_HASH_ALG_OID_GROUP_ID,
        //        ObjectIdPublicKeyFlags.XCN_CRYPT_OID_INFO_PUBKEY_ANY,
        //        AlgorithmFlags.AlgorithmFlagsNone, "SHA512");

        //    // add extended key usage if you want - look at MSDN for a list of possible OIDs
        //    var oid = new CObjectId();
        //    oid.InitializeFromValue("1.3.6.1.5.5.7.3.1"); // SSL server
        //    var oidlist = new CObjectIds {oid};
        //    var eku = new CX509ExtensionEnhancedKeyUsage();
        //    eku.InitializeEncode(oidlist);

        //    // Create the self signing request
        //    var cert = new CX509CertificateRequestCertificate();
        //    cert.InitializeFromPrivateKey(X509CertificateEnrollmentContext.ContextMachine, privateKey, "");
        //    cert.Subject = dn;
        //    cert.Issuer = dn; // the issuer and the subject are the same
        //    cert.NotBefore = DateTime.Now;
        //    // this cert expires immediately. Change to whatever makes sense for you
        //    cert.NotAfter = DateTime.Now;
        //    cert.X509Extensions.Add((CX509Extension)eku); // add the EKU
        //    cert.HashAlgorithm = hashobj; // Specify the hashing algorithm
        //    cert.Encode(); // encode the certificate

        //    // Do the final enrollment process
        //    var enroll = new CX509Enrollment();
        //    enroll.InitializeFromRequest(cert); // load the certificate
        //    enroll.CertificateFriendlyName = subjectName; // Optional: add a friendly name
        //    string csr = enroll.CreateRequest(); // Output the request in base64
        //                                         // and install it back as the response
        //    enroll.InstallResponse(InstallResponseRestrictionFlags.AllowUntrustedCertificate,
        //        csr, EncodingType.XCN_CRYPT_STRING_BASE64, ""); // no password
        //                                                        // output a base64 encoded PKCS#12 so we can import it back to the .Net security classes
        //    var base64Encoded = enroll.CreatePFX("", // no password, this is for internal consumption
        //        PFXExportOptions.PFXExportChainWithRoot);

        //    // instantiate the target class with the PKCS#12 data (and the empty password)
        //    _sslCertificate = new X509Certificate2(
        //        Convert.FromBase64String(base64Encoded), "",
        //        // mark the private key as exportable (this is usually what you want to do)
        //        X509KeyStorageFlags.Exportable
        //    );
        //}
    }
}
