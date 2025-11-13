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

       

        private const string ConnString = "Host=aws-1-ap-southeast-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.eeqqiyfukvhbwystupei;Password=SearenaDB123";

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
