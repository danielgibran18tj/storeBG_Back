using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using proyectop.Data.Models;
using proyectop.Services;

namespace proyectop.Controllers;

[Route("api/")]
public class ProductController: ControllerBase
{
    private ProductService _productService;
    
    
    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    
    [HttpGet]
    [Route("getProducts")]
    public IActionResult Get(string? categoryId)
    {
        
        if (int.TryParse(categoryId, out int categoryIdInt))
        {
            Console.WriteLine("categoryId " + categoryId);
            IEnumerable<ProductoEntity> productos = _productService.FindProductByCategory(categoryIdInt);
            return Ok(productos);
        }
        
        return Ok(_productService.Get());   
    }
    
    
    [HttpPost]
    [Route("getProductsSelect")]
    public IActionResult GetProductsSelect([FromBody] ProductSelectRequest request)
    {
        if (request == null || request.SelectedProductIds == null || !request.SelectedProductIds.Any())
        {
            return BadRequest("No products selected.");
        }

        Console.WriteLine("llegamos bien ");
        var products = _productService.FindProductsByIds(request.SelectedProductIds);
        return Ok(products);   
    }
    
    
    [HttpGet]
    [Route("getProductId/{id}")]
    public IActionResult ProductById([FromRoute] int id)
    {
        return Ok(_productService.productById(id));
    }
    
    
    [HttpPost]
    [Route("createProduct")]
    public IActionResult CreateProduct([FromBody] ProductoEntity productoEntity)
    {
        var response = _productService.createProduct(productoEntity);
        return Ok(response);
    }
    
    
    [HttpPost]
    [Route("createCategory")]
    public IActionResult CreateCategory([FromBody] Category category)
    {
        var response = _productService.createCategory(category);
        return Ok(response);
    }


    [HttpGet]
    [Route("categories")]
    public IActionResult GetCategory()
    {
        return Ok(_productService.getCategories());
    }
    

}