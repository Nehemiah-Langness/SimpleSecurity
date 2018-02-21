using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Bases;

namespace Common
{
#pragma warning disable 660, 661, CS0659    // GetHashCode overidden in base class

    public class Data : PassThroughList<byte[], byte>
    {
        public Data(MemoryStream data) : this(data?.ToArray()) { }
        public Data(IEnumerable<byte> data) : this(data?.ToArray()) { }
        private Data(byte[] data) : base(data ?? new byte[0]) { }

        public byte[] ToArray() => Value;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;

            if (obj is IEnumerable<byte> bytes)
                return Value.FastEquals(bytes.ToArray());

            return false;
        }

        public static bool operator ==(Data a, Data b)
        {
            if (a is null && b is null) return true;
            if (a is null || b is null) return false;

            return a.Value.FastEquals(b.Value);
        }
        public static bool operator !=(Data a, Data b) => !(a == b);

        public static implicit operator byte[] (Data data) => data?.Value;
        public static implicit operator Data(byte[] bytes) => bytes == null ? null : new Data(bytes);
        public static implicit operator Data(MemoryStream data) => data == null ? null : new Data(data);

        public override string ToString() => Value.ToBase64();
        
    }

#pragma warning restore 660, 661, CS0659
}