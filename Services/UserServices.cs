using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using proyectop.Data.Models;
using proyectop.Domain;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using proyectop.Data.Entities;
using proyectop.Data.Models.Response;


namespace proyectop.Services;

public class UserServices
{

    IUsuariosRepository _usuariosRepository;
    private readonly IConfiguration _configuration;
    private readonly JwtSettings _jwtSettings;
    
    public UserServices(IUsuariosRepository usuariosRepository, IConfiguration configuration, IOptions<JwtSettings> jwtSettings)
    {
        _usuariosRepository = usuariosRepository;
        _configuration = configuration;
        _jwtSettings = jwtSettings.Value;
    }

    public IEnumerable<UsuarioEntity> Get()
    {
        return _usuariosRepository.Get();
    }

    public string createUser(UsuarioEntity usuarioEntity)
    {
        var user = _usuariosRepository.VerifyIfUserExist(usuarioEntity.Username);

        if (user == null)
        {
            if (!string.IsNullOrEmpty(usuarioEntity.Username) || !string.IsNullOrEmpty(usuarioEntity.Email) || !string.IsNullOrEmpty(usuarioEntity.Password))
            {
                usuarioEntity.PasswordByte = EncryptAsymmetric(usuarioEntity.Password , RsaKeyManagerFile.Instance.PublicKey);
                usuarioEntity.Password = Encrypt(usuarioEntity.Password , GenerateKey());
                usuarioEntity.status = "Activo";
                _usuariosRepository.createUser(usuarioEntity);
            }
            else
            {
                return "Todos los campos son requeridos";
            }
        }
        else
        {
            throw new Exception("El usuario ya existe");
        }
        return "Usuario creado con exito";
    }

    public LoginRS Login(LoginRq login)
    {
        var encryptedCredentials = Convert.FromBase64String(login.EncryptedCredentials);
        // Desencriptar las credenciales con la clave privada
        var decryptedCredentials = DecryptAsymmetric(encryptedCredentials, RsaKeyManagerFile.Instance.PrivateKey);
        Console.WriteLine("decryptedCredentials: " + decryptedCredentials);
        var credentials = JsonConvert.DeserializeObject<Credentials>(decryptedCredentials);

        var user = _usuariosRepository.GetUserLogin(credentials);
        if (user == null)
        {
            throw new Exception("Usuario no encontrado");
        }

        var passwordDecrypSym = user.Password != null ? Decrypt(user.Password, GenerateKey()) : "";
        var passwordDecrypAsy = user.PasswordByte != null ? DecryptAsymmetric(user.PasswordByte, RsaKeyManagerFile.Instance.PrivateKey) : "";

        if ( !(credentials.password.Equals(passwordDecrypSym) || credentials.password.Equals(passwordDecrypAsy)))
        {
            throw new Exception("Contraseña incorrecta");
        }
        
        LoginRS token = new LoginRS();
        var jwtToken = GenerateJwtToken(user);
        token.Token = jwtToken ?? throw new Exception("Error al generar el token JWT");
        token.Role = user.Role.Nombre;
        
        return token;
    }
    
    
    private string GenerateJwtToken(UsuarioEntity user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UsuarioId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("role", user.Role.Nombre),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtSettings.ExpirationMinutes),    // tiempo de valides del token
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    
    private static string Encrypt(string plainText, byte[] key)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.GenerateIV();    // Genera un nuevo IV para cada cifrado
            byte[] iv = aesAlg.IV;

            aesAlg.Mode = CipherMode.CBC; // Modo de cifrado CBC
            aesAlg.Padding = PaddingMode.PKCS7; // Padding para ajustar a bloques

            using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
            using (var msEncrypt = new MemoryStream())
            {
                msEncrypt.Write(iv, 0, iv.Length); // Guarda el IV al principio del texto cifrado
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }

                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }
    
    private static string Decrypt( string encrypted, byte[] key)
    {
        byte[] fullCipher = Convert.FromBase64String(encrypted);

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;

            byte[] iv = new byte[aesAlg.BlockSize / 8];
            byte[] cipher = new byte[fullCipher.Length - iv.Length];

            Array.Copy(fullCipher, iv, iv.Length);
            Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

            aesAlg.IV = iv;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            using (var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
            using (var msDecrypt = new MemoryStream(cipher))
            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (var srDecrypt = new StreamReader(csDecrypt))
            {
                return srDecrypt.ReadToEnd();
            }
        }
    }
    
    private byte[] GenerateKey()
    {
        // Utiliza una clave de tamaño adecuado
        using (var sha256 = SHA256.Create())
        {
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        }
    }
    
    
    
    private static byte[] EncryptAsymmetric(string plainText, RSAParameters publicKey)
    {
        using (RSA rsa = RSA.Create(2048))
        {
            rsa.ImportParameters(publicKey);
            byte[] dataToEncrypt = Encoding.UTF8.GetBytes(plainText);
            return rsa.Encrypt(dataToEncrypt, RSAEncryptionPadding.OaepSHA256);
        }
    }
    
    private static string DecryptAsymmetric(byte[] cipherText, RSAParameters privateKey)
    {
        using (RSA rsa = RSA.Create(2048))
        {
            rsa.ImportParameters(privateKey);
            byte[] decryptedData = rsa.Decrypt(cipherText, RSAEncryptionPadding.OaepSHA256);
            return Encoding.UTF8.GetString(decryptedData);
        }
    }
    
}