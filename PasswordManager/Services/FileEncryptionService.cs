using PasswordManager.Definitions;
using PasswordManager.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;

namespace PasswordManager.Services
{
    [Export(typeof(IFileEncryptionService))]
    public class FileEncryptionService : IFileEncryptionService
    {
        IEncryptionManager encryptionManager;

        [ImportingConstructor]
        public FileEncryptionService(IEncryptionManager encryptionManager)
        {
            this.encryptionManager = encryptionManager;
        }

        public ObservableCollection<IPasswordModel> LoadPasswords(string password)
        {
            ObservableCollection<IPasswordModel> passwords = new ObservableCollection<IPasswordModel>();

            string databaseContents = encryptionManager.DecryptFileToString(FileDefinitions.DATABASE_FILE_PATH, password);

            string[] lines = databaseContents.Split(FileDefinitions.DATABASE_LINE_SEPARATOR_CHARACTER);
            
            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue; 

                string[] properties = line.Split(FileDefinitions.DATABASE_PROPERTY_SEPARATOR_CHARACTER);

                IPasswordModel passwordModel = new PasswordModel()
                { 
                    Name = properties[0],
                    Login = properties[1],
                    Password = properties[2]
                };

                passwords.Add(passwordModel);
            }

            return passwords;
        }

        public void SavePasswords(ObservableCollection<IPasswordModel> passwords, string password)
        {
            string content = "";

            foreach(IPasswordModel passwordModel in passwords)
            {
                content += string.Join(FileDefinitions.DATABASE_PROPERTY_SEPARATOR_CHARACTER,
                    passwordModel.Name,
                    passwordModel.Login,
                    passwordModel.Password);

                content += FileDefinitions.DATABASE_LINE_SEPARATOR_CHARACTER;
            }

            encryptionManager.EncryptStringToFile(content, FileDefinitions.DATABASE_FILE_PATH, password);
        }
    }
}
