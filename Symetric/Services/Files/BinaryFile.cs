using System;
using System.IO;
using Common;
using Security.Exceptions;

namespace Security.Services.Files
{
    public class BinaryFile
    {
        protected static byte SystemIntSize => (byte)BitConverter.GetBytes(0).Length;

        protected readonly Stream Stream;
        protected int IntSize = SystemIntSize;

        public BinaryFile(Stream stream)
        {
            Stream = stream;
        }

        protected void WriteByte(byte value) => Stream.WriteByte(value);
        protected int ReadByte() => Stream.ReadByte();

        protected void WriteInt(int value)
        {
            var count = BitConverter.GetBytes(value);
            Stream.Write(count, 0, count.Length);
        }
        protected int ReadInt()
        {
            var size = new byte[IntSize];
            Stream.Read(size, 0, IntSize);

            return BitConverter.ToInt32(size, 0);
        }

        protected void WriteString(string data) => WriteByteArray(data.ToBytes());
        protected string ReadString() => ReadByteArray().ToUtf8();

        protected void WriteByteArray(byte[] data)
        {
            WriteInt(data.Length);
            Stream.Write(data, 0, data.Length);
            Stream.Flush();
        }
        protected byte[] ReadByteArray()
        {
            var count = ReadInt();

            var data = new byte[count];
            if (Stream.Read(data, 0, count) != count)
                throw new FileCorruptException("Recorded data size does not match actual data size");

            return data;
        }

        protected bool EndOfStream => Stream.Length == Stream.Position;
    }
}