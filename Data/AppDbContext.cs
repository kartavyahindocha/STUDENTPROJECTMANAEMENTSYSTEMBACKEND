using Microsoft.EntityFrameworkCore;
using STUDENTPROJECTMANAGEMENTSYSTEM.Models;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Priority> Prioritys => Set<Priority>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Status> Status => Set<Status>();
        public DbSet<Tasks> Tasks => Set<Tasks>();
        public DbSet<User> Users => Set<User>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ================= KEYS =================
            // (Table names come from [Table("SPM_...")] on the model classes already.)
            modelBuilder.Entity<Role>().HasKey(r => r.RoleId);
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<UserRole>().HasKey(ur => ur.RolePermissionId);
            modelBuilder.Entity<Status>().HasKey(s => s.StatusID);
            modelBuilder.Entity<Priority>().HasKey(p => p.PriorityID);
            modelBuilder.Entity<Project>().HasKey(p => p.ProjectId);
            modelBuilder.Entity<Tasks>().HasKey(t => t.TaskId);

            // ================= UNIQUE CONSTRAINTS =================
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Status>().HasIndex(s => s.StatusName).IsUnique();
            modelBuilder.Entity<Priority>().HasIndex(p => p.PriorityName).IsUnique();

            // ================= RELATIONSHIPS =================
            // All Restrict (NO ACTION). SPM_Project has three FKs (StudentId,
            // FacultyId -> SPM_User, ProjectStatus -> SPM_Status); if more than
            // one were CASCADE, SQL Server throws:
            //   "Introducing FOREIGN KEY constraint '...' may cause cycles or
            //    multiple cascade paths."
            // Restrict is also correct behavior: deleting a User/Status/Priority
            // should never silently wipe Projects/Tasks — use the IsDeleted
            // soft-delete columns instead.

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany()
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany()
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Student)
                .WithMany()
                .HasForeignKey(p => p.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Faculty)
                .WithMany()
                .HasForeignKey(p => p.FacultyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Status)
                .WithMany()
                .HasForeignKey(p => p.ProjectStatus)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tasks>()
                .HasOne(t => t.Status)
                .WithMany()
                .HasForeignKey(t => t.TaskStatus)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tasks>()
                .HasOne(t => t.Priority)
                .WithMany()
                .HasForeignKey(t => t.PriorityID)
                .OnDelete(DeleteBehavior.Restrict);

            // ================= SEED DATA =================

            var systemUserId = 1;
            var seedDate = DateTime.Parse("2026-01-01");

            modelBuilder.Entity<Status>().HasData(
                new Status
                {
                    StatusID = 1,
                    StatusName = "Not Started",
                    StatusCssClass = "tone-neutral",
                    CreatedBy = systemUserId,
                    CreatedOn = seedDate
                },
                new Status
                {
                    StatusID = 2,
                    StatusName = "Pending",
                    StatusCssClass = "tone-warning",
                    CreatedBy = systemUserId,
                    CreatedOn = seedDate
                },
                new Status
                {
                    StatusID = 3,
                    StatusName = "In Progress",
                    StatusCssClass = "tone-info",
                    CreatedBy = systemUserId,
                    CreatedOn = seedDate
                },
                new Status
                {
                    StatusID = 4,
                    StatusName = "Completed",
                    StatusCssClass = "tone-success",
                    CreatedBy = systemUserId,
                    CreatedOn = seedDate
                },
                new Status
                {
                    StatusID = 5,
                    StatusName = "Rejected",
                    StatusCssClass = "tone-danger",
                    CreatedBy = systemUserId,
                    CreatedOn = seedDate
                }
            );

            modelBuilder.Entity<Priority>().HasData(
                new Priority
                {
                    PriorityID = 1,
                    PriorityName = "Low",
                    PriorityCssClass = "tone-neutral",
                    CreatedBy = systemUserId,
                    CreatedOn = seedDate
                },
                new Priority
                {
                    PriorityID = 2,
                    PriorityName = "Medium",
                    PriorityCssClass = "tone-warning",
                    CreatedBy = systemUserId,
                    CreatedOn = seedDate
                },
                new Priority
                {
                    PriorityID = 3,
                    PriorityName = "High",
                    PriorityCssClass = "tone-danger",
                    CreatedBy = systemUserId,
                    CreatedOn = seedDate
                },
                new Priority
                {
                    PriorityID = 4,
                    PriorityName = "Critical",
                    PriorityCssClass = "tone-danger",
                    CreatedBy = systemUserId,
                    CreatedOn = seedDate
                }
            );
        }

    }
}