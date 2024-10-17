namespace proyectop.Data.Models;

public class LoginRq
{
    public string EncryptedCredentials { get; set; }
}


public class Credentials
{
    public string username { get; set; }
    public string password { get; set; }
}