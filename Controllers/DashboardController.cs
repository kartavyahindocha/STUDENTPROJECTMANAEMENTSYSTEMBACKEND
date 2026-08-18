using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Data;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext context;

        public DashboardController(AppDbContext context)
        {
            this.context = context;
        }

        #region Standard Counts
        [HttpGet("counts")]
        [HttpGet("/dashboard/counts")]
        [HttpGet("/dashboard/metrics")]
        public async Task<IActionResult> GetDashboardCounts()
        {
            var totalUsers = await context.Users.AsNoTracking().CountAsync();
            var totalRoles = await context.Roles.AsNoTracking().CountAsync();
            var totalProjects = await context.ProjectMasters.AsNoTracking().CountAsync();
            var totalTasks = await context.Tasks.AsNoTracking().CountAsync();

            var totalStudents = await context.Users
                .Include(u => u.UserType)
                .AsNoTracking()
                .Where(u => u.UserType != null && u.UserType.UserTypeName.ToLower().Contains("student"))
                .CountAsync();

            var totalFaculty = await context.Users
                .Include(u => u.UserType)
                .AsNoTracking()
                .Where(u => u.UserType != null && (u.UserType.UserTypeName.ToLower().Contains("faculty") || u.UserType.UserTypeName.ToLower().Contains("teacher") || u.UserType.UserTypeName.ToLower().Contains("prof")))
                .CountAsync();

            var completedTasks = await context.Tasks
                .AsNoTracking()
                .Where(t => t.ProgressPercentage >= 100)
                .CountAsync();

            var pendingTasks = totalTasks - completedTasks;

            var recentProjects = await context.ProjectMasters
                .Include(p => p.ProjectAllocations)
                    .ThenInclude(pa => pa.Student)
                .Include(p => p.ProjectAllocations)
                    .ThenInclude(pa => pa.Faculty)
                .OrderByDescending(p => p.ProjectID)
                .Take(5)
                .AsNoTracking()
                .ToListAsync();

            var recentTasks = await context.Tasks
                .Include(t => t.TaskStatus)
                .Include(t => t.TaskPriority)
                .OrderByDescending(t => t.TaskID)
                .Take(5)
                .AsNoTracking()
                .ToListAsync();

            var countsDTO = new DashboardCountsDTO
            {
                TotalUsers = totalUsers,
                TotalStudents = totalStudents > 0 ? totalStudents : (totalUsers > 0 ? totalUsers : 0),
                TotalFaculty = totalFaculty,
                TotalProjects = totalProjects,
                TotalTasks = totalTasks,
                CompletedTasks = completedTasks,
                PendingTasks = pendingTasks,
                TotalRoles = totalRoles,
                RecentProjects = recentProjects,
                RecentTasks = recentTasks
            };

            var response = ApiResponse.Success(countsDTO, "Dashboard metrics fetched successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region 1) Total Students Registered
        // 1) Total number of students registered
        [HttpGet("total-students")]
        public async Task<IActionResult> GetTotalStudents()
        {
            var count = await context.Users
                .AsNoTracking()
                .Where(u => u.UserType != null && u.UserType.UserTypeName.ToLower().Contains("student"))
                .CountAsync();

            var response = ApiResponse.Success(new { TotalStudents = count }, "Total students retrieved successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region 2) Total Faculty Guiding Projects
        // 2) Total number of faculty members guiding projects
        [HttpGet("total-guiding-faculties")]
        public async Task<IActionResult> GetTotalGuidingFaculties()
        {
            var count = await context.ProjectAllocations
                .AsNoTracking()
                .Select(pa => pa.FacultyID)
                .Distinct()
                .CountAsync();

            var response = ApiResponse.Success(new { TotalGuidingFaculties = count }, "Total guiding faculties retrieved successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region 3) Total Projects Available
        // 3) Total number of projects available
        [HttpGet("total-projects")]
        public async Task<IActionResult> GetTotalProjects()
        {
            var count = await context.ProjectMasters.AsNoTracking().CountAsync();
            var response = ApiResponse.Success(new { TotalProjects = count }, "Total projects retrieved successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region 4) Tasks Count By Status Category
        // 4) Tasks count per status category
        [HttpGet("tasks-by-status")]
        public async Task<IActionResult> GetTasksByStatus()
        {
            var result = await context.Tasks
                .AsNoTracking()
                .GroupBy(t => t.TaskStatus != null ? t.TaskStatus.TaskStatusName : "Unknown")
                .Select(g => new
                {
                    Status = g.Key,
                    TasksCount = g.Count()
                })
                .ToListAsync();

            var response = ApiResponse.Success(result, "Tasks count by status retrieved successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region 5) Priority Wise Task Count
        // 5) Priority wise task count
        [HttpGet("tasks-by-priority")]
        public async Task<IActionResult> GetTasksByPriority()
        {
            var result = await context.Tasks
                .AsNoTracking()
                .GroupBy(t => t.TaskPriority != null ? t.TaskPriority.TaskPriorityName : "Unknown")
                .Select(g => new
                {
                    Priority = g.Key,
                    TasksCount = g.Count()
                })
                .ToListAsync();

            var response = ApiResponse.Success(result, "Tasks count by priority retrieved successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region 6) Projects Assigned Per Faculty
        // 6) Projects assigned to each faculty member
        [HttpGet("projects-per-faculty")]
        public async Task<IActionResult> GetProjectsPerFaculty()
        {
            var result = await context.ProjectAllocations
                .AsNoTracking()
                .Where(pa => pa.Faculty != null)
                .GroupBy(pa => new { pa.FacultyID, pa.Faculty!.FullName })
                .Select(g => new
                {
                    FacultyID = g.Key.FacultyID,
                    FacultyName = g.Key.FullName,
                    ProjectsCount = g.Select(pa => pa.ProjectID).Distinct().Count()
                })
                .ToListAsync();

            var response = ApiResponse.Success(result, "Projects count per faculty retrieved successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region 7) Tasks Assigned Per Student
        // 7) Tasks assigned to each student
        [HttpGet("tasks-per-student")]
        public async Task<IActionResult> GetTasksPerStudent()
        {
            var result = await context.Tasks
                .AsNoTracking()
                .Where(t => t.ProjectAllocation != null && t.ProjectAllocation.Student != null)
                .GroupBy(t => new { t.ProjectAllocation!.StudentID, t.ProjectAllocation.Student!.FullName })
                .Select(g => new
                {
                    StudentID = g.Key.StudentID,
                    StudentName = g.Key.FullName,
                    TasksCount = g.Count()
                })
                .ToListAsync();

            var response = ApiResponse.Success(result, "Tasks count per student retrieved successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region 8) Top 10 Students By Average Earned Score
        // 8) Top 10 students having the highest average earned score
        [HttpGet("top-10-students")]
        public async Task<IActionResult> GetTop10Students()
        {
            var result = await context.Tasks
                .AsNoTracking()
                .Where(t => t.ProjectAllocation != null && t.ProjectAllocation.Student != null && t.EarnedScore.HasValue)
                .GroupBy(t => new { t.ProjectAllocation!.StudentID, t.ProjectAllocation.Student!.FullName })
                .Select(g => new
                {
                    StudentID = g.Key.StudentID,
                    StudentName = g.Key.FullName,
                    AverageScore = Math.Round((double)g.Average(t => t.EarnedScore!.Value), 2)
                })
                .OrderByDescending(s => s.AverageScore)
                .Take(10)
                .ToListAsync();

            var response = ApiResponse.Success(result, "Top 10 students retrieved successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region 9) Bottom 10 Students By Average Earned Score
        // 9) Bottom 10 students based on average earned score
        [HttpGet("bottom-10-students")]
        public async Task<IActionResult> GetBottom10Students()
        {
            var result = await context.Tasks
                .AsNoTracking()
                .Where(t => t.ProjectAllocation != null && t.ProjectAllocation.Student != null && t.EarnedScore.HasValue)
                .GroupBy(t => new { t.ProjectAllocation!.StudentID, t.ProjectAllocation.Student!.FullName })
                .Select(g => new
                {
                    StudentID = g.Key.StudentID,
                    StudentName = g.Key.FullName,
                    TotalTasks = g.Count(),
                    AverageScore = Math.Round((double)g.Average(t => t.EarnedScore!.Value), 2)
                })
                .OrderBy(s => s.AverageScore)
                .Take(10)
                .ToListAsync();

            var rankedResult = result.Select((s, index) => new
            {
                Rank = index + 1,
                s.StudentID,
                s.StudentName,
                s.TotalTasks,
                s.AverageScore
            });

            var response = ApiResponse.Success(rankedResult, "Bottom 10 students retrieved successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region 10) Overdue Tasks
        // 10) Overdue tasks (due date passed but not completed)
        [HttpGet("overdue-tasks")]
        public async Task<IActionResult> GetOverdueTasks()
        {
            var currentDate = DateTime.Now;
            var result = await context.Tasks
                .AsNoTracking()
                .Where(t => t.TaskDueDate.HasValue && t.TaskDueDate.Value < currentDate && (t.TaskStatus == null || t.TaskStatus.TaskStatusName.ToLower() != "completed") && t.ProgressPercentage < 100)
                .Select(t => new
                {
                    TaskID = t.TaskID,
                    TaskTitle = t.TaskTitle,
                    StudentName = t.ProjectAllocation != null && t.ProjectAllocation.Student != null ? t.ProjectAllocation.Student.FullName : "N/A",
                    FacultyName = t.ProjectAllocation != null && t.ProjectAllocation.Faculty != null ? t.ProjectAllocation.Faculty.FullName : "N/A",
                    DueDate = t.TaskDueDate,
                    DaysOverdue = EF.Functions.DateDiffDay(t.TaskDueDate.Value, currentDate)
                })
                .ToListAsync();

            var response = ApiResponse.Success(result, "Overdue tasks retrieved successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region 11) Upcoming Follow-Up Tasks
        // 11) Tasks having follow-up dates within the next 7 days
        [HttpGet("upcoming-followups")]
        public async Task<IActionResult> GetUpcomingFollowups()
        {
            var currentDate = DateTime.Now;
            var next7Days = currentDate.AddDays(7);
            var result = await context.Tasks
                .AsNoTracking()
                .Where(t => t.NextFollowUpDate.HasValue && t.NextFollowUpDate.Value >= currentDate && t.NextFollowUpDate.Value <= next7Days)
                .Select(t => new
                {
                    TaskTitle = t.TaskTitle,
                    StudentName = t.ProjectAllocation != null && t.ProjectAllocation.Student != null ? t.ProjectAllocation.Student.FullName : "N/A",
                    FacultyName = t.ProjectAllocation != null && t.ProjectAllocation.Faculty != null ? t.ProjectAllocation.Faculty.FullName : "N/A",
                    FollowUpDate = t.NextFollowUpDate
                })
                .ToListAsync();

            var response = ApiResponse.Success(result, "Upcoming follow-up tasks retrieved successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region 12) Grade Distribution
        // 12) Grade distribution of students
        [HttpGet("grade-distribution")]
        public async Task<IActionResult> GetGradeDistribution()
        {
            var result = await context.ProjectAllocations
                .AsNoTracking()
                .Where(pa => !string.IsNullOrEmpty(pa.OverAllGrade))
                .GroupBy(pa => pa.OverAllGrade!)
                .Select(g => new
                {
                    Grade = g.Key,
                    StudentsCount = g.Select(pa => pa.StudentID).Distinct().Count()
                })
                .ToListAsync();

            var response = ApiResponse.Success(result, "Grade distribution retrieved successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region 13) Month-Wise Completed Tasks
        // 13) Month-wise completed task count
        [HttpGet("monthwise-completed-tasks")]
        public async Task<IActionResult> GetMonthwiseCompletedTasks()
        {
            var data = await context.Tasks
                .AsNoTracking()
                .Where(t => t.TaskCompletedDate.HasValue && ((t.TaskStatus != null && t.TaskStatus.TaskStatusName.ToLower() == "completed") || t.ProgressPercentage >= 100))
                .GroupBy(t => new { Year = t.TaskCompletedDate!.Value.Year, Month = t.TaskCompletedDate!.Value.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    CompletedTasks = g.Count()
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync();

            var result = data.Select(x => new
            {
                x.Year,
                MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x.Month),
                x.Month,
                x.CompletedTasks
            });

            var response = ApiResponse.Success(result, "Month-wise completed tasks retrieved successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region 14) Role-Wise Active User Count
        // 14) Role Wise Active User Count
        [HttpGet("role-wise-active-users")]
        public async Task<IActionResult> GetRoleWiseActiveUsers()
        {
            var result = await context.UserRoles
                .AsNoTracking()
                .Where(ur => ur.User != null && ur.User.IsActive && ur.Role != null)
                .GroupBy(ur => ur.Role!.RoleName)
                .Select(g => new
                {
                    Role = g.Key,
                    ActiveUsers = g.Count()
                })
                .ToListAsync();

            var response = ApiResponse.Success(result, "Role-wise active user count retrieved successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region 15) Users By Role
        // 15) Each role with users assigned to it
        [HttpGet("users-by-role")]
        public async Task<IActionResult> GetUsersByRole()
        {
            var result = await context.UserRoles
                .AsNoTracking()
                .Where(ur => ur.Role != null && ur.User != null)
                .Select(ur => new
                {
                    Role = ur.Role!.RoleName,
                    UserName = ur.User!.FullName
                })
                .OrderBy(r => r.Role)
                .ThenBy(r => r.UserName)
                .ToListAsync();

            var response = ApiResponse.Success(result, "Users by role retrieved successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region 16) Roles With More Than 10 Users
        // 16) Roles Having More Than 10 Users
        [HttpGet("roles-with-more-than-10-users")]
        public async Task<IActionResult> GetRolesWithMoreThan10Users()
        {
            var result = await context.UserRoles
                .AsNoTracking()
                .Where(ur => ur.Role != null)
                .GroupBy(ur => ur.Role!.RoleName)
                .Where(g => g.Count() > 10)
                .Select(g => new
                {
                    Role = g.Key,
                    TotalUsers = g.Count()
                })
                .ToListAsync();

            var response = ApiResponse.Success(result, "Roles having more than 10 users retrieved successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region 17) Role Statistics
        // 17) Role statistics (Total, Active, Inactive users)
        [HttpGet("role-statistics")]
        public async Task<IActionResult> GetRoleStatistics()
        {
            var result = await context.Roles
                .AsNoTracking()
                .Select(r => new
                {
                    Role = r.RoleName,
                    TotalUsers = r.UserRoles.Count(),
                    ActiveUsers = r.UserRoles.Count(ur => ur.User != null && ur.User.IsActive),
                    InactiveUsers = r.UserRoles.Count(ur => ur.User != null && !ur.User.IsActive)
                })
                .ToListAsync();

            var response = ApiResponse.Success(result, "Role statistics retrieved successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region 18) Tasks Due Next 7 Days
        // 18) Tasks due within next 7 days
        [HttpGet("tasks-due-next-7-days")]
        public async Task<IActionResult> GetTasksDueNext7Days()
        {
            var currentDate = DateTime.Now;
            var next7Days = currentDate.AddDays(7);
            var result = await context.Tasks
                .AsNoTracking()
                .Where(t => t.TaskDueDate.HasValue && t.TaskDueDate.Value >= currentDate && t.TaskDueDate.Value <= next7Days && t.ProgressPercentage < 100)
                .Select(t => new
                {
                    TaskID = t.TaskID,
                    TaskTitle = t.TaskTitle,
                    ProjectTitle = t.ProjectAllocation != null && t.ProjectAllocation.ProjectMaster != null ? t.ProjectAllocation.ProjectMaster.ProjectTitle : "N/A",
                    StudentName = t.ProjectAllocation != null && t.ProjectAllocation.Student != null ? t.ProjectAllocation.Student.FullName : "N/A",
                    DueDate = t.TaskDueDate,
                    DaysRemaining = EF.Functions.DateDiffDay(currentDate, t.TaskDueDate.Value)
                })
                .ToListAsync();

            var response = ApiResponse.Success(result, "Tasks due within next 7 days retrieved successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region 19) Project Progress Summary
        // 19) Project progress summary
        [HttpGet("project-progress-summary")]
        public async Task<IActionResult> GetProjectProgressSummary()
        {
            var result = await context.ProjectMasters
                .AsNoTracking()
                .Select(p => new
                {
                    ProjectTitle = p.ProjectTitle,
                    TasksCount = p.ProjectAllocations.SelectMany(pa => pa.Tasks).Count(),
                    CompletedTasks = p.ProjectAllocations.SelectMany(pa => pa.Tasks).Count(t => t.ProgressPercentage >= 100 || (t.TaskStatus != null && t.TaskStatus.TaskStatusName.ToLower() == "completed")),
                    PendingTasks = p.ProjectAllocations.SelectMany(pa => pa.Tasks).Count(t => t.ProgressPercentage < 100 && (t.TaskStatus == null || t.TaskStatus.TaskStatusName.ToLower() != "completed")),
                    AvgProgress = Math.Round(p.ProjectAllocations.SelectMany(pa => pa.Tasks).Any() ? (double)p.ProjectAllocations.SelectMany(pa => pa.Tasks).Average(t => t.ProgressPercentage) : (p.ProjectAllocations.Any() ? (double)p.ProjectAllocations.Average(pa => pa.ProgressPercentage) : 0), 2)
                })
                .ToListAsync();

            var response = ApiResponse.Success(result, "Project progress summary retrieved successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region 20) Project Score Analysis
        // 20) Project-wise score analysis
        [HttpGet("project-score-analysis")]
        public async Task<IActionResult> GetProjectScoreAnalysis()
        {
            var result = await context.ProjectMasters
                .AsNoTracking()
                .Select(p => new
                {
                    ProjectTitle = p.ProjectTitle,
                    TotalAssignedScore = p.ProjectAllocations.SelectMany(pa => pa.Tasks).Sum(t => (decimal?)t.AssignedScore) ?? 0m,
                    TotalEarnedScore = p.ProjectAllocations.SelectMany(pa => pa.Tasks).Sum(t => t.EarnedScore) ?? 0m,
                    ScorePercentage = (p.ProjectAllocations.SelectMany(pa => pa.Tasks).Sum(t => (decimal?)t.AssignedScore) ?? 0m) > 0 ?
                        Math.Round((double)((p.ProjectAllocations.SelectMany(pa => pa.Tasks).Sum(t => t.EarnedScore) ?? 0m) / (p.ProjectAllocations.SelectMany(pa => pa.Tasks).Sum(t => (decimal?)t.AssignedScore) ?? 1m) * 100m), 2) : 0
                })
                .ToListAsync();

            var response = ApiResponse.Success(result, "Project score analysis retrieved successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region 21) Top 10 Projects By Average Score
        // 21) Top 10 projects based on average earned score
        [HttpGet("top-10-projects")]
        public async Task<IActionResult> GetTop10Projects()
        {
            var projects = await context.ProjectMasters
                .AsNoTracking()
                .Select(p => new
                {
                    ProjectTitle = p.ProjectTitle,
                    AverageScore = p.ProjectAllocations.SelectMany(pa => pa.Tasks).Where(t => t.EarnedScore.HasValue).Any() ?
                        Math.Round((double)p.ProjectAllocations.SelectMany(pa => pa.Tasks).Where(t => t.EarnedScore.HasValue).Average(t => t.EarnedScore!.Value), 2) : 0
                })
                .OrderByDescending(p => p.AverageScore)
                .Take(10)
                .ToListAsync();

            var rankedResult = projects.Select((p, index) => new
            {
                Rank = index + 1,
                p.ProjectTitle,
                p.AverageScore
            });

            var response = ApiResponse.Success(rankedResult, "Top 10 projects retrieved successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region 22) Faculty Workload Statistics
        // 22) Faculty workload and progress statistics
        [HttpGet("faculty-workload-statistics")]
        public async Task<IActionResult> GetFacultyWorkloadStatistics()
        {
            var result = await context.ProjectAllocations
                .AsNoTracking()
                .Where(pa => pa.Faculty != null)
                .GroupBy(pa => new { pa.FacultyID, pa.Faculty!.FullName })
                .Select(g => new
                {
                    FacultyName = g.Key.FullName,
                    TotalProjects = g.Select(pa => pa.ProjectID).Distinct().Count(),
                    TotalTasks = g.SelectMany(pa => pa.Tasks).Count(),
                    AvgProgress = Math.Round(g.SelectMany(pa => pa.Tasks).Any() ? (double)g.SelectMany(pa => pa.Tasks).Average(t => t.ProgressPercentage) : (double)g.Average(pa => pa.ProgressPercentage), 2)
                })
                .ToListAsync();

            var response = ApiResponse.Success(result, "Faculty workload statistics retrieved successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region 23) Student Task Completion Statistics
        // 23) Student task completion and score statistics
        [HttpGet("student-completion-statistics")]
        public async Task<IActionResult> GetStudentCompletionStatistics()
        {
            var result = await context.ProjectAllocations
                .AsNoTracking()
                .Where(pa => pa.Student != null)
                .GroupBy(pa => new { pa.StudentID, pa.Student!.FullName })
                .Select(g => new
                {
                    StudentName = g.Key.FullName,
                    TotalTasks = g.SelectMany(pa => pa.Tasks).Count(),
                    CompletedTasks = g.SelectMany(pa => pa.Tasks).Count(t => t.ProgressPercentage >= 100 || (t.TaskStatus != null && t.TaskStatus.TaskStatusName.ToLower() == "completed")),
                    PendingTasks = g.SelectMany(pa => pa.Tasks).Count(t => t.ProgressPercentage < 100 && (t.TaskStatus == null || t.TaskStatus.TaskStatusName.ToLower() != "completed")),
                    AvgScore = Math.Round(g.SelectMany(pa => pa.Tasks).Where(t => t.EarnedScore.HasValue).Any() ? (double)g.SelectMany(pa => pa.Tasks).Where(t => t.EarnedScore.HasValue).Average(t => t.EarnedScore!.Value) : 0, 2)
                })
                .ToListAsync();

            var response = ApiResponse.Success(result, "Student completion statistics retrieved successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region 24) Overdue Projects
        // 24) Incomplete projects past due date
        [HttpGet("overdue-projects")]
        public async Task<IActionResult> GetOverdueProjects()
        {
            var currentDate = DateTime.Now;
            var result = await context.ProjectAllocations
                .AsNoTracking()
                .Where(pa => pa.ProjectEndDate < currentDate && pa.ProgressPercentage < 100)
                .Select(pa => new
                {
                    ProjectTitle = pa.ProjectMaster != null ? pa.ProjectMaster.ProjectTitle : "N/A",
                    StudentName = pa.Student != null ? pa.Student.FullName : "N/A",
                    FacultyName = pa.Faculty != null ? pa.Faculty.FullName : "N/A",
                    EndDate = pa.ProjectEndDate,
                    ProgressPercentage = pa.ProgressPercentage
                })
                .ToListAsync();

            var response = ApiResponse.Success(result, "Overdue projects retrieved successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region 25) Month-Wise Completed Tasks Numeric
        // 25) Month-wise completed task count (Numeric month version)
        [HttpGet("month-wise-completed-tasks-numeric")]
        public async Task<IActionResult> GetMonthwiseCompletedTasksNumeric()
        {
            var result = await context.Tasks
                .AsNoTracking()
                .Where(t => t.TaskCompletedDate.HasValue && ((t.TaskStatus != null && t.TaskStatus.TaskStatusName.ToLower() == "completed") || t.ProgressPercentage >= 100))
                .GroupBy(t => new { Year = t.TaskCompletedDate!.Value.Year, Month = t.TaskCompletedDate!.Value.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    CompletedTasks = g.Count()
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync();

            var response = ApiResponse.Success(result, "Month-wise completed task count retrieved successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region 26) Faculty Rankings
        // 26) Rank faculties based on average project progress
        [HttpGet("faculty-rankings")]
        public async Task<IActionResult> GetFacultyRankings()
        {
            var faculties = await context.ProjectAllocations
                .AsNoTracking()
                .Where(pa => pa.Faculty != null)
                .GroupBy(pa => new { pa.FacultyID, pa.Faculty!.FullName })
                .Select(g => new
                {
                    FacultyName = g.Key.FullName,
                    AvgProgress = Math.Round((double)g.Average(pa => pa.ProgressPercentage), 2)
                })
                .OrderByDescending(f => f.AvgProgress)
                .ToListAsync();

            var rankedResult = faculties.Select((f, index) => new
            {
                Rank = index + 1,
                f.FacultyName,
                f.AvgProgress
            });

            var response = ApiResponse.Success(rankedResult, "Faculty rankings retrieved successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region 27) Project Task Statistics
        // 27) Project-wise detailed task status breakdown
        [HttpGet("project-task-statistics")]
        public async Task<IActionResult> GetProjectTaskStatistics()
        {
            var currentDate = DateTime.Now;
            var result = await context.ProjectMasters
                .AsNoTracking()
                .Select(p => new
                {
                    ProjectTitle = p.ProjectTitle,
                    TotalTasks = p.ProjectAllocations.SelectMany(pa => pa.Tasks).Count(),
                    Completed = p.ProjectAllocations.SelectMany(pa => pa.Tasks).Count(t => t.ProgressPercentage >= 100 || (t.TaskStatus != null && t.TaskStatus.TaskStatusName.ToLower() == "completed")),
                    Pending = p.ProjectAllocations.SelectMany(pa => pa.Tasks).Count(t => t.ProgressPercentage < 100 && (t.TaskStatus == null || t.TaskStatus.TaskStatusName.ToLower() != "completed")),
                    Overdue = p.ProjectAllocations.SelectMany(pa => pa.Tasks).Count(t => t.TaskDueDate.HasValue && t.TaskDueDate.Value < currentDate && t.ProgressPercentage < 100 && (t.TaskStatus == null || t.TaskStatus.TaskStatusName.ToLower() != "completed"))
                })
                .ToListAsync();

            var response = ApiResponse.Success(result, "Project task statistics retrieved successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion
    }

    public class DashboardCountsDTO
    {
        public int TotalUsers { get; set; }
        public int TotalStudents { get; set; }
        public int TotalFaculty { get; set; }
        public int TotalProjects { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int PendingTasks { get; set; }
        public int TotalRoles { get; set; }
        public List<ProjectMaster> RecentProjects { get; set; } = new List<ProjectMaster>();
        public List<Tasks> RecentTasks { get; set; } = new List<Tasks>();
    }
}
