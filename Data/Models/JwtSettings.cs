namespace proyectop.Data.Models;

public class JwtSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SecretKey { get; set; }
    public Double ExpirationMinutes { get; set; }
    
}