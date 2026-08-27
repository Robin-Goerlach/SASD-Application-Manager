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

    [Theory]
    [InlineData(WorkItemKind.Action, "ACTION")]
    [InlineData(WorkItemKind.WaitingFor, "WAITING_FOR")]
    public void WorkItemKind_ReturnsOperationalLabel(WorkItemKind kind, string expected)
    {
        Assert.Equal(expected, DisplayText.WorkItemKind(kind));
    }

    [Theory]
    [InlineData(ActivityKind.Interview, "Interview")]
    [InlineData(ActivityKind.AuthorityAppointment, "Behördentermin")]
    [InlineData(ActivityKind.PhoneCall, "Telefonat")]
    public void ActivityKind_ReturnsGermanLabel(ActivityKind kind, string expected)
    {
        Assert.Equal(expected, DisplayText.ActivityKind(kind));
    }
}
