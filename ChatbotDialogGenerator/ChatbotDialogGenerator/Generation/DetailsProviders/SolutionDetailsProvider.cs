namespace ChatbotDialogGenerator.Generation.DetailsProviders
{
    using ChatbotDialogGenerator.Configuration;

    public class SolutionDetailsProvider
    {
        private readonly CommonConfiguration _commonConfiguration;

        public SolutionDetailsProvider(CommonConfiguration commonConfiguration)
        {
            this._commonConfiguration = commonConfiguration;
        }

        public string GetDialogCsprojPath()
        {
            var solutionPath = this._commonConfiguration.SolutionPath;
            var projectName = this._commonConfiguration.DialogProjectName;

            return $@"{solutionPath}\{projectName}\{projectName}.csproj";
        }
    }
}