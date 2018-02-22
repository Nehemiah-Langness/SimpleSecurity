using System;
using System.IO;
using Security.App;
using Security.Exceptions;

namespace Security.Services.Files
{
    public class AliasFile : BinaryFile
    {
        private const byte CurrentVersion = 1;

        private AliasFile(Stream stream) 
            : base(stream) { }

        private AliasListing Load()
        {
            var version = ReadByte();
            switch (version)
            {
                case 1:
                {
                    IntSize = ReadByte();
                    var count = ReadInt();

                    var listing = new AliasListing();

                    for (var i = 0; i < count; i++)
                    {
                        var alias = ReadString();
                        var id = ReadString();
                        listing[alias] = id;
                    }

                    CheckFullyRead();
                    return listing;
                }

                default:
                    throw new InvalidOperationException($"Unsupported key version: {version}");
            }
        }

        private void Save(AliasListing listing)
        {
            WriteByte(CurrentVersion);
            WriteByte(SystemIntSize);
            WriteInt(listing.Count);
            foreach (var alias in listing.Keys)
            {
                WriteString(alias);
                WriteString(listing[alias]);
            }
        }

        private void Add(AliasListing listing, KeyAlias key)
        {
            listing[key.Alias] = key.Id;
            Save(listing);
        }

        private void Remove(AliasListing listing, string alias)
        {
            listing.Remove(alias);
            Save(listing);
        }

        public static AliasListing Load(string path)
        {
            if (!File.Exists(path))
                return new AliasListing();

            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                return new AliasFile(stream).Load();
        }

        public static void Save(string path, KeyAlias key)
        {
            var listing = Load(path);
            if (listing.Contains(key.Alias, key.Id))
                return;

            using (var stream = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.Read))
                new AliasFile(stream).Add(listing, key);
        }

        public static void Remove(string path, string alias)
        {
            var listing = Load(path);
            if (!listing.Contains(alias))
                return;

            using (var stream = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.Read))
                new AliasFile(stream).Remove(listing, alias);
        }

        private void CheckFullyRead()
        {
            if (!EndOfStream)
                throw new FileCorruptException("Key data did not fill the file");
        }
    }
}