using ICTAZEVoting.Shared.Security;
using LevelDB;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace ICTAZEVoting.BlockChain.Extensions
{
    public static class Extensions
    {    
    /// <summary>
    ///   Saves the blockchain in the key store.
    /// </summary>
    /// <param name="database"></param>
    /// <param name="blockChain"></param>
    /// <returns>The Secret Key and IV as Base64 String Array</returns>
    
        public static string[] SaveEncrypted(this DB database, Models.BlockChain blockChain)
        {
            try
            {
                var itemJson = JsonConvert.SerializeObject(blockChain); //convert blockcahin to json       
                using var aesCrypto = Aes.Create();
                byte[] encrypted = EncryptionService.EncryptStringToBytes_Aes(itemJson, aesCrypto.Key, aesCrypto.IV);  //encrypt the blockchain
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
        public static void UpdateBlock(this DB database, Models.BlockChain blockChain, string key, string IV)
        {
            var itemJson = JsonConvert.SerializeObject(blockChain);//convert blockcahin to json   
            byte[] encrypted = EncryptionService.EncryptStringToBytes_Aes(itemJson, Convert.FromBase64String(key), Convert.FromBase64String(IV));  //encrypt the blockchain
            var base64Json = Convert.ToBase64String(encrypted);          
            database.Put(key, base64Json);
        }
        public static Models.BlockChain GetEncrypted(this DB database, string key,string IV)
        {
            var keyAsBytes = Convert.FromBase64String(key);
            var iVAsBytes = Convert.FromBase64String(IV);
            var encrypted = database.Get(key);
            var bytes = Convert.FromBase64String(encrypted);
            var decrypted = EncryptionService.DecryptStringFromBytes_Aes(bytes, keyAsBytes, iVAsBytes);
            var block = JsonConvert.DeserializeObject<Models.BlockChain>(decrypted);
            return block;
        }
       
    }
}
