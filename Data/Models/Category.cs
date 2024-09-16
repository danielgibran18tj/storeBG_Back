using System.Text.Json.Serialization;

namespace proyectop.Data.Models;

public class Category
{
    public int? id { get; set; }
    public string name { get; set; }
    public string images { get; set; }
    public string creationAt { get; set; }
    public string updatedAt { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<Producto>? Productos { get; set; }
}