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
            modelBuilder.Entity<User>().Property(up => up.user_id).HasDefaultValue(Guid.NewGuid());
            modelBuilder.Entity<User>().Property(up => up.user_failcount).HasDefaultValueSql("0");
            modelBuilder.Entity<User>().Property(up => up.user_lastlogin).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<User>().HasIndex(u => u.user_mobile).IsUnique();
            modelBuilder.Entity<User>().HasData(new User() { user_mobile = "admin",user_isadmin = true, user_name="Admin",user_pass=PasswordHasher.GenrateHash("admin",out salt),user_salt=salt,user_id= Guid.NewGuid()});

            modelBuilder.Entity<Contact>().HasOne(c => c.PrimaryUser).WithMany().HasForeignKey(nameof(Contact.PrimaryUserId)).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Contact>().HasOne(c => c.SecondUser).WithMany().HasForeignKey(nameof(Contact.SecondUserId)).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Contact>().HasOne(c => c.CreatedBy).WithMany().HasForeignKey(nameof(Contact.CreatedById)).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Notification>().HasOne(N => N.User).WithMany().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Detection>().HasOne(d => d.User).WithMany().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Detection>().HasOne(d => d.CreatedBy).WithMany().OnDelete(DeleteBehavior.NoAction);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Detection> Detections { get; set; }

    }
}
