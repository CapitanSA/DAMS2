using DAMS.EventReminder.Notifier;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAMS.EventReminder.Event
{
    class PeriodEvent : IEvent
    {
        public DateTime Date { get; set; }
        public PeriodType PeriodType { get; set; }
        public DateTime NextNotificationDate { get { return GetNextNotificationDate(); } }
        public string Name { get; set; }
        public TimeSpan NotifyBefore { get; set; }
        public INotifier Notifier { get; set; }
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

        private DateTime GetNextNotificationDate()
        {
            DateTime NextDate = Date;
            if (PeriodType == PeriodType.None)
            {
                NextDate = Date - NotifyBefore; ;
            }
            if (PeriodType == PeriodType.Daily)
            {
                if (NextDate - NotifyBefore > DateTime.Now)
                {
                    NextDate = NextDate - NotifyBefore;
                }
                else
                {
                    while (NextDate - NotifyBefore < DateTime.Now)
                    {
                        NextDate = NextDate.AddDays(1);
                    }
                    NextDate = NextDate - NotifyBefore;
                }
            }
            if (PeriodType == PeriodType.Weekly)
            {
                if (NextDate - NotifyBefore > DateTime.Now)
                {
                    NextDate = NextDate - NotifyBefore;
                }
                else
                {
                    while (NextDate - NotifyBefore < DateTime.Now)
                    {
                        NextDate = NextDate.AddDays(7);
                    }
                    NextDate = NextDate - NotifyBefore;
                }
            }
            if (PeriodType == PeriodType.Monthly)
            {
                if (NextDate - NotifyBefore > DateTime.Now)
                {
                    NextDate = NextDate - NotifyBefore;
                }
                else
                {
                    while (NextDate - NotifyBefore < DateTime.Now)
                    {
                        NextDate = NextDate.AddMonths(1);
                    }
                    NextDate = NextDate - NotifyBefore;
                }
            }
            if (PeriodType == PeriodType.Yearly)
            {
                if (NextDate - NotifyBefore > DateTime.Now)
                {
                    NextDate = NextDate - NotifyBefore;
                }
                else
                {
                    while (NextDate - NotifyBefore < DateTime.Now)
                    {
                        NextDate = NextDate.AddYears(1);
                    }
                    NextDate = NextDate - NotifyBefore;
                }
            }
            return NextDate;
        }
    }
}
