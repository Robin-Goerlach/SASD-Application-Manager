using Xunit;
using SASD.Bewerbungsmanager.Domain.Enums;
using SASD.Bewerbungsmanager.WinForms.Presentation;

namespace SASD.Bewerbungsmanager.Presentation.Tests;

public sealed class DisplayTextTests
{
    [Theory]
    [InlineData(ApplicationStage.Draft, "Entwurf")]
    [InlineData(ApplicationStage.Submitted, "Versendet")]
    [InlineData(ApplicationStage.Interview, "Interview")]
    [InlineData(ApplicationStage.Rejected, "Absage")]
    public void ApplicationStage_ReturnsStableGermanUiLabel(ApplicationStage stage, string expected)
    {
        Assert.Equal(expected, DisplayText.ApplicationStage(stage));
    }
}
