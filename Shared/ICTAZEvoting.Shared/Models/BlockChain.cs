using System;
using System.Collections.Generic;
using System.Text;

namespace ICTAZEvoting.Shared.Models
{
    public class BlockChain
    {
        public IList<Block> Chain { get; set; }
        public BlockChain()
        {
            InitializeChain();

        }
        public void InitializeChain()
        {
            Chain = new List<Block>();
        }
        public Block CreateGenesisBlock()
        {
            return new Block(DateTime.Now, "",null);
        }

    }
}
