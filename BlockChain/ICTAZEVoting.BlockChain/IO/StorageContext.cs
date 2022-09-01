using ICTAZEVoting.BlockChain.Extensions;
using ICTAZEVoting.Shared.Models;

using LevelDB;

using Newtonsoft.Json;

namespace ICTAZEVoting.BlockChain.IO
{
    public static class StorageContext 
    { 
        readonly static DB database = new(new Options { CreateIfMissing=true},Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"Evoting","database"));
        static Dictionary<string,string> keyStore { get; set; } = new Dictionary<string,string>();
      
        public static Models.BlockChain GetBlockChain()
        {  
            if (keyStore.Any())
            {
                return database.GetEncrypted(keyStore["key"], keyStore["iv"]);
            }
            return null;
        }

        public static Task SetAsync(string key, string value)
        {
            database.Put(key, value);
            return Task.CompletedTask;
        }

        public static Task<string> GetAsync(string key)
        {
            var token = database.Get(key);
            return Task.FromResult(token);
        }

        public static void UpdateBlockChain(Models.BlockChain chain)
        {
            database.UpdateBlock(chain, keyStore["key"], keyStore["iv"]);
        }
        public static Task<bool> AddBallot(Ballot ballot)
        {
            var chain = GetBlockChain();
            if(chain == null)
            {
                return Task.FromResult(false);
            }
            chain.AddBallot(ballot);
            if(chain.ProcessPendingBallots())
            {   
                UpdateBlockChain(chain);
                return Task.FromResult(true);
            }
            else
            {
                return Task.FromResult(false);
            }
            
        }
        public static void InitializeBlockChain()
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
      
        

        public static void Remove(string key)
        {
            database.Delete(key);
        }

        public static void Dispose()
        {
            database.Dispose();
        }
    }
}
