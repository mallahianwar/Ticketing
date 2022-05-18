using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace library.elegalhubs.com.lms.Admin
{
    public class TicketEmployees
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string TicketID { get; set; }
        [ForeignKey("Employee")]
        public string AssignTo { get; set; }
        public virtual Users Employee { get; set; }
        public DateTime AssignDate { get; set; }
        public bool IsCurrent { get; set; }
    }
}
