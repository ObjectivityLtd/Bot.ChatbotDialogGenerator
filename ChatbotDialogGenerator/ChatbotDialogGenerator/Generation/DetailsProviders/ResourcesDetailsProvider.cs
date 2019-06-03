namespace ChatbotDialogGenerator.Generation.DetailsProviders
{
    using ChatbotDialogGenerator.Configuration;
    using ChatbotDialogGenerator.Generation.ResourcesGeneration;

    public class ResourcesDetailsProvider
    {
        private readonly CommonConfiguration _commonConfiguration;
        private readonly StaticResources _staticResources;

        public ResourcesDetailsProvider(CommonConfiguration commonConfiguration, StaticResources staticResources)
        {
            this._commonConfiguration = commonConfiguration;
            this._staticResources = staticResources;
        }

        public ResourcesDetails GetResourcesDetails(string dialogName)
        {
            return new ResourcesDetails
            {
                ClassName = this._staticResources.ResourcesClass,
                NamespaceName = $"{this._commonConfiguration.DialogNamespaceName}.{this._commonConfiguration.ResourcesNamespaceName(dialogName)}",
                Usings = this._commonConfiguration.ResourceAccessorUsings,
                ResourcesBaseName = this.GetResourcesBaseName(dialogName)
            };
        }

        private string GetResourcesBaseName(string dialogName)
        {
            var projectName = this._commonConfiguration.DialogProjectName;
            var folderPath = $@"{this._commonConfiguration.DialogFolderPath}\{dialogName}";
            var fileName = this._staticResources.ResourcesFile;

            return $"{projectName}.{folderPath.Replace(@"\", ".")}.{fileName}";
        }

        public string GetResourcesPath(string dialogName)
        {
            var solutionPath = this._commonConfiguration.SolutionPath;
            var projectName = this._commonConfiguration.DialogProjectName;
            var folderPath = $@"{this._commonConfiguration.DialogFolderPath}\{dialogName}";
            var fileName = this._staticResources.ResourcesFile;

            return $@"{solutionPath}\{projectName}\{folderPath}\{fileName}.resx";
        }
    }
}