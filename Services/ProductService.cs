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
    public IEnumerable<Producto> Get()
    {
        IEnumerable<Producto> products = _productRepository.Get();
        foreach (var product in products)
        {
            List<string> stringList = new List<string>();
            foreach (var image in product.images)
            {
                stringList.Add(image.imagenString);
            }
            product.listImages = stringList.ToArray();
        }
        return products;
    }

    
    public Producto productById(int id)
    {
        Producto product = _productRepository.getProductId(id);
        List<string> stringList = new List<string>();
        
        foreach (var image in product.images)
        {
            stringList.Add(image.imagenString);
        }
        product.listImages = stringList.ToArray();
        return product;
    }

    
    public IEnumerable<Producto> FindProductByCategory(int id)
    {
        IEnumerable<Producto> products = _productRepository.findProductsByCategory(id);
        foreach (var product in products)
        {
            List<string> stringList = new List<string>();
            foreach (var image in product.images)
            {
                stringList.Add(image.imagenString);
            }
            product.listImages = stringList.ToArray();
        }

        return products;
    }

    // [
    // {
    //     "title": "Gatito",
    //     "price": 123123,
    //     "description": "No borrar",
    //     "images": [
    //     {
    //         "imagenString": "https://i.imgur.com/QrckuvZ.png"
    //     }
    //     ],
    //     "stock": 25,
    //     "creationAt": "2024-04-25T11:17:25.000Z",
    //     "categoryId": 2,
    //     "status": "A"
    // },
    // {
    //     "title": "Florecitaaas",
    //     "price": 1111,
    //     "description": "eee",
    //     "images": [
    //     {
    //         "imagenString": "https://media.admagazine.com/photos/650a9a94f7fc9e9f64e8400f/master/w_1600%2Cc_limit/flor-de-maga-caracteristicas.jpg"
    //     },
    //     {
    //         "imagenString": "https://media.admagazine.com/photos/650a96a14e52ff077a4d305d/16:9/w_2560%2Cc_limit/flor-de-maga.jpg"
    //     }
    //     ],
    //     "stock": 35,
    //     "creationAt": "2024-04-25T11:17:39.000Z",
    //     "categoryId": 4,
    //     "status": "A"
    // }
    // ]
    public string createProductMasivo(List<Producto> productos)
    {
        string messaje = "productos creados: ";
        int productSave = 0;
        try
        {
            foreach (var product in productos)
            {
                product.id = null;
                product.images = new List<Imagen>();
                foreach (var image in product.listImages)
                {
                    var img = new Imagen
                    {
                        imagenString = image
                    };
                    product.images.Add(img);
                }
                createProduct(product);
                productSave++;
            }
            
            return (messaje + productSave);
        }
        catch (Exception e)
        {
            Console.WriteLine(messaje + productSave);
            Console.WriteLine(e);
            throw;
        }
    }
    
    
    // {
    //     "title": "Classic Blue Baseball Cap",
    //     "price": 86,
    //     "description": "Top off your casual look with our Classic Blue Baseball Cap",
    //     "imagenes": [
    //     {
    //         "imagenString": "https://i.imgur.com/KeqG6r4.jpeg"
    //     },
    //     {
    //         "imagenString": "https://i.imgur.com/xGQOw3p.jpeg"
    //     },
    //     {
    //         "imagenString": "https://i.imgur.com/oO5OUjb.jpeg"
    //     }
    //     ],
    //     "stock": 43,
    //     "categoryId": 1
    // }
    public string createProduct(Producto producto)
    {
        if (!string.IsNullOrEmpty(producto.title) || !string.IsNullOrEmpty(producto.price.ToString()) )
        {
            // producto.creationAt = DateTime.Now.ToString();
            producto.status = "A";
            _productRepository.createProduct(producto);
        }
        else
        {
            return "Error al crear Producto ";
        }
        return "Producto creado con exito";
    }

    // {
    //     "name": "string",
    //     "images": "string",
    // }
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
    
    
    // {
    //     "id": 1,
    //     "title": "Classic Blue Baseball Cap",
    //     "price": 44,
    //     "description": "Top off your casual look with our Classic Blue Baseball Cap.",
    //     "stock": 11,
    //     "creationAt": "24/04/2024 17:42:30",
    //     "categoryId": 1,
    //     "status": "A"
    // }
    public Producto updateProduct(Producto newProduct)
    {
        try
        {
            _productRepository.updateProduct(newProduct);
            if (newProduct.id == null)
            {
                throw new Exception();
            }
            return _productRepository.findById(newProduct.id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new InvalidOperationException("Producto no encontrado.");
        }
    }
    
    
    public string deleteLogicProduct(int id)
    {
        var producto = _productRepository.findById(id);

        if (producto != null)
        {
            producto.status = "I";
            _productRepository.deleteProduct(producto);
            return "producto eliminado logicamente";
        }
        return "producto no encontrado";
    }
}