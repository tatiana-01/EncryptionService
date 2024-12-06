using System.Security.Cryptography;
using System.Text;

namespace EncryptionServiceApi.Service
{

    public interface IEncrypt
    {
        public string GetRSA();
        public string Decrypt(string encryptedClientKey);
    }

    public class Encrypt: IEncrypt
    {

        private static RSA _rsa;

        public string GetRSA()
        {
           _rsa = RSA.Create();
            var publicKey = Convert.ToBase64String(_rsa.ExportRSAPublicKey());
            return publicKey;
        }

        public string Decrypt(string encryptedClientKey)
        {
            // Decifrar la llave del cliente
            byte[] encryptedBytes = Convert.FromBase64String(encryptedClientKey);
            byte[] decryptedBytes = _rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);
            string clientKey = Encoding.UTF8.GetString(decryptedBytes);

            // Cifrar el mensaje "Hola mundo" con la llave del cliente
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(clientKey);
                aes.GenerateIV();

                // Cifrar el mensaje
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (var ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length); // Escribir el IV al inicio
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] messageBytes = Encoding.UTF8.GetBytes("Hola mundo");
                        cs.Write(messageBytes, 0, messageBytes.Length);
                    }
                    return Convert.ToBase64String(ms.ToArray()); // Retornar el mensaje cifrado
                }
            }
        }
    }
}

