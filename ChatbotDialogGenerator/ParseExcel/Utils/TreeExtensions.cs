namespace ParseExcel.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class TreeExtensions
    {
        public static IEnumerable<T> Flatten<T>(
            this IEnumerable<T> entity,
            Func<T, IEnumerable<T>> func)
        {
            return entity.SelectMany(c => func(c).Flatten(func)).Concat(entity);
        }
    }
}