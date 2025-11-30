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
            ScrBarDashboard.Scroll += ScrBarDashboard_Scroll;
            flpDashboard.MouseWheel += FlpDashboard_MouseWheel;
            flpDashboard.Scroll += FlpDashboard_Scroll;
        }

        private void ScrBarDashboard_Scroll(object sender, ScrollEventArgs e)
        {
            flpDashboard.HorizontalScroll.Value = Math.Min(e.NewValue, flpDashboard.HorizontalScroll.Maximum);
        }

        private void FlpDashboard_MouseWheel(object sender, MouseEventArgs e)
        {
            if (flpDashboard.HorizontalScroll.Enabled)
            {
                int currentValue = flpDashboard.HorizontalScroll.Value;
                int delta = e.Delta > 0 ? -50 : 50; 
                int newValue = Math.Max(flpDashboard.HorizontalScroll.Minimum, 
                               Math.Min(currentValue + delta, flpDashboard.HorizontalScroll.Maximum));
                
                flpDashboard.HorizontalScroll.Value = newValue;
                flpDashboard.PerformLayout();
                ScrBarDashboard.Value = Math.Min(newValue, ScrBarDashboard.Maximum);
            }
        }

        private void FlpDashboard_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                ScrBarDashboard.Value = Math.Min(e.NewValue, ScrBarDashboard.Maximum);
            }
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void DashboardItem_Load(object sender, EventArgs e)
        {
            UpdateScrollBar();
        }

        private void UpdateScrollBar()
        {
            int totalWidth = 0;
            foreach (Control ctrl in flpDashboard.Controls)
            {
                totalWidth += ctrl.Width + ctrl.Margin.Left + ctrl.Margin.Right;
            }

            int visibleWidth = flpDashboard.Width;
            ScrBarDashboard.Maximum = Math.Max(0, totalWidth - visibleWidth + flpDashboard.Padding.Right);
            ScrBarDashboard.LargeChange = visibleWidth;
            ScrBarDashboard.SmallChange = 50;
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

        private void flpDashboard_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}