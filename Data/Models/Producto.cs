using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using proyectop.Data.Models;

public class Producto
{
    public int? id { get; set; }
    public string title { get; set; }
    public decimal price { get; set; }
    public string description { get; set; }
    
    // Relación uno a muchos con imágenes
    public ICollection<Imagen> images { get; set; }
    
    [NotMapped]
    public string[] listImages { get; set; }
    public int stock { get; set; }
    public string creationAt { get; set; }
    public int categoryId { get; set; }
    public Category category { get; set; }
    public string status { get; set; }
}


public class Imagen
{
    public int ImagenId { get; set; } // Clave primaria
    public int ProductoId { get; set; } // Clave foránea para el producto
    [JsonIgnore]
    public Producto Producto { get; set; }
    
    
    
    public string imagenString { get; set; } // Almacena la cadena
}