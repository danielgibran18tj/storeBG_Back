using proyectop.Data.Models;

namespace proyectop.Domain;

public interface IProductRepository
{
    IEnumerable<ProductoEntity> Get();

    ProductoEntity getProductId(int id);
    void createProduct(ProductoEntity productoEntity);

    IEnumerable<ProductoEntity> findProductsByCategory(int id);
    
    IEnumerable<Category> getCategory();
    void createCategory(Category category);
    
    IEnumerable<ProductoEntity> findProductsByIds(List<int> id);
}