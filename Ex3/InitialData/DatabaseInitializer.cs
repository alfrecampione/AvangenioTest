using Ex3.Data.Model;
using Ex3.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InitialData;

public interface IDatabaseInitializer
{
    void EnsureInitialData();
}

public class DatabaseInitializer(ApplicationDbContext context, RoleManager<IdentityRole> roleManager,
    UserManager<User> userManager) : IDatabaseInitializer
{
    public void EnsureInitialData()
    {
        if (!AllMigrationsApplied()) throw new Exception("Not all migrations applied before seeding");

        SeedData();
    }


    private bool AllMigrationsApplied()
    {
        var applied = context.GetService<IHistoryRepository>().GetAppliedMigrations().Select(m => m.MigrationId);
        var total = context.GetService<IHistoryRepository>().GetAppliedMigrations().Select(m => m.MigrationId);
        return !total.Except(applied).Any();
    }


    private void SeedData()
    {
        var roleSeeder = new RoleSeeder(roleManager);
        var userSeeder = new UserSeeder(userManager, roleManager);
        userSeeder.Seed().Wait();
    }
}

public class RoleSeeder(RoleManager<IdentityRole> roleManager)
{
    public async Task Seed()
    {
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            var admin = new IdentityRole { Name = "Admin" };
            await roleManager.CreateAsync(admin);
        }

        if (!await roleManager.RoleExistsAsync("Manager"))
        {
            var moderator = new IdentityRole { Name = "Manager" };
            await roleManager.CreateAsync(moderator);
        }

        if (!await roleManager.RoleExistsAsync("Project Administrator"))
        {
            var user = new IdentityRole { Name = "Project Administrator" };
            await roleManager.CreateAsync(user);
        }

        if (!await roleManager.RoleExistsAsync("User"))
        {
            var user = new IdentityRole { Name = "User" };
            await roleManager.CreateAsync(user);
        }
    }
}

public class UserSeeder(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
{
    public async Task Seed()
    {
        if (await userManager.FindByNameAsync("admin") == null)
        {
            var user = new User
            {
                FirstName = "Alfredo",
                LastName = "Montero",
                UserName = "admin",
                Email = "alfrecampione@gmail.com",
                Country = "CU"
            };
            var result = await userManager.CreateAsync(user, "admin");
            if (result.Succeeded && await roleManager.RoleExistsAsync("Admin"))
                await userManager.AddToRoleAsync(user, "Admin");
        }
    }
}