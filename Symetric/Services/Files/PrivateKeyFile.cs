using System;
using System.IO;
using Security.App;
using Security.Exceptions;

namespace Security.Services.Files
{
    public class PrivateKeyFile : BinaryFile
    {
        private const byte CurrentVersion = 2;

        private PrivateKeyFile(Stream stream) : base(stream) { }

        private PrivateKey Load()
        {
            var version = ReadByte();
            switch (version)
            {
                case 1:
                {
                    IntSize = BitConverter.GetBytes(0).Length;
                    var key = ReadByteArray();
                    var iv = ReadByteArray();

                    CheckFullyRead();
                    return new PrivateKey(null, key, iv);
                }

                case 2:
                {
                    IntSize = ReadByte();
                    var alias = ReadString();
                    var key = ReadByteArray();
                    var iv = ReadByteArray();

                    CheckFullyRead();
                    return new PrivateKey(alias, key, iv);
                }

                default:
                    throw new InvalidOperationException($"Unsupported key version: {version}");
            }
        }
        private void Save(PrivateKey key)
        {
            WriteByte(CurrentVersion);
            WriteByte(SystemIntSize);
            WriteString(key.Alias);
            WriteByteArray(key.Key);
            WriteByteArray(key.InitializationVector);
        }

        private void CheckFullyRead()
        {
            if (!EndOfStream)
                throw new FileCorruptException("Key data did not fill the file");
        }

        public static PrivateKey Load(string path)
        {
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                return new PrivateKeyFile(stream).Load();
        }

        public static void Save(string path, PrivateKey key)
        {
            using (var stream = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.Read))
                new PrivateKeyFile(stream).Save(key);
        }
    }
}