namespace ChatbotDialogGenerator.StepTreeHelpers
{
    using System;
    using System.Collections.Generic;
    using ChatbotDialogGenerator.Steps;

    public class StepsTreeToListConverter
    {
        private List<Step> _steps;
        
        public List<Step> ConvertToList(Step head)
        {
            _steps = new List<Step>();

            FlattenTree(head);

            return _steps;
        }

        private void FlattenTree(Step step)
        {
            switch (step)
            {
                case EndStep endStep:
                    _steps.Add(endStep);
                    break;
                case ChoiceStep choiceStep:
                    _steps.Add(choiceStep);
                    foreach (var choice in choiceStep.ChoiceSteps)
                        FlattenTree(choice.Value);
                    break;
                case YesNoStep yesNoStep:
                    _steps.Add(yesNoStep);
                    FlattenTree(yesNoStep.YesPath);
                    FlattenTree(yesNoStep.NoPath);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(step));
            }
        }
    }
}