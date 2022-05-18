using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace library.elegalhubs.com.Ticketing
{
    //[Table("Ticketing")]
    public class Tickets
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public int TicketNumber { get; set; }

        [ForeignKey("TicketTypes")]
        [Required]

        [Display(Name = "Ticket Type")]
        public string TicketTypeId { get; set; }
        public virtual TicketTypes TicketTypes { get; set; }
        
        [ForeignKey("Projects")]
        [Required]

        [Display(Name = "Project")]
        public string ProjectId { get; set; }
        public virtual Projects Projects { get; set; }

        [ForeignKey("ContactTypes")]
        //[Required]

        [Display(Name = "Contact Type")]
        public string ContactId { get; set; }
        public virtual ContactTypes ContactTypes { get; set; }
        [Required]
        public string Subject { get; set; }

        [ForeignKey("StatusTypes")]

        [Display(Name = "Statue Type")]
        public string StatusId { get; set; }
        public virtual StatusTypes StatusTypes { get; set; }

        [Display(Name = "Open DateTime")]
        public DateTime OpenDateTime { get; set; } //= DateTime.Now;
        [Display(Name = "Close DateTime")]
        public DateTime? CloseDateTime { get; set; }

        [ForeignKey("CategoryTypes")]
        [Required]

        [Display(Name = "Category Type")]
        public string CategoryId { get; set; }
        public virtual CategoryTypes CategoryTypes { get; set; }

        [ForeignKey("PriorityTypes")]

        [Display(Name = "Priority Type")]
        public string PriorityId { get; set; } //= "0";
        public virtual PriorityTypes PriorityTypes { get; set; }

        [ForeignKey("ContactMethods")]
        [Required]
        public string PreferredContactMethodId { get; set; }
        //[Required]

        [Display(Name = "Contact Methods")]
        public virtual ContactMethods ContactMethods { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }
        //[ForeignKey("Employee")]
        //public string AssignTo { get; set; }
        //public virtual User Employee { get; set; }

        [ForeignKey("Departments")]
        public string DepartmentId { get; set; }
        public virtual Departments Department { get; set; }
        //[NotMapped]
        //public string Category { get; set; }
        public bool IsDelete { get; set; } = false;
    }

    public class TicketDetails
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [ForeignKey("Tickets")]
        public string TicketId { get; set; }
        public virtual Tickets Tickets { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        //[Required]
        [Display(Name = "Start DateTime")]
        public DateTime? StartDateTime { get; set; }
        //[Required]
        [Display(Name = "End DateTime")]
        public DateTime? EndDateTime { get; set; }
        //public byte[] Attachment { get; set; }
        public int RatingScore { get; set; } //= 0;
        public DateTime CreatedAt { get; set; } //= DateTime.Now;
        public string CreatedBy { get; set; }
        public bool IsDelete { get; set; } = false;

    }
    public class TicketComments
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [ForeignKey("Tickets")]
        public string TicketId { get; set; }
        public virtual Tickets Tickets { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User user { get; set; }
        public DateTime CommentDate { get; set; }
        public bool IsDelete { get; set; } = false;

    }

    public class TicketAttachements
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey("Tickets")]
        public string TicketId { get; set; }
        public virtual Tickets Tickets { get; set; }
        public DateTime AddedDate { get; set; } //= DateTime.Now;
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User user { get; set; }
        [Required]
        public byte[] Attachment { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public string FileExt { get; set; }
        public DateTime CreatedAt { get; set; } //= DateTime.Now;
        public string CreatedBy { get; set; }
        public bool IsDelete { get; set; } //= false;

    }
    public class Projects
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Name { get; set; }
        public string APIEndPoint { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; } //= DateTime.Now;
        public string CreatedBy { get; set; }
        public bool IsDelete { get; set; } //= false;
    }

    public class TicketTypes
    {
        [Key]

        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [Display(Name = "Ticket Type")]
        public string TicketType { get; set; }
        public DateTime CreatedAt { get; set; } //= DateTime.Now;
        public string CreatedBy { get; set; }
        public bool IsDelete { get; set; } //= false;
    }

    public class StatusTypes
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Status { get; set; }
        public string Color { get; set; }
        public DateTime CreatedAt { get; set; } //= DateTime.Now;
        public string CreatedBy { get; set; }
        public bool IsDelete { get; set; } //= false;
    }

    public class ContactTypes
    {
        [Key]

        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [Display(Name = "Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }
        public DateTime CreatedAt { get; set; } //= DateTime.Now;
        public string CreatedBy { get; set; }
        public bool IsDelete { get; set; } //= false;
    }

    public class CategoryTypes
    {
        [Key]

        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Display(Name = "Category")]
        [Required]
        public string Category { get; set; }
        public DateTime CreatedAt { get; set; } //= DateTime.Now;
        public string CreatedBy { get; set; }
        public bool IsDelete { get; set; } //= false;
    }

    public class PriorityTypes
    {
        [Key]

        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Priority { get; set; }
        public string Color { get; set; }
        public DateTime CreatedAt { get; set; } //= DateTime.Now;
        public string CreatedBy { get; set; }
        public bool IsDelete { get; set; } //= false;
    }

    public class ContactMethods
    {
        [Key]

        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Display(Name = "Contact Method")]
        [Required]
        public string ContactMethod { get; set; }
        public DateTime CreatedAt { get; set; } //= DateTime.Now;
        public string CreatedBy { get; set; }
        public bool IsDelete { get; set; } //= false;
    }

    public class User
    {
        [Key]

        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }
        [Required]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        [ForeignKey("Projects")]
        public string ProjectId { get; set; }
        public virtual Projects Projects { get; set; }
        //اي دي اليوزر في البروجكت الي أرسللنا منه وذلك حتى يتم انشاء يوزر واحد هنا لاي يوزر بضيف تكيكت مهما كان عددها
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; } //= DateTime.Now;
        public string CreatedBy { get; set; }
        public bool IsDelete { get; set; } //= false;
    }

    public class Departments
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; } //= DateTime.Now;
        public string CreatedBy { get; set; }
        public bool IsDelete { get; set; } //= false;
    }
}