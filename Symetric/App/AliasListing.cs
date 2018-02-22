using System.Collections.Generic;

namespace Security.App
{
    public class AliasListing : Dictionary<string, string>
    {
        public string GetId(string aliasOrId)
        {
            return Contains(aliasOrId) ? this[aliasOrId] : aliasOrId;
        }

        public bool Contains(string alias, string id)
        {
            if (!ContainsKey(alias)) return false;
            return this[alias] == id;
        }

        public bool Contains(string alias) => ContainsKey(alias);
    }
}