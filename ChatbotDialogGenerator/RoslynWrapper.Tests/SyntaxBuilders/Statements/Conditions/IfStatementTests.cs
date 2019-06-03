namespace RoslynWrapper.Tests.SyntaxBuilders.Statements.Conditions
{
    using FluentAssertions;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using RoslynWrapper.SyntaxBuilders.Statements.Conditions;
    using RoslynWrapper.Tests.Utils;
    using Xunit;

    public class IfStatementTests
    {
        [Fact]
        public void Can_Create_If_Statement()
        {
            // arrange
            const string ifTrueStatement = "var x = 1;";
            var ifTrueStatementSyntax = ifTrueStatement.ConvertToLocalDeclarationStatementSyntax();

            // act
            var ifStatement = new IfStatement("x == 1", new StatementSyntax[] { ifTrueStatementSyntax })
                .Build()
                .NormalizeWhitespace().ToFullString();

            //assert
            const string expectedStatement = @"if (x == 1)
{
    var x = 1;
}";

            ifStatement.Should().Be(expectedStatement);
        }

        [Fact]
        public void Can_Create_If_Statement_With_ElseIf_Statements()
        {
            // arrange
            const string ifTrueStatement = "var x = 1;";
            var ifTrueStatementSyntax = ifTrueStatement.ConvertToLocalDeclarationStatementSyntax();

            const string elseIfStatement1 = "var x = 2;";
            var elseIfStatementSyntax1 = elseIfStatement1.ConvertToLocalDeclarationStatementSyntax();

            const string elseIfStatement2 = "var x = 3;";
            var elseIfStatementSyntax2 = elseIfStatement2.ConvertToLocalDeclarationStatementSyntax();

            const string elseIfStatement3 = "var x = 4;";
            var elseIfStatementSyntax3 = elseIfStatement3.ConvertToLocalDeclarationStatementSyntax();

            // act
            var ifStatement = new IfStatement("x == 1", new StatementSyntax[] { ifTrueStatementSyntax })
                .ElseIf("x == 2", new StatementSyntax[] { elseIfStatementSyntax1 })
                .ElseIf("x == 3", new StatementSyntax[] { elseIfStatementSyntax2 })
                .ElseIf("x == 4", new StatementSyntax[] { elseIfStatementSyntax3 })
                .Build()
                .NormalizeWhitespace().ToFullString();

            //assert
            const string expectedStatement = @"if (x == 1)
{
    var x = 1;
}
else if (x == 2)
{
    var x = 2;
}
else if (x == 3)
{
    var x = 3;
}
else if (x == 4)
{
    var x = 4;
}";

            ifStatement.Should().Be(expectedStatement);
        }

        [Fact]
        public void Can_Create_If_Statement_With_And_Else_Statements()
        {
            // arrange
            const string ifTrueStatement = "var x = 1;";
            var ifTrueStatementSyntax = ifTrueStatement.ConvertToLocalDeclarationStatementSyntax();

            const string ifFalseStatement = "var x = 5;";
            var ifFalseStatementSyntax = ifFalseStatement.ConvertToLocalDeclarationStatementSyntax();

            // act
            var ifStatement = new IfStatement("x == 1", new StatementSyntax[] { ifTrueStatementSyntax })
                .Else(new StatementSyntax[] { ifFalseStatementSyntax })
                .Build()
                .NormalizeWhitespace().ToFullString();

            //assert
            const string expectedStatement = @"if (x == 1)
{
    var x = 1;
}
else
{
    var x = 5;
}";

            ifStatement.Should().Be(expectedStatement);
        }

        [Fact]
        public void Can_Create_If_Statement_With_ElseIf_And_Else_Statements()
        {
            // arrange
            const string ifTrueStatement = "var x = 1;";
            var ifTrueStatementSyntax = ifTrueStatement.ConvertToLocalDeclarationStatementSyntax();

            const string elseIfStatement1 = "var x = 2;";
            var elseIfStatementSyntax1 = elseIfStatement1.ConvertToLocalDeclarationStatementSyntax();

            const string elseIfStatement2 = "var x = 3;";
            var elseIfStatementSyntax2 = elseIfStatement2.ConvertToLocalDeclarationStatementSyntax();

            const string elseIfStatement3 = "var x = 4;";
            var elseIfStatementSyntax3 = elseIfStatement3.ConvertToLocalDeclarationStatementSyntax();

            const string ifFalseStatement = "var x = 5;";
            var ifFalseStatementSyntax = ifFalseStatement.ConvertToLocalDeclarationStatementSyntax();

            // act
            var ifStatement = new IfStatement("x == 1", new StatementSyntax[] { ifTrueStatementSyntax })
                .ElseIf("x == 2", new StatementSyntax[] { elseIfStatementSyntax1 })
                .ElseIf("x == 3", new StatementSyntax[] { elseIfStatementSyntax2 })
                .ElseIf("x == 4", new StatementSyntax[] { elseIfStatementSyntax3 })
                .Else(new StatementSyntax[] { ifFalseStatementSyntax })
                .Build()
                .NormalizeWhitespace().ToFullString();

            //assert
            const string expectedStatement = @"if (x == 1)
{
    var x = 1;
}
else if (x == 2)
{
    var x = 2;
}
else if (x == 3)
{
    var x = 3;
}
else if (x == 4)
{
    var x = 4;
}
else
{
    var x = 5;
}";

            ifStatement.Should().Be(expectedStatement);
        }
    }
}