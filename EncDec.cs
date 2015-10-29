using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace iSpyApplication
{
    /// <summary>
    /// Encrypts and Decrypts data
    /// </summary>
    public static class EncDec
    {
        /// <summary>
        /// Use AES to encrypt data string. The output string is the encrypted bytes as a base64 string.
        /// The same password must be used to decrypt the string.
        /// </summary>
        /// <param name="data">Clear string to encrypt.</param>
        /// <param name="password">Password used to encrypt the string.</param>
        /// <returns>Encrypted result as Base64 string.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// data
        /// or
        /// password
        /// </exception>
        public static string EncryptData(string data, string password)
        {
            if (string.IsNullOrEmpty(data) || string.IsNullOrEmpty(password))
                return "";
            
            byte[] encBytes = EncryptData(Encoding.UTF8.GetBytes(data), password, PaddingMode.ISO10126);
            return Convert.ToBase64String(encBytes);
        }
        /// <summary>
        /// Decrypt the data string to the original string.  The data must be the base64 string
        /// returned from the EncryptData method.
        /// </summary>
        /// <param name="data">Encrypted data generated from EncryptData method.</param>
        /// <param name="password">Password used to decrypt the string.</param>
        /// <returns>Decrypted string.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// data
        /// or
        /// password
        /// </exception>
        public static string DecryptData(string data, string password)
        {
            if (string.IsNullOrEmpty(data) || string.IsNullOrEmpty(password))
                return "";
            byte[] encBytes = Convert.FromBase64String(data);
            byte[] decBytes = DecryptData(encBytes, password, PaddingMode.ISO10126);
            return Encoding.UTF8.GetString(decBytes);
        }

        /// <summary>
        /// Encrypts a byte array with a password
        /// </summary>
        /// <param name="data">Data to encrypt</param>
        /// <param name="password">Password to use</param>
        /// <param name="paddingMode">Padding mode to use</param>
        /// <returns>Encrypted byte array</returns>
        /// <exception cref="System.ArgumentNullException">
        /// data
        /// or
        /// password
        /// </exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static byte[] EncryptData(byte[] data, string password, PaddingMode paddingMode)
        {
            if (data == null || data.Length == 0)
                throw new ArgumentNullException("data");
            if (password == null)
                throw new ArgumentNullException("password");
            var pdb = new PasswordDeriveBytes(password, Encoding.UTF8.GetBytes("Salt"));
            var rm = new RijndaelManaged { Padding = paddingMode };
            ICryptoTransform encryptor = rm.CreateEncryptor(pdb.GetBytes(16), pdb.GetBytes(16));
            pdb.Dispose();
            using (var msEncrypt = new MemoryStream())
            using (var encStream = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                encStream.Write(data, 0, data.Length);
                encStream.FlushFinalBlock();
                return msEncrypt.ToArray();
            }
        }
        /// <summary>
        /// Decrypts a byte array with a password
        /// </summary>
        /// <param name="data">Data to decrypt</param>
        /// <param name="password">Password to use</param>
        /// <param name="paddingMode">Padding mode to use</param>
        /// <returns>Decrypted byte array</returns>
        /// <exception cref="System.ArgumentNullException">
        /// data
        /// or
        /// password
        /// </exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static byte[] DecryptData(byte[] data, string password, PaddingMode paddingMode)
        {
            if (data == null || data.Length == 0)
                throw new ArgumentNullException("data");
            if (password == null)
                throw new ArgumentNullException("password");
            var pdb = new PasswordDeriveBytes(password, Encoding.UTF8.GetBytes("Salt"));
            var rm = new RijndaelManaged { Padding = paddingMode };
            ICryptoTransform decryptor = rm.CreateDecryptor(pdb.GetBytes(16), pdb.GetBytes(16));
            pdb.Dispose();
            using (var msDecrypt = new MemoryStream(data))
            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            {
                // Decrypted bytes will always be less then encrypted bytes, so length of encrypted data will be big enough for buffer.
                byte[] fromEncrypt = new byte[data.Length];
                // Read as many bytes as possible.
                int read = csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);
                if (read < fromEncrypt.Length)
                {
                    // Return a byte array of proper size.
                    byte[] clearBytes = new byte[read];
                    Buffer.BlockCopy(fromEncrypt, 0, clearBytes, 0, read);
                    return clearBytes;
                }
                return fromEncrypt;
            }
        }
    }
}
