namespace proyectop.Data.Models;


public class Usuario
{
    public int UsuarioId { get; set; }
    public int RoleId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public byte[] PasswordByte { get; set; }
    public virtual Role Role { get; set; }
    public string status { get; set; }
} 