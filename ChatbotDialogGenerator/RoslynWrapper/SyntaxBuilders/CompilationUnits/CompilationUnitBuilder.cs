namespace RoslynWrapper.SyntaxBuilders.CompilationUnits
{
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public class CompilationUnitBuilder
    {
        private MemberDeclarationSyntax[] _members;

        public CompilationUnitBuilder WithMembers(params MemberDeclarationSyntax[] members)
        {
            _members = members;

            return this;
        }

        public CompilationUnitSyntax Build()
        {
            return SyntaxFactory.CompilationUnit()
                .AddMembers(_members);
        }
    }
}