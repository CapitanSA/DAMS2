using DAMS.EventReminder.Notifier;
using System;

namespace DAMS.EventReminder.Event
{
   public class OneTimeEvent : IEvent
    {
        private INotifier Notifier;

        public DateTime Date { get; set; }
        public DateTime NextNotificationDate { get { return Date - NotifyBefore; } }
        public string Name { get; set; }
        public TimeSpan NotifyBefore { get; set; }
        public int Id { get; set; }

        public EventStatus Status { get; set; }

        public OneTimeEvent()
        {
                
        }


        public OneTimeEvent(INotifier notifier, DateTime date)
        {
            Notifier = notifier;
            Date = date;
            Name = "My Event";
            NotifyBefore = new TimeSpan(0, 5, 0);
            Status = EventStatus.Active;

        }
        public OneTimeEvent(INotifier notifier, DateTime date, string name, TimeSpan time, EventStatus status)
        {
            Notifier = notifier;
            Date = date;
            Name = name;
            NotifyBefore = time;
            Status = status;
        }


        public void Notify()
        {
            var eventInfo = new NotificationInfo(Name, Date,"");
            NotificationResult notificationResult = Notifier.Notify(eventInfo);
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
    }
}
