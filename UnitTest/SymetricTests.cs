using System;
using System.IO;
using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Security.App;
using Security.Services;

namespace UnitTest
{
    [TestClass]
    public class SymetricTests
    {
        private const string ClearText = "Here is the sensitive cleartext information";

        private readonly PrivateKey _key = Cryptography.GenerateKey();

        private readonly MemoryStream _mockFile = new MemoryStream();
        private readonly MemoryStream _mockEncryptedFile = new MemoryStream();

        [TestMethod]
        public void TestId()
        {
            var key = Cryptography.GenerateKey();
            Console.WriteLine(key.Id);

            KeyRing.Save(key);
            Assert.AreEqual(key, KeyRing.Load(key.Id));

            KeyRing.Remove(key);
        }

        [TestMethod]
        public void RoundTripTest()
        {
            Console.WriteLine("Cleartext: " + ClearText);

            var encrypted = Encrypt(ClearText);
            Console.WriteLine("Encrypted: " + encrypted);

            var decrypted = Decrypt(encrypted);
            Console.WriteLine("Decrypted: " + decrypted);
            Assert.AreEqual(ClearText, decrypted);
        }

        [TestMethod]
        public void EncryptTests()
        {
            Console.WriteLine("Cleartext: " + ClearText);

            var encryptedPrev = Encrypt(ClearText);
            Console.WriteLine("Encrypted: " + encryptedPrev);

            var encryptedNext = Encrypt_StreamReadToEnd(ClearText);
            Console.WriteLine("Encrypted: " + encryptedNext);
            Assert.AreEqual(encryptedPrev, encryptedNext);

            encryptedPrev = encryptedNext;
            encryptedNext = Encrypt_StreamRead(ClearText);
            Console.WriteLine("Encrypted: " + encryptedNext);
            Assert.AreEqual(encryptedPrev, encryptedNext);
        }

        [TestMethod]
        public void DecryptTests()
        {
            Console.WriteLine("Cleartext: " + ClearText);
            var encrypted = Encrypt(ClearText);
            Console.WriteLine("Encrypted: " + encrypted);



            var decryptedPrev = Decrypt(encrypted);
            Console.WriteLine("Decrypted: " + decryptedPrev);

            var decryptedNext = Decrypt_StreamReadToEnd(encrypted);
            Console.WriteLine("Decrypted: " + decryptedNext);
            Assert.AreEqual(decryptedPrev, decryptedNext);

            decryptedPrev = decryptedNext;
            decryptedNext = Decrypt_StreamRead(encrypted);
            Console.WriteLine("Decrypted: " + decryptedNext);
            Assert.AreEqual(decryptedPrev, decryptedNext);
        }


        [TestMethod]
        public void WriteTests()
        {
            Console.WriteLine("Cleartext: " + ClearText);

            using (var stream = EncryptingStream.Write(_mockEncryptedFile, _key))
                stream.Write(ClearText);

            var encrypted = new EncryptedString(_mockEncryptedFile.ToArray());
            Console.WriteLine("Encrypted: " + encrypted);

            using (var stream = DecryptingStream.Write(_mockFile, _key))
                stream.Write(encrypted);

            var decrypted = new Data (_mockFile).ToUtf8();
            Console.WriteLine("Decrypted: " + decrypted);
            Assert.AreEqual(ClearText, decrypted);
        }

