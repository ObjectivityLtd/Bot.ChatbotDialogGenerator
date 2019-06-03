namespace ChatbotDialogGenerator.Steps
{
    using System.Collections.Generic;
    using ChatbotDialogGenerator.Configuration;

    public class StepBuilder
    {
        private readonly CommonConfiguration commonConfiguration;
        private readonly StaticResources _staticResources;

        public StepBuilder(CommonConfiguration commonConfiguration, StaticResources staticResources)
        {
            this.commonConfiguration = commonConfiguration;
            this._staticResources = staticResources;
        }

        public DoneStep DoneStep(string action)
        {
            return new DoneStep(this._staticResources, this.commonConfiguration, action);
        }

        public RedirectStep RedirectStep(string targetDialogName)
        {
            return new RedirectStep(this._staticResources, this.commonConfiguration, targetDialogName);
        }

        public YesNoStep YesNoStep(string question, Step yesPath, Step noPath)
        {
            return new YesNoStep(this._staticResources, this.commonConfiguration, question, yesPath, noPath);
        }

        public ChoiceStep ChoiceStep(string question, Dictionary<ChoiceDescription, Step> choiceSteps)
        {
            return new ChoiceStep(this._staticResources, this.commonConfiguration, question, choiceSteps);
        }
    }
}