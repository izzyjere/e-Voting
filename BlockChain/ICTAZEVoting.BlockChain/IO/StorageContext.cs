using ICTAZEVoting.BlockChain.Extensions;

using LevelDB;

using Newtonsoft.Json;

using System.IO.IsolatedStorage;
namespace ICTAZEVoting.BlockChain.IO
{
    public class StorageContext : IDisposable
    {
        readonly DB database;
        IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User |IsolatedStorageScope.Assembly | IsolatedStorageScope.Domain, null, null);
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
        {   if()
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
        public void InitializeBlockChain()
        {
            var chain = new Models.BlockChain();
            chain.InitializeChain();
            var keyAndIV = database.SaveEncrypted(chain);
            if (keyAndIV != null)
            {
                AddKeys(keyAndIV);
            }
        }
        void AddKeys(string[] keys)
        {
           
            if (!isoStore.DirectoryExists("Data"))
            {
                isoStore.CreateDirectory("Data");
                isoStore.CreateDirectory("Data/BlockChain");
                isoStore.CreateDirectory("Data/BlockChain/Keys");
            }
            var dict = new Dictionary<string, string>();
            dict.Add("key", keys[0]);
            dict.Add("iv", keys[1]);
            var json = JsonConvert.SerializeObject(dict);
            using IsolatedStorageFileStream isoStream = new("Data/BlockChain/Keys/keys.txt", FileMode.CreateNew, isoStore);
            using StreamWriter writer = new(isoStream);
            writer.WriteLine(json);        


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
