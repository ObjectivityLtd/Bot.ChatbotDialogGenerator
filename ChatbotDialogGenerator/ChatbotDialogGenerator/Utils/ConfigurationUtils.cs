namespace ChatbotDialogGenerator.Utils
{
    using System;
    using System.Collections.Specialized;

    public static class ConfigurationUtils
    {
        public static string[] GetUsingsFromSetting(StringCollection collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            var usings = new string[collection.Count];
            collection.CopyTo(usings, 0);
            return usings;
        }
    }
}