using System;
using System.Linq;
using Common;
using Security.Services.Files;

namespace Security.App
{
#pragma warning disable 660, 661, CS0659    // GetHashCode overidden in base class

    public class PrivateKey
    {
        public PrivateKey(string alias, byte[] key, byte[] initializationVector)
        {
            Key = key;
            InitializationVector = initializationVector;

            var id = BitConverter.ToString(Key.Take(2).Concat(InitializationVector.Take(2)).ToArray()).Replace("-", "").Last(8);
            Alias = new KeyAlias(alias, id);
        }


        public string Id => Alias.Id;
        public readonly KeyAlias Alias;

        public readonly byte[] Key;
        public readonly byte[] InitializationVector;


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (obj is PrivateKey key)
                return this == key;
            return false;
        }
        public static bool operator ==(PrivateKey a, PrivateKey b)
        {
            if (a is null && b is null) return true;
            if (a is null || b is null) return false;

            return a.Key.FastEquals(b.Key) &&
                   a.InitializationVector.FastEquals(b.InitializationVector);
        }
        public static bool operator !=(PrivateKey a, PrivateKey b) => !(a == b);
    }

#pragma warning restore 660, 661, CS0659
}