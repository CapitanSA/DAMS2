using System;
using Terminal.Gui;
using DAMS.EntityFrameworkCore;
using DAMS.EventReminder.Event;
using Microsoft.EntityFrameworkCore;
using DAMS.EventReminder;
using System.Collections.Generic;
using DAMS.NotificationSystems.All.Telegram;
using Newtonsoft.Json.Linq;

namespace DAMS.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            InitApp();
            Application.Run();
        }

        static void InitApp()
        {
            Application.Init();
            var top = Application.Top;

            DAMSDbContextFactory create_context = new DAMSDbContextFactory();
            DAMSDbContext dams_context = create_context.CreateDbContext(null);
            List<DateTime> dateTimes = new List<DateTime>()
            {
                new DateTime (2020,10,16),
                new DateTime (2020,10,17)
            };

            TelegramNotifier telegramNotifier = new TelegramNotifier();

            OneTimeEvent oneTime = new OneTimeEvent() {  Date = DateTime.Now, Name = "Тестовий", Status = EventStatus.Active, NotifyBefore = new TimeSpan(5) };
            CustomEvent customEvent = new CustomEvent(telegramNotifier,dateTimes){Name="куку"};
            dams_context.OneTimeEvents.Add(oneTime);
            dams_context.CustomEvents.Add(customEvent);
            dams_context.SaveChanges();

            var window = new Window("DAMS Console")
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            top.Add(window);
        }
    }
}
