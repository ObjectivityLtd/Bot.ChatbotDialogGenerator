namespace RoslynWrapper.SyntaxBuilders.Common
{
    using System;
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis.CSharp;

    public static class AccessModifierMapper
    {
        private static readonly Dictionary<AccessModifier, SyntaxKind> AccessModifierToSyntaxKindMapping = new Dictionary<AccessModifier, SyntaxKind>
        {
            { AccessModifier.Private, SyntaxKind.PrivateKeyword },
            { AccessModifier.Public, SyntaxKind.PublicKeyword },
            { AccessModifier.Protected, SyntaxKind.ProtectedKeyword },
            { AccessModifier.Internal, SyntaxKind.InternalKeyword},
        };

        public static SyntaxKind MapToSyntaxKind(this AccessModifier accessModifier)
        {
            var hasMappingSucceeded = AccessModifierToSyntaxKindMapping.TryGetValue(accessModifier, out var result);

            if (hasMappingSucceeded)
                return result;

            throw new InvalidOperationException($"Unsupported access modifier - {accessModifier.ToString()}");
        }
    }

    public enum AccessModifier
    {
        None = 0,
        Private = 1,
        Public = 2,
        Protected = 3,
        Internal = 4
    }
}