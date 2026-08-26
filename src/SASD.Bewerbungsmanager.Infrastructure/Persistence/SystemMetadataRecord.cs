namespace SASD.Bewerbungsmanager.Infrastructure.Persistence;

/// <summary>
/// Infrastructure-owned operational metadata. This is deliberately not a business-domain entity.
/// </summary>
public sealed class SystemMetadataRecord
{
    /// <summary>Gets or sets the surrogate key.</summary>
    public int Id { get; set; }

    /// <summary>Gets or sets the stable metadata key.</summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>Gets or sets the metadata value.</summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>Gets or sets the UTC time of the last update.</summary>
    public DateTime UpdatedAtUtc { get; set; }
}
