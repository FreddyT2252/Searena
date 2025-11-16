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
                    string query = @"SELECT id, nama, alamat, deskripsi, harga_tiket 
                                     FROM destinasi ORDER BY id DESC";

                    using (var cmd = new NpgsqlCommand(query, connection))
                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        // Binding ke DataGridView jika ada
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
                    string query = @"SELECT id, destinasi_id, user_name, rating, review_text 
                                     FROM reviews WHERE response IS NULL OR response = '' 
                                     ORDER BY id DESC";

                    using (var cmd = new NpgsqlCommand(query, connection))
                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvRiwayat.DataSource = dt;
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

            try
            {
                using (var connection = new NpgsqlConnection(CONNECTION_STRING))
                {
                    connection.Open();
                    string query = @"INSERT INTO destinasi (nama, alamat, deskripsi, harga_tiket, rekomendasi_cuaca, aktivitas, waktu_terbaik) 
                                     VALUES (@nama, @alamat, @deskripsi, @harga, @rekomendasi, @aktivitas, @waktu)";

                    string aktivitas = GetSelectedAktivitas();
                    string waktuTerbaik = GetSelectedWaktu();

                    if (string.IsNullOrWhiteSpace(aktivitas))
                    {
                        MessageBox.Show("Pilih minimal satu aktivitas!");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(waktuTerbaik))
                    {
                        MessageBox.Show("Pilih minimal satu bulan!");
                        return;
                    }

                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@nama", txtNamaDestinasi.Text.Trim());
                        cmd.Parameters.AddWithValue("@alamat", txtAlamatDestinasi.Text.Trim());
                        cmd.Parameters.AddWithValue("@deskripsi", txtDeskripsi.Text.Trim());
                        cmd.Parameters.AddWithValue("@harga", int.Parse(txtHargaTiket.Text.Trim()));
                        cmd.Parameters.AddWithValue("@rekomendasi", txtRekomendasiCuaca.Text.Trim());
                        cmd.Parameters.AddWithValue("@aktivitas", aktivitas);
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
            if (selectedDestinasiId == -1)
            {
                MessageBox.Show("Pilih destinasi yang ingin diupdate!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNamaDestinasi.Text))
            {
                MessageBox.Show("Nama destinasi harus diisi!");
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection(CONNECTION_STRING))
                {
                    connection.Open();
                    string query = @"UPDATE destinasi SET nama=@nama, alamat=@alamat, deskripsi=@deskripsi, 
                                     harga_tiket=@harga, rekomendasi_cuaca=@rekomendasi, aktivitas=@aktivitas, 
                                     waktu_terbaik=@waktu WHERE id=@id";

                    string aktivitas = GetSelectedAktivitas();
                    string waktuTerbaik = GetSelectedWaktu();

                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", selectedDestinasiId);
                        cmd.Parameters.AddWithValue("@nama", txtNamaDestinasi.Text.Trim());
                        cmd.Parameters.AddWithValue("@alamat", txtAlamatDestinasi.Text.Trim());
                        cmd.Parameters.AddWithValue("@deskripsi", txtDeskripsi.Text.Trim());
                        cmd.Parameters.AddWithValue("@harga", int.Parse(txtHargaTiket.Text.Trim()));
                        cmd.Parameters.AddWithValue("@rekomendasi", txtRekomendasiCuaca.Text.Trim());
                        cmd.Parameters.AddWithValue("@aktivitas", aktivitas);
                        cmd.Parameters.AddWithValue("@waktu", waktuTerbaik);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Destinasi berhasil diupdate!");
                        ClearForm();
                        LoadDestinasi();
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
            if (selectedDestinasiId == -1)
            {
                MessageBox.Show("Pilih destinasi yang ingin dihapus!");
                return;
            }

            if (MessageBox.Show("Yakin ingin menghapus destinasi ini?", "Konfirmasi", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            try
            {
                using (var connection = new NpgsqlConnection(CONNECTION_STRING))
                {
                    connection.Open();
                    string query = "DELETE FROM destinasi WHERE id=@id";

                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", selectedDestinasiId);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Destinasi berhasil dihapus!");
                        ClearForm();
                        LoadDestinasi();
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
                    string query = "UPDATE reviews SET response=@response WHERE id=@id";

                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", selectedReviewId);
                        cmd.Parameters.AddWithValue("@response", txtBalasReview.Text.Trim());

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Balasan berhasil dikirim!");
                        txtBalasReview.Clear();
                        selectedReviewId = -1;
                        LoadReview();
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
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvRiwayat.Rows[e.RowIndex];
                selectedReviewId = (int)row.Cells["id"].Value;
            }
        }

        private string GetSelectedAktivitas()
        {
            List<string> aktivitas = new List<string>();

            if (cbSnorkling.Checked) aktivitas.Add("Snorkling");
            if (cbDiving.Checked) aktivitas.Add("Diving");
            if (cbSunset.Checked) aktivitas.Add("Sunset");
            if (cbCamping.Checked) aktivitas.Add("Camping");

            return string.Join(", ", aktivitas);
        }

        private string GetSelectedWaktu()
        {
            List<string> waktu = new List<string>();

            // Cari CheckBox untuk bulan
            CheckBox cbJan = Controls.Find("cbJan", true).Length > 0 ?
                             (CheckBox)Controls.Find("cbJan", true)[0] : null;
            CheckBox cbFeb = Controls.Find("cbFeb", true).Length > 0 ?
                             (CheckBox)Controls.Find("cbFeb", true)[0] : null;
            CheckBox cbMar = Controls.Find("cbMar", true).Length > 0 ?
                             (CheckBox)Controls.Find("cbMar", true)[0] : null;
            CheckBox cbApr = Controls.Find("cbApr", true).Length > 0 ?
                             (CheckBox)Controls.Find("cbApr", true)[0] : null;
            CheckBox cbMei = Controls.Find("cbMei", true).Length > 0 ?
                             (CheckBox)Controls.Find("cbMei", true)[0] : null;
            CheckBox cbJun = Controls.Find("cbJun", true).Length > 0 ?
                             (CheckBox)Controls.Find("cbJun", true)[0] : null;
            CheckBox cbJul = Controls.Find("cbJul", true).Length > 0 ?
                             (CheckBox)Controls.Find("cbJul", true)[0] : null;
            CheckBox cbAug = Controls.Find("cbAug", true).Length > 0 ?
                             (CheckBox)Controls.Find("cbAug", true)[0] : null;
            CheckBox cbSept = Controls.Find("cbSept", true).Length > 0 ?
                              (CheckBox)Controls.Find("cbSept", true)[0] : null;
            CheckBox cbOkt = Controls.Find("cbOkt", true).Length > 0 ?
                             (CheckBox)Controls.Find("cbOkt", true)[0] : null;
            CheckBox cbNov = Controls.Find("cbNov", true).Length > 0 ?
                             (CheckBox)Controls.Find("cbNov", true)[0] : null;
            CheckBox cbDes = Controls.Find("cbDes", true).Length > 0 ?
                             (CheckBox)Controls.Find("cbDes", true)[0] : null;

            if (cbJan?.Checked == true) waktu.Add("Januari");
            if (cbFeb?.Checked == true) waktu.Add("Februari");
            if (cbMar?.Checked == true) waktu.Add("Maret");
            if (cbApr?.Checked == true) waktu.Add("April");
            if (cbMei?.Checked == true) waktu.Add("Mei");
            if (cbJun?.Checked == true) waktu.Add("Juni");
            if (cbJul?.Checked == true) waktu.Add("Juli");
            if (cbAug?.Checked == true) waktu.Add("Agustus");
            if (cbSept?.Checked == true) waktu.Add("September");
            if (cbOkt?.Checked == true) waktu.Add("Oktober");
            if (cbNov?.Checked == true) waktu.Add("November");
            if (cbDes?.Checked == true) waktu.Add("Desember");

            return string.Join(", ", waktu);
        }

        private void ClearForm()
        {
            txtNamaDestinasi.Clear();
            txtAlamatDestinasi.Clear();
            txtDeskripsi.Clear();
            txtHargaTiket.Clear();
            txtRekomendasiCuaca.Clear();

            // Uncheck semua checkbox aktivitas
            UncheckControl("cbSnorkling");
            UncheckControl("cbDiving");
            UncheckControl("cbSunset");
            UncheckControl("cbFotografi");
            UncheckControl("cbCamping");

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
            UncheckControl("cbDes");

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