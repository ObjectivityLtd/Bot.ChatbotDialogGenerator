namespace ParseExcel.Utils
{
    using System.Text.RegularExpressions;
    using ParseExcel.Configuration;

    public static class StringExtensions
    {
        public static bool MatchesPattern(this string value, string pattern)
        {
            return Regex.IsMatch(value, pattern);
        }

        public static string RemoveNonAlphanumeric(this string value)
        {
            return Regex.Replace(value, @"[^a-zA-Z0-9]", string.Empty);
        }

        public static bool IsSurroundedByAnyBraces(this string value)
        {
            return Regex.IsMatch(value, @"[\{\[\(].+[\}\]\)]");
        }
    }
}