using System.Reflection;
using System.Xml.Linq;
using Promotion.Core;
using Promotion.Core.Contracts;
using Xunit;

namespace Promotion.ArchitectureTests;

public sealed class ArchitectureTests
{
    [Fact]
    public void ARC_001_CoreHasNoForbiddenProjectOrAssemblyDependency()
    {
        var root = FindSolutionRoot();
        var coreProject = XDocument.Load(Path.Combine(root, "src", "Promotion.Core", "Promotion.Core.csproj"));
        var refs = coreProject.Descendants("ProjectReference")
            .Select(element => (string?)element.Attribute("Include")).ToArray();
        Assert.Single(refs);
        Assert.Contains("Promotion.Core.Contracts.csproj", refs[0]);

        var forbidden = new[] { "MysqlAcess", "MySql", "Promotion.Data.MySql", "DBCache", "Tests" };
        var assemblyNames = typeof(PromotionResultFactory).Assembly.GetReferencedAssemblies()
            .Select(assembly => assembly.Name ?? string.Empty).ToArray();
        foreach (var name in assemblyNames)
            Assert.DoesNotContain(forbidden, item => name.Contains(item, StringComparison.OrdinalIgnoreCase));

        foreach (var projectPath in Directory.GetFiles(Path.Combine(root, "src"), "*.csproj", SearchOption.AllDirectories))
        {
            var project = XDocument.Load(projectPath);
            foreach (var reference in project.Descendants("ProjectReference"))
                Assert.DoesNotContain("Tests", (string?)reference.Attribute("Include") ?? string.Empty);
        }
        Assert.NotNull(typeof(PromotionResult<>).Assembly);
    }

    private static string FindSolutionRoot()
    {
        var path = new DirectoryInfo(AppContext.BaseDirectory);
        while (path is not null && !File.Exists(Path.Combine(path.FullName, "PromotionCore.sln")))
            path = path.Parent;
        return path?.FullName ?? throw new InvalidOperationException("Solution root not found.");
    }
}
