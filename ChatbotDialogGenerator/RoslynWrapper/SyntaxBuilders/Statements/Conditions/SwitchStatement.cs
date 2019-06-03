namespace RoslynWrapper.SyntaxBuilders.Statements.Conditions
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public class SwitchStatementBuilder
    {
        private readonly string _parameter;
        private readonly List<SwitchSectionSyntax> _switchSections;

        public SwitchStatementBuilder(string parameter)
        {
            _parameter = parameter;
            _switchSections = new List<SwitchSectionSyntax>();
        }

        public SwitchStatementBuilder AddSection(SwitchSectionSyntax switchSection)
        {
            _switchSections.Add(switchSection);

            return this;
        }

        public StatementSyntax Build()
        {
            var switchStatement = SyntaxFactory.SwitchStatement(SyntaxFactory.IdentifierName(_parameter));

            if (_switchSections != null && _switchSections.Count > 0)
                switchStatement = switchStatement.AddSections(_switchSections.ToArray());

            return switchStatement;
        }
    }
}