﻿using HackChain.Utilities;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
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