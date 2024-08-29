using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using proyectop.Data.Models;

public class ProductoEntity
{
    public int id { get; set; }
    public string title { get; set; }
    public decimal price { get; set; }
    public string description { get; set; }
    
    public string Images { get; set; }
    public int categoryId { get; set; }
    public Category category { get; set; }
}


public class ProductSelect
{
    public int Id { get; set; }
    public int ProductoId { get; set; } 
}

public class ProductSelectRequest
{
    public List<int> SelectedProductIds { get; set; }
}