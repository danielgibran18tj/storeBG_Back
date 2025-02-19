using BG.Data.Entities;
using BG.Data.Models;
using Microsoft.AspNetCore.Mvc;
using BG.Services;

namespace BG.Controllers;

[Route("api/")]
public class UsuarioController : ControllerBase
{
    private readonly UserServices _userServices;

    public UsuarioController(UserServices service)
    {
        _userServices = service;
    }

    [HttpGet]
    [Route("user")]
    public IActionResult Get()
    {
        return Ok(_userServices.Get());
    }

    [HttpGet]
    [Route("public-key")]
    public IActionResult GetPublicKey()
    {
        var publicKey = RsaKeyManagerFile.Instance.PublicKey;
        string modulusHex = BitConverter.ToString(publicKey.Modulus).Replace("-", "").ToLower(); // Enviar en formato hexadecimal
        string exponentHex = BitConverter.ToString(publicKey.Exponent).Replace("-", "").ToLower(); // Enviar en formato hexadecimal

        return Ok(new { Modulus = modulusHex, Exponent = exponentHex });
    }

    [HttpPost]
    [Route("createUser")]
    public IActionResult createUser([FromBody] UsuarioEntity usuarioEntity)
    {
        var response = _userServices.createUser(usuarioEntity);
        return Ok(response);
    }

    [HttpPost]
    [Route("login")]
    public IActionResult Login([FromBody] LoginRq login)
    {
        Console.WriteLine(login);
        var response = _userServices.Login(login);
        return Ok(response);
    }
}