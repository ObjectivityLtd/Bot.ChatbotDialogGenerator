namespace RoslynWrapper.SyntaxBuilders.Methods
{
    using System;
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using RoslynWrapper.SyntaxBuilders.Common;
    using RoslynWrapper.SyntaxBuilders.Common.Attributes;
    using RoslynWrapper.SyntaxBuilders.Common.Parameters;

    public class MethodBuilder
    {
        private readonly SyntaxToken _methodName;
        private readonly List<ParameterSyntax> _parameters;
        private readonly TypeSyntax _returnType;
        private readonly List<StatementSyntax> _statements;

        private AccessModifier? _accessModifier;
        private bool _isAsync;
        private AttributeSyntax _attributeSyntax;

        public MethodBuilder(string returnType, string methodName)
        {
            _parameters = new List<ParameterSyntax>();
            _statements = new List<StatementSyntax>();

            _methodName = SyntaxFactory.Identifier(methodName);
            _returnType = SyntaxFactory.IdentifierName(returnType);
        }

        public MethodBuilder WithAccessModifier(AccessModifier accessModifier)
        {
            _accessModifier = accessModifier;

            return this;
        }

        public MethodBuilder MakeAsync()
        {
            _isAsync = true;

            return this;
        }

        public MethodBuilder WithParameters(params Parameter[] parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            foreach (var parameter in parameters)
            {
                _parameters.Add(ParametersExtensions.Parameter(parameter));
            }

            return this;
        }

        public MethodBuilder WithAttribute(string attributeName)
        {
            _attributeSyntax = new AttributeBuilder()
                .WithName(attributeName)
                .Build();

            return this;
        }

        public MethodBuilder AddStatements(params StatementSyntax[] statement)
        {
            _statements.AddRange(statement);   

            return this;
        }

        public MethodDeclarationSyntax Build()
        {
            var declaration = SyntaxFactory.MethodDeclaration(_returnType, _methodName)
                .WithBody(SyntaxFactory.Block());

            if (_attributeSyntax != null)
            {
                declaration = declaration.WithAttribute(_attributeSyntax);
            }

            if (_accessModifier != null)
            {
                declaration = declaration.AddModifiers(SyntaxFactory.Token(_accessModifier.Value.MapToSyntaxKind()));
            }

            if (_isAsync)
                declaration = declaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.AsyncKeyword));

            if (_parameters.Count > 0)
                declaration = declaration.AddParameterListParameters(_parameters.ToArray());

            if (_statements.Count > 0)
                declaration = declaration.AddBodyStatements(_statements.ToArray());

            return declaration;
        }
    }
}