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

        private void DetailDestinasi_Load(object sender, EventArgs e)
        {
            // tampilkan nama & lokasi (kalau ctor param dipakai, nilainya terisi)
            if (!string.IsNullOrWhiteSpace(_destName)) lblDestinasi1.Text = _destName;
            if (!string.IsNullOrWhiteSpace(_destLocation)) lblLokasi1.Text = _destLocation;

            // default rating bintang
            if (guna2RatingStar1 != null) guna2RatingStar1.Value = 5;
            FixLabelWrap(guna2HtmlLabel36);
            FixLabelWrap(guna2HtmlLabel37);
            FixLabelWrap(guna2HtmlLabel38);
            FixLabelWrap(guna2HtmlLabel39);

            // Check if already bookmarked dan ubah icon/warna button
            CheckBookmarkStatus();
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
                        "SELECT COUNT(*) FROM bookmark WHERE user_id = @uid AND destinasi_id = @did", conn))
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

        // Event handler untuk button bookmark
        private async void BtnBookmark_Click(object sender, EventArgs e)
        {
            try
            {
                string status = btnBookmark.Tag?.ToString() ?? "not_bookmarked";

                if (status == "bookmarked")
                {
                    // Hapus bookmark
                    await RemoveBookmark();
                }
                else
                {
                    // Tambah bookmark
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

        // Tambah bookmark ke database
        private async Task AddBookmark()
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnString))
                {
                    await conn.OpenAsync();

                    using (var cmd = new NpgsqlCommand(
                        @"INSERT INTO bookmark (user_id, destinasi_id, tanggal_bookamark)
                          VALUES (@uid, @did, NOW())", conn))
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
                        @"DELETE FROM bookmark WHERE user_id = @uid AND destinasi_id = @did", conn))
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

        private void guna2HtmlLabel38_Click(object sender, EventArgs e)
        {

        }
    }
}