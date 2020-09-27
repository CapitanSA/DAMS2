using System;
using DAMS.EventReminder;
using DAMS.EventReminder.Event;
using DAMS.EventReminder.Notifier;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAMS.Core.Tests.EventReminder.Event
{
    public class PeriodEventTests
    {
        /// <summary>
        /// System under test
        /// </summary>
        public PeriodEvent PeriodEventInstance { get; set; }

        private INotifier _notifier;
        private DateTime _eventDate;

        [SetUp]
        public void Setup()
        {
            _notifier = Substitute.For<INotifier>();   // Here is mock
            _eventDate = DateTime.Now;

            PeriodEventInstance = new PeriodEvent(_notifier, _eventDate, PeriodType.Weekly);
        }

        #region Notify()

        [Test]
        public void Notify_should_call_INotifier()
        {
            // Arrange
            NotificationInfo notificationInfo = new NotificationInfo();
            // Act
            PeriodEventInstance.Notify();

            // Assert
            _notifier.Received().Notify(notificationInfo);  // Here is mock checking
        }

        #endregion

        #region UpdateStatus()

        //[Test]
        //public void UpdateStatus_should_set_status_success_if_notification_was_successful()
        //{
        //    // Arrange
        //    var result = new NotificationResult {IsSuccess = true};

        //    // Act
        //    PeriodEventInstance.UpdateStatus(result);

        //    // Assert
        //    PeriodEventInstance.Status.Should().Be(EventStatus.Closed);
        //}

        //[Test]
        //public void UpdateStatus_should_set_status_failed_if_notification_was_failed()
        //{
        //    // Arrange
        //    var result = new NotificationResult {IsSuccess = false};

        //    // Act
        //    PeriodEventInstance.UpdateStatus(result);

        //    // Assert
        //    PeriodEventInstance.Status.Should().Be(EventStatus.Failed);
        //}

        // Parameterized test
        [TestCase(true, EventStatus.Closed)]
        [TestCase(false, EventStatus.Failed)]
        public void UpdateStatus_should_update_status_after_notification(bool isSuccessful, EventStatus resultStatus)
        {
            // Arrange
            var result = new NotificationResult { IsSuccess = isSuccessful };

            // Act
            PeriodEventInstance.UpdateStatus(result);

            // Assert
            PeriodEventInstance.Status.Should().Be(resultStatus);
        }

        #endregion
    }
}