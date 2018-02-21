using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Security.App;
using Security.Services;

namespace UnitTest
{
    [TestClass]
    public class Examples
    {
        [TestMethod]
        public void WriteAndReadFile()
        {
            WriteEncryptedFileManual();
            DecryptEncryptedFileManual();
        }

        [TestMethod]
        public void WriteFileSaveDecryptLater()
        {
            const string clearText = "Here is some data that will be encrypted and nobody even can know what it is";

            WriteEncryptedFile(clearText);

            var encryptedValue = ReadEncryptedFile();
            var decryptedValue1 = DecryptEncryptedFile();
            Console.WriteLine("Encrypted: " + encryptedValue);
            Console.WriteLine("Decrypted immediately: " + decryptedValue1);

            var decryptedValue2 = Cryptography.Decrypt(encryptedValue, Key);
            Console.WriteLine("Decrypted later: " + decryptedValue2);

            Assert.AreNotEqual(clearText, encryptedValue);
            Assert.AreEqual(decryptedValue1, decryptedValue2);
            Assert.AreEqual(clearText, decryptedValue1);
        }

        private const string FilePath = @"C:\Temp\MyEncryptedFile.txt";
        private static PrivateKey Key => PrivateKeyFile.Load(@"C:\Temp\PKFile.dat");

        public static void WriteEncryptedFileManual()
        {
            using (var encryptedStream =  EncryptingStream.Write(File.OpenWrite(FilePath), Key))
            {
                encryptedStream.WriteLine("Here is some data");
                encryptedStream.WriteLine("Here is some more data");
            }

        }

        public static void DecryptEncryptedFileManual()
        {
            using (var encryptedStream = DecryptingStream.Read(File.OpenRead(FilePath), Key))
            {
                while (!encryptedStream.EndOfStream)
                    Console.WriteLine(encryptedStream.ReadLine());
            }
        }

        public static void WriteEncryptedFile(string data)
        {
            EncryptedFile.Write(FilePath, data, Key);
        }

        public static EncryptedString ReadEncryptedFile()
        {
            return EncryptedFile.Read(FilePath);
        }

        public static string DecryptEncryptedFile()
        {
            return EncryptedFile.Decrypt(FilePath, Key);
        }
    }
}