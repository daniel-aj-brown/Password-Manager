using PasswordManager.Definitions;
using PasswordManager.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;

namespace PasswordManager
{
    [Export(typeof(IPasswordManagerViewModel))]
    public class PasswordManagerViewModel : BindableBase, IPasswordManagerViewModel
    {
        #region Private Backing Fields

        private IFileEncryptionService fileEncryptionService;
        private ObservableCollection<IPasswordModel> passwords;
        private IPasswordModel selectedPassword;
        private string masterPassword;
        private bool isLoggedIn;

        #endregion

        #region Public Properties

        public ObservableCollection<IPasswordModel> Passwords
        {
            get { return passwords; }
            set { SetProperty(ref passwords, value); }
        }

        public IPasswordModel SelectedPassword
        {
            get { return selectedPassword; }
            set
            {
                if (SetProperty(ref selectedPassword, value))
                {
                    RaisePropertyChanged(nameof(IsSelectedPasswordValid));
                }
            }

        }

        public string MasterPassword
        {
            get { return masterPassword; }
            set { SetProperty(ref masterPassword, value); }
        }

        public bool IsLoggedIn
        {
            get { return isLoggedIn; }
            set
            {
                if (SetProperty(ref isLoggedIn, value))
                {
                    RaisePropertyChanged(nameof(LoginButtonBinding));
                }
            }
        }

        public string LoginButtonBinding
        {
            get
            {
                return IsLoggedIn ? "Logout": LoginLocalisation;
            }
        }

        public bool IsSelectedPasswordValid
        {
            get
            {
                return SelectedPassword != null;
            }
        }

        #endregion

        #region Commands

        public ICommand LoginCommand { get; private set; }
        public ICommand SaveChangesCommand { get; private set; }
        public ICommand RevertChangesCommand { get; private set; }
        public ICommand CopyCommand { get; private set; }
        public ICommand NewCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        #endregion

        #region Private Localisation Fields

        private string? newButtonLocalisation;
        private string? saveButtonLocalisation;
        private string? renameButtonLocalisation;
        private string? deleteButtonLocalisation;
        private string? mainWindowLocalisation;
        private string? masterPasswordLocalisation;
        private string? saveChangesLocalisation;
        private string? revertChangesLocalisation;
        private string? nameLocalisation;
        private string? loginLocalisation;
        private string? passwordLocalisation;
        private string? copyLocalisation;
        private string? unableToLoginLocalisation;
        private string? errorLocalisation;
        private string? invalidCharacterFoundLocalisation;
        private string? passwordCopiedToClipboardLocalisation;
        private string? passwordCopiedLocalisation;
        private string? newLocalisation;
        private string? changesSavedSuccessfullyLocalisation;
        private string? changesSavedLocalisation;

        #endregion

        #region Public Localisation

        public string? NewButtonLocalisation
        {
            get { return newButtonLocalisation; }
            set { SetProperty(ref newButtonLocalisation, value); }
        }

        public string? SaveButtonLocalisation
        {
            get { return saveButtonLocalisation; }
            set { SetProperty(ref saveButtonLocalisation, value); }
        }

        public string? RenameButtonLocalisation
        {
            get { return renameButtonLocalisation; }
            set { SetProperty(ref renameButtonLocalisation, value); }
        }

        public string? DeleteButtonLocalisation
        {
            get { return deleteButtonLocalisation; }
            set { SetProperty(ref deleteButtonLocalisation, value); }
        }

        public string? MainWindowLocalisation
        {
            get { return mainWindowLocalisation; }
            set { SetProperty(ref mainWindowLocalisation, value); }
        }

        public string? MasterPasswordLocalisation
        {
            get { return masterPasswordLocalisation; }
            set { SetProperty(ref masterPasswordLocalisation, value); }
        }

        public string? SaveChangesLocalisation
        {
            get { return saveChangesLocalisation; }
            set { SetProperty(ref saveChangesLocalisation, value); }
        }

        public string? RevertChangesLocalisation
        {
            get { return revertChangesLocalisation; }
            set { SetProperty(ref revertChangesLocalisation, value); }
        }

        public string? NameLocalisation
        {
            get { return nameLocalisation; }
            set { SetProperty(ref nameLocalisation, value); }
        }

        public string? LoginLocalisation
        {
            get { return loginLocalisation; }
            set { SetProperty(ref loginLocalisation, value); }
        }

        public string? PasswordLocalisation
        {
            get { return passwordLocalisation; }
            set { SetProperty(ref passwordLocalisation, value); }
        }

        public string? CopyLocalisation
        {
            get { return copyLocalisation; }
            set { SetProperty(ref copyLocalisation, value); }
        }

        public string? UnableToLoginLocalisation
        {
            get { return unableToLoginLocalisation; }
            set { SetProperty(ref unableToLoginLocalisation, value); }
        }

        public string? ErrorLocalisation
        {
            get { return errorLocalisation; }
            set { SetProperty(ref errorLocalisation, value); }
        }

        public string? InvalidCharacterFoundLocalisation
        {
            get { return invalidCharacterFoundLocalisation; }
            set { SetProperty(ref invalidCharacterFoundLocalisation, value); }
        }

        public string? PasswordCopiedToClipboardLocalisation
        {
            get { return passwordCopiedToClipboardLocalisation; }
            set { SetProperty(ref passwordCopiedToClipboardLocalisation, value); }
        }

        public string? PasswordCopiedLocalisation
        {
            get { return passwordCopiedLocalisation; }
            set { SetProperty(ref passwordCopiedLocalisation, value); }
        }

