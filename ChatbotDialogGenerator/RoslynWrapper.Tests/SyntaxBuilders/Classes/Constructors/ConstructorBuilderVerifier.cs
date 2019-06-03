namespace RoslynWrapper.Tests.SyntaxBuilders.Classes.Constructors
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public class ConstructorBuilderVerifier
    {
        public void VerifyConstructorName(SyntaxToken identifier, string expectedName)
        {
            identifier.ValueText.Should().Be(expectedName);
        }

        public void VerifyClassAttribute(AttributeListSyntax classAttribute, string expectedAttributeName)
        {
            if (classAttribute == null)
            {
                throw new ArgumentNullException(nameof(classAttribute));
            }

            classAttribute.Attributes.Should().HaveCount(1);

            var attribute = classAttribute.Attributes.Single();
            attribute.Name.ToFullString().Should().Be(expectedAttributeName);
        }
    }
}