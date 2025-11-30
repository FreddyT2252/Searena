using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Npgsql;
using Guna.UI2.WinForms;

namespace SEARENA2025
{
    public partial class PageAdmin : Form
    {
        
        private static readonly string CONNECTION_STRING = DotNetEnv.Env.GetString("DB_CONNECTION");

        private int selectedDestinasiId = -1;
        private int selectedReviewId = -1;
        private bool _isLoggingOut = false;

        public PageAdmin()
        {
            InitializeComponent();
        }

        private void PageAdmin_Load(object sender, EventArgs e)
        {
            LoadDestinasi();
            LoadReview();

            // Wire up button events
            btnInsert.Click += BtnInsert_Click;
            btnUpdate.Click += BtnUpdate_Click;
            guna2Button3.Click += BtnDelete_Click;
            btnKirim.Click += BtnKirim_Click;
            dgvRiwayat.CellClick += DgvReview_CellClick;
            btnLogout.Click += btnLogout_Click;
            this.FormClosed += PageAdmin_FormClosed;
        }

        private void LoadDestinasi()
        {
            try
            {
                using (var connection = new NpgsqlConnection(CONNECTION_STRING))
                {
                    connection.Open();
                    string query = @"SELECT destinasi_id, nama_destinasi, lokasi, pulau, deskripsi, 
                                           harga_min, harga_max, rating_avg, total_review, waktu_terbaik
                                    FROM destinasi ORDER BY destinasi_id DESC";

                    using (var cmd = new NpgsqlCommand(query, connection))
                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (Controls.Find("dgvDestinasi", true).Length > 0)
                        {
                            ((DataGridView)Controls.Find("dgvDestinasi", true)[0]).DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Load Destinasi: " + ex.Message);
            }
        }

        private void LoadReview()
        {
            try
            {
                using (var connection = new NpgsqlConnection(CONNECTION_STRING))
                {
                    connection.Open();
                    string query = @"SELECT review_id, username, dest_name, dest_location, 
                                   rating, review_text, tanggal_review
                            FROM reviews
                            WHERE response IS NULL OR response = ''
                            ORDER BY tanggal_review DESC";

                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            dgvRiwayat.Rows.Clear();

                            while (reader.Read())
                            {
                                int reviewId = reader.GetInt32(0);
                                string username = reader.IsDBNull(1) ? "-" : reader.GetString(1);
                                string destinasi = reader.IsDBNull(2) ? "-" : reader.GetString(2);
                                string lokasi = reader.IsDBNull(3) ? "-" : reader.GetString(3);
                                int rating = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                                string review = reader.IsDBNull(5) ? "-" : reader.GetString(5);
                                string ratingStars = new string('★', Math.Max(0, Math.Min(5, rating)))
                                                   + new string('☆', Math.Max(0, 5 - Math.Max(0, Math.Min(5, rating))));

                                // Tambahkan row: Username | Destinasi | Lokasi | Rating | Review
                                int rowIdx = dgvRiwayat.Rows.Add(username, destinasi, lokasi, ratingStars, review);
                                dgvRiwayat.Rows[rowIdx].Tag = reviewId;
                            }

                            dgvRiwayat.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Symbol", 9f);
                            dgvRiwayat.ReadOnly = true;
                            dgvRiwayat.AllowUserToAddRows = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Load Review: " + ex.Message);
            }
        }

        private void BtnInsert_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNamaDestinasi.Text))
            {
                MessageBox.Show("Nama destinasi harus diisi!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDeskripsi.Text))
            {
                MessageBox.Show("Deskripsi harus diisi!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtAlamatDestinasi.Text))
            {
                MessageBox.Show("Lokasi harus diisi!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPulau.Text))
            {
                MessageBox.Show("Pulau harus diisi!");
                return;
            }

            // Validasi harga min
            string hargaMinText = txtTiketMin.Text.Trim().Replace(".", "").Replace(",", "");
            if (!int.TryParse(hargaMinText, out int hargaMin))
            {
                MessageBox.Show("Harga minimum harus berupa angka yang valid!");
                return;
            }

            // Validasi harga max
            string hargaMaxText = txtTiketMax.Text.Trim().Replace(".", "").Replace(",", "");
            if (!int.TryParse(hargaMaxText, out int hargaMax))
            {
                MessageBox.Show("Harga maksimum harus berupa angka yang valid!");
                return;
            }

            string waktuTerbaik = GetSelectedWaktu();
            if (string.IsNullOrWhiteSpace(waktuTerbaik))
            {
                MessageBox.Show("Pilih minimal satu bulan untuk waktu terbaik!");
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection(CONNECTION_STRING))
                {
                    connection.Open();

                    // Cek duplikasi nama destinasi (case-insensitive)
                    using (var check = new NpgsqlCommand("SELECT COUNT(*) FROM destinasi WHERE LOWER(nama_destinasi)=LOWER(@nama)", connection))
                    {
                        check.Parameters.AddWithValue("@nama", txtNamaDestinasi.Text.Trim());
                        var exists = Convert.ToInt64(check.ExecuteScalar());
                        if (exists > 0)
                        {
                            MessageBox.Show("Destinasi dengan nama tersebut sudah ada (duplikasi ditolak).", "Duplikasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    string query = @"INSERT INTO destinasi (nama_destinasi, deskripsi, lokasi, pulau, 
                                                           harga_min, harga_max, rating_avg, total_review, waktu_terbaik) 
                                     VALUES (@nama, @deskripsi, @lokasi, @pulau, @harga_min, @harga_max, 
                                             0, 0, @waktu)";

                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@nama", txtNamaDestinasi.Text.Trim());
                        cmd.Parameters.AddWithValue("@deskripsi", txtDeskripsi.Text.Trim());
                        cmd.Parameters.AddWithValue("@lokasi", txtAlamatDestinasi.Text.Trim());
                        cmd.Parameters.AddWithValue("@pulau", txtPulau.Text.Trim());
                        cmd.Parameters.AddWithValue("@harga_min", hargaMin);
                        cmd.Parameters.AddWithValue("@harga_max", hargaMax);
                        cmd.Parameters.AddWithValue("@waktu", waktuTerbaik);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Destinasi berhasil ditambahkan!");
                        ClearForm();
                        LoadDestinasi();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Insert: " + ex.Message);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            // Validasi semua field harus diisi
            if (string.IsNullOrWhiteSpace(txtNamaDestinasi.Text))
            {
                MessageBox.Show("Nama destinasi harus diisi!");
                txtNamaDestinasi.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDeskripsi.Text))
            {
                MessageBox.Show("Deskripsi harus diisi!");
                txtDeskripsi.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtAlamatDestinasi.Text))
            {
                MessageBox.Show("Alamat destinasi harus diisi!");
                txtAlamatDestinasi.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPulau.Text))
            {
                MessageBox.Show("Pulau harus diisi!");
                txtPulau.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtRekomendasiCuaca.Text))
            {
                MessageBox.Show("Rekomendasi cuaca harus diisi!");
                txtRekomendasiCuaca.Focus();
                return;
            }

            // Validasi harga min
            string hargaMinText = txtTiketMin.Text.Trim().Replace(".", "").Replace(",", "");
            if (!int.TryParse(hargaMinText, out int hargaMin))
            {
                MessageBox.Show("Harga minimum harus berupa angka yang valid!");
                txtTiketMin.Focus();
                return;
            }

            // Validasi harga max
            string hargaMaxText = txtTiketMax.Text.Trim().Replace(".", "").Replace(",", "");
            if (!int.TryParse(hargaMaxText, out int hargaMax))
            {
                MessageBox.Show("Harga maksimum harus berupa angka yang valid!");
                txtTiketMax.Focus();
                return;
            }

            // Validasi harga min < harga max
            if (hargaMin > hargaMax)
            {
                MessageBox.Show("Harga minimum tidak boleh lebih besar dari harga maksimum!");
                return;
            }

            string waktuTerbaik = GetSelectedWaktu();
            if (string.IsNullOrWhiteSpace(waktuTerbaik))
            {
                MessageBox.Show("Pilih minimal satu bulan untuk waktu terbaik!");
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection(CONNECTION_STRING))
                {
                    connection.Open();

                    // Cek apakah destinasi dengan nama ini ada di database
                    string checkQuery = "SELECT destinasi_id FROM destinasi WHERE nama_destinasi = @nama";
                    using (var checkCmd = new NpgsqlCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@nama", txtNamaDestinasi.Text.Trim());
                        var result = checkCmd.ExecuteScalar();

                        if (result == null)
                        {
                            MessageBox.Show("Destinasi dengan nama '" + txtNamaDestinasi.Text.Trim() + "' tidak ditemukan!");
                            return;
                        }

                        int destinasiId = Convert.ToInt32(result);

                        // Hitung rating dan total review dari tabel reviews
                        decimal avgRating = 0;
                        int totalReviews = 0;

                        string statsQuery = @"SELECT 
                                                COALESCE(AVG(rating)::numeric, 0) AS avg_rating,
                                                COUNT(*) AS total_reviews
                                             FROM reviews 
                                             WHERE dest_name = @nama";

                        using (var statsCmd = new NpgsqlCommand(statsQuery, connection))
                        {
                            statsCmd.Parameters.AddWithValue("@nama", txtNamaDestinasi.Text.Trim());
                            using (var reader = statsCmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    avgRating = Convert.ToDecimal(reader["avg_rating"]);
                                    totalReviews = Convert.ToInt32(reader["total_reviews"]);
                                }
                            }
                        }

                        // Update destinasi
                        string updateQuery = @"UPDATE destinasi SET deskripsi=@deskripsi, 
                                     lokasi=@lokasi, pulau=@pulau, harga_min=@harga_min, harga_max=@harga_max, 
                                     rating_avg=@rating, total_review=@total_review, waktu_terbaik=@waktu 
                                     WHERE destinasi_id=@id";

                        using (var updateCmd = new NpgsqlCommand(updateQuery, connection))
                        {
                            updateCmd.Parameters.AddWithValue("@id", destinasiId);
                            updateCmd.Parameters.AddWithValue("@deskripsi", txtDeskripsi.Text.Trim());
                            updateCmd.Parameters.AddWithValue("@lokasi", txtAlamatDestinasi.Text.Trim());
                            updateCmd.Parameters.AddWithValue("@pulau", txtPulau.Text.Trim());
                            updateCmd.Parameters.AddWithValue("@harga_min", hargaMin);
                            updateCmd.Parameters.AddWithValue("@harga_max", hargaMax);
                            updateCmd.Parameters.AddWithValue("@rating", avgRating);
                            updateCmd.Parameters.AddWithValue("@total_review", totalReviews);
                            updateCmd.Parameters.AddWithValue("@waktu", waktuTerbaik);

                            updateCmd.ExecuteNonQuery();
                            MessageBox.Show($"Destinasi berhasil diupdate!\nRating: {avgRating:F2} | Total Review: {totalReviews}");
                            ClearForm();
                            LoadDestinasi();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Update: " + ex.Message);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            // Validasi nama destinasi harus diisi 
            if (string.IsNullOrWhiteSpace(txtNamaDestinasi.Text))
            {
                MessageBox.Show("Nama destinasi harus diisi untuk menghapus!");
                txtNamaDestinasi.Focus();
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection(CONNECTION_STRING))
                {
                    connection.Open();

                    // Cek apakah destinasi dengan nama ini ada di database
                    string checkQuery = "SELECT destinasi_id FROM destinasi WHERE nama_destinasi = @nama";
                    using (var checkCmd = new NpgsqlCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@nama", txtNamaDestinasi.Text.Trim());
                        var result = checkCmd.ExecuteScalar();

                        if (result == null)
                        {
                            MessageBox.Show("Destinasi dengan nama '" + txtNamaDestinasi.Text.Trim() + "' tidak ditemukan!");
                            return;
                        }

                        int destinasiId = Convert.ToInt32(result);

                        // Konfirmasi delete
                        if (MessageBox.Show("Yakin ingin menghapus destinasi '" + txtNamaDestinasi.Text.Trim() + "'?",
                            "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            return;

                        // Delete destinasi
                        string deleteQuery = "DELETE FROM destinasi WHERE destinasi_id=@id";
                        using (var deleteCmd = new NpgsqlCommand(deleteQuery, connection))
                        {
                            deleteCmd.Parameters.AddWithValue("@id", destinasiId);
                            deleteCmd.ExecuteNonQuery();
                            MessageBox.Show("Destinasi berhasil dihapus!");
                            ClearForm();
                            LoadDestinasi();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Delete: " + ex.Message);
            }
        }

        private void BtnKirim_Click(object sender, EventArgs e)
        {
            if (selectedReviewId == -1)
            {
                MessageBox.Show("Pilih review yang ingin dibalas!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtBalasReview.Text))
            {
                MessageBox.Show("Balasan tidak boleh kosong!");
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection(CONNECTION_STRING))
                {
                    connection.Open();
                    // Sesuaikan nama column dengan struktur DB 
                    string query = "UPDATE reviews SET response=@response WHERE review_id=@id";

                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", selectedReviewId);
                        cmd.Parameters.AddWithValue("@response", txtBalasReview.Text.Trim());

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Balasan berhasil dikirim!");
                        txtBalasReview.Clear();
                        selectedReviewId = -1;
                        LoadReview(); 
                        LoadDestinasi(); 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Balas Review: " + ex.Message);
            }
        }

        private void DgvReview_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvRiwayat.Rows[e.RowIndex].Tag != null)
            {
                selectedReviewId = Convert.ToInt32(dgvRiwayat.Rows[e.RowIndex].Tag);
                if (e.ColumnIndex >= 0 && dgvRiwayat.Rows[e.RowIndex].Cells.Count > 4)
                {
                    var reviewText = dgvRiwayat.Rows[e.RowIndex].Cells[4].Value?.ToString();
                }
            }
        }
        private void PageAdmin_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_isLoggingOut)
                return;
            var home = new DashboardItem();
            home.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
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
                var home = new DashboardItem();
                home.Show();
                this.Close();
            }
        }

        private string GetSelectedWaktu()
        {
            List<string> waktu = new List<string>();

            
            if (cbJan.Checked) waktu.Add("Januari");
            if (cbFeb.Checked) waktu.Add("Februari");
            if (cbMar.Checked) waktu.Add("Maret");
            if (cbApr.Checked) waktu.Add("April");
            if (cbMei.Checked) waktu.Add("Mei");
            if (cbJun.Checked) waktu.Add("Juni");
            if (cbJul.Checked) waktu.Add("Juli");
            if (cbAug.Checked) waktu.Add("Agustus");
            if (cbSept.Checked) waktu.Add("September");
            if (cbOkt.Checked) waktu.Add("Oktober");
            if (cbNov.Checked) waktu.Add("November");
            if (cbDec.Checked) waktu.Add("Desember");

            return string.Join(", ", waktu);
        }

        private void ClearForm()
        {
            txtNamaDestinasi.Clear();
            txtDeskripsi.Clear();
            txtAlamatDestinasi.Clear();
            txtPulau.Clear();
            txtTiketMin.Clear();
            txtTiketMax.Clear();
            txtRekomendasiCuaca.Clear();

            UncheckControl("cbJan");
            UncheckControl("cbFeb");
            UncheckControl("cbMar");
            UncheckControl("cbApr");
            UncheckControl("cbMei");
            UncheckControl("cbJun");
            UncheckControl("cbJul");
            UncheckControl("cbAug");
            UncheckControl("cbSept");
            UncheckControl("cbOkt");
            UncheckControl("cbNov");
            UncheckControl("cbDec");

            UncheckControl("cbSnorkling");
            UncheckControl("cbDiving");
            UncheckControl("cbSunset");
            UncheckControl("cbCamping");

            selectedDestinasiId = -1;
            txtBalasReview.Clear();
        }

        private void UncheckControl(string controlName)
        {
            var controls = Controls.Find(controlName, true);
            if (controls.Length > 0 && controls[0] is CheckBox cb)
            {
                cb.Checked = false;
            }
        }

        private void Navbar_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}