using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Npgsql;

namespace SEARENA2025
{
    public partial class Form3 : Form
    {
        private Form2 parentForm;
        private string connectionString = "Host=localhost;Port=5432;Database=searena_db;Username=postgres;Password=Putriananev2412";
        private List<BookmarkData> bookmarkList = new List<BookmarkData>();
        private int selectedBookmarkId = -1; // Menyimpan bookmark yang dipilih

        public Form3(Form2 parent)
        {
            InitializeComponent();
            parentForm = parent;
            LoadUserProfile();
            LoadBookmarkData();
            AttachNavbarEvents();
            AttachCardClickEvents();
        }

        // Class untuk menyimpan data bookmark
        private class BookmarkData
        {
            public int BookmarkId { get; set; }
            public int DestinasiId { get; set; }
            public string NamaDestinasi { get; set; }
            public string Lokasi { get; set; }
            public string Pulau { get; set; }
            public string Deskripsi { get; set; }
            public decimal HargaMin { get; set; }
            public decimal HargaMax { get; set; }
            public string WaktuTerbaik { get; set; }
            public decimal RatingAvg { get; set; }
            public int TotalReview { get; set; }
            public string Activity { get; set; }
        }

        private async void LoadUserProfile()
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    using (var cmd = new NpgsqlCommand("SELECT nama_lengkap, email, no_telepon, tanggal_bergabung FROM users WHERE user_id = @uid", conn))
                    {
                        cmd.Parameters.AddWithValue("@uid", UserSession.UserId);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                lblNama.Text = reader["nama_lengkap"]?.ToString() ?? "-";
                                lblEmail.Text = reader["email"]?.ToString() ?? "-";
                                guna2HtmlLabel1.Text = reader["nama_lengkap"]?.ToString() ?? "-";
                                lblPengguna.Text = "Pengguna";
                                lblBergabung.Text = "Bergabung sejak " + ((DateTime)reader["tanggal_bergabung"]).ToString("d MMMM yyyy");
                            }
                            else
                            {
                                MessageBox.Show("User tidak ditemukan.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data profil: " + ex.Message);
            }
        }

        private async void LoadBookmarkData()
        {
            try
            {
                bookmarkList.Clear();

                using (var conn = new NpgsqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    using (var cmd = new NpgsqlCommand(@"
                        SELECT b.bookmark_id, b.destinasi_id, d.nama_destinasi, d.lokasi, d.pulau, d.deskripsi, 
                               d.harga_min, d.harga_max, d.waktu_terbaik, d.rating_avg, d.total_review, d.activity
                        FROM bookmark b
                        INNER JOIN destinasi d ON b.destinasi_id = d.destinasi_id
                        WHERE b.user_id = @uid
                        ORDER BY b.tanggal_bookamark DESC", conn))
                    {
                        cmd.Parameters.AddWithValue("@uid", UserSession.UserId);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                bookmarkList.Add(new BookmarkData
                                {
                                    BookmarkId = (int)reader["bookmark_id"],
                                    DestinasiId = (int)reader["destinasi_id"],
                                    NamaDestinasi = reader["nama_destinasi"]?.ToString() ?? "-",
                                    Lokasi = reader["lokasi"]?.ToString() ?? "-",
                                    Pulau = reader["pulau"]?.ToString() ?? "-",
                                    Deskripsi = reader["deskripsi"]?.ToString() ?? "-",
                                    HargaMin = reader["harga_min"] != DBNull.Value ? (decimal)reader["harga_min"] : 0,
                                    HargaMax = reader["harga_max"] != DBNull.Value ? (decimal)reader["harga_max"] : 0,
                                    WaktuTerbaik = reader["waktu_terbaik"]?.ToString() ?? "-",
                                    RatingAvg = reader["rating_avg"] != DBNull.Value ? (decimal)reader["rating_avg"] : 0,
                                    TotalReview = reader["total_review"] != DBNull.Value ? (int)reader["total_review"] : 0,
                                    Activity = reader["activity"]?.ToString() ?? "-"
                                });
                            }
                        }
                    }
                }

                DisplayBookmarks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading bookmark data: {ex.Message}");
            }
        }

        private void DisplayBookmarks()
        {
            panelDestinasi1.Visible = false;
            panelDestinasi2.Visible = false;

            if (bookmarkList.Count >= 1)
            {
                panelDestinasi1.Visible = true;
                SetBookmarkUI(bookmarkList[0], 1);
            }

            if (bookmarkList.Count >= 2)
            {
                panelDestinasi2.Visible = true;
                SetBookmarkUI(bookmarkList[1], 2);
            }

            selectedBookmarkId = -1; // Reset selection
            ResetCardHighlight();
        }

        private void SetBookmarkUI(BookmarkData bookmark, int position)
        {
            if (position == 1)
            {
                lblDestinasi1.Text = bookmark.NamaDestinasi;
                lblLokasi1.Text = $"{bookmark.Pulau}, {bookmark.Lokasi}";
                lblDeskripsi1.Text = bookmark.Deskripsi;
                lblFasilitas1.Text = bookmark.Activity;
                lblWaktu1.Text = "Terbaik: " + bookmark.WaktuTerbaik;
                panelDestinasi1.Tag = bookmark.BookmarkId;
            }
            else if (position == 2)
            {
                lblDestinasi2.Text = bookmark.NamaDestinasi;
                lblLokasi2.Text = $"{bookmark.Pulau}, {bookmark.Lokasi}";
                lblDeskripsi2.Text = bookmark.Deskripsi;
                lblFasilitas2.Text = bookmark.Activity;
                lblWaktu2.Text = "Terbaik: " + bookmark.WaktuTerbaik;
                panelDestinasi2.Tag = bookmark.BookmarkId;
            }
        }

