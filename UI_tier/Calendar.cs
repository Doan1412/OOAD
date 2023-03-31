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
        List<User> users = new List<User>();
        BSAppointment bsa = new BSAppointment();
        BSUser bsu = new BSUser();
        public Calendar()
        {
            InitializeComponent();
            listApp.DisplayMember = "Title";
            listUser.DisplayMember = "Name";
            users = bsu.getUser();
            displayComboBox();
        }
        private void displayComboBox()
        {
            listUser.Items.Clear();
            foreach (var user in users)
            {
                listUser.Items.Add(user);
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
            if (listUser.SelectedIndex != -1 && monthCalendar1.SelectionRange.Start != DateTime.MinValue) {
                User user = listUser.SelectedItem as User;
                appointments.Clear();
                string date = monthCalendar1.SelectionRange.Start.ToShortDateString();
                appointments=bsa.getListAppointmentByIdAndDate(user.Id,DateTime.Parse(date));
                displayListbox();

            }
        }

        private void listUser_SelectedIndexChanged(object sender, EventArgs e)
        {
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
            textBox4.Text = string.Join(Environment.NewLine, users.Select(u => u.Name));
            textBox5.Text = appointment.Description;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime StartTime = new DateTime(dateTimePicker3.Value.Year, dateTimePicker3.Value.Month, dateTimePicker3.Value.Day, dateTimePicker1.Value.Hour, dateTimePicker1.Value.Minute, dateTimePicker1.Value.Second);
            DateTime EndTime = new DateTime(dateTimePicker3.Value.Year, dateTimePicker3.Value.Month, dateTimePicker3.Value.Day, dateTimePicker2.Value.Hour, dateTimePicker2.Value.Minute, dateTimePicker2.Value.Second);
            if (appointments.Any(a => (a.StartTime <= StartTime && a.EndTime >= StartTime) || (a.StartTime <= EndTime && a.EndTime >= EndTime)))
            {
                        // Appointment mới bị trùng, thực hiện xử lý tại đây
                        // POP up lỗi trùng
            }
            else
            {
                foreach(var appointment in appointments)
                {
                    if(textTitle.Text == appointment.Title)
                    {
                        appointment.StartTime = StartTime;
                        appointment.EndTime = EndTime;
                        appointment.Description = textBox5.Text;
                        int appId = bsa.addAppointment(appointment);
                        string text = textBox4.Text;
                        string[] names = text.Split('\n');
                        foreach(string name in names)
                        {
                            foreach (User user in users)
                            {
                                if(user.Name == name)
                                {
                                    bsu.addUserToAppointment(user.Id, appId);
                                }
                            }
                        } 
                        return;
                    }
                }
                Appointment appointment1 = new Appointment();
                appointment1.Title = textTitle.Text;
                appointment1.StartTime = new DateTime(dateTimePicker3.Value.Year, dateTimePicker3.Value.Month, dateTimePicker3.Value.Day, dateTimePicker1.Value.Hour, dateTimePicker1.Value.Minute, dateTimePicker1.Value.Second);
                appointment1.EndTime = new DateTime(dateTimePicker3.Value.Year, dateTimePicker3.Value.Month, dateTimePicker3.Value.Day, dateTimePicker2.Value.Hour, dateTimePicker2.Value.Minute, dateTimePicker2.Value.Second);
                appointment1.Description = textBox5.Text;
                int app1Id = bsa.addAppointment(appointment1);
                string text1 = textBox4.Text;
                string[] names1 = text1.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string name in names1)
                {
                    foreach (User user in users)
                    {
                        if (user.Name == name)
                        {
                            bsu.addUserToAppointment(user.Id, app1Id);
                            break;
                        }
                    }
                }
                appointments.Add(appointment1);
                displayListbox();
                return;
            }
        }
    }
}
