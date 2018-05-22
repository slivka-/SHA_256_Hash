using System;
using System.Text;
using System.Security.Cryptography;

namespace Sha256Hash
{
    class Program
    {
        static void Main(string[] args)
        {
            if(BitConverter.IsLittleEndian)
                Console.WriteLine("System is in little endian");
            else
                Console.WriteLine("System is in big endian");
            Console.WriteLine();

            Console.WriteLine("Input message:");
            byte[] byteMessage = Encoding.UTF8.GetBytes(Console.ReadLine());
            Console.WriteLine();

            //LIBRARY HASH
            byte[] libHash = SHA256.Create().ComputeHash(byteMessage);
            Console.WriteLine("Hash from standard library:\n{0}", libHash.ToHexString());
            Console.WriteLine();

            //MY HASH
            byte[] myHash = Hash256.GenerateHash(byteMessage);
            Console.WriteLine("Hash from my function:\n{0}", myHash.ToHexString());
            Console.WriteLine();

            if(libHash.ToHexString().Equals(myHash.ToHexString()))
                Console.WriteLine("Hash generated correctly!");
            else
                Console.WriteLine("Error generating hash!");

            Console.ReadLine();
        }
    }
}
