using Abp.Extensions;
using Castle.Components.DictionaryAdapter;
using DAMS.EventReminder.Event;
using DAMS.NotificationSystems.All.Telegram;
using DAMS.NotificationSystems.All.Email;
using System;
using System.Collections.Generic;
using Terminal.Gui;
using DAMS.EntityFrameworkCore;
using DAMS.EventReminder.Event;
using Microsoft.EntityFrameworkCore;
using DAMS.EventReminder;
using System.Collections.Generic;
using DAMS.NotificationSystems.All.Telegram;
using Newtonsoft.Json.Linq;
using System.Drawing;
using Color = Terminal.Gui.Color;
using Attribute = Terminal.Gui.Attribute;
using Newtonsoft.Json;
using System.Xml.Linq;
using Abp.Json;
using System.Linq.Dynamic.Core;
using System.Linq;

namespace DAMS.ConsoleApp
{
    //TODO:зробити в інтерфейсі IEvent enum (None,TElegram,email)

    //TODO:кнопка нотифікації
    class Program
    {
        static void Main(string[] args)
        {
            Application.Init();
            var top = Application.Top;

            //база даних
            DAMSDbContextFactory create_context = new DAMSDbContextFactory();
            DAMSDbContext dams_db = create_context.CreateDbContext(null);

            // елементи основного вікна 
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
            var check_box_OnenTimeEvent = new CheckBox(14, 7, "One time Event");
            var check_box_PeriodEvent = new CheckBox(14, 8, "Period Event");
            var period = new Label(2, 11, "Period type :");
            var notifier = new Label(3, 17, "Notifier :");
            var notifier_telegram = new CheckBox(14, 17, "Telegram");
            var notifier_email = new CheckBox(14, 18, "Email");
            var button_create = new Button(2, 22, "Create");
            var format_date = new Label(14, 5, "(day.month.year)");
            var period_type = new RadioGroup(14, 11, new[] { "None", "Daily", "Weekly", "Monthly", "Yearly" });
            var button_create_custom_event = new Button(14, 9, "Create custom event");

            window.Add(button_create_custom_event, period, period_type, name_event, name, date_event, date, view, check_box_OnenTimeEvent, check_box_PeriodEvent, button_create, notifier, notifier_telegram, notifier_email, format_date);

            var window_custom_event = new Window("Custom Event")
            {
                X = Pos.Right(button_create_custom_event),
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            List<DateTime> custom_dates = new List<DateTime>();
            var my_custom_event_ListView = new ListView(new Rect(28, 1, 50, 20), custom_dates);
            var name_custom = new Label(1, 2, "Name Custom Event:");
            var name_custom_text = new TextField(1, 3, 20, "");
            var date_custom = new Label(1, 5, "Date:");
            var date_custom_text = new TextField(1, 6, 20, "");
            var format_custom_date = new Label(1, 7, "(day.month.year)");
            var button_add_date = new Button(1, 9, "Add date");
            var create_custom_event = new Button(1, 11, "Create event");
            var button_close_window_custom = new Button(1, 20, "close");
            my_custom_event_ListView.ColorScheme = Colors.Base;
            window_custom_event.ColorScheme = Colors.Dialog;

            window_custom_event.Add(button_close_window_custom, my_custom_event_ListView, name_custom, name_custom_text, date_custom, date_custom_text, format_custom_date, button_add_date, create_custom_event);

            button_create_custom_event.Clicked = () => window.Add(window_custom_event);
            button_close_window_custom.Clicked = () => window.Remove(window_custom_event);
            button_add_date.Clicked = () =>
            {
                var date_event = Convert.ToDateTime(date_custom_text.Text);
                custom_dates.Add(date_event);

            };
            create_custom_event.Clicked = () =>
            {
                if (notifier_telegram.Checked)
                {
                    TelegramNotifier telegramNotifier = new TelegramNotifier();
                    var date_event = Convert.ToDateTime(date_custom_text.Text);
                    CustomEvent customEvent = new CustomEvent(telegramNotifier, custom_dates);
                    customEvent.Name = name_custom_text.Text.ToString();
                    dams_db.CustomEvents.Add(customEvent);
                    dams_db.SaveChanges();

                };
                if (notifier_email.Checked)
                {
                    EmailNotifier emailNotifier = new EmailNotifier();
                    var date_event = Convert.ToDateTime(date_custom_text.Text);
                    CustomEvent customEvent = new CustomEvent(emailNotifier, custom_dates);
                    customEvent.Name = name_custom_text.Text.ToString();
                    dams_db.CustomEvents.Add(customEvent);
                    dams_db.SaveChanges();

                };
            };

            //елементи вікна OneTimeEvent
            var window_of_onetimeevents = new Window("One Time Event ")
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()

            };
            var db_list_onetime = dams_db.OneTimeEvents.ToList();
            var oneTimeEvent_ListView = new ListView(new Rect(1, 1, 100, 20), db_list_onetime);
            var button_delete_item = new Button(30, 22, "Delete item");
            var button_close = new Button(60, 22, "Close");
            var button_notification_one_time_event = new Button(1, 22, "Notification");

            oneTimeEvent_ListView.AllowsMarking = true;
            oneTimeEvent_ListView.AllowsMultipleSelection = true;
            Colors.Menu.Normal = Application.Driver.MakeAttribute(Color.White, Color.Blue);
            oneTimeEvent_ListView.ColorScheme = Colors.Dialog;

            button_close.Clicked = () => window.Remove(window_of_onetimeevents);
            button_delete_item.Clicked = () =>
            {
                for (int i = 0; i < dams_db.OneTimeEvents.Count(); i++)
                {
                    if (oneTimeEvent_ListView.Source.IsMarked(i) == true)
                    {
                        var element = db_list_onetime.ElementAt(i);
                        dams_db.RemoveRange(element);
                        dams_db.SaveChanges();
                        db_list_onetime.RemoveAt(i);
                    }
                }

            };
            button_notification_one_time_event.Clicked = () =>
            {
                for (int i = 0; i < dams_db.OneTimeEvents.Count(); i++)
                {
                    if (oneTimeEvent_ListView.Source.IsMarked(i) == true)
                    {
                        var element = db_list_onetime.ElementAt(i);
                        if (element.NotifyStatus!=0)
                        {
                            if (element.NotifyStatus==NotifyStatus.Telegram)
                            {
                                TelegramNotifier telegramNotifier = new TelegramNotifier();
                                element.Notification = telegramNotifier;
                                element.Notify();
                                dams_db.SaveChanges();
                            }
                            if (element.NotifyStatus==NotifyStatus.Email)
                            {
                                EmailNotifier emailNotifier = new EmailNotifier();
                                element.Notification = emailNotifier;
                                element.Notify();
                                dams_db.SaveChanges();
                            }
                        }
                      
                    }
                }

            };

            window_of_onetimeevents.Add(oneTimeEvent_ListView, button_close, button_delete_item, button_notification_one_time_event);

            //елементи вікна PeriodEvent
            var window_of_periodevent = new Window("Period Event")
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()

            };
            var db_list_periodevent = dams_db.PeriodEvents.ToList();
            var periodEvent_ListView = new ListView(new Rect(1, 1, 100, 20), db_list_periodevent);
            var button_delete_item_period_event = new Button(30, 22, "Delete item");
            var button_close_period_event = new Button(60, 22, "close");