        [TestMethod]
        public void EncryptedStringEqualityTests()
        {
            EncryptedString nullString = null;
            var empty = new EncryptedString();
            var encryptedA = Encrypt(ClearText);
            var encryptedB = Encrypt(ClearText);
            var encryptedC = Encrypt(ClearText.ToLowerInvariant());

            Assert.IsFalse(ReferenceEquals(nullString, empty));
            Assert.IsFalse(ReferenceEquals(nullString, encryptedA));
            Assert.IsFalse(ReferenceEquals(nullString, encryptedB));
            Assert.IsFalse(ReferenceEquals(empty, encryptedA));
            Assert.IsFalse(ReferenceEquals(empty, encryptedB));
            Assert.IsFalse(ReferenceEquals(encryptedA, encryptedB));

            Assert.IsTrue(nullString != empty);
            Assert.IsTrue(nullString != encryptedA);
            Assert.IsTrue(empty != nullString);
            Assert.IsTrue(encryptedA != nullString);

            Assert.IsTrue(empty != encryptedA);
            Assert.IsTrue(encryptedA != empty);

            Assert.IsTrue(encryptedA == encryptedB);
            Assert.IsTrue(encryptedA != encryptedC);

            Assert.AreEqual<object>(encryptedA, encryptedB);
            Assert.AreEqual<object>(encryptedA, encryptedB.ToArray());
            Assert.AreEqual<object>(encryptedA, encryptedB.ToString());

            Assert.AreNotEqual<object>(encryptedA, empty);
            Assert.AreNotEqual<object>(encryptedA, empty.ToArray());
            Assert.AreNotEqual<object>(encryptedA, empty.ToString());
            Assert.AreNotEqual<object>(encryptedA, 5);
        }

        [TestMethod]
        public void EncryptedStringHashTest()
        {
            var encryptedBytes = Encrypt(ClearText).ToArray();
            var secureString = new EncryptedString(encryptedBytes);
            Assert.AreEqual(encryptedBytes.GetHashCode(), secureString.GetHashCode());
        }

        [TestMethod]
        public void EncryptedStringOperatorTest()
        {
            EncryptedString nullString = null;
            var empty = new EncryptedString();
            var encryptedA = Encrypt(ClearText);
            var encryptedC = Encrypt(ClearText.ToLowerInvariant());

            var added = encryptedA + encryptedC;
            var addedToString = added.ToString();
            Assert.AreEqual(added, new EncryptedString(addedToString));

            Assert.AreEqual(encryptedA, encryptedA + empty);
            Assert.AreEqual(encryptedA, empty + encryptedA);
            Assert.AreEqual(encryptedA, encryptedA + nullString);
            Assert.AreEqual(encryptedA, nullString + encryptedA);

            Assert.AreEqual(empty, nullString + nullString);
            Assert.AreEqual(empty, empty + empty);
        }


        [TestMethod]
        public void TestPrivateKeyFile()
        {
            const string path = @"C:\Temp\PKFile.dat";
            PrivateKeyFile.Save(path, _key);

            var key = PrivateKeyFile.Load(path);

            Assert.AreEqual(key, _key);
        }

        private EncryptedString Encrypt(string text)
        {
            var encrypted = Cryptography.Encrypt(text, _key);
            Assert.AreNotEqual(text, encrypted);
            return encrypted;
        }

        private EncryptedString Encrypt_StreamReadToEnd(string text)
        {
            EncryptedString encrypted;
            using (var stream = EncryptingStream.Read(text, _key))
                encrypted = stream.ReadToEnd();

            Assert.AreNotEqual(text, encrypted);
            return encrypted;
        }

        private EncryptedString Encrypt_StreamRead(string text)
        {
            var encrypted = EncryptedString.Empty;

            using (var stream = EncryptingStream.Read(text, _key))
            {
                var newlyEncrypted = stream.Read(5);
                while (newlyEncrypted != null)
                {
                    encrypted += newlyEncrypted;
                    newlyEncrypted = stream.Read(5);
                }
            }
            Assert.AreNotEqual(text, encrypted);
            return encrypted;
        }

        private string Decrypt(EncryptedString text)
        {
            var decrypted = Cryptography.Decrypt(text, _key);
            Assert.AreNotEqual(text, decrypted);
            return decrypted;
        }

        private string Decrypt_StreamReadToEnd(EncryptedString text)
        {
            string decrypted;
            using (var stream = DecryptingStream.Read(text, _key))
                decrypted = stream.ReadToEnd();

            Assert.AreNotEqual(text, decrypted);
            return decrypted;
        }

        private string Decrypt_StreamRead(EncryptedString text)
        {
            var decrypted = string.Empty;

            using (var stream = DecryptingStream.Read(text, _key))
            {
                var newlyDecrypted = stream.Read(5);
                while (newlyDecrypted != null)
                {
                    decrypted += newlyDecrypted;
                    newlyDecrypted = stream.Read(5);
                }
            }
            Assert.AreNotEqual(text, decrypted);
            return decrypted;
        }
    }
}