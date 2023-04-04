using ReminderAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Model
{
    public class MyReminder : Reminder
    {
        private static NotifyIcon NotifyIcon = new NotifyIcon();
        public DateTime remindTime { get; set; }
        public int userId { get; set; }
        public int reminderId { get; set; }
        public Appointment appointment { get; set; }
        public Action OnReminded { get; set; }
        static void func(){
            NotifyIcon.Visible = true;
            NotifyIcon.Icon = new System.Drawing.Icon(@"D:\App\Download\icons8-alarm-50.png");
}
        public MyReminder()
        {
            OnReminded += () => MessageBox.Show(appointment.StartTime.ToShortDateString() + " " + appointment.StartTime.ToShortTimeString(), appointment.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
