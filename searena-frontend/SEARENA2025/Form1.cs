using Npgsql;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SEARENA2025
{
    public partial class Form1 : Form
    {
        private const string CONNECTION_STRING =
             "Host=aws-1-ap-southeast-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.eeqqiyfukvhbwystupei;Password=SearenaDB123";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetIcons();
            // Langsung ke login panel, skip role selection
            pnlRole.Visible = false;
            pnlLogin.Visible = true;
            pnlRegister.Visible = false;
            lblTitle.Visible = true;
            lblTitleReg.Visible = false;
            lblTitle.Text = "Hai, Sea-Mates!";
        }

        private void SetIcons()
        {
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

        // ===== VALIDATION METHODS =====
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Regex pattern untuk validasi email
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        // ===== PANEL SWITCHING =====
        private void ShowLoginPanel()
        {
            pnlLogin.Visible = true;
            pnlRegister.Visible = false;
            pnlRole.Visible = false;
            lblTitle.Visible = true;
            lblTitleReg.Visible = false;
            lblTitle.Text = "Hai, Sea-Mates!";
            ClearInputs();
        }

        private void ShowRegisterPanel()
        {
            pnlLogin.Visible = false;
            pnlRegister.Visible = true;
            pnlRole.Visible = false;
            lblTitle.Visible = false;
            lblTitleReg.Visible = true;
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
            ShowLoginPanel();
        }

        // ===== ROLE SELECTION (UNTUK COMPATIBILITY DENGAN DESIGNER) =====
        private void btnRoleAdmin_Click(object sender, EventArgs e)
        {
            // Fitur role selection sudah dinonaktifkan
            // Role ditentukan dari database saat login
            ShowLoginPanel();
        }

        private void btnRoleUser_Click(object sender, EventArgs e)
        {
            // Fitur role selection sudah dinonaktifkan
            // Role ditentukan dari database saat login
            ShowLoginPanel();
        }

        // ===== LOGIN (PLAIN PASSWORD) =====
        private void btnMasuk_Click(object sender, EventArgs e)
        {
            // Validasi input
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Email wajib diisi", "Validasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Password wajib diisi", "Validasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            try
            {
                using (var conn = new NpgsqlConnection(CONNECTION_STRING))
                {
                    conn.Open();

                    // Query: ambil user dengan email & password
                    string query = "SELECT user_id, nama_lengkap, email, role FROM users WHERE LOWER(email) = LOWER(@email) AND password = @password";

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
                                string userRole = reader["role"]?.ToString() ?? "pengguna";

                                // Set user session dengan role dari database
                                UserSession.SetUser(userId, nama, email, userRole);

                                MessageBox.Show($"Selamat datang, {nama}!", "Login Berhasil",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                                ClearInputs();
                                PindahKeDashboard();
                            }
                            else
                            {
                                MessageBox.Show("Email atau password salah", "Login Gagal",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtPassword.Clear();
                                txtPassword.Focus();
                            }
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show($"Error koneksi database: {ex.Message}", "Error Database",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error tidak terduga: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===== REGISTER (PLAIN PASSWORD) =====
        private void btnDaftar_Click(object sender, EventArgs e)
        {
            // Validasi nama
            if (string.IsNullOrWhiteSpace(txtRegNama.Text))
            {
                MessageBox.Show("Nama lengkap harus diisi", "Validasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRegNama.Focus();
                return;
            }

            if (txtRegNama.Text.Trim().Length < 3)
            {
                MessageBox.Show("Nama lengkap minimal 3 karakter", "Validasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRegNama.Focus();
                return;
            }

            // Validasi email dengan regex
            if (!IsValidEmail(txtRegEmail.Text))
            {
                MessageBox.Show("Format email tidak valid\nContoh: nama@domain.com", "Validasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRegEmail.Focus();
                return;
            }

            // Validasi password
            if (string.IsNullOrWhiteSpace(txtRegPassword.Text))
            {
                MessageBox.Show("Password harus diisi", "Validasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRegPassword.Focus();
                return;
            }

            if (txtRegPassword.Text.Length < 6)
            {
                MessageBox.Show("Password minimal 6 karakter", "Validasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRegPassword.Focus();
                return;
            }

            try
            {
                using (var conn = new NpgsqlConnection(CONNECTION_STRING))
                {
                    conn.Open();

                    // Cek email duplikat (case-insensitive)
                    using (var cek = new NpgsqlCommand("SELECT COUNT(*) FROM users WHERE LOWER(email) = LOWER(@email)", conn))
                    {
                        cek.Parameters.AddWithValue("@email", txtRegEmail.Text.Trim());
                        long count = Convert.ToInt64(cek.ExecuteScalar() ?? 0);
                        if (count > 0)
                        {
                            MessageBox.Show("Email sudah terdaftar. Silakan gunakan email lain atau login.", "Email Sudah Ada",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtRegEmail.Focus();
                            return;
                        }
                    }

                    // Insert user baru dengan password plain text
                    string insert = "INSERT INTO users (nama_lengkap, email, password, role) VALUES (@nama, @email, @password, @role)";
                    using (var cmd = new NpgsqlCommand(insert, conn))
                    {
                        cmd.Parameters.AddWithValue("@nama", txtRegNama.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", txtRegEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@password", txtRegPassword.Text);
                        cmd.Parameters.AddWithValue("@role", "pengguna"); // Default: pengguna biasa

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Registrasi berhasil!\n\nSilakan login dengan akun Anda.", "Sukses",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ShowLoginPanel();
                txtEmail.Text = txtRegEmail.Text.Trim();
                txtPassword.Focus();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show($"Error database: {ex.Message}", "Error Registrasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error tidak terduga: {ex.Message}", "Error",
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

        // ===== HELPER METHODS =====
        private void ClearInputs()
        {
            txtEmail.Text = "";
            txtPassword.Text = "";
            txtRegNama.Text = "";
            txtRegEmail.Text = "";
            txtRegPassword.Text = "";
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