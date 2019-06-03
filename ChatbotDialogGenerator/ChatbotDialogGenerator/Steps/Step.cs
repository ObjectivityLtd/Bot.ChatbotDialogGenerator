namespace ChatbotDialogGenerator.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using ChatbotDialogGenerator.Configuration;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using RoslynWrapper.SyntaxBuilders.Statements.MethodCalls;

    public abstract class Step
    {

        private StaticResources staticResources;
        private readonly CommonConfiguration commonConfiguration;

        private List<string> _additionalMessages;
        private string _name;

        protected Step(StaticResources staticResources, CommonConfiguration commonConfiguration)
        {
            this.Resources = new List<StringResource>();
            this.staticResources = staticResources;
            this.commonConfiguration = commonConfiguration;
        }

        protected StaticResources StaticResources => this.staticResources;

        public List<StringResource> Resources { get; }

        public Step Parent { get; set; }

        public List<string> AdditionalMessages
        {
            get => _additionalMessages ?? new List<string>();
            set => _additionalMessages = value;
        }

        public string Name
        {
            get => _name;
            set => _name = Regex.Replace(value ?? string.Empty, @"[^0-9a-zA-Z_]", string.Empty);
        }

        public int Id { get; set; }

        public abstract MethodDeclarationSyntax GenerateStartMethod();

        public abstract MethodDeclarationSyntax GenerateMethod();

        public virtual StatementSyntax[] GeneratePostAdditionalMessagesStatements()
        {
            return this.commonConfiguration.UsePostAsSeparateBubble ?
                GenerateAsSquashedMessage() :
                GenerateAsSeparateMessages();
        }

        private StatementSyntax[] GenerateAsSquashedMessage()
        {
            var squashedMessages = string.Join("", AdditionalMessages.ToArray());
            if (!string.IsNullOrEmpty(squashedMessages))
            {
                var messagesResource = new StringResource($"{this.GetMethodName()}AdditionalMessages", squashedMessages);
                Resources.Add(messagesResource);
                return new[]
                {
                    MethodCallStatement.BuildAwaitedCall($"{this.StaticResources.context.PostAsSeperateBubblesAsync}", $"{this.StaticResources.ResourcesClass}.{messagesResource.Name}")
                };
            }

            return new StatementSyntax[0];
        }

        private StatementSyntax[] GenerateAsSeparateMessages()
        {
            var statements = new List<StatementSyntax>();

            var count = 0;
            foreach (var msg in AdditionalMessages)
            {
                var messageResource = new StringResource($"{this.GetMethodName()}AdditionalMessage{count++}",msg);
                Resources.Add(messageResource);

                statements.Add(
                    MethodCallStatement.BuildAwaitedCall($"{this.StaticResources.context.PostAsync}", $"{this.StaticResources.ResourcesClass}.{messageResource.Name}"));
            }

            return statements.ToArray();
        }

        public abstract StatementSyntax GenerateInvokeStepStatement();

        public string GetMethodName()
        {
            return $"Step{Id}{Name}";
        }
    }

    public abstract class QuestionStep : Step
    {
        protected QuestionStep(StaticResources staticResources, CommonConfiguration commonConfiguration, string question)
            : base(staticResources, commonConfiguration)
        {
            Question = question;
        }

        public string Question { get; }
    }

    public abstract class EndStep : Step
    {
        protected EndStep(StaticResources staticResources, CommonConfiguration commonConfiguration)
            : base(staticResources, commonConfiguration)
        {
        }
    }
}