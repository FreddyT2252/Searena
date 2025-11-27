// DetailDestinasi.cs
using Npgsql;
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
    public partial class DetailDestinasi : Form
    {
        private readonly int _destId;
        private readonly string _destName;
        private readonly string _destLocation;



        private const string ConnString = "Host=aws-1-ap-southeast-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.eeqqiyfukvhbwystupei;Password=SearenaDB123";

        public class ReviewItem
        {
            public string Username { get; set; }
            public int Rating { get; set; }
            public string ReviewText { get; set; }
            public DateTime Tanggal { get; set; }
        }
        public DetailDestinasi(int destId, string destName, string destLocation)
        {
            InitializeComponent();
            _destId = destId;
            _destName = destName;
            _destLocation = destLocation;

            this.Load += DetailDestinasi_Load;

            btnKirim.Click -= BtnKirim_Click;
            btnKirim.Click += BtnKirim_Click;
        }

        public DetailDestinasi()
        {
            InitializeComponent();
            this.Load += DetailDestinasi_Load;
            btnKirim.Click -= BtnKirim_Click;
            btnKirim.Click += BtnKirim_Click;
        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel33_Click(object sender, EventArgs e)
        {

        }

        private void FixLabelWrap(Guna.UI2.WinForms.Guna2HtmlLabel lbl)
        {
            lbl.AutoSize = false;
            lbl.AutoSizeHeightOnly = true;
            lbl.MaximumSize = new Size(300, 0); // sesuaikan dengan lebar panel (318 total - padding)
        }
        private async Task LoadDestinasiInfoAsync()
        {
            using (var conn = new NpgsqlConnection(ConnString))
            {
                await conn.OpenAsync();

                string sql = @"
            SELECT deskripsi, harga_min, harga_max, waktu_terbaik, activity
            FROM destinasi
            WHERE destinasi_id = @id;
        ";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", _destId);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                           
                            string deskripsi = reader.IsDBNull(0) ? "" : reader.GetString(0);
                            decimal hargaMin = reader.IsDBNull(1) ? 0 : reader.GetDecimal(1);
                            decimal hargaMax = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2);
                            string waktuTerbaik = reader.IsDBNull(3) ? "" : reader.GetString(3);
                            string aktivitasStr = reader.IsDBNull(4) ? "" : reader.GetString(4);

                        
                            lblDeskripsi.Text = deskripsi;
                        

                            FixLabelWrap(lblDeskripsi);

                            
                            if (hargaMin > 0 || hargaMax > 0)
                            {
                                lblHargaTiket.Text =
                                    $"Rp {hargaMin:N0} - {hargaMax:N0}";
                            }
                            else
                            {
                                lblHargaTiket.Text = "-";
                            }

                            if (!string.IsNullOrWhiteSpace(waktuTerbaik))
                            {
                                
                                var formatted = string.Join(", ",
                                    waktuTerbaik
                                        .Split(',')
                                        .Select(s => s.Trim())
                                        .Where(s => s.Length > 0));

                                lblWaktuTerbaik.Text = formatted;
                            }
                            else
                            {
                                lblWaktuTerbaik.Text = "-";
                            }


                            if (!string.IsNullOrWhiteSpace(aktivitasStr))
                            {
                                var activities = aktivitasStr.Split(',');

                                var lbls = new[]
                                {
                            lblFasilitas1,        
                            lblFasilitas2,     
                            lblFasilitas3,     
                            lblFasilitas4,     
                            lblFasilitas5      
                        };

                                var panels = new[]
                                {
                            PnlAktivitas1,
                            PnlAktivitas2,
                            PnlAktivitas3,
                            PnlAktivitas4,
                            PnlAktivitas5
                        };

                                for (int i = 0; i < lbls.Length; i++)
                                {
                                    if (i < activities.Length)
                                    {
                                        lbls[i].Text = activities[i].Trim();
                                        panels[i].Visible = true;
                                    }
                                    else
                                    {
                                        panels[i].Visible = false;
                                    }
                                }
                            }
                            else
                            {
                              
                                PnlAktivitas1.Visible = false;
                                PnlAktivitas2.Visible = false;
                                PnlAktivitas3.Visible = false;
                                PnlAktivitas4.Visible = false;
                                PnlAktivitas5.Visible = false;
                            }
                        }
                    }
                }
            }
        }

        private async void DetailDestinasi_Load(object sender, EventArgs e)
        {
         
            if (!string.IsNullOrWhiteSpace(_destName)) lblDestinasi1.Text = _destName;
            if (!string.IsNullOrWhiteSpace(_destLocation)) lblLokasi1.Text = _destLocation;

           
            if (guna2RatingStar1 != null) guna2RatingStar1.Value = 5;
            FixLabelWrap(lblDeskripsi);
          

            try
            {
                await LoadDestinasiInfoAsync();

                var reviews = await GetReviewsAsync();
                TampilkanReviewsKeUI(reviews);

                await LoadWeatherAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat ulasan: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void TampilkanReviewsKeUI(List<ReviewItem> reviews)
        {
            // Kosongkan dulu (biar kalau tidak ada data, labelnya kosong)
            lblNama1.Text = "";
            lblTanggal1.Text = "";
            lblReview1.Text = "";
            ratingStar1.Text = "";

            lblNama2.Text = "";
            lblTanggal2.Text = "";
            lblReview2.Text = "";
            ratingStar2.Text = "";

            // Review pertama
            if (reviews.Count > 0)
            {
                lblNama1.Text = reviews[0].Username;
                lblTanggal1.Text = reviews[0].Tanggal.ToString("dd/MM/yyyy");
                lblReview1.Text = reviews[0].ReviewText;
                ratingStar1.Text = reviews[0].Rating.ToString();
            }

            // Review kedua
            if (reviews.Count > 1)
            {
                lblNama2.Text = reviews[1].Username;
                lblTanggal2.Text = reviews[1].Tanggal.ToString("dd/MM/yyyy");
                lblReview2.Text = reviews[1].ReviewText;
                ratingStar2.Text = reviews[1].Rating.ToString();
            }
            await LoadWeatherAsync();


        }

        private async Task<List<ReviewItem>> GetReviewsAsync()
        {
            var list = new List<ReviewItem>();

            using (var conn = new NpgsqlConnection(ConnString))
            {
                await conn.OpenAsync();

                string sql = @"
            SELECT username, rating, review_text, tanggal_review
            FROM public.reviews
            WHERE destinasi_id = @destId
            ORDER BY tanggal_review DESC
            LIMIT 3;      -- misal cuma mau tampil 3 review terbaru
        ";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@destId", _destId);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new ReviewItem
                            {
                                Username = reader.GetString(0),
                                Rating = reader.GetInt32(1),
                                ReviewText = reader.GetString(2),
                                Tanggal = reader.GetDateTime(3)
                            });
                        }
                    }
                }
            }

            return list;
        }
        private async void BtnKirim_Click(object sender, EventArgs e)
        {
            try
            {
                string ulasan = (tbUlasan?.Text ?? "").Trim();
                if (string.IsNullOrWhiteSpace(ulasan))
                {
                    MessageBox.Show("Ulasan tidak boleh kosong.", "Validasi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tbUlasan?.Focus();
                    return;
                }


                int rating = guna2RatingStar1 != null ? (int)guna2RatingStar1.Value : 5;

                using (var conn = new NpgsqlConnection(ConnString))
                {
                    await conn.OpenAsync();


                    string createSql = @"
                    CREATE TABLE IF NOT EXISTS public.reviews (
                        review_id      SERIAL PRIMARY KEY,
                        user_id        INT NOT NULL,
                        username       TEXT NOT NULL,
                        destinasi_id        INT NOT NULL,
                        dest_name      TEXT NOT NULL,
                        dest_location  TEXT NOT NULL,
                        review_text    TEXT NOT NULL,
                        rating         INT  NOT NULL CHECK (rating BETWEEN 1 AND 5),
                        tanggal_review     TIMESTAMPTZ NOT NULL DEFAULT NOW()
                    );";
                    using (var cmdCreate = new NpgsqlCommand(createSql, conn))
                        await cmdCreate.ExecuteNonQueryAsync();


                    string insertSql = @"
                    INSERT INTO public.reviews
                        (user_id, username, destinasi_id, dest_name, dest_location, review_text, rating)
                    VALUES
                        (@uid, @uname, @destinasi_id, @dname, @dloc, @txt, @rate);";

                    using (var cmd = new NpgsqlCommand(insertSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@uid", UserSession.UserId);
                        cmd.Parameters.AddWithValue("@uname", UserSession.Username);
                        cmd.Parameters.AddWithValue("@destinasi_id", _destId);
                        cmd.Parameters.AddWithValue("@dname", string.IsNullOrWhiteSpace(_destName) ? lblDestinasi1.Text : _destName);
                        cmd.Parameters.AddWithValue("@dloc", string.IsNullOrWhiteSpace(_destLocation) ? lblLokasi1.Text : _destLocation);
                        cmd.Parameters.AddWithValue("@txt", ulasan);
                        cmd.Parameters.AddWithValue("@rate", rating);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                MessageBox.Show("Ulasan & rating berhasil dikirim.", "Berhasil",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                // reset input
                if (tbUlasan != null) tbUlasan.Text = "";
                if (guna2RatingStar1 != null) guna2RatingStar1.Value = 5;

                var reviews = await GetReviewsAsync();
                TampilkanReviewsKeUI(reviews);

                // (opsional) buka profil agar user lihat riwayatnya
                // var profil = new Form2();
                // profil.Show();
                // this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal menyimpan ulasan: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task LoadWeatherAsync()
        {

            string lokasiFull = !string.IsNullOrWhiteSpace(_destLocation)
                ? _destLocation
                : lblLokasi1.Text;

            if (string.IsNullOrWhiteSpace(lokasiFull))
                return;


            string lokasi = lokasiFull.Split(',')[0].Trim();

            try
            {
                var w = await WeatherService.GetCurrentAsync(lokasi);


                lblSuhu.Text = string.Format("{0:F1} °C", w.TemperatureC);
                lblAngin.Text = string.Format("{0:F1} km/jam", w.WindSpeedKmh);
                lblKelembapan.Text = w.Humidity + " %";
                lblDeskripsiCuaca.Text = w.Description;


                string status;
                if (w.Description.ToLower().Contains("rain") ||
                    w.Description.ToLower().Contains("shower") ||
                    w.Description.ToLower().Contains("storm"))
                {
                    status = "Kurang Baik";
                    lblCuaca.BackColor = Color.OrangeRed;
                }
                else if (w.WindSpeedKmh > 25 || w.Humidity > 85)
                {
                    status = "Cukup Baik";
                    lblCuaca.BackColor = Color.Gold;
                }
                else
                {
                    status = "Sangat Baik";
                    lblCuaca.BackColor = Color.LightGreen;
                }

                lblCuaca.Text = status;
            }
            catch (Exception ex)
            {

                lblCuaca.Text = "Tidak Ada Data";

            }
        }



        private void guna2HtmlLabel38_Click(object sender, EventArgs e)
        {

        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2HtmlLabel28_Click(object sender, EventArgs e)
        {

        }

        private void pnlCuaca_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2HtmlLabel28_Click_1(object sender, EventArgs e)
        {

        }

        private void lblCuaca_Click(object sender, EventArgs e)
        {

        }
        private void PnlRekomendasiCuaca_Paint(object sender, PaintEventArgs e)
        {
            // kosong juga tidak apa-apa
        }

        private void PnlDeskripsi_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnKembali_Click_1(object sender, EventArgs e)
        {
            this.Close();

        }

        private void lblDeskripsi_Click(object sender, EventArgs e)
        {

        }
    }
}
