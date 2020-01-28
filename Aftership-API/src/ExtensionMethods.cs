using Newtonsoft.Json.Linq;

namespace AftershipAPI
{
    internal static class ExtensionMethods
    {
        public static bool IsNullOrEmpty(this JToken token)
        {
            return (token == null || token.Type == JTokenType.Null);
        }
    }
}
