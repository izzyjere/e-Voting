﻿using ICTAZEVoting.Shared.Models;

namespace ICTAZEVoting.BlockChain.Models
{
    public class BlockChain
    {
        public IList<Block> Chain { get; set; }       
        public List<Ballot> PendingBallots { get; set; } = new();
        public static int Difficulty { set; get; } = 2;
        public BlockChain()
        {
            InitializeChain();
        }
        public void AddBallot(Ballot Ballot)
        {
           PendingBallots.Add(Ballot);
        }
        public bool ProcessPendingBallots()
        {   
            if(!PendingBallots.Any())
            {
                return true;
            }
            foreach(var ballot in PendingBallots)
            {
                Block block = new(DateTime.Now, GetLatestBlock().Hash, ballot);
                //Verify Validity of block
                //Proof of work etc.
                AddBlock(block);
            }
            
            PendingBallots = new();
            return true;
        }
        public void InitializeChain()
        {
            Chain = new List<Block>();
            AddGenesisBlock();
        }
        /// <summary>
        /// This method creates the genesis
        /// </summary>
        /// <returns></returns>
        public static Block CreateGenesisBlock()
        {
            var block = new Block(DateTime.Now, "", new Ballot() { TimeStamp = DateTime.Now, ElectionId=Guid.Empty});
           // block.Mine(Difficulty);
            block.Hash = block.CalculateHash();
            return block;             
        }
        public void AddGenesisBlock()
        {
            Chain.Add(CreateGenesisBlock());
        }
        public List<Block> GetLatestBlocks(int count)
        {
            return Chain.TakeLast(count).ToList();
        }
        public Block GetLatestBlock()
        {
            return Chain[Chain.Count - 1];
        }
        
        /// <summary>
        /// Counts the Ballots for a particular candidate
        /// </summary>
        /// <param name="candidateId"></param>
        /// <returns></returns>
        public int GetBallotsCount(Guid candidateId)
        {
            return 0;
        }
        /// <summary>
        /// Adds a new block to the chain.
        /// </summary>
        /// <param name="block"></param>
        public void AddBlock(Block block)
        {
            Block latestBlock = GetLatestBlock();
            block.Index = latestBlock.Index + 1;
            block.PreviousHash = latestBlock.Hash;
            block.Hash = block.CalculateHash();
            //block.Mine(Difficulty);
            Chain.Add(block);
        }
        /// <summary>
        ///    The IsValid method will check two things.
        ///    Each block’s hash to see if the block is changed
        ///    Previous block’s hash to see if the block is changed and recalculated
        /// </summary>
        /// <returns>true if the block is valid.</returns>
        public bool IsValid()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                Block currentBlock = Chain[i];
                Block previousBlock = Chain[i - 1];

                if (currentBlock.Hash != currentBlock.CalculateHash())
                {
                    return false;
                }

                if (currentBlock.PreviousHash != previousBlock.Hash)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
