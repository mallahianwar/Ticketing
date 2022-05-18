using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace library.elegalhubs.com.lms.Admin
{
    public class Users : IdentityUser
    {
        [Required]
        public string FullName { get; set; }
        [MaxLength]
        [Column(TypeName = "image")]
        public byte[] Avtar { get; set; }
        public string Address { get; set; }
        //[Required]
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; }
        public bool IsDelete { get; set; } = false;
    }
}
