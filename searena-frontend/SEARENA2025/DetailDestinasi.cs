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

        private const string ConnString = "Host=localhost;Port=5432;Database=searena_db;Username=postgres;Password=Putriananev2412";

        public DetailDestinasi(int destId, string destName, string destLocation)
        {
            InitializeComponent();
            _destId = destId;
            _destName = destName;
            _destLocation = destLocation;

            this.Load += DetailDestinasi_Load;

            btnKirim.Click -= BtnKirim_Click;
            btnKirim.Click += BtnKirim_Click;
            // gunakan handler designer untuk bookmark; jika belum terpasang, fallback ke handler ini
            this.btnBookmark.Click -= btnBookmark_Click_1;
            this.btnBookmark.Click += btnBookmark_Click_1;
        }

        public DetailDestinasi()
        {
            InitializeComponent();
            this.Load += DetailDestinasi_Load;
            btnKirim.Click -= BtnKirim_Click;
            btnKirim.Click += BtnKirim_Click;
            this.btnBookmark.Click -= btnBookmark_Click_1;
            this.btnBookmark.Click += btnBookmark_Click_1;
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
            // Sesuaikan lebar dengan panel deskripsi jika parent panel
            int maxWidth = 300;
            if (lbl.Parent != null)
            {
                try
                {
                    maxWidth = Math.Max(100, lbl.Parent.Width - 24); // padding kiri/kanan
                }
                catch { }
            }
            lbl.MaximumSize = new Size(maxWidth, 0);
        }

        private async void DetailDestinasi_Load(object sender, EventArgs e)
        {
            // Load data destinasi dari database jika _destId valid
            if (_destId > 0)
            {
                LoadDestinasiDetail();
            }
            else
            {
                // tampilkan nama & lokasi dari parameter (kalau ctor param dipakai, nilainya terisi)
                if (!string.IsNullOrWhiteSpace(_destName)) lblDestinasi1.Text = _destName;
                if (!string.IsNullOrWhiteSpace(_destLocation)) lblLokasi1.Text = _destLocation;
            }

            // default rating bintang
            if (guna2RatingStar1 != null) guna2RatingStar1.Value = 5;
            FixLabelWrap(guna2HtmlLabel36);
            FixLabelWrap(guna2HtmlLabel37);
            FixLabelWrap(guna2HtmlLabel38);
            FixLabelWrap(guna2HtmlLabel39);

            // Check if already bookmarked dan ubah icon/warna button
            await EnsureBookmarkTableAsync();
            CheckBookmarkStatus();

            // Muat review secara real-time
            await EnsureReviewsTableAsync();
            await LoadReviewsAsync();
        }

        // Check apakah destinasi ini sudah di-bookmark
        private async void CheckBookmarkStatus()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnString))
                {
                    await conn.OpenAsync();

                    using (var cmd = new NpgsqlCommand(
                        "SELECT COUNT(*) FROM bookmarks WHERE user_id = @uid AND destinasi_id = @did", conn))
                    {
                        cmd.Parameters.AddWithValue("@uid", UserSession.UserId);
                        cmd.Parameters.AddWithValue("@did", _destId);

                        var result = (long)await cmd.ExecuteScalarAsync();

                        // Update button appearance
                        if (result > 0)
                        {
                            // Sudah bookmark
                            btnBookmark.Text = "Hapus dari Bookmark";
                            btnBookmark.FillColor = Color.LightCoral;
                            btnBookmark.Tag = "bookmarked"; // Mark sebagai bookmarked
                        }
                        else
                        {
                            // Belum bookmark
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

        // Event handler untuk button bookmark (dipanggil dari designer)
        private async void btnBookmark_Click_1(object sender, EventArgs e)
        {
            await EnsureBookmarkTableAsync();
            BtnBookmark_Toggle();
        }

        // Helper untuk toggle bookmark
        private async void BtnBookmark_Toggle()
        {
            try
            {
                string status = btnBookmark.Tag?.ToString() ?? "not_bookmarked";

                if (status == "bookmarked")
                {
                    await RemoveBookmark();
                }
                else
                {
                    await AddBookmark();
                }

                // Refresh status
                CheckBookmarkStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        // Pastikan tabel bookmarks tersedia
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

        // Event handler untuk button bookmark (lama) - tidak digunakan langsung
        private async void BtnBookmark_Click(object sender, EventArgs e)
        {
            await EnsureBookmarkTableAsync();
            BtnBookmark_Toggle();
        }

        // Tambah bookmark ke database
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

        // Hapus bookmark dari database
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

        private void LoadDestinasiDetail()
        {
            try
            {
                var destData = SEARENA2025.Destinasi.GetById(_destId);
                if (destData != null)
                {
                    // Update UI dengan data dari database
                    if (lblDestinasi1 != null)
                        lblDestinasi1.Text = destData.NamaDestinasi;
                    
                    if (lblLokasi1 != null)
                        lblLokasi1.Text = destData.Lokasi;

                    // Tampilkan deskripsi di PnlDeskripsi (auto wrap)
                    PopulateDescriptionInPanel(destData.Deskripsi);

                    // Update waktu terbaik jika ada label untuk itu
                    var lblWaktuTerbaik = this.Controls.Find("lblWaktuTerbaik", true).FirstOrDefault() as Guna.UI2.WinForms.Guna2HtmlLabel;
                    if (lblWaktuTerbaik != null && !string.IsNullOrEmpty(destData.WaktuTerbaik))
                    {
                        lblWaktuTerbaik.Text = "Waktu Terbaik: " + destData.WaktuTerbaik;
                    }

                    // Update rating average jika ada label untuk itu
                    var lblRatingAvg = this.Controls.Find("lblRatingAvg", true).FirstOrDefault() as Guna.UI2.WinForms.Guna2HtmlLabel;
                    if (lblRatingAvg != null)
                    {
                        lblRatingAvg.Text = $"★ {destData.RatingAvg:F1}";
                    }

                    // Update harga jika ada label untuk itu
                    var lblHarga = this.Controls.Find("lblHarga", true).FirstOrDefault() as Guna.UI2.WinForms.Guna2HtmlLabel;
                    if (lblHarga != null && destData.HargaMin > 0)
                    {
                        if (destData.HargaMax > destData.HargaMin)
                        {
                            lblHarga.Text = $"Rp {destData.HargaMin:N0} - Rp {destData.HargaMax:N0}";
                        }
                        else
                        {
                            lblHarga.Text = $"Mulai dari Rp {destData.HargaMin:N0}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading detail destinasi: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Render deskripsi pada panel khusus dengan auto-wrapping
        private void PopulateDescriptionInPanel(string deskripsi)
        {
            try
            {
                if (PnlDeskripsi == null) return;

                // Simpan judul jika ada (guna2HtmlLabel35)
                var title = PnlDeskripsi.Controls.OfType<Guna.UI2.WinForms.Guna2HtmlLabel>()
                    .FirstOrDefault(x => x.Name == "guna2HtmlLabel35" || (x.Text != null && x.Text.Contains("Tentang Destinasi")));

                // Bersihkan semua kontrol kecuali judul
                var toRemove = PnlDeskripsi.Controls.Cast<Control>()
                    .Where(c => c != title).ToList();
                foreach (var c in toRemove) PnlDeskripsi.Controls.Remove(c);

                if (title == null)
                {
                    title = new Guna.UI2.WinForms.Guna2HtmlLabel
                    {
                        Name = "lblTitleDeskripsi",
                        Text = "Tentang Destinasi",
                        Location = new Point(8, 8),
                        Font = new Font("Malgun Gothic", 8.25f, FontStyle.Bold),
                        BackColor = Color.Transparent
                    };
                    PnlDeskripsi.Controls.Add(title);
                }

                var lblDesc = new Guna.UI2.WinForms.Guna2HtmlLabel
                {
                    Name = "lblDeskripsiDetail",
                    BackColor = Color.Transparent,
                    Location = new Point(12, title.Bottom + 8),
                    Font = new Font("Malgun Gothic Semilight", 8.25f, FontStyle.Regular)
                };
                lblDesc.Text = string.IsNullOrWhiteSpace(deskripsi) ? "Deskripsi belum tersedia." : deskripsi;
                lblDesc.AutoSize = false;
                lblDesc.AutoSizeHeightOnly = true;
                lblDesc.MaximumSize = new Size(Math.Max(100, PnlDeskripsi.Width - 24), 0);

                PnlDeskripsi.Controls.Add(lblDesc);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error rendering deskripsi: {ex.Message}");
            }
        }

        // Pastikan tabel reviews tersedia
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

        // Muat review dari database dan tampilkan di panel
        private async Task LoadReviewsAsync()
        {
            try
            {
                if (PnlRatingReview == null) return;

                // Bersihkan panel dan tambahkan judul
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
                    var sql = @"SELECT username, rating, review_text, tanggal_review
                                FROM public.reviews
                                WHERE destinasi_id = @did
                                ORDER BY tanggal_review DESC
                                LIMIT 5";
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
                                int rate = reader.GetInt32(1);
                                string text = reader.GetString(2);
                                DateTime dt = reader.GetDateTime(3);

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
                                    Image = SystemIcons.Information.ToBitmap(), // placeholder icon
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

                // AMBIL DARI BINTANG, tidak ada numRating di sini
                int rating = guna2RatingStar1 != null ? (int)guna2RatingStar1.Value : 5;

                using (var conn = new NpgsqlConnection(ConnString))
                {
                    await conn.OpenAsync();

                    // safety: buat tabel kalau belum ada
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

                    // insert review
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

                // refresh daftar review
                await LoadReviewsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal menyimpan ulasan: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2HtmlLabel38_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel36_Click(object sender, EventArgs e)
        {

        }

        private void PnlDeskripsi_Paint(object sender, PaintEventArgs e)
        {

        }

        private void PnlRatingReview_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}