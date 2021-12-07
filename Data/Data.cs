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

            modelBuilder.Entity<Contact>().HasOne(c => c.PrimaryUser).WithMany().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Contact>().HasOne(c => c.SecondUser).WithMany().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Contact>().HasOne(c => c.CreatedBy).WithMany().OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Notification>().HasOne(N => N.notif_user).WithMany().OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Detection>().HasOne(d => d.User).WithMany().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Detection>().HasOne(d => d.CreatedBy).WithMany().OnDelete(DeleteBehavior.NoAction);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Detection> Detections { get; set; }
    }
}
