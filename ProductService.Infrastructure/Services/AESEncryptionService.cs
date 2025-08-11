using ProductService.Domain.Services;
using ProductService.Domain.Shared;
using System.Security.Cryptography;

namespace ProductService.Infrastructure.Services
{
    public class AESEncryptionService : IAESEncryptionService
    {
        private readonly byte[] key;

        private readonly byte[] initializationVector;

        public AESEncryptionService()
        {
            key = Constants.Key;
            initializationVector = Constants.Vector;
        }

        public string Encrypt(string plainText)
        {
            if (string.IsNullOrWhiteSpace(plainText))
            {
                throw new ArgumentNullException("plainText");
            }

            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = initializationVector;
            ICryptoTransform transform = aes.CreateEncryptor(aes.Key, aes.IV);
            using MemoryStream memoryStream = new MemoryStream();
            using CryptoStream stream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
            using (StreamWriter streamWriter = new StreamWriter(stream))
            {
                streamWriter.Write(plainText);
            }

            return Convert.ToBase64String(memoryStream.ToArray());
        }

        public string Decrypt(string encryptedText)
        {
            if (string.IsNullOrWhiteSpace(encryptedText))
            {
                throw new ArgumentNullException("encryptedText");
            }

            string text = null;
            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = initializationVector;
            ICryptoTransform transform = aes.CreateDecryptor(aes.Key, aes.IV);
            using MemoryStream stream = new MemoryStream(Convert.FromBase64String(encryptedText));
            using CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
            using StreamReader streamReader = new StreamReader(stream2);
            return streamReader.ReadToEnd();
        }
    }
}
