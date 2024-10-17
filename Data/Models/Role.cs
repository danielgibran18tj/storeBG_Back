using System.Text.Json.Serialization;
using proyectop.Data.Entities;

namespace proyectop.Data.Models;

public class Role
{
    public int RoleId { get; set; }
    public string Nombre { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<UsuarioEntity>? Usuarios { get; set; }
    
}