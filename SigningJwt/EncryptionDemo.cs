using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SigningJwt
{
    internal class EncryptionDemo
    {
        public static (string, string) GenerateRsaSha384Key()
        {
            //using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            using (RSA rsa = RSA.Create())
            {
                string publicKey = rsa.ToXmlString(false);
                string privateKey = rsa.ToXmlString(true);

                return (publicKey, privateKey);
            }
        }

        public static (string, string) GenerateKey()
        {
            //using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            using (RSA rsa = RSA.Create())
            {
                string publicKey = rsa.ToXmlString(false);
                string privateKey = rsa.ToXmlString(true);

                return (publicKey, privateKey);
            }
        }

        public static byte[] Encrypt(string publicKey, string dataToEncrypt)
        {
            //using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            using (RSA rsa = RSA.Create())
            {
                rsa.FromXmlString(publicKey);
                byte[] messageBytes = Encoding.UTF8.GetBytes(dataToEncrypt);
                byte[] encryptedBytes = rsa.Encrypt(messageBytes, RSAEncryptionPadding.Pkcs1);
                return encryptedBytes;
            }
        }

        public static string Decrypt(string privateKey, byte[] encryptedBytes)
        {
            //using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            using (RSA rsa = RSA.Create())
            {
                rsa.FromXmlString(privateKey);
                byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);
                string decryptedMessage = Encoding.UTF8.GetString(decryptedBytes);
                return decryptedMessage;
            }
        }

        public static byte[] Sign(string privateKey, string dataToSign)
        {
            //using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            using (RSA rsa = RSA.Create())
            {
                rsa.FromXmlString(privateKey);
                byte[] messageBytes = Encoding.UTF8.GetBytes(dataToSign);
                byte[] signatureBytes = rsa.SignData(messageBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                return signatureBytes;
            }
        }

        public static bool Verify(string publicKey, string dataToValidate, byte[] signature)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            //using (RSA rsa = RSA.Create())
            {
                rsa.FromXmlString(publicKey);
                byte[] messageBytes = Encoding.UTF8.GetBytes(dataToValidate);
                return rsa.VerifyData(messageBytes, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }
    }
}
