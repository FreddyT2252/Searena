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
        private const string CONNECTION_STRING = "Host=aws-1-ap-southeast-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.eeqqiyfukvhbwystupei;Password=SearenaDB123";

        private int selectedDestinasiId = -1;
        private int selectedReviewId = -1;

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
                    // Query diperbaiki sesuai struktur DB yang benar
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

                                // Buat string bintang
                                string ratingStars = new string('★', Math.Max(0, Math.Min(5, rating)))
                                                   + new string('☆', Math.Max(0, 5 - Math.Max(0, Math.Min(5, rating))));

                                // Tambahkan row: Username | Destinasi | Lokasi | Rating | Review
                                int rowIdx = dgvRiwayat.Rows.Add(username, destinasi, lokasi, ratingStars, review);
                                dgvRiwayat.Rows[rowIdx].Tag = reviewId;
                            }

                            // Font untuk mendukung bintang
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
            // Validasi input wajib
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

            // Validasi rating
            string ratingText = txtRating.Text.Trim();
            if (!float.TryParse(ratingText, out float rating))
            {
                MessageBox.Show("Rating harus berupa angka yang valid!");
                return;
            }

            // Validasi total review
            string totalReviewText = txtTotalReview.Text.Trim();
            if (!int.TryParse(totalReviewText, out int totalReview))
            {
                MessageBox.Show("Total review harus berupa angka yang valid!");
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
                    string query = @"INSERT INTO destinasi (nama_destinasi, deskripsi, lokasi, pulau, 
                                                           harga_min, harga_max, rating_avg, total_review, waktu_terbaik) 
                                     VALUES (@nama, @deskripsi, @lokasi, @pulau, @harga_min, @harga_max, 
                                             @rating, @total_review, @waktu)";

                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@nama", txtNamaDestinasi.Text.Trim());
                        cmd.Parameters.AddWithValue("@deskripsi", txtDeskripsi.Text.Trim());
                        cmd.Parameters.AddWithValue("@lokasi", txtAlamatDestinasi.Text.Trim());
                        cmd.Parameters.AddWithValue("@pulau", txtPulau.Text.Trim());
                        cmd.Parameters.AddWithValue("@harga_min", hargaMin);
                        cmd.Parameters.AddWithValue("@harga_max", hargaMax);
                        cmd.Parameters.AddWithValue("@rating", rating);
                        cmd.Parameters.AddWithValue("@total_review", totalReview);
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

            // Validasi rating
            string ratingText = txtRating.Text.Trim();
            if (!float.TryParse(ratingText, out float rating) || rating < 0 || rating > 5)
            {
                MessageBox.Show("Rating harus berupa angka antara 0-5!");
                txtRating.Focus();
                return;
            }

            // Validasi total review
            string totalReviewText = txtTotalReview.Text.Trim();
            if (!int.TryParse(totalReviewText, out int totalReview))
            {
                MessageBox.Show("Total review harus berupa angka yang valid!");
                txtTotalReview.Focus();
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
                            updateCmd.Parameters.AddWithValue("@rating", rating);
                            updateCmd.Parameters.AddWithValue("@total_review", totalReview);
                            updateCmd.Parameters.AddWithValue("@waktu", waktuTerbaik);

                            updateCmd.ExecuteNonQuery();
                            MessageBox.Show("Destinasi berhasil diupdate!");
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
            // Validasi nama destinasi harus diisi (field paling penting untuk identifikasi)
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
                    // Sesuaikan nama column dengan struktur DB mu
                    string query = "UPDATE reviews SET response=@response WHERE review_id=@id";

                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", selectedReviewId);
                        cmd.Parameters.AddWithValue("@response", txtBalasReview.Text.Trim());

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Balasan berhasil dikirim!");
                        txtBalasReview.Clear();
                        selectedReviewId = -1;
                        LoadReview(); // Refresh tabel review
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
                // Optional: tampilkan review yang dipilih di textbox
                if (e.ColumnIndex >= 0 && dgvRiwayat.Rows[e.RowIndex].Cells.Count > 4)
                {
                    var reviewText = dgvRiwayat.Rows[e.RowIndex].Cells[4].Value?.ToString();
                    // txtBalasReview.Hint = $"Balas review: {reviewText}"; // kalau mau tampilkan preview
                }
            }
        }

        private string GetSelectedWaktu()
        {
            List<string> waktu = new List<string>();

            // Ambil semua checkbox bulan
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
            txtRating.Clear();
            txtTotalReview.Clear();
            txtRekomendasiCuaca.Clear();

            // Uncheck semua checkbox bulan
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

            // Uncheck semua checkbox aktivitas (opsional)
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
    }
}