using System;
using System.Linq;
using Common;
using Common.Bases;
using Security.Utilities;

namespace Security.App
{
#pragma warning disable 660, 661, CS0659    // GetHashCode overidden in base class

    public class EncryptedString : PassThroughList<byte[], byte>
    {
        public EncryptedString() : this((byte[])null) { }

        public EncryptedString(string value) : this(value != null ? Convert.FromBase64String(value) : null) { }

        public EncryptedString(byte[] value) : base(value ?? new byte[0]) { }

        public byte[] ToArray() => Value;

        public override string ToString() => Convert.ToBase64String(Value);

        public static EncryptedString Empty => new EncryptedString();

        public static implicit operator string(EncryptedString value) => value?.ToString();

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            switch (obj)
            {
                case EncryptedString encryptedString:
                    return this == encryptedString;
                case Data data:
                    return this == new EncryptedString(data);
                case byte[] bytes:
                    return this == new EncryptedString(bytes);
                case string ascii64:
                    return this == new EncryptedString(ascii64);
            }

            return false;
        }

        public static bool operator !=(EncryptedString a, EncryptedString b) => !(a == b);
        public static bool operator ==(EncryptedString a, EncryptedString b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Value.FastEquals(b.Value);
        }

        public static EncryptedString operator +(EncryptedString a, EncryptedString b)
        {
            var skipA = a == null || a.Length < 1;
            var skipB = b == null || b.Length < 1;

            if (skipA && skipB) return new EncryptedString();

            if (skipA) return new EncryptedString(b.Value);
            if (skipB) return new EncryptedString(a.Value);

            return new EncryptedString(a.Value.Concat(b.Value).ToArray());
        }
    }

#pragma warning restore 660, 661, CS0659
}