using DAMS.EventReminder;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;
using DAMS.EventReminder.Notifier;
using FluentAssertions;
using DAMS.EventReminder.Event;

namespace DAMS.Core.Tests.EventReminder.Event
{
    class CustomEventTests
    {
        public CustomEvent CustomEventTest { get; set; }

        private INotifier notifier;
        private IEnumerable<DateTime> dates;
        private KeyValuePair<DateTime, EventStatus> element = new KeyValuePair<DateTime, EventStatus>(new DateTime(2020, 10, 16), EventStatus.Active);

        [SetUp]
        public void Setup()
        {
            notifier = Substitute.For<INotifier>();
            dates = new List<DateTime>();
            CustomEventTest = new CustomEvent(notifier, dates);

        }


        [Test]
        public void Notify_should_call_INotifier()
        {   
            // Arrange

            // Act
            CustomEventTest.Notify();

            // Assert
            notifier.Received().Notify();
        }


        [Test]
        public void Updatestatus_should_set_status_success_if_notification_was_successfu()
        {
            //Arrange
            var result = new NotificationResult { IsSuccess = true };
            CustomEventTest.Dates = new Dictionary<DateTime, EventStatus>
            {
                {new DateTime(2020,10,16),EventStatus.Active}
            };

            //Act
            CustomEventTest.UpdateStatus(result);

            //Assert
            CustomEventTest.Dates[element.Key].Should().Be(EventStatus.Closed);
        }
    }
}
