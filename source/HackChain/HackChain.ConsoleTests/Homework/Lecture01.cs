using HackChain.ConsoleTests.BlockProducer;
using HackChain.Utilities;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackChain.ConsoleTests.Homework
{
    public class Lecture01 : LectureBase
    {
        public static void Run()
        {
            Console.WriteLine(SeparatorLine);
            Console.WriteLine("Homework for lecture 01...");
            Console.WriteLine(SeparatorLine);
            RunTask01();
            Console.WriteLine(SeparatorLine);
            RunTask02();
            Console.WriteLine(SeparatorLine);
        }

        private static void RunTask01()
        {
            Console.WriteLine(@"
    Task 1: Block producer
	- append only linked list of data;
	- fn: add new data;	- fn: preview by index;
	- fn: preview whole chain of data;
");


            Console.WriteLine("Solution:");

            var blockChain = new BlockChain();

            Console.WriteLine($"New Blockchain instance created. Blocks count '{blockChain.Height}'");

            List<string> data = new List<string>
            {
                "content of the first block",
                "content of the second block",
                "content of the third block",
                "content of the fourth block",
                "content of the fifth block",
                "content of the sixth block",
                "content of the 7 block",
                "content of the 8 block",
                "content of the 9 block",
                "content of the 10 block",
                "content of the 11 block",
                "content of the 12 block",
                "content of the 13 block",
                "content of the 14 block",
                "content of the 15 block",
            };

            foreach (var row in data)
            {
                blockChain.AddNewData(row);
            }

            Console.WriteLine($"'{data.Count}' blocks added. Blocks count '{blockChain.Height}'");

            int index = 3;
            var thirdBlock = blockChain.FindByIndex(index);
            Console.WriteLine($"Raw data at index '{index}' is '{data[index]}'.");
            Console.WriteLine($"Data in block at index '{index}' is '{thirdBlock.Data}'.");

            string filename = "BlockchainDump.txt";
            Console.WriteLine($"Saving the entire Blockchain to a file '{filename}'.");
            blockChain.SaveToFile(filename);

            Console.WriteLine($"Loading the entire Blockchain from a file '{filename}'.");
            var loadedBlockchain = BlockChain.LoadFromFile(filename);

            Console.WriteLine($"Original Blockchain blocks count '{blockChain.Height}', loaded Blockchain blocks count '{loadedBlockchain.Height}'.");

            Console.WriteLine("Manually modifying the data in the saved file.");

            var fileContent = File.ReadAllText(filename);
            var blocks = JsonConvert.DeserializeObject<List<Block>>(fileContent);

            var fiftBlock = blocks[4];
            fiftBlock.GetType().GetProperty("Data").SetValue(fiftBlock, "new data for block with index 4", null);
            
            var serializedBlocks = JsonConvert.SerializeObject(blocks);

            FileUtils.ReplaceFileStringContent(filename, serializedBlocks);

            Console.WriteLine($"Loading the entire Blockchain from a tampered file '{filename}'. The change is made to block with index 4, the error becomes visible when the Blockchain tries to verify the next block - the hash doesn't match...");

            try
            {
                loadedBlockchain = BlockChain.LoadFromFile(filename);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
            }
        }

        private static void RunTask02()
        {
            Console.WriteLine(@"
    Task 2: Keypair generator and signer
    - generate a keypair(ECDSA);
    -sign a message with that keypair;
    -verify if a signed message is not tampered;
");


            Console.WriteLine("Solution:");
            var keyPair = CryptoUtils.GenerateRandomKeys();
            var privateKey = (ECPrivateKeyParameters)keyPair.Private;
            var publicKey = (ECPublicKeyParameters)keyPair.Public;

            Console.WriteLine("Random key pair generated.");
            Console.WriteLine($"Private key '{privateKey.D}'");
            Console.WriteLine($"Public key '{publicKey.Q}'");

            string messageToSign = "Very important string content";

            string signature = CryptoUtils.SignData(messageToSign, privateKey);

            Console.WriteLine($"Message to sign '{messageToSign}'.");

            Console.WriteLine($"Signature: '{signature}'");

            var isProperlySigned = CryptoUtils.VerifySignature(publicKey, signature, messageToSign);

            Console.WriteLine($"'{signature}' is the {(isProperlySigned ? "CORRECT" : "INCORRECT")} signature for '{messageToSign}', verified with Publik key '{publicKey.Q}'.");

            string alteredMessage = messageToSign + " some change";
            Console.WriteLine();

            isProperlySigned = CryptoUtils.VerifySignature(publicKey, signature, alteredMessage);

            Console.WriteLine($"'{signature}' is the {(isProperlySigned ? "CORRECT" : "INCORRECT")} signature for '{alteredMessage}', verified with Publik key '{publicKey.Q}'.");
        }
    }
}
