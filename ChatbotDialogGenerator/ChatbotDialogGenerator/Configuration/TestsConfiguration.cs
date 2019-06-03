namespace ChatbotDialogGenerator.Configuration
{
    using System;
    using System.Linq;
    using Utils;

    public class TestsConfiguration
    {
        public string UnitTestsProjectName => UnitTests.Default.UnitTestsProjectName;

        public string UnitTestsFolderPath => UnitTests.Default.UnitTestsFolderPath;

        public string UnitTestNamespace => UnitTests.Default.UnitTestNamespace;

        public string UnitTestBaseClass => UnitTests.Default.UnitTestBaseClass;

        public string UnitTestDonePredicate => UnitTests.Default.UnitTestDonePredicate;

        public string UnitTestResultObject => CommonSettings.Default.ResultObject;

        public string[] UnitTestsUsings => ConfigurationUtils.GetUsingsFromSetting(UnitTests.Default.UnitTestsUsings);

        public Func<string, string> UnitTestClassName => step => $"{step}Tests";

        public bool IsComplete() => !UnitTestsProjectName.IsNullOrWhitespace() &&
                                           !UnitTestsFolderPath.IsNullOrWhitespace() &&
                                           !UnitTestNamespace.IsNullOrWhitespace() &&
                                           !UnitTestBaseClass.IsNullOrWhitespace() &&
                                           !UnitTestDonePredicate.IsNullOrWhitespace() &&
                                           !UnitTestResultObject.IsNullOrWhitespace() &&
                                           UnitTestsUsings.Any();
    }
}