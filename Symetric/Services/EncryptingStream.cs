using System.IO;
using Common;
using Security.App;
using Security.Contracts;
using Security.Utilities;
using Security.Utilities.Base;

namespace Security.Services
{
    public class EncryptingStream : SecureStream
    {
        private EncryptingStream(Stream baseStream, PrivateKey key, AccessMode accessMode) : base(baseStream, key, accessMode, EncryptionMode.Encrypt) { }

        public static EncryptingStreamReader Read(Stream baseStream, PrivateKey key) => new EncryptingStreamReader(new EncryptingStream(baseStream, key, AccessMode.Read));
        public static EncryptingStreamReader Read(string stream, PrivateKey key) => Read(new MemoryStream(stream.ToBytes()), key);

        public static EncryptingStreamWriter Write(string stream, PrivateKey key) => Write(new MemoryStream(stream.ToBytes()), key);
        public static EncryptingStreamWriter Write(Stream baseStream, PrivateKey key) => new EncryptingStreamWriter(new EncryptingStream(baseStream, key, AccessMode.Write));
    }
}