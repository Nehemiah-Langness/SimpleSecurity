using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Security.Utilities;
using Security.Utilities.Base;

namespace Security.Base
{
    public class DataStreamReader : StreamReader
    {
        private readonly SecureStream _stream;

        public DataStreamReader(Stream baseStream) : base(baseStream) { }
        protected DataStreamReader(SecureStream stream) : base(stream.BaseStream)
        {
            _stream = stream;
        }

        public Data ReadData(int length)
            => ReadDataAsync(length).Result;

        public Data ReadDataToEnd()
            => ReadDataToEndAsync().Result;

        public async Task<Data> ReadDataAsync(int length)
        {
            var buffer = new byte[length];
            var countRead = await BaseStream.ReadAsync(buffer, 0, length);

            if (countRead == 0) return null;

            if (countRead == length)
                return buffer;

            var result = buffer.Take(countRead).ToArray();

            return result;
        }

        public async Task<Data> ReadDataToEndAsync()
        {
            using (var memory = new MemoryStream())
            {
                await BaseStream.CopyToAsync(memory);
                return memory;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
                _stream?.Dispose();
        }
    }
}