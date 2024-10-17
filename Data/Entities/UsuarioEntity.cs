using proyectop.Data.Models;

namespace proyectop.Data.Entities;


public class UsuarioEntity
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