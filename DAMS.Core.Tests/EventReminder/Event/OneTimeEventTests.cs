using System;
using System.Collections.Generic;
using System.Text;
using DAMS.EventReminder;
using DAMS.EventReminder.Event;
using DAMS.EventReminder.Notifier;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace DAMS.Core.Tests.EventReminder.Event
{
    class OneTimeEventTests
    {
        public OneTimeEvent OneTimeEventTest { get; set; }

        private INotifier notifier;
        private DateTime date;

        [SetUp]
        public void Setup()
        {
            notifier = Substitute.For<INotifier>();
            date = DateTime.Now;
            OneTimeEventTest = new OneTimeEvent(notifier, date);
        }


        [Test]
        public void Notify_should_call_INotifier()
        {
            // Arrange

            // Act
            OneTimeEventTest.Notify();

            // Assert
            notifier.Received().Notify();
        }


        [Test]
        public void Updatestatus_should_set_status_success_if_notification_was_successful()
        {
            // arrange
            var result = new NotificationResult { IsSuccess = true };

            // act
            OneTimeEventTest.UpdateStatus(result);

            // assert
            OneTimeEventTest.Status.Should().Be(EventStatus.Closed);
        }


        [Test]
        public void UpdateStatus_should_set_status_failed_if_notification_was_failed()
        {
            // arrange
            var result = new NotificationResult { IsSuccess = false };

            // act
            OneTimeEventTest.UpdateStatus(result);

            // assert
            OneTimeEventTest.Status.Should().Be(EventStatus.Failed);
        }
    }
}
