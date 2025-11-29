using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;
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
        public int TotalReview { get; set; }
        public string Deskripsi { get; set; }
        public string WaktuTerbaik { get; set; }
        public string Activity { get; set; }

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
            this.Click += Control_Click;

            
           
        }

        public void LoadData(Destinasi destinasi)
        {
            if (destinasi == null) return;

            DestinasiId = destinasi.Id;
            NamaDestinasi = destinasi.NamaDestinasi ?? "";
            Lokasi = destinasi.Lokasi ?? "";
            Pulau = destinasi.Pulau ?? "";
            Rating = destinasi.RatingAvg;
            TotalReview = destinasi.TotalReview;
            Deskripsi = destinasi.Deskripsi ?? "";
            WaktuTerbaik = destinasi.WaktuTerbaik ?? "";
            Activity = destinasi.Activity ?? "";

            UpdateUI();
            LoadWeatherAsync(); 
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
                    lblNama.AutoSize = false;
                    lblNama.MaximumSize = new Size(400, 0);
                    lblNama.AutoSizeHeightOnly = true;
                }

                // Update label lokasi dengan pulau
                if (lblLokasi != null)
                {
                    lblLokasi.Text = $"{Lokasi}, {Pulau}";
                    lblLokasi.Cursor = Cursors.Hand;
                    lblLokasi.AutoSize = false;
                    lblLokasi.MaximumSize = new Size(400, 0);
                }

                // Update deskripsi
                if (lblDeskripsi != null)
                {
                    string shortDesc = Deskripsi;
                    if (shortDesc.Length > 70)
                    {
                        shortDesc = shortDesc.Substring(0, 67) + "...";
                    }
                    lblDeskripsi.Text = shortDesc;
                    lblDeskripsi.Cursor = Cursors.Hand;
                    lblDeskripsi.AutoSize = false;
                    lblDeskripsi.MaximumSize = new Size(400, 60); 
                }

                // Update rating dan review count 
                if (lblRatingReview != null)
                {
                    // Jika belum ada review
                    if (TotalReview == 0)
                    {
                        lblRatingReview.Text = "Belum ada rating dan review";
                    }
                    else
                    {
                        // Format: Rating: 4.5 | 123 ulasan
                        lblRatingReview.Text = $"Rating: {Rating:F1} | {TotalReview} ulasan";
                    }
                    lblRatingReview.Cursor = Cursors.Hand;
                    lblRatingReview.BackColor = Color.Transparent;
                    lblRatingReview.ForeColor = Color.FromArgb(80, 80, 80); 
                    lblRatingReview.AutoSize = false;
                    lblRatingReview.Size = new Size(300, 20);
                }

                // Update waktu terbaik
                if (lblWaktuTerbaik != null)
                {
                    // PERBAIKI: Batasi panjang waktu terbaik
                    string waktuText = "Terbaik: " + WaktuTerbaik;
                    if (waktuText.Length > 35)
                    {
                        waktuText = waktuText.Substring(0, 32) + "...";
                    }
                    lblWaktuTerbaik.Text = waktuText;
                    lblWaktuTerbaik.Cursor = Cursors.Hand;
                    lblWaktuTerbaik.AutoSize = false;
                    lblWaktuTerbaik.Size = new Size(400, 20);
                }

                // Update aktivitas
                if (lblAktivitas != null && !string.IsNullOrWhiteSpace(Activity))
                {
                    // Ambil 2 aktivitas pertama
                    var activities = Activity.Split(',')
                        .Select(a => a.Trim())
                        .Where(a => !string.IsNullOrEmpty(a))
                        .Take(2)
                        .ToArray();
                    
                    if (activities.Length > 0)
                    {
                        string activityText = string.Join(", ", activities);
                        if (Activity.Split(',').Length > 2)
                            activityText += "...";
                        
                        // PERBAIKI: Batasi panjang aktivitas
                        if (activityText.Length > 30)
                        {
                            activityText = activityText.Substring(0, 27) + "...";
                        }
                        
                        lblAktivitas.Text = activityText;
                        lblAktivitas.Visible = true;
                        lblAktivitas.AutoSize = false;
                        lblAktivitas.Size = new Size(400, 20);
                    }
                    else
                    {
                        lblAktivitas.Visible = false;
                    }
                    lblAktivitas.Cursor = Cursors.Hand;
                }
                else if (lblAktivitas != null)
                {
                    lblAktivitas.Visible = false;
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

        // Load cuaca dari WeatherService
        private async void LoadWeatherAsync()
        {
            await LoadWeatherInternalAsync();
        }

        // Method internal untuk load weather yang bisa dipanggil secara async
        private async Task LoadWeatherInternalAsync()
        {
            if (lblCuaca == null) return;

            try
            {
                // Default sementara cuaca loading
                lblCuaca.Text = "Loading...";
                lblCuaca.BackColor = Color.LightGray;
                lblCuaca.ForeColor = Color.Black;

                // Ambil data cuaca
                var weather = await WeatherService.GetCurrentAsync(NamaDestinasi);

                // Tentukan status cuaca berdasarkan kondisi
                string status;
                Color bgColor;
                Color fgColor;

                if (weather.Description.ToLower().Contains("rain") ||
                    weather.Description.ToLower().Contains("shower") ||
                    weather.Description.ToLower().Contains("storm"))
                {
                    status = "Kurang Baik";
                    bgColor = Color.FromArgb(255, 102, 102); // Red
                    fgColor = Color.White;
                }
                else if (weather.WindSpeedKmh > 25 || weather.Humidity > 85)
                {
                    status = "Cukup Baik";
                    bgColor = Color.FromArgb(255, 204, 102); // Orange
                    fgColor = Color.Black;
                }
                else
                {
                    status = "Sangat Baik";
                    bgColor = Color.FromArgb(144, 238, 144); // Light green
                    fgColor = Color.FromArgb(0, 100, 0); // Dark green
                }

                lblCuaca.Text = status;
                lblCuaca.BackColor = bgColor;
                lblCuaca.ForeColor = fgColor;
            }
            catch
            {
                // Jika gagal ambil cuaca, tampilkan default
                lblCuaca.Text = "Tidak Ada Data";
                lblCuaca.BackColor = Color.LightGray;
                lblCuaca.ForeColor = Color.Black;
            }
        }

        // Public method untuk reload cuaca (dapat dipanggil dari DashboardUtama)
        public async Task ReloadWeatherAsync()
        {
            await LoadWeatherInternalAsync();
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

        private void shadowPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
