namespace ICTAZEVoting.BlockChain.Models
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
        /// <summary>
        /// This method creates the genesis
        /// </summary>
        /// <returns></returns>
        public Block CreateGenesisBlock()
        {
            return new Block(DateTime.Now, "", null);
        }
        public void AddGenesisBlock()
        {
            Chain.Add(CreateGenesisBlock());
        }
        public Block GetLatestBlock()
        {
            return Chain[Chain.Count - 1];
        }
        /// <summary>
        /// Counts the votes for a particular candidate
        /// </summary>
        /// <param name="candidateId"></param>
        /// <returns></returns>
        public int GetVotesCount(Guid candidateId)
        {
            return Chain.Count(b => b.Data.CandidateId == candidateId);
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
