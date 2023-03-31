using Bus_Tier;
using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI_tier
{
    public partial class Form1 : Form
    {
        List<Appointment> appointments = new List<Appointment>();
        List<User>users=new List<User>();
        BSAppointment bsa = new BSAppointment();
        BSUser bsu = new BSUser();
        public Form1()
        {
            InitializeComponent();
            listApp.DisplayMember = "Title";
            listUser.DisplayMember = "Name";
            users = bsu.getUser();
            displayComboBox();
        }
        private void listUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            User user = listUser.SelectedItem as User;
            appointments.Clear();
            appointments = bsa.getListAppointmentByUserId(user.Id);
            displayListbox();
        }
        private void listApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            Appointment appointment = listApp.SelectedItem as Appointment;
            List<User> users = bsu.getListUserByAppointmentId(appointment.Id);
            textTitle.Text = appointment.Title;
            textBox1.Text = appointment.StartTime.ToString("MM/dd/yyyy");
            textBox2.Text = appointment.StartTime.ToString("hh:mm:ss tt"); ;
            textBox3.Text = appointment.EndTime.ToString("hh:mm:ss tt");
            textBox4.Text = string.Join(Environment.NewLine, users.Select(u => u.Name));
            textBox5.Text = appointment.Description;
        }
        private void displayComboBox()
        {
            listUser.Items.Clear();
            foreach(var user in users)
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

    }
}
