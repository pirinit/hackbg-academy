using HackChain.Utilities;
using System;

namespace HackChain.ConsoleTests.BlockProducer
{
    public class Block
    {
        public int Index { get; private set; }
        public Block PreviousBlock { get; private set; }
        public string Data { get; private set; }
        public string Hash { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Block(int index, string data, Block previousBlock, DateTime? createdAt = null)
        {
            Index = index;
            Data = data;
            PreviousBlock = previousBlock;
            CreatedAt = createdAt ?? DateTime.UtcNow;
            
            Hash = CryptoUtils.CalcSHA256(GetBlockContentForHashing());
        }

        private string GetBlockContentForHashing()
        {
            string previoudBlockHash = PreviousBlock != null ? PreviousBlock.Hash : "no previous block hash for the genesis block";
            
            return $"{Index} {CreatedAt} {Data} {previoudBlockHash}";
        }

        public override bool Equals(object obj) => this.Equals(obj as Block);

        public override int GetHashCode()
        {
            return GetBlockContentForHashing().GetHashCode();
        }

        public bool Equals(Block other)
        {
            if (other is null)
            {
                return false;
            }
            
            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }

            bool areEqual = 
                Index == other.Index &&
                CreatedAt == other.CreatedAt &&
                Data == other.Data &&
                Hash == other.Hash;

            return areEqual;
        }
    }
}
