using Bus_Tier;
using Model;
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
                User user = listUser.SelectedItem as User;
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
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DateTime StartTime = new DateTime(dateTimePicker3.Value.Year, dateTimePicker3.Value.Month, dateTimePicker3.Value.Day, dateTimePicker1.Value.Hour, dateTimePicker1.Value.Minute, dateTimePicker1.Value.Second);
            DateTime EndTime = new DateTime(dateTimePicker3.Value.Year, dateTimePicker3.Value.Month, dateTimePicker3.Value.Day, dateTimePicker2.Value.Hour, dateTimePicker2.Value.Minute, dateTimePicker2.Value.Second);
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
                    bsa.addAppointment(appointment1, ids);
                    appointments.Remove(app);
                    appointments.Add(appointment1);
                    displayListbox();
                }
            }
            else
            {
                foreach (var appointment in appointments)
                {
                    if (textTitle.Text == appointment.Title)
                    {
                        appointment.StartTime = StartTime;
                        appointment.EndTime = EndTime;
                        appointment.Description = textBox5.Text;

                        List<int> ids1 = new List<int>();
                        foreach (ListViewItem item in lvAtt.Items)
                        {
                            ids1.Add(Convert.ToInt32(item.SubItems[0].Text));
                        }
                        bsa.addAppointment(appointment, ids1);
                        appointments.Add(appointment);
                        return;
                    }
                }
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
            User user = listUser.SelectedItem as User;
            appointments.Clear();
            var date = dateTimePicker3.Value;
            appointments = bsa.getListAppointmentByIdAndDate(user.Id, date);
            displayListbox();
        }
    }
}
