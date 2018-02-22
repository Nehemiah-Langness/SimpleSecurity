using System.Threading.Tasks;
using Security.App;
using Security.Base;
using Security.Common;
using Security.Services;

namespace Security.Utilities
{
    public class DecryptingStreamWriter : DataStreamWriter
    {

        public DecryptingStreamWriter(DecryptingStream stream) : base(stream) { }

        public override void Write(string value) => Write(new EncryptedString(value));

        public void Write(EncryptedString text) => WriteAsync(text).Wait();

        public override Task WriteAsync(string value) => WriteAsync(new EncryptedString(value));

        public async Task WriteAsync(EncryptedString text) => await WriteDataAsync(text.ToArray());

        protected async Task WriteDataAsync(Data data)
        {
            var toWrite = data.ToArray();
            await BaseStream.WriteAsync(toWrite, 0, toWrite.Length);
        }
    }
}