using library.elegalhubs.com.lms.Admin;
using library.elegalhubs.com.Ticketing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace lms.elegalhubs.com.Areas.Admin.Models.Tickets
{
    public class IndexViewModel
    {
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
        [ForeignKey("Employee")]
        public string AssignTo { get; set; }
        public virtual Users Employee { get; set; }

        [ForeignKey("Departments")]
        public string DepartmentId { get; set; }
        public virtual Departments Department { get; set; }
        //[NotMapped]
        //public string Category { get; set; }
        public bool IsDelete { get; set; } = false;
    }
}
