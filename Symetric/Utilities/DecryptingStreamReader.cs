using System.Threading.Tasks;
using Security.Base;
using Security.Services;

namespace Security.Utilities
{
    public class DecryptingStreamReader : DataStreamReader
    {
        public DecryptingStreamReader(DecryptingStream stream) : base(stream) { }

        public override string ReadLine() => base.ReadLine()?.TrimEnd('\0');

        public override string ReadToEnd() => base.ReadToEnd().TrimEnd('\0');

        public string Read(int length) => ReadAsync(length).Result;

        public override async Task<string> ReadLineAsync()
        {
            return (await base.ReadLineAsync())?.TrimEnd('\0');
        }

        public override async Task<string> ReadToEndAsync()
        {
            return (await base.ReadToEndAsync()).TrimEnd('\0');
        }

        public async Task<string> ReadAsync(int length)
        {
            var result = await ReadDataAsync(length);
            return result?.ToUtf8();
        }
    }
}