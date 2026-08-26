using Microsoft.Extensions.Logging;
using SASD.Bewerbungsmanager.Application.Exceptions;

namespace SASD.Bewerbungsmanager.WinForms.Presentation;

/// <summary>
/// Converts application failures into concise user-visible messages while sending technical detail
/// to logging. Sensitive business data is deliberately not embedded in log messages here.
/// </summary>
public sealed class UiExceptionPresenter(ILogger<UiExceptionPresenter> logger)
{
    /// <summary>Shows a suitable error message for a failure raised by a UI operation.</summary>
    public void Show(Exception exception, IWin32Window? owner = null)
    {
        ArgumentNullException.ThrowIfNull(exception);

        var message = exception switch
        {
            ValidationException => exception.Message,
            KeyNotFoundException => exception.Message,
            _ => "Die Aktion konnte nicht abgeschlossen werden. Details wurden protokolliert.",
        };

        logger.LogError(exception, "UI operation failed with {ExceptionType}.", exception.GetType().Name);
        MessageBox.Show(owner, message, "SASD Bewerbungsmanager", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
