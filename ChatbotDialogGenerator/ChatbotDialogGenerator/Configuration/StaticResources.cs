// ReSharper disable InconsistentNaming
namespace ChatbotDialogGenerator.Configuration
{
    public class StaticResources
    {
        public DialogContextResources context => new DialogContextResources();

        public PromptInvokerResources promptInvoker => new PromptInvokerResources();

        public DialogInvokerResources dialogInvoker => new DialogInvokerResources();

        public string ResourcesClass => "ResourcesAccessor";

        public string ResourcesFile => "Resources";

        public string Export => nameof(Export);

        public string StartAsync => nameof(StartAsync);
    }

    public class PromptInvokerResources
    {
        public string Name => "promptInvoker";

        public string Type => "IPromptInvoker";

        public string Choice => $"{Name}.Choice";

        public string Confirm => $"{Name}.Confirm";

        public override string ToString()
        {
            return Name;
        }
    }

    public class DialogInvokerResources
    {
        public string Name => "dialogInvoker";

        public string Type => "IDialogInvoker";

        public string Done => $"{Name}.Done";

        public override string ToString()
        {
            return Name;
        }
    }

    public class DialogContextResources
    {
        public string Name => "context";

        public string Type => "IDialogContext";

        public string Done => $"{Name}.Done";

        public string PostAsync => $"{Name}.PostAsync";

        public string PostAsSeperateBubblesAsync => $"{Name}.PostAsSeperateBubblesAsync";

        public override string ToString()
        {
            return Name;
        }
    }
}