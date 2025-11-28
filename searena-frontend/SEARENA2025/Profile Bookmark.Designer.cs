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
            this.Navbar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Navbar.Name = "Navbar";
            this.Navbar.Size = new System.Drawing.Size(1203, 68);
            this.Navbar.TabIndex = 1;
            this.Navbar.Paint += new System.Windows.Forms.PaintEventHandler(this.Navbar_Paint);
            // 
            // lblProfile
            // 
            this.lblProfile.AutoSize = true;
            this.lblProfile.Font = new System.Drawing.Font("Malgun Gothic Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfile.Location = new System.Drawing.Point(1117, 19);
            this.lblProfile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProfile.Name = "lblProfile";
            this.lblProfile.Size = new System.Drawing.Size(66, 28);
            this.lblProfile.TabIndex = 7;
            this.lblProfile.Text = "Profile";
            this.lblProfile.Click += new System.EventHandler(this.lblProfile_Click_1);
            // 
            // Profile
            // 
            this.Profile.Image = ((System.Drawing.Image)(resources.GetObject("Profile.Image")));
            this.Profile.ImageRotate = 0F;
            this.Profile.Location = new System.Drawing.Point(1052, 9);
            this.Profile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Profile.Name = "Profile";
            this.Profile.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.Profile.Size = new System.Drawing.Size(57, 54);
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
            this.Logo.Location = new System.Drawing.Point(3, 5);
            this.Logo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Logo.Name = "Logo";
            this.Logo.Size = new System.Drawing.Size(189, 59);
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
            this.btnKembali.Location = new System.Drawing.Point(57, 101);
            this.btnKembali.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnKembali.Name = "btnKembali";
            this.btnKembali.Size = new System.Drawing.Size(116, 28);
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
            this.PnlProfile.Location = new System.Drawing.Point(57, 165);
            this.PnlProfile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PnlProfile.Name = "PnlProfile";
            this.PnlProfile.Size = new System.Drawing.Size(357, 158);
            this.PnlProfile.TabIndex = 7;
            // 
            // guna2PictureBox1
            // 
            this.guna2PictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("guna2PictureBox1.Image")));
            this.guna2PictureBox1.ImageRotate = 0F;
            this.guna2PictureBox1.Location = new System.Drawing.Point(156, 100);
            this.guna2PictureBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.guna2PictureBox1.Name = "guna2PictureBox1";
            this.guna2PictureBox1.Size = new System.Drawing.Size(27, 31);
            this.guna2PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.guna2PictureBox1.TabIndex = 2;
            this.guna2PictureBox1.TabStop = false;
            // 
            // lblBergabung
            // 
            this.lblBergabung.AutoSize = false;
            this.lblBergabung.BackColor = System.Drawing.Color.Transparent;
            this.lblBergabung.Font = new System.Drawing.Font("Malgun Gothic Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBergabung.Location = new System.Drawing.Point(188, 102);
            this.lblBergabung.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblBergabung.Name = "lblBergabung";
            this.lblBergabung.Size = new System.Drawing.Size(161, 49);
            this.lblBergabung.TabIndex = 2;
            this.lblBergabung.Text = "Bergabung sejak 2021";
            this.lblBergabung.Click += new System.EventHandler(this.lblBergabung_Click);
            // 
            // lblPengguna
            // 
            this.lblPengguna.BackColor = System.Drawing.Color.Transparent;
            this.lblPengguna.Font = new System.Drawing.Font("Malgun Gothic Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPengguna.Location = new System.Drawing.Point(156, 61);
            this.lblPengguna.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblPengguna.Name = "lblPengguna";
            this.lblPengguna.Size = new System.Drawing.Size(91, 30);
            this.lblPengguna.TabIndex = 2;
            this.lblPengguna.Text = "Pengguna";
            // 
            // guna2HtmlLabel1
            // 
            this.guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel1.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel1.Location = new System.Drawing.Point(156, 22);
            this.guna2HtmlLabel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            this.guna2HtmlLabel1.Size = new System.Drawing.Size(129, 30);
            this.guna2HtmlLabel1.TabIndex = 2;
            this.guna2HtmlLabel1.Text = "Tasya Aurora";
            // 
            // ProfilePage
            // 
            this.ProfilePage.Image = ((System.Drawing.Image)(resources.GetObject("ProfilePage.Image")));
            this.ProfilePage.ImageRotate = 0F;
            this.ProfilePage.Location = new System.Drawing.Point(6, 22);
            this.ProfilePage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ProfilePage.Name = "ProfilePage";
            this.ProfilePage.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.ProfilePage.Size = new System.Drawing.Size(125, 108);
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
            this.PanelProfile2.Location = new System.Drawing.Point(57, 362);
            this.PanelProfile2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PanelProfile2.Name = "PanelProfile2";
            this.PanelProfile2.Size = new System.Drawing.Size(357, 226);
            this.PanelProfile2.TabIndex = 8;
            // 
            // guna2HtmlLabel7
            // 
            this.guna2HtmlLabel7.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel7.Font = new System.Drawing.Font("Malgun Gothic", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel7.Location = new System.Drawing.Point(17, 126);
            this.guna2HtmlLabel7.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.guna2HtmlLabel7.Name = "guna2HtmlLabel7";
            this.guna2HtmlLabel7.Size = new System.Drawing.Size(54, 25);
            this.guna2HtmlLabel7.TabIndex = 3;
            this.guna2HtmlLabel7.Text = "E-mail";
            // 
            // guna2PictureBox2
            // 
            this.guna2PictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("guna2PictureBox2.Image")));
            this.guna2PictureBox2.ImageRotate = 0F;
            this.guna2PictureBox2.Location = new System.Drawing.Point(15, 115);
            this.guna2PictureBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.guna2PictureBox2.Name = "guna2PictureBox2";
            this.guna2PictureBox2.Size = new System.Drawing.Size(264, 15);
            this.guna2PictureBox2.TabIndex = 3;
            this.guna2PictureBox2.TabStop = false;
            // 
            // lblEmail
            // 
            this.lblEmail.BackColor = System.Drawing.Color.Transparent;
            this.lblEmail.Font = new System.Drawing.Font("Malgun Gothic Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.Location = new System.Drawing.Point(18, 156);
            this.lblEmail.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(187, 25);
            this.lblEmail.TabIndex = 4;
            this.lblEmail.Text = "tasyaaurora@gmail.com";
            // 
            // lblNama
            // 
            this.lblNama.BackColor = System.Drawing.Color.Transparent;
            this.lblNama.Font = new System.Drawing.Font("Malgun Gothic Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNama.Location = new System.Drawing.Point(15, 82);
            this.lblNama.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblNama.Name = "lblNama";
            this.lblNama.Size = new System.Drawing.Size(102, 25);
            this.lblNama.TabIndex = 3;
            this.lblNama.Text = "Tasya Aurora ";
            // 
            // guna2HtmlLabel6
            // 
            this.guna2HtmlLabel6.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel6.Font = new System.Drawing.Font("Malgun Gothic", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel6.Location = new System.Drawing.Point(14, 55);
            this.guna2HtmlLabel6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.guna2HtmlLabel6.Name = "guna2HtmlLabel6";
            this.guna2HtmlLabel6.Size = new System.Drawing.Size(125, 25);
            this.guna2HtmlLabel6.TabIndex = 3;
            this.guna2HtmlLabel6.Text = "Nama Lengkap";
            // 
            // guna2HtmlLabel5
            // 
            this.guna2HtmlLabel5.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel5.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel5.Location = new System.Drawing.Point(54, 11);
            this.guna2HtmlLabel5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.guna2HtmlLabel5.Name = "guna2HtmlLabel5";
            this.guna2HtmlLabel5.Size = new System.Drawing.Size(194, 30);
            this.guna2HtmlLabel5.TabIndex = 3;
            this.guna2HtmlLabel5.Text = "Informasi Pengguna";
            // 
            // guna2CirclePictureBox2
            // 
            this.guna2CirclePictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("guna2CirclePictureBox2.Image")));
            this.guna2CirclePictureBox2.ImageRotate = 0F;
            this.guna2CirclePictureBox2.Location = new System.Drawing.Point(11, 10);
            this.guna2CirclePictureBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.guna2CirclePictureBox2.Name = "guna2CirclePictureBox2";
            this.guna2CirclePictureBox2.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.guna2CirclePictureBox2.Size = new System.Drawing.Size(39, 41);
            this.guna2CirclePictureBox2.TabIndex = 3;
            this.guna2CirclePictureBox2.TabStop = false;
            // 
            // guna2PictureBox3
            // 
            this.guna2PictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("guna2PictureBox3.Image")));
            this.guna2PictureBox3.ImageRotate = 0F;
            this.guna2PictureBox3.Location = new System.Drawing.Point(18, 189);
            this.guna2PictureBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.guna2PictureBox3.Name = "guna2PictureBox3";
            this.guna2PictureBox3.Size = new System.Drawing.Size(264, 15);
            this.guna2PictureBox3.TabIndex = 6;
            this.guna2PictureBox3.TabStop = false;
            // 
            // btnLogOut
            // 
            this.btnLogOut.BackColor = System.Drawing.Color.Red;
            this.btnLogOut.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogOut.Location = new System.Drawing.Point(156, 608);
            this.btnLogOut.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnLogOut.Name = "btnLogOut";
            this.btnLogOut.Size = new System.Drawing.Size(123, 59);
            this.btnLogOut.TabIndex = 9;
            this.btnLogOut.Text = "Log Out";
            this.btnLogOut.UseVisualStyleBackColor = false;
            // 
            // guna2Panel3
            // 
            this.guna2Panel3.BorderRadius = 12;
            this.guna2Panel3.Controls.Add(this.btnBookmark);
            this.guna2Panel3.Controls.Add(this.btnRatingReview);
            this.guna2Panel3.FillColor = System.Drawing.Color.FloralWhite;
            this.guna2Panel3.Location = new System.Drawing.Point(512, 94);
            this.guna2Panel3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.guna2Panel3.Name = "guna2Panel3";
            this.guna2Panel3.Size = new System.Drawing.Size(566, 48);
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
            this.btnBookmark.Location = new System.Drawing.Point(291, 5);
            this.btnBookmark.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnBookmark.Name = "btnBookmark";
            this.btnBookmark.Size = new System.Drawing.Size(270, 39);
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
            this.btnRatingReview.Location = new System.Drawing.Point(4, 5);
            this.btnRatingReview.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRatingReview.Name = "btnRatingReview";
            this.btnRatingReview.Size = new System.Drawing.Size(270, 39);
            this.btnRatingReview.TabIndex = 0;
            this.btnRatingReview.Text = "Rating dan Review";
            this.btnRatingReview.Click += new System.EventHandler(this.btnRatingReview_Click_1);
            // 
            // guna2HtmlLabel2
            // 
            this.guna2HtmlLabel2.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel2.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel2.Location = new System.Drawing.Point(512, 158);
            this.guna2HtmlLabel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.guna2HtmlLabel2.Name = "guna2HtmlLabel2";
            this.guna2HtmlLabel2.Size = new System.Drawing.Size(256, 34);
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
            this.btnUnbookmark.Location = new System.Drawing.Point(975, 151);
            this.btnUnbookmark.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnUnbookmark.Name = "btnUnbookmark";
            this.btnUnbookmark.Size = new System.Drawing.Size(156, 35);
            this.btnUnbookmark.TabIndex = 18;
            this.btnUnbookmark.Text = "Unbookmark";
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PapayaWhip;
            this.ClientSize = new System.Drawing.Size(1200, 692);
            this.Controls.Add(this.btnUnbookmark);
            this.Controls.Add(this.guna2HtmlLabel2);
            this.Controls.Add(this.guna2Panel3);
            this.Controls.Add(this.btnLogOut);
            this.Controls.Add(this.PanelProfile2);
            this.Controls.Add(this.PnlProfile);
            this.Controls.Add(this.btnKembali);
            this.Controls.Add(this.Navbar);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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