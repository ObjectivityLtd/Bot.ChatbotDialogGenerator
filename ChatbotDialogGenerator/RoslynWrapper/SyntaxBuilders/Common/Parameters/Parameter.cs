namespace RoslynWrapper.SyntaxBuilders.Common.Parameters
{
    public class Parameter
    {
        public string TypeName { get; }
        public string ParameterName { get; }

        public Parameter(string typeName, string parameterName)
        {
            TypeName = typeName;
            ParameterName = parameterName;
        }
    }
}