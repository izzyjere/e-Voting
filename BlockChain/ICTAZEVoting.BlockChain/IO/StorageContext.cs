using ICTAZEVoting.BlockChain.Extensions;
using ICTAZEVoting.Shared.Models;

using LevelDB;

using Newtonsoft.Json;

namespace ICTAZEVoting.BlockChain.IO
{
    public class StorageContext : IDisposable
    {
        readonly DB database;
        static Dictionary<string,string> keyStore { get; set; } = new Dictionary<string,string>();
        public StorageContext(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }
            var options = new Options { CreateIfMissing = true };
            database = new(options, filePath);
            var keys = database.Get("keys");
            if(!string.IsNullOrEmpty(keys))
            {
                keyStore = JsonConvert.DeserializeObject<Dictionary<string,string>>(keys);
            }
        }
        public Models.BlockChain GetBlockChain()
        {  
            if (keyStore.Any())
            {
                return database.GetEncrypted(keyStore["key"], keyStore["iv"]);
            }
            return null;
        }

        public Task SetAsync(string key, string value)
        {
            database.Put(key, value);
            return Task.CompletedTask;
        }

        public Task<string> GetAsync(string key)
        {
            var token = database.Get(key);
            return Task.FromResult(token);
        }

        public void UpdateBlockChain(Models.BlockChain chain)
        {
            database.UpdateBlock(chain, keyStore["key"], keyStore["iv"]);
        }
        public void AddBallot(Ballot ballot)
        {
            var chain = GetBlockChain();
            if(chain == null)
            {
                return;
            }
            chain.AddBallot(ballot);
            chain.ProcessPendingBallots();
            UpdateBlockChain(chain);
        }
        public void InitializeBlockChain()
        {
            var chain = new Models.BlockChain();
            chain.InitializeChain();
            var keyAndIV = database.SaveEncrypted(chain);
            if (keyAndIV != null)
            {
                keyStore.Clear();
                keyStore.Add("key",keyAndIV[0]);
                keyStore.Add("iv",keyAndIV[1]);
                database.Put("keys", JsonConvert.SerializeObject(keyStore));
            }
        }
      
        public bool ChainExists()
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            database.Delete(key);
        }

        public void Dispose()
        {
            database.Dispose();
        }
    }
}
