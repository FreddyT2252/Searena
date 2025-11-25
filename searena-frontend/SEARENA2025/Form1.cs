using Npgsql;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace SEARENA2025
{
    public partial class Form1 : Form
    {
        // Kontrol Utama
        private Guna2BorderlessForm _gunaBorderlessForm;
        private Guna2Panel _mainPanel;
        private Label _lblTitle;
        private Guna2Button _btnTabMasuk, _btnTabDaftar;

        // Role Selection Controls
        private Guna2Panel _roleSelectionPanel;
        private Guna2Button _btnRoleAdmin, _btnRolePengguna;
        private Label _lblRoleTitle;
        private Label _lblRoleSubtitle;

        // Login Form Controls
        private Guna2TextBox _txtEmail, _txtPassword;
        private Guna2Button _btnMasuk;

        // Register Form Controls
        private Guna2TextBox _txtRegNama, _txtRegEmail, _txtRegPassword;
        private Guna2Button _btnDaftar;

        // Role yang dipilih
        private string _selectedRole = "";

        // Ukuran dan Warna
        private const int PANEL_WIDTH = 400;
        private const int CONTENT_WIDTH = 330;
        private const int FORM_WIDTH = 900;
        private const int FORM_HEIGHT = 600;
        private const int PANEL_HEIGHT_LOGIN = 310;
        private const int PANEL_HEIGHT_REGISTER = 380;
        private const int PANEL_HEIGHT_ROLE = 300;
        private const int BORDER_RADIUS = 25;

        private static readonly Color COLOR_PRIMARY = Color.FromArgb(109, 175, 207);
        private static readonly Color COLOR_SECONDARY = Color.FromArgb(244, 185, 128);
        private static readonly Color COLOR_PANEL_BG = Color.FromArgb(200, 255, 255, 255);
        private static readonly Color COLOR_TAB_CONTAINER = Color.FromArgb(220, 255, 255, 255);
        private static readonly Color COLOR_ACCENT_CYAN = Color.FromArgb(94, 226, 220);
        private static readonly Color COLOR_ACCENT_CYAN_DARK = Color.FromArgb(70, 200, 195);
        private static readonly Color COLOR_ACCENT_ORANGE = Color.FromArgb(255, 159, 111);
        private static readonly Color COLOR_TEXT_DARK = Color.FromArgb(80, 80, 80);
        private static readonly Color COLOR_TEXT_LIGHT = Color.FromArgb(130, 130, 130);
        private static readonly Color COLOR_LINK = Color.FromArgb(31, 95, 127);

        // Database connection string
        private const string CONNECTION_STRING = "Host=aws-1-ap-southeast-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.eeqqiyfukvhbwystupei;Password=SearenaDB123";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConfigureForm();
            InitializeGunaBorderlessForm();
            CreateRoleSelectionPanel();
            CreateMainPanel();
            CreateTitleLabel();
            CreateTabButtons();
            CreateLoginForm();
            CreateRegisterForm();
            ShowRoleSelection();
        }

        private void ConfigureForm()
        {
            this.Size = new Size(FORM_WIDTH, FORM_HEIGHT);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Paint += Form1_Paint;
        }

        private void InitializeGunaBorderlessForm()
        {
            _gunaBorderlessForm = new Guna2BorderlessForm { ContainerControl = this, TransparentWhileDrag = true };
        }

        private void CreateRoleSelectionPanel()
        {
            _roleSelectionPanel = new Guna2Panel
            {
                Size = new Size(PANEL_WIDTH, PANEL_HEIGHT_ROLE),
                Location = new Point((this.Width - PANEL_WIDTH) / 2, (this.Height - PANEL_HEIGHT_ROLE) / 2 + 30),
                BackColor = Color.Transparent,
                BorderRadius = BORDER_RADIUS,
                FillColor = COLOR_PANEL_BG
            };
            this.Controls.Add(_roleSelectionPanel);

            _lblRoleTitle = new Label
            {
                Text = "Pilih Peran Anda",
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                ForeColor = COLOR_TEXT_DARK,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Width = PANEL_WIDTH,
                Height = 50,
                Location = new Point(0, 20)
            };
            _roleSelectionPanel.Controls.Add(_lblRoleTitle);

            _lblRoleSubtitle = new Label
            {
                Text = "Masuk sebagai admin atau pengguna biasa",
                Font = new Font("Segoe UI", 9F),
                ForeColor = COLOR_TEXT_LIGHT,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Width = PANEL_WIDTH,
                Height = 25,
                Location = new Point(0, 70)
            };
            _roleSelectionPanel.Controls.Add(_lblRoleSubtitle);

            int contentStartX = (PANEL_WIDTH - 300) / 2;

            _btnRoleAdmin = new Guna2Button
            {
                Size = new Size(140, 100),
                Location = new Point(contentStartX, 120),
                Text = "Admin",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                FillColor = COLOR_ACCENT_CYAN,
                ForeColor = Color.Black,
                BorderRadius = 15,
                Cursor = Cursors.Hand
            };
            _btnRoleAdmin.Click += (s, e) => SelectRole("admin");
            _roleSelectionPanel.Controls.Add(_btnRoleAdmin);

            _btnRolePengguna = new Guna2Button
            {
                Size = new Size(140, 100),
                Location = new Point(contentStartX + 160, 120),
                Text = "Pengguna",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                FillColor = COLOR_ACCENT_ORANGE,
                ForeColor = Color.White,
                BorderRadius = 15,
                Cursor = Cursors.Hand
            };
            _btnRolePengguna.Click += (s, e) => SelectRole("pengguna");
            _roleSelectionPanel.Controls.Add(_btnRolePengguna);
        }

        private void SelectRole(string role)
        {
            _selectedRole = role;
            _roleSelectionPanel.Visible = false;
            ShowLoginForm();
        }

        private void CreateMainPanel()
        {
            _mainPanel = new Guna2Panel
            {
                Size = new Size(PANEL_WIDTH, PANEL_HEIGHT_LOGIN),
                Location = new Point((this.Width - PANEL_WIDTH) / 2, (this.Height - PANEL_HEIGHT_LOGIN) / 2 + 30),
                BackColor = Color.Transparent,
                BorderRadius = BORDER_RADIUS,
                FillColor = COLOR_PANEL_BG,
                Visible = false
            };
            this.Controls.Add(_mainPanel);

            Guna2Button btnKembali = new Guna2Button
            {
                Size = new Size(100, 30),
                Location = new Point(12, 8),
                Text = "← Kembali",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                FillColor = Color.Transparent,
                ForeColor = Color.Black,
                BorderRadius = 10,
                Cursor = Cursors.Hand,
                TextAlign = HorizontalAlignment.Left
            };
            btnKembali.Click += (s, e) => KembaliKeRoleSelection();
            _mainPanel.Controls.Add(btnKembali);
        }

        private void KembaliKeRoleSelection()
        {
            _selectedRole = "";
            _mainPanel.Visible = false;
            _roleSelectionPanel.Visible = true;
            _txtEmail.Text = "";
            _txtPassword.Text = "";
            _txtRegNama.Text = "";
            _txtRegEmail.Text = "";
            _txtRegPassword.Text = "";
        }

        private void CreateTitleLabel()
        {
            _lblTitle = new Label
            {
                Text = "Hai, Sea-Mates!",
                Font = new Font("Segoe UI", 26F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Width = PANEL_WIDTH,
                Height = 50,
                Visible = false
            };
            _lblTitle.Location = new Point(_mainPanel.Left, _mainPanel.Top - 65);
            this.Controls.Add(_lblTitle);
        }

        private void CreateTabButtons()
        {
            Guna2Panel tabContainer = new Guna2Panel
            {
                Size = new Size(300, 45),
                Location = new Point((PANEL_WIDTH - 300) / 2, 25),
                FillColor = COLOR_TAB_CONTAINER,
                BorderRadius = 22
            };
            _mainPanel.Controls.Add(tabContainer);

            _btnTabMasuk = new Guna2Button
            {
                Text = "Masuk",
                Size = new Size(145, 38),
                Location = new Point(4, 4),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                FillColor = COLOR_ACCENT_CYAN,
                ForeColor = Color.Black,
                BorderRadius = 19,
                Cursor = Cursors.Hand
            };
            _btnTabMasuk.Click += (s, e) => ShowLoginForm();
            tabContainer.Controls.Add(_btnTabMasuk);

            _btnTabDaftar = new Guna2Button
            {
                Text = "Daftar",
                Size = new Size(145, 38),
                Location = new Point(151, 4),
                Font = new Font("Segoe UI", 11F),
                FillColor = Color.Transparent,
                ForeColor = COLOR_TEXT_LIGHT,
                BorderRadius = 19,
                Cursor = Cursors.Hand
            };
            _btnTabDaftar.Click += (s, e) => ShowRegisterForm();
            tabContainer.Controls.Add(_btnTabDaftar);
        }

        private void CreateLoginForm()
        {
            int contentStartX = (PANEL_WIDTH - CONTENT_WIDTH) / 2;
            int currentY = 85;

            _txtEmail = new Guna2TextBox
            {
                Size = new Size(CONTENT_WIDTH, 55),
                Location = new Point(contentStartX, currentY),
                BorderRadius = 10,
                Font = new Font("Segoe UI", 10F),
                IconLeft = Image.FromFile("Icons/gridicons_mail.png"),
                IconLeftOffset = new Point(12, 0),
                TextOffset = new Point(5, 10),
                PlaceholderText = "Alamat E-mail",
                BorderColor = Color.FromArgb(220, 220, 220),
                FocusedState = { BorderColor = COLOR_ACCENT_CYAN_DARK },
                FillColor = Color.White
            };
            _txtEmail.Controls.Add(CreateInnerLabel("Alamat E-mail", 45, 5));
            _mainPanel.Controls.Add(_txtEmail);

            currentY += _txtEmail.Height + 15;

            _txtPassword = new Guna2TextBox
            {
                Size = new Size(CONTENT_WIDTH, 55),
                Location = new Point(contentStartX, currentY),
                BorderRadius = 10,
                Font = new Font("Segoe UI", 10F),
                PasswordChar = '●',
                IconLeft = Image.FromFile("Icons/Group 25.png"),
                IconLeftOffset = new Point(12, 0),
                TextOffset = new Point(5, 10),
                IconRight = Image.FromFile("Icons/mdi_eye.png"),
                IconRightCursor = Cursors.Hand,
                PlaceholderText = "Kata Sandi",
                BorderColor = Color.FromArgb(220, 220, 220),
                FocusedState = { BorderColor = COLOR_ACCENT_CYAN_DARK },
                FillColor = Color.White
            };
            _txtPassword.IconRightClick += (s, e) => TogglePasswordVisibility(_txtPassword);
            _txtPassword.Controls.Add(CreateInnerLabel("Kata Sandi", 45, 5));
            _mainPanel.Controls.Add(_txtPassword);

            currentY += _txtPassword.Height + 30;

            _btnMasuk = new Guna2Button
            {
                Size = new Size(CONTENT_WIDTH, 48),
                Location = new Point(contentStartX, currentY),
                Text = "Masuk",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                FillColor = COLOR_ACCENT_ORANGE,
                ForeColor = Color.White,
                BorderRadius = 24,
                Cursor = Cursors.Hand
            };
            _btnMasuk.Click += (s, e) => Login();
            _mainPanel.Controls.Add(_btnMasuk);
        }

        private void CreateRegisterForm()
        {
            int contentStartX = (PANEL_WIDTH - CONTENT_WIDTH) / 2;
            int currentY = 85;

            _txtRegNama = new Guna2TextBox
            {
                Size = new Size(CONTENT_WIDTH, 55),
                Location = new Point(contentStartX, currentY),
                BorderRadius = 10,
                Font = new Font("Segoe UI", 10F),
                IconLeft = Image.FromFile("Icons/Vector.png"),
                IconLeftOffset = new Point(12, 0),
                TextOffset = new Point(5, 10),
                PlaceholderText = "Nama Lengkap",
                BorderColor = Color.FromArgb(220, 220, 220),
                FocusedState = { BorderColor = COLOR_ACCENT_CYAN_DARK },
                FillColor = Color.White,
                Visible = false
            };
            _txtRegNama.Controls.Add(CreateInnerLabel("Nama Lengkap", 45, 5));
            _mainPanel.Controls.Add(_txtRegNama);

            currentY += _txtRegNama.Height + 15;

            _txtRegEmail = new Guna2TextBox
            {
                Size = new Size(CONTENT_WIDTH, 55),
                Location = new Point(contentStartX, currentY),
                BorderRadius = 10,
                Font = new Font("Segoe UI", 10F),
                IconLeft = Image.FromFile("Icons/gridicons_mail.png"),
                IconLeftOffset = new Point(12, 0),
                TextOffset = new Point(5, 10),
                PlaceholderText = "Alamat E-mail",
                BorderColor = Color.FromArgb(220, 220, 220),
                FocusedState = { BorderColor = COLOR_ACCENT_CYAN_DARK },
                FillColor = Color.White,
                Visible = false
            };
            _txtRegEmail.Controls.Add(CreateInnerLabel("Alamat E-mail", 45, 5));
            _mainPanel.Controls.Add(_txtRegEmail);

            currentY += _txtRegEmail.Height + 15;

            _txtRegPassword = new Guna2TextBox
            {
                Size = new Size(CONTENT_WIDTH, 55),
                Location = new Point(contentStartX, currentY),
                BorderRadius = 10,
                Font = new Font("Segoe UI", 10F),
                PasswordChar = '●',
                IconLeft = Image.FromFile("Icons/Group 25.png"),
                IconLeftOffset = new Point(12, 0),
                TextOffset = new Point(5, 10),
                IconRight = Image.FromFile("Icons/mdi_eye.png"),
                IconRightCursor = Cursors.Hand,
                PlaceholderText = "Kata Sandi",
                BorderColor = Color.FromArgb(220, 220, 220),
                FocusedState = { BorderColor = COLOR_ACCENT_CYAN_DARK },
                FillColor = Color.White,
                Visible = false
            };
            _txtRegPassword.IconRightClick += (s, e) => TogglePasswordVisibility(_txtRegPassword);
            _txtRegPassword.Controls.Add(CreateInnerLabel("Kata Sandi", 45, 5));
            _mainPanel.Controls.Add(_txtRegPassword);

            currentY += _txtRegPassword.Height + 30;

            _btnDaftar = new Guna2Button
            {
                Size = new Size(CONTENT_WIDTH, 48),
                Location = new Point(contentStartX, currentY),
                Text = "Daftar",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                FillColor = COLOR_ACCENT_ORANGE,
                ForeColor = Color.White,
                BorderRadius = 24,
                Cursor = Cursors.Hand,
                Visible = false
            };
            _btnDaftar.Click += (s, e) => Register();
            _mainPanel.Controls.Add(_btnDaftar);
        }

        private void ShowRoleSelection()
        {
            _roleSelectionPanel.Visible = true;
            _mainPanel.Visible = false;
            _lblTitle.Visible = false;
        }

        private void ShowLoginForm()
        {
            _mainPanel.Visible = true;
            _roleSelectionPanel.Visible = false;
            _mainPanel.Size = new Size(PANEL_WIDTH, PANEL_HEIGHT_LOGIN);
            _mainPanel.Location = new Point((this.Width - PANEL_WIDTH) / 2, (this.Height - PANEL_HEIGHT_LOGIN) / 2 + 30);
            _lblTitle.Visible = true;
            _lblTitle.Text = "Hai, Sea-Mates!";
            _lblTitle.Location = new Point(_mainPanel.Left, _mainPanel.Top - 65);

            _btnTabMasuk.FillColor = COLOR_ACCENT_CYAN;
            _btnTabMasuk.ForeColor = Color.Black;
            _btnTabMasuk.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            _btnTabDaftar.FillColor = Color.Transparent;
            _btnTabDaftar.ForeColor = COLOR_TEXT_LIGHT;
            _btnTabDaftar.Font = new Font("Segoe UI", 11F);

            SetControlsVisibility(true, _txtEmail, _txtPassword, _btnMasuk);
            SetControlsVisibility(false, _txtRegNama, _txtRegEmail, _txtRegPassword, _btnDaftar);
        }

        private void ShowRegisterForm()
        {
            _mainPanel.Visible = true;
            _roleSelectionPanel.Visible = false;
            _mainPanel.Size = new Size(PANEL_WIDTH, PANEL_HEIGHT_REGISTER);
            _mainPanel.Location = new Point((this.Width - PANEL_WIDTH) / 2, (this.Height - PANEL_HEIGHT_REGISTER) / 2 + 30);
            _lblTitle.Visible = true;
            _lblTitle.Text = "Searena";
            _lblTitle.Location = new Point(_mainPanel.Left, _mainPanel.Top - 65);

            _btnTabDaftar.FillColor = COLOR_ACCENT_CYAN;
            _btnTabDaftar.ForeColor = Color.Black;
            _btnTabDaftar.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            _btnTabMasuk.FillColor = Color.Transparent;
            _btnTabMasuk.ForeColor = COLOR_TEXT_LIGHT;
            _btnTabMasuk.Font = new Font("Segoe UI", 11F);

            SetControlsVisibility(false, _txtEmail, _txtPassword, _btnMasuk);
            SetControlsVisibility(true, _txtRegNama, _txtRegEmail, _txtRegPassword, _btnDaftar);
        }

        private void Login()
        {
            if (string.IsNullOrWhiteSpace(_txtEmail.Text) || string.IsNullOrWhiteSpace(_txtPassword.Text))
            {
                MessageBox.Show("Email dan password wajib diisi");
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection(CONNECTION_STRING))
                {
                    connection.Open();
                    string query = "SELECT user_id, nama_lengkap, email FROM users WHERE email = @email AND password = @password";

                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@email", _txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@password", _txtPassword.Text);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string userId = reader["user_id"]?.ToString() ?? "0";
                                string namaLengkap = reader["nama_lengkap"]?.ToString() ?? "User";
                                string email = reader["email"]?.ToString() ?? "";

                                // Set user session dengan role yang dipilih
                                UserSession.SetUser(int.Parse(userId), namaLengkap, email, _selectedRole);

                                MessageBox.Show($"Selamat datang, {namaLengkap}!", "Login Berhasil",
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
                MessageBox.Show("Error: " + ex.Message, "Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Register()
        {
            if (string.IsNullOrWhiteSpace(_txtRegNama.Text))
            {
                MessageBox.Show("Nama lengkap harus diisi", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtRegNama.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(_txtRegEmail.Text))
            {
                MessageBox.Show("Email harus diisi", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtRegEmail.Focus();
                return;
            }

            if (!_txtRegEmail.Text.Contains("@"))
            {
                MessageBox.Show("Format email tidak valid", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtRegEmail.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(_txtRegPassword.Text) || _txtRegPassword.Text.Length < 6)
            {
                MessageBox.Show("Password minimal 6 karakter", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtRegPassword.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(_selectedRole))
            {
                MessageBox.Show("Pilih peran terlebih dahulu (Admin atau Pengguna)", "Validasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection(CONNECTION_STRING))
                {
                    connection.Open();

                    // Cek email sudah ada atau belum
                    string checkQuery = "SELECT COUNT(*) FROM users WHERE email = @email";
                    using (var cmd = new NpgsqlCommand(checkQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@email", _txtRegEmail.Text.Trim());
                        long count = Convert.ToInt64(cmd.ExecuteScalar() ?? 0);
                        if (count > 0)
                        {
                            MessageBox.Show("Email sudah terdaftar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Insert user baru
                    string insertQuery = "INSERT INTO users (user_id, nama_lengkap, email, password) VALUES (@user_id, @nama, @email, @password)";

                    using (var cmd = new NpgsqlCommand(insertQuery, connection))
                    {
                        // Generate user_id unik (integer)
                        int newUserId = (int)(DateTime.Now.Ticks / 10000000);

                        cmd.Parameters.AddWithValue("@user_id", newUserId);
                        cmd.Parameters.AddWithValue("@nama", _txtRegNama.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", _txtRegEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@password", _txtRegPassword.Text);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Registrasi berhasil! Silakan login dengan akun Anda.", "Sukses",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        ShowLoginForm();
                        _txtEmail.Text = _txtRegEmail.Text.Trim();
                        _txtPassword.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error Registrasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PindahKeDashboard()
        {
            if (UserSession.IsAdmin())
            {
                PageAdmin pageAdmin = new PageAdmin();
                pageAdmin.FormClosed += (s, args) => Application.Exit();
                pageAdmin.Show();
                this.Hide();
            }
            else
            {
                DashboardUtama dashboard = new DashboardUtama();
                dashboard.FormClosed += (s, args) => Application.Exit();
                dashboard.Show();
                this.Hide();
            }
        }

        private Label CreateInnerLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 7.5F),
                ForeColor = COLOR_TEXT_LIGHT,
                BackColor = Color.White,
                AutoSize = true,
                Location = new Point(x, y)
            };
        }

        private void TogglePasswordVisibility(Guna2TextBox textBox)
        {
            textBox.PasswordChar = (textBox.PasswordChar == '●') ? '\0' : '●';
        }

        private void SetControlsVisibility(bool isVisible, params Control[] controls)
        {
            foreach (var control in controls)
            {
                if (control != null)
                    control.Visible = isVisible;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(
                this.ClientRectangle, COLOR_PRIMARY, COLOR_SECONDARY, LinearGradientMode.Vertical))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }
    }
}