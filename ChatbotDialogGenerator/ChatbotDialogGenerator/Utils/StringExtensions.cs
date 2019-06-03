namespace ChatbotDialogGenerator.Utils
{
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        public static bool IsNullOrWhitespace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static string RemoveNonAlphanumeric(this string value)
        {
            return Regex.Replace(value, @"[^a-zA-Z0-9]", string.Empty);
        }
    }
}