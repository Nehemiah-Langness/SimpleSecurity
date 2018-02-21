using System;
using System.IO;
using System.Security.Cryptography;
using Security.App;
using Security.Contracts;

namespace Security.Utilities.Base
{
    public abstract class SecureStream : IDisposable
    {
        protected readonly CryptoStream Stream;
        private readonly Rijndael _algorithm;
        private readonly Stream _baseStream;
        internal Stream BaseStream => Stream;

        protected SecureStream(Stream baseStream, PrivateKey key, AccessMode accessMode, EncryptionMode encryptionMode)
        {
            _baseStream = baseStream;
            _algorithm = GetAlgorithm(key);

            Stream = new CryptoStream(
                _baseStream,
                encryptionMode == EncryptionMode.Decrypt ? _algorithm.CreateDecryptor() : _algorithm.CreateEncryptor(), 
                accessMode == AccessMode.Read ? CryptoStreamMode.Read : CryptoStreamMode.Write
            );
        }

        private static Rijndael GetAlgorithm(PrivateKey key)
        {
            var algorithm = Rijndael.Create();
            algorithm.Key = key.Key;
            algorithm.IV = key.InitializationVector;
            algorithm.Padding = PaddingMode.Zeros;
            return algorithm;
        }

        public virtual void Dispose()
        {
            Stream?.Dispose();
            _baseStream?.Dispose();
            _algorithm?.Dispose();
        }
    }
}