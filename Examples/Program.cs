using System;
using System.IO;
using Security.App;
using Security.Services;

namespace Examples
{
    class Program
    {
        internal const string ConfigurationFilename = "Configuration.dat";
        internal const string UserDataFilename = "MyEncryptedData.dat";
        internal const int MaxAttemptsBeforeShutdown = 5;

        static void Main(string[] args)
        {
            var enteredPassword = Terminal.Prompt("What is your password?");
            var attemptsMade = 1;
            while (!SecurityLayer.CheckPassword(enteredPassword))
            {
                if (MaxAttemptsBeforeShutdown == attemptsMade) return;

                Terminal.Message("Password is incorrect.");
                enteredPassword = Terminal.Prompt("What is your password?");
                attemptsMade++;
            }

            Terminal.Message("Printing your saved password");
            Terminal.Message(EncryptedFile.Read(ConfigurationFilename));

            EncryptDataForUser();

            Terminal.Prompt("Press enter to exit");
        }

        private static void EncryptDataForUser()
        {
            Terminal.Message("You may now encrypt some data.");
            var data = Terminal.Prompt("What data would you like to encrypt?");

            SecurityLayer.EncryptData(data);
            Terminal.Message("Printing your encrypted data");
            Terminal.Message(EncryptedFile.Read(UserDataFilename));

            SecurityLayer.DecryptData();
        }
    }

    public class Terminal
    {
        public static string Prompt(string message)
        {
            Console.WriteLine(message);
            Console.Write(">> ");
            return Console.ReadLine();
        }

        public static void Message(string message) => Console.WriteLine(message);
    }

    public class SecurityLayer
    {
        public static PrivateKey ApplicationKey => KeyRing.LoadOrCreate("My Application Key");

        public static bool CheckPassword(string password)
        {
            if (!File.Exists(Program.ConfigurationFilename))
            {
                SetPassword(password);
                return true;
            }

            var currentPassword = EncryptedFile.Decrypt(Program.ConfigurationFilename, ApplicationKey).AsHash();
            return currentPassword.Compare(password);
        }

        private static void SetPassword(string password)
        {
            EncryptedFile.Write(Program.ConfigurationFilename, Hashing.Hash(password).Serialize(), ApplicationKey);
        }

        public static void EncryptData(string data)
        {
            EncryptedFile.Write(Program.UserDataFilename, data, ApplicationKey);
        }

        public static void DecryptData()
        {
            using (var stream = DecryptingStream.Read(File.OpenRead(Program.UserDataFilename), ApplicationKey))
            {
                Terminal.Message("Decrypting data while minimizing decrypted data's exposure to memory:");
                Terminal.Message(stream.ReadToEnd());
            }
        }
    }
}
