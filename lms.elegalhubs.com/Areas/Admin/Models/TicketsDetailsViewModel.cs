using library.elegalhubs.com.Ticketing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace lms.elegalhubs.com.Areas.Admin.Models
{
    public class TicketsDetailsViewModel
    {
        public library.elegalhubs.com.Ticketing.Tickets Tickets { get; set; }
        public TicketDetailsVM TicketsDetails { get; set; }
        public TicketAttachementsVM TicketAttachements { get; set; }
    }
    public class TicketIndexViewModel
    {
        public library.elegalhubs.com.Ticketing.Tickets Tickets { get; set; }
        public TicketDetails TicketDetails { get; set; }
        public IEnumerable<TicketAttachements> TicketAttachements { get; set; }
        public IEnumerable<TicketComments> TicketComments { get; set; }
    }
    public class TicketDetailsVM
    {
        public string TicketId { get; set; }
        //public virtual Tickets Tickets { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Start DateTime")]
        public DateTime? StartDateTime { get; set; }
        [Display(Name = "End DateTime")]
        public DateTime? EndDateTime { get; set; }

    }
    public class TicketCommentsVM
    {
        //[Required]
        public string TicketId { get; set; }
        public virtual library.elegalhubs.com.Ticketing.Tickets Tickets { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        //[ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User user { get; set; }
        public DateTime CommentDate { get; set; }
    }

    public class TicketAttachementsVM
    {
        //[ForeignKey("Tickets")]
        public string TicketId { get; set; }
        public virtual library.elegalhubs.com.Ticketing.Tickets Tickets { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.Now;
        //[ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User user { get; set; }
        [Required]
        public byte[] Attachment { get; set; }
    }
}
