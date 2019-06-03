namespace ChatbotDialogGenerator.Steps
{
    using ChatbotDialogGenerator.Configuration;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using RoslynWrapper.SyntaxBuilders.Common;
    using RoslynWrapper.SyntaxBuilders.Common.Parameters;
    using RoslynWrapper.SyntaxBuilders.Methods;
    using RoslynWrapper.SyntaxBuilders.Statements.MethodCalls;

    public class DoneStep : EndStep
    {
        public DoneStep(StaticResources staticResources, CommonConfiguration commonConfiguration, string action)
            : base(staticResources, commonConfiguration)
        {
            Action = action;
        }

        public string Action { get; }

        public override MethodDeclarationSyntax GenerateStartMethod()
        {
            return new MethodBuilder(returnType: "Task", methodName: this.StaticResources.StartAsync)
                .WithAccessModifier(AccessModifier.Public)
                .MakeAsync()
                .WithParameters(
                    new Parameter(typeName: this.StaticResources.context.Type, parameterName: this.StaticResources.context.Name))
                .AddStatements(this.GeneratePostAdditionalMessagesStatements())
                .AddStatements(
                    MethodCallStatement.BuildAwaitedCall($"this.{this.GetMethodName()}", this.StaticResources.context.Name))
                .Build();
        }

        public override MethodDeclarationSyntax GenerateMethod()
        {
            return new MethodBuilder(returnType: "Task", methodName: this.GetMethodName())
                .WithAccessModifier(AccessModifier.Private)
                .MakeAsync()
                .WithParameters(
                    new Parameter(typeName: this.StaticResources.context.Type, parameterName: this.StaticResources.context.Name))
                .AddStatements(
                    MethodCallStatement.BuildCall($"{this.StaticResources.context.Done}", this.Action))
                .Build();
        }

        public override StatementSyntax GenerateInvokeStepStatement()
        {
            return MethodCallStatement.BuildAwaitedCall($"this.{this.GetMethodName()}", this.StaticResources.context.Name);
        }
    }
}