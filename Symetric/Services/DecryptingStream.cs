using System.IO;
using Security.App;
using Security.Contracts;
using Security.Utilities;
using Security.Utilities.Base;

namespace Security.Services
{
    public class DecryptingStream : SecureStream
    {
        private DecryptingStream(Stream baseStream, PrivateKey key, AccessMode accessMode)
            : base(baseStream, key, accessMode, EncryptionMode.Decrypt) { }

        public static DecryptingStreamReader Read(EncryptedString stream, PrivateKey key) => Read(new MemoryStream(stream.ToArray()), key);
        public static DecryptingStreamReader Read(Stream baseStream, PrivateKey key) => new DecryptingStreamReader(new DecryptingStream(baseStream, key, AccessMode.Read));

        public static DecryptingStreamWriter Write(EncryptedString stream, PrivateKey key) => Write(new MemoryStream(stream.ToArray()), key);
        public static DecryptingStreamWriter Write(Stream baseStream, PrivateKey key) => new DecryptingStreamWriter(new DecryptingStream(baseStream, key, AccessMode.Write));
    }
}
