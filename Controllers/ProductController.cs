using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using proyectop.Data.Models;
using proyectop.Services;

namespace proyectop.Controllers;

[Authorize]
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
        var userClaims = User.Claims;
        var userRol = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        if (Constants.ROL_RESTRINGIDO.Equals(userRol) || Constants.ROL_ADMINISTRADOR.Equals(userRol) )
        {
            if ( !string.IsNullOrEmpty(categoryId) ){
                Console.WriteLine("categoryId " + categoryId);
                IEnumerable<Producto> productos = _productService.FindProductByCategory(int.Parse(categoryId)); 
                return Ok(productos);
            }
        
            return Ok(_productService.Get());   
        }
        return Unauthorized($"Rol no autorizado, eres {userRol}");
    }
    
    
    [HttpGet]
    [Route("getProductId/{id}")]
    [Authorize(Roles = Constants.ROL_ADMINISTRADOR)]
    public IActionResult ProductById([FromRoute] int id)
    {
        return Ok(_productService.productById(id));
    }
    
    
    [HttpPost]
    [Route("createProduct")]
    public IActionResult CreateProduct([FromBody] Producto producto)
    {
        var response = _productService.createProduct(producto);
        return Ok(response);
    }


    [HttpPost]
    [Route("createProductMasive")]
    public IActionResult CreateProductsMasivo([FromBody] List<Producto> productos)
    {
        return Ok(_productService.createProductMasivo(productos));
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
    
    
    [HttpPut]
    [Route("product/update")]
    public IActionResult Update( [FromBody] Producto producto)
    {
        return Ok(_productService.updateProduct(producto));
    }
    
    
    [HttpDelete]
    [Route("deletedProduct/{id}")]
    public IActionResult Delete([FromRoute] int id)
    {
        return Ok(_productService.deleteLogicProduct(id));
    }
    

}