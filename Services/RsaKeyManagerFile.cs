using System.Security.Cryptography;

namespace proyectop.Services;

public class RsaKeyManagerFile
{
    private static readonly Lazy<RsaKeyManagerFile> _instance = new Lazy<RsaKeyManagerFile>(() => new RsaKeyManagerFile());

    public static RsaKeyManagerFile Instance => _instance.Value;

    public RSAParameters PublicKey { get; private set; }
    public RSAParameters PrivateKey { get; private set; }

    private const string PublicKeyPath = "path_to_store_public_key.xml";
    private const string PrivateKeyPath = "path_to_store_private_key.xml";

    private RsaKeyManagerFile()
    {
        if (File.Exists(PublicKeyPath) && File.Exists(PrivateKeyPath))
        {
            // Cargar llaves existentes
            PublicKey = LoadKey(PublicKeyPath);
            PrivateKey = LoadKey(PrivateKeyPath);
        }
        else
        {
            // Generar nuevas llaves
            using (RSA rsa = RSA.Create())
            {
                PublicKey = rsa.ExportParameters(false);
                PrivateKey = rsa.ExportParameters(true);

                // Guardar llaves en archivos
                SaveKey(PublicKeyPath, PublicKey, false);
                SaveKey(PrivateKeyPath, PrivateKey, true);
            }
        }
    }

    private void SaveKey(string path, RSAParameters key, bool includePrivateParameters)
    {
        using (RSA rsa = RSA.Create())
        {
            rsa.ImportParameters(key);
            string xmlString = rsa.ToXmlString(includePrivateParameters);

            File.WriteAllText(path, xmlString);
        }
    }

    private RSAParameters LoadKey(string path)
    {
        using (RSA rsa = RSA.Create())
        {
            string xmlString = File.ReadAllText(path);
            rsa.FromXmlString(xmlString);
            return rsa.ExportParameters(xmlString.Contains("<D>")); // Si contiene <D>, es una clave privada
        }
    }
}