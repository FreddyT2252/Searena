using Guna.UI2.WinForms;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static SEARENA2025.DashboardUtama;

namespace SEARENA2025
{
    public partial class Form2 : Form
    {
        private Form dashboardParent;
        private bool _isLoggingOut = false;

        private const string Conn = "Host=aws-1-ap-southeast-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.eeqqiyfukvhbwystupei;Password=SearenaDB123";

        public Form2(Form dashboard = null)
        {
            InitializeComponent();
            dashboardParent = dashboard;
            LoadUserProfile();
            this.FormClosed += Form2_FormClosed;
        }
        private async void LoadUserProfile()
        {
            try
            {
                using (var conn = new Npgsql.NpgsqlConnection("Host=aws-1-ap-southeast-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.eeqqiyfukvhbwystupei;Password=SearenaDB123"))
                {
                    await conn.OpenAsync();

                    using (var cmd = new Npgsql.NpgsqlCommand("SELECT nama_lengkap, email, no_telepon, tanggal_bergabung FROM users WHERE user_id = @uid", conn))
                    {
                        cmd.Parameters.AddWithValue("@uid", UserSession.UserId);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                lblNama.Text = reader["nama_lengkap"]?.ToString() ?? "-";
                                lblEmail.Text = reader["email"]?.ToString() ?? "-";
                                lblTelepon.Text = reader["no_telepon"]?.ToString() ?? "-";
                                guna2HtmlLabel1.Text = reader["nama_lengkap"]?.ToString() ?? "-";
                                lblPengguna.Text = "Pengguna"; // bisa dari role
                                
                                // PERBAIKI: Ambil tanggal bergabung dari database
                                if (!reader.IsDBNull(reader.GetOrdinal("tanggal_bergabung")))
                                {
                                    DateTime tanggalBergabung = reader.GetDateTime(reader.GetOrdinal("tanggal_bergabung"));
                                    
                                    // Format Indonesia: "Bergabung sejak 15 Januari 2024"
                                    System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("id-ID");
                                    lblBergabung.Text = "Bergabung sejak " + tanggalBergabung.ToString("d MMMM yyyy", culture);
                                }
                                else
                                {
                                    lblBergabung.Text = "Bergabung sejak -";
                                }
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

        private void Form2_Load(object sender, System.EventArgs e)
        {
            LoadUserData();
            HideDuplicateGrids();
            LoadUserReviews();
        }
        private void HideDuplicateGrids()
        {
            var all = GetAllGrids(this).ToList();
            DataGridView keep = all.FirstOrDefault(g => g.Name == "dgvRiwayat") ?? all.FirstOrDefault();
            foreach (var g in all)
                g.Visible = (g == keep);
        }
        private IEnumerable<DataGridView> GetAllGrids(Control root)
        {
            foreach (Control c in root.Controls)
            {
                if (c is DataGridView dg) yield return dg;
                foreach (var child in GetAllGrids(c)) yield return child;
            }
        }

        private async void LoadUserReviews()
        {
            try
            {
                using (var conn = new NpgsqlConnection(Conn))
                {
                    await conn.OpenAsync();
                    using (var cmd = new NpgsqlCommand(@"
                SELECT r.review_id AS ""ReviewId"", r.tanggal_review AS ""Tanggal"",
                       COALESCE(d.nama_destinasi, 'ID '||r.destinasi_id::text) AS ""Destinasi"",
                       r.rating AS ""Rating"",
                       r.review_text AS ""Ulasan""
                FROM public.reviews r
                LEFT JOIN public.destinasi d ON d.destinasi_id = r.destinasi_id
                WHERE r.user_id = @uid
                ORDER BY r.tanggal_review DESC;", conn))
                    {
                        cmd.Parameters.AddWithValue("@uid", UserSession.UserId);
                        using (var da = new NpgsqlDataAdapter(cmd))
                        {
                            var dt = new DataTable();
                            da.Fill(dt);

                            // Tambah kolom bintang visual jika belum ada
                            if (!dt.Columns.Contains("⭐ Rating"))
                                dt.Columns.Add("⭐ Rating", typeof(string));

                            foreach (DataRow row in dt.Rows)
                            {
                                int r = 0;
                                if (row["Rating"] != DBNull.Value)
                                    r = Convert.ToInt32(row["Rating"]);

                                // ★ = filled, ☆ = empty
                                row["⭐ Rating"] = new string('★', Math.Max(0, Math.Min(5, r)))
                                                + new string('☆', Math.Max(0, 5 - Math.Max(0, Math.Min(5, r))));
                            }

                            dgvRiwayat.AutoGenerateColumns = false;
                            dgvRiwayat.Columns.Clear();

                            // Kolom ReviewId (disembunyikan untuk operasi hapus)
                            AddTextCol(dgvRiwayat, "ReviewId", "ReviewId");
                            dgvRiwayat.Columns["ReviewId"].Visible = false;

                            AddTextCol(dgvRiwayat, "Tanggal", "Tanggal");
                            AddTextCol(dgvRiwayat, "Destinasi", "Destinasi");
                            AddTextCol(dgvRiwayat, "⭐ Rating", "⭐ Rating");
                            AddTextCol(dgvRiwayat, "Ulasan", "Ulasan");

                            dgvRiwayat.DataSource = dt;
                            dgvRiwayat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                            dgvRiwayat.ReadOnly = true;
                            dgvRiwayat.AllowUserToAddRows = false;

                            // Pakai font yang mendukung simbol ★ ☆
                            dgvRiwayat.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Symbol", 9f);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat riwayat review: " + ex.Message);
            }
        }
        private void AddTextCol(DataGridView dgv, string header, string dataProperty)
        {
            var col = new DataGridViewTextBoxColumn
            {
                HeaderText = header,
                DataPropertyName = dataProperty,
                Name = dataProperty,
                ReadOnly = true
            };
            dgv.Columns.Add(col);
        }
        private void LoadUserData()
        {
            try
            {
                lblNama.Text = string.IsNullOrWhiteSpace(UserSession.Username) ? "Pengguna" : UserSession.Username;
                lblPengguna.Text = "Pengguna";
            }
            catch { }
        }

        // ===== EVENT HANDLERS UNTUK DESIGNER =====

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {
            // Klik pada foto profil di navbar - untuk mengubah foto
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Profile.Image = Image.FromFile(openFileDialog.FileName);
            }
        }

        private void btnBookmark_Click(object sender, EventArgs e)
        {
           

            // Buka Form3 dan tutup Form2
            Form3 form3 = new Form3(this);
            form3.Show();
            this.Hide();
        }

        private void btnLihatLainnya_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Menampilkan semua rating dan review", "Semua Review");
        }

        private void btnRatingReview_Click(object sender, EventArgs e)
        {
            // Tampilkan halaman rating dan review
            if (btnRatingReview != null)
                btnRatingReview.FillColor = Color.LightGreen;
            if (btnBookmark != null)
                btnBookmark.FillColor = Color.FloralWhite;
            MessageBox.Show("Menampilkan halaman rating dan review", "Rating & Review");
        }

        private void guna2HtmlLabel18_Click(object sender, EventArgs e)
        {
            // Klik pada review - tampilkan detail
            MessageBox.Show("Review Pantai Wedi Ombo: Salah satu pantai di Yogyakarta yang akan saya kunjungi berulang kali", "Detail Review");
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {
            // Klik pada nama - mungkin untuk edit profil
            MessageBox.Show("Fitur edit profil akan segera tersedia", "Edit Profil");
        }

        private void guna2HtmlLabel4_Click(object sender, EventArgs e)
        {
            // Navigasi ke halaman Tentang Kami
            MessageBox.Show("Navigasi ke halaman Tentang Kami", "Tentang Kami");
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            // PERBAIKI: Kembali ke parent dashboard jika ada
            if (dashboardParent != null)
            {
                dashboardParent.Show();
                this.Close();
            }
            else
            {
                // Jika tidak ada parent, buka dashboard baru
                DashboardUtama dashboard = new DashboardUtama();
                dashboard.Show();
                this.Close();
            }
        }

        private void lblProfile_Click(object sender, EventArgs e)
        {
            // Sudah di halaman profile, tidak perlu navigasi
            MessageBox.Show("Anda sudah berada di halaman profile", "Profile");
        }

        private void ProfilePage_Click(object sender, EventArgs e)
        {
            // Klik pada foto profil utama - untuk mengubah foto
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ProfilePage.Image = Image.FromFile(openFileDialog.FileName);
            }
        }

        // ===== EVENT HANDLERS NAVIGASI =====

        private void Beranda_Click(object sender, EventArgs e)
        {
            // PERBAIKI: Kembali ke parent dashboard jika ada
            if (dashboardParent != null)
            {
                dashboardParent.Show();
                this.Close();
            }
            else
            {
                // Jika tidak ada parent, buka dashboard baru
                DashboardUtama dashboard = new DashboardUtama();
                dashboard.Show();
                this.Close();
            }
        }

        private void Destinasi_Click(object sender, EventArgs e)
        {
            // PERBAIKI: Kembali ke parent dashboard jika ada
            if (dashboardParent != null)
            {
                dashboardParent.Show();
                this.Close();
            }
            else
            {
                // Jika tidak ada parent, buka dashboard baru
                DashboardUtama dashboard = new DashboardUtama();
                dashboard.Show();
                this.Close();
            }
        }

        private void Kontak_Click(object sender, EventArgs e)
        {
            // Tetap di Form2 tapi scroll ke bagian kontak jika ada
            MessageBox.Show("Navigasi ke Kontak", "Kontak");
        }

        // ===== EVENT HANDLERS UTAMA =====

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
            "Apakah Anda yakin ingin keluar?",
            "Konfirmasi Log Out",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _isLoggingOut = true;
                UserSession.Clear();
                DashboardItem home = new DashboardItem();
                home.Show();
                this.Close();  
            }
        }
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            
            if (_isLoggingOut)
                return;

            
            if (dashboardParent != null && !dashboardParent.IsDisposed)
            {
                dashboardParent.Show();
            }
        }


        private void btnKembali_Click(object sender, EventArgs e)
        {
            
            if (dashboardParent != null)
            {
                dashboardParent.Show();
            }
            else
            {
                
                DashboardUtama dashboard = new DashboardUtama();
                dashboard.Show();
            }
            this.Close();
        }

       

        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bergabung sejak tahun 2021", "Info Bergabung");
        }

        private void guna2HtmlLabel7_Click(object sender, EventArgs e)
        {
            // Event handler untuk label email
        }

        private void guna2HtmlLabel25_Click(object sender, EventArgs e)
        {
            // Event handler tambahan
        }

        private void guna2HtmlLabel26_Click(object sender, EventArgs e)
        {
            // Event handler tambahan
        }

        private void Navbar_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvRiwayat_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private async void btnHapus_Click(object sender, EventArgs e)
        {
            if (dgvRiwayat.CurrentRow == null)
            {
                MessageBox.Show("Pilih dulu salah satu review yang mau dihapus.");
                return;
            }


            var idObj = dgvRiwayat.CurrentRow.Cells["ReviewId"].Value;
            if (idObj == null || idObj == DBNull.Value)
            {
                MessageBox.Show("Review ini tidak punya ID, tidak bisa dihapus.");
                return;
            }

            int reviewId = Convert.ToInt32(idObj);


            var confirm = MessageBox.Show(
                "Yakin mau menghapus review ini?",
                "Konfirmasi Hapus",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                using (var conn = new NpgsqlConnection(Conn))
                {
                    await conn.OpenAsync();
                    using (var cmd = new NpgsqlCommand(
                        "DELETE FROM reviews WHERE review_id = @id AND user_id = @uid", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", reviewId);
                        cmd.Parameters.AddWithValue("@uid", UserSession.UserId);

                        int affected = await cmd.ExecuteNonQueryAsync();

                        if (affected > 0)
                        {
                            MessageBox.Show("Review berhasil dihapus.");
                            LoadUserReviews();
                        }
                        else
                        {
                            MessageBox.Show("Review tidak ditemukan atau bukan milik akun ini.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menghapus review: " + ex.Message);
            }
        }
    }
}