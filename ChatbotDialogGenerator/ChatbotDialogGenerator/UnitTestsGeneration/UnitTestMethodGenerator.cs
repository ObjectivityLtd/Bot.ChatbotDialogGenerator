namespace ChatbotDialogGenerator.UnitTestsGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ChatbotDialogGenerator.Configuration;
    using ChatbotDialogGenerator.Steps;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using RoslynWrapper.SyntaxBuilders.Common;
    using RoslynWrapper.SyntaxBuilders.Methods;
    using RoslynWrapper.SyntaxBuilders.Statements.Assignments;
    using RoslynWrapper.SyntaxBuilders.Statements.MethodCalls;

    public class UnitTestMethodGenerator : IUnitTestMethodGenerator
    {
        private readonly TestsConfiguration testsConfiguration;

        public UnitTestMethodGenerator(TestsConfiguration testConfiguration)
        {
            this.testsConfiguration = testConfiguration;
        }

        private const string StoryBuilder = "storyBuilder";
        private const string Story = "story";
        private const string User = "user";
        private const string Bot = "bot";
        private const string BotSays = Bot + ".Says";
        private const string BotGivesChoice = Bot + ".GivesChoice";
        private const string UserSays = User + ".Says";

        public MethodDeclarationSyntax GenerateMethod(EndStep step)
        {
            return this.GenerateMethod(step, new List<string>(), new List<StatementSyntax>());
        }

        private MethodDeclarationSyntax GenerateMethod(Step step, List<string> name, List<StatementSyntax> commands)
        {
            name.Add(step.Name);

            switch (step)
            {
                case YesNoStep yesNoStep:
                    this.AddYesNoCommands(commands, yesNoStep);
                    break;
                case DoneStep doneStep:
                    this.AddDoneCommands(commands);
                    break;
                case RedirectStep redirectStep:
                    this.AddRedirectCommands(commands, redirectStep);
                    break;
                case ChoiceStep choiceStep:
                    this.AddChoiceCommans(commands, choiceStep);
                    break;
                default:
                    throw new InvalidOperationException($"{step.GetType().Name} is no supported.");
            }

            this.AddCommandsForAdditionalMessages(step, commands);

            if (step.Parent != null)
            {
                commands.Add(MethodCallStatement.BuildCall(UserSays, $"\"{step.Name}\""));
                this.GenerateMethod(step.Parent, name, commands);
            }

            return this.GenerateMethodForPath(string.Join("_", name.ToArray().Reverse()), commands.ToArray().Reverse());
        }

        private void AddChoiceCommans(List<StatementSyntax> commands, ChoiceStep choiceStep) => commands.Add(
            MethodCallStatement.BuildCall(BotGivesChoice,
                $"\"{choiceStep.Question}\", new[] {{{string.Join(",", choiceStep.ChoiceSteps.Select(s => "\"" + s.Key.Description + "\""))}}}"));

        private void AddCommandsForAdditionalMessages(Step step, List<StatementSyntax> commands) =>
            commands.AddRange(step.AdditionalMessages.ToArray().Reverse().Select(additionalMessage =>
                MethodCallStatement.BuildCall(BotSays,
                    $"\"{additionalMessage.Replace(Environment.NewLine, string.Empty)}\"")));

        private void AddRedirectCommands(List<StatementSyntax> commands, RedirectStep redirectStep)
        {
            commands.Add(VariableAssignmentStatement.Build($"var {Story}",
                MethodCallStatement.BuildCallWithoutLastTrivia($"{StoryBuilder}.Rewind").ToFullString()));
            commands.Add(MethodCallStatement.BuildCall(BotSays,
                $"$\">> Called {{nameof({redirectStep.TargetDialogName})}}\""));
        }

        private void AddDoneCommands(List<StatementSyntax> commands) =>
            commands.Add(VariableAssignmentStatement.Build($"var {Story}",
                MethodCallStatement.BuildCallWithoutLastTrivia(
                        $"{StoryBuilder}.DialogDoneWithResult<{this.testsConfiguration.UnitTestResultObject}>",
                        this.testsConfiguration.UnitTestDonePredicate)
                    .ToFullString()));

        private void AddYesNoCommands(List<StatementSyntax> commands, YesNoStep yesNoStep)
        {
            commands.Add(MethodCallStatement.BuildCall(BotSays, $"\"{yesNoStep.Question}\""));
        }

        private MethodDeclarationSyntax GenerateMethodForPath(string name, IEnumerable<StatementSyntax> commands)
        {
            var storyAssignment = VariableAssignmentStatement.Build($"var {StoryBuilder}", "new StoryRecorder()");
            var botAssignment = VariableAssignmentStatement.Build($"var {Bot}", $"{StoryBuilder}.Bot");
            var userAssignment = VariableAssignmentStatement.Build($"var {User}", $"{StoryBuilder}.User");

            return new MethodBuilder("Task", name)
                .WithAccessModifier(AccessModifier.Public)
                .MakeAsync()
                .AddStatements(storyAssignment)
                .AddStatements(botAssignment)
                .AddStatements(userAssignment)
                .AddStatements(commands.ToArray())
                .AddStatements(MethodCallStatement.BuildAwaitedCall("this.Play", Story))
                .WithAttribute("Fact").Build();
        }
    }
}