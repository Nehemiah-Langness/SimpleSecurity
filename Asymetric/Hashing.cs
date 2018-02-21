using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Common;
using Security.Contracts;

namespace Security
{
    public static class Hashing
    {
        public static Level SaltLevel = Level.Normal;
        public static Hash Hash(string value, Level? saltLevel = null) => new Hash(value, saltLevel);

        internal static byte[] GetSalt(Level keyLength)
        {
            var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[(int)keyLength];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return randomBytes;
        }

        internal static Data GetSaltedHash(string value, IEnumerable<byte> salt)
        {
            var plainTextWithSaltBytes = salt.Concat(value.ToBytes());
            using (var algorithm = new SHA256Managed())
                return new Data(algorithm.ComputeHash(plainTextWithSaltBytes.ToArray()));
        }
    }
}