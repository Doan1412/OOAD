using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ReminderAPI
{
    public interface Reminder
    {
        DateTime remindTime { get; set; }
        Action OnReminded { get; set; }
    }
    public class Reminders
    {
        private static System.Timers.Timer timer;
        private static List<Reminder> _list = null;
        public static void SetListReminder(List<Reminder> list)
        {
            _list = list;
        }
        public static void checkTime()
        {
            if (_list == null || _list.Count == 0) return;
            DateTime currentDateTime = DateTime.Now;
            var reminder = _list.ElementAtOrDefault(0);
            if (reminder != null)
            {
                _list.RemoveAt(0);
                reminder.OnReminded.Invoke();
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
