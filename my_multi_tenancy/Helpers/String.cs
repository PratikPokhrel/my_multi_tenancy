using System.Text.RegularExpressions;

namespace my_multi_tenancy.Helpers
{
    public static class String
    {
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }

            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }

        public static string Excerpt(this string str, int limit)
            =>string.IsNullOrEmpty(str)?string.Empty : str.Length > limit ? $"{str[..limit]}..." : str;

        public static string UppercaseFirst(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            char[] a = str.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
    }
}
