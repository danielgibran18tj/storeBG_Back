using proyectop.Data.Models;
using proyectop.Domain;

namespace proyectop.Services;

public class ProductService
{
    private IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    // traer todos los productos con estado existente
    public IEnumerable<ProductoEntity> Get()
    {
        IEnumerable<ProductoEntity> products = _productRepository.Get();
        return products;
    }

    
    public ProductoEntity productById(int id)
    {
        ProductoEntity product = _productRepository.getProductId(id);
        return product;
    }

    
    public IEnumerable<ProductoEntity> FindProductByCategory(int id)
    {
        IEnumerable<ProductoEntity> products = _productRepository.findProductsByCategory(id);

        return products;
    }

    
    public IEnumerable<ProductoEntity> FindProductsByIds(List<int> ids)
    {
        IEnumerable<ProductoEntity> products = _productRepository.findProductsByIds(ids);

        return products;    }
    
    public string createProduct(ProductoEntity productoEntity)
    {
        if (!string.IsNullOrEmpty(productoEntity.title) || !string.IsNullOrEmpty(productoEntity.price.ToString()) )
        {
            _productRepository.createProduct(productoEntity);
        }
        else
        {
            return "Error al crear Producto ";
        }
        return "Producto creado con exito";
    }

    
    public string createCategory(Category category)
    {
        if (!string.IsNullOrEmpty(category.name))
        {
            category.creationAt = DateTime.Now.ToString();
            _productRepository.createCategory(category);
            return "Categoria creada con exito";
        }
        return "Error al crear Categoria ";
    }


    public IEnumerable<Category> getCategories()
    {
        return _productRepository.getCategory();
    }

}