using System.Text.Json.Serialization;

namespace proyectop.Data.Models;

public class Category
{
    public int? id { get; set; }
    public string name { get; set; }
    public string creationAt { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<ProductoEntity>? Productos { get; set; }
}