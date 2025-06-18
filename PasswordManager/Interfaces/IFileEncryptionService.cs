using System.Collections.ObjectModel;

namespace PasswordManager.Interfaces
{
    public interface IFileEncryptionService
    {
        ObservableCollection<IPasswordModel> LoadPasswords(string password);

        void SavePasswords(ObservableCollection<IPasswordModel> passwords, string password);
    }
}
