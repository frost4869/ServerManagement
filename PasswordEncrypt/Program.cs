using Paddedwall.CryptoLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PasswordEncrypt
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "Thi$ is @ str!&n to tEst encrypti0n!";
            Crypto c = new Crypto(Crypto.CryptoTypes.encTypeTripleDES);
            string s1 = c.Encrypt(input);
            string s2 = c.Decrypt(s1);

            Console.WriteLine(s1);
            Console.WriteLine(s2);


            //string s1 = Hashing.Hash(input);
            //Console.WriteLine("Hash only: " + s1);
            //string s2 = Hashing.Hash(input, Hashing.HashingTypes.MD5);
            //Console.WriteLine("Hash MD5: " + s2);
            //s1 = Hashing.Hash(input, Hashing.HashingTypes.SHA512);
            //Console.WriteLine("Hash SHA512: " + s1);

            Console.Read();
        }

        
    }
}
