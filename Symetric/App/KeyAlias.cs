namespace Security.App
{
    public class KeyAlias
    {
        public string Alias { get; }
        public string Id { get; }

        public KeyAlias(string alias, string id)
        {
            Alias = alias;
            Id = id;
        }

        public static implicit operator string(KeyAlias alias) => alias?.Alias;
    }
}