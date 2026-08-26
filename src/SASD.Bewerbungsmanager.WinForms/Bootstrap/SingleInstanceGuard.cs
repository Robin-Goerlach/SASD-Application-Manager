namespace SASD.Bewerbungsmanager.WinForms.Bootstrap;

/// <summary>
/// Prevents accidental concurrent application instances for the same Windows session.
/// Named-pipe activation forwarding remains an explicit M0 follow-up.
/// </summary>
internal sealed class SingleInstanceGuard : IDisposable
{
    private readonly Mutex _mutex;
    private readonly bool _ownsMutex;

    private SingleInstanceGuard(Mutex mutex, bool ownsMutex)
    {
        _mutex = mutex;
        _ownsMutex = ownsMutex;
    }

    /// <summary>Gets whether this process owns the application instance mutex.</summary>
    public bool IsPrimaryInstance => _ownsMutex;

    /// <summary>Attempts to acquire the application instance mutex.</summary>
    public static SingleInstanceGuard TryAcquire()
    {
        var mutex = new Mutex(
            initiallyOwned: true,
            name: @"Local\SASD.Bewerbungsmanager",
            createdNew: out var createdNew);

        return new SingleInstanceGuard(mutex, createdNew);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_ownsMutex)
        {
            _mutex.ReleaseMutex();
        }

        _mutex.Dispose();
    }
}
