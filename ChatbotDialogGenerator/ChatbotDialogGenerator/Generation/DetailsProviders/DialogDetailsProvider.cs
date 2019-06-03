namespace ChatbotDialogGenerator.Generation.DetailsProviders
{
    using System.Collections.Generic;
    using System.Linq;
    using ChatbotDialogGenerator.Configuration;
    using ChatbotDialogGenerator.Generation.DialogGeneration;

    public class DialogDetailsProvider
    {
        private readonly CommonConfiguration _commonConfiguration;
        private readonly DialogInterfaceDetailsProvider _dialogInterfaceDetailsProvider;
        private readonly ResourcesDetailsProvider _resourcesDetailsProvider;

        public DialogDetailsProvider(
            CommonConfiguration commonConfiguration,
            DialogInterfaceDetailsProvider dialogInterfaceDetailsProvider,
            ResourcesDetailsProvider resourcesDetailsProvider)
        {
            this._commonConfiguration = commonConfiguration;
            this._dialogInterfaceDetailsProvider = dialogInterfaceDetailsProvider;
            this._resourcesDetailsProvider = resourcesDetailsProvider;
        }

        public DialogDetails GetDialogDetails(string dialogName)
        {
            var dialogNamespace = this._commonConfiguration.DialogNamespaceName;
            var details = new DialogDetails
            {
                NamespaceName = dialogNamespace,
                Usings = this.GetDialogUsings(dialogName, dialogNamespace),
                DialogName = this._commonConfiguration.DialogClassName(dialogName)
            };

            return details;
        }

        private string[] GetDialogUsings(string dialogName, string dialogNamespace)
        {
            var dialogUsings = this._commonConfiguration.DialogUsings.ToList();

            var dialogInterfaceNamespace = this._dialogInterfaceDetailsProvider.GetDialogInterfaceDetails(dialogName).NamespaceName;
            if (dialogNamespace != dialogInterfaceNamespace && !dialogUsings.Contains(dialogInterfaceNamespace))
            {
                dialogUsings.Add(dialogInterfaceNamespace);
            }

            var resourcesInterfaceNamespace = this._resourcesDetailsProvider.GetResourcesDetails(dialogName).NamespaceName;
            if (dialogNamespace != resourcesInterfaceNamespace && !dialogUsings.Contains(resourcesInterfaceNamespace))
            {
                dialogUsings.Add(resourcesInterfaceNamespace);
            }

            return dialogUsings.ToArray();
        }
    }
}