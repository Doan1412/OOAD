using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ReminderAPI
{
    public interface Reminder
    {
        DateTime getDateTime();
        Action OnReminded  { get; set; }
    }
    public static class Reminders
    {
        private static System.Timers.Timer timer;
        private static List<Reminder> _list;
        public static void SetListReminder(List<Reminder> list)
        {
            _list = list;
        }
        public static void checkTime()
        {
            if (_list == null) return;
            DateTime currentDateTime = DateTime.Now;
            var reminder = _list[0];
            if (reminder.getDateTime() <= currentDateTime)
            {
                reminder.OnReminded.Invoke();
                _list.RemoveAt(0);
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
