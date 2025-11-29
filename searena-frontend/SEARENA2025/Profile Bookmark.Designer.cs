namespace SEARENA2025
{
    partial class Form3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            this.Navbar = new Guna.UI2.WinForms.Guna2Panel();
            this.lblProfile = new System.Windows.Forms.Label();
            this.Profile = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            this.Logo = new Guna.UI2.WinForms.Guna2PictureBox();
            this.btnKembali = new Guna.UI2.WinForms.Guna2Button();
            this.PnlProfile = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.lblBergabung = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblPengguna = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.ProfilePage = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            this.PanelProfile2 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2HtmlLabel7 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2PictureBox2 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.lblEmail = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblNama = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel6 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel5 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2CirclePictureBox2 = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            this.guna2PictureBox3 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.btnLogOut = new System.Windows.Forms.Button();
            this.guna2Panel3 = new Guna.UI2.WinForms.Guna2Panel();
            this.btnBookmark = new Guna.UI2.WinForms.Guna2Button();
            this.btnRatingReview = new Guna.UI2.WinForms.Guna2Button();
            this.guna2HtmlLabel2 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.btnUnbookmark = new Guna.UI2.WinForms.Guna2Button();
            this.Navbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Profile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).BeginInit();
            this.PnlProfile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProfilePage)).BeginInit();
            this.PanelProfile2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2CirclePictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox3)).BeginInit();
            this.guna2Panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // Navbar
            // 
            this.Navbar.BackColor = System.Drawing.Color.FloralWhite;
            this.Navbar.Controls.Add(this.lblProfile);
            this.Navbar.Controls.Add(this.Profile);
            this.Navbar.Controls.Add(this.Logo);
            this.Navbar.Location = new System.Drawing.Point(-1, 0);
            this.Navbar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Navbar.Name = "Navbar";
            this.Navbar.Size = new System.Drawing.Size(1069, 54);
            this.Navbar.TabIndex = 1;
            this.Navbar.Paint += new System.Windows.Forms.PaintEventHandler(this.Navbar_Paint);
            // 
            // lblProfile
            // 
            this.lblProfile.AutoSize = true;
            this.lblProfile.Font = new System.Drawing.Font("Malgun Gothic Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfile.Location = new System.Drawing.Point(993, 15);
            this.lblProfile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProfile.Name = "lblProfile";
            this.lblProfile.Size = new System.Drawing.Size(58, 23);
            this.lblProfile.TabIndex = 7;
            this.lblProfile.Text = "Profile";
            this.lblProfile.Click += new System.EventHandler(this.lblProfile_Click_1);
            // 
            // Profile
            // 
            this.Profile.Image = ((System.Drawing.Image)(resources.GetObject("Profile.Image")));
            this.Profile.ImageRotate = 0F;
            this.Profile.Location = new System.Drawing.Point(935, 7);
            this.Profile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Profile.Name = "Profile";
            this.Profile.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.Profile.Size = new System.Drawing.Size(51, 43);
            this.Profile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Profile.TabIndex = 6;
            this.Profile.TabStop = false;
            this.Profile.Click += new System.EventHandler(this.Profile_Click_1);
            // 
            // Logo
            // 
            this.Logo.BackColor = System.Drawing.Color.FloralWhite;
            this.Logo.Image = ((System.Drawing.Image)(resources.GetObject("Logo.Image")));
            this.Logo.ImageRotate = 0F;
            this.Logo.Location = new System.Drawing.Point(3, 4);
            this.Logo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Logo.Name = "Logo";
            this.Logo.Size = new System.Drawing.Size(168, 47);
            this.Logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Logo.TabIndex = 1;
            this.Logo.TabStop = false;
            this.Logo.Click += new System.EventHandler(this.Logo_Click);
            // 
            // btnKembali
            // 
            this.btnKembali.BackColor = System.Drawing.Color.FloralWhite;
            this.btnKembali.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnKembali.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnKembali.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnKembali.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnKembali.FillColor = System.Drawing.Color.FloralWhite;
            this.btnKembali.Font = new System.Drawing.Font("Malgun Gothic Semilight", 8.25F);
            this.btnKembali.ForeColor = System.Drawing.Color.Black;
            this.btnKembali.Image = ((System.Drawing.Image)(resources.GetObject("btnKembali.Image")));
            this.btnKembali.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnKembali.Location = new System.Drawing.Point(51, 81);
            this.btnKembali.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnKembali.Name = "btnKembali";
            this.btnKembali.Size = new System.Drawing.Size(103, 22);
            this.btnKembali.TabIndex = 6;
            this.btnKembali.Text = "Kembali";
            this.btnKembali.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnKembali.Click += new System.EventHandler(this.btnKembali_Click_1);
            // 
            // PnlProfile
            // 
            this.PnlProfile.BackColor = System.Drawing.Color.FloralWhite;
            this.PnlProfile.Controls.Add(this.guna2PictureBox1);
            this.PnlProfile.Controls.Add(this.lblBergabung);
            this.PnlProfile.Controls.Add(this.lblPengguna);
            this.PnlProfile.Controls.Add(this.guna2HtmlLabel1);
            this.PnlProfile.Controls.Add(this.ProfilePage);
            this.PnlProfile.Location = new System.Drawing.Point(51, 132);
            this.PnlProfile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PnlProfile.Name = "PnlProfile";
            this.PnlProfile.Size = new System.Drawing.Size(317, 126);
            this.PnlProfile.TabIndex = 7;
            // 
            // guna2PictureBox1
            // 
            this.guna2PictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("guna2PictureBox1.Image")));
            this.guna2PictureBox1.ImageRotate = 0F;
            this.guna2PictureBox1.Location = new System.Drawing.Point(139, 80);
            this.guna2PictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.guna2PictureBox1.Name = "guna2PictureBox1";
            this.guna2PictureBox1.Size = new System.Drawing.Size(24, 25);
            this.guna2PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.guna2PictureBox1.TabIndex = 2;
            this.guna2PictureBox1.TabStop = false;
            // 
            // lblBergabung
            // 
            this.lblBergabung.AutoSize = false;
            this.lblBergabung.BackColor = System.Drawing.Color.Transparent;
            this.lblBergabung.Font = new System.Drawing.Font("Malgun Gothic Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBergabung.Location = new System.Drawing.Point(167, 82);
            this.lblBergabung.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblBergabung.Name = "lblBergabung";
            this.lblBergabung.Size = new System.Drawing.Size(143, 39);
            this.lblBergabung.TabIndex = 2;
            this.lblBergabung.Text = "Bergabung sejak 2021";
            this.lblBergabung.Click += new System.EventHandler(this.lblBergabung_Click);
            // 
            // lblPengguna
            // 
            this.lblPengguna.BackColor = System.Drawing.Color.Transparent;
            this.lblPengguna.Font = new System.Drawing.Font("Malgun Gothic Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPengguna.Location = new System.Drawing.Point(139, 49);
            this.lblPengguna.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblPengguna.Name = "lblPengguna";
            this.lblPengguna.Size = new System.Drawing.Size(73, 23);
            this.lblPengguna.TabIndex = 2;
            this.lblPengguna.Text = "Pengguna";
            // 
            // guna2HtmlLabel1
            // 
            this.guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel1.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel1.Location = new System.Drawing.Point(139, 18);
            this.guna2HtmlLabel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            this.guna2HtmlLabel1.Size = new System.Drawing.Size(104, 23);
            this.guna2HtmlLabel1.TabIndex = 2;
            this.guna2HtmlLabel1.Text = "Tasya Aurora";
            // 
            // ProfilePage
            // 
            this.ProfilePage.Image = ((System.Drawing.Image)(resources.GetObject("ProfilePage.Image")));
            this.ProfilePage.ImageRotate = 0F;
            this.ProfilePage.Location = new System.Drawing.Point(5, 18);
            this.ProfilePage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ProfilePage.Name = "ProfilePage";
            this.ProfilePage.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.ProfilePage.Size = new System.Drawing.Size(111, 86);
            this.ProfilePage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ProfilePage.TabIndex = 2;
            this.ProfilePage.TabStop = false;
            // 
            // PanelProfile2
            // 
            this.PanelProfile2.BackColor = System.Drawing.Color.FloralWhite;
            this.PanelProfile2.Controls.Add(this.guna2HtmlLabel7);
            this.PanelProfile2.Controls.Add(this.guna2PictureBox2);
            this.PanelProfile2.Controls.Add(this.lblEmail);
            this.PanelProfile2.Controls.Add(this.lblNama);
            this.PanelProfile2.Controls.Add(this.guna2HtmlLabel6);
            this.PanelProfile2.Controls.Add(this.guna2HtmlLabel5);
            this.PanelProfile2.Controls.Add(this.guna2CirclePictureBox2);
            this.PanelProfile2.Controls.Add(this.guna2PictureBox3);
            this.PanelProfile2.Location = new System.Drawing.Point(51, 290);
            this.PanelProfile2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PanelProfile2.Name = "PanelProfile2";
            this.PanelProfile2.Size = new System.Drawing.Size(317, 181);
            this.PanelProfile2.TabIndex = 8;
            // 
            // guna2HtmlLabel7
            // 
            this.guna2HtmlLabel7.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel7.Font = new System.Drawing.Font("Malgun Gothic", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel7.Location = new System.Drawing.Point(15, 101);
            this.guna2HtmlLabel7.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.guna2HtmlLabel7.Name = "guna2HtmlLabel7";
            this.guna2HtmlLabel7.Size = new System.Drawing.Size(46, 21);
            this.guna2HtmlLabel7.TabIndex = 3;
            this.guna2HtmlLabel7.Text = "E-mail";
            // 
            // guna2PictureBox2
            // 
            this.guna2PictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("guna2PictureBox2.Image")));
            this.guna2PictureBox2.ImageRotate = 0F;
            this.guna2PictureBox2.Location = new System.Drawing.Point(13, 92);
            this.guna2PictureBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.guna2PictureBox2.Name = "guna2PictureBox2";
            this.guna2PictureBox2.Size = new System.Drawing.Size(235, 12);
            this.guna2PictureBox2.TabIndex = 3;
            this.guna2PictureBox2.TabStop = false;
            // 
            // lblEmail
            // 
            this.lblEmail.BackColor = System.Drawing.Color.Transparent;
            this.lblEmail.Font = new System.Drawing.Font("Malgun Gothic Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.Location = new System.Drawing.Point(16, 125);
            this.lblEmail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(150, 21);
            this.lblEmail.TabIndex = 4;
            this.lblEmail.Text = "tasyaaurora@gmail.com";
            // 
            // lblNama
            // 
            this.lblNama.BackColor = System.Drawing.Color.Transparent;
            this.lblNama.Font = new System.Drawing.Font("Malgun Gothic Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNama.Location = new System.Drawing.Point(13, 66);
            this.lblNama.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblNama.Name = "lblNama";
            this.lblNama.Size = new System.Drawing.Size(83, 21);
            this.lblNama.TabIndex = 3;
            this.lblNama.Text = "Tasya Aurora ";
            // 
            // guna2HtmlLabel6
            // 
            this.guna2HtmlLabel6.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel6.Font = new System.Drawing.Font("Malgun Gothic", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel6.Location = new System.Drawing.Point(12, 44);
            this.guna2HtmlLabel6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.guna2HtmlLabel6.Name = "guna2HtmlLabel6";
            this.guna2HtmlLabel6.Size = new System.Drawing.Size(106, 21);
            this.guna2HtmlLabel6.TabIndex = 3;
            this.guna2HtmlLabel6.Text = "Nama Lengkap";
            // 
            // guna2HtmlLabel5
            // 
            this.guna2HtmlLabel5.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel5.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel5.Location = new System.Drawing.Point(48, 9);
            this.guna2HtmlLabel5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.guna2HtmlLabel5.Name = "guna2HtmlLabel5";
            this.guna2HtmlLabel5.Size = new System.Drawing.Size(159, 23);
            this.guna2HtmlLabel5.TabIndex = 3;
            this.guna2HtmlLabel5.Text = "Informasi Pengguna";
            // 
            // guna2CirclePictureBox2
            // 
            this.guna2CirclePictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("guna2CirclePictureBox2.Image")));
            this.guna2CirclePictureBox2.ImageRotate = 0F;
            this.guna2CirclePictureBox2.Location = new System.Drawing.Point(10, 8);
            this.guna2CirclePictureBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.guna2CirclePictureBox2.Name = "guna2CirclePictureBox2";
            this.guna2CirclePictureBox2.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.guna2CirclePictureBox2.Size = new System.Drawing.Size(35, 33);
            this.guna2CirclePictureBox2.TabIndex = 3;
            this.guna2CirclePictureBox2.TabStop = false;
            // 
            // guna2PictureBox3
            // 
            this.guna2PictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("guna2PictureBox3.Image")));
            this.guna2PictureBox3.ImageRotate = 0F;
            this.guna2PictureBox3.Location = new System.Drawing.Point(16, 151);
            this.guna2PictureBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.guna2PictureBox3.Name = "guna2PictureBox3";
            this.guna2PictureBox3.Size = new System.Drawing.Size(235, 12);
            this.guna2PictureBox3.TabIndex = 6;
            this.guna2PictureBox3.TabStop = false;
            // 
            // btnLogOut
            // 
            this.btnLogOut.BackColor = System.Drawing.Color.Red;
            this.btnLogOut.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogOut.Location = new System.Drawing.Point(139, 486);
            this.btnLogOut.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLogOut.Name = "btnLogOut";
            this.btnLogOut.Size = new System.Drawing.Size(109, 47);
            this.btnLogOut.TabIndex = 9;
            this.btnLogOut.Text = "Log Out";
            this.btnLogOut.UseVisualStyleBackColor = false;
            this.btnLogOut.Click += new System.EventHandler(this.btnLogOut_Click);
            // 
            // guna2Panel3
            // 
            this.guna2Panel3.BorderRadius = 12;
            this.guna2Panel3.Controls.Add(this.btnBookmark);
            this.guna2Panel3.Controls.Add(this.btnRatingReview);
            this.guna2Panel3.FillColor = System.Drawing.Color.FloralWhite;
            this.guna2Panel3.Location = new System.Drawing.Point(455, 75);
            this.guna2Panel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.guna2Panel3.Name = "guna2Panel3";
            this.guna2Panel3.Size = new System.Drawing.Size(503, 38);
            this.guna2Panel3.TabIndex = 10;
            // 
            // btnBookmark
            // 
            this.btnBookmark.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnBookmark.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnBookmark.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnBookmark.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnBookmark.FillColor = System.Drawing.Color.LightGreen;
            this.btnBookmark.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBookmark.ForeColor = System.Drawing.Color.Black;
            this.btnBookmark.Location = new System.Drawing.Point(259, 4);
            this.btnBookmark.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBookmark.Name = "btnBookmark";
            this.btnBookmark.Size = new System.Drawing.Size(240, 31);
            this.btnBookmark.TabIndex = 1;
            this.btnBookmark.Text = "Bookmark";
            // 
            // btnRatingReview
            // 
            this.btnRatingReview.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnRatingReview.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnRatingReview.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnRatingReview.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnRatingReview.FillColor = System.Drawing.Color.FloralWhite;
            this.btnRatingReview.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRatingReview.ForeColor = System.Drawing.Color.Black;
            this.btnRatingReview.Location = new System.Drawing.Point(4, 4);
            this.btnRatingReview.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRatingReview.Name = "btnRatingReview";
            this.btnRatingReview.Size = new System.Drawing.Size(240, 31);
            this.btnRatingReview.TabIndex = 0;
            this.btnRatingReview.Text = "Rating dan Review";
            this.btnRatingReview.Click += new System.EventHandler(this.btnRatingReview_Click_1);
            // 
            // guna2HtmlLabel2
            // 
            this.guna2HtmlLabel2.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel2.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel2.Location = new System.Drawing.Point(455, 126);
            this.guna2HtmlLabel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.guna2HtmlLabel2.Name = "guna2HtmlLabel2";
            this.guna2HtmlLabel2.Size = new System.Drawing.Size(214, 30);
            this.guna2HtmlLabel2.TabIndex = 11;
            this.guna2HtmlLabel2.Text = "Riwayat Bookmark (7)";
            // 
            // btnUnbookmark
            // 
            this.btnUnbookmark.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnUnbookmark.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnUnbookmark.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnUnbookmark.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnUnbookmark.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnUnbookmark.Font = new System.Drawing.Font("Malgun Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUnbookmark.ForeColor = System.Drawing.Color.White;
            this.btnUnbookmark.Location = new System.Drawing.Point(867, 121);
            this.btnUnbookmark.Name = "btnUnbookmark";
            this.btnUnbookmark.Size = new System.Drawing.Size(139, 28);
            this.btnUnbookmark.TabIndex = 18;
            this.btnUnbookmark.Text = "Unbookmark";
            this.btnUnbookmark.Click += new System.EventHandler(this.btnUnbookmark_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PapayaWhip;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.btnUnbookmark);
            this.Controls.Add(this.guna2HtmlLabel2);
            this.Controls.Add(this.guna2Panel3);
            this.Controls.Add(this.btnLogOut);
            this.Controls.Add(this.PanelProfile2);
            this.Controls.Add(this.PnlProfile);
            this.Controls.Add(this.btnKembali);
            this.Controls.Add(this.Navbar);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form3";
            this.Text = "Form3";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.Navbar.ResumeLayout(false);
            this.Navbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Profile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).EndInit();
            this.PnlProfile.ResumeLayout(false);
            this.PnlProfile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProfilePage)).EndInit();
            this.PanelProfile2.ResumeLayout(false);
            this.PanelProfile2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2CirclePictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox3)).EndInit();
            this.guna2Panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel Navbar;
        private System.Windows.Forms.Label lblProfile;
        private Guna.UI2.WinForms.Guna2CirclePictureBox Profile;
        private Guna.UI2.WinForms.Guna2PictureBox Logo;
        private Guna.UI2.WinForms.Guna2Button btnKembali;
        private Guna.UI2.WinForms.Guna2Panel PnlProfile;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblBergabung;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblPengguna;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
        private Guna.UI2.WinForms.Guna2CirclePictureBox ProfilePage;
        private Guna.UI2.WinForms.Guna2Panel PanelProfile2;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel7;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox2;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblEmail;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblNama;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel6;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel5;
        private Guna.UI2.WinForms.Guna2CirclePictureBox guna2CirclePictureBox2;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox3;
        private System.Windows.Forms.Button btnLogOut;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel3;
        private Guna.UI2.WinForms.Guna2Button btnBookmark;
        private Guna.UI2.WinForms.Guna2Button btnRatingReview;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel2;
        private Guna.UI2.WinForms.Guna2Button btnUnbookmark;
    }
}