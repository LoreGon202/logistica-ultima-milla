using System.Reflection;
using Logistica.Application;
using Logistica.Domain;
using Logistica.Infrastructure;
using Xunit;

namespace Api.IntegrationTests;

// Fitness functions: comprobaciones automáticas de las reglas de arquitectura.
public class ArchitectureTests
{
    private static readonly Assembly DomainAssembly = typeof(Delivery).Assembly;
    private static readonly Assembly ApplicationAssembly = typeof(AssignCourierUseCase).Assembly;
    private static readonly Assembly InfrastructureAssembly = typeof(SqliteMigrator).Assembly;

    private static List<string> ReferencesOf(Assembly assembly) =>
        assembly.GetReferencedAssemblies()
            .Select(a => a.Name ?? string.Empty)
            .ToList();

    [Fact]
    public void Domain_no_depende_de_Application_ni_de_Infrastructure()
    {
        var referencias = ReferencesOf(DomainAssembly);
        Assert.DoesNotContain("Application", referencias);
        Assert.DoesNotContain("Infrastructure", referencias);
        Assert.DoesNotContain("Api", referencias);
    }

    [Fact]
    public void Domain_no_depende_de_SQLite_ni_de_AspNetCore()
    {
        var referencias = ReferencesOf(DomainAssembly);
        Assert.DoesNotContain("Microsoft.Data.Sqlite", referencias);
        Assert.DoesNotContain(referencias, r => r.StartsWith("Microsoft.AspNetCore"));
    }

    [Fact]
    public void Application_no_depende_de_Infrastructure_ni_de_SQLite()
    {
        var referencias = ReferencesOf(ApplicationAssembly);
        Assert.DoesNotContain("Infrastructure", referencias);
        Assert.DoesNotContain("Api", referencias);
        Assert.DoesNotContain("Microsoft.Data.Sqlite", referencias);
    }

    [Fact]
    public void Infrastructure_depende_de_Application_y_de_Domain()
    {
        var referencias = ReferencesOf(InfrastructureAssembly);
        Assert.Contains("Application", referencias);
        Assert.Contains("Domain", referencias);
    }
}