using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace library.elegalhubs.com.lms.Admin.Chat
{
    public class UsersMessages
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [ForeignKey("Sender")]
        public string UserFrom { get; set; }
        [ForeignKey("Reciver")]
        public string UserTo { get; set; }
        public Users Sender { get; set; }
        public Users Reciver { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string Content { get; set; }
        public Byte[] ImageId { get; set; }
        public Byte[] Document { get; set; }
        public Byte[] VoiceRecord { get; set; }
        public bool IsDelete { get; set; } = false;

    }
}
