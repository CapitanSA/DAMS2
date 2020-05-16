﻿using DAMS.EventReminder.Event;
using DAMS.EventReminder.Notifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DAMS.EventReminder
{
    public class CustomEvent : IEvent
    {
        public IDictionary<DateTime, EventStatus> Dates { get; set; }
        public DateTime NextNotificationDate { get; set; }
        public string Name { get; set; }
        public TimeSpan NotifyBefore { get; set; }
        public INotifier Notifier { get; set; }
        public static EventStatus Status { get; set; }

        public CustomEvent(INotifier notifier, IEnumerable<DateTime> dates)
        {
            Notifier = notifier;
            Status = EventStatus.Active;
            Dates = dates.ToDictionary((current) => current, (current) => EventStatus.Active);
        }


        public void Notify()
        {
            Notifier.Notify();
        }

        public void UpdateStatus(NotificationResult result)
        {
            foreach (KeyValuePair<DateTime, EventStatus> element in Dates)
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
}