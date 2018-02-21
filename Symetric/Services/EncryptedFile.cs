using System.IO;
using System.Threading.Tasks;
using Security.App;
using Security.Base;

namespace Security.Services
{
    public static class EncryptedFile
    {
        public static string Decrypt(string filepath, PrivateKey key) => DecryptAsync(filepath, key).Result;
        public static EncryptedString Read(string filepath) => ReadAsync(filepath).Result;
        public static void Write(string filepath, string data, PrivateKey key) => WriteAsync(filepath, data, key).Wait();
        public static void Append(string filepath, string data, PrivateKey key) => AppendAsync(filepath, data, key).Wait();

        public static async Task<string> DecryptAsync(string filepath, PrivateKey key)
        {
            using (var encryptedStream = DecryptingStream.Read(File.OpenRead(filepath), key))
                return await encryptedStream.ReadToEndAsync();
        }

        public static async Task<EncryptedString> ReadAsync(string filepath)
        {
            using (var fileStream = File.OpenRead(filepath))
            using (var reader = new DataStreamReader(fileStream))
                return new EncryptedString(await reader.ReadDataToEndAsync());
        }

        public static async Task WriteAsync(string filepath, string data, PrivateKey key)
        {
            using (var encryptedStream = EncryptingStream.Write(File.OpenWrite(filepath), key))
                await encryptedStream.WriteAsync(data);
        }

        public static async Task AppendAsync(string filepath, string data, PrivateKey key)
        {
            using (var encryptedStream = EncryptingStream.Write(File.Open(filepath, FileMode.Append, FileAccess.Write), key))
                await encryptedStream.WriteAsync(data);
        }
    }
}