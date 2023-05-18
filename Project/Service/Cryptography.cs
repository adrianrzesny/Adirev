using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Adirev.Service
{
    public class Cryptography
    {
        #region Variables
        static readonly string PasswordHash = "4Q@WEE89el20";
        static readonly string SaltKey = "1r30P2RG6RB@";
        static readonly string VIKey = "@1B2c3D4e5F6g7H8";
        #endregion

        #region Public Method
        public static string Encrypt(string textToEncrypt)
        {
            string result = string.Empty;
            if (!String.IsNullOrEmpty(textToEncrypt))
            {
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(textToEncrypt);

                byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
                var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
                var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

                byte[] cipherTextBytes;

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        cipherTextBytes = memoryStream.ToArray();
                        cryptoStream.Close();
                    }
                    memoryStream.Close();
                }

                result = Convert.ToBase64String(cipherTextBytes);
            }
            return result;
        }

        public static string Decrypt(string encryptedText)
        {
            string result = string.Empty;
            if (!String.IsNullOrEmpty(encryptedText))
            {
                byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
                byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
                var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

                var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
                var memoryStream = new MemoryStream(cipherTextBytes);
                var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] textBytes = new byte[cipherTextBytes.Length];

                int decryptedByteCount = cryptoStream.Read(textBytes, 0, textBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();

                result = Encoding.UTF8.GetString(textBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
            }
            return result;
        }
        #endregion
    }
}
