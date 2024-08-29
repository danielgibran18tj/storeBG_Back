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

    public IEnumerable<ProductoEntity> Get()
    {
        return _context.Producto
            .Include(c => c.category);
    }

    public IEnumerable<ProductoEntity> findProductsByIds(List<int> ids)
    {
        return _context.Producto
            .Include(p => p.category)
            .Where(p => ids.Contains(p.id)) 
            .ToList();
    }

    public ProductoEntity getProductId(int id)
    {
        return _context.Producto
            .Include(c => c.category)
            .FirstOrDefault(i => i.id == id) ?? throw new InvalidOperationException();
    }

    public void createProduct(ProductoEntity productoEntity)
    {
        _context.Producto.Add(productoEntity);
        _context.SaveChanges();
    }

    public IEnumerable<ProductoEntity> findProductsByCategory(int id)
    {
        return _context.Producto
            .Include(c => c.category)
            .Where(p => p.categoryId == id);
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


    public void deleteProduct(ProductoEntity productoEntity)
    {
        _context.Producto.Update(productoEntity);
        _context.SaveChanges();
    }

}