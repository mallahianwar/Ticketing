using System;
using System.Collections.Generic;
using System.Text;
using library.elegalhubs.com.Notification;
using library.elegalhubs.com.Ticketing;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace www.service.com.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        #region Ticketing System
        public virtual DbSet<Tickets> Tickets { get; set; }
        public virtual DbSet<TicketDetails> TicketDetails { get; set; }
        public virtual DbSet<TicketTypes> TicketTypes { get; set; }
        public virtual DbSet<StatusTypes> StatusTypes { get; set; }
        public virtual DbSet<ContactTypes> ContactTypes { get; set; }
        public virtual DbSet<CategoryTypes> CategoryTypes { get; set; }
        public virtual DbSet<PriorityTypes> PriorityTypes { get; set; }
        public virtual DbSet<ContactMethods> ContactMethods { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Departments> Departments { get; set; }
        public virtual DbSet<Projects> Projects { get; set; }
        public virtual DbSet<TicketAttachements> TicketAttachements { get; set; }
        public virtual DbSet<TicketComments> TicketComments { get; set; }
        #endregion

        #region Notifications
        public virtual DbSet<Notifications> Notifications { get; set; }
        public virtual DbSet<NotificationTypes> NotificationTypes { get; set; }
        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tickets>()
            .Property(b => b.StatusId)
            .HasDefaultValue("1");
            modelBuilder.Entity<Tickets>()
            .Property(b => b.PriorityId)
            .HasDefaultValue("0");
            modelBuilder.Entity<Tickets>()
            .Property(b => b.OpenDateTime)
            .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<TicketDetails>()
            .Property(b => b.CreatedAt)
            .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<TicketDetails>()
            .Property(b => b.RatingScore)
            .HasDefaultValue(0);
            modelBuilder.Entity<TicketAttachements>()
            .Property(b => b.AddedDate)
            .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<TicketAttachements>()
            .Property(b => b.CreatedAt)
            .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<TicketAttachements>()
            .Property(b => b.IsDelete)
            .HasDefaultValue(false);
            modelBuilder.Entity<Projects>()
            .Property(b => b.IsDelete)
            .HasDefaultValue(false);
            modelBuilder.Entity<Projects>()
            .Property(b => b.CreatedAt)
            .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<TicketTypes>()
            .Property(b => b.IsDelete)
            .HasDefaultValue(false);
            modelBuilder.Entity<TicketTypes>()
            .Property(b => b.CreatedAt)
            .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<StatusTypes>()
            .Property(b => b.CreatedAt)
            .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<StatusTypes>()
            .Property(b => b.IsDelete)
            .HasDefaultValue(false);
            modelBuilder.Entity<ContactTypes>()
            .Property(b => b.CreatedAt)
            .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<ContactTypes>()
            .Property(b => b.IsDelete)
            .HasDefaultValue(false);
            modelBuilder.Entity<CategoryTypes>()
            .Property(b => b.CreatedAt)
            .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<CategoryTypes>()
            .Property(b => b.IsDelete)
            .HasDefaultValue(false);
            modelBuilder.Entity<PriorityTypes>()
            .Property(b => b.CreatedAt)
            .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<PriorityTypes>()
            .Property(b => b.IsDelete)
            .HasDefaultValue(false);
            modelBuilder.Entity<ContactMethods>()
            .Property(b => b.CreatedAt)
            .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<ContactMethods>()
            .Property(b => b.IsDelete)
            .HasDefaultValue(false);
            modelBuilder.Entity<User>()
            .Property(b => b.CreatedAt)
            .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<User>()
            .Property(b => b.IsDelete)
            .HasDefaultValue(false);
            modelBuilder.Entity<Departments>()
            .Property(b => b.CreatedAt)
            .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Departments>()
            .Property(b => b.IsDelete)
            .HasDefaultValue(false);


            var status1 = new StatusTypes
            {
                Id = "1",
                Status = "Open",
                Color = "danger"
            };
            var status2 = new StatusTypes
            {
                Id = "2",
                Status = "In Progress",
                Color = "warning"
            };
            var status3 = new StatusTypes
            {
                Id = "3",
                Status = "Resolved",
                Color = "success"
            };
            var status4 = new StatusTypes
            {
                Id = "4",
                Status = "closed",
                Color = "success"
            };
            
            
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StatusTypes>().HasData(status1, status2, status3, status4);

            var priority0= new PriorityTypes
            {
                Id = "0",
                Priority = "Not Set",
                Color = "danger"
            };
            var priority1 = new PriorityTypes
            {
                Id = "1",
                Priority = "High",
                Color = "danger"
            };
            var priority2 = new PriorityTypes
            {
                Id = "2",
                Priority = "low",
                Color = "primary"
            };
            var priority3 = new PriorityTypes
            {
                Id = "3",
                Priority = "Medium",
                Color = "secondary"
            };
            modelBuilder.Entity<PriorityTypes>().HasData(priority0,priority1, priority2, priority3);

            var method1 = new ContactMethods
            {
                Id = "1",
                ContactMethod = "Web App"
            };
            var method2 = new ContactMethods
            {
                Id = "2",
                ContactMethod = "Email"
            };
            var method3 = new ContactMethods
            {
                Id = "3",
                ContactMethod = "Mobile App"
            };
            modelBuilder.Entity<ContactMethods>().HasData(method1, method2, method3);


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

            var category1 = new CategoryTypes
            {
                Id = "1",
                Category = "Login"
            };
            var category2 = new CategoryTypes
            {
                Id = "2",
                Category = "Dashboard"
            };
            modelBuilder.Entity<CategoryTypes>().HasData(category1, category2);

            var tickettype1 = new TicketTypes
            {
                Id = "1",
                TicketType = "Service"
            };
            var tickettype2 = new TicketTypes
            {
                Id = "2",
                TicketType = "Bug"
            };
            var tickettype3 = new TicketTypes
            {
                Id = "3",
                TicketType = "Sales"
            };
            modelBuilder.Entity<TicketTypes>().HasData(tickettype1, tickettype2, tickettype3);

            modelBuilder.Entity<Projects>().HasData(new Projects
            {
                Id = "1",
                Name ="Admin"
            },
            new Projects
            {
                Id = "2",
                Name = "Foody Link"
            });;

        }
    }
}
