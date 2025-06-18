using PasswordManager.Interfaces;

namespace PasswordManager
{
    public class PasswordModel: IPasswordModel
    {
        public string Name {  get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
