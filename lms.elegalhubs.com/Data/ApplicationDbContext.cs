using library.elegalhubs.com.lms.Admin;
using library.elegalhubs.com.lms.Admin.Chat;
using library.elegalhubs.com.Ticketing;
using lms.elegalhubs.com.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace lms.elegalhubs.com.Data
{
    public class ApplicationDbContext : IdentityDbContext<Users>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Users>()
                .Property(e => e.FullName)
                .HasMaxLength(250);

            modelBuilder.Entity<Users>()
                .Property(e => e.Avtar)
                .HasMaxLength(250);

            modelBuilder.Entity<Users>()
                .Property(e => e.Email)
                .IsRequired();

            modelBuilder.Entity<Users>()
                .Property(e => e.PhoneNumber)
                .IsRequired();

            var type1 = new NotificationTypes
            {
                Id = "1",
                NotificationType = "Web App"
            };
            var type2 = new NotificationTypes
            {
                Id = "2",
                NotificationType = "Email"
            };
            var type3 = new NotificationTypes
            {
                Id = "3",
                NotificationType = "Mobile App"
            };
            modelBuilder.Entity<NotificationTypes>().HasData(type1, type2, type3);
            modelBuilder.Entity<Notifications>()
            .Property(b => b.ActivityDate)
            .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Notifications>()
            .Property(b => b.Status)
            .HasDefaultValue("0");// 0 not open 
                                  // 1 opended
            //modelBuilder.Entity<TicketEmployees>()
            //.HasKey(b => new { b.TicketID, b.AssignTo });

            modelBuilder.Entity<TicketEmployees>()
            .Property(b => b.AssignDate)
            .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<TicketEmployees>()
            .Property(b => b.IsCurrent)
            .HasDefaultValue(true);
        }
        public virtual DbSet<UsersMessages> UsersMessages { get; set; }
        public DbSet<lms.elegalhubs.com.Areas.Admin.Models.Notifications> Notifications { get; set; }
        public DbSet<lms.elegalhubs.com.Areas.Admin.Models.NotificationTypes> NotificationTypes { get; set; }
        public DbSet<TicketEmployees> TicketEmployees { get; set; }

    }
}
