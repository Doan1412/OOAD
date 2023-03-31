using Bus_Tier;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class Form1 : Form
    {
        List<Appointment>appointments = new List<Appointment>(); 
        BSAppointment bsa=new BSAppointment();
        public Form1()
        {
            InitializeComponent();
            listApp.DisplayMember = "Title";
            appointments=bsa.getListAppointmentByUserId(2);
            displayListbox();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void displayListbox()
        {
            listApp.Items.Clear();
            foreach(var item in appointments)
            {
                listApp.Items.Add(item);
            }
        }
    }
}
