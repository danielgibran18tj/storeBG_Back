using proyectop.Data.Models;

namespace proyectop.Domain;

public interface IProductRepository
{
    IEnumerable<Producto> Get();

    Producto getProductId(int id);
    void createProduct(Producto producto);
    void updateProduct(Producto producto);

    IEnumerable<Producto> findProductsByCategory(int id);
    
    IEnumerable<Category> getCategory();
    void createCategory(Category category);
    
    void deleteProduct(Producto producto);

    Producto findById(int? id);

}