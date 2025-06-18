using PasswordManager.Interfaces;
using System.ComponentModel.Composition;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManager.Managers
{
    [Export(typeof(IEncryptionManager))]
    public class EncryptionManager : IEncryptionManager
    {
        public string DecryptFileToString(string encryptedFilePath, string password)
        {
            byte[] key = GenerateKeyFromPassword(password);

            if (!File.Exists(encryptedFilePath))
            {
                throw new FileNotFoundException($"The file path {encryptedFilePath} does not exist.");
            }

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;

                using (var inputFileStream = new FileStream(encryptedFilePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] iv = new byte[16];
                    inputFileStream.Read(iv, 0, iv.Length);

                    using (var decryptor = aes.CreateDecryptor(aes.Key, iv))
                    using (var cryptoStream = new CryptoStream(inputFileStream, decryptor, CryptoStreamMode.Read))
                    using (var reader = new StreamReader(cryptoStream, Encoding.UTF8))
                    {
                        // Read the decrypted content and return as a string
                        string decryptedContents = reader.ReadToEnd();
                        return decryptedContents;
                    }
                }
            }
        }

        public void EncryptStringToFile(string content, string outputFilePath, string password)
        {
            byte[] key = GenerateKeyFromPassword(password);

            if (!File.Exists(outputFilePath))
            {
                throw new FileNotFoundException($"The file path {outputFilePath} does not exist.");
            }

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.GenerateIV();
                byte[] iv = aes.IV;

                using (var outputFileStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
                {
                    // Write the IV to the start of the file
                    outputFileStream.Write(iv, 0, iv.Length);

                    using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                    using (var cryptoStream = new CryptoStream(outputFileStream, encryptor, CryptoStreamMode.Write))
                    using (var writer = new StreamWriter(cryptoStream, Encoding.UTF8))
                    {
                        // Write the content to the file
                        writer.Write(content);
                    }
                }
            }
        }

        private static byte[] GenerateKeyFromPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
