namespace RoslynWrapper.SolutionExtender
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Build.Locator;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.MSBuild;

    public class SolutionExtender
    {
        public SolutionExtender()
        {
            MSBuildLocator.RegisterDefaults();
        }

        public async Task TryGenerateAddCompilationUnitToProjectAsync(ProjectConfiguration configuration, CompilationUnitSyntax compilationUnit)
        {
            var solutionPath = configuration.SolutionPath;
            var solutionName = configuration.SolutionName;
            var projectName = configuration.ProjectName;
            var documentPath = $@"{configuration.FolderPath}\{configuration.FileName}.cs";

            var workspace = MSBuildWorkspace.Create();

            var solution = await workspace.OpenSolutionAsync($@"{solutionPath}\{solutionName}");
            var project = solution.Projects.FirstOrDefault(n => n.Name == projectName);

            if(project == null)
                throw new InvalidOperationException($"Requested project doesn't exits. Requested project name: {projectName}");

            var newDocument = project.AddDocument(documentPath, compilationUnit.NormalizeWhitespace().ToFullString());

            workspace.TryApplyChanges(newDocument.Project.Solution);
        }

        public static void TryAddFileToProjectAsync(string csprojPath, string documentFullPath)
        {
            var p = new Microsoft.Build.Evaluation.Project(csprojPath);
            p.AddItem("EmbeddedResource", documentFullPath);
            p.Save();

            p.ProjectCollection.UnloadProject(p);
        }
    }
}