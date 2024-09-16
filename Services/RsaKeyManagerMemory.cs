using System.Security.Cryptography;

namespace proyectop.Services;

public class RsaKeyManagerMemory
{
    private static RsaKeyManagerMemory _instance;
    private static readonly object _lock = new object();
    public RSAParameters PublicKey { get; private set; }
    public RSAParameters PrivateKey { get; private set; }

    // Constructor privado para evitar instanciación externa
    private RsaKeyManagerMemory()
    {
        GenerateAndStoreKeys();
    }

    // Método para obtener la instancia única
    public static RsaKeyManagerMemory Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new RsaKeyManagerMemory();
                }
                return _instance;
            }
        }
    }

    private void GenerateAndStoreKeys()
    {
        using (RSA rsa = RSA.Create())
        {
            PublicKey = rsa.ExportParameters(false);  // Solo clave pública
            PrivateKey = rsa.ExportParameters(true);  // Clave privada completa
        }
    }
}