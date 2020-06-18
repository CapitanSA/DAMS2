using DAMS.EventReminder.Event;
using DAMS.EventReminder.Notifier;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace DAMS.EventReminder
{
    // TODO: Refactor: extract logic responsible for calculation of the next event date to private method.
    //       Use this method wherever it could be used.

    public class CustomEvent : IEvent
    {
        private INotifier Notifier;
         
        public IDictionary<DateTime, EventStatus> Dates { get; set; }
        public int Id { get; set; }

        public DateTime NextNotificationDate
        {
            get
            {
                foreach (KeyValuePair<DateTime, EventStatus> element in Dates)
                {
                    if (element.Value == EventStatus.Active)
                    {
                        return element.Key - NotifyBefore;
                    }
                }
                return DateTime.MinValue;
            }
        }
        public string Name { get; set; }
        public TimeSpan NotifyBefore { get; set; }        
        public EventStatus Status { get; set; }


        public CustomEvent()
        {
                
        }
        public CustomEvent(INotifier notifier, IEnumerable<DateTime> dates)
        {
            Notifier = notifier;
            Dates = dates.ToDictionary((current) => current, (current) => EventStatus.Active);
            Name = "My Event";
            NotifyBefore = new TimeSpan(0, 5, 0);
            Status = EventStatus.Active;
        }

        public CustomEvent(INotifier notifier, IEnumerable<DateTime> dates, string name, TimeSpan time, EventStatus status)
        {
            Notifier = notifier;
            Dates = dates.ToDictionary((current) => current, (current) => EventStatus.Active);
            Name = name;
            NotifyBefore = time;
            Status = status;
        }


        public void Notify()
        {
            // Remove following line after GetNextEventDate() will be implemented.
            //DateTime nextEventDate = Dates.First(d => d.Value == EventStatus.Active).Key;
            DateTime nextEventDate = GetNextEventDate();

            var eventInfo = new NotificationInfo(Name, nextEventDate,"");
            NotificationResult notificationResult = Notifier.Notify(eventInfo);
            UpdateStatus(notificationResult);
        }

        public void UpdateStatus(NotificationResult result)
        {
            foreach (KeyValuePair<DateTime, EventStatus> element in Dates)
            {
                if (element.Value == EventStatus.Active)
                {
                    if (result.IsSuccess == true)
                    {
                        Dates[element.Key] = EventStatus.Closed;
                    }
                    else
                    {
                        Dates[element.Key] = EventStatus.Failed;
                    }
                    break;
                }
            }
        }

        private DateTime GetNextEventDate()
        {
            throw new NotImplementedException();
        }
    }
}
