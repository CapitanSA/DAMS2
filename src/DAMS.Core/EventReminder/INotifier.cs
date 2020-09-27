using DAMS.EventReminder.Notifier;
using System.Collections;

namespace DAMS.EventReminder
{
    public interface INotifier
    {
        NotificationResult Notify(NotificationInfo eventInfo);
     
    }
}