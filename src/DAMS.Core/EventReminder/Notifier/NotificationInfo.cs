using System;

namespace DAMS.EventReminder.Notifier
{
    public class NotificationInfo
    {
        public string EventName { get; }
        public DateTime EventDate { get; }
        public string EmailRecipient { get; set; }
        public NotificationInfo()
        {
                
        }

        public NotificationInfo(string eventName, DateTime eventDate, string emailRecipient)
        {
            EventName = eventName;
            EventDate = eventDate;
            EmailRecipient = emailRecipient;
        }
    }
}