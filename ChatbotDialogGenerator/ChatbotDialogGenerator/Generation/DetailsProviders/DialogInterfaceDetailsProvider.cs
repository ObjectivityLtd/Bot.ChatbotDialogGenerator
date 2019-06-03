namespace ChatbotDialogGenerator.Generation.DetailsProviders
{
    using ChatbotDialogGenerator.Configuration;
    using ChatbotDialogGenerator.Generation.InterfaceGeneration;

    public class DialogInterfaceDetailsProvider
    {
        private readonly CommonConfiguration _commonConfiguration;

        public DialogInterfaceDetailsProvider(CommonConfiguration commonConfiguration)
        {
            this._commonConfiguration = commonConfiguration;
        }

        public DialogInterfaceDetails GetDialogInterfaceDetails(string dialogName)
        {
            return new DialogInterfaceDetails
            {
                NamespaceName = $"{this._commonConfiguration.DialogNamespaceName}.{dialogName}",
                BaseInterface = this._commonConfiguration.BaseDialogInterface,
                Usings = this._commonConfiguration.InterfaceUsings,
                InterfaceName = $"I{this._commonConfiguration.DialogClassName(dialogName)}"
            };
        }
    }
}