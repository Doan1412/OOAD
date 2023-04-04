using Bus_Tier;
using Model;
using ReminderAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace UI_tier
{
    public partial class Calendar : Form
    {
        List<Appointment> appointments = new List<Appointment>();
        User user;
        List<User> usersCalendar = new List<User>();
        BSAppointment bsa = new BSAppointment();
        BSUser bsu = new BSUser();
        BSReminder bsr = new BSReminder();
        private List<Reminder> myReminder;
        public Calendar(User user)
        {
            InitializeComponent();
            listApp.DisplayMember = "Title";
            listUser.DisplayMember = "Name";
            this.user = user;
            usersCalendar = bsu.getUser().Where(s => s.Id != user.Id).ToList();
            listUser.DataSource = usersCalendar;

            lvAtt.View = View.Details;
            lvAtt.Columns.Add("Id");
            lvAtt.Columns.Add("Name");
            lvRemind.View = View.Details;
            lvRemind.Columns.Add("", lvRemind.Width);
            cbxRemind.DataSource = new List<string> { "Trước 5 phút", "Trước 30 phút", "Trước 1 tiếng", "Trước 1 ngày"};

            myReminder = bsr.getListReminderByUserId(user.Id, DateTime.Now);
            myReminder.OrderBy(x => x.remindTime);
            Reminders.SetListReminder(myReminder);
            checkOverlap();
        }
        private void checkOverlap()
        {
            List<Appointment> listAppNotAccYet = new List<Appointment>();
            listAppNotAccYet = bsa.getListAppointmentNotAcceptedYet(user.Id);
            foreach (Appointment appointment in listAppNotAccYet)
            {
                List<Appointment> listAppDate = new List<Appointment>();
                string date = appointment.StartTime.ToShortDateString();
                listAppDate = bsa.getListAppointmentByIdAndDateAndStatus(user.Id, DateTime.Parse(date),true);
                Appointment app = listAppDate.FirstOrDefault(a => (a.StartTime <= appointment.StartTime && a.EndTime >= appointment.StartTime) || (a.StartTime <= appointment.EndTime && a.EndTime >= appointment.EndTime));
                if (app != null)
                {
                    var res = MessageBox.Show("Bạn đã có lịch vào khoảng thời gian này, bạn có muốn thay thế lịch đã có", "Warning", MessageBoxButtons.YesNo);
                    if (res == DialogResult.Yes)
                    {
                        bsa.updateStatus(user.Id, appointment.Id, true);
                        bsa.deleteUserAppointment(user.Id, app.Id);
                    }
                    else
                    {
                        bsa.deleteUserAppointment(user.Id, appointment.Id);
                    }
                }
            }
        }
        private void displayListbox()
        {
            listApp.Items.Clear();
            foreach (var item in appointments)
            {
                listApp.Items.Add(item);
            }
        }
        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            dateTimePicker3.Value = monthCalendar1.SelectionRange.Start;
            if (listUser.SelectedIndex != -1 && monthCalendar1.SelectionRange.Start != DateTime.MinValue)
            {
                appointments.Clear();
                string date = monthCalendar1.SelectionRange.Start.ToShortDateString();
                appointments = bsa.getListAppointmentByIdAndDate(user.Id, DateTime.Parse(date));
                displayListbox();

            }
        }


        private void listApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            Appointment appointment = listApp.SelectedItem as Appointment;
            List<User> users = bsu.getListUserByAppointmentId(appointment.Id);
            textTitle.Text = appointment.Title;
            dateTimePicker3.Value = appointment.StartTime;
            dateTimePicker1.Text = appointment.StartTime.ToString("hh:mm:ss tt"); ;
            dateTimePicker2.Text = appointment.EndTime.ToString("hh:mm:ss tt");
            textLoca.Text = appointment.Location;
            lvAtt.Items.Clear();
            foreach (User user in users)
            {
                ListViewItem item = new ListViewItem(user.Id.ToString());
                item.SubItems.Add(user.Name);
                lvAtt.Items.Add(item);
            }
            //textBox4.Text = string.Join(Environment.NewLine, users.Select(u => u.Name));
            textBox5.Text = appointment.Description;
            List<MyReminder>myReminders= bsr.getListReminderByUserIdAndDate(user.Id, appointment.StartTime);
            //"Trước 5 phút", "Trước 30 phút", "Trước 1 tiếng", "Trước 1 ngày"
            lvRemind.Items.Clear();
            foreach (MyReminder reminder in myReminders)
            {
                if(reminder.appointment.Id == appointment.Id)
                {
                    if(-reminder.remindTime.Minute + appointment.StartTime.Minute == 5)
                    {
                        lvRemind.Items.Add(new ListViewItem("Trước 5 phút"));
                    }
                    else if(-reminder.remindTime.Minute + appointment.StartTime.Minute == 30)
                    {
                        lvRemind.Items.Add(new ListViewItem("Trước 30 phút"));
                    } 
                    else if (-reminder.remindTime.Hour + appointment.StartTime.Hour == 1)
                    {
                        lvRemind.Items.Add(new ListViewItem("Trước 1 tiếng"));
                    }
                    else if (appointment.StartTime.Day - reminder.remindTime.Day == 1)
                    {
                        lvRemind.Items.Add(new ListViewItem("Trước 1 ngày"));
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DateTime StartTime = new DateTime(dateTimePicker3.Value.Year, dateTimePicker3.Value.Month, dateTimePicker3.Value.Day, dateTimePicker1.Value.Hour, dateTimePicker1.Value.Minute, dateTimePicker1.Value.Second);
            DateTime EndTime = new DateTime(dateTimePicker3.Value.Year, dateTimePicker3.Value.Month, dateTimePicker3.Value.Day, dateTimePicker2.Value.Hour, dateTimePicker2.Value.Minute, dateTimePicker2.Value.Second);
            foreach (var appointment in appointments)
            {
                if (textTitle.Text == appointment.Title)
                {
                    appointment.StartTime = StartTime;
                    appointment.EndTime = EndTime;
                    appointment.Description = textBox5.Text;
                    appointment.Location = textLoca.Text;
                    appointment.HostId = user.Id;

                    bsa.updateAppointment(appointment);
                    appointments.Add(appointment);
                    return;
                }
            }
            Appointment app = appointments.FirstOrDefault(a => (a.StartTime <= StartTime && a.EndTime >= StartTime) || (a.StartTime <= EndTime && a.EndTime >= EndTime));
            if (app != null)
            {
                var res = MessageBox.Show("Bạn đã có lịch vào khoảng thời gian này, bạn có muốn thay thế lịch đã có","Warning",MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    bsa.deleteAppointment(app.Id);
                    Appointment appointment1 = new Appointment
                    {
                        Title = textTitle.Text,
                        StartTime = new DateTime(dateTimePicker3.Value.Year, dateTimePicker3.Value.Month, dateTimePicker3.Value.Day, dateTimePicker1.Value.Hour, dateTimePicker1.Value.Minute, dateTimePicker1.Value.Second),
                        EndTime = new DateTime(dateTimePicker3.Value.Year, dateTimePicker3.Value.Month, dateTimePicker3.Value.Day, dateTimePicker2.Value.Hour, dateTimePicker2.Value.Minute, dateTimePicker2.Value.Second),
                        Description = textBox5.Text,
                        Location = textLoca.Text,
                        HostId = user.Id,
                    };
                    List<int> ids = new List<int>();
                    foreach (ListViewItem item in lvAtt.Items)
                    {
                        ids.Add(Convert.ToInt32(item.SubItems[0].Text));
                    }
                    ids.Add(user.Id);
                    bsa.addAppointment(appointment1, ids);
                    appointments.Remove(app);
                    appointments.Add(appointment1);
                    displayListbox();
                }
            }
            else
            {
                Appointment appointment1 = new Appointment
                {
                    Title = textTitle.Text,
                    StartTime = new DateTime(dateTimePicker3.Value.Year, dateTimePicker3.Value.Month, dateTimePicker3.Value.Day, dateTimePicker1.Value.Hour, dateTimePicker1.Value.Minute, dateTimePicker1.Value.Second),
                    EndTime = new DateTime(dateTimePicker3.Value.Year, dateTimePicker3.Value.Month, dateTimePicker3.Value.Day, dateTimePicker2.Value.Hour, dateTimePicker2.Value.Minute, dateTimePicker2.Value.Second),
                    Description = textBox5.Text,
                    Location = textLoca.Text,
                    HostId = user.Id,
                };
                List<int> ids = new List<int>();
                foreach (ListViewItem item in lvAtt.Items)
                {
                    ids.Add(Convert.ToInt32(item.SubItems[0].Text));
                }
                ids.Add(user.Id);
                bsa.addAppointment(appointment1, ids);
                appointments.Add(appointment1);
                displayListbox();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var user = listUser.SelectedItem as User;
            foreach (ListViewItem it in lvAtt.Items)
            {
                if (it.SubItems[0].Text == user.Id.ToString()) return;
            }
            ListViewItem item = new ListViewItem(user.Id.ToString());
            item.SubItems.Add(user.Name.ToString());
            lvAtt.Items.Add(item);
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            appointments.Clear();
            var date = dateTimePicker3.Value;
            appointments = bsa.getListAppointmentByIdAndDate(user.Id, date);
            displayListbox();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textTitle.Text = "";
            textLoca.Text = "";
            textBox5.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lvAtt.Items.Clear();
        }

        private void btnRemind_Click(object sender, EventArgs e)
        {
            Appointment appointment = listApp.SelectedItem as Appointment;
            foreach(ListViewItem it in lvRemind.Items)
            {
                if (it.SubItems[0].Text == cbxRemind.SelectedItem.ToString()) return;
            }    
            ListViewItem item = new ListViewItem(cbxRemind.SelectedItem.ToString());
            lvRemind.Items.Add(item);
            MyReminder reminder = new MyReminder();
            reminder.remindTime = DateTime.Now;
            reminder.appointment = appointment;
            switch (cbxRemind.SelectedIndex)
            {
                case 0:
                    {
                        reminder.remindTime = new DateTime(appointment.StartTime.Year, appointment.StartTime.Month, appointment.StartTime.Day, appointment.StartTime.Hour, appointment.StartTime.Minute - 5, 0);
                        break;
                    }
                case 1:
                    {
                        reminder.remindTime = new DateTime(appointment.StartTime.Year, appointment.StartTime.Month, appointment.StartTime.Day, appointment.StartTime.Hour, appointment.StartTime.Minute - 30, 0);
                        break;
                    }
                case 2:
                    {
                        reminder.remindTime = new DateTime(appointment.StartTime.Year, appointment.StartTime.Month, appointment.StartTime.Day, appointment.StartTime.Hour -1, appointment.StartTime.Minute, 0);
                        break;
                    }
                case 3:
                    {
                        reminder.remindTime = new DateTime(appointment.StartTime.Year, appointment.StartTime.Month, appointment.StartTime.Day-1, appointment.StartTime.Hour, appointment.StartTime.Minute, 0);
                        break;
                    }

            }
            reminder.userId = user.Id;
            bsr.addReminder(reminder);
        }

        private void lvRemind_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            lvRemind.Items.Remove(lvRemind.SelectedItems[0]);
        }
    }
}
