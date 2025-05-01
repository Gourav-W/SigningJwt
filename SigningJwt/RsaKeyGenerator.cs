using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SigningJwt
{
    public class RsaKeyGenerator
    {


        /// <summary>
        /// Generates RSA public and private keys suitable for use with SHA384 hash algorithm
        /// </summary>
        /// <param name="keySize">Size of the RSA key in bits (e.g., 2048, 3072, 4096)</param>
        /// <param name="publicKeyPath">File path where the public key will be saved</param>
        /// <param name="privateKeyPath">File path where the private key will be saved</param>
        public static void GenerateRsaKeysForSha384(int keySize, string publicKeyPath, string privateKeyPath)
        {
            try
            {
                // Create a new instance of RSACryptoServiceProvider with the specified key size
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(keySize))
                {
                    // Export public key
                    string publicKey = rsa.ToXmlString(false); // false = public key only
                    File.WriteAllText(publicKeyPath, publicKey);
                    Console.WriteLine($"Public key saved to: {publicKeyPath}");

                    // Export private key
                    string privateKey = rsa.ToXmlString(true); // true = include private parameters
                    File.WriteAllText(privateKeyPath, privateKey);
                    Console.WriteLine($"Private key saved to: {privateKeyPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating RSA keys: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Sign a message using RSA-SHA384
        /// </summary>
        /// <param name="message">Message to sign</param>
        /// <param name="privateKeyPath">Path to the private key file</param>
        /// <returns>Base64 encoded signature</returns>
        public static string SignWithRsaSha384(string message, string privateKeyPath)
        {
            try
            {
                // Load private key
                string privateKeyXml = File.ReadAllText(privateKeyPath);

                // Create a new instance of RSACryptoServiceProvider and import the private key
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(privateKeyXml);

                    // Create a byte array from the message
                    byte[] data = Encoding.UTF8.GetBytes(message);

                    // Sign the data using SHA384
                    byte[] signature = rsa.SignData(data, new SHA384Managed());

                    // Return the signature as a Base64 string
                    return Convert.ToBase64String(signature);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error signing message: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Verify a signature using RSA-SHA384
        /// </summary>
        /// <param name="message">Original message</param>
        /// <param name="signature">Base64 encoded signature</param>
        /// <param name="publicKeyPath">Path to the public key file</param>
        /// <returns>True if verification succeeds, otherwise false</returns>
        public static bool VerifyRsaSha384Signature(string message, string signature, string publicKeyPath)
        {
            try
            {
                // Load public key
                string publicKeyXml = File.ReadAllText(publicKeyPath);

                // Create a new instance of RSACryptoServiceProvider and import the public key
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(publicKeyXml);

                    // Convert the message to a byte array
                    byte[] data = Encoding.UTF8.GetBytes(message);

                    // Convert the Base64 signature back to a byte array
                    byte[] signatureBytes = Convert.FromBase64String(signature);

                    // Verify the signature using SHA384
                    return rsa.VerifyData(data, new SHA384Managed(), signatureBytes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verifying signature: {ex.Message}");
                throw;
            }
        }
    }
}
