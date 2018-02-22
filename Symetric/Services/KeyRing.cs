using System;
using System.IO;
using Security.App;
using Security.Services.Files;

namespace Security.Services
{
    public static class KeyRing
    {
        private const string KeyRingDirectory = "KeyRing";
        private const string AliasFilename = ".aliases";

        private static readonly string KeyRingPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), KeyRingDirectory);
        private static string GetPath(string file) => Path.Combine(KeyRingPath, file);

        /// <summary>
        /// Check if a key with a given id exists
        /// </summary>
        public static bool KeyExists(string keyId) => File.Exists(GetPath(keyId));

        /// <summary>
        /// Load a key by alias.  If it does not exist, a new one will be created and saved to the key ring
        /// </summary>
        public static PrivateKey LoadOrCreate(string alias)
        {
            if (!Directory.Exists(KeyRingPath)) Directory.CreateDirectory(KeyRingPath);

            var keyId = AliasFile.Load(GetPath(AliasFilename)).GetId(alias);

            return KeyExists(keyId) 
                ? PrivateKeyFile.Load(GetPath(keyId)) 
                : Cryptography.GenerateKey(alias).Save();
        }

        /// <summary>
        /// Load a key from the key ring.  If "aliasOrId" is a valid alias, the corresponding key will be used.  Otherwise, it will be treate as the id.
        /// </summary>
        public static PrivateKey Load(this string aliasOrId)
        {
            if (!Directory.Exists(KeyRingPath))
                throw new Exception("No key ring exists on this machine for the current user");

            var aliases = AliasFile.Load(GetPath(AliasFilename));
            var keyId = aliases.GetId(aliasOrId);

            if (!KeyExists(keyId))
                throw new Exception($"key {keyId} was not found on the key ring");

            return PrivateKeyFile.Load(GetPath(keyId));
        }

        /// <summary>
        /// Save a key to the key ring
        /// </summary>
        public static PrivateKey Save(this PrivateKey key)
        {
            if (!Directory.Exists(KeyRingPath)) Directory.CreateDirectory(KeyRingPath);
            PrivateKeyFile.Save(GetPath(key.Id), key);
            AliasFile.Save(GetPath(AliasFilename), key.Alias);
            return key;
        }

        /// <summary>
        /// Remove a key from the key ring
        /// </summary>
        public static void Remove(this PrivateKey key)
        {
            if (!Directory.Exists(KeyRingPath)) return;

            var filePath = GetPath(key.Id);
            if (File.Exists(filePath))
                File.Delete(filePath);

            AliasFile.Remove(GetPath(AliasFilename), key.Alias);
        }
    }
}