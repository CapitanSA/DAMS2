using DAMS.EventReminder.Notifier;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAMS.EventReminder.Event
{
   public class OneTimeEvent : IEvent
    {
        public DateTime Date { get; set; }
        public DateTime NextNotificationDate { get { return Date - NotifyBefore; } }
        public string Name { get; set; }
        public TimeSpan NotifyBefore { get; set; }
        public INotifier Notifier { get; set; }
        public EventStatus Status { get; set; }


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
            Notifier.Notify();
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
