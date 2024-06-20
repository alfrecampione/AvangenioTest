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

public class DatabaseInitializer(ApplicationDbContext context, UserManager<User> userManager)
    : IDatabaseInitializer
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
        // if (!_context.Set<Faculty>().Any())
        //     _context.Set<Faculty>().AddRange(InitialDomainData.GetFaculties());
        
        var userSeeder = new UserSeeder(userManager);
        userSeeder.Seed().Wait();
    }
}

public class UserSeeder(UserManager<User> userManager)
{
    public async Task Seed()
    {
        if (await userManager.FindByNameAsync("admin") == null)
        {
            var user = new User
            {
                FirstName = "Alfredo",
                LastName = "Montero",
                UserName = "admin"
            };
            var result = await userManager.CreateAsync(user, "admin");
        }
    }
}