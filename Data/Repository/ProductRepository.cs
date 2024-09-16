using Microsoft.EntityFrameworkCore;
using proyectop.Data.Models;
using proyectop.Domain;

namespace proyectop.Data.Repository;

public class ProductRepository: IProductRepository
{
    DataBaseContext _context;
    
    public ProductRepository(DataBaseContext context)
    {
        _context = context;
    }

    public IEnumerable<Producto> Get()
    {
        return _context.Producto
            .Include(i => i.images)
            .Include(c => c.category)
            .Where(p => p.status == "A");
    }

    public Producto getProductId(int id)
    {
        return _context.Producto
            .Include(i => i.images)
            .Include(c => c.category)
            .FirstOrDefault(i => i.id == id) ?? throw new InvalidOperationException();
    }

    public void createProduct(Producto producto)
    {
        _context.Producto.Add(producto);
        _context.SaveChanges();
    }

    public void updateProduct(Producto producto)
    {
        _context.Producto.Update(producto);
        _context.SaveChanges();
    }

    public IEnumerable<Producto> findProductsByCategory(int id)
    {
        return _context.Producto
            .Include(i => i.images)
            .Include(c => c.category)
            .Where(p => p.status == "A" && p.categoryId == id);
    }

    public IEnumerable<Category> getCategory()
    {
        return _context.Categories;
    }

    public void createCategory(Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChanges();
    }


    public void deleteProduct(Producto producto)
    {
        _context.Producto.Update(producto);
        _context.SaveChanges();
    }

    public Producto findById(int? id)
    {
        return _context.Producto.Find(id) ?? throw new InvalidOperationException();
    }
}