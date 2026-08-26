namespace SASD.Bewerbungsmanager.WinForms.Presentation.MainShell;

/// <summary>
/// Presenter for shell-level behavior. Product workflows are introduced only in later vertical slices.
/// </summary>
public sealed class MainShellPresenter
{
    private IMainShellView? _view;

    /// <summary>Attaches the view that receives presentation state.</summary>
    /// <param name="view">Shell view.</param>
    public void Attach(IMainShellView view)
    {
        _view = view ?? throw new ArgumentNullException(nameof(view));
    }

    /// <summary>Updates the shell once the window has loaded.</summary>
    public void OnLoaded()
    {
        _view?.SetStatus("M0 Architecture Skeleton – lokale Datenbank bereit");
    }
}
