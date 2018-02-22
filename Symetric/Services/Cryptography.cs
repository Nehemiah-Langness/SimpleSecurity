using System.Security.Cryptography;
using System.Threading.Tasks;
using Security.App;

namespace Security.Services
{
    public static class Cryptography
    {
        public static PrivateKey GenerateKey(string alias = null)
        {
            using (var algorithm = Rijndael.Create())
                return new PrivateKey(alias, algorithm.Key, algorithm.IV);
        }

        public static EncryptedString Encrypt(string text, PrivateKey key)
            => EncryptAsync(text, key).Result;

        public static async Task<EncryptedString> EncryptAsync(string text, PrivateKey key)
        {
            using (var stream = EncryptingStream.Read(text, key))
                return await stream.ReadToEndAsync();
        }

        public static string Decrypt(EncryptedString text, PrivateKey key)
            => DecryptAsync(text, key).Result;

        public static async Task<string> DecryptAsync(EncryptedString text, PrivateKey key)
        {
            using (var stream = DecryptingStream.Read(text, key))
                return await stream.ReadToEndAsync();
        }
    }
}