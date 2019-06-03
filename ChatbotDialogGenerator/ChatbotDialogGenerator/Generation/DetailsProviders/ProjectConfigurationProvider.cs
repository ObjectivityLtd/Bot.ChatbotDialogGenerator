namespace ChatbotDialogGenerator.Generation.DetailsProviders
{
    using ChatbotDialogGenerator.Configuration;
    using RoslynWrapper.SolutionExtender;

    public class ProjectConfigurationProvider
    {
        private readonly CommonConfiguration _commonConfiguration;
        private readonly StaticResources _staticResources;
        private readonly TestsConfiguration _testsConfiguration;

        public ProjectConfigurationProvider(CommonConfiguration commonConfiguration, StaticResources staticResources, TestsConfiguration testsConfiguration)
        {
            this._commonConfiguration = commonConfiguration;
            this._staticResources = staticResources;
            this._testsConfiguration = testsConfiguration;
        }

        public ProjectConfiguration GetDialogConfiguration(string dialogName)
        {
            return new ProjectConfiguration
            {
                SolutionPath = this._commonConfiguration.SolutionPath,
                SolutionName = this._commonConfiguration.SolutionName,
                ProjectName = this._commonConfiguration.DialogProjectName,
                FolderPath = $@"{this._commonConfiguration.InterfaceFolderPath}\{dialogName}",
                FileName = this._commonConfiguration.DialogClassName(dialogName)
            };
        }

        public ProjectConfiguration GetResourcesConfiguration(string dialogName)
        {
            return new ProjectConfiguration
            {
                SolutionPath = this._commonConfiguration.SolutionPath,
                SolutionName = this._commonConfiguration.SolutionName,
                ProjectName = this._commonConfiguration.DialogProjectName,
                FolderPath = $@"{this._commonConfiguration.DialogFolderPath}\{dialogName}",
                FileName = this._staticResources.ResourcesClass
            };
        }

        public ProjectConfiguration GetInterfaceConfiguration(string dialogName)
        {
            return new ProjectConfiguration
            {
                SolutionPath = this._commonConfiguration.SolutionPath,
                SolutionName = this._commonConfiguration.SolutionName,
                ProjectName = this._commonConfiguration.InterfaceProjectName,
                FolderPath = $@"{this._commonConfiguration.DialogFolderPath}\{dialogName}",
                FileName = $"I{this._commonConfiguration.DialogClassName(dialogName)}"
            };
        }

        public ProjectConfiguration GetTestProjectConfiguration(string dialogName)
        {
            return new ProjectConfiguration
            {
                SolutionPath = this._commonConfiguration.SolutionPath,
                SolutionName = this._commonConfiguration.SolutionName,
                ProjectName = this._testsConfiguration.UnitTestsProjectName,
                FolderPath = $@"{this._testsConfiguration.UnitTestsFolderPath}\{dialogName}",
                FileName = $"{this._testsConfiguration.UnitTestClassName(dialogName)}"
            };
        }
    }
}