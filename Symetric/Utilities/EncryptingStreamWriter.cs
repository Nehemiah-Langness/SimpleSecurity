using Security.Base;
using Security.Services;

namespace Security.Utilities
{
    public class EncryptingStreamWriter : DataStreamWriter
    {
        public EncryptingStreamWriter(EncryptingStream stream) : base(stream) { }
    }
}