        public string? NewLocalisation
        {
            get { return newLocalisation; }
            set { SetProperty(ref newLocalisation, value); }
        }

        public string? ChangesSavedSuccessfullyLocalisation
        {
            get { return changesSavedSuccessfullyLocalisation; }
            set { SetProperty(ref changesSavedSuccessfullyLocalisation, value); }
        }

        public string? ChangesSavedLocalisation
        {
            get { return changesSavedLocalisation; }
            set { SetProperty(ref changesSavedLocalisation, value); }
        }

        #endregion

        #region Constructors

        [ImportingConstructor]
        public PasswordManagerViewModel(IFileEncryptionService fileEncryptionService)
        {
            this.fileEncryptionService = fileEncryptionService;

            LoginCommand = new DelegateCommand(LoginCommandHandler);
            SaveChangesCommand = new DelegateCommand(() => _ = SaveChangesCommandHandler());
            RevertChangesCommand = new DelegateCommand(() => _ = RevertChangesCommandHandler());
            CopyCommand = new DelegateCommand(CopyCommandHandler);
            NewCommand = new DelegateCommand(NewCommandHandler);
            DeleteCommand = new DelegateCommand(DeleteCommandHandler);

            GetLocalisations();

            RaisePropertyChanged(nameof(LoginButtonBinding));
        }

        #endregion

        #region Methods

        private void GetLocalisations()
        {
            NewButtonLocalisation = Localisations.NewButtonLocalisation;
            SaveButtonLocalisation = Localisations.SaveButtonLocalisation;
            RenameButtonLocalisation = Localisations.RenameButtonLocalisation;
            DeleteButtonLocalisation = Localisations.DeleteButtonLocalisation;
            MainWindowLocalisation = Localisations.MainWindowLocalisation;
            MasterPasswordLocalisation = Localisations.MasterPasswordLocalisation;
            SaveChangesLocalisation = Localisations.SaveChangesLocalisation;
            RevertChangesLocalisation = Localisations.RevertChangesLocalisation;
            NameLocalisation = Localisations.NameLocalisation;
            LoginLocalisation = Localisations.LoginLocalisation;
            PasswordLocalisation = Localisations.PasswordLocalisation;
            CopyLocalisation = Localisations.CopyLocalisation;
            UnableToLoginLocalisation = Localisations.UnableToLoginLocalisation;
            ErrorLocalisation = Localisations.ErrorLocalisation;
            InvalidCharacterFoundLocalisation = Localisations.InvalidCharacterFoundLocalisation;
            PasswordCopiedToClipboardLocalisation = Localisations.PasswordCopiedToClipboardLocalisation;
            PasswordCopiedLocalisation = Localisations.PasswordCopiedLocalisation;
            NewLocalisation = Localisations.NewLocalisation;
            ChangesSavedSuccessfullyLocalisation = Localisations.ChangesSavedSuccessfullyLocalisation;
            ChangesSavedLocalisation = Localisations.ChangesSavedLocalisation;
        }

        private void LoginCommandHandler()
        {
            if (!isLoggedIn)
            {
                try
                {
                    Passwords = this.fileEncryptionService.LoadPasswords(MasterPassword);

                    IsLoggedIn = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(UnableToLoginLocalisation,
                                    ErrorLocalisation,
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }
            else
            {
                IsLoggedIn = false;
                MasterPassword = string.Empty;
                Passwords.Clear();
            }
        }

        private async Task SaveChangesCommandHandler()
        {
            string feedbackMessage = string.Empty;

            foreach(IPasswordModel password in Passwords)
            {
                if (password.Name.Contains(FileDefinitions.DATABASE_PROPERTY_SEPARATOR_CHARACTER) ||
                    password.Login.Contains(FileDefinitions.DATABASE_PROPERTY_SEPARATOR_CHARACTER) ||
                    password.Password.Contains(FileDefinitions.DATABASE_PROPERTY_SEPARATOR_CHARACTER))
                {
                    feedbackMessage += (string.Format(InvalidCharacterFoundLocalisation,
                        password.Name,
                        FileDefinitions.DATABASE_PROPERTY_SEPARATOR_CHARACTER));
                }
            }

            if (feedbackMessage != string.Empty)
            {
                ShowError(feedbackMessage);
                return;
            }

            try
            {
                await Task.Run(() => this.fileEncryptionService.SavePasswords(Passwords, masterPassword));

                MessageBox.Show(ChangesSavedSuccessfullyLocalisation,
                                ChangesSavedLocalisation,
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }

        }

        private async Task RevertChangesCommandHandler()
        {
            try
            {
                await Task.Run(() => Passwords = this.fileEncryptionService.LoadPasswords(MasterPassword));
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void CopyCommandHandler()
        {
            if (SelectedPassword != null)
            {
                Clipboard.SetText(SelectedPassword.Password);

                MessageBox.Show(PasswordCopiedToClipboardLocalisation,
                                PasswordCopiedLocalisation,
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
        }

        private void NewCommandHandler()
        {
            IPasswordModel newItem = new PasswordModel
            {
                Name = NewLocalisation,
                Login = string.Empty,
                Password = string.Empty
            };

            Passwords.Add(newItem);
            SelectedPassword = newItem;
        }

        private void DeleteCommandHandler()
        {
            if (SelectedPassword != null)
            {
                Passwords.Remove(SelectedPassword);
                SelectedPassword = null;
            }
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message,
                            ErrorLocalisation,
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
        }

        #endregion
    }
}
