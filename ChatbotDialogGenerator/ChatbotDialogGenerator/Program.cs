namespace ChatbotDialogGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ChatbotDialogGenerator.Configuration;
    using ChatbotDialogGenerator.Generation.DetailsProviders;
    using ChatbotDialogGenerator.Generation.DialogGeneration;
    using ChatbotDialogGenerator.Generation.InterfaceGeneration;
    using ChatbotDialogGenerator.Generation.ResourcesGeneration;
    using ChatbotDialogGenerator.Steps;
    using ChatbotDialogGenerator.StepTreeHelpers;
    using ChatbotDialogGenerator.UnitTestsGeneration;
    using NLog;
    using ParseExcel;
    using ParseExcel.Configuration;
    using ParseExcel.Luis;
    using RoslynWrapper.SolutionExtender;

    public static class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static readonly StaticResources StaticResources = new StaticResources();
        private static readonly CommonConfiguration CommonConfiguration = new CommonConfiguration();
        private static readonly TestsConfiguration TestsConfiguration = new TestsConfiguration();

        private static readonly StepBuilder StepBuilder = new StepBuilder(CommonConfiguration, StaticResources);

        private static readonly DuplicateStepsFilter DuplicateStepsFilter = new DuplicateStepsFilter();
        private static readonly StepsTreeToListConverter StepsTreeToListConverter = new StepsTreeToListConverter();

        private static readonly DialogGenerator DialogGenerator = new DialogGenerator();
        private static readonly InterfaceGenerator InterfaceGenerator = new InterfaceGenerator();
        private static readonly ResourceGenerator ResourceGenerator = new ResourceGenerator();

        private static readonly SolutionExtender SolutionExtender = new SolutionExtender();
        private static readonly UnitTestMethodGenerator UnitTestMethodGenerator = new UnitTestMethodGenerator(TestsConfiguration);
        private static readonly UnitTestForDialogGenerator UnitTestGenerator = new UnitTestForDialogGenerator(UnitTestMethodGenerator, CommonConfiguration, TestsConfiguration);

        private static readonly DialogInterfaceDetailsProvider DialogInterfaceDetailsProvider = new DialogInterfaceDetailsProvider(CommonConfiguration);
        private static readonly ResourcesDetailsProvider ResourcesDetailsProvider = new ResourcesDetailsProvider(CommonConfiguration, StaticResources);
        private static readonly SolutionDetailsProvider SolutionDetailsProvider = new SolutionDetailsProvider(CommonConfiguration);
        private static readonly ProjectConfigurationProvider ProjectConfigurationProvider = new ProjectConfigurationProvider(CommonConfiguration, StaticResources, TestsConfiguration);

        private static readonly DialogDetailsProvider DialogDetailsProvider = new DialogDetailsProvider(CommonConfiguration, DialogInterfaceDetailsProvider, ResourcesDetailsProvider);

        public static async Task Main(string[] args)
        {
            NLogConfig.Configure();
            if (!CommonConfiguration.IsComplete() || !TestsConfiguration.IsComplete() || !ExcelConfiguration.IsComplete())
            {
                Logger.Info(() => "Configuration is not complete. Program will be terminated.");
                Console.WriteLine("Configuration is not complete. Program will be terminated.");
                return;
            }

            var trees = GetDialogTreesFromFile();

            foreach (var head in trees)
            {
                Console.WriteLine($"Processing {head.Name}...");

                await GenerateInterface(head);
                await GenerateDialog(head);
                await GenerateTests(head);

                Console.WriteLine($"{head.Name} has been processed.");
                Console.WriteLine();
            }
        }

        private static List<Step> GetDialogTreesFromFile()
        {
            var path = CommonConfiguration.ExcelFilePath;

            IInputParser parser = new DialogExcelParser(new UtteranceFinder(), new NodeTypeCheckerBasedOnNextCell());
            return parser.ParseInputFromFile(path).Select(new ConverterExcelCellToStep(CommonConfiguration, StepBuilder).ConvertToStep).ToList();
        }

        private static async Task GenerateInterface(Step head)
        {
            var dialogName = head.Name;
            var interfaceDetails = DialogInterfaceDetailsProvider.GetDialogInterfaceDetails(dialogName);
            var interfaceCode = InterfaceGenerator.GenerateInterface(interfaceDetails);

            var interfaceConfiguration = ProjectConfigurationProvider.GetInterfaceConfiguration(dialogName);
            await SolutionExtender.TryGenerateAddCompilationUnitToProjectAsync(interfaceConfiguration, interfaceCode);
        }

        private static async Task GenerateDialog(Step head)
        {
            var dialogName = head.Name;
            var dialogDetails = DialogDetailsProvider.GetDialogDetails(dialogName);

            var stepsList = StepsTreeToListConverter.ConvertToList(head);
            var stepsQueued = DuplicateStepsFilter.RemoveDuplicatesAndOrderByStepId(stepsList);
            var dialog = DialogGenerator.GenerateDialog(stepsQueued, dialogDetails);

            var dialogConfiguration = ProjectConfigurationProvider.GetDialogConfiguration(dialogName);
            await SolutionExtender.TryGenerateAddCompilationUnitToProjectAsync(dialogConfiguration, dialog.GeneratedCode);

            var resourcesConfiguration = ProjectConfigurationProvider.GetResourcesConfiguration(dialogName);
            await GenerateResources(resourcesConfiguration, dialog.Resources, dialogName);
        }

        private static async Task GenerateResources(ProjectConfiguration projectConfiguration, List<StringResource> resources, string dialogName)
        {
            var csprojPath = SolutionDetailsProvider.GetDialogCsprojPath();
            var resourcesPath = ResourcesDetailsProvider.GetResourcesPath(dialogName);

            ResourceGenerator.GenerateResxFile(resources, resourcesPath);
            SolutionExtender.TryAddFileToProjectAsync(csprojPath, resourcesPath);

            var resourceDetails = ResourcesDetailsProvider.GetResourcesDetails(dialogName);

            var resourcesAccessorCode = ResourceGenerator.GenerateResourcesAccessor(resources, resourceDetails);
            await SolutionExtender.TryGenerateAddCompilationUnitToProjectAsync(projectConfiguration, resourcesAccessorCode);
        }

        private static async Task GenerateTests(Step head)
        {
            var testsConfiguration = ProjectConfigurationProvider.GetTestProjectConfiguration(head.Name);

            var unitTestsCode = UnitTestGenerator.GenerateUnitTestsForDialog(head);
            await SolutionExtender.TryGenerateAddCompilationUnitToProjectAsync(testsConfiguration, unitTestsCode);
        }
    }
}
