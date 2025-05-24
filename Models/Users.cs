using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace _26._02_sushi_market_back.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }

        public string? Email { get; set; }
    }

    public class UserRequestLog
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }

    }
    public class UserRequestReg
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }

        public string? Email { get; set; }
    }



    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;

      
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //    var config = new ConfigurationBuilder()
        //                    .AddJsonFile("appsettings.json")
        //                    .SetBasePath(Directory.GetCurrentDirectory())
        //                    .Build();

        //    optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        //}
    }
}
