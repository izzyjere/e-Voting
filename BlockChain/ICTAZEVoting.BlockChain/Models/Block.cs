﻿using ICTAZEvoting.Shared.Models;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
namespace ICTAZEVoting.BlockChain.Models
{
    public class Block
    {
        public int Index { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public Vote Data { get; set; }
        public Block(DateTime timeStamp, string previousHash, Vote data)
        {
            TimeStamp = timeStamp;
            PreviousHash = previousHash;
            Data = data;
            Hash = CalculateHash();
        }
        public string CalculateHash()
        {
            SHA256 sha256 = SHA256.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash ?? ""}-{JsonConvert.SerializeObject(Data)}");
            byte[] outputBytes = sha256.ComputeHash(inputBytes);
            return Convert.ToBase64String(outputBytes);
        }

    }
}