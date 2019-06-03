namespace RoslynWrapper.SyntaxBuilders.Statements
{
    using System;
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public class MultipleStatementBuilder
    {
        private readonly List<StatementSyntax> _statements;

        public MultipleStatementBuilder()
        {
            _statements = new List<StatementSyntax>();
        }

        public MultipleStatementBuilder AddStatements(params StatementSyntax[] statements)
        {
            if (statements == null)
            {
                throw new ArgumentNullException(nameof(statements));
            }

            if (statements.Length > 0)
                _statements.AddRange(statements);

            return this;
        }

        public StatementSyntax[] Build()
        {
            return _statements.ToArray();
        }
    }
}