using System;
using Security.Contracts;

namespace Security.Services
{
    public static class Extensions
    {
        public static Hash AsHash(this string text, Level? saltLevel = null)
        {
            return new Hash(Convert.FromBase64String(text), saltLevel ?? Hashing.SaltLevel);
        }
    }
}
