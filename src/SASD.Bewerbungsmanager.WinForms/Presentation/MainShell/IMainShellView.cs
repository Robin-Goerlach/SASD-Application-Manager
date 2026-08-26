namespace SASD.Bewerbungsmanager.WinForms.Presentation.MainShell;

/// <summary>
/// View contract for the main application shell. The presenter does not need a concrete Form.
/// </summary>
public interface IMainShellView
{
    /// <summary>Sets the current status text shown to the user.</summary>
    /// <param name="text">Human-readable status.</param>
    void SetStatus(string text);
}
