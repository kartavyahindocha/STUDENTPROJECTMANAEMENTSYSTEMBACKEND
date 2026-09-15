using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Migrations
{
    /// <inheritdoc />
    public partial class status_seed_safe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM [SPM_TaskStatus] WHERE [TaskStatusName] = 'Not Started')
                    INSERT INTO [SPM_TaskStatus] ([TaskStatusName], [TaskStatusCssClass], [CreatedBy], [CreatedOn]) VALUES ('Not Started', 'tone-neutral', 0, GETDATE());

                IF NOT EXISTS (SELECT 1 FROM [SPM_TaskStatus] WHERE [TaskStatusName] = 'Under Review')
                    INSERT INTO [SPM_TaskStatus] ([TaskStatusName], [TaskStatusCssClass], [CreatedBy], [CreatedOn]) VALUES ('Under Review', 'tone-primary', 0, GETDATE());

                IF NOT EXISTS (SELECT 1 FROM [SPM_TaskStatus] WHERE [TaskStatusName] = 'On Hold')
                    INSERT INTO [SPM_TaskStatus] ([TaskStatusName], [TaskStatusCssClass], [CreatedBy], [CreatedOn]) VALUES ('On Hold', 'tone-neutral', 0, GETDATE());

                IF NOT EXISTS (SELECT 1 FROM [SPM_TaskStatus] WHERE [TaskStatusName] = 'Assigned')
                    INSERT INTO [SPM_TaskStatus] ([TaskStatusName], [TaskStatusCssClass], [CreatedBy], [CreatedOn]) VALUES ('Assigned', 'tone-success', 0, GETDATE());

                IF NOT EXISTS (SELECT 1 FROM [SPM_TaskStatus] WHERE [TaskStatusName] = 'Unassigned')
                    INSERT INTO [SPM_TaskStatus] ([TaskStatusName], [TaskStatusCssClass], [CreatedBy], [CreatedOn]) VALUES ('Unassigned', 'tone-warning', 0, GETDATE());
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM [SPM_TaskStatus] WHERE [TaskStatusName] IN ('Not Started', 'Under Review', 'On Hold', 'Assigned', 'Unassigned');
            ");
        }
    }
}
