using System;
using System.Collections.Generic;
using DAMS.EventReminder;
using NUnit.Framework;
using SchedulerNS = DAMS.EventReminder.Scheduler;
using DAMS.EventReminder.Scheduler;
using DAMS.EventReminder.Event;
using FluentAssertions;
using NSubstitute;

namespace DAMS.Core.Tests.EventReminder.Scheduler
{
    public class SchedulerTest
    {
        /// <summary>
        /// System under test
        /// </summary>
        public SchedulerNS.Scheduler Scheduler { get; set; }

        private INotifier _notifier;
        private DateTime _eventDate;

        [SetUp]
        public void Setup()
        {
            _notifier = Substitute.For<INotifier>();
            _eventDate = DateTime.Now;

            Scheduler = new SchedulerNS.Scheduler();
        }

        [Test]
        public void PrepareNotificationBucket_should_sort_all_events_and_add_needed_event_in_a_new_bucket()
        {
            // Arrange
            var eventTest = new OneTimeEvent( DateTime.Now.AddMinutes(3).AddSeconds(50)) { Name = "event1", NotifyBefore = new TimeSpan (0,0,0) };
            var events = new List<IEvent>()
            {
               eventTest,
               new OneTimeEvent ( DateTime.Now.AddMinutes(15).AddSeconds(50)) { Name = "event2", NotifyBefore = new TimeSpan (0,0,0) },
               new OneTimeEvent ( DateTime.Now.AddMinutes(3).AddSeconds(50)) { Name = "event3", NotifyBefore = new TimeSpan (0,0,0) },
            };

            //Act 
            NotificationBucket result = Scheduler.PrepareNotificationBucket(events);

            //Assert
            result.Should().NotBeNull();
            result.NextEvents.Should().HaveCount(2);
            result.NextEvents.Should().Contain(eventTest);
        }
    }
}
