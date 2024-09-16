using Microsoft.AspNetCore.Mvc;
using proyectop.Data.Models;
using proyectop.Data.Models.Request;
using proyectop.Services;

namespace proyectop.Controllers;

[Route("api/")]
public class UsuarioController : ControllerBase
{
    UsuarioServices usuarioServices;
    
    public UsuarioController(UsuarioServices service)
    {
        usuarioServices = service;
    }
    
    [HttpGet]
    [Route("user")]
    public IActionResult Get()
    {
        return Ok(usuarioServices.Get());
    }

    [HttpPost]
    [Route("createUser")]
    public IActionResult createUser([FromBody] Usuario usuario)
    {
        var response = usuarioServices.createUser(usuario);
        return Ok(response);
    }
    
    [HttpPost]
    [Route("login")]
    public IActionResult Login([FromBody] LoginRQ login)
    {
        var response = usuarioServices.Login(login);
        return Ok(response);
    }
}