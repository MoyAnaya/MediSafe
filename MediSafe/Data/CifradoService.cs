
using System.Security.Cryptography;
using System.Text;

namespace MediSafe.Data
{
    public class CifradoService
    {
        private readonly byte[] _clave;

        public CifradoService(IConfiguration configuration)
        {
            var claveConfigurada =
                configuration["Cifrado:Clave"];

            if (string.IsNullOrWhiteSpace(claveConfigurada))
            {
                throw new InvalidOperationException(
                    "No se encontró la clave de cifrado.");
            }

            _clave = Convert.FromBase64String(claveConfigurada);

            if (_clave.Length != 32)
            {
                throw new InvalidOperationException(
                    "La clave de cifrado debe tener 32 bytes.");
            }
        }

        public string Cifrar(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return texto;

            byte[] nonce =
                RandomNumberGenerator.GetBytes(12);

            byte[] datos =
                Encoding.UTF8.GetBytes(texto);

            byte[] cifrado =
                new byte[datos.Length];

            byte[] etiqueta =
                new byte[16];

            using (var aes = new AesGcm(_clave, 16))
            {
                aes.Encrypt(
                    nonce,
                    datos,
                    cifrado,
                    etiqueta);
            }

            byte[] resultado =
                new byte[nonce.Length + etiqueta.Length + cifrado.Length];

            Buffer.BlockCopy(
                nonce, 0, resultado, 0, nonce.Length);

            Buffer.BlockCopy(
                etiqueta, 0, resultado, nonce.Length, etiqueta.Length);

            Buffer.BlockCopy(
                cifrado, 0, resultado,
                nonce.Length + etiqueta.Length,
                cifrado.Length);

            return "MS1:" + Convert.ToBase64String(resultado);
        }

        public string Descifrar(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return texto;

            // Permite leer temporalmente registros antiguos
            // que todavía no han sido cifrados.
            if (!texto.StartsWith("MS1:"))
                return texto;

            byte[] datos =
                Convert.FromBase64String(texto.Substring(4));

            if (datos.Length < 28)
            {
                throw new CryptographicException(
                    "El contenido cifrado no es válido.");
            }

            byte[] nonce = datos[..12];
            byte[] etiqueta = datos[12..28];
            byte[] cifrado = datos[28..];

            byte[] resultado =
                new byte[cifrado.Length];

            using (var aes = new AesGcm(_clave, 16))
            {
                aes.Decrypt(
                    nonce,
                    cifrado,
                    etiqueta,
                    resultado);
            }

            return Encoding.UTF8.GetString(resultado);
        }
    }
}
