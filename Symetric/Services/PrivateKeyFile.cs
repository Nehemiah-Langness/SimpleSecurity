using System;
using System.Diagnostics;
using System.IO;
using Security.App;
using Security.Exceptions;

namespace Security.Services
{
    public static class PrivateKeyFile
    {
        private static readonly int IntSize = BitConverter.GetBytes(0).Length;
        private const byte CurrentVersion = 1;

        public static PrivateKey Load(string path)
        {
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var version = stream.ReadByte();
                switch (version)
                {
                    case 1:
                    {
                        var key = ReadByteArray(stream);
                        var iv = ReadByteArray(stream);

                        if (stream.Length != stream.Position)
                            throw new FileCorruptException("Key data did not fill the file");

                        return new PrivateKey(key, iv);
                    }

                    default:
                        throw new InvalidOperationException($"Unsupported key version: {version}");
                }
            }
        }

        public static void Save(string path, PrivateKey key)
        {
            using (var stream = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                stream.WriteByte(CurrentVersion);
                WriteByteArray(key.Key, stream);
                WriteByteArray(key.InitializationVector, stream);
            }
        }

        private static void WriteByteArray(byte[] data, Stream stream)
        {
            var count = BitConverter.GetBytes(data.Length);
            Debug.Assert(count.Length == IntSize);

            stream.Write(count, 0, count.Length);
            stream.Write(data, 0, data.Length);
            stream.Flush();
        }

        private static byte[] ReadByteArray(Stream stream)
        {
            var size = new byte[IntSize];
            var bytesRead = stream.Read(size, 0, IntSize);
            Debug.Assert(bytesRead == IntSize);

            var count = BitConverter.ToInt32(size, 0);

            var data = new byte[count];
            bytesRead = stream.Read(data, 0, count);

            if (bytesRead != count)
                throw new FileCorruptException("Recorded data size does not match actual data size");

            return data;
        }
    }
}