using NBitcoin;
using QBitNinja.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace HackChain.ConsoleTests.Homework
{
    public class Lecture04 : LectureBase
    {
        public static void Run()
        {
            //Test();
            Test2().Wait();
            //task.Wait();

            //SpendMultisigTransaction();
        }
        // 
        // 
        private static void Test()
        {
            Console.WriteLine(SeparatorLine);
            Console.WriteLine("Homework for lecture 04...");
            Console.WriteLine(SeparatorLine);
            Console.WriteLine(@"
    Task 1: Use Bitcoin test net
	- create transaction with lock script multisig 5 out of 7
	- create transaction that spends the previous transaction
");


            Console.WriteLine("Solution:");
            Console.WriteLine(SeparatorLine);

            Console.WriteLine($"Main address '{BitcoinSecrets.Main.PubKey}'");
            var index = 1;
            foreach (var privateKey in BitcoinSecrets.Secrets)
            {
                Console.WriteLine($"Multisig address #{index++} '{privateKey.PubKey}'");
            }
        }

        private static string PrivateKeyBase58ToPublicAddress(string privateKeyBase58, Network network = null, ScriptPubKeyType publicKeyType = ScriptPubKeyType.Segwit)
        {
            network = network ?? Network.TestNet;
            var bitcoinPrivateKey = new BitcoinSecret(privateKeyBase58, network); //tb1q07ahvzerc5njjf2jch0dx3wzsvaedkdln45f2k
            //var network = bitcoinPrivateKey.Network;
            var address = bitcoinPrivateKey.GetAddress(publicKeyType);

            return address.ToString();
        }


        private async static Task Test2()
        {
            var client = new QBitNinjaClient(Network.TestNet);
            var transactionId = uint256.Parse("fbffe7043bab5fb40cdcae7254958be6b755d52ac735bbc8c2ba6fac7035d89b");
            var transactionResponse = client.GetTransaction(transactionId).Result;

            var newInput = transactionResponse.ReceivedCoins[0];

            //var transactionMultisig = Transaction.Create(Network.TestNet);
            //transactionMultisig.Inputs.Add(new TxIn()
            //{
            //    PrevOut = newInput.Outpoint
            //});

            Script multisigRedeemScript =
                 PayToMultiSigTemplate
                 .Instance
                 .GenerateScriptPubKey(3, BitcoinSecrets.Secrets.Select(k => k.PubKey).ToArray());

            //transactionMultisig.Outputs.Add(Money.Coins(0.0003m), multisigRedeemScript.Hash.ScriptPubKey);

            //transactionMultisig.Sign(BitcoinSecrets.Main, newInput);

            var builder = BitcoinSecrets.Main.Network.CreateTransactionBuilder();
            var transactionMultisig = builder
                .AddCoin(newInput)
                .AddKeys(BitcoinSecrets.Main)
                .Send(multisigRedeemScript, Money.Coins(0.0003m))
                .SubtractFees()
                .SendFees(Money.Coins(0.00000141m))
                .SetChange(BitcoinSecrets.Main)
                .BuildTransaction(true);


            Console.WriteLine(builder.Verify(transactionMultisig));

            var response = await client.Broadcast(transactionMultisig);
            var trHex = transactionMultisig.ToHex();

            Console.WriteLine(trHex);

            //Console.WriteLine(response.));
            //Console.WriteLine(response.Error);
        }

        private static void SpendMultisigTransaction()
        {
            var client = new QBitNinjaClient(Network.TestNet);
            var transactionId = uint256.Parse("628f6cde93ca8ee5aaea260c79ae1390e1886abfecb048c90d4fb1eca1d603c0");
            var received = client.GetTransaction(transactionId).Result;


            Script multisigRedeemScript =
                 PayToMultiSigTemplate
                 .Instance
                 .GenerateScriptPubKey(1, BitcoinSecrets.Secrets.Select(k => k.PubKey).ToArray());


            Coin coin = received.Transaction.Outputs.AsCoins().First();

            TransactionBuilder builder = BitcoinSecrets.Main.Network.CreateTransactionBuilder();
            Transaction unsigned =
             builder
             .AddCoins(coin.ToScriptCoin(multisigRedeemScript))
             .Send(BitcoinSecrets.Main, Money.Coins(0.0000000100m))
             .SendFees(Money.Coins(0.00000141m))
             .SetChange(BitcoinSecrets.Main)
             .BuildTransaction(false);

            List<Transaction> partiallySigned = new List<Transaction>();

            foreach (var secret in BitcoinSecrets.Secrets.Take(3))
            {
                builder = BitcoinSecrets.Main.Network.CreateTransactionBuilder();
                Transaction partial =
                 builder
                 .AddCoins(coin)
                 .AddKeys(secret)
                 .SignTransaction(unsigned);

                partiallySigned.Add(partial);
            }

            builder = BitcoinSecrets.Main.Network.CreateTransactionBuilder();
            Transaction fullySigned =
             builder
             .AddCoins(coin)
             .CombineSignatures(partiallySigned.ToArray());

            Console.WriteLine(builder.Verify(fullySigned));
            Console.WriteLine(fullySigned.ToHex());
        }
    }
}
