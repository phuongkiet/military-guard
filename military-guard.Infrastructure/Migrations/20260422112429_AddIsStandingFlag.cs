using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace military_guard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsStandingFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsStandby",
                table: "ShiftAssignments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsStandby",
                table: "ShiftAssignments");
        }
    }
}
