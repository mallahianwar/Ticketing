using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace library.elegalhubs.com.Chat
{
    public class Chat
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FromProject { get; set; }
        public string ToProject { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string Content { get; set; }
        public Byte[] ImageId { get; set; }
        public Byte[] Document { get; set; }
        public Byte[] VoiceRecord { get; set; }
    }
}
