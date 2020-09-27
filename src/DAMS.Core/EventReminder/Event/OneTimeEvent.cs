using DAMS.EventReminder.Notifier;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAMS.EventReminder.Event
{
   public class OneTimeEvent : IEvent
    {
        private INotifier notify_type;

        public DateTime Date { get; set; }
        public DateTime NextNotificationDate { get { return Date - NotifyBefore; } }
        public string Name { get; set; }
        public TimeSpan NotifyBefore { get; set; }
        public int Id { get; set; }
        public EventStatus Status { get; set; }
        public NotifyStatus NotifyStatus { get; set; }
        [NotMapped]
        public INotifier Notification { get { return notify_type; } set { notify_type = value; } }

        public OneTimeEvent()
        {
                
        }


        public OneTimeEvent( DateTime date)
        {
            Date = date;
            Name = "My Event";
            NotifyBefore = new TimeSpan(0, 5, 0);
            Status = EventStatus.Active;
            NotifyStatus = NotifyStatus.None;

        }
        public OneTimeEvent( DateTime date, string name, TimeSpan time, EventStatus status)
        { 
            Date = date;
            Name = name;
            NotifyBefore = time;
            Status = status;
        }


        public void Notify()
        {
            
            var eventInfo = new NotificationInfo(Name, Date,"");
            NotificationResult notificationResult = notify_type.Notify(eventInfo);
            UpdateStatus(notificationResult);
        }

        public void UpdateStatus(NotificationResult result)
        {
            if (result.IsSuccess == true)
            {
                Status = EventStatus.Closed;
            }
            if (result.IsSuccess == false)
            {
                Status = EventStatus.Failed;
            }
        }
        public override string ToString()
        {
            return "Id:"+Id+ "  "+ "Name: "+ Name +"  " +"Date: "+ Date+ "  "+"Status: "+ Status +" NotifyStatus:"+ NotifyStatus;
        }
    }
}
