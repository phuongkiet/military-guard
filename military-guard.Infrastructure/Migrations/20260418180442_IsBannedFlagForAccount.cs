using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace military_guard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IsBannedFlagForAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBanned",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBanned",
                table: "Accounts");
        }
    }
}
