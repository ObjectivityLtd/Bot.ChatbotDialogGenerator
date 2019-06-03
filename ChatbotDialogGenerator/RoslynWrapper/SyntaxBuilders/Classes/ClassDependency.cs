namespace RoslynWrapper.SyntaxBuilders.Classes
{
    public class ClassDependency
    {
        public ClassDependency(string typeName, string fieldName)
        {
            TypeName = typeName;
            FieldName = fieldName;
        }

        public string TypeName { get; }
        public string FieldName { get; }
    }
}