            periodEvent_ListView.AllowsMarking = true;
            periodEvent_ListView.ColorScheme = Colors.Dialog;

            button_close_period_event.Clicked = () => window.Remove(window_of_periodevent);
            button_delete_item_period_event.Clicked = () =>
              {
                  for (int i = 0; i < dams_db.PeriodEvents.Count(); i++)
                  {
                      if (periodEvent_ListView.Source.IsMarked(i) == true)
                      {
                          var element = db_list_periodevent.ElementAt(i);
                          dams_db.RemoveRange(element);
                          dams_db.SaveChanges();
                          db_list_periodevent.RemoveAt(i);
                      }
                  }
              };

            window_of_periodevent.Add(periodEvent_ListView, button_delete_item_period_event, button_close_period_event);

            //елементи вікна Custom Event
            var window_of_cust = new Window("Custom Event")
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()

            };
            var db_list_custom = dams_db.CustomEvents.ToList();
            var customEvent_ListView = new ListView(new Rect(1, 1, 100, 20), db_list_custom);
            var button_delete_item_custom_event = new Button(30, 22, "Delete item");
            var button_close_custom_event = new Button(60, 22, "close");
            customEvent_ListView.AllowsMarking = true;
            customEvent_ListView.ColorScheme = Colors.Dialog;

