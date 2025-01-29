using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    //Purpose of the Code: This code is creating a special kind of database context for authentication purposes.
    //It will handle users, roles, and other identity-related information like login credentials.
    //NZWalksAuthDbContext--Ctrl+.-->Generate component with option parameter
    //IdentityDbContext, which is a built-in class that already knows how to handle user authentication data (like users and roles).

    public class NZWalksAuthDbContext : IdentityDbContext
    {
        //This constructor is like a setup function. It accepts configuration options (DbContextOptions) needed to connect to the database.
        //The base(options) part passes these settings to the parent class (IdentityDbContext).
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
        {
        }

        //Seeding data
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "929a8a8f-704d-4e14-961e-0f9da08c110e";
            var writeRoleId = "e61c024b-381b-4c95-8aba-a51eb6a03275";

            var roles = new List<IdentityRole>         
            {
                new IdentityRole
                {
                    Id = readerRoleId,
                    ConcurrencyStamp = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper()
                },
                new IdentityRole
                {
                    Id = writeRoleId,
                    ConcurrencyStamp = writeRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper()
                }
            };

            //seed in builder object
            builder.Entity<IdentityRole>().HasData(roles);

        }

    }
}
