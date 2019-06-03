namespace RoslynWrapper.SyntaxBuilders.Statements.Declarations
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using RoslynWrapper.SyntaxBuilders.Common;
    using RoslynWrapper.SyntaxBuilders.Common.VariableDeclaration;

    public static class FieldDeclarationSyntaxExtensions
    {
        public static FieldDeclarationSyntax Build(string fieldType, string fieldName)
        {
            return SyntaxFactory.FieldDeclaration(
                SyntaxFactory.VariableDeclaration(SyntaxFactory.IdentifierName(fieldType))
                    .AddVariables(SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(fieldName))));
        }

        public static FieldDeclarationSyntax Build(string fieldType, string fieldName, string rightValue)
        {
            var variableDeclarator = SyntaxFactory.VariableDeclarator(fieldName)
                .WithInitializer(rightValue);

            return SyntaxFactory.FieldDeclaration(
                SyntaxFactory.VariableDeclaration(SyntaxFactory.IdentifierName(fieldType))
                    .AddVariables(variableDeclarator));
        }

        public static FieldDeclarationSyntax WithAccessModifier(this FieldDeclarationSyntax property, AccessModifier modifier)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            return property.AddModifiers(SyntaxFactory.Token(modifier.MapToSyntaxKind()));
        }

        public static FieldDeclarationSyntax SetStatic(this FieldDeclarationSyntax field)
        {
            if (field == null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            return field.AddModifiers(SyntaxFactory.Token(SyntaxKind.StaticKeyword));
        }

        public static FieldDeclarationSyntax SetReadonly(this FieldDeclarationSyntax field)
        {
            if (field == null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            return field.AddModifiers(SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword));
        }
    }
}