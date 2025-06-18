namespace PasswordManager.Interfaces
{
    public interface IPasswordModel
    {
        string Name { get; set; }
        string Login { get; set; }
        string Password { get; set; }
    }
}
