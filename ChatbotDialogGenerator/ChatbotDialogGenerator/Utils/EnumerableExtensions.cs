namespace ChatbotDialogGenerator.Utils
{
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Except<T>(this IEnumerable<T> collection, T elementToBeExcluded)
        {
            return collection.Except(new[] { elementToBeExcluded });
        }
    }
}