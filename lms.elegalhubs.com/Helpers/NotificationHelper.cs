using library.elegalhubs.com.Notification;
using lms.elegalhubs.com.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lms.elegalhubs.com.Helpers
{
    public static class NotificationHelper
    {
        public static async Task SendNotification(ApplicationDbContext context, Notifications obj)
        {
            //context.Notifications.Add(obj);
            try
            {
                //await context.SaveChangesAsync();
                if(obj.NotificationTypeId == "2")
                {
                    MailHelper.SendEmail(obj.Subject, obj.Reciver.EmailAddress, obj.Message);
                }
            }
            catch(Exception ex)
            {

            }
        }
    }
}
