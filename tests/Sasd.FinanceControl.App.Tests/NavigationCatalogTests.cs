using Sasd.FinanceControl.App.Presentation;
using Xunit;

namespace Sasd.FinanceControl.App.Tests;

public sealed class NavigationCatalogTests
{
    [Fact]
    public void Get_Dashboard_ReturnsImplementedMilestoneNinePage()
    {
        var page = NavigationCatalog.Get(NavigationTarget.Dashboard);

        Assert.Equal("Dashboard", page.Title);
        Assert.True(page.IsImplemented);
        Assert.Equal("Milestone 9", page.RoadmapPhase);
    }

    [Theory]
    [InlineData(NavigationTarget.Suppliers)]
    [InlineData(NavigationTarget.Categories)]
    [InlineData(NavigationTarget.Documents)]
    [InlineData(NavigationTarget.Banking)]
    [InlineData(NavigationTarget.Payments)]
    [InlineData(NavigationTarget.Reconciliation)]
    [InlineData(NavigationTarget.Contracts)]
    [InlineData(NavigationTarget.Invoices)]
    [InlineData(NavigationTarget.Orders)]
    public void Get_ImplementedPagesThroughMilestoneNine_AreEnabled(NavigationTarget target)
    {
        Assert.True(NavigationCatalog.Get(target).IsImplemented);
    }

    [Fact]
    public void Get_Banking_IsImplementedInPhaseFour()
    {
        var page = NavigationCatalog.Get(NavigationTarget.Banking);

        Assert.True(page.IsImplemented);
        Assert.Contains("Phase 4", page.RoadmapPhase);
        Assert.Contains("manuell", page.Description, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Get_Invoices_IsImplementedInPhaseSeven()
    {
        var page = NavigationCatalog.Get(NavigationTarget.Invoices);

        Assert.True(page.IsImplemented);
        Assert.Equal("Rechnungen", page.Title);
        Assert.Contains("Phase 7", page.RoadmapPhase);
    }


    [Fact]
    public void Get_Reconciliation_IsImplementedInPhaseEight()
    {
        var page = NavigationCatalog.Get(NavigationTarget.Reconciliation);

        Assert.True(page.IsImplemented);
        Assert.Equal("Zahlungsabgleich", page.Title);
        Assert.Contains("Phase 8", page.RoadmapPhase);
    }

    [Fact]
    public void Get_Orders_IsImplementedInPhaseNine()
    {
        var page = NavigationCatalog.Get(NavigationTarget.Orders);

        Assert.True(page.IsImplemented);
        Assert.Equal("Bestellungen", page.Title);
        Assert.Contains("Phase 9", page.RoadmapPhase);
    }

    [Fact]
    public void Get_UnknownEnumValue_ThrowsArgumentOutOfRangeException()
    {
        var action = () => NavigationCatalog.Get((NavigationTarget)999);

        Assert.Throws<ArgumentOutOfRangeException>(action);
    }
}
