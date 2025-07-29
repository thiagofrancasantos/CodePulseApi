using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data;

public class AuthDbContext : IdentityDbContext
{
    public AuthDbContext(DbContextOptions options) : base(options)
    {    
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var readerRoleId = "c6bc7e7e-01a4-4a27-8271-4b0aa6767ba5";
        var writerRoleId = "dbe07dcc-bdcd-45bc-81dc-56fdf52559e6";

        // Create reader and writer role
        var roles = new List<IdentityRole>
        {
            new IdentityRole()
            {
                Id = readerRoleId,
                Name = "Reader",
                NormalizedName = "Reader".ToUpper(),
                ConcurrencyStamp = readerRoleId
            },

            new IdentityRole(){
                Id = writerRoleId,
                Name = "Writer",
                NormalizedName = "Writer".ToUpper(),
                ConcurrencyStamp= writerRoleId
            }
        };

        // Seed the roles
        builder.Entity<IdentityRole>().HasData(roles);

        // Create an Admin User
        var adminUserId = "c9a183af-f94f-41c9-9be4-970a6e69539b";
        var admin = new IdentityUser()
        {
            Id = adminUserId,
            UserName = "admin@codepulse.com",
            Email = "admin@codepulse.com",
            NormalizedEmail = "admin@codepulse.com".ToUpper(),
            NormalizedUserName = "admin@codepulse.com".ToUpper()
        };

        admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");

        builder.Entity<IdentityUser>().HasData(admin);

        // Give Roles to Admin
        var adminRoles = new List<IdentityUserRole<string>>()
        {
            new()
            {
               UserId = adminUserId,
               RoleId = readerRoleId
            },
            new()
            {
                UserId = adminUserId,
                RoleId = writerRoleId
            }
        };

        builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
    }
}
