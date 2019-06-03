namespace ChatbotDialogGenerator.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ChatbotDialogGenerator.Configuration;
    using ChatbotDialogGenerator.Utils;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using RoslynWrapper.SyntaxBuilders.Common;
    using RoslynWrapper.SyntaxBuilders.Common.Parameters;
    using RoslynWrapper.SyntaxBuilders.Methods;
    using RoslynWrapper.SyntaxBuilders.Statements;
    using RoslynWrapper.SyntaxBuilders.Statements.Conditions;
    using RoslynWrapper.SyntaxBuilders.Statements.Declarations;
    using RoslynWrapper.SyntaxBuilders.Statements.MethodCalls;

    public class ChoiceStep : QuestionStep
    {
        public ChoiceStep(StaticResources staticResources, CommonConfiguration commonConfiguration, string question, Dictionary<ChoiceDescription, Step> choiceSteps)
            : base(staticResources, commonConfiguration, question)
        {
            this.ChoiceSteps = choiceSteps;
        }

        public Dictionary<ChoiceDescription, Step> ChoiceSteps { get; }

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
            const string choice = nameof(choice);

            return new MethodBuilder(returnType: "Task", methodName: this.GetMethodName())
                .WithAccessModifier(AccessModifier.Private)
                .MakeAsync()
                .WithParameters(
                    new Parameter(typeName: this.StaticResources.context.Type, parameterName: this.StaticResources.context.Name),
                    new Parameter(typeName: "IAwaitable<string>", parameterName: result))
                .AddStatements(
                    LocalVariableDeclarationStatement.BuildWithAwaitedDeclaration(variableType: "var", variableName: choice, rightValue: $"{result}"),
                    this.BuildElseIfStatement(parameterName: choice))
                .Build();
        }

        private StatementSyntax BuildElseIfStatement(string parameterName)
        {
            if (this.ChoiceSteps.Count == 0)
            {
                throw new InvalidOperationException("Choices collection should not be empty");
            }

            var firstChoice = this.ChoiceSteps.First();

            var choiceResource = new StringResource($"{this.GetMethodName()}Choice{firstChoice.Key.Description.RemoveNonAlphanumeric()}", firstChoice.Key.Description);
            this.Resources.Add(choiceResource);

            var ifStatement = new IfStatement(
                condition: $"{parameterName} == {this.StaticResources.ResourcesClass}.{choiceResource.Name}",
                statements: new MultipleStatementBuilder()
                    .AddStatements(firstChoice.Value.GeneratePostAdditionalMessagesStatements())
                    .AddStatements(firstChoice.Value.GenerateInvokeStepStatement())
                    .Build());

            foreach (var choice in this.ChoiceSteps.Except(firstChoice))
            {
                choiceResource = new StringResource($"{this.GetMethodName()}Choice{choice.Key.Description.RemoveNonAlphanumeric()}", choice.Key.Description);
                this.Resources.Add(choiceResource);

                ifStatement = ifStatement.ElseIf(
                    condition: $"{parameterName} == {this.StaticResources.ResourcesClass}.{choiceResource.Name}",
                    statements: new MultipleStatementBuilder()
                        .AddStatements(choice.Value.GeneratePostAdditionalMessagesStatements())
                        .AddStatements(choice.Value.GenerateInvokeStepStatement())
                        .Build());
            }

            return ifStatement.Build();
        }

        public override StatementSyntax GenerateInvokeStepStatement()
        {
            var questionResource = new StringResource($"{this.GetMethodName()}Question", this.Question);
            this.Resources.Add(questionResource);

            var squashedOptions = string.Join(", ", this.ChoiceSteps.Keys.Select(choice => choice.Description).ToArray());
            var optionsResource = new StringResource($"{this.GetMethodName()}Choices", squashedOptions);
            this.Resources.Add(optionsResource);

            return MethodCallStatement.BuildCall(
                $"this.{this.StaticResources.promptInvoker.Choice}",
                this.StaticResources.context.Name,
                $"{this.StaticResources.ResourcesClass}.{questionResource.Name}",
                $"{this.StaticResources.ResourcesClass}.{optionsResource.Name}",
                $"this.{this.GetMethodName()}");
        }
    }

    public class ChoiceDescription
    {
        public ChoiceDescription(string description)
        {
            this.Description = description;
        }

        public string Description { get; }
    }
}