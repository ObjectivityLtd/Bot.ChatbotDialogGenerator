namespace RoslynWrapper.SyntaxBuilders.Namespaces
{
    using System;
    using System.Collections.Generic;

    public class NamespaceComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (string.IsNullOrEmpty(x))
            {
                return -1;
            }

            if (string.IsNullOrEmpty(y))
            {
                return 1;
            }

            if (x.ToLower().StartsWith("system", StringComparison.CurrentCultureIgnoreCase) &&
                y.ToLower().StartsWith("system", StringComparison.CurrentCultureIgnoreCase))
            {
                return string.Compare(x, y, StringComparison.Ordinal);
            }

            if (x.ToLower().StartsWith("system", StringComparison.CurrentCultureIgnoreCase))
            {
                return 1;
            }

            if (y.ToLower().StartsWith("system", StringComparison.CurrentCultureIgnoreCase))
            {
                return -1;
            }

            return string.Compare(x, y, StringComparison.Ordinal);
        }
    }
}