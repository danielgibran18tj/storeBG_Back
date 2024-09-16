using System.Text.Json.Serialization;

namespace proyectop.Data.Models;

public class Role
{
    public int RoleId { get; set; }
    public string Nombre { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<Usuario>? Usuarios { get; set; }
    
}