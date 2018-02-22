using System.Collections.Generic;
using System.Linq;
using Security.Common;
using Security.Contracts;
using Security.Services;

namespace Security
{
    public class Hash
    {
        private readonly IReadOnlyList<byte> _salt;
        private readonly Data _saltedHash;
        private IEnumerable<byte> AllBytes => _salt.Concat(_saltedHash.ToArray());

        public Hash(string text, Level? saltiness = null) 
            : this(text, Hashing.GetSalt(saltiness ?? Hashing.SaltLevel)) { }

        private Hash(string text, IReadOnlyList<byte> salt)
        {
            _salt = salt;
            _saltedHash = Hashing.GetSaltedHash(text, _salt);
        }

        public Hash(IReadOnlyList<byte> saltedHash, Level? saltiness = null)
        {
            saltiness = saltiness ?? Hashing.SaltLevel;

            var splitValues = saltedHash.Bisect((int) saltiness);
            _salt = splitValues.FirstSegment;
            _saltedHash = new Data(splitValues.SecondSegment);
        }

        public bool Compare(string text) => Compare(new Hash(text, _salt));

        public bool Compare(Hash hash) => _saltedHash.ToArray().SlowEquals(hash._saltedHash.ToArray());

        /// <summary>
        /// Returns a base 64 string of the hashed value.
        /// </summary>
        public override string ToString() => _saltedHash.ToBase64();

        /// <summary>
        /// Returns all bytes in this hashing context, including salt.
        /// </summary>
        public byte[] ToArray() => AllBytes.ToArray();

        /// <summary>
        /// Returns a base 64 string of all data needed to rebuild this hashing context, including salt.
        /// </summary>
        public string Serialize() => AllBytes.ToBase64();
    }
}