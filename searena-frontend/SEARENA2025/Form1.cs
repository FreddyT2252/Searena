using Npgsql;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace SEARENA2025
{
    public partial class Form1 : Form
    {
        private string _selectedRole = "";
        private const string CONNECTION_STRING =
             "Host=aws-1-ap-southeast-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.eeqqiyfukvhbwystupei;Password=SearenaDB123";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Set icons untuk textbox
            SetIcons();

            // Show role selection panel
            pnlRole.Visible = true;
            pnlLogin.Visible = false;
            pnlRegister.Visible = false;
            lblTitle.Visible = false;
            lblTitleReg.Visible = false;
        }

        private void SetIcons()
        {
            // Icons untuk login
            if (File.Exists("Icons/gridicons_mail.png"))
            {
                txtEmail.IconLeft = Image.FromFile("Icons/gridicons_mail.png");
                txtEmail.IconLeftOffset = new Point(10, 0);
                txtEmail.TextOffset = new Point(38, 0);
            }

            if (File.Exists("Icons/Group 25.png"))
            {
                txtPassword.IconLeft = Image.FromFile("Icons/Group 25.png");
                txtPassword.IconLeftOffset = new Point(10, 0);
                txtPassword.TextOffset = new Point(38, 0);
            }

            if (File.Exists("Icons/mdi_eye.png"))
            {
                txtPassword.IconRight = Image.FromFile("Icons/mdi_eye.png");
                txtPassword.IconRightCursor = Cursors.Hand;
                txtPassword.IconRightClick += TxtPassword_IconRightClick;
            }

            // Icons untuk register
            if (File.Exists("Icons/Vector.png"))
            {
                txtRegNama.IconLeft = Image.FromFile("Icons/Vector.png");
                txtRegNama.IconLeftOffset = new Point(10, 0);
                txtRegNama.TextOffset = new Point(38, 0);
            }

            if (File.Exists("Icons/gridicons_mail.png"))
            {
                txtRegEmail.IconLeft = Image.FromFile("Icons/gridicons_mail.png");
                txtRegEmail.IconLeftOffset = new Point(10, 0);
                txtRegEmail.TextOffset = new Point(38, 0);
            }

            if (File.Exists("Icons/Group 25.png"))
            {
                txtRegPassword.IconLeft = Image.FromFile("Icons/Group 25.png");
                txtRegPassword.IconLeftOffset = new Point(10, 0);
                txtRegPassword.TextOffset = new Point(38, 0);
            }

            if (File.Exists("Icons/mdi_eye.png"))
            {
                txtRegPassword.IconRight = Image.FromFile("Icons/mdi_eye.png");
                txtRegPassword.IconRightCursor = Cursors.Hand;
                txtRegPassword.IconRightClick += TxtRegPassword_IconRightClick;
            }
        }

        private void TxtPassword_IconRightClick(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = (txtPassword.PasswordChar == '●') ? '\0' : '●';
        }

        private void TxtRegPassword_IconRightClick(object sender, EventArgs e)
        {
            txtRegPassword.PasswordChar = (txtRegPassword.PasswordChar == '●') ? '\0' : '●';
        }

        // ===== ROLE SELECTION =====
        private void btnRoleAdmin_Click(object sender, EventArgs e)
        {
            _selectedRole = "admin";
            ShowLoginPanel();
        }

        private void btnRoleUser_Click(object sender, EventArgs e)
        {
            _selectedRole = "pengguna";
            ShowLoginPanel();
        }

        // ===== PANEL SWITCHING =====
        private void ShowLoginPanel()
        {
            pnlRole.Visible = false;
            pnlLogin.Visible = true;
            pnlRegister.Visible = false;
            lblTitle.Visible = true;
            lblTitleReg.Visible = false;
            lblTitle.Text = "Hai, Sea-Mates!";
        }

        private void ShowRegisterPanel()
        {
            pnlRole.Visible = false;
            pnlLogin.Visible = false;
            pnlRegister.Visible = true;
            lblTitle.Visible = false;
            lblTitleReg.Visible = true;
        }

        private void ShowRolePanel()
        {
            _selectedRole = "";
            pnlRole.Visible = true;
            pnlLogin.Visible = false;
            pnlRegister.Visible = false;
            lblTitle.Visible = false;
            lblTitleReg.Visible = false;

            // Clear inputs
            txtEmail.Text = "";
            txtPassword.Text = "";
            txtRegNama.Text = "";
            txtRegEmail.Text = "";
            txtRegPassword.Text = "";
        }

        // ===== TAB SWITCHING =====
        private void btnTabMasuk_Click(object sender, EventArgs e)
        {
            ShowLoginPanel();
        }

        private void btnTabDaftar_Click(object sender, EventArgs e)
        {
            ShowRegisterPanel();
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            ShowRolePanel();
        }

        // ===== LOGIN =====
        private void btnMasuk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Email dan password wajib diisi", "Validasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = new NpgsqlConnection(CONNECTION_STRING))
                {
                    conn.Open();

                    string query = "SELECT user_id, nama_lengkap, email FROM users WHERE email = @email AND password = @password";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@password", txtPassword.Text);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int userId = Convert.ToInt32(reader["user_id"]);
                                string nama = reader["nama_lengkap"]?.ToString() ?? "User";
                                string email = reader["email"]?.ToString() ?? "";

                                UserSession.SetUser(userId, nama, email, _selectedRole);

                                MessageBox.Show($"Selamat datang, {nama}!", "Login Berhasil",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                                PindahKeDashboard();
                            }
                            else
                            {
                                MessageBox.Show("Email atau password salah", "Login Gagal",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}\n\nDetail: {ex.ToString()}", "Error Login",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===== REGISTER =====
        private void btnDaftar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRegNama.Text))
            {
                MessageBox.Show("Nama lengkap harus diisi", "Validasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRegNama.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtRegEmail.Text))
            {
                MessageBox.Show("Email harus diisi", "Validasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRegEmail.Focus();
                return;
            }

            if (!txtRegEmail.Text.Contains("@"))
            {
                MessageBox.Show("Format email tidak valid", "Validasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRegEmail.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtRegPassword.Text) || txtRegPassword.Text.Length < 6)
            {
                MessageBox.Show("Password minimal 6 karakter", "Validasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRegPassword.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(_selectedRole))
            {
                MessageBox.Show("Pilih peran terlebih dahulu", "Validasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = new NpgsqlConnection(CONNECTION_STRING))
                {
                    conn.Open();

                    // Cek email duplikat
                    using (var cek = new NpgsqlCommand("SELECT COUNT(*) FROM users WHERE email = @email", conn))
                    {
                        cek.Parameters.AddWithValue("@email", txtRegEmail.Text.Trim());
                        long count = Convert.ToInt64(cek.ExecuteScalar() ?? 0);
                        if (count > 0)
                        {
                            MessageBox.Show("Email sudah terdaftar", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Insert user baru
                    string insert = "INSERT INTO users (user_id, nama_lengkap, email, password) VALUES (@id, @nama, @email, @password)";
                    using (var cmd = new NpgsqlCommand(insert, conn))
                    {
                        int newId = (int)(DateTime.Now.Ticks / 10000000);
                        cmd.Parameters.AddWithValue("@id", newId);
                        cmd.Parameters.AddWithValue("@nama", txtRegNama.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", txtRegEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@password", txtRegPassword.Text);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Registrasi berhasil! Silakan login.", "Sukses",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Pindah ke login dan isi email
                ShowLoginPanel();
                txtEmail.Text = txtRegEmail.Text.Trim();
                txtPassword.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}\n\nDetail: {ex.ToString()}", "Error Registrasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===== NAVIGATION =====
        private void PindahKeDashboard()
        {
            if (UserSession.IsAdmin())
            {
                var admin = new PageAdmin();
                admin.FormClosed += (s, args) => Application.Exit();
                admin.Show();
                this.Hide();
            }
            else
            {
                var dash = new DashboardUtama();
                dash.FormClosed += (s, args) => Application.Exit();
                dash.Show();
                this.Hide();
            }
        }

        // ===== BACKGROUND GRADIENT =====
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            using (var brush = new LinearGradientBrush(
                this.ClientRectangle,
                Color.FromArgb(109, 175, 207),
                Color.FromArgb(244, 185, 128),
                LinearGradientMode.Vertical))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }
    }
}