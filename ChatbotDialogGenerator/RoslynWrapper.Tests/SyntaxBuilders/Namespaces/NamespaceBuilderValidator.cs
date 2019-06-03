namespace RoslynWrapper.Tests.SyntaxBuilders.Namespaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public class NamespaceBuilderValidator
    {
        public void VerifyNamespaceWasCreated(NamespaceDeclarationSyntax @namespace)
        {
            @namespace.Should().NotBeNull();
        }

        public void VerifyNamespaceName(NameSyntax name, string expectedName)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            name.ToFullString().Trim().Should().Be(expectedName);
        }

        public void VerifyNamespaceUsings(SyntaxList<UsingDirectiveSyntax> usings, string[] expectedUsings)
        {
            if (expectedUsings == null)
            {
                throw new ArgumentNullException(nameof(expectedUsings));
            }

            usings.Count.Should().Be(expectedUsings.Length);

            var usingsToString = usings
                .Select(@using => @using.Name.ToFullString().Trim())
                .ToArray();

            usingsToString.Should().BeEquivalentTo(expectedUsings, opt => opt.WithStrictOrdering(),
                because: $"{{{string.Join(", ", usingsToString)}}} should be equivalent to {{{string.Join(", ", expectedUsings)}}}");
        }

        public void VerifyClasses(List<ClassDeclarationSyntax> classes, string[] expectedClasses)
        {
            if (classes == null)
            {
                throw new ArgumentNullException(nameof(classes));
            }
            if (expectedClasses == null)
            {
                throw new ArgumentNullException(nameof(expectedClasses));
            }

            classes.Count.Should().Be(expectedClasses.Length);

            var classesToString = classes
                .Select(@class => @class.Identifier.Value.ToString())
                .ToArray();

            classesToString.Should().BeEquivalentTo(expectedClasses);
        }
    }
}