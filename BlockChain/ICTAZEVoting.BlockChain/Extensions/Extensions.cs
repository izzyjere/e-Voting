using LevelDB;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ICTAZEVoting.BlockChain.Extensions
{
    public static class Extensions
    {    
    /// <summary>
    ///   Saves the blockchain in the key store.
    /// </summary>
    /// <param name="database"></param>
    /// <param name="blockChain"></param>
    /// <returns>The Secret Key as Base64 String</returns>
        public static string[] SaveEncrypted(this DB database, Models.BlockChain blockChain)
        {
            try
            {
                var itemJson = JsonConvert.SerializeObject(blockChain); //convert blockcahin to json       
                using var aesCrypto = Aes.Create();
                byte[] encrypted = EncryptStringToBytes_Aes(itemJson, aesCrypto.Key, aesCrypto.IV);  //encrypt the blockchain
                var base64Json = Convert.ToBase64String(encrypted);
                var key = Convert.ToBase64String(aesCrypto.Key);
                var iv = Convert.ToBase64String(aesCrypto.IV);
                database.Put(key, base64Json);

                return new string[] { key, iv };

            }
            catch 
            {
                 return null;              
            }
        }
        public static Models.BlockChain GetEncrypted(this DB database, string key,string IV)
        {
            var keyAsBytes = Convert.FromBase64String(key);
            var iVAsBytes = Convert.FromBase64String(IV);
            var encrypted = database.Get(key);
            var bytes = Convert.FromBase64String(encrypted);
            var decrypted = DecryptStringFromBytes_Aes(bytes, keyAsBytes, iVAsBytes);
            var block = JsonConvert.DeserializeObject<Models.BlockChain>(decrypted);
            return block;
        }
        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
}
