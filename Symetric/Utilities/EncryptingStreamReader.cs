using System.Threading.Tasks;
using Common;
using Security.App;
using Security.Base;
using Security.Services;

namespace Security.Utilities
{
    public class EncryptingStreamReader : DataStreamReader
    {

        public EncryptingStreamReader(EncryptingStream stream) : base(stream) { }

        public new EncryptedString ReadToEnd() => ReadToEndAsync().Result;

        public EncryptedString Read(int length) => ReadAsync(length).Result;

        public new async Task<EncryptedString> ReadToEndAsync() => GetString(await ReadDataToEndAsync());

        public async Task<EncryptedString> ReadAsync(int length)
        {
            var result = await ReadDataAsync(length);
            return result == null ? null : GetString(result);
        }

        public static EncryptedString GetString(Data data) => new EncryptedString(data);
    }
}