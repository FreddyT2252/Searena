// DetailDestinasi.cs
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
        

        // Pakai koneksi Supabase (punya kamu yang pertama)
        private const string ConnString =
            "Host=aws-1-ap-southeast-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.eeqqiyfukvhbwystupei;Password=SearenaDB123";

        // ===== Model review sederhana (untuk versi label tetap) =====
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
            

            this.Load += DetailDestinasi_Load;

            btnKirim.Click -= BtnKirim_Click;
            btnKirim.Click += BtnKirim_Click;

            if (btnBookmark != null)
            {
                btnBookmark.Click -= btnBookmark_Click_1;
                btnBookmark.Click += btnBookmark_Click_1;
            }

            // PERBAIKI: Setup event handler untuk button kembali
            if (btnKembali != null)
            {
                btnKembali.Click -= btnKembali_Click;
                btnKembali.Click += btnKembali_Click;
                btnKembali.Cursor = Cursors.Hand;
                btnKembali.Enabled = true;
            }

            // PERBAIKI: Setup event handler untuk profile picture - CARI SEMUA KEMUNGKINAN NAMA
            SetupProfileClickHandler();
        }

        // Method terpisah untuk setup profile click handler
        private void SetupProfileClickHandler()
        {
            // Coba berbagai kemungkinan nama control profile
            string[] possibleNames = new[] { 
                "PctProfile", "Profile", "guna2CirclePictureBox1", 
                "ProfilePicture", "pictureBoxProfile", "CircleProfile" 
            };

            foreach (var name in possibleNames)
            {
                var profilePic = this.Controls.Find(name, true).FirstOrDefault();
                
                if (profilePic != null)
                {
                    if (profilePic is Guna.UI2.WinForms.Guna2CirclePictureBox gunaPic)
                    {
                        gunaPic.Click -= ProfilePic_Click;
                        gunaPic.Click += ProfilePic_Click;
                        gunaPic.Cursor = Cursors.Hand;
                        gunaPic.Enabled = true;
                        System.Diagnostics.Debug.WriteLine($"Profile picture found: {name}");
                        break;
                    }
                    else if (profilePic is PictureBox pic)
                    {
                        pic.Click -= ProfilePic_Click;
                        pic.Click += ProfilePic_Click;
                        pic.Cursor = Cursors.Hand;
                        pic.Enabled = true;
                        System.Diagnostics.Debug.WriteLine($"Profile picture found: {name}");
                        break;
                    }
                }
            }
        }

        // =================== LOAD FORM ===================

        private async void DetailDestinasi_Load(object sender, EventArgs e)
        {
            // Tampilkan nama & lokasi dari ctor, kalau ada
            if (!string.IsNullOrWhiteSpace(_destName)) lblDestinasi1.Text = _destName;
            

            // Default rating
            if (guna2RatingStar1 != null) guna2RatingStar1.Value = 5;

            // Perbaiki wrapping label deskripsi (kalau ada)
            FixLabelWrap(lblDeskripsi);

            try
            {
                // Set cursor loading hanya sekali di awal
                Cursor = Cursors.WaitCursor;

                // Load info destinasi dari tabel destinasi
                if (_destId > 0)
                {
                    await LoadDestinasiInfoAsync();
                }

                // Pastikan tabel bookmark & review ada
                await EnsureBookmarkTableAsync();
                await EnsureReviewsTableAsync();

                // Update status bookmark button
                await CheckBookmarkStatusAsync(); // Ubah jadi async

                // Load review ke panel dinamis
                await LoadReviewsAsync();

                // Load cuaca
                await LoadWeatherAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat detail destinasi: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // PERBAIKI: Kembalikan cursor normal di finally
                Cursor = Cursors.Default;
            }
        }

        // =================== HELPER WRAP LABEL ===================

        private void FixLabelWrap(Guna.UI2.WinForms.Guna2HtmlLabel lbl)
        {
            if (lbl == null) return;

            lbl.AutoSize = false;
            lbl.AutoSizeHeightOnly = true;

            int maxWidth = 300;
            if (lbl.Parent != null)
            {
                try
                {
                    // Hitung lebar maksimal berdasarkan parent width
                    maxWidth = Math.Max(100, lbl.Parent.Width - 50); // Tambah margin kiri/kanan
                }
                catch { }
            }
            lbl.MaximumSize = new Size(maxWidth, 0);
            
            // PERBAIKI: Tambahkan word wrap agar text tidak overflow
            lbl.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
        }

        // =================== LOAD DESTINASI INFO (DESKRIPSI / HARGA / WAKTU / AKTIVITAS) ===================

        private async Task LoadDestinasiInfoAsync()
        {
            using (var conn = new NpgsqlConnection(ConnString))
            {
                await conn.OpenAsync();

                string sql = @"
                    SELECT deskripsi, harga_min, harga_max, waktu_terbaik, activity, lokasi, pulau
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
                            string deskripsi   = reader.IsDBNull(0) ? "" : reader.GetString(0);
                            decimal hargaMin   = reader.IsDBNull(1) ? 0 : reader.GetDecimal(1);
                            decimal hargaMax   = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2);
                            string waktuTerbaik = reader.IsDBNull(3) ? "" : reader.GetString(3);
                            string aktivitasStr = reader.IsDBNull(4) ? "" : reader.GetString(4);
                            string lokasi = reader.IsDBNull(5) ? "" : reader.GetString(5);
                            string pulau = reader.IsDBNull(6) ? "" : reader.GetString(6);

                            lblLokasi1.Text = $"{lokasi}, {pulau}".Trim().Trim(',');

                            // Deskripsi
                            lblDeskripsi.Text = deskripsi;
                            FixLabelWrap(lblDeskripsi);

                            // Harga tiket
                            if (hargaMin > 0 || hargaMax > 0)
                            {
                                lblHargaTiket.Text = $"Rp {hargaMin:N0} - {hargaMax:N0}";
                            }
                            else
                            {
                                lblHargaTiket.Text = "-";
                            }

                            // Waktu terbaik
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

                            // Aktivitas
                            if (!string.IsNullOrWhiteSpace(aktivitasStr))
                            {
                                var activities = aktivitasStr.Split(',')
                                    .Select(a => a.Trim())
                                    .Where(a => !string.IsNullOrWhiteSpace(a))
                                    .ToArray();

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
                                    if (i < activities.Length && lbls[i] != null && panels[i] != null)
                                    {
                                        // Batasi panjang text aktivitas
                                        string activityText = activities[i];
                                        if (activityText.Length > 15)
                                        {
                                            activityText = activityText.Substring(0, 12) + "...";
                                        }
                                        
                                        lbls[i].Text = activityText;
                                        lbls[i].AutoSize = false;
                                        lbls[i].Size = new Size(Math.Min(120, panels[i].Width - 10), 20);
                                        panels[i].Visible = true;
                                    }
                                    else if (panels[i] != null)
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

        // =================== REVIEW: VERSI LABEL TETAP (OPSIONAL) ===================

        private async void TampilkanReviewsKeUI(List<ReviewItem> reviews)
        {
            // Kosongkan dulu
            lblNama1.Text = "";
            lblTanggal1.Text =("");
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

            // Kalau mau, bisa panggil lagi cuaca di sini
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
                    LIMIT 3;
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
                                Username   = reader.GetString(0),
                                Rating     = reader.GetInt32(1),
                                ReviewText = reader.GetString(2),
                                Tanggal    = reader.GetDateTime(3)
                            });
                        }
                    }
                }
            }

            return list;
        }

        // =================== REVIEW: TABEL & PANEL DINAMIS ===================

        private async Task EnsureReviewsTableAsync()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnString))
                {
                    await conn.OpenAsync();

                    string createSql = @"
                        CREATE TABLE IF NOT EXISTS public.reviews (
                            review_id      SERIAL PRIMARY KEY,
                            user_id        INT NOT NULL,
                            username       TEXT NOT NULL,
                            destinasi_id   INT NOT NULL,
                            dest_name      TEXT NOT NULL,
                            dest_location  TEXT NOT NULL,
                            review_text    TEXT NOT NULL,
                            rating         INT  NOT NULL CHECK (rating BETWEEN 1 AND 5),
                            tanggal_review TIMESTAMPTZ NOT NULL DEFAULT NOW()
                        );";
                    using (var cmdCreate = new NpgsqlCommand(createSql, conn))
                        await cmdCreate.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error ensuring reviews table: {ex.Message}");
            }
        }

        private async Task LoadReviewsAsync()
        {
            try
            {
                if (PnlRatingReview == null) return;

                PnlRatingReview.Controls.Clear();

                var title = new Guna.UI2.WinForms.Guna2HtmlLabel
                {
                    Text = "Ulasan Pengunjung",
                    Location = new Point(16, 15),
                    Font = new Font("Malgun Gothic", 8.25f, FontStyle.Bold),
                    BackColor = Color.Transparent
                };
                PnlRatingReview.Controls.Add(title);

                using (var conn = new NpgsqlConnection(ConnString))
                {
                    await conn.OpenAsync();
                    var sql = @"
                        SELECT username, rating, review_text, tanggal_review
                        FROM public.reviews
                        WHERE destinasi_id = @did
                        ORDER BY tanggal_review DESC
                        LIMIT 5;
                    ";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@did", _destId);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            int y = title.Bottom + 10;
                            bool any = false;

                            while (await reader.ReadAsync())
                            {
                                any = true;
                                string uname = reader.GetString(0);
                                int rate      = reader.GetInt32(1);
                                string text   = reader.GetString(2);
                                DateTime dt   = reader.GetDateTime(3);

                                var lblUser = new Guna.UI2.WinForms.Guna2HtmlLabel
                                {
                                    Text = uname,
                                    Location = new Point(60, y),
                                    Font = new Font("Malgun Gothic", 8.25f, FontStyle.Bold),
                                    BackColor = Color.Transparent
                                };
                                var lblDate = new Guna.UI2.WinForms.Guna2HtmlLabel
                                {
                                    Text = dt.ToString("dd/MM/yyyy"),
                                    Location = new Point(PnlRatingReview.Width - 110, y),
                                    Font = new Font("Malgun Gothic", 8.25f, FontStyle.Regular),
                                    BackColor = Color.Transparent
                                };
                                var lblRate = new Guna.UI2.WinForms.Guna2HtmlLabel
                                {
                                    Text = rate.ToString(),
                                    Location = new Point(26, y + 28),
                                    Font = new Font("Malgun Gothic", 8.25f, FontStyle.Bold),
                                    BackColor = Color.Transparent
                                };
                                var star = new PictureBox
                                {
                                    // TODO: ganti icon bintang yang bener
                                    Image = SystemIcons.Information.ToBitmap(),
                                    SizeMode = PictureBoxSizeMode.StretchImage,
                                    Size = new Size(18, 18),
                                    Location = new Point(24, y + 8)
                                };
                                var lblText = new Guna.UI2.WinForms.Guna2HtmlLabel
                                {
                                    BackColor = Color.Transparent,
                                    Location = new Point(60, y + 22),
                                    Font = new Font("Malgun Gothic Semilight", 8.25f, FontStyle.Regular),
                                    Name = "lblReviewText"
                                };
                                lblText.Text = text;
                                lblText.AutoSize = false;
                                lblText.AutoSizeHeightOnly = true;
                                lblText.MaximumSize = new Size(Math.Max(120, PnlRatingReview.Width - 80), 0);

                                PnlRatingReview.Controls.Add(lblUser);
                                PnlRatingReview.Controls.Add(lblDate);
                                PnlRatingReview.Controls.Add(star);
                                PnlRatingReview.Controls.Add(lblRate);
                                PnlRatingReview.Controls.Add(lblText);

                                y = lblText.Bottom + 12;
                            }

                            if (!any)
                            {
                                var empty = new Guna.UI2.WinForms.Guna2HtmlLabel
                                {
                                    Text = "Belum ada review",
                                    Location = new Point(16, title.Bottom + 10),
                                    Font = new Font("Malgun Gothic", 8.25f, FontStyle.Italic),
                                    BackColor = Color.Transparent
                                };
                                PnlRatingReview.Controls.Add(empty);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading reviews: {ex.Message}");
            }
        }

        // =================== KIRIM REVIEW ===================

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

                    // pastikan tabel reviews ada
                    string createSql = @"
                        CREATE TABLE IF NOT EXISTS public.reviews (
                            review_id      SERIAL PRIMARY KEY,
                            user_id        INT NOT NULL,
                            username       TEXT NOT NULL,
                            destinasi_id   INT NOT NULL,
                            dest_name      TEXT NOT NULL,
                            dest_location  TEXT NOT NULL,
                            review_text    TEXT NOT NULL,
                            rating         INT  NOT NULL CHECK (rating BETWEEN 1 AND 5),
                            tanggal_review TIMESTAMPTZ NOT NULL DEFAULT NOW()
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
                        cmd.Parameters.AddWithValue("@dloc", lblLokasi1.Text);
                        cmd.Parameters.AddWithValue("@txt", ulasan);
                        cmd.Parameters.AddWithValue("@rate", rating);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                MessageBox.Show("Ulasan & rating berhasil dikirim.", "Berhasil",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (tbUlasan != null) tbUlasan.Text = "";
                if (guna2RatingStar1 != null) guna2RatingStar1.Value = 5;

                // refresh UI review
                await LoadReviewsAsync();
                // atau kalau mau versi label:
                // var reviews = await GetReviewsAsync();
                // TampilkanReviewsKeUI(reviews);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal menyimpan ulasan: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =================== BOOKMARK ===================

        private async Task EnsureBookmarkTableAsync()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnString))
                {
                    await conn.OpenAsync();
                    string createSql = @"
                        CREATE TABLE IF NOT EXISTS public.bookmarks (
                            id SERIAL PRIMARY KEY,
                            user_id INT NOT NULL,
                            destinasi_id INT NOT NULL,
                            tanggal_bookmark TIMESTAMPTZ NOT NULL DEFAULT NOW(),
                            CONSTRAINT uq_user_dest UNIQUE(user_id, destinasi_id)
                        );";
                    using (var cmdCreate = new NpgsqlCommand(createSql, conn))
                        await cmdCreate.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error ensuring bookmark table: {ex.Message}");
            }
        }

        private async Task CheckBookmarkStatusAsync() // Ubah jadi async Task
        {
            try
            {
                if (btnBookmark == null) return;

                using (var conn = new NpgsqlConnection(ConnString))
                {
                    await conn.OpenAsync();

                    using (var cmd = new NpgsqlCommand(
                        "SELECT COUNT(*) FROM bookmarks WHERE user_id = @uid AND destinasi_id = @did", conn))
                    {
                        cmd.Parameters.AddWithValue("@uid", UserSession.UserId);
                        cmd.Parameters.AddWithValue("@did", _destId);

                        var result = (long)await cmd.ExecuteScalarAsync();

                        if (result > 0)
                        {
                            btnBookmark.Text = "Hapus dari Bookmark";
                            btnBookmark.FillColor = Color.LightCoral;
                            btnBookmark.Tag = "bookmarked";
                        }
                        else
                        {
                            btnBookmark.Text = "Bookmark";
                            btnBookmark.FillColor = Color.LightBlue;
                            btnBookmark.Tag = "not_bookmarked";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking bookmark: {ex.Message}");
            }
        }

        private async void btnBookmark_Click_1(object sender, EventArgs e)
        {
            await EnsureBookmarkTableAsync();
            await BtnBookmark_Toggle(); // Ubah jadi async
        }

        private async Task BtnBookmark_Toggle() // Ubah jadi async Task
        {
            try
            {
                if (btnBookmark == null) return;

                string status = btnBookmark.Tag?.ToString() ?? "not_bookmarked";

                if (status == "bookmarked")
                {
                    await RemoveBookmark();
                }
                else
                {
                    await AddBookmark();
                }

                await CheckBookmarkStatusAsync(); // Panggil yang async
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async Task AddBookmark()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnString))
                {
                    await conn.OpenAsync();

                    using (var cmd = new NpgsqlCommand(
                        @"INSERT INTO bookmarks (user_id, destinasi_id)
                          VALUES (@uid, @did)
                          ON CONFLICT (user_id, destinasi_id) DO NOTHING;", conn))
                    {
                        cmd.Parameters.AddWithValue("@uid", UserSession.UserId);
                        cmd.Parameters.AddWithValue("@did", _destId);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                MessageBox.Show("Destinasi berhasil di-bookmark!", "Sukses",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal menambah bookmark: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task RemoveBookmark()
        {
            try
            {
                DialogResult result = MessageBox.Show("Hapus dari bookmark?", "Konfirmasi",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result != DialogResult.Yes) return;

                using (var conn = new NpgsqlConnection(ConnString))
                {
                    await conn.OpenAsync();

                    using (var cmd = new NpgsqlCommand(
                        @"DELETE FROM bookmarks WHERE user_id = @uid AND destinasi_id = @did", conn))
                    {
                        cmd.Parameters.AddWithValue("@uid", UserSession.UserId);
                        cmd.Parameters.AddWithValue("@did", _destId);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                MessageBox.Show("Bookmark berhasil dihapus!", "Sukses",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal menghapus bookmark: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =================== CUACA ===================

        private async Task LoadWeatherAsync()
        {
            string lokasiFull = lblLokasi1.Text;


            if (string.IsNullOrWhiteSpace(lokasiFull))
                return;

            string lokasi = lokasiFull.Split(',')[0].Trim();

            try
            {
                var w = await WeatherService.GetCurrentAsync(lokasi);

                lblSuhu.Text        = string.Format("{0:F1} °C", w.TemperatureC);
                lblAngin.Text       = string.Format("{0:F1} km/jam", w.WindSpeedKmh);
                lblKelembapan.Text  = w.Humidity + " %";
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
            catch
            {
                lblCuaca.Text = "Tidak Ada Data";
            }
        }

        // =================== EVENT-EVENT KECIL ===================

        private void guna2PictureBox2_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel33_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel38_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel36_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel28_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel28_Click_1(object sender, EventArgs e) { }
        private void lblCuaca_Click(object sender, EventArgs e) { }

        private void PnlRekomendasiCuaca_Paint(object sender, PaintEventArgs e) { }
        private void PnlDeskripsi_Paint(object sender, PaintEventArgs e) { }
        private void PnlRatingReview_Paint(object sender, PaintEventArgs e) { }
        private void pnlCuaca_Paint(object sender, PaintEventArgs e) { }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            // PERBAIKI: Proper form closure
            this.Close();
        }

        private void btnKembali_Click_1(object sender, EventArgs e)
        {
            // PERBAIKI: Proper form closure
            this.Close();
        }

        private void lblDeskripsi_Click(object sender, EventArgs e) { }

        // PERBAIKI: Tambahkan handler untuk profile picture
        private void ProfilePic_Click(object sender, EventArgs e)
        {
            try
            {
                // Buka halaman profil
                Form2 profileForm = new Form2(this);
                profileForm.FormClosed += (s, args) => this.Show();
                profileForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error membuka profil: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
