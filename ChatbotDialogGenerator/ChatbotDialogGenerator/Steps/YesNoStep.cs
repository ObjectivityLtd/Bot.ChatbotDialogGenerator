namespace ChatbotDialogGenerator.Steps
{
    using ChatbotDialogGenerator.Configuration;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using RoslynWrapper.SyntaxBuilders.Common;
    using RoslynWrapper.SyntaxBuilders.Common.Parameters;
    using RoslynWrapper.SyntaxBuilders.Methods;
    using RoslynWrapper.SyntaxBuilders.Statements;
    using RoslynWrapper.SyntaxBuilders.Statements.Conditions;
    using RoslynWrapper.SyntaxBuilders.Statements.Declarations;
    using RoslynWrapper.SyntaxBuilders.Statements.MethodCalls;

    public class YesNoStep : QuestionStep
    {
        public YesNoStep(StaticResources staticResources, CommonConfiguration commonConfiguration, string question, Step yesPath, Step noPath)
            : base(staticResources, commonConfiguration, question)
        {
            this.YesPath = yesPath;
            this.NoPath = noPath;
        }

        public Step YesPath { get; }

        public Step NoPath { get; }

        public override MethodDeclarationSyntax GenerateStartMethod()
        {
            var method = new MethodBuilder(returnType: "Task", methodName: this.StaticResources.StartAsync)
                .WithAccessModifier(AccessModifier.Public)
                .MakeAsync()
                .WithParameters(
                    new Parameter(typeName: this.StaticResources.context.Type, parameterName: this.StaticResources.context.Name))
                .AddStatements(this.GeneratePostAdditionalMessagesStatements())
                .AddStatements(this.GenerateInvokeStepStatement());

            return method.Build();
        }

        public override MethodDeclarationSyntax GenerateMethod()
        {
            const string result = nameof(result);
            const string yesNoResult = nameof(yesNoResult);

            return new MethodBuilder(returnType: "Task", methodName: this.GetMethodName())
                .WithAccessModifier(AccessModifier.Private)
                .MakeAsync()
                .WithParameters(
                    new Parameter(typeName: this.StaticResources.context.Type, parameterName: this.StaticResources.context.Name),
                    new Parameter(typeName: "IAwaitable<bool>", parameterName: result))
                .AddStatements(
                    LocalVariableDeclarationStatement.BuildWithAwaitedDeclaration("var", yesNoResult, $"{result}"),
                    new IfStatement(
                        yesNoResult,
                        new MultipleStatementBuilder()
                            .AddStatements(this.YesPath.GeneratePostAdditionalMessagesStatements())
                            .AddStatements(this.YesPath.GenerateInvokeStepStatement())
                            .Build())
                        .Else(new MultipleStatementBuilder()
                            .AddStatements(this.NoPath.GeneratePostAdditionalMessagesStatements())
                            .AddStatements(this.NoPath.GenerateInvokeStepStatement())
                            .Build())
                        .Build())
                .Build();
        }

        public override StatementSyntax GenerateInvokeStepStatement()
        {
            var questionResource = new StringResource($"{this.GetMethodName()}Question", this.Question);
            this.Resources.Add(questionResource);

            return MethodCallStatement.BuildCall(
                $"this.{this.StaticResources.promptInvoker.Confirm}",
                this.StaticResources.context.Name,
                $"{this.StaticResources.ResourcesClass}.{questionResource.Name}",
                $"this.{this.GetMethodName()}");
        }
    }
}