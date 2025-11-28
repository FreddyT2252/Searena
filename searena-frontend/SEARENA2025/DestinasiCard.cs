using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace SEARENA2025
{
    public partial class DestinasiCard : UserControl
    {
        public int DestinasiId { get; set; }
        public string NamaDestinasi { get; set; }
        public string Lokasi { get; set; }
        public string Pulau { get; set; }
        public double Rating { get; set; }
        public string Deskripsi { get; set; }
        public string WaktuTerbaik { get; set; }

        public event EventHandler CardClicked;

        public DestinasiCard()
        {
            InitializeComponent();
            SetupClickEvents();
        }

        public DestinasiCard(Destinasi destinasi)
        {
            InitializeComponent();
            SetupClickEvents();
            LoadData(destinasi);
        }

        private void SetupClickEvents()
        {
            // Set cursor untuk seluruh control
            this.Cursor = Cursors.Hand;
            
            // Tambahkan event click ke semua child controls
            foreach (Control control in this.Controls)
            {
                AttachClickEventRecursive(control);
            }
        }

        private void AttachClickEventRecursive(Control control)
        {
            control.Cursor = Cursors.Hand;
            control.Click += Control_Click;
            
            foreach (Control child in control.Controls)
            {
                AttachClickEventRecursive(child);
            }
        }

        public void LoadData(Destinasi destinasi)
        {
            if (destinasi == null) return;

            DestinasiId = destinasi.Id;
            NamaDestinasi = destinasi.NamaDestinasi ?? "";
            Lokasi = destinasi.Lokasi ?? "";
            Pulau = destinasi.Pulau ?? "";
            Rating = destinasi.RatingAvg;
            Deskripsi = destinasi.Deskripsi ?? "";
            WaktuTerbaik = destinasi.WaktuTerbaik ?? "";

            UpdateUI();
        }

        private void UpdateUI()
        {
            try
            {
                // Update label nama destinasi
                if (lblNama != null)
                {
                    lblNama.Text = NamaDestinasi;
                    lblNama.Cursor = Cursors.Hand;
                }

                // Update label lokasi dengan icon
                if (lblLokasi != null)
                {
                    lblLokasi.Text = Lokasi;
                    lblLokasi.Cursor = Cursors.Hand;
                }

                // Update icon lokasi (gunakan unicode character atau image)
                if (iconLokasi != null)
                {
                    iconLokasi.Cursor = Cursors.Hand;
                    // Bisa set image untuk icon lokasi jika ada
                }

                // Update deskripsi
                if (lblDeskripsi != null)
                {
                    // Batasi panjang deskripsi
                    string shortDesc = Deskripsi;
                    if (shortDesc.Length > 70)
                    {
                        shortDesc = shortDesc.Substring(0, 67) + "...";
                    }
                    lblDeskripsi.Text = shortDesc;
                    lblDeskripsi.Cursor = Cursors.Hand;
                }

                // Update rating badge
                if (lblRating != null)
                {
                    string ratingText = GetRatingText(Rating);
                    lblRating.Text = ratingText;
                    lblRating.Cursor = Cursors.Hand;
                    
                    // Set warna background berdasarkan rating
                    if (Rating >= 4.5)
                    {
                        lblRating.BackColor = Color.FromArgb(144, 238, 144); // Light green
                        lblRating.ForeColor = Color.FromArgb(0, 100, 0); // Dark green
                    }
                    else if (Rating >= 4.0)
                    {
                        lblRating.BackColor = Color.FromArgb(173, 216, 230); // Light blue
                        lblRating.ForeColor = Color.FromArgb(0, 0, 139); // Dark blue
                    }
                    else if (Rating >= 3.5)
                    {
                        lblRating.BackColor = Color.FromArgb(255, 255, 224); // Light yellow
                        lblRating.ForeColor = Color.FromArgb(184, 134, 11); // Dark goldenrod
                    }
                    else
                    {
                        lblRating.BackColor = Color.FromArgb(255, 218, 185); // Peach
                        lblRating.ForeColor = Color.FromArgb(139, 69, 19); // Saddle brown
                    }
                }

                // Update waktu terbaik
                if (lblWaktuTerbaik != null)
                {
                    lblWaktuTerbaik.Text = "Terbaik: " + WaktuTerbaik;
                    lblWaktuTerbaik.Cursor = Cursors.Hand;
                }

                // Update icon waktu
                if (iconWaktu != null)
                {
                    iconWaktu.Cursor = Cursors.Hand;
                    // Bisa set image untuk icon waktu jika ada
                }

                // Set shadow panel cursor
                if (shadowPanel != null)
                {
                    shadowPanel.Cursor = Cursors.Hand;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating UI: {ex.Message}", "Error");
            }
        }

        // Ini masih salah, harusnya get api weather buat dapetin (sangat baik, potensi, dan buruk), tapi masih diisi rating
        private string GetRatingText(double rating)
        {
            if (rating >= 4.5) return "Sangat Baik";
            if (rating >= 4.0) return "Baik";
            if (rating >= 3.5) return "Cukup Baik";
            if (rating >= 3.0) return "Lumayan";
            return "Biasa";
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            RaiseCardClicked();
        }

        // Forward click dari child controls
        private void Control_Click(object sender, EventArgs e)
        {
            RaiseCardClicked();
        }

        private void RaiseCardClicked()
        {
            CardClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
