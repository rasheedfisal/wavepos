using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CafeteriaOrderingSystem.UI
{
    public partial class AdminUI : Form
    {
        public AdminUI()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            if (CustomMsgBoxUI.Show("هل انت متأكد من الخروج", "رسالة تأكيد", "نعم", "لا") == DialogResult.Yes)
            {
                Application.Exit();
            }
            
        }

        private void label6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void label7_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MCategoryUI mcU = new MCategoryUI();
            mcU.TopLevel = false;
            mcU.Dock = DockStyle.Fill;
            panel3.Controls.Add(mcU);
            mcU.BringToFront();
            mcU.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MProductUI mpU = new MProductUI();
            mpU.TopLevel = false;
            mpU.Dock = DockStyle.Fill;
            panel3.Controls.Add(mpU);
            mpU.BringToFront();
            mpU.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            LoginUI admin = new LoginUI();
            this.Hide();
            admin.ShowDialog();
            

        }

        private void AdminUI_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            SalesUI mcU = new SalesUI();
            mcU.TopLevel = false;
            mcU.Dock = DockStyle.Fill;
            panel3.Controls.Add(mcU);
            mcU.BringToFront();
            mcU.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MUsersUI mcU = new MUsersUI();
            mcU.TopLevel = false;
            mcU.Dock = DockStyle.Fill;
            panel3.Controls.Add(mcU);
            mcU.BringToFront();
            mcU.Show();
        }
    }
}
