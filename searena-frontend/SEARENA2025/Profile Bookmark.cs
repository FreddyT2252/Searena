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
        private string connectionString = "Host=aws-1-ap-southeast-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.eeqqiyfukvhbwystupei;Password=SearenaDB123";
        private List<BookmarkData> bookmarkList = new List<BookmarkData>();
        private FlowLayoutPanel flowBookmarks;
        private List<BookmarkWrapper> bookmarkWrappers = new List<BookmarkWrapper>();
        private int selectedBookmarkId = -1;

        private class BookmarkWrapper
        {
            public int BookmarkId { get; set; }
            public Destinasi Destinasi { get; set; }
        }

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

        public Form3(Form2 parent)
        {
            InitializeComponent();
            parentForm = parent;
            EnsureFlowBookmarks();
            LoadUserProfile();
            LoadBookmarks();
            AttachNavbarEvents();
        }

        private void EnsureFlowBookmarks()
        {
            flowBookmarks = new FlowLayoutPanel
            {
                Name = "flowBookmarks",
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Color.FloralWhite,
                Location = new Point(512, 200),
                Size = new Size(566, 430),
                Padding = new Padding(4),
                Margin = new Padding(0)
            };
            this.Controls.Add(flowBookmarks);
            flowBookmarks.BringToFront();
        }

        private async void LoadUserProfile()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    if (conn == null) return;

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
                                var bergabung = reader.IsDBNull(reader.GetOrdinal("tanggal_bergabung")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("tanggal_bergabung"));
                                if (bergabung.HasValue)
                                    lblBergabung.Text = "Bergabung sejak " + bergabung.Value.ToString("d MMMM yyyy");
                                else
                                    lblBergabung.Text = "Bergabung sejak -";
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

        private void LoadBookmarks()
        {
            try
            {
                selectedBookmarkId = -1;
                List<Destinasi> bookmarkDestinasi = Bookmark.GetBookmarksByUserId(UserSession.UserId) ?? new List<Destinasi>();

                DebugLog($"LoadBookmarks: Ditemukan {bookmarkDestinasi.Count} destinasi");

                LoadBookmarkDetailsWithIds(bookmarkDestinasi);

                DebugLog($"LoadBookmarks: {bookmarkWrappers.Count} wrapper siap ditampilkan");
                guna2HtmlLabel2.Text = $"Riwayat Bookmark ({bookmarkWrappers.Count})";
                RenderBookmarkCards();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading bookmarks: {ex.Message}");
            }
        }

        private void LoadBookmarkDetailsWithIds(List<Destinasi> destinasiList)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    bookmarkWrappers.Clear();

                    DebugLog($"LoadBookmarkDetailsWithIds: Membuka koneksi untuk {destinasiList.Count} destinasi");

                    foreach (var destinasi in destinasiList)
                    {
                        using (var cmd = new NpgsqlCommand(
                            "SELECT bookmark_id FROM bookmarks WHERE user_id = @uid AND destinasi_id = @did LIMIT 1",
                            conn))
                        {
                            cmd.Parameters.AddWithValue("@uid", UserSession.UserId);
                            cmd.Parameters.AddWithValue("@did", destinasi.Id);

                            var result = cmd.ExecuteScalar();

                            if (result != null && int.TryParse(result.ToString(), out int bookmarkId))
                            {
                                DebugLog($"  - Found BookmarkID {bookmarkId} untuk Destinasi {destinasi.Id} ({destinasi.NamaDestinasi})");

                                bookmarkWrappers.Add(new BookmarkWrapper
                                {
                                    BookmarkId = bookmarkId,
                                    Destinasi = destinasi
                                });
                            }
                            else
                            {
                                DebugLog($"  - GAGAL: BookmarkID tidak ditemukan untuk Destinasi {destinasi.Id}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading bookmark IDs: {ex.Message}\n\n{ex.StackTrace}");
                DebugLog($"ERROR LoadBookmarkDetailsWithIds: {ex.Message}");
            }
        }

        private void RenderBookmarkCards()
        {
            flowBookmarks.SuspendLayout();
            flowBookmarks.Controls.Clear();

            DebugLog($"RenderBookmarkCards: Rendering {bookmarkWrappers.Count} cards");

            foreach (var wrapper in bookmarkWrappers)
            {
                var card = new DestinasiCard(wrapper.Destinasi)
                {
                    Width = 270,
                    Height = 160,
                    Margin = new Padding(6),
                    Tag = wrapper.BookmarkId
                };
                card.CardClicked += (s, e) => OnCardClicked(card, wrapper.BookmarkId);
                flowBookmarks.Controls.Add(card);
            }

            flowBookmarks.ResumeLayout();
            DebugLog($"RenderBookmarkCards: Selesai - Total {flowBookmarks.Controls.Count} card ditampilkan");
        }

        private void OnCardClicked(DestinasiCard card, int bookmarkId)
        {
            selectedBookmarkId = bookmarkId;
            DebugLog($"OnCardClicked: Selected BookmarkID = {bookmarkId}");
            HighlightSelectedCard(card);
        }

        private void HighlightSelectedCard(DestinasiCard selected)
        {
            foreach (Control ctrl in flowBookmarks.Controls)
            {
                if (ctrl is Guna2ShadowPanel sp)
                {
                    sp.BackColor = Color.White;
                }
                else
                {
                    ctrl.BackColor = Color.White;
                }
            }

            selected.BackColor = Color.LightYellow;
        }

        private void btnBookmark_Click(object sender, EventArgs e)
        {
            btnBookmark.FillColor = Color.LightGreen;
            btnRatingReview.FillColor = Color.FloralWhite;
            LoadBookmarks();
        }

        private async void btnUnbookmark_Click(object sender, EventArgs e)
        {
            DebugLog("=== btnUnbookmark_Click START ===");
            DebugLog($"selectedBookmarkId = {selectedBookmarkId}");

            if (selectedBookmarkId <= 0)
            {
                DebugLog("ABORT: selectedBookmarkId <= 0");
                MessageBox.Show("Silakan pilih destinasi terlebih dahulu", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selected = bookmarkWrappers.FirstOrDefault(x => x.BookmarkId == selectedBookmarkId);

            DebugLog($"Looking for BookmarkID {selectedBookmarkId} in {bookmarkWrappers.Count} wrappers");

            if (selected == null)
            {
                DebugLog("ABORT: selected wrapper is NULL");
                MessageBox.Show("Bookmark tidak ditemukan", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DebugLog($"Found: {selected.Destinasi.NamaDestinasi}");

            DialogResult result = MessageBox.Show($"Hapus bookmark untuk {selected.Destinasi.NamaDestinasi}?",
                "Konfirmasi Unbookmark", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            DebugLog($"User pilih: {result}");

            if (result == DialogResult.Yes)
            {
                DebugLog($"Calling UnbookmarkSelectedAsync dengan BookmarkID: {selectedBookmarkId}");
                await UnbookmarkSelectedAsync(selectedBookmarkId);
                LoadBookmarks();
                DebugLog("=== btnUnbookmark_Click END (berhasil) ===");
            }
            else
            {
                DebugLog("=== btnUnbookmark_Click END (dibatalkan user) ===");
            }
        }

        private async Task UnbookmarkSelectedAsync(int bookmarkId)
        {
            DebugLog("=== UnbookmarkSelectedAsync START ===");
            DebugLog($"BookmarkID to delete: {bookmarkId}");

            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    DebugLog("Opening connection...");
                    await conn.OpenAsync();
                    DebugLog("Connection opened successfully");

                    // Query untuk cek apakah bookmark ada
                    using (var checkCmd = new NpgsqlCommand("SELECT COUNT(*) FROM bookmarks WHERE bookmark_id = @bid", conn))
                    {
                        checkCmd.Parameters.AddWithValue("@bid", bookmarkId);
                        long count = (long)await checkCmd.ExecuteScalarAsync();
                        DebugLog($"CHECK: Found {count} record(s) dengan bookmark_id = {bookmarkId}");
                    }

                    // DELETE
                    using (var cmd = new NpgsqlCommand("DELETE FROM bookmarks WHERE bookmark_id = @bid", conn))
                    {
                        cmd.Parameters.AddWithValue("@bid", bookmarkId);
                        DebugLog("Executing DELETE query...");

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        DebugLog($"DELETE result: {rowsAffected} row(s) affected");

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("✓ Bookmark berhasil dihapus!", "Sukses",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            DebugLog("SUCCESS: Bookmark deleted");
                        }
                        else
                        {
                            MessageBox.Show("Bookmark tidak ditemukan atau sudah terhapus sebelumnya",
                                "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            DebugLog("INFO: No rows affected");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DebugLog($"EXCEPTION: {ex.GetType().Name}");
                DebugLog($"Message: {ex.Message}");
                DebugLog($"StackTrace: {ex.StackTrace}");
                MessageBox.Show($"Gagal menghapus bookmark: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            DebugLog("=== UnbookmarkSelectedAsync END ===");
        }

        private void DebugLog(string message)
        {
            System.Diagnostics.Debug.WriteLine($"[Form3] {DateTime.Now:HH:mm:ss.fff} - {message}");
        }

        public void AttachNavbarEvents()
        {
            if (lblProfile != null) lblProfile.Click += lblProfile_Click;
            if (guna2PictureBox1 != null) guna2PictureBox1.Click += Logo_Click;
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

        private void Navbar_Paint(object sender, PaintEventArgs e) { }
        private void lblProfile_Click_1(object sender, EventArgs e) { }
        private void Profile_Click_1(object sender, EventArgs e) { }
        private void guna2Panel4_Paint(object sender, PaintEventArgs e) { }
        private void lblBergabung_Click(object sender, EventArgs e) { }
        private void Destinasi_Click_1(object sender, EventArgs e) { }
    }
}