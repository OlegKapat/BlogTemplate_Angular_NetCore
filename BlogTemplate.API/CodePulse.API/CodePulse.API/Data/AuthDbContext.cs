using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var readerRoleId = "ff51ed52-eaf9-4f59-b4c8-18103c29221a";
            var writerRoleId = "191e3d92-9d23-4b3d-bae7-97fc67511d2f";
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "READER",
                    ConcurrencyStamp = readerRoleId
                },
                new IdentityRole
                {
                    Id = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "WRITER",
                    ConcurrencyStamp = writerRoleId
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);
            var adminUserId = "f1b3b3b4-3b3b-4b3b-b3b3-3b3b3b3b3b3b";
            var admin = new IdentityUser
            {
                Id = adminUserId,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@ukr.net",
                NormalizedEmail = "admin@ukr.net".ToUpper(),
            };
            var passwordHasher = new PasswordHasher<IdentityUser>().HashPassword(
                admin,
                "Admin123!"
            );
            builder.Entity<IdentityUser>().HasData(admin);
            var adminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string> { RoleId = readerRoleId, UserId = adminUserId },
                new IdentityUserRole<string> { RoleId = writerRoleId, UserId = adminUserId }
            };
            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }
    }
}
