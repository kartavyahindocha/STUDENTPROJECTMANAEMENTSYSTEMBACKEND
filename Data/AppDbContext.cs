using Microsoft.EntityFrameworkCore;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserType> UserTypes => Set<UserType>();
        public DbSet<User> Users => Set<User>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<TaskStatusMaster> TaskStatus => Set<TaskStatusMaster>();
        public DbSet<TaskPriorityMaster> TaskPriorities => Set<TaskPriorityMaster>();
        public DbSet<ProjectMaster> ProjectMasters => Set<ProjectMaster>();
        public DbSet<ProjectAllocation> ProjectAllocations => Set<ProjectAllocation>();
        public DbSet<Tasks> Tasks => Set<Tasks>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ================= KEYS =================
            modelBuilder.Entity<Role>().HasKey(r => r.RoleID);
            modelBuilder.Entity<UserType>().HasKey(ut => ut.UserTypeID);
            modelBuilder.Entity<User>().HasKey(u => u.UserID);
            modelBuilder.Entity<UserRole>().HasKey(ur => ur.RolePermissionID);
            modelBuilder.Entity<TaskStatusMaster>().HasKey(s => s.TaskStatusID);
            modelBuilder.Entity<TaskPriorityMaster>().HasKey(p => p.TaskPriorityID);
            modelBuilder.Entity<ProjectMaster>().HasKey(p => p.ProjectID);
            modelBuilder.Entity<ProjectAllocation>().HasKey(pa => pa.ProjectAllocationID);
            modelBuilder.Entity<Tasks>().HasKey(t => t.TaskID);

            // ================= UNIQUE CONSTRAINTS =================
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Role>().HasIndex(r => r.RoleName).IsUnique();
            modelBuilder.Entity<UserType>().HasIndex(ut => ut.UserTypeName).IsUnique();
            modelBuilder.Entity<TaskStatusMaster>().HasIndex(s => s.TaskStatusName).IsUnique();
            modelBuilder.Entity<TaskPriorityMaster>().HasIndex(p => p.TaskPriorityName).IsUnique();

            // ================= DEFAULTS =================
            modelBuilder.Entity<ProjectAllocation>()
                .Property(pa => pa.AssignedDate)
                .HasDefaultValueSql("GETDATE()");

            // ================= RELATIONSHIPS =================
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserType)
                .WithMany(ut => ut.Users)
                .HasForeignKey(u => u.UserTypeID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectAllocation>()
                .HasOne(pa => pa.ProjectMaster)
                .WithMany(pm => pm.ProjectAllocations)
                .HasForeignKey(pa => pa.ProjectID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectAllocation>()
                .HasOne(pa => pa.Student)
                .WithMany(u => u.ProjectAllocationsAsStudent)
                .HasForeignKey(pa => pa.StudentID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectAllocation>()
                .HasOne(pa => pa.Faculty)
                .WithMany(u => u.ProjectAllocationsAsFaculty)
                .HasForeignKey(pa => pa.FacultyID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tasks>()
                .HasOne(t => t.ProjectAllocation)
                .WithMany(pa => pa.Tasks)
                .HasForeignKey(t => t.ProjectAllocationID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tasks>()
                .HasOne(t => t.TaskStatus)
                .WithMany(s => s.Tasks)
                .HasForeignKey(t => t.TaskStatusID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tasks>()
                .HasOne(t => t.TaskPriority)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.TaskPriorityID)
                .OnDelete(DeleteBehavior.Restrict);

            // ================= MIGRATION SEEDING IN EXPLICIT DEPENDENCY ORDER =================
            var seedDate = new DateTime(2026, 1, 1);

            // STEP 1: Independent Lookup Masters (No Foreign Keys)
            modelBuilder.Entity<UserType>().HasData(
                new UserType { UserTypeID = 1, UserTypeName = "Admin", Description = "Full system access", CreatedOn = seedDate },
                new UserType { UserTypeID = 2, UserTypeName = "Faculty", Description = "Supervises student projects", CreatedOn = seedDate },
                new UserType { UserTypeID = 3, UserTypeName = "Student", Description = "Works on assigned projects", CreatedOn = seedDate }
            );

            modelBuilder.Entity<Role>().HasData(
                new Role { RoleID = 1, RoleName = "Admin", Description = "Full system management access", CreatedOn = seedDate },
                new Role { RoleID = 2, RoleName = "Faculty", Description = "Supervises and evaluates student projects", CreatedOn = seedDate },
                new Role { RoleID = 3, RoleName = "Student", Description = "Works on assigned projects and tasks", CreatedOn = seedDate },
                new Role { RoleID = 4, RoleName = "Guest", Description = "Read-only limited access", CreatedOn = seedDate }
            );

            modelBuilder.Entity<TaskStatusMaster>().HasData(
                new TaskStatusMaster { TaskStatusID = 1, TaskStatusName = "Pending", TaskStatusCssClass = "tone-warning", CreatedOn = seedDate },
                new TaskStatusMaster { TaskStatusID = 2, TaskStatusName = "Ongoing", TaskStatusCssClass = "tone-info", CreatedOn = seedDate },
                new TaskStatusMaster { TaskStatusID = 3, TaskStatusName = "Completed", TaskStatusCssClass = "tone-success", CreatedOn = seedDate },
                new TaskStatusMaster { TaskStatusID = 4, TaskStatusName = "Cancelled", TaskStatusCssClass = "tone-danger", CreatedOn = seedDate },
                new TaskStatusMaster { TaskStatusID = 101, TaskStatusName = "Not Started", TaskStatusCssClass = "tone-neutral", CreatedOn = seedDate },
                new TaskStatusMaster { TaskStatusID = 102, TaskStatusName = "Under Review", TaskStatusCssClass = "tone-primary", CreatedOn = seedDate },
                new TaskStatusMaster { TaskStatusID = 103, TaskStatusName = "On Hold", TaskStatusCssClass = "tone-neutral", CreatedOn = seedDate },
                new TaskStatusMaster { TaskStatusID = 104, TaskStatusName = "Assigned", TaskStatusCssClass = "tone-success", CreatedOn = seedDate },
                new TaskStatusMaster { TaskStatusID = 105, TaskStatusName = "Unassigned", TaskStatusCssClass = "tone-warning", CreatedOn = seedDate }
            );

            modelBuilder.Entity<TaskPriorityMaster>().HasData(
                new TaskPriorityMaster { TaskPriorityID = 1, TaskPriorityName = "Low", TaskPriortyCssClass = "tone-neutral", CreatedOn = seedDate },
                new TaskPriorityMaster { TaskPriorityID = 2, TaskPriorityName = "Moderate", TaskPriortyCssClass = "tone-warning", CreatedOn = seedDate },
                new TaskPriorityMaster { TaskPriorityID = 3, TaskPriorityName = "Critical", TaskPriortyCssClass = "tone-danger", CreatedOn = seedDate }
            );

            modelBuilder.Entity<ProjectMaster>().HasData(
                new ProjectMaster { ProjectID = 1, ProjectTitle = "Online Examination System", Description = "A web-based exam management system", CreatedOn = seedDate },
                new ProjectMaster { ProjectID = 2, ProjectTitle = "Hospital Management System", Description = "System to manage hospital records and appointments", CreatedOn = seedDate },
                new ProjectMaster { ProjectID = 3, ProjectTitle = "Library Management System", Description = "Digital library book tracking and management", CreatedOn = seedDate },
                new ProjectMaster { ProjectID = 4, ProjectTitle = "E-Commerce Platform", Description = "Online shopping portal with cart and payment", CreatedOn = seedDate },
                new ProjectMaster { ProjectID = 5, ProjectTitle = "Student Attendance Tracker", Description = "Mobile app for tracking student attendance", CreatedOn = seedDate },
                new ProjectMaster { ProjectID = 6, ProjectTitle = "Restaurant Billing System", Description = "POS system for restaurant orders and billing", CreatedOn = seedDate },
                new ProjectMaster { ProjectID = 7, ProjectTitle = "Blood Bank Management System", Description = "Manage blood donations, donors and requests", CreatedOn = seedDate },
                new ProjectMaster { ProjectID = 8, ProjectTitle = "Job Portal", Description = "Platform connecting recruiters and job seekers", CreatedOn = seedDate },
                new ProjectMaster { ProjectID = 9, ProjectTitle = "Inventory Management System", Description = "Stock tracking and inventory control for business", CreatedOn = seedDate },
                new ProjectMaster { ProjectID = 10, ProjectTitle = "Social Media Dashboard", Description = "Analytics dashboard for social media accounts", CreatedOn = seedDate }
            );

            // STEP 2: Users (Depends on UserTypeID 1, 2, 3)
            modelBuilder.Entity<User>().HasData(
                new User { UserID = 1, FullName = "Admin User", UserCode = "ADM001", Email = "admin@spms.com", Password = "admin123", MobileNumber = "9000000000", ProfilePicturePath = "/images/default-user.png", IsActive = true, IsDeleted = false, UserTypeID = 1, CreatedOn = seedDate },
                new User { UserID = 2, FullName = "Ravi Mehta", UserCode = "F001", Email = "ravi.mehta@spms.com", Password = "faculty123", MobileNumber = "9100000001", ProfilePicturePath = "/images/default-user.png", IsActive = true, IsDeleted = false, UserTypeID = 2, CreatedOn = seedDate },
                new User { UserID = 3, FullName = "Priya Shah", UserCode = "F002", Email = "priya.shah@spms.com", Password = "faculty123", MobileNumber = "9100000002", ProfilePicturePath = "/images/default-user.png", IsActive = true, IsDeleted = false, UserTypeID = 2, CreatedOn = seedDate },
                new User { UserID = 4, FullName = "Dinesh Patel", UserCode = "F003", Email = "dinesh.patel@spms.com", Password = "faculty123", MobileNumber = "9100000003", ProfilePicturePath = "/images/default-user.png", IsActive = true, IsDeleted = false, UserTypeID = 2, CreatedOn = seedDate },
                new User { UserID = 5, FullName = "Sonal Joshi", UserCode = "F004", Email = "sonal.joshi@spms.com", Password = "faculty123", MobileNumber = "9100000004", ProfilePicturePath = "/images/default-user.png", IsActive = true, IsDeleted = false, UserTypeID = 2, CreatedOn = seedDate },
                new User { UserID = 6, FullName = "Het Patel", UserCode = "25010101622", Email = "het.patel@spms.com", Password = "student123", MobileNumber = "9200000001", ProfilePicturePath = "/images/default-user.png", IsActive = true, IsDeleted = false, UserTypeID = 3, CreatedOn = seedDate },
                new User { UserID = 7, FullName = "Vasu Desai", UserCode = "25010101623", Email = "vasu.desai@spms.com", Password = "student123", MobileNumber = "9200000002", ProfilePicturePath = "/images/default-user.png", IsActive = true, IsDeleted = false, UserTypeID = 3, CreatedOn = seedDate },
                new User { UserID = 8, FullName = "Kartavya Hindocha", UserCode = "25010101682", Email = "kartavya.hindocha@spms.com", Password = "student123", MobileNumber = "9200000003", ProfilePicturePath = "/images/default-user.png", IsActive = true, IsDeleted = false, UserTypeID = 3, CreatedOn = seedDate },
                new User { UserID = 9, FullName = "Dev Shah", UserCode = "25010101625", Email = "dev.shah@spms.com", Password = "student123", MobileNumber = "9200000004", ProfilePicturePath = "/images/default-user.png", IsActive = true, IsDeleted = false, UserTypeID = 3, CreatedOn = seedDate },
                new User { UserID = 10, FullName = "Neel Trivedi", UserCode = "25010101626", Email = "neel.trivedi@spms.com", Password = "student123", MobileNumber = "9200000005", ProfilePicturePath = "/images/default-user.png", IsActive = true, IsDeleted = false, UserTypeID = 3, CreatedOn = seedDate },
                new User { UserID = 11, FullName = "Jay Chauhan", UserCode = "25010101627", Email = "jay.chauhan@spms.com", Password = "student123", MobileNumber = "9200000006", ProfilePicturePath = "/images/default-user.png", IsActive = true, IsDeleted = false, UserTypeID = 3, CreatedOn = seedDate },
                new User { UserID = 12, FullName = "Raj Solanki", UserCode = "25010101628", Email = "raj.solanki@spms.com", Password = "student123", MobileNumber = "9200000007", ProfilePicturePath = "/images/default-user.png", IsActive = true, IsDeleted = false, UserTypeID = 3, CreatedOn = seedDate },
                new User { UserID = 13, FullName = "Om Parmar", UserCode = "25010101629", Email = "om.parmar@spms.com", Password = "student123", MobileNumber = "9200000008", ProfilePicturePath = "/images/default-user.png", IsActive = true, IsDeleted = false, UserTypeID = 3, CreatedOn = seedDate },
                new User { UserID = 14, FullName = "Krish Rathod", UserCode = "25010101630", Email = "krish.rathod@spms.com", Password = "student123", MobileNumber = "9200000009", ProfilePicturePath = "/images/default-user.png", IsActive = true, IsDeleted = false, UserTypeID = 3, CreatedOn = seedDate },
                new User { UserID = 15, FullName = "Manan Jain", UserCode = "25010101631", Email = "manan.jain@spms.com", Password = "student123", MobileNumber = "9200000010", ProfilePicturePath = "/images/default-user.png", IsActive = true, IsDeleted = false, UserTypeID = 3, CreatedOn = seedDate }
            );

            // STEP 3: UserRole (Depends on User & Role)
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { RolePermissionID = 1, RoleID = 1, UserID = 1, CreatedOn = seedDate },
                new UserRole { RolePermissionID = 2, RoleID = 2, UserID = 2, CreatedOn = seedDate },
                new UserRole { RolePermissionID = 3, RoleID = 2, UserID = 3, CreatedOn = seedDate },
                new UserRole { RolePermissionID = 4, RoleID = 2, UserID = 4, CreatedOn = seedDate },
                new UserRole { RolePermissionID = 5, RoleID = 2, UserID = 5, CreatedOn = seedDate },
                new UserRole { RolePermissionID = 6, RoleID = 3, UserID = 6, CreatedOn = seedDate },
                new UserRole { RolePermissionID = 7, RoleID = 3, UserID = 7, CreatedOn = seedDate },
                new UserRole { RolePermissionID = 8, RoleID = 3, UserID = 8, CreatedOn = seedDate },
                new UserRole { RolePermissionID = 9, RoleID = 3, UserID = 9, CreatedOn = seedDate },
                new UserRole { RolePermissionID = 10, RoleID = 3, UserID = 10, CreatedOn = seedDate },
                new UserRole { RolePermissionID = 11, RoleID = 3, UserID = 11, CreatedOn = seedDate },
                new UserRole { RolePermissionID = 12, RoleID = 3, UserID = 12, CreatedOn = seedDate },
                new UserRole { RolePermissionID = 13, RoleID = 3, UserID = 13, CreatedOn = seedDate },
                new UserRole { RolePermissionID = 14, RoleID = 3, UserID = 14, CreatedOn = seedDate },
                new UserRole { RolePermissionID = 15, RoleID = 3, UserID = 15, CreatedOn = seedDate }
            );

            // STEP 4: ProjectAllocation (Depends on ProjectMaster & User)
            modelBuilder.Entity<ProjectAllocation>().HasData(
                new ProjectAllocation { ProjectAllocationID = 1, AssignedDate = seedDate, ProjectStartDate = seedDate, ProjectEndDate = seedDate.AddDays(90), TotalTasksGiven = 5, TotalCompletedTasks = 2, ProgressPercentage = 40.00m, OverAllGrade = "B", ProjectID = 1, StudentID = 6, FacultyID = 2, CreatedOn = seedDate },
                new ProjectAllocation { ProjectAllocationID = 2, AssignedDate = seedDate, ProjectStartDate = seedDate, ProjectEndDate = seedDate.AddDays(90), TotalTasksGiven = 5, TotalCompletedTasks = 4, ProgressPercentage = 80.00m, OverAllGrade = "A", ProjectID = 2, StudentID = 7, FacultyID = 3, CreatedOn = seedDate },
                new ProjectAllocation { ProjectAllocationID = 3, AssignedDate = seedDate, ProjectStartDate = seedDate, ProjectEndDate = seedDate.AddDays(90), TotalTasksGiven = 5, TotalCompletedTasks = 1, ProgressPercentage = 20.00m, OverAllGrade = "C", ProjectID = 3, StudentID = 8, FacultyID = 4, CreatedOn = seedDate },
                new ProjectAllocation { ProjectAllocationID = 4, AssignedDate = seedDate, ProjectStartDate = seedDate, ProjectEndDate = seedDate.AddDays(90), TotalTasksGiven = 5, TotalCompletedTasks = 5, ProgressPercentage = 100.00m, OverAllGrade = "A", ProjectID = 4, StudentID = 9, FacultyID = 5, CreatedOn = seedDate },
                new ProjectAllocation { ProjectAllocationID = 5, AssignedDate = seedDate, ProjectStartDate = seedDate, ProjectEndDate = seedDate.AddDays(90), TotalTasksGiven = 5, TotalCompletedTasks = 3, ProgressPercentage = 60.00m, OverAllGrade = "B", ProjectID = 5, StudentID = 10, FacultyID = 2, CreatedOn = seedDate },
                new ProjectAllocation { ProjectAllocationID = 6, AssignedDate = seedDate, ProjectStartDate = seedDate, ProjectEndDate = seedDate.AddDays(90), TotalTasksGiven = 5, TotalCompletedTasks = 0, ProgressPercentage = 0.00m, OverAllGrade = "F", ProjectID = 6, StudentID = 11, FacultyID = 3, CreatedOn = seedDate },
                new ProjectAllocation { ProjectAllocationID = 7, AssignedDate = seedDate, ProjectStartDate = seedDate, ProjectEndDate = seedDate.AddDays(90), TotalTasksGiven = 5, TotalCompletedTasks = 2, ProgressPercentage = 40.00m, OverAllGrade = "C", ProjectID = 7, StudentID = 12, FacultyID = 4, CreatedOn = seedDate },
                new ProjectAllocation { ProjectAllocationID = 8, AssignedDate = seedDate, ProjectStartDate = seedDate, ProjectEndDate = seedDate.AddDays(90), TotalTasksGiven = 5, TotalCompletedTasks = 4, ProgressPercentage = 80.00m, OverAllGrade = "A", ProjectID = 8, StudentID = 13, FacultyID = 5, CreatedOn = seedDate },
                new ProjectAllocation { ProjectAllocationID = 9, AssignedDate = seedDate, ProjectStartDate = seedDate, ProjectEndDate = seedDate.AddDays(90), TotalTasksGiven = 5, TotalCompletedTasks = 3, ProgressPercentage = 60.00m, OverAllGrade = "B", ProjectID = 9, StudentID = 14, FacultyID = 2, CreatedOn = seedDate },
                new ProjectAllocation { ProjectAllocationID = 10, AssignedDate = seedDate, ProjectStartDate = seedDate, ProjectEndDate = seedDate.AddDays(90), TotalTasksGiven = 5, TotalCompletedTasks = 1, ProgressPercentage = 20.00m, OverAllGrade = "D", ProjectID = 10, StudentID = 15, FacultyID = 3, CreatedOn = seedDate }
            );

            // STEP 5: Tasks (Depends on ProjectAllocation, TaskStatus, TaskPriority)
            modelBuilder.Entity<Tasks>().HasData(
                new Tasks { TaskID = 1, TaskTitle = "Requirement Gathering", TaskDescription = "Complete requirements documentation", AssignedScore = 10.00m, EarnedScore = 8.00m, ProgressPercentage = 80.00m, TaskAssignedDate = seedDate, TaskStartDate = seedDate, TaskDueDate = seedDate.AddDays(15), TaskCompletedDate = seedDate.AddDays(10), NextFollowUpDate = seedDate.AddDays(12), FacultyRemarks = "Good initial work", StudentRemarks = "Understood specs", ProjectAllocationID = 1, TaskStatusID = 3, TaskPriorityID = 2, CreatedOn = seedDate },
                new Tasks { TaskID = 2, TaskTitle = "Database Design", TaskDescription = "Create ERD and schema script", AssignedScore = 10.00m, EarnedScore = 9.00m, ProgressPercentage = 90.00m, TaskAssignedDate = seedDate, TaskStartDate = seedDate, TaskDueDate = seedDate.AddDays(15), TaskCompletedDate = seedDate.AddDays(14), NextFollowUpDate = seedDate.AddDays(15), FacultyRemarks = "Well structured schema", StudentRemarks = "Schema finalized", ProjectAllocationID = 2, TaskStatusID = 3, TaskPriorityID = 3, CreatedOn = seedDate },
                new Tasks { TaskID = 3, TaskTitle = "UI Wireframing", TaskDescription = "Design Figma mockups for core pages", AssignedScore = 10.00m, EarnedScore = 5.00m, ProgressPercentage = 50.00m, TaskAssignedDate = seedDate, TaskStartDate = seedDate, TaskDueDate = seedDate.AddDays(15), TaskCompletedDate = null, NextFollowUpDate = seedDate.AddDays(10), FacultyRemarks = "Needs cleaner layout", StudentRemarks = "Working on dashboard screen", ProjectAllocationID = 3, TaskStatusID = 2, TaskPriorityID = 1, CreatedOn = seedDate },
                new Tasks { TaskID = 4, TaskTitle = "Backend API Development", TaskDescription = "Build CRUD REST APIs with EF Core", AssignedScore = 10.00m, EarnedScore = 10.00m, ProgressPercentage = 100.00m, TaskAssignedDate = seedDate, TaskStartDate = seedDate, TaskDueDate = seedDate.AddDays(20), TaskCompletedDate = seedDate.AddDays(18), NextFollowUpDate = seedDate.AddDays(20), FacultyRemarks = "Excellent work!", StudentRemarks = "All endpoints tested", ProjectAllocationID = 4, TaskStatusID = 3, TaskPriorityID = 3, CreatedOn = seedDate },
                new Tasks { TaskID = 5, TaskTitle = "Frontend Integration", TaskDescription = "Connect Razor views with backend API", AssignedScore = 10.00m, EarnedScore = 6.00m, ProgressPercentage = 60.00m, TaskAssignedDate = seedDate, TaskStartDate = seedDate, TaskDueDate = seedDate.AddDays(25), TaskCompletedDate = null, NextFollowUpDate = seedDate.AddDays(22), FacultyRemarks = "Focus on error handling", StudentRemarks = "Integrating POST endpoints", ProjectAllocationID = 5, TaskStatusID = 2, TaskPriorityID = 2, CreatedOn = seedDate },
                new Tasks { TaskID = 6, TaskTitle = "Unit Testing", TaskDescription = "Write xUnit tests for controller layer", AssignedScore = 10.00m, EarnedScore = 0.00m, ProgressPercentage = 0.00m, TaskAssignedDate = seedDate, TaskStartDate = null, TaskDueDate = seedDate.AddDays(30), TaskCompletedDate = null, NextFollowUpDate = seedDate.AddDays(25), FacultyRemarks = "Please start testing soon", StudentRemarks = "Not started yet", ProjectAllocationID = 6, TaskStatusID = 1, TaskPriorityID = 1, CreatedOn = seedDate },
                new Tasks { TaskID = 7, TaskTitle = "Integration Testing", TaskDescription = "Test flow between client and server", AssignedScore = 10.00m, EarnedScore = 4.00m, ProgressPercentage = 40.00m, TaskAssignedDate = seedDate, TaskStartDate = seedDate, TaskDueDate = seedDate.AddDays(30), TaskCompletedDate = null, NextFollowUpDate = seedDate.AddDays(28), FacultyRemarks = "Check edge cases", StudentRemarks = "Found 2 minor issues", ProjectAllocationID = 7, TaskStatusID = 2, TaskPriorityID = 2, CreatedOn = seedDate },
                new Tasks { TaskID = 8, TaskTitle = "Deployment Setup", TaskDescription = "Configure IIS and SQL Server hosting", AssignedScore = 10.00m, EarnedScore = 9.00m, ProgressPercentage = 90.00m, TaskAssignedDate = seedDate, TaskStartDate = seedDate, TaskDueDate = seedDate.AddDays(35), TaskCompletedDate = seedDate.AddDays(32), NextFollowUpDate = seedDate.AddDays(35), FacultyRemarks = "Smooth deployment", StudentRemarks = "App live on staging environment", ProjectAllocationID = 8, TaskStatusID = 3, TaskPriorityID = 3, CreatedOn = seedDate },
                new Tasks { TaskID = 9, TaskTitle = "Documentation", TaskDescription = "Write user manual and technical guide", AssignedScore = 10.00m, EarnedScore = 7.00m, ProgressPercentage = 70.00m, TaskAssignedDate = seedDate, TaskStartDate = seedDate, TaskDueDate = seedDate.AddDays(40), TaskCompletedDate = null, NextFollowUpDate = seedDate.AddDays(38), FacultyRemarks = "Add architectural diagrams", StudentRemarks = "Draft ready", ProjectAllocationID = 9, TaskStatusID = 2, TaskPriorityID = 1, CreatedOn = seedDate },
                new Tasks { TaskID = 10, TaskTitle = "Final Presentation Preparation", TaskDescription = "Prepare slides and demo video", AssignedScore = 10.00m, EarnedScore = 2.00m, ProgressPercentage = 20.00m, TaskAssignedDate = seedDate, TaskStartDate = seedDate, TaskDueDate = seedDate.AddDays(45), TaskCompletedDate = null, NextFollowUpDate = seedDate.AddDays(40), FacultyRemarks = "Practice timing", StudentRemarks = "Outline created", ProjectAllocationID = 10, TaskStatusID = 2, TaskPriorityID = 2, CreatedOn = seedDate }
            );
        }
    }
}
