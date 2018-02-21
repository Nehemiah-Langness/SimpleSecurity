using System.Collections.Generic;
using System.Linq;
using Common;
using Security.Contracts;

namespace Security
{
    public class Hash
    {
        private readonly IReadOnlyList<byte> _salt;
        private readonly Data _saltedHash;

        public Hash(string order, Level? saltiness = null) 
            : this(order, Hashing.GetSalt(saltiness ?? Hashing.SaltLevel)) { }

        private Hash(string order, IReadOnlyList<byte> salt)
        {
            _salt = salt;
            _saltedHash = Hashing.GetSaltedHash(order, _salt);
        }

        public Hash(IReadOnlyList<byte> saltedHash, Level? saltiness = null)
        {
            saltiness = saltiness ?? Hashing.SaltLevel;

            var splitValues = saltedHash.Bisect((int) saltiness);
            _salt = splitValues.FirstSegment;
            _saltedHash = new Data(splitValues.SecondSegment);
        }

        public bool Compare(string hash) => Compare(new Hash(hash, _salt));

        public bool Compare(Hash hash) => _saltedHash.ToArray().SlowEquals(hash._saltedHash.ToArray());

        public byte[] ToArray() => _salt.Concat(_saltedHash.ToArray()).ToArray();

        public override string ToString() => _saltedHash.ToString();
    }
}