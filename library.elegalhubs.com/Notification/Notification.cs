using library.elegalhubs.com.Ticketing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace library.elegalhubs.com.Notification
{
    [Table("Notification")]
    public class Notifications
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Module { get; set; }

        [ForeignKey("NotificationTypes")]
        public string NotificationTypeId { get; set; }
        public virtual NotificationTypes NotificationTypes { get; set; }

        public string Status { get; set; }
        public string Purpose { get; set; }
        public string Subject { get; set; }

        public string ActivityDate { get; set; }
        public string Message { get; set; }
        [ForeignKey("Sender")]
        public string UserFrom { get; set; }
        [ForeignKey("Reciver")]
        public string UserTo { get; set; }
        public User Sender { get; set; }
        public User Reciver { get; set; }
    }

    [Table("NotificationTypes")]
    public class NotificationTypes
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string NotificationType { get; set; }
    }

}
