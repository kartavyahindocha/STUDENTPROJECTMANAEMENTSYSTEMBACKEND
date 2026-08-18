using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Scalar.AspNetCore;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Data;
using FluentValidation;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Validators.RoleValidator;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container with JSON ReferenceHandler.IgnoreCycles to prevent object cycle loops
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddValidatorsFromAssemblyContaining<RoleCreateEditValidator>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Scalar API Reference
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Student Project Management System API")
            .WithTheme(ScalarTheme.Kepler)
            .WithCustomCss("""
                /* =========================================
                   SCALAR CUSTOM THEME
                   ========================================= */

                /* Main accent color */
                :root {
                    --scalar-color-accent: #8b5cf6;
                }

                /* Light mode */
                .light-mode {
                    --scalar-color-1: #1e1b4b;
                    --scalar-color-2: #4c1d95;
                    --scalar-color-3: #6d28d9;

                    --scalar-color-accent: #8b5cf6;

                    --scalar-background-1: #ffffff;
                    --scalar-background-2: #f5f3ff;
                    --scalar-background-3: #ede9fe;
                }

                /* Dark mode */
                .dark-mode {
                    --scalar-color-1: #f5f3ff;
                    --scalar-color-2: #ddd6fe;
                    --scalar-color-3: #a78bfa;

                    --scalar-color-accent: #a855f7;

                    --scalar-background-1: #0f0a1f;
                    --scalar-background-2: #18122b;
                    --scalar-background-3: #24183d;
                }


                /* =========================================
                   ENDPOINT / HTTP METHOD COLORS
                   ========================================= */

                /* GET - Green */
                [data-method="get"] {
                    background-color: #22c55e !important;
                    color: #ffffff !important;
                    border-color: #16a34a !important;
                }

                /* POST - Blue */
                [data-method="post"] {
                    background-color: #3b82f6 !important;
                    color: #ffffff !important;
                    border-color: #2563eb !important;
                }

                /* PUT - Orange */
                [data-method="put"] {
                    background-color: #f59e0b !important;
                    color: #ffffff !important;
                    border-color: #d97706 !important;
                }

                /* PATCH - Purple */
                [data-method="patch"] {
                    background-color: #a855f7 !important;
                    color: #ffffff !important;
                    border-color: #9333ea !important;
                }

                /* DELETE - Red */
                [data-method="delete"] {
                    background-color: #ef4444 !important;
                    color: #ffffff !important;
                    border-color: #dc2626 !important;
                }


                /* =========================================
                   ENDPOINT HOVER EFFECT
                   ========================================= */

                [data-method="get"]:hover {
                    background-color: #16a34a !important;
                }

                [data-method="post"]:hover {
                    background-color: #2563eb !important;
                }

                [data-method="put"]:hover {
                    background-color: #d97706 !important;
                }

                [data-method="patch"]:hover {
                    background-color: #9333ea !important;
                }

                [data-method="delete"]:hover {
                    background-color: #dc2626 !important;
                }


                /* =========================================
                   SCALAR LINKS / ACCENT
                   ========================================= */

                a {
                    color: #8b5cf6 !important;
                }

                a:hover {
                    color: #a855f7 !important;
                }


                /* =========================================
                   BUTTONS
                   ========================================= */

                button:hover {
                    border-color: #8b5cf6 !important;
                }


                /* =========================================
                   SEARCH INPUT
                   ========================================= */

                input:focus {
                    border-color: #8b5cf6 !important;
                    box-shadow: 0 0 0 2px rgba(139, 92, 246, 0.25) !important;
                }


                /* =========================================
                   SELECTED / ACTIVE ELEMENT
                   ========================================= */

                [aria-current="true"] {
                    color: #8b5cf6 !important;
                }


                /* =========================================
                   SCROLLBAR
                   ========================================= */

                ::-webkit-scrollbar {
                    width: 8px;
                    height: 8px;
                }

                ::-webkit-scrollbar-thumb {
                    background-color: #8b5cf6;
                    border-radius: 10px;
                }

                ::-webkit-scrollbar-track {
                    background-color: transparent;
                }
            """);
    });
}

app.MapGet("/", () => Results.Redirect("/scalar"));

app.UseAuthorization();

app.MapControllers();

app.Run();
