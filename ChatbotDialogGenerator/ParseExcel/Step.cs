using System.Collections.Generic;
/// <summary>
/// These should be used from other place - its just for testing purposes
/// </summary>
namespace ParseExcel
{
    public abstract class Step
    {
        public List<string> AdditionalMessages { get; set; }
        public string Name { get; set; }

        public int Id { get; set; }
    }

    public abstract class QuestionStep : Step
    {
        public string Question { get; set; }
    }

    public class YesNoStep : QuestionStep
    {
        public Step YesPath { get; set; }
        public Step NoPath { get; set; }
    }

    public class ChoiceStep : QuestionStep
    {
        public Dictionary<ChoiceDescription, Step> ChoiceSteps { get; set; }
    }

    public class EndStep : Step
    {
    }

    public class ChoiceDescription
    {
        public int Number { get; set; }
        public string Description { get; set; }
    }
}