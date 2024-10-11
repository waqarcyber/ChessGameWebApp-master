using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.Generic;


namespace DbContextDao
{
    public class AuthContext : DbContext
    {
        public DbSet<Account> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public AuthContext(DbContextOptions<AuthContext> options) : base(options)
        {
            
            if (Database.EnsureCreated())
            {
                DefaultConfigure();
            }

        }

        private void DefaultConfigure()
        {
            CreateRoles();
            CreateAdmin();
        }

        private void CreateRoles()
        {
            Roles.Add(new Role() { Name = "user" });
            Roles.Add(new Role() { Name = "admin" });
            SaveChanges();
        }

        private void CreateAdmin()
        {
            var user = new Account()
            {
                Username = "admin",
                //admin
                Password = @"AQAAAAEAACcQAAAAEMd4eLM2HUtked/uK+0KtvObLD3OZRnQxT/Z3E9TOo/D4ktXvTzX0JfaC/7csbUkYQ==",
                Login = "admin",
                Email = "admin@admin.org",
                IsBanned = false
            };

            user.Roles.Add(Roles.First(r => r.Name == "admin"));
            user.Roles.Add(Roles.First(r => r.Name == "user"));
            Users.Add(user);
            SaveChanges();
        }

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;port=9999;user=root;password=qwerty;database=Auth;",
                new MySqlServerVersion(new Version(8, 0, 28))
            );
        }*/
    }
}