            button_close_custom_event.Clicked = () => window.Remove(window_of_cust);
            button_delete_item_custom_event.Clicked = () =>
            {
                for (int i = 0; i < dams_db.CustomEvents.Count(); i++)
                {
                    if (customEvent_ListView.Source.IsMarked(i) == true)
                    {
                        var element = db_list_custom.ElementAt(i);
                        dams_db.RemoveRange(element);
                        dams_db.SaveChanges();
                        db_list_custom.RemoveAt(i);
                    }
                }
            };

            window_of_cust.Add(customEvent_ListView, button_close_custom_event, button_delete_item_custom_event);

            // інтерфейс меню та логіка кнопки "Create"
            button_create.Clicked = () =>
            {
                if (check_box_OnenTimeEvent.Checked & notifier_telegram.Checked)
                {
                    var date_event = Convert.ToDateTime(date.Text);
                    OneTimeEvent oneTimeEvent = new OneTimeEvent(date_event);
                    oneTimeEvent.Name = name.Text.ToString();
                    oneTimeEvent.NotifyStatus = NotifyStatus.Telegram;
                    dams_db.OneTimeEvents.Add(oneTimeEvent);
                    dams_db.SaveChanges();

                }
                if (check_box_OnenTimeEvent.Checked & notifier_email.Checked)
                {
                    var date_event = Convert.ToDateTime(date.Text);
                    OneTimeEvent oneTimeEvent = new OneTimeEvent(date_event);
                    oneTimeEvent.Name = name.Text.ToString();
                    dams_db.OneTimeEvents.Add(oneTimeEvent);
                    dams_db.SaveChanges();

                }
                if (check_box_PeriodEvent.Checked & notifier_telegram.Checked)
                {
                    TelegramNotifier telegramNotifier = new TelegramNotifier();
                    var date_event = Convert.ToDateTime(date.Text);
                    PeriodType period;

                    period = (period_type.Selected == 1) ? (PeriodType.Daily) :
                    (period_type.Selected == 2) ? (PeriodType.Weekly) :
                    (period_type.Selected == 3) ? (PeriodType.Monthly) :
                    (period_type.Selected == 4) ? (PeriodType.Yearly) : (PeriodType.None);
                    PeriodEvent periodEvent = new PeriodEvent(telegramNotifier, date_event, period);
                    periodEvent.Name = name.Text.ToString();
                    dams_db.PeriodEvents.Add(periodEvent);
                    dams_db.SaveChanges();

                }
                if (check_box_PeriodEvent.Checked & notifier_email.Checked)
                {
                    EmailNotifier emailNotifier = new EmailNotifier();
                    var date_event = Convert.ToDateTime(date.Text);
                    PeriodType period;
                    period = (period_type.Selected == 1) ? (PeriodType.Daily) :
                         (period_type.Selected == 2) ? (PeriodType.Weekly) :
                         (period_type.Selected == 3) ? (PeriodType.Monthly) :
                         (period_type.Selected == 4) ? (PeriodType.Yearly) : (PeriodType.None);
                    PeriodEvent periodEvent = new PeriodEvent(emailNotifier, date_event, period);
                    periodEvent.Name = name.Text.ToString();
                    dams_db.PeriodEvents.Add(periodEvent);
                    dams_db.SaveChanges();

                }


            };

            var menu = new MenuBar(new MenuBarItem[] {
                    new MenuBarItem("My Events", new MenuItem[] {
                        new MenuItem("One Time Events", "", () =>  window.Add(window_of_onetimeevents)),
                        new MenuItem("Period Event","",()=> window.Add(window_of_periodevent)),
                        new MenuItem("Custom Event","",()=>window.Add(window_of_cust))
                }) });
            top.Add(menu);
            top.Add(window);
            Application.Run();
        }

    }
}
