using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Migrations
{
    /// <inheritdoc />
    public partial class initdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SPM_Priority",
                columns: table => new
                {
                    PriorityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PriorityName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PriorityCssClass = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SPM_Priority", x => x.PriorityID);
                });

            migrationBuilder.CreateTable(
                name: "SPM_Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SPM_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "SPM_Status",
                columns: table => new
                {
                    StatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StatusCssClass = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SPM_Status", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "SPM_User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    ProfilePicturePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SPM_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "SPM_Task",
                columns: table => new
                {
                    TaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TaskDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    EarnedScore = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    ProgressPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FacultyRemarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StudentRemarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    AllocationID = table.Column<int>(type: "int", nullable: false),
                    TaskStatus = table.Column<int>(type: "int", nullable: false),
                    PriorityID = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SPM_Task", x => x.TaskId);
                    table.ForeignKey(
                        name: "FK_SPM_Task_SPM_Priority_PriorityID",
                        column: x => x.PriorityID,
                        principalTable: "SPM_Priority",
                        principalColumn: "PriorityID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SPM_Task_SPM_Status_TaskStatus",
                        column: x => x.TaskStatus,
                        principalTable: "SPM_Status",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SPM_Project",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalTasks = table.Column<int>(type: "int", nullable: false),
                    CompletedTasks = table.Column<int>(type: "int", nullable: false),
                    ProgressPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    FacultyId = table.Column<int>(type: "int", nullable: false),
                    ProjectStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SPM_Project", x => x.ProjectId);
                    table.ForeignKey(
                        name: "FK_SPM_Project_SPM_Status_ProjectStatus",
                        column: x => x.ProjectStatus,
                        principalTable: "SPM_Status",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SPM_Project_SPM_User_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "SPM_User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SPM_Project_SPM_User_StudentId",
                        column: x => x.StudentId,
                        principalTable: "SPM_User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SPM_UserRole",
                columns: table => new
                {
                    RolePermissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SPM_UserRole", x => x.RolePermissionId);
                    table.ForeignKey(
                        name: "FK_SPM_UserRole_SPM_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "SPM_Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SPM_UserRole_SPM_User_UserId",
                        column: x => x.UserId,
                        principalTable: "SPM_User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "SPM_Priority",
                columns: new[] { "PriorityID", "CreatedBy", "CreatedOn", "PriorityCssClass", "PriorityName", "UpdatedBy", "UpdatedOn" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "tone-neutral", "Low", null, null },
                    { 2, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "tone-warning", "Medium", null, null },
                    { 3, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "tone-danger", "High", null, null },
                    { 4, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "tone-danger", "Critical", null, null }
                });

            migrationBuilder.InsertData(
                table: "SPM_Status",
                columns: new[] { "StatusID", "CreatedBy", "CreatedOn", "StatusCssClass", "StatusName", "UpdatedBy", "UpdatedOn" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "tone-neutral", "Not Started", null, null },
                    { 2, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "tone-warning", "Pending", null, null },
                    { 3, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "tone-info", "In Progress", null, null },
                    { 4, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "tone-success", "Completed", null, null },
                    { 5, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "tone-danger", "Rejected", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SPM_Priority_PriorityName",
                table: "SPM_Priority",
                column: "PriorityName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SPM_Project_FacultyId",
                table: "SPM_Project",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_SPM_Project_ProjectStatus",
                table: "SPM_Project",
                column: "ProjectStatus");

            migrationBuilder.CreateIndex(
                name: "IX_SPM_Project_StudentId",
                table: "SPM_Project",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_SPM_Status_StatusName",
                table: "SPM_Status",
                column: "StatusName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SPM_Task_PriorityID",
                table: "SPM_Task",
                column: "PriorityID");

            migrationBuilder.CreateIndex(
                name: "IX_SPM_Task_TaskStatus",
                table: "SPM_Task",
                column: "TaskStatus");

            migrationBuilder.CreateIndex(
                name: "IX_SPM_User_Email",
                table: "SPM_User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SPM_UserRole_RoleId",
                table: "SPM_UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SPM_UserRole_UserId",
                table: "SPM_UserRole",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SPM_Project");

            migrationBuilder.DropTable(
                name: "SPM_Task");

            migrationBuilder.DropTable(
                name: "SPM_UserRole");

            migrationBuilder.DropTable(
                name: "SPM_Priority");

            migrationBuilder.DropTable(
                name: "SPM_Status");

            migrationBuilder.DropTable(
                name: "SPM_Role");

            migrationBuilder.DropTable(
                name: "SPM_User");
        }
    }
}
