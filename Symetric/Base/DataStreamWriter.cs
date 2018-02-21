using System.IO;
using Security.Utilities.Base;

namespace Security.Base
{
    public abstract class DataStreamWriter : StreamWriter
    {
        private readonly SecureStream _stream;

        protected DataStreamWriter(SecureStream stream) : base(stream.BaseStream)
        {
            _stream = stream;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
                _stream?.Dispose();
        }
    }
}