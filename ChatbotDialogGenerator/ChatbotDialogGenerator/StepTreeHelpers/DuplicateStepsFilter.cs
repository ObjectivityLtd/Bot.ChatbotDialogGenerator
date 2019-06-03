namespace ChatbotDialogGenerator.StepTreeHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ChatbotDialogGenerator.Steps;

    public class DuplicateStepsFilter
    {
        private Dictionary<int, Step> _stepsToBeGenerated;
        
        public List<Step> RemoveDuplicatesAndOrderByStepId(List<Step> steps)
        {
            if (steps == null)
            {
                throw new ArgumentNullException(nameof(steps));
            }

            _stepsToBeGenerated = new Dictionary<int, Step>();

            foreach (var step in steps)
            {
                AddStepToQueue(step);
            }

            return _stepsToBeGenerated.Select(pair => pair.Value).ToList();
        }

        private void AddStepToQueue(Step step)
        {
            if(!_stepsToBeGenerated.ContainsKey(step.Id))
                _stepsToBeGenerated.Add(step.Id, step);
        }
    }
}