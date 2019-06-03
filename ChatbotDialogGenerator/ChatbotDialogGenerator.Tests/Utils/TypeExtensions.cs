namespace ChatbotDialogGenerator.Tests.Utils
{
    using System;
    using System.Linq;

    public static class TypeExtensions
    {
        public static Type[] GetSubclasses(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(type) && !t.IsAbstract)
                .ToArray();
        }
    }
}