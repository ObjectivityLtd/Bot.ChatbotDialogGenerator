namespace RoslynWrapper.SyntaxBuilders.Classes.Constructors
{
    using System;
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using RoslynWrapper.SyntaxBuilders.Common.Attributes;
    using RoslynWrapper.SyntaxBuilders.Common.ExpressionStatements;
    using RoslynWrapper.SyntaxBuilders.Common.Parameters;

    public class ConstructorBuilder
    {
        private readonly List<ParameterSyntax> _parameters;
        private readonly List<StatementSyntax> _statements;
        private AttributeSyntax _attribute;
        private readonly string _className;

        public ConstructorBuilder(string containingClassName)
        {
            _className = containingClassName;

            _parameters = new List<ParameterSyntax>();
            _statements = new List<StatementSyntax>();
        }

        public ConstructorBuilder WithAttribute(string attributeName)
        {
            _attribute = new AttributeBuilder()
                .WithName(attributeName)
                .Build();

            return this;
        }

        public ConstructorBuilder InjectParameter(Parameter parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }
            _parameters.Add(ParametersExtensions.Parameter(parameter));
            _statements.Add(ExpressionStatementExtensions.AssignValueToField(parameter.ParameterName, rightValue: parameter.ParameterName));

            return this;
        }

        public ConstructorDeclarationSyntax Build()
        {
            var declaration = SyntaxFactory.ConstructorDeclaration(SyntaxFactory.Identifier(_className))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            if (_attribute != null)
                declaration = declaration.AddAttribute(_attribute);

            if (_parameters.Count > 0)
                declaration = declaration.AddParameterListParameters(_parameters.ToArray());

            if(_statements.Count > 0)
                declaration = declaration.WithBody(SyntaxFactory.Block(_statements));

            return declaration;
        }
    }
}