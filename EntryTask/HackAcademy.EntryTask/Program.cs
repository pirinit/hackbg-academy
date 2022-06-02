using System;
using System.Security.Cryptography;
using System.Text;

namespace HackAcademy.EntryTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < int.MaxValue; i++)
            {
                var inputString = $"hack{i}";
                var hash = ComputeSha256Hash(inputString);
                if (hash.StartsWith("00000"))
                {
                    Console.WriteLine($"{inputString}_{hash}");
                    break;
                }
            }
        }

        static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
