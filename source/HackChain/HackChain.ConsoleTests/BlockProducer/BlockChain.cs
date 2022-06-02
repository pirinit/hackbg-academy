using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackChain.ConsoleTests.BlockProducer
{
    public class BlockChain
    {
        private List<Block> _blocks;

        public BlockChain()
        {
            _blocks = new List<Block>();
        }

        public BlockChain(List<Block> blocks) : this()
        {
            foreach (var block in blocks)
            {
                var newBlock = AddNewData(block.Data, block.CreatedAt);

                if(!newBlock.Equals(block))
                {
                    throw new Exception($"Block[Index{block.Index}] doesn't match.");
                }
            }
        }

        public int Height
        {
            get
            {
                return _blocks.Count;
            }
        }

        public Block AddNewData(string data, DateTime? createdAt = null)
        {
            var previousBlock = _blocks.LastOrDefault();
            var newblockIndex = _blocks.Count;
            var newBlock = new Block(newblockIndex, data, previousBlock, createdAt);
            _blocks.Add(newBlock);

            return newBlock;
        }

        public Block FindByIndex(int index)
        {
            if(index < 0 || index >= _blocks.Count)
            {
                throw new ArgumentOutOfRangeException($"{nameof(index)} out of range. Can not be less than 0 and greater than the index of the last block.");
            }

            return _blocks[index];
        }

        public IEnumerable<Block> GetAllBlocks()
        {
            return _blocks;
        }

        public void SaveToFile(string filename)
        {
            var serializedBlocks = JsonConvert.SerializeObject(_blocks);
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            File.AppendAllText(filename, serializedBlocks);
        }

        public static BlockChain LoadFromFile(string filename)
        {
            var serializedBlocks = File.ReadAllText(filename);
            var blocks = JsonConvert.DeserializeObject<List<Block>>(serializedBlocks);

            return new BlockChain(blocks);
        }
    }
}
