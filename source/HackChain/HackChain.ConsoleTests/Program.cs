using HackChain.ConsoleTests.Homework;
using System;
using System.Runtime.InteropServices;

namespace HackChain.ConsoleTests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.SetWindowSize(120, 40);
            }
            
            //Lecture01.Run();

            Lecture04.Run();

        }
    }
}
