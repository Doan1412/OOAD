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
        List<User> users = new List<User>();
        BSUser bsu = new BSUser();
        public Form1()
        {
            InitializeComponent();
            users = bsu.getUser();
            cbxUser.DataSource = users;
            cbxUser.DisplayMember = "Name";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            User user = cbxUser.SelectedItem as User;
            Calendar f = new Calendar(user);
            f.ShowDialog();

        }
    }
}
