namespace proyectop.Data.Models;

public class Producto
{
    public int id { get; set; }
    public string title { get; set; }
    public decimal price { get; set; }
    public string description { get; set; }
    
    public string Images { get; set; }
    public int categoryId { get; set; }
    public Category category { get; set; }

}