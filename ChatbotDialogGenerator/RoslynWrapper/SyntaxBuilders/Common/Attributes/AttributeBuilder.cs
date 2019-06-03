namespace RoslynWrapper.SyntaxBuilders.Common.Attributes
{
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public class AttributeBuilder
    {
        private string _attributeName;
        private string _typeOfArgument;

        public AttributeBuilder WithName(string attributeName)
        {
            _attributeName = attributeName;

            return this;
        }

        public AttributeBuilder WithTypeOfArgument(string argument)
        {
            _typeOfArgument = argument;

            return this;
        }

        public AttributeSyntax Build()
        {
            var attribute = SyntaxFactory.Attribute(SyntaxFactory.IdentifierName(_attributeName));

            if(!string.IsNullOrWhiteSpace(_typeOfArgument))
                attribute = attribute.WithTypeOfArgument(_typeOfArgument);

            return attribute;
        }
    }
}