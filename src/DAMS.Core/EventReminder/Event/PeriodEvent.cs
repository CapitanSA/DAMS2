using DAMS.EventReminder.Notifier;
using System;

namespace DAMS.EventReminder.Event
{
    // TODO: Refactor: extract logic responsible for calculation of the next event date to private method.
    //       Use this method wherever it could be used.

    public class PeriodEvent : IEvent
    {
        private INotifier Notifier;

        public DateTime Date { get; set; }
        public PeriodType PeriodType { get; set; }
        public DateTime NextNotificationDate { get { return GetNextNotificationDate(); } }
        public string Name { get; set; }
        public TimeSpan NotifyBefore { get; set; }
        public EventStatus Status { get; set; }


        public PeriodEvent(INotifier notifier, DateTime date, PeriodType period)
        {
            Notifier = notifier;
            Date = date;
            PeriodType = period;
            Name = "My Event";
            NotifyBefore = new TimeSpan(0, 5, 0);
            Status = EventStatus.Active;
        }

        public PeriodEvent(INotifier notifier, DateTime date, string name, TimeSpan time, EventStatus status)
        {
            Notifier = notifier;
            Date = date;
            Name = name;
            NotifyBefore = time;
            Status = status;
        }


        public void Notify()
        {
            var nextEventDate = GetNextEventDate();
            var eventInfo = new EventInfo(Name, nextEventDate);
            var notificationResult = Notifier.Notify(eventInfo);
            UpdateStatus(notificationResult);
        }

        public void UpdateStatus(NotificationResult result)
        {
            if (result.IsSuccess)
            {
                Status = EventStatus.Closed;
            }
            if (!result.IsSuccess)
            {
                Status = EventStatus.Failed;
            }
        }

        private DateTime GetNextNotificationDate()
        {
            DateTime nextNotificatioDate = GetNextEventDate() - NotifyBefore;
            return nextNotificatioDate;
        }

        private DateTime GetNextEventDate()
        {
            DateTime nextEventDate = Date;
            if (PeriodType == PeriodType.None)
            {
                return nextEventDate;
            }
            if (PeriodType == PeriodType.Daily)
            {
                if (nextEventDate > DateTime.Now)
                {
                    return nextEventDate;
                }
                else
                {
                    while (nextEventDate < DateTime.Now)
                    {
                        nextEventDate.AddDays(1);
                    }
                    return nextEventDate;
                }
            }
            if (PeriodType == PeriodType.Weekly)
            {
                if (nextEventDate > DateTime.Now)
                {
                    return nextEventDate;
                }
                else
                {
                    while (nextEventDate < DateTime.Now)
                    {
                        nextEventDate.AddDays(7);
                    }
                    return nextEventDate;
                }
            }
            if (PeriodType == PeriodType.Monthly)
            {
                if (nextEventDate > DateTime.Now)
                {
                    return nextEventDate;
                }
                else
                {
                    while (nextEventDate < DateTime.Now)
                    {
                        nextEventDate.AddMonths(1);
                    }
                    return nextEventDate;
                }
            }
            if (PeriodType == PeriodType.Yearly)
            {
                if (nextEventDate > DateTime.Now)
                {
                    return nextEventDate;
                }
                else
                {
                    while (nextEventDate < DateTime.Now)
                    {
                        nextEventDate.AddYears(1);
                    }
                    return nextEventDate;
                }
            }
            return nextEventDate;
        }
    }
}
