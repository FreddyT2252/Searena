namespace SEARENA2025
{
    partial class DestinasiCard
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.shadowPanel = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.lblAktivitas = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblWaktuTerbaik = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblRatingReview = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblCuaca = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblDeskripsi = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblLokasi = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblNama = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.shadowPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // shadowPanel
            // 
            this.shadowPanel.BackColor = System.Drawing.Color.Transparent;
            this.shadowPanel.Controls.Add(this.lblAktivitas);
            this.shadowPanel.Controls.Add(this.lblWaktuTerbaik);
            this.shadowPanel.Controls.Add(this.lblRatingReview);
            this.shadowPanel.Controls.Add(this.lblCuaca);
            this.shadowPanel.Controls.Add(this.lblDeskripsi);
            this.shadowPanel.Controls.Add(this.lblLokasi);
            this.shadowPanel.Controls.Add(this.lblNama);
            this.shadowPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.shadowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shadowPanel.FillColor = System.Drawing.Color.White;
            this.shadowPanel.Location = new System.Drawing.Point(0, 0);
            this.shadowPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.shadowPanel.Name = "shadowPanel";
            this.shadowPanel.Radius = 8;
            this.shadowPanel.ShadowColor = System.Drawing.Color.Black;
            this.shadowPanel.ShadowDepth = 60;
            this.shadowPanel.Size = new System.Drawing.Size(450, 277);
            this.shadowPanel.TabIndex = 0;
            this.shadowPanel.Click += new System.EventHandler(this.Control_Click);
            this.shadowPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.shadowPanel_Paint);
            // 
            // lblAktivitas
            // 
            this.lblAktivitas.BackColor = System.Drawing.Color.Transparent;
            this.lblAktivitas.Font = new System.Drawing.Font("Segoe UI", 7.5F);
            this.lblAktivitas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblAktivitas.Location = new System.Drawing.Point(26, 238);
            this.lblAktivitas.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblAktivitas.Name = "lblAktivitas";
            this.lblAktivitas.Size = new System.Drawing.Size(123, 22);
            this.lblAktivitas.TabIndex = 7;
            this.lblAktivitas.Text = "Snorkeling, Diving";
            this.lblAktivitas.Click += new System.EventHandler(this.Control_Click);
            // 
            // lblWaktuTerbaik
            // 
            this.lblWaktuTerbaik.BackColor = System.Drawing.Color.Transparent;
            this.lblWaktuTerbaik.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblWaktuTerbaik.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblWaktuTerbaik.Location = new System.Drawing.Point(26, 210);
            this.lblWaktuTerbaik.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblWaktuTerbaik.Name = "lblWaktuTerbaik";
            this.lblWaktuTerbaik.Size = new System.Drawing.Size(199, 23);
            this.lblWaktuTerbaik.TabIndex = 6;
            this.lblWaktuTerbaik.Text = "Terbaik: Oktober, November";
            this.lblWaktuTerbaik.Click += new System.EventHandler(this.Control_Click);
            // 
            // lblRatingReview
            // 
            this.lblRatingReview.BackColor = System.Drawing.Color.Transparent;
            this.lblRatingReview.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblRatingReview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblRatingReview.Location = new System.Drawing.Point(26, 135);
            this.lblRatingReview.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblRatingReview.Name = "lblRatingReview";
            this.lblRatingReview.Size = new System.Drawing.Size(164, 23);
            this.lblRatingReview.TabIndex = 5;
            this.lblRatingReview.Text = "Rating: 4.5 | 123 ulasan";
            this.lblRatingReview.Click += new System.EventHandler(this.Control_Click);
            // 
            // lblCuaca
            // 
            this.lblCuaca.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(238)))), ((int)(((byte)(144)))));
            this.lblCuaca.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblCuaca.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(0)))));
            this.lblCuaca.Location = new System.Drawing.Point(26, 163);
            this.lblCuaca.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblCuaca.Name = "lblCuaca";
            this.lblCuaca.Padding = new System.Windows.Forms.Padding(12, 5, 12, 5);
            this.lblCuaca.Size = new System.Drawing.Size(117, 33);
            this.lblCuaca.TabIndex = 4;
            this.lblCuaca.Text = "Sangat Baik";
            this.lblCuaca.TextAlignment = System.Drawing.ContentAlignment.TopCenter;
            this.lblCuaca.Click += new System.EventHandler(this.Control_Click);
            // 
            // lblDeskripsi
            // 
            this.lblDeskripsi.BackColor = System.Drawing.Color.Transparent;
            this.lblDeskripsi.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblDeskripsi.ForeColor = System.Drawing.Color.Gray;
            this.lblDeskripsi.Location = new System.Drawing.Point(22, 82);
            this.lblDeskripsi.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblDeskripsi.MaximumSize = new System.Drawing.Size(405, 77);
            this.lblDeskripsi.Name = "lblDeskripsi";
            this.lblDeskripsi.Size = new System.Drawing.Size(405, 44);
            this.lblDeskripsi.TabIndex = 3;
            this.lblDeskripsi.Text = "Surga bawah laut yang memiliki keindahan alam bawah laut terbaik di dunia";
            this.lblDeskripsi.Click += new System.EventHandler(this.Control_Click);
            // 
            // lblLokasi
            // 
            this.lblLokasi.BackColor = System.Drawing.Color.Transparent;
            this.lblLokasi.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblLokasi.ForeColor = System.Drawing.Color.Black;
            this.lblLokasi.Location = new System.Drawing.Point(24, 57);
            this.lblLokasi.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblLokasi.Name = "lblLokasi";
            this.lblLokasi.Size = new System.Drawing.Size(226, 23);
            this.lblLokasi.TabIndex = 1;
            this.lblLokasi.Text = "Waisai, Raja Ampat, Papua Barat";
            this.lblLokasi.Click += new System.EventHandler(this.Control_Click);
            // 
            // lblNama
            // 
            this.lblNama.BackColor = System.Drawing.Color.Transparent;
            this.lblNama.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblNama.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(50)))), ((int)(((byte)(65)))));
            this.lblNama.Location = new System.Drawing.Point(22, 15);
            this.lblNama.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblNama.Name = "lblNama";
            this.lblNama.Size = new System.Drawing.Size(281, 34);
            this.lblNama.TabIndex = 0;
            this.lblNama.Text = "Raja Ampat Marine Park";
            this.lblNama.Click += new System.EventHandler(this.Control_Click);
            // 
            // DestinasiCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.shadowPanel);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "DestinasiCard";
            this.Size = new System.Drawing.Size(450, 277);
            this.Click += new System.EventHandler(this.Control_Click);
            this.shadowPanel.ResumeLayout(false);
            this.shadowPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2ShadowPanel shadowPanel;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblNama;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblLokasi;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblDeskripsi;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblRatingReview;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblCuaca;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblWaktuTerbaik;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblAktivitas;
    }
}
