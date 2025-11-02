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

namespace SEARENA2025
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            LoadBookmarkData();
            AttachNavbarEvents();
        }

        private void LoadBookmarkData()
        {
            try
            {
                // Set data pengguna
                lblNama.Text = "Tasya Aurora";
                lblPengguna.Text = "Pengguna";
                lblBergabung.Text = "Bergabung sejak 2021";
                lblEmail.Text = "tasyaaurora@gmail.com";
                lblTelepon.Text = "08213867890";

                // Set data destinasi bookmark
                lblDestinasi1.Text = "Raja Ampat Marine Park";
                lblLokasi1.Text = "Wisata, Raja Ampat, Papua Barat";
                lblDeskripsi1.Text = "Surga bawah laut yang memiliki keindahan alam bawah laut terbaik di dunia";

                lblDestinasi2.Text = "Raja Ampat Marine Park";
                lblLokasi2.Text = "Wisata, Raja Ampat, Papua Barat";
                lblDeskripsi2.Text = "Surga bawah laut yang memiliki keindahan alam bawah laut terbaik di dunia";

                // Set fasilitas
                lblFasilitas1.Text = "Sunset\nDiving\nLainnya";
                lblFasilitas2.Text = "Sunset\nDiving\nLainnya";

                // Set waktu terbaik
                lblWaktu1.Text = "Terbaik: Oktober, November";
                lblWaktu2.Text = "Terbaik: Oktober, November";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading bookmark data: {ex.Message}");
            }
        }

        private void Logo_Click(object sender, EventArgs e)
        {
            // Kembali ke DashboardUtama
            DashboardUtama dashboard = new DashboardUtama();
            dashboard.Show();
            this.Close();
        }

        private void guna2Panel4_Paint(object sender, PaintEventArgs e)
        {
            // Optional: Custom painting jika diperlukan
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            // Kembali ke DashboardUtama
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
            // Sudah di halaman bookmark, ubah warna button
            btnBookmark.FillColor = Color.LightGreen;
            btnRatingReview.FillColor = Color.FloralWhite;

            // Refresh data bookmark
            LoadBookmarkData();
        }

        private void btnRatingReview_Click(object sender, EventArgs e)
        {
            // Navigasi ke halaman rating dan review (Form2)
            btnRatingReview.FillColor = Color.LightGreen;
            btnBookmark.FillColor = Color.FloralWhite;

            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        // Event handler untuk navigasi menu navbar
        private void Beranda_Click(object sender, EventArgs e)
        {
            // Kembali ke DashboardUtama
            DashboardUtama dashboard = new DashboardUtama();
            dashboard.Show();
            this.Close();
        }

        private void Destinasi_Click(object sender, EventArgs e)
        {
            // Kembali ke DashboardUtama
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
            // Navigasi ke profile rating (Form2)
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        private void btnHapusBookmark1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Hapus bookmark untuk Raja Ampat Marine Park?",
                "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Logic untuk hapus bookmark pertama
                panelDestinasi1.Visible = false;
                MessageBox.Show("Bookmark berhasil dihapus", "Info");
            }
        }

        private void btnHapusBookmark2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Hapus bookmark untuk Raja Ampat Marine Park?",
                "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Logic untuk hapus bookmark kedua
                panelDestinasi2.Visible = false;
                MessageBox.Show("Bookmark berhasil dihapus", "Info");
            }
        }

        private void btnLihatDetail1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Menampilkan detail Raja Ampat Marine Park", "Detail Destinasi");
        }

        private void btnLihatDetail2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Menampilkan detail Raja Ampat Marine Park", "Detail Destinasi");
        }

        // Method untuk menambahkan event handler ke kontrol navbar
        public void AttachNavbarEvents()
        {
            // Pastikan kontrol ini ada di Designer
            if (Beranda != null) Beranda.Click += Beranda_Click;
            if (Destinasi != null) Destinasi.Click += Destinasi_Click;
            if (Kontak != null) Kontak.Click += Kontak_Click;
            if (TentangKami != null) TentangKami.Click += TentangKami_Click;
            if (lblProfile != null) lblProfile.Click += lblProfile_Click;

            // Event handler untuk logo
            if (guna2PictureBox1 != null) guna2PictureBox1.Click += Logo_Click;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // Inisialisasi tambahan saat form load
        }

        // Event handler untuk foto profil
        private void Profile_Click(object sender, EventArgs e)
        {
            // Klik pada foto profil - untuk mengubah foto
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Profile.Image = Image.FromFile(openFileDialog.FileName);
            }
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
    }
}