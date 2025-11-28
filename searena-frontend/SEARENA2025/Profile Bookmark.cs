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
        private int selectedBookmarkId = -1; // Menyimpan bookmark yang dipilih
        private FlowLayoutPanel flowBookmarks;
        private List<Destinasi> bookmarkDestinasi = new List<Destinasi>();
        private int selectedDestinasiId = -1;

        public Form3(Form2 parent)
        {
            InitializeComponent();
            parentForm = parent;
            EnsureFlowBookmarks();
            LoadUserProfile();
            LoadBookmarks();
            AttachNavbarEvents();
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

        private void EnsureFlowBookmarks()
        {
            // Create a FlowLayoutPanel to host dynamic cards if not already present
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
                selectedDestinasiId = -1;
                bookmarkDestinasi = Bookmark.GetBookmarksByUserId(UserSession.UserId) ?? new List<Destinasi>();
                guna2HtmlLabel2.Text = $"Riwayat Bookmark ({bookmarkDestinasi.Count})";
                RenderBookmarkCards();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading bookmarks: {ex.Message}");
            }
        }

        private void RenderBookmarkCards()
        {
            flowBookmarks.SuspendLayout();
            flowBookmarks.Controls.Clear();

            foreach (var d in bookmarkDestinasi)
            {
                var card = new DestinasiCard(d)
                {
                    Width = 270,
                    Height = 160,
                    Margin = new Padding(6)
                };
                card.CardClicked += (s, e) => OnCardClicked(card, d.Id);
                flowBookmarks.Controls.Add(card);
            }

            flowBookmarks.ResumeLayout();
        }

        private void OnCardClicked(DestinasiCard card, int destinasiId)
        {
            selectedDestinasiId = destinasiId;
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

            // Try to find internal shadow panel if exists
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
            if (selectedDestinasiId <= 0)
            {
                MessageBox.Show("Silakan pilih destinasi terlebih dahulu", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selected = bookmarkDestinasi.FirstOrDefault(x => x.Id == selectedDestinasiId);
            if (selected == null) return;

            DialogResult result = MessageBox.Show($"Hapus bookmark untuk {selected.NamaDestinasi}?",
                "Konfirmasi Unbookmark", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                await UnbookmarkSelectedAsync(UserSession.UserId, selectedDestinasiId);
                LoadBookmarks();
            }
        }

        private async Task UnbookmarkSelectedAsync(int userId, int destinasiId)
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    if (conn == null) return;

                    using (var cmd = new NpgsqlCommand("DELETE FROM bookmarks WHERE user_id = @uid AND destinasi_id = @did", conn))
                    {
                        cmd.Parameters.AddWithValue("@uid", userId);
                        cmd.Parameters.AddWithValue("@did", destinasiId);
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

        // Stub methods untuk Designer
        private void Navbar_Paint(object sender, PaintEventArgs e) { }
        private void lblProfile_Click_1(object sender, EventArgs e) { }
        private void Profile_Click_1(object sender, EventArgs e) { }
        private void guna2Panel4_Paint(object sender, PaintEventArgs e) { }
        private void lblBergabung_Click(object sender, EventArgs e) { }
        private void Destinasi_Click_1(object sender, EventArgs e) { }
    }
}