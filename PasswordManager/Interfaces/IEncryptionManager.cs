namespace PasswordManager.Interfaces
{
    public interface IEncryptionManager
    {
        string DecryptFileToString(string encryptedFilePath, string password);

        void EncryptStringToFile(string content, string outputFilePath, string password);
    }
}
