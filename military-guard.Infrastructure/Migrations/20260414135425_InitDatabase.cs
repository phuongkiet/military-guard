using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace military_guard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DutyShifts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    ShiftOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DutyShifts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuardPosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MinPersonnel = table.Column<int>(type: "int", nullable: false, defaultValue: 2),
                    MaxPersonnel = table.Column<int>(type: "int", nullable: false, defaultValue: 4),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuardPosts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Militias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    JoinDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Militias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Militias_Militias_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Militias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeaveRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MilitiaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ApproverId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_Militias_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "Militias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_Militias_MilitiaId",
                        column: x => x.MilitiaId,
                        principalTable: "Militias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShiftAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MilitiaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GuardPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DutyShiftId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    IsLeader = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShiftAssignments_DutyShifts_DutyShiftId",
                        column: x => x.DutyShiftId,
                        principalTable: "DutyShifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShiftAssignments_GuardPosts_GuardPostId",
                        column: x => x.GuardPostId,
                        principalTable: "GuardPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShiftAssignments_Militias_MilitiaId",
                        column: x => x.MilitiaId,
                        principalTable: "Militias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_ApproverId",
                table: "LeaveRequests",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_MilitiaId",
                table: "LeaveRequests",
                column: "MilitiaId");

            migrationBuilder.CreateIndex(
                name: "IX_Militias_Email",
                table: "Militias",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Militias_ManagerId",
                table: "Militias",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAssignments_DutyShiftId",
                table: "ShiftAssignments",
                column: "DutyShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAssignments_GuardPostId",
                table: "ShiftAssignments",
                column: "GuardPostId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAssignments_MilitiaId_Date_DutyShiftId",
                table: "ShiftAssignments",
                columns: new[] { "MilitiaId", "Date", "DutyShiftId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeaveRequests");

            migrationBuilder.DropTable(
                name: "ShiftAssignments");

            migrationBuilder.DropTable(
                name: "DutyShifts");

            migrationBuilder.DropTable(
                name: "GuardPosts");

            migrationBuilder.DropTable(
                name: "Militias");
        }
    }
}
