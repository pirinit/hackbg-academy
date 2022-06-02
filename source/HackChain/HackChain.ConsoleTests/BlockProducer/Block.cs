using HackChain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackChain.ConsoleTests.BlockProducer
{
    public class Block
    {
        public int Index { get; private set; }
        public Block PreviousBlock { get; private set; }
        public string Data { get; private set; }
        public string Hash { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Block(int index, string data, Block previousBlock)
        {
            Index = index;
            Data = data;
            PreviousBlock = previousBlock;
            CreatedAt = DateTime.UtcNow;

            string previoudBlockHash = previousBlock != null ? previousBlock.Hash : "no previous block hash for the genesis block";
            Hash = CryptoUtils.CalcSHA256(data + previoudBlockHash);
        }
    }
}
