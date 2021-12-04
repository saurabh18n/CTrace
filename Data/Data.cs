using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTrace.Helpers;
using CTrace.Models;


namespace CTrace.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string salt;
            modelBuilder.Entity<User>().Property(up => up.user_failcount).HasDefaultValueSql("0");
            modelBuilder.Entity<User>().Property(up => up.user_lastlogin).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<User>().HasIndex(u => u.user_name).IsUnique();
            modelBuilder.Entity<User>().HasData(new User() {user_name="admin",user_isadmin = true, user_fname="Admin",user_pass=PasswordHasher.GenrateHash("admin",out salt),user_salt=salt, user_id=1});
        }
        public DbSet<User> Users { get; set; }
    }
}
