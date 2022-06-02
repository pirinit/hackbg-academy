using System;
using System.Collections.Generic;
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

        public int Height
        {
            get
            {
                return _blocks.Count;
            }
        }

        public void AddNewData(string data)
        {
            var previousBlock = _blocks.LastOrDefault();
            var newblockIndex = _blocks.Count;
            var newBlock = new Block(newblockIndex, data, previousBlock);
            _blocks.Add(newBlock);
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
    }
}
