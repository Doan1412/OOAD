using ReminderAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class MyReminder : Reminder
    {
        public DateTime remindTime { get; set; }
        public int userId { get; set; }
        public int reminderId { get; set; }
        public Appointment appointment { get; set; }
        public Action OnReminded { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public DateTime getDateTime()
        {
            return this.remindTime;
        }
        public void setDateTime(DateTime remindTime)
        {
            this.remindTime = remindTime;
        }
    }
}