        // Attach click events ke card untuk selection
        private void AttachCardClickEvents()
        {
            panelDestinasi1.Click += (s, e) => SelectCard(panelDestinasi1, 1);
            panelDestinasi2.Click += (s, e) => SelectCard(panelDestinasi2, 2);

            // Juga attach ke semua child controls untuk propagate event
            AttachClickToChildren(panelDestinasi1, (s, e) => SelectCard(panelDestinasi1, 1));
            AttachClickToChildren(panelDestinasi2, (s, e) => SelectCard(panelDestinasi2, 2));
        }

        private void AttachClickToChildren(Control parent, EventHandler handler)
        {
            foreach (Control child in parent.Controls)
            {
                child.Click += handler;
                AttachClickToChildren(child, handler);
            }
        }

        private void SelectCard(Control card, int position)
        {
            if (card.Tag == null) return;

            int bookmarkId = (int)card.Tag;
            selectedBookmarkId = bookmarkId;

            ResetCardHighlight();
            HighlightCard(card);
        }

        private void HighlightCard(Control card)
        {
            if (card is Guna2ShadowPanel shadowPanel)
            {
                shadowPanel.BackColor = Color.LightYellow; // Highlight dengan warna kuning
            }
        }

        private void ResetCardHighlight()
        {
            if (panelDestinasi1 is Guna2ShadowPanel panel1)
            {
                panel1.BackColor = Color.White;
            }
            if (panelDestinasi2 is Guna2ShadowPanel panel2)
            {
                panel2.BackColor = Color.White;
            }
        }

        private void Logo_Click(object sender, EventArgs e)
        {
            DashboardUtama dashboard = new DashboardUtama();
            dashboard.Show();
            this.Close();
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            if (parentForm != null)
                parentForm.Close();

            DashboardUtama dashboard = new DashboardUtama();
            dashboard.Show();
            this.Close();
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah Anda yakin ingin keluar?", "Konfirmasi Log Out",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnBookmark_Click(object sender, EventArgs e)
        {
            btnBookmark.FillColor = Color.LightGreen;
            btnRatingReview.FillColor = Color.FloralWhite;
            LoadBookmarkData();
        }

        private void Beranda_Click(object sender, EventArgs e)
        {
            DashboardUtama dashboard = new DashboardUtama();
            dashboard.Show();
            this.Close();
        }

        private void Destinasi_Click(object sender, EventArgs e)
        {
            DashboardUtama dashboard = new DashboardUtama();
            dashboard.Show();
            this.Close();
        }

        private void Kontak_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Navigasi ke Kontak", "Kontak");
        }

        private void TentangKami_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Navigasi ke Tentang Kami", "Tentang Kami");
        }

        private void lblProfile_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Anda sudah berada di halaman profile", "Profile");
        }

        // Button unbookmark - dihapus ketika card sudah dipilih
        private async void btnUnbookmark_Click(object sender, EventArgs e)
        {
            if (selectedBookmarkId == -1)
            {
                MessageBox.Show("Silakan pilih destinasi terlebih dahulu", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            BookmarkData selected = bookmarkList.FirstOrDefault(b => b.BookmarkId == selectedBookmarkId);
            if (selected == null) return;

            DialogResult result = MessageBox.Show($"Hapus bookmark untuk {selected.NamaDestinasi}?",
                "Konfirmasi Unbookmark", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                await DeleteBookmarkFromDatabase(selectedBookmarkId);
                LoadBookmarkData();
            }
        }

        private async Task DeleteBookmarkFromDatabase(int bookmarkId)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    using (var cmd = new NpgsqlCommand("DELETE FROM bookmark WHERE bookmark_id = @bid", conn))
                    {
                        cmd.Parameters.AddWithValue("@bid", bookmarkId);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                MessageBox.Show("Bookmark berhasil dihapus", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal menghapus bookmark: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AttachNavbarEvents()
        {
            if (lblProfile != null) lblProfile.Click += lblProfile_Click;
            if (guna2PictureBox1 != null) guna2PictureBox1.Click += Logo_Click;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
        }

        private void Profile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Profile.Image = Image.FromFile(openFileDialog.FileName);
            }
        }

        private void ProfilePage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ProfilePage.Image = Image.FromFile(openFileDialog.FileName);
            }
        }

        private void btnRatingReview_Click_1(object sender, EventArgs e)
        {
            if (parentForm != null)
            {
                parentForm.Show();
                this.Close();
            }
        }

        private void btnKembali_Click_1(object sender, EventArgs e)
        {
            if (parentForm != null)
                parentForm.Close();

            DashboardUtama dashboard = new DashboardUtama();
            dashboard.Show();
            this.Close();
        }

        // Stub methods untuk Designer
        private void Navbar_Paint(object sender, PaintEventArgs e) { }
        private void lblProfile_Click_1(object sender, EventArgs e) { }
        private void Profile_Click_1(object sender, EventArgs e) { }
        private void guna2Panel4_Paint(object sender, PaintEventArgs e) { }
        private void lblBergabung_Click(object sender, EventArgs e) { }
        private void Destinasi_Click_1(object sender, EventArgs e) { }
    }
}