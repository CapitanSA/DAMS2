using Abp.Extensions;
using Castle.Components.DictionaryAdapter;
using DAMS.EventReminder.Event;
using DAMS.NotificationSystems.All.Telegram;
using DAMS.NotificationSystems.All.Email;
using System;
using System.Collections.Generic;
using Terminal.Gui;

namespace DAMS.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.Init();
            var top = Application.Top;
            var window = new Window("DAMS Console")
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            var name_event = new Label(2, 2, "Name Event");
            var name = new TextField(14, 2, 30, "");
            var date_event = new Label(2, 4, "Date event");
            var date = new TextField(14, 4, 30, "");
            var view = new Label(2, 7, "View Event :");
            var check_box1 = new CheckBox(14, 7, "One time Event");
            var check_box2 = new CheckBox(14, 8, "Custom Event");
            var check_box3 = new CheckBox(14, 9, "Period Event");
            var notifier = new Label(3, 11, "Notifier :");
            var notifier_telegram = new CheckBox(14, 11, "Telegram");
            var notifier_email = new CheckBox(14, 12, "Email");
            var buton = new Button(2, 20, "Create");
            var result = new Label(50, 0, "-------------You Events--------------------------");
            var format_date = new Label(14, 5, "(day.month.year)");

            List<string> list = new List<string>();
            buton.Clicked = () =>
            {
                if (check_box1.Checked & notifier_telegram.Checked)
                {
                    TelegramNotifier telegramNotifier = new TelegramNotifier();
                    var date_event = Convert.ToDateTime(date.Text);//date picker!!
                    OneTimeEvent oneTimeEvent = new OneTimeEvent(telegramNotifier, date_event);
                    oneTimeEvent.Name = name.Text.ToString();
                    list.Add(oneTimeEvent.Name + "  " + oneTimeEvent.Date + " Status " + oneTimeEvent.Status);

                }
                if (check_box1.Checked & notifier_email.Checked)
                {
                    EmailNotifier emailNotifier = new EmailNotifier();
                    var date_event = Convert.ToDateTime(date.Text);
                    OneTimeEvent oneTimeEvent = new OneTimeEvent(emailNotifier, date_event);
                    oneTimeEvent.Name = name.Text.ToString();
                    list.Add(oneTimeEvent.Name + "  " + oneTimeEvent.Date + " Status " + oneTimeEvent.Status);

                }

            };
            var container = new ListView(new Rect(50, 1, 60, 50), list);
            window.Add(name_event, name,date_event, date, view,check_box1, check_box2, check_box3,  buton, container,
              result,notifier, notifier_telegram, notifier_email, format_date
              );
            top.Add(window);
            Application.Run();
        }

    }
}
