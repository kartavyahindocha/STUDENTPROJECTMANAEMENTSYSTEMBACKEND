using Microsoft.EntityFrameworkCore;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // Run only if DB is reachable
            await context.Database.MigrateAsync();

            int systemUser = 1;
            var now = DateTime.Now;

            // ─────────────────────────────────────────────
            // 1. ROLES
            // ─────────────────────────────────────────────
            if (!await context.Roles.AnyAsync())
            {
                await context.Roles.AddRangeAsync(
                    new Role { RoleName = "Admin",    Description = "Full system management access",            CreatedBy = systemUser, CreatedOn = now },
                    new Role { RoleName = "Faculty",  Description = "Supervises and evaluates student projects", CreatedBy = systemUser, CreatedOn = now },
                    new Role { RoleName = "Student",  Description = "Works on assigned projects and tasks",      CreatedBy = systemUser, CreatedOn = now },
                    new Role { RoleName = "Guest",    Description = "Read-only limited access",                  CreatedBy = systemUser, CreatedOn = now }
                );
                await context.SaveChangesAsync();
            }

            // ─────────────────────────────────────────────
            // 2. USER TYPES  (skip if already seeded by migration)
            // ─────────────────────────────────────────────
            if (!await context.UserTypes.AnyAsync())
            {
                await context.UserTypes.AddRangeAsync(
                    new UserType { UserTypeName = "Admin",   Description = "Full system access",           CreatedBy = systemUser, CreatedOn = now },
                    new UserType { UserTypeName = "Faculty", Description = "Supervises student projects",   CreatedBy = systemUser, CreatedOn = now },
                    new UserType { UserTypeName = "Student", Description = "Works on assigned projects",    CreatedBy = systemUser, CreatedOn = now }
                );
                await context.SaveChangesAsync();
            }

            // ─────────────────────────────────────────────
            // 3. TASK STATUS  (skip if already seeded by migration)
            // ─────────────────────────────────────────────
            if (!await context.TaskStatus.AnyAsync())
            {
                await context.TaskStatus.AddRangeAsync(
                    new TaskStatusMaster { TaskStatusName = "Pending",   TaskStatusCssClass = "tone-warning", CreatedBy = systemUser, CreatedOn = now },
                    new TaskStatusMaster { TaskStatusName = "Ongoing",   TaskStatusCssClass = "tone-info",    CreatedBy = systemUser, CreatedOn = now },
                    new TaskStatusMaster { TaskStatusName = "Completed", TaskStatusCssClass = "tone-success", CreatedBy = systemUser, CreatedOn = now },
                    new TaskStatusMaster { TaskStatusName = "Cancelled", TaskStatusCssClass = "tone-danger",  CreatedBy = systemUser, CreatedOn = now }
                );
                await context.SaveChangesAsync();
            }

            // ─────────────────────────────────────────────
            // 4. TASK PRIORITY  (skip if already seeded by migration)
            // ─────────────────────────────────────────────
            if (!await context.TaskPriorities.AnyAsync())
            {
                await context.TaskPriorities.AddRangeAsync(
                    new TaskPriorityMaster { TaskPriorityName = "Low",      TaskPriortyCssClass = "tone-neutral", CreatedBy = systemUser, CreatedOn = now },
                    new TaskPriorityMaster { TaskPriorityName = "Moderate", TaskPriortyCssClass = "tone-warning", CreatedBy = systemUser, CreatedOn = now },
                    new TaskPriorityMaster { TaskPriorityName = "Critical", TaskPriortyCssClass = "tone-danger",  CreatedBy = systemUser, CreatedOn = now }
                );
                await context.SaveChangesAsync();
            }

            // ─────────────────────────────────────────────
            // 5. USERS  (1 Admin + 4 Faculty + 10 Students)
            // ─────────────────────────────────────────────
            if (!await context.Users.AnyAsync())
            {
                var adminTypeId   = (await context.UserTypes.FirstAsync(ut => ut.UserTypeName == "Admin")).UserTypeID;
                var facultyTypeId = (await context.UserTypes.FirstAsync(ut => ut.UserTypeName == "Faculty")).UserTypeID;
                var studentTypeId = (await context.UserTypes.FirstAsync(ut => ut.UserTypeName == "Student")).UserTypeID;

                await context.Users.AddRangeAsync(

                    // ── Admin ──
                    new User
                    {
                        FullName = "Admin User", UserCode = "ADM001",
                        Email = "admin@spms.com", Password = "admin123",
                        MobileNumber = "9000000000", IsActive = true,
                        UserTypeID = adminTypeId, CreatedBy = systemUser, CreatedOn = now
                    },

                    // ── Faculty (4) ──
                    new User
                    {
                        FullName = "Ravi Mehta", UserCode = "F001",
                        Email = "ravi.mehta@spms.com", Password = "faculty123",
                        MobileNumber = "9100000001", IsActive = true,
                        UserTypeID = facultyTypeId, CreatedBy = systemUser, CreatedOn = now
                    },
                    new User
                    {
                        FullName = "Priya Shah", UserCode = "F002",
                        Email = "priya.shah@spms.com", Password = "faculty123",
                        MobileNumber = "9100000002", IsActive = true,
                        UserTypeID = facultyTypeId, CreatedBy = systemUser, CreatedOn = now
                    },
                    new User
                    {
                        FullName = "Dinesh Patel", UserCode = "F003",
                        Email = "dinesh.patel@spms.com", Password = "faculty123",
                        MobileNumber = "9100000003", IsActive = true,
                        UserTypeID = facultyTypeId, CreatedBy = systemUser, CreatedOn = now
                    },
                    new User
                    {
                        FullName = "Sonal Joshi", UserCode = "F004",
                        Email = "sonal.joshi@spms.com", Password = "faculty123",
                        MobileNumber = "9100000004", IsActive = true,
                        UserTypeID = facultyTypeId, CreatedBy = systemUser, CreatedOn = now
                    },

                    // ── Students (10) ──
                    new User
                    {
                        FullName = "Het Patel", UserCode = "25010101622",
                        Email = "het.patel@spms.com", Password = "student123",
                        MobileNumber = "9200000001", IsActive = true,
                        UserTypeID = studentTypeId, CreatedBy = systemUser, CreatedOn = now
                    },
                    new User
                    {
                        FullName = "Vasu Desai", UserCode = "25010101623",
                        Email = "vasu.desai@spms.com", Password = "student123",
                        MobileNumber = "9200000002", IsActive = true,
                        UserTypeID = studentTypeId, CreatedBy = systemUser, CreatedOn = now
                    },
                    new User
                    {
                        FullName = "Kartavya Hindocha", UserCode = "25010101624",
                        Email = "kartavya.hindocha@spms.com", Password = "student123",
                        MobileNumber = "9200000003", IsActive = true,
                        UserTypeID = studentTypeId, CreatedBy = systemUser, CreatedOn = now
                    },
                    new User
                    {
                        FullName = "Dev Shah", UserCode = "25010101625",
                        Email = "dev.shah@spms.com", Password = "student123",
                        MobileNumber = "9200000004", IsActive = true,
                        UserTypeID = studentTypeId, CreatedBy = systemUser, CreatedOn = now
                    },
                    new User
                    {
                        FullName = "Neel Trivedi", UserCode = "25010101626",
                        Email = "neel.trivedi@spms.com", Password = "student123",
                        MobileNumber = "9200000005", IsActive = true,
                        UserTypeID = studentTypeId, CreatedBy = systemUser, CreatedOn = now
                    },
                    new User
                    {
                        FullName = "Jay Chauhan", UserCode = "25010101627",
                        Email = "jay.chauhan@spms.com", Password = "student123",
                        MobileNumber = "9200000006", IsActive = true,
                        UserTypeID = studentTypeId, CreatedBy = systemUser, CreatedOn = now
                    },
                    new User
                    {
                        FullName = "Raj Solanki", UserCode = "25010101628",
                        Email = "raj.solanki@spms.com", Password = "student123",
                        MobileNumber = "9200000007", IsActive = true,
                        UserTypeID = studentTypeId, CreatedBy = systemUser, CreatedOn = now
                    },
                    new User
                    {
                        FullName = "Om Parmar", UserCode = "25010101629",
                        Email = "om.parmar@spms.com", Password = "student123",
                        MobileNumber = "9200000008", IsActive = true,
                        UserTypeID = studentTypeId, CreatedBy = systemUser, CreatedOn = now
                    },
                    new User
                    {
                        FullName = "Krish Rathod", UserCode = "25010101630",
                        Email = "krish.rathod@spms.com", Password = "student123",
                        MobileNumber = "9200000009", IsActive = true,
                        UserTypeID = studentTypeId, CreatedBy = systemUser, CreatedOn = now
                    },
                    new User
                    {
                        FullName = "Manan Jain", UserCode = "25010101631",
                        Email = "manan.jain@spms.com", Password = "student123",
                        MobileNumber = "9200000010", IsActive = true,
                        UserTypeID = studentTypeId, CreatedBy = systemUser, CreatedOn = now
                    }
                );
                await context.SaveChangesAsync();
            }

            // ─────────────────────────────────────────────
            // 6. USER ROLES
            // ─────────────────────────────────────────────
            if (!await context.UserRoles.AnyAsync())
            {
                var adminRole   = await context.Roles.FirstAsync(r => r.RoleName == "Admin");
                var facultyRole = await context.Roles.FirstAsync(r => r.RoleName == "Faculty");
                var studentRole = await context.Roles.FirstAsync(r => r.RoleName == "Student");

                var adminUser    = await context.Users.FirstAsync(u => u.UserCode == "ADM001");
                var faculty1     = await context.Users.FirstAsync(u => u.UserCode == "F001");
                var faculty2     = await context.Users.FirstAsync(u => u.UserCode == "F002");
                var faculty3     = await context.Users.FirstAsync(u => u.UserCode == "F003");
                var faculty4     = await context.Users.FirstAsync(u => u.UserCode == "F004");
                var students     = await context.Users.Where(u => u.UserCode!.StartsWith("250")).ToListAsync();

                var userRoles = new List<UserRole>
                {
                    new UserRole { RoleID = adminRole.RoleID,   UserID = adminUser.UserID,   CreatedBy = systemUser, CreatedOn = now },
                    new UserRole { RoleID = facultyRole.RoleID, UserID = faculty1.UserID,    CreatedBy = systemUser, CreatedOn = now },
                    new UserRole { RoleID = facultyRole.RoleID, UserID = faculty2.UserID,    CreatedBy = systemUser, CreatedOn = now },
                    new UserRole { RoleID = facultyRole.RoleID, UserID = faculty3.UserID,    CreatedBy = systemUser, CreatedOn = now },
                    new UserRole { RoleID = facultyRole.RoleID, UserID = faculty4.UserID,    CreatedBy = systemUser, CreatedOn = now },
                };

                foreach (var s in students)
                    userRoles.Add(new UserRole { RoleID = studentRole.RoleID, UserID = s.UserID, CreatedBy = systemUser, CreatedOn = now });

                await context.UserRoles.AddRangeAsync(userRoles);
                await context.SaveChangesAsync();
            }

            // ─────────────────────────────────────────────
            // 7. PROJECTS (10)
            // ─────────────────────────────────────────────
            if (!await context.ProjectMasters.AnyAsync())
            {
                await context.ProjectMasters.AddRangeAsync(
                    new ProjectMaster { ProjectTitle = "Online Examination System",           Description = "A web-based exam management system",               CreatedBy = systemUser, CreatedOn = now },
                    new ProjectMaster { ProjectTitle = "Hospital Management System",          Description = "System to manage hospital records and appointments", CreatedBy = systemUser, CreatedOn = now },
                    new ProjectMaster { ProjectTitle = "Library Management System",           Description = "Digital library book tracking and management",       CreatedBy = systemUser, CreatedOn = now },
                    new ProjectMaster { ProjectTitle = "E-Commerce Platform",                 Description = "Online shopping portal with cart and payment",       CreatedBy = systemUser, CreatedOn = now },
                    new ProjectMaster { ProjectTitle = "Student Attendance Tracker",          Description = "Mobile app for tracking student attendance",         CreatedBy = systemUser, CreatedOn = now },
                    new ProjectMaster { ProjectTitle = "Restaurant Billing System",           Description = "POS system for restaurant orders and billing",       CreatedBy = systemUser, CreatedOn = now },
                    new ProjectMaster { ProjectTitle = "Blood Bank Management System",        Description = "Manage blood donations, donors and requests",        CreatedBy = systemUser, CreatedOn = now },
                    new ProjectMaster { ProjectTitle = "Job Portal",                          Description = "Platform connecting recruiters and job seekers",     CreatedBy = systemUser, CreatedOn = now },
                    new ProjectMaster { ProjectTitle = "Inventory Management System",         Description = "Stock tracking and inventory control for business",   CreatedBy = systemUser, CreatedOn = now },
                    new ProjectMaster { ProjectTitle = "Social Media Dashboard",              Description = "Analytics dashboard for social media accounts",      CreatedBy = systemUser, CreatedOn = now }
                );
                await context.SaveChangesAsync();
            }

            // ─────────────────────────────────────────────
            // 8. PROJECT ALLOCATIONS (10 — 1 per student)
            // ─────────────────────────────────────────────
            if (!await context.ProjectAllocations.AnyAsync())
            {
                var students  = await context.Users.Where(u => u.UserCode!.StartsWith("250")).OrderBy(u => u.UserCode).ToListAsync();
                var faculties = await context.Users.Where(u => u.UserCode!.StartsWith("F")).OrderBy(u => u.UserCode).ToListAsync();
                var projects  = await context.ProjectMasters.OrderBy(p => p.ProjectID).ToListAsync();

                var allocations = new List<ProjectAllocation>();
                for (int i = 0; i < 10; i++)
                {
                    allocations.Add(new ProjectAllocation
                    {
                        AssignedDate        = now.AddDays(-30),
                        ProjectStartDate    = now.AddDays(-25),
                        ProjectEndDate      = now.AddDays(60),
                        TotalTasksGiven     = 5,
                        TotalCompletedTasks = i % 5,
                        ProgressPercentage  = (i % 5) * 20,
                        OverAllGrade        = null,
                        ProjectID           = projects[i].ProjectID,
                        StudentID           = students[i].UserID,
                        FacultyID           = faculties[i % faculties.Count].UserID,
                        CreatedBy           = systemUser,
                        CreatedOn           = now
                    });
                }

                await context.ProjectAllocations.AddRangeAsync(allocations);
                await context.SaveChangesAsync();
            }

            // ─────────────────────────────────────────────
            // 9. TASKS (10 — 1 per project allocation)
            // ─────────────────────────────────────────────
            if (!await context.Tasks.AnyAsync())
            {
                var allocations = await context.ProjectAllocations.OrderBy(pa => pa.ProjectAllocationID).ToListAsync();
                var statuses    = await context.TaskStatus.OrderBy(s => s.TaskStatusID).ToListAsync();
                var priorities  = await context.TaskPriorities.OrderBy(p => p.TaskPriorityID).ToListAsync();

                string[] taskTitles =
                {
                    "Requirement Gathering",
                    "Database Design",
                    "UI Wireframing",
                    "Backend API Development",
                    "Frontend Development",
                    "Unit Testing",
                    "Integration Testing",
                    "Deployment Setup",
                    "Documentation",
                    "Final Presentation Preparation"
                };

                var tasks = new List<Tasks>();
                for (int i = 0; i < 10; i++)
                {
                    tasks.Add(new Tasks
                    {
                        TaskTitle           = taskTitles[i],
                        TaskDescription     = $"Complete {taskTitles[i]} phase for the allocated project",
                        AssignedScore       = 10,
                        EarnedScore         = i < 5 ? (i + 1) * 2 : null,
                        ProgressPercentage  = i < 5 ? (i + 1) * 20 : 0,
                        TaskAssignedDate    = now.AddDays(-25),
                        TaskStartDate       = now.AddDays(-20 + i),
                        TaskDueDate         = now.AddDays(10 + i),
                        TaskCompletedDate   = i < 3 ? now.AddDays(-5 + i) : null,
                        NextFollowUpDate    = i >= 3 ? now.AddDays(5 + i) : null,
                        FacultyRemarks      = i < 3 ? "Good progress, keep it up." : "Pending review.",
                        StudentRemarks      = i < 3 ? "Completed as per requirement." : "In progress.",
                        ProjectAllocationID = allocations[i].ProjectAllocationID,
                        TaskStatusID        = statuses[i % statuses.Count].TaskStatusID,
                        TaskPriorityID      = priorities[i % priorities.Count].TaskPriorityID,
                        CreatedBy           = systemUser,
                        CreatedOn           = now
                    });
                }

                await context.Tasks.AddRangeAsync(tasks);
                await context.SaveChangesAsync();
            }

            Console.WriteLine("✅ Database seeding completed successfully.");
        }
    }
}
