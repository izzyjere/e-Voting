using ICTAZEvoting.Shared.Models;

using ICTAZEVoting.BlockChain.Extensions;

using LevelDB;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ICTAZEVoting.BlockChain.IO
{
    public class StorageContext
    {
        DB database;
        Dictionary<string, string> keyStore = new Dictionary<string, string>();
        public StorageContext(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }
            var options = new Options { CreateIfMissing = true };
            database = new(options, filePath);
        }
        public Models.BlockChain GetBlockChain()
        {
            if (keyStore.Any())
            {
                return database.GetEncrypted(keyStore["key"], keyStore["iv"]);
            }
            return null;
        }

        public void UpdateBlockChain(Models.BlockChain chain)
        {
            database.UpdateBlock(chain, keyStore["key"], keyStore["iv"]);
        }
        public void InitializeBlockChain()
        {
            var chain = new Models.BlockChain();
            chain.InitializeChain();
            var keyAndIV = database.SaveEncrypted(chain);
            if (keyAndIV != null)
            {
                keyStore.Add("key", keyAndIV[0]);
                keyStore.Add("iv", keyAndIV[1]);
            }
        }
    }
}
