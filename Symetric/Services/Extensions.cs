using Security.Common;

namespace Security.Services
{
    public static class Extensions
    {
        public static string ToUtf8(this Data data) => data.ToArray().ToUtf8().TrimEnd('\0');
    }
}