using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Migrations
{
    /// <inheritdoc />
    public partial class seeddata_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var seedDate = new DateTime(2026, 1, 1);

            // 1. UserTypes (FIRST dependency for User)
            migrationBuilder.InsertData(
                table: "SPM_UserType",
                columns: new[] { "UserTypeID", "UserTypeName", "Description", "CreatedBy", "CreatedOn" },
                values: new object[,]
                {
                    { 1, "Admin", "Full system access", 0, seedDate },
                    { 2, "Faculty", "Supervises student projects", 0, seedDate },
                    { 3, "Student", "Works on assigned projects", 0, seedDate }
                });

            // 2. Roles
            migrationBuilder.InsertData(
                table: "SPM_Role",
                columns: new[] { "RoleID", "RoleName", "Description", "CreatedBy", "CreatedOn" },
                values: new object[,]
                {
                    { 1, "Admin", "Full system management access", 0, seedDate },
                    { 2, "Faculty", "Supervises and evaluates student projects", 0, seedDate },
                    { 3, "Student", "Works on assigned projects and tasks", 0, seedDate },
                    { 4, "Guest", "Read-only limited access", 0, seedDate }
                });

            // 3. Task Status Master
            migrationBuilder.InsertData(
                table: "SPM_TaskStatus",
                columns: new[] { "TaskStatusID", "TaskStatusName", "TaskStatusCssClass", "CreatedBy", "CreatedOn" },
                values: new object[,]
                {
                    { 1, "Pending", "tone-warning", 0, seedDate },
                    { 2, "Ongoing", "tone-info", 0, seedDate },
                    { 3, "Completed", "tone-success", 0, seedDate },
                    { 4, "Cancelled", "tone-danger", 0, seedDate }
                });

            // 4. Task Priority Master
            migrationBuilder.InsertData(
                table: "SPM_TaskPriority",
                columns: new[] { "TaskPriorityID", "TaskPriorityName", "TaskPriortyCssClass", "CreatedBy", "CreatedOn" },
                values: new object[,]
                {
                    { 1, "Low", "tone-neutral", 0, seedDate },
                    { 2, "Moderate", "tone-warning", 0, seedDate },
                    { 3, "Critical", "tone-danger", 0, seedDate }
                });

            // 5. Project Master (10 Projects)
            migrationBuilder.InsertData(
                table: "SPM_ProjectMaster",
                columns: new[] { "ProjectID", "ProjectTitle", "Description", "CreatedBy", "CreatedOn" },
                values: new object[,]
                {
                    { 1, "Online Examination System", "A web-based exam management system", 0, seedDate },
                    { 2, "Hospital Management System", "System to manage hospital records and appointments", 0, seedDate },
                    { 3, "Library Management System", "Digital library book tracking and management", 0, seedDate },
                    { 4, "E-Commerce Platform", "Online shopping portal with cart and payment", 0, seedDate },
                    { 5, "Student Attendance Tracker", "Mobile app for tracking student attendance", 0, seedDate },
                    { 6, "Restaurant Billing System", "POS system for restaurant orders and billing", 0, seedDate },
                    { 7, "Blood Bank Management System", "Manage blood donations, donors and requests", 0, seedDate },
                    { 8, "Job Portal", "Platform connecting recruiters and job seekers", 0, seedDate },
                    { 9, "Inventory Management System", "Stock tracking and inventory control for business", 0, seedDate },
                    { 10, "Social Media Dashboard", "Analytics dashboard for social media accounts", 0, seedDate }
                });

            // 6. Users (Depends on UserTypeID 1, 2, 3)
            migrationBuilder.InsertData(
                table: "SPM_User",
                columns: new[] { "UserID", "FullName", "UserCode", "Email", "Password", "MobileNumber", "ProfilePicturePath", "IsActive", "IsDeleted", "UserTypeID", "CreatedBy", "CreatedOn" },
                values: new object[,]
                {
                    { 1, "Admin User", "ADM001", "admin@spms.com", "admin123", "9000000000", "/images/default-user.png", true, false, 1, 0, seedDate },
                    { 2, "Ravi Mehta", "F001", "ravi.mehta@spms.com", "faculty123", "9100000001", "/images/default-user.png", true, false, 2, 0, seedDate },
                    { 3, "Priya Shah", "F002", "priya.shah@spms.com", "faculty123", "9100000002", "/images/default-user.png", true, false, 2, 0, seedDate },
                    { 4, "Dinesh Patel", "F003", "dinesh.patel@spms.com", "faculty123", "9100000003", "/images/default-user.png", true, false, 2, 0, seedDate },
                    { 5, "Sonal Joshi", "F004", "sonal.joshi@spms.com", "faculty123", "9100000004", "/images/default-user.png", true, false, 2, 0, seedDate },
                    { 6, "Het Patel", "25010101622", "het.patel@spms.com", "student123", "9200000001", "/images/default-user.png", true, false, 3, 0, seedDate },
                    { 7, "Vasu Desai", "25010101623", "vasu.desai@spms.com", "student123", "9200000002", "/images/default-user.png", true, false, 3, 0, seedDate },
                    { 8, "Kartavya Hindocha", "25010101682", "kartavya.hindocha@spms.com", "student123", "9200000003", "/images/default-user.png", true, false, 3, 0, seedDate },
                    { 9, "Dev Shah", "25010101625", "dev.shah@spms.com", "student123", "9200000004", "/images/default-user.png", true, false, 3, 0, seedDate },
                    { 10, "Neel Trivedi", "25010101626", "neel.trivedi@spms.com", "student123", "9200000005", "/images/default-user.png", true, false, 3, 0, seedDate },
                    { 11, "Jay Chauhan", "25010101627", "jay.chauhan@spms.com", "student123", "9200000006", "/images/default-user.png", true, false, 3, 0, seedDate },
                    { 12, "Raj Solanki", "25010101628", "raj.solanki@spms.com", "student123", "9200000007", "/images/default-user.png", true, false, 3, 0, seedDate },
                    { 13, "Om Parmar", "25010101629", "om.parmar@spms.com", "student123", "9200000008", "/images/default-user.png", true, false, 3, 0, seedDate },
                    { 14, "Krish Rathod", "25010101630", "krish.rathod@spms.com", "student123", "9200000009", "/images/default-user.png", true, false, 3, 0, seedDate },
                    { 15, "Manan Jain", "25010101631", "manan.jain@spms.com", "student123", "9200000010", "/images/default-user.png", true, false, 3, 0, seedDate }
                });

            // 7. Project Allocations
            migrationBuilder.InsertData(
                table: "SPM_ProjectAllocation",
                columns: new[] { "ProjectAllocationID", "AssignedDate", "ProjectStartDate", "ProjectEndDate", "TotalTasksGiven", "TotalCompletedTasks", "ProgressPercentage", "OverAllGrade", "ProjectID", "StudentID", "FacultyID", "CreatedBy", "CreatedOn" },
                values: new object[,]
                {
                    { 1, seedDate, seedDate, seedDate.AddDays(90), 5, 2, 40.00m, "B", 1, 6, 2, 0, seedDate },
                    { 2, seedDate, seedDate, seedDate.AddDays(90), 5, 4, 80.00m, "A", 2, 7, 3, 0, seedDate },
                    { 3, seedDate, seedDate, seedDate.AddDays(90), 5, 1, 20.00m, "C", 3, 8, 4, 0, seedDate },
                    { 4, seedDate, seedDate, seedDate.AddDays(90), 5, 5, 100.00m, "A", 4, 9, 5, 0, seedDate },
                    { 5, seedDate, seedDate, seedDate.AddDays(90), 5, 3, 60.00m, "B", 5, 10, 2, 0, seedDate },
                    { 6, seedDate, seedDate, seedDate.AddDays(90), 5, 0, 0.00m, "F", 6, 11, 3, 0, seedDate },
                    { 7, seedDate, seedDate, seedDate.AddDays(90), 5, 2, 40.00m, "C", 7, 12, 4, 0, seedDate },
                    { 8, seedDate, seedDate, seedDate.AddDays(90), 5, 4, 80.00m, "A", 8, 13, 5, 0, seedDate },
                    { 9, seedDate, seedDate, seedDate.AddDays(90), 5, 3, 60.00m, "B", 9, 14, 2, 0, seedDate },
                    { 10, seedDate, seedDate, seedDate.AddDays(90), 5, 1, 20.00m, "D", 10, 15, 3, 0, seedDate }
                });

            // 8. User Roles
            migrationBuilder.InsertData(
                table: "SPM_UserRole",
                columns: new[] { "RolePermissionID", "RoleID", "UserID", "CreatedBy", "CreatedOn" },
                values: new object[,]
                {
                    { 1, 1, 1, 0, seedDate },
                    { 2, 2, 2, 0, seedDate },
                    { 3, 2, 3, 0, seedDate },
                    { 4, 2, 4, 0, seedDate },
                    { 5, 2, 5, 0, seedDate },
                    { 6, 3, 6, 0, seedDate },
                    { 7, 3, 7, 0, seedDate },
                    { 8, 3, 8, 0, seedDate },
                    { 9, 3, 9, 0, seedDate },
                    { 10, 3, 10, 0, seedDate },
                    { 11, 3, 11, 0, seedDate },
                    { 12, 3, 12, 0, seedDate },
                    { 13, 3, 13, 0, seedDate },
                    { 14, 3, 14, 0, seedDate },
                    { 15, 3, 15, 0, seedDate }
                });

            // 9. Tasks
            migrationBuilder.InsertData(
                table: "SPM_Task",
                columns: new[] { "TaskID", "TaskTitle", "TaskDescription", "AssignedScore", "EarnedScore", "ProgressPercentage", "TaskAssignedDate", "TaskStartDate", "TaskDueDate", "TaskCompletedDate", "NextFollowUpDate", "FacultyRemarks", "StudentRemarks", "ProjectAllocationID", "TaskStatusID", "TaskPriorityID", "CreatedBy", "CreatedOn" },
                values: new object[,]
                {
                    { 1, "Requirement Gathering", "Complete requirements documentation", 10.00m, 8.00m, 80.00m, seedDate, seedDate, seedDate.AddDays(15), seedDate.AddDays(10), seedDate.AddDays(12), "Good initial work", "Understood specs", 1, 3, 2, 0, seedDate },
                    { 2, "Database Design", "Create ERD and schema script", 10.00m, 9.00m, 90.00m, seedDate, seedDate, seedDate.AddDays(15), seedDate.AddDays(14), seedDate.AddDays(15), "Well structured schema", "Schema finalized", 2, 3, 3, 0, seedDate },
                    { 3, "UI Wireframing", "Design Figma mockups for core pages", 10.00m, 5.00m, 50.00m, seedDate, seedDate, seedDate.AddDays(15), null, seedDate.AddDays(10), "Needs cleaner layout", "Working on dashboard screen", 3, 2, 1, 0, seedDate },
                    { 4, "Backend API Development", "Build CRUD REST APIs with EF Core", 10.00m, 10.00m, 100.00m, seedDate, seedDate, seedDate.AddDays(20), seedDate.AddDays(18), seedDate.AddDays(20), "Excellent work!", "All endpoints tested", 4, 3, 3, 0, seedDate },
                    { 5, "Frontend Integration", "Connect Razor views with backend API", 10.00m, 6.00m, 60.00m, seedDate, seedDate, seedDate.AddDays(25), null, seedDate.AddDays(22), "Focus on error handling", "Integrating POST endpoints", 5, 2, 2, 0, seedDate },
                    { 6, "Unit Testing", "Write xUnit tests for controller layer", 10.00m, 0.00m, 0.00m, seedDate, null, seedDate.AddDays(30), null, seedDate.AddDays(25), "Please start testing soon", "Not started yet", 6, 1, 1, 0, seedDate },
                    { 7, "Integration Testing", "Test flow between client and server", 10.00m, 4.00m, 40.00m, seedDate, seedDate, seedDate.AddDays(30), null, seedDate.AddDays(28), "Check edge cases", "Found 2 minor issues", 7, 2, 2, 0, seedDate },
                    { 8, "Deployment Setup", "Configure IIS and SQL Server hosting", 10.00m, 9.00m, 90.00m, seedDate, seedDate, seedDate.AddDays(35), seedDate.AddDays(32), seedDate.AddDays(35), "Smooth deployment", "App live on staging environment", 8, 3, 3, 0, seedDate },
                    { 9, "Documentation", "Write user manual and technical guide", 10.00m, 7.00m, 70.00m, seedDate, seedDate, seedDate.AddDays(40), null, seedDate.AddDays(38), "Add architectural diagrams", "Draft ready", 9, 2, 1, 0, seedDate },
                    { 10, "Final Presentation Preparation", "Prepare slides and demo video", 10.00m, 2.00m, 20.00m, seedDate, seedDate, seedDate.AddDays(45), null, seedDate.AddDays(40), "Practice timing", "Outline created", 10, 2, 2, 0, seedDate }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
