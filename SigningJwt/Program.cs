using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SigningJwt
{
    internal class Program
    {
        static void Main(string[] args)
        {
        }


        public void GenerateJwtToken()
        {
            try
            {
                // Define file paths for the keys
                string publicKeyPath = Path.Combine(Environment.CurrentDirectory, "public_key.xml");
                string privateKeyPath = Path.Combine(Environment.CurrentDirectory, "private_key.xml");

                // Generate RSA key pair (using 3072 bits for good security)
                Console.WriteLine("Generating RSA key pair for use with SHA384...");
                RsaKeyGenerator.GenerateRsaKeysForSha384(3072, publicKeyPath, privateKeyPath);

                // Test signing and verification
                string message = "This is a test message that needs to be signed with RSA-SHA384";
                Console.WriteLine($"\nOriginal Message: {message}");

                // Sign the message
                Console.WriteLine("\nSigning message with private key...");
                string signature = RsaKeyGenerator.SignWithRsaSha384(message, privateKeyPath);
                Console.WriteLine($"Signature: {signature}");

                var rsaKey = RSA.Create();

                rsaKey.FromXmlString()
                RsaSecurityKey rsaSecurityKey = new RsaSecurityKey(RSA.Create());

                //// Verify the signature
                //Console.WriteLine("\nVerifying signature with public key...");
                //bool isValid = RsaKeyGenerator.VerifyRsaSha384Signature(message, signature, publicKeyPath);
                //Console.WriteLine($"Signature verification result: {(isValid ? "VALID" : "INVALID")}");

                //// Tamper with the message to show verification failure
                //string tamperedMessage = message + " (tampered)";
                //Console.WriteLine($"\nTampered Message: {tamperedMessage}");

                //bool isTamperedValid = RsaKeyGenerator.VerifyRsaSha384Signature(tamperedMessage, signature, publicKeyPath);
                //Console.WriteLine($"Tampered message verification result: {(isTamperedValid ? "VALID" : "INVALID")} (should be INVALID)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        public static void GenerateKeys()
        { 
            string keyDirectorypath = Path.Combine(Environment.CurrentDirectory, "Keys");
            if (!Directory.Exists(keyDirectorypath))
            {
                Directory.CreateDirectory(keyDirectorypath);
            }

            var rsa = RSA.Create();
            string privateKeyXml = rsa.ToXmlString(true);
            string publicKeyXml = rsa.ToXmlString(false);

            var privateFile = File.Create(Path.Combine(keyDirectorypath, "PrivateKey.xml"));
            var publicFile = File.Create(Path.Combine(keyDirectorypath, "PubliceKey.xml"));
            var privateKeyBytes = Encoding.UTF8.GetBytes(privateKeyXml);
            var publicKeyBytes = Encoding.UTF8.GetBytes(publicKeyXml);

            privateFile.Write(privateKeyBytes, 0 , privateKeyBytes.Length);
            publicFile.Write(publicKeyBytes, 0, publicKeyBytes.Length);
        }
    }
}
