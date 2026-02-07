using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BlueCraeftBowl.Data;
using BlueCraeftBowl.Models;
using System.Security.Claims;

namespace BlueCraeftBowl.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        // 1. Ensure Database is created/migrated
        // Note: For local development with LocalDB, EnsureCreated is often enough, 
        // but MigrateAsync is better for production scenarios with migrations.
        await context.Database.EnsureCreatedAsync();

        // 2. Ensure Admin User exists
        var adminEmail = configuration["InitialAdmin:Email"] ?? "admin@bluecraeftbowl.com";
        var adminPassword = configuration["InitialAdmin:Password"] ?? "Admin123!";
        
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "System Admin",
                IsActive = true,
                IsAdmin = true,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            
            if (result.Succeeded)
            {
                // Add the claims required by policies in Program.cs
                await userManager.AddClaimAsync(adminUser, new Claim("IsAdmin", "True"));
                await userManager.AddClaimAsync(adminUser, new Claim("IsActive", "True"));
            }
        }

        // 3. Ensure a Parlay Session exists
        if (!await context.ParlaySessions.AnyAsync())
        {
            context.ParlaySessions.Add(new ParlaySession 
            { 
                Name = "BlueCraeft Bowl 2026", 
                GameStartTime = DateTime.Now.AddDays(1) 
            });
            await context.SaveChangesAsync();
        }
    }
}
