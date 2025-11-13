using System.Windows.Forms;

namespace SEARENA2025
{
    partial class DashboardItem
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DashboardItem));
            this.Navbar = new Guna.UI2.WinForms.Guna2Panel();
            this.btnSignIn = new Guna.UI2.WinForms.Guna2Button();
            this.Logo = new Guna.UI2.WinForms.Guna2PictureBox();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2CirclePictureBox1 = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            this.ScrBarDashboard = new Guna.UI2.WinForms.Guna2HScrollBar();
            this.guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.flpDashboard = new System.Windows.Forms.FlowLayoutPanel();
            this.guna2PictureBox4 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.Navbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).BeginInit();
            this.guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2CirclePictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).BeginInit();
            this.flpDashboard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // Navbar
            // 
            this.Navbar.BackColor = System.Drawing.Color.FloralWhite;
            this.Navbar.Controls.Add(this.btnSignIn);
            this.Navbar.Controls.Add(this.Logo);
            this.Navbar.Location = new System.Drawing.Point(-2, 0);
            this.Navbar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Navbar.Name = "Navbar";
            this.Navbar.Size = new System.Drawing.Size(1203, 68);
            this.Navbar.TabIndex = 2;
            this.Navbar.Paint += new System.Windows.Forms.PaintEventHandler(this.Navbar_Paint);
            // 
            // btnSignIn
            // 
            this.btnSignIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnSignIn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSignIn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSignIn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSignIn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSignIn.FillColor = System.Drawing.Color.Coral;
            this.btnSignIn.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSignIn.ForeColor = System.Drawing.Color.Black;
            this.btnSignIn.Location = new System.Drawing.Point(984, 9);
            this.btnSignIn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSignIn.Name = "btnSignIn";
            this.btnSignIn.Size = new System.Drawing.Size(183, 49);
            this.btnSignIn.TabIndex = 6;
            this.btnSignIn.Text = "Sign In";
            this.btnSignIn.Click += new System.EventHandler(this.btnSignIn_Click);
            // 
            // Logo
            // 
            this.Logo.BackColor = System.Drawing.Color.FloralWhite;
            this.Logo.Image = ((System.Drawing.Image)(resources.GetObject("Logo.Image")));
            this.Logo.ImageRotate = 0F;
            this.Logo.Location = new System.Drawing.Point(6, 5);
            this.Logo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Logo.Name = "Logo";
            this.Logo.Size = new System.Drawing.Size(222, 58);
            this.Logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Logo.TabIndex = 1;
            this.Logo.TabStop = false;
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.guna2Panel1.Controls.Add(this.guna2HtmlLabel1);
            this.guna2Panel1.Controls.Add(this.guna2CirclePictureBox1);
            this.guna2Panel1.Location = new System.Drawing.Point(5, 77);
            this.guna2Panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(909, 56);
            this.guna2Panel1.TabIndex = 3;
            // 
            // guna2HtmlLabel1
            // 
            this.guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel1.Font = new System.Drawing.Font("Malgun Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel1.Location = new System.Drawing.Point(56, 12);
            this.guna2HtmlLabel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            this.guna2HtmlLabel1.Size = new System.Drawing.Size(836, 27);
            this.guna2HtmlLabel1.TabIndex = 1;
            this.guna2HtmlLabel1.Text = "Login terlebih dahulu untuk mengakses fitur lengkap: rekomendasi cuaca, filter de" +
    "stinasi, dan notifikasi";
            this.guna2HtmlLabel1.Click += new System.EventHandler(this.guna2HtmlLabel1_Click);
            // 
            // guna2CirclePictureBox1
            // 
            this.guna2CirclePictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("guna2CirclePictureBox1.Image")));
            this.guna2CirclePictureBox1.ImageRotate = 0F;
            this.guna2CirclePictureBox1.Location = new System.Drawing.Point(12, 2);
            this.guna2CirclePictureBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.guna2CirclePictureBox1.Name = "guna2CirclePictureBox1";
            this.guna2CirclePictureBox1.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.guna2CirclePictureBox1.Size = new System.Drawing.Size(40, 42);
            this.guna2CirclePictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.guna2CirclePictureBox1.TabIndex = 0;
            this.guna2CirclePictureBox1.TabStop = false;
            // 
            // ScrBarDashboard
            // 
            this.ScrBarDashboard.BackColor = System.Drawing.Color.FloralWhite;
            this.ScrBarDashboard.FillColor = System.Drawing.Color.FloralWhite;
            this.ScrBarDashboard.InUpdate = false;
            this.ScrBarDashboard.LargeChange = 10;
            this.ScrBarDashboard.Location = new System.Drawing.Point(392, 655);
            this.ScrBarDashboard.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ScrBarDashboard.Name = "ScrBarDashboard";
            this.ScrBarDashboard.ScrollbarSize = 28;
            this.ScrBarDashboard.Size = new System.Drawing.Size(450, 28);
            this.ScrBarDashboard.TabIndex = 5;
            this.ScrBarDashboard.ThumbColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            // 
            // guna2PictureBox1
            // 
            this.guna2PictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("guna2PictureBox1.Image")));
            this.guna2PictureBox1.ImageRotate = 0F;
            this.guna2PictureBox1.Location = new System.Drawing.Point(4, 5);
            this.guna2PictureBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.guna2PictureBox1.Name = "guna2PictureBox1";
            this.guna2PictureBox1.Size = new System.Drawing.Size(796, 485);
            this.guna2PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.guna2PictureBox1.TabIndex = 6;
            this.guna2PictureBox1.TabStop = false;
            this.guna2PictureBox1.Click += new System.EventHandler(this.guna2PictureBox1_Click);
            // 
            // flpDashboard
            // 
            this.flpDashboard.AutoScroll = true;
            this.flpDashboard.Controls.Add(this.guna2PictureBox1);
            this.flpDashboard.Controls.Add(this.guna2PictureBox4);
            this.flpDashboard.Location = new System.Drawing.Point(1, 147);
            this.flpDashboard.Name = "flpDashboard";
            this.flpDashboard.Size = new System.Drawing.Size(1615, 498);
            this.flpDashboard.TabIndex = 9;
            // 
            // guna2PictureBox4
            // 
            this.guna2PictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("guna2PictureBox4.Image")));
            this.guna2PictureBox4.ImageRotate = 0F;
            this.guna2PictureBox4.Location = new System.Drawing.Point(808, 5);
            this.guna2PictureBox4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.guna2PictureBox4.Name = "guna2PictureBox4";
            this.guna2PictureBox4.Size = new System.Drawing.Size(796, 485);
            this.guna2PictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.guna2PictureBox4.TabIndex = 7;
            this.guna2PictureBox4.TabStop = false;
            // 
            // DashboardItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PapayaWhip;
            this.ClientSize = new System.Drawing.Size(1200, 701);
            this.Controls.Add(this.flpDashboard);
            this.Controls.Add(this.ScrBarDashboard);
            this.Controls.Add(this.guna2Panel1);
            this.Controls.Add(this.Navbar);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "DashboardItem";
            this.Text = "Form4";
            this.Load += new System.EventHandler(this.DashboardItem_Load);
            this.Navbar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).EndInit();
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2CirclePictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).EndInit();
            this.flpDashboard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel Navbar;
        private Guna.UI2.WinForms.Guna2PictureBox Logo;
        private Guna.UI2.WinForms.Guna2Button btnSignIn;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
        private Guna.UI2.WinForms.Guna2CirclePictureBox guna2CirclePictureBox1;
        private Guna.UI2.WinForms.Guna2HScrollBar ScrBarDashboard;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
        private FlowLayoutPanel sliderPanel;
        private FlowLayoutPanel flpDashboard;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox4;
    }
}