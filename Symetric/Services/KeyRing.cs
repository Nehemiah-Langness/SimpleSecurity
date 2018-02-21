using System;
using System.IO;
using Security.App;

namespace Security.Services
{
    public static class KeyRing
    {
        private static readonly string KeyRingPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Keyring");

        public static bool KeyExists(string keyId) => File.Exists(Path.Combine(KeyRingPath, keyId));

        public static PrivateKey Load(string keyId)
        {
            if (!Directory.Exists(KeyRingPath)) throw new Exception("No key ring exists on this machine for the current user");
            if (!KeyExists(keyId)) throw new Exception($"key {keyId} was not found on the key ring");
            return PrivateKeyFile.Load(Path.Combine(KeyRingPath, keyId));
        }

        public static void Save(PrivateKey key)
        {
            if (!Directory.Exists(KeyRingPath)) Directory.CreateDirectory(KeyRingPath);
            PrivateKeyFile.Save(Path.Combine(KeyRingPath, key.Id), key);
        }
    }
}