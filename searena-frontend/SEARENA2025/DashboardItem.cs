using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SEARENA2025
{
    public partial class DashboardItem : Form
    {
        public DashboardItem()
        {
            InitializeComponent();
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void DashboardItem_Load(object sender, EventArgs e)
        {
        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            Form1 loginForm = new Form1();
            loginForm.Show();
            this.Hide();
        }

        private void Beranda_Click(object sender, EventArgs e)
        {
        }

        private void Navbar_Paint(object sender, PaintEventArgs e)
        {
        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }
    }
}