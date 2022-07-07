using ICTAZEvoting.Shared.Models;

using LevelDB;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICTAZEVoting.BlockChain.IO
{
    public class StorageContext
    {
        DB database;
        public StorageContext(string filePath)
        {
            if(string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }
            var options = new Options { CreateIfMissing=true };
            database = new(options,filePath);
        }
        public Models.BlockChain GetBlockChain()
        {
            throw new NotImplementedException();
        }

        internal void UpdateBlockChain(Models.BlockChain chain)
        {
            throw new NotImplementedException();
        }
    }
}
