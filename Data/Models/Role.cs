using System.Text.Json.Serialization;
using BG.Data.Entities;

namespace BG.Data.Models;

public class Role
{
    public int RoleId { get; set; }
    public string Nombre { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<UsuarioEntity>? Usuarios { get; set; }
    
}