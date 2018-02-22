using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Security;
using Security.Common;
using Security.Services;

namespace UnitTest
{
    [TestClass]
    public class CommonTests
    {
        [TestMethod]
        public void TestBisectionThrowsArgumentOutOfRangeExceptionIfInvalidLength()
        {
            var salt = Hashing.GetSalt(Hashing.SaltLevel);

            var result = salt.Bisect((int)Hashing.SaltLevel);
            Assert.IsTrue(result.FirstSegment.Length == (int)Hashing.SaltLevel);
            Assert.IsTrue(result.SecondSegment.Length == 0);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => salt.Bisect(((int)Hashing.SaltLevel) + 1));
        }
    }
}