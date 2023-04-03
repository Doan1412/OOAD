using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ReminderAPI
{
    public interface Reminder
    {
        DateTime getDateTime();
        Action ShowNoti  { get; set; }
    }
    public static class Reminders
    {
        private static System.Timers.Timer timer;
        public static Action action;
        public static List<Reminder> list;
        private static bool check = false;
        public static void checkTime()
        {
            if (list == null) return;
            DateTime currentDateTime = DateTime.Now;
            foreach (var reminder in list)
            {
                if (reminder.getDateTime() <= currentDateTime && check == false)
                {
                    reminder.ShowNoti.Invoke();
                    check = true;
                }
     
            }
        }
        static Reminders()
        {
            timer = new System.Timers.Timer(100);
            timer.Elapsed += (sender, e) => checkTime();
            //timer.AutoReset = false; 
            timer.Start();
        }
    }
}
