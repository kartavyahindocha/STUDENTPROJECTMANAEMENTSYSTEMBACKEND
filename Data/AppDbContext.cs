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
            // (Table names come from [Table("SPM_...")] on the model classes already.)
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
            // Email uniqueness is explicitly documented in the PDF ("Not Null,
            // Unique"). RoleName/UserTypeName/TaskStatusName/TaskPriorityName
            // aren't marked Unique in the schema, but as lookup/master tables
            // duplicate entries would be nonsensical — added defensively; drop
            // these four if your actual DB doesn't enforce them.
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Role>().HasIndex(r => r.RoleName).IsUnique();
            modelBuilder.Entity<UserType>().HasIndex(ut => ut.UserTypeName).IsUnique();
            modelBuilder.Entity<TaskStatusMaster>().HasIndex(s => s.TaskStatusName).IsUnique();
            modelBuilder.Entity<TaskPriorityMaster>().HasIndex(p => p.TaskPriorityName).IsUnique();

            // ================= DEFAULTS =================
            // SPM_ProjectAllocation.AssignedDate -> "Not Null, Default GETDATE()"
            modelBuilder.Entity<ProjectAllocation>()
                .Property(pa => pa.AssignedDate)
                .HasDefaultValueSql("GETDATE()");

            // ================= RELATIONSHIPS =================
            // All Restrict (NO ACTION). SPM_ProjectAllocation alone has three
            // FKs (StudentID, FacultyID -> SPM_User, ProjectID -> SPM_ProjectMaster);
            // if more than one FK to the same table were CASCADE, SQL Server
            // throws "Introducing FOREIGN KEY constraint '...' may cause
            // cycles or multiple cascade paths." Restrict is also correct
            // behavior here: deleting a User/Status/Priority/Project should
            // never silently wipe allocations/tasks — use IsDeleted instead.
            //
            // Every .WithMany(...) points at the matching named ICollection<T>
            // on the "one" side, so reverse-navigation collections populate
            // correctly on .Include(...) queries.

            // ---- UserType (1) -> User (N) ----
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserType)
                .WithMany(ut => ut.Users)
                .HasForeignKey(u => u.UserTypeID)
                .OnDelete(DeleteBehavior.Restrict);

            // ---- Role (1) -> UserRole (N) ; User (1) -> UserRole (N) ----
            // Together these two 1:N relationships give the User <-> Role
            // many-to-many, expressed through the explicit SPM_UserRole join
            // entity (it has its own surrogate key, so it's kept explicit
            // rather than using EF Core's implicit skip-navigation N:N).
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

            // ---- ProjectMaster (1) -> ProjectAllocation (N) ----
            modelBuilder.Entity<ProjectAllocation>()
                .HasOne(pa => pa.ProjectMaster)
                .WithMany(pm => pm.ProjectAllocations)
                .HasForeignKey(pa => pa.ProjectID)
                .OnDelete(DeleteBehavior.Restrict);

            // ---- User (1) -> ProjectAllocation (N), as Student ----
            modelBuilder.Entity<ProjectAllocation>()
                .HasOne(pa => pa.Student)
                .WithMany(u => u.ProjectAllocationsAsStudent)
                .HasForeignKey(pa => pa.StudentID)
                .OnDelete(DeleteBehavior.Restrict);

            // ---- User (1) -> ProjectAllocation (N), as Faculty ----
            modelBuilder.Entity<ProjectAllocation>()
                .HasOne(pa => pa.Faculty)
                .WithMany(u => u.ProjectAllocationsAsFaculty)
                .HasForeignKey(pa => pa.FacultyID)
                .OnDelete(DeleteBehavior.Restrict);

            // ---- ProjectAllocation (1) -> Task (N) ----
            modelBuilder.Entity<Tasks>()
                .HasOne(t => t.ProjectAllocation)
                .WithMany(pa => pa.Tasks)
                .HasForeignKey(t => t.ProjectAllocationID)
                .OnDelete(DeleteBehavior.Restrict);

            // ---- TaskStatusMaster (1) -> Task (N) ----
            modelBuilder.Entity<Tasks>()
                .HasOne(t => t.TaskStatus)
                .WithMany(s => s.Tasks)
                .HasForeignKey(t => t.TaskStatusID)
                .OnDelete(DeleteBehavior.Restrict);

            // ---- TaskPriorityMaster (1) -> Task (N) ----
            modelBuilder.Entity<Tasks>()
                .HasOne(t => t.TaskPriority)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.TaskPriorityID)
                .OnDelete(DeleteBehavior.Restrict);

            // No 1:1 relationship exists anywhere in this schema — every table
            // relates through N:1/1:N (or N:N via UserRole).

            // ================= SEED DATA =================

            var systemUserId = 1;
            var seedDate = DateTime.Parse("2026-01-01");

            modelBuilder.Entity<UserType>().HasData(
                new UserType { UserTypeID = 1, UserTypeName = "Admin", Description = "Full system access", CreatedBy = systemUserId, CreatedOn = seedDate },
                new UserType { UserTypeID = 2, UserTypeName = "Faculty", Description = "Supervises student projects", CreatedBy = systemUserId, CreatedOn = seedDate },
                new UserType { UserTypeID = 3, UserTypeName = "Student", Description = "Works on assigned projects", CreatedBy = systemUserId, CreatedOn = seedDate }
            );

            modelBuilder.Entity<TaskStatusMaster>().HasData(
                new TaskStatusMaster { TaskStatusID = 1, TaskStatusName = "Pending", TaskStatusCssClass = "tone-warning", CreatedBy = systemUserId, CreatedOn = seedDate },
                new TaskStatusMaster { TaskStatusID = 2, TaskStatusName = "Ongoing", TaskStatusCssClass = "tone-info", CreatedBy = systemUserId, CreatedOn = seedDate },
                new TaskStatusMaster { TaskStatusID = 3, TaskStatusName = "Completed", TaskStatusCssClass = "tone-success", CreatedBy = systemUserId, CreatedOn = seedDate },
                new TaskStatusMaster { TaskStatusID = 4, TaskStatusName = "Cancelled", TaskStatusCssClass = "tone-danger", CreatedBy = systemUserId, CreatedOn = seedDate }
            );

            modelBuilder.Entity<TaskPriorityMaster>().HasData(
                new TaskPriorityMaster { TaskPriorityID = 1, TaskPriorityName = "Low", TaskPriortyCssClass = "tone-neutral", CreatedBy = systemUserId, CreatedOn = seedDate },
                new TaskPriorityMaster { TaskPriorityID = 2, TaskPriorityName = "Moderate", TaskPriortyCssClass = "tone-warning", CreatedBy = systemUserId, CreatedOn = seedDate },
                new TaskPriorityMaster { TaskPriorityID = 3, TaskPriorityName = "Critical", TaskPriortyCssClass = "tone-danger", CreatedBy = systemUserId, CreatedOn = seedDate }
            );
        }
    }
}
