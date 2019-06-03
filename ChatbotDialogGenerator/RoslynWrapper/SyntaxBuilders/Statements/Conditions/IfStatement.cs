namespace RoslynWrapper.SyntaxBuilders.Statements.Conditions
{
    using System;
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public class IfStatement
    {
        private readonly List<IfStatementPayload> _elsIfStatements;
        private readonly IfStatementPayload _ifStatementPayload;
        private BlockSyntax _elseStatement;

        public IfStatement(string condition, StatementSyntax[] statements)
        {
            if (statements == null)
            {
                throw new ArgumentNullException(nameof(statements));
            }

            this._elsIfStatements = new List<IfStatementPayload>();

            var body = statements.Length > 0
                ? SyntaxFactory.Block().AddStatements(statements)
                : SyntaxFactory.Block();

            this._ifStatementPayload = new IfStatementPayload
            {
                Condition = condition,
                Body = body
            };
        }

        public IfStatement ElseIf(string condition, StatementSyntax[] statements)
        {
            if (statements == null)
            {
                throw new ArgumentNullException(nameof(statements));
            }

            var body = statements.Length > 0
                ? SyntaxFactory.Block().AddStatements(statements)
                : SyntaxFactory.Block();

            var elseIf = new IfStatementPayload
            {
                Condition = condition,
                Body = body
            };

            this._elsIfStatements.Add(elseIf);

            return this;
        }

        public IfStatement Else(StatementSyntax[] statements)
        {
            if (statements == null)
            {
                throw new ArgumentNullException(nameof(statements));
            }

            this._elseStatement = statements.Length > 0
                ? SyntaxFactory.Block().AddStatements(statements)
                : SyntaxFactory.Block();

            return this;
        }

        public StatementSyntax Build()
        {
            this._elsIfStatements.Reverse();

            var ifStatement = SyntaxFactory.IfStatement(
                SyntaxFactory.IdentifierName(this._ifStatementPayload.Condition),
                this._ifStatementPayload.Body);

            if (this._elsIfStatements.Count > 0)
            {
                ifStatement = ifStatement.WithElse(
                    SyntaxFactory.ElseClause(this.BuildIfStatement(this._elsIfStatements.Count - 1)));
            }
            else if (this._elseStatement != null)
            {
                ifStatement = ifStatement.WithElse(
                    SyntaxFactory.ElseClause(this._elseStatement));
            }

            return ifStatement;
        }

        private IfStatementSyntax BuildIfStatement(int n)
        {
            var currentStatement = this._elsIfStatements[n];

            if (n == 0)
            {
                var ifStatement = SyntaxFactory.IfStatement(
                    SyntaxFactory.IdentifierName(currentStatement.Condition),
                    currentStatement.Body);

                if (this._elseStatement != null)
                {
                    ifStatement = ifStatement.WithElse(SyntaxFactory.ElseClause(this._elseStatement));
                }

                return ifStatement;
            }

            var elseStatement = this.BuildIfStatement(n - 1);

            return SyntaxFactory.IfStatement(
                SyntaxFactory.IdentifierName(currentStatement.Condition),
                currentStatement.Body,
                SyntaxFactory.ElseClause(elseStatement));
        }
    }
}