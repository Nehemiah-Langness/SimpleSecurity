using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Security;
using Security.Contracts;
using Security.Services;

namespace UnitTest
{
    [TestClass]
    public class AsymmetricTests
    {
        private const string Cleartext = "I am the most secret secret in the world";
        private static Hash HashedText => Hashing.Hash(Cleartext, Level.Light);

        [TestMethod]
        public void TwoHashedValuesDiffer()
        {
            const string cleartext = "I am a secret phrase";

            var hash1 = Hashing.Hash(cleartext);
            var hashString1 = hash1.ToString();

            Assert.IsTrue(hash1.Compare(cleartext));

            var hash2 = Hashing.Hash(cleartext);
            var hashString2 = hash2.ToString();

            Assert.IsTrue(hash2.Compare(cleartext));

            Assert.IsFalse(hash1.Compare(hash2));
            Assert.IsFalse(hashString1 == hashString2);
        }

        [TestMethod]
        public void CompareHashToPreviousValue()
        {
            var hashedBytes = HashedText.ToArray();

            var reHashedValue = new Hash(hashedBytes, Level.Light);

            Assert.IsTrue(reHashedValue.Compare(Cleartext));
        }

        [TestMethod]
        public void CompareHashToBase64Representation()
        {
            var notValid = HashedText.ToString();
            var hashedString = HashedText.Serialize();

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => notValid.AsHash(Level.Light));

            var reHashedValue = hashedString.AsHash(Level.Light);

            Assert.IsTrue(reHashedValue.Compare(Cleartext));
        }
    }
}
