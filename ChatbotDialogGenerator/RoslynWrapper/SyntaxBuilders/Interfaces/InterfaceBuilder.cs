namespace RoslynWrapper.SyntaxBuilders.Interfaces
{
    using System.Linq;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using RoslynWrapper.SyntaxBuilders.Common;

    public class InterfaceBuilder
    {
        private readonly string _name;
        private BaseTypeSyntax[] _baseTypes;
        private AccessModifier? _accessModifier;

        public InterfaceBuilder(string interfaceName)
        {
            _name = interfaceName;
        }

        public InterfaceBuilder WithAccessModifier(AccessModifier accessModifier)
        {
            _accessModifier = accessModifier;

            return this;
        }

        public InterfaceBuilder WithBaseTypes(params string[] baseTypes)
        {
            _baseTypes = baseTypes.Select(type => SyntaxFactory.SimpleBaseType(SyntaxFactory.IdentifierName(type)))
                .ToArray();

            return this;
        }

        public InterfaceDeclarationSyntax Build()
        {
            var declaration = SyntaxFactory.InterfaceDeclaration(_name);

            if (_baseTypes != null && _baseTypes.Length > 0)
                declaration = declaration.AddBaseListTypes(_baseTypes);

            if(_accessModifier != null)
                declaration = declaration.AddModifiers(SyntaxFactory.Token(_accessModifier.Value.MapToSyntaxKind()));

            return declaration;
        }
    }
}