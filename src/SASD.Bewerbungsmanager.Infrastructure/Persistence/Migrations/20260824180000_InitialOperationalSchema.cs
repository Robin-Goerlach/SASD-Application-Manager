using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SASD.Bewerbungsmanager.Infrastructure.Persistence.Migrations;

/// <summary>
/// Creates the minimal operational schema required by the M0 architecture skeleton.
/// Product-domain tables are intentionally introduced only by their vertical feature slices.
/// </summary>
[DbContext(typeof(ApplicationDbContext))]
[Migration("20260824180000_InitialOperationalSchema")]
public sealed class InitialOperationalSchema : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        ArgumentNullException.ThrowIfNull(migrationBuilder);

        migrationBuilder.CreateTable(
            name: "SystemMetadata",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Key = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                Value = table.Column<string>(type: "TEXT", maxLength: 2_000, nullable: false),
                UpdatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SystemMetadata", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_SystemMetadata_Key",
            table: "SystemMetadata",
            column: "Key",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        ArgumentNullException.ThrowIfNull(migrationBuilder);
        migrationBuilder.DropTable(name: "SystemMetadata");
    }
}
