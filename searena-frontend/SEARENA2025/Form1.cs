// Form1.cs - Login dan Register (PERBAIKAN)
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace SEARENA2025
{
    public partial class Form1 : Form
    {
        // === DEKLARASI KONTROL ===
        private Guna2BorderlessForm _gunaBorderlessForm;
        private Guna2Panel _mainPanel;
        private Label _lblTitle;
        private Guna2Button _btnTabMasuk, _btnTabDaftar;

        // Login Form Controls
        private Guna2TextBox _txtEmail, _txtPassword;
        private Guna2CheckBox _chkIngatSaya;
        private LinkLabel _lnkLupaSandi;
        private Guna2Button _btnMasuk;
        private Label _lblAtauMasuk;
        private Guna2ImageButton _btnGoogleLogin, _btnAppleLogin, _btnFacebookLogin;

        // Register Form Controls
        private Guna2TextBox _txtRegNama, _txtRegEmail, _txtRegPassword;
        private Guna2CheckBox _chkSetuju;
        private Guna2Button _btnDaftar;
        private Label _lblAtauDaftar;
        private Guna2ImageButton _btnGoogleReg, _btnAppleReg, _btnFacebookReg;

        // === KONSTANTA UKURAN & WARNA ===
        private const int PANEL_WIDTH = 400;
        private const int CONTENT_WIDTH = 330;
        private const int FORM_WIDTH = 900;
        private const int FORM_HEIGHT = 600;
        private const int PANEL_HEIGHT_LOGIN = 420;
        private const int PANEL_HEIGHT_REGISTER = 480;
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

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConfigureForm();
            InitializeGunaBorderlessForm();
            CreateMainPanel();
            CreateTitleLabel();
            CreateTabButtons();
            CreateLoginForm();
            CreateRegisterForm();
            ShowLoginForm();
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

        private void CreateMainPanel()
        {
            _mainPanel = new Guna2Panel
            {
                Size = new Size(PANEL_WIDTH, PANEL_HEIGHT_LOGIN),
                Location = new Point((this.Width - PANEL_WIDTH) / 2, (this.Height - PANEL_HEIGHT_LOGIN) / 2 + 30),
                BackColor = Color.Transparent,
                BorderRadius = BORDER_RADIUS,
                FillColor = COLOR_PANEL_BG
            };
            this.Controls.Add(_mainPanel);
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
                Height = 50
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
            int currentY = 90;

            // Email TextBox dengan Placeholder YANG BENAR
            _txtEmail = new Guna2TextBox
            {
                Size = new Size(CONTENT_WIDTH, 55),
                Location = new Point(contentStartX, currentY),
                BorderRadius = 10,
                Font = new Font("Segoe UI", 10F),
                IconLeft = Image.FromFile("Icons/gridicons_mail.png"),
                IconLeftOffset = new Point(12, 0),
                TextOffset = new Point(5, 10),
                PlaceholderText = "Alamat E-mail",  // PERBAIKAN: Gunakan PlaceholderText
                BorderColor = Color.FromArgb(220, 220, 220),
                FocusedState = { BorderColor = COLOR_ACCENT_CYAN_DARK },
                FillColor = Color.White
            };
            _txtEmail.Controls.Add(CreateInnerLabel("Alamat E-mail", 45, 5));
            _mainPanel.Controls.Add(_txtEmail);

            currentY += _txtEmail.Height + 10;

            // Password TextBox dengan Placeholder YANG BENAR
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
                PlaceholderText = "Kata Sandi",  // PERBAIKAN: Gunakan PlaceholderText
                BorderColor = Color.FromArgb(220, 220, 220),
                FocusedState = { BorderColor = COLOR_ACCENT_CYAN_DARK },
                FillColor = Color.White
            };
            _txtPassword.IconRightClick += (s, e) => TogglePasswordVisibility(_txtPassword);
            _txtPassword.Controls.Add(CreateInnerLabel("Kata Sandi", 45, 5));
            _mainPanel.Controls.Add(_txtPassword);

            currentY += _txtPassword.Height + 20;

            _chkIngatSaya = new Guna2CheckBox
            {
                Text = "Ingat Saya",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = COLOR_TEXT_DARK,
                BackColor = Color.Transparent,
                Location = new Point(contentStartX, currentY),
                AutoSize = true
            };
            _chkIngatSaya.CheckedState.BorderColor = COLOR_ACCENT_ORANGE;
            _chkIngatSaya.CheckedState.FillColor = COLOR_ACCENT_ORANGE;
            _mainPanel.Controls.Add(_chkIngatSaya);

            _lnkLupaSandi = new LinkLabel
            {
                Text = "Lupa Kata Sandi",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                LinkColor = COLOR_LINK,
                BackColor = Color.Transparent,
                AutoSize = true,
                Cursor = Cursors.Hand
            };
            _lnkLupaSandi.Location = new Point(contentStartX + CONTENT_WIDTH - _lnkLupaSandi.Width, currentY + 2);
            _lnkLupaSandi.Click += (s, e) => MessageBox.Show("Fitur lupa kata sandi akan segera tersedia", "Lupa Kata Sandi");
            _mainPanel.Controls.Add(_lnkLupaSandi);

            currentY += _chkIngatSaya.Height + 20;

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

            currentY += _btnMasuk.Height + 15;

            _lblAtauMasuk = new Label
            {
                Text = "Atau Masuk Dengan",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = COLOR_TEXT_LIGHT,
                BackColor = Color.Transparent,
                AutoSize = true
            };
            _lblAtauMasuk.Location = new Point((PANEL_WIDTH - _lblAtauMasuk.Width) / 2, currentY);
            _mainPanel.Controls.Add(_lblAtauMasuk);

            currentY += _lblAtauMasuk.Height + 8;

            int buttonSize = 35;
            int spacing = 18;
            int totalSocialWidth = buttonSize * 3 + spacing * 2;
            int socialStartX = (PANEL_WIDTH - totalSocialWidth) / 2;

            _btnGoogleLogin = CreateSocialButton(socialStartX, currentY, "Icons/material-icon-theme_google.png", buttonSize);
            _btnAppleLogin = CreateSocialButton(socialStartX + buttonSize + spacing, currentY, "Icons/streamline-logos_apple-logo-block.png", buttonSize);
            _btnFacebookLogin = CreateSocialButton(socialStartX + (buttonSize + spacing) * 2, currentY, "Icons/devicon_facebook.png", buttonSize);

            _mainPanel.Controls.Add(_btnGoogleLogin);
            _mainPanel.Controls.Add(_btnAppleLogin);
            _mainPanel.Controls.Add(_btnFacebookLogin);
        }

        private void CreateRegisterForm()
        {
            int contentStartX = (PANEL_WIDTH - CONTENT_WIDTH) / 2;
            int currentY = 90;

            // Nama TextBox dengan Placeholder
            _txtRegNama = new Guna2TextBox
            {
                Size = new Size(CONTENT_WIDTH, 55),
                Location = new Point(contentStartX, currentY),
                BorderRadius = 10,
                Font = new Font("Segoe UI", 10F),
                IconLeft = Image.FromFile("Icons/Vector.png"),
                IconLeftOffset = new Point(12, 0),
                TextOffset = new Point(5, 10),
                PlaceholderText = "Nama Lengkap",  // PERBAIKAN
                BorderColor = Color.FromArgb(220, 220, 220),
                FocusedState = { BorderColor = COLOR_ACCENT_CYAN_DARK },
                FillColor = Color.White,
                Visible = false
            };
            _txtRegNama.Controls.Add(CreateInnerLabel("Nama Lengkap", 45, 5));
            _mainPanel.Controls.Add(_txtRegNama);

            currentY += _txtRegNama.Height + 10;

            // Email TextBox dengan Placeholder
            _txtRegEmail = new Guna2TextBox
            {
                Size = new Size(CONTENT_WIDTH, 55),
                Location = new Point(contentStartX, currentY),
                BorderRadius = 10,
                Font = new Font("Segoe UI", 10F),
                IconLeft = Image.FromFile("Icons/gridicons_mail.png"),
                IconLeftOffset = new Point(12, 0),
                TextOffset = new Point(5, 10),
                PlaceholderText = "Alamat E-mail",  // PERBAIKAN
                BorderColor = Color.FromArgb(220, 220, 220),
                FocusedState = { BorderColor = COLOR_ACCENT_CYAN_DARK },
                FillColor = Color.White,
                Visible = false
            };
            _txtRegEmail.Controls.Add(CreateInnerLabel("Alamat E-mail", 45, 5));
            _mainPanel.Controls.Add(_txtRegEmail);

            currentY += _txtRegEmail.Height + 10;

            // Password TextBox dengan Placeholder
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
                PlaceholderText = "Kata Sandi",  // PERBAIKAN
                BorderColor = Color.FromArgb(220, 220, 220),
                FocusedState = { BorderColor = COLOR_ACCENT_CYAN_DARK },
                FillColor = Color.White,
                Visible = false
            };
            _txtRegPassword.IconRightClick += (s, e) => TogglePasswordVisibility(_txtRegPassword);
            _txtRegPassword.Controls.Add(CreateInnerLabel("Kata Sandi", 45, 5));
            _mainPanel.Controls.Add(_txtRegPassword);

            currentY += _txtRegPassword.Height + 25;

            _chkSetuju = new Guna2CheckBox
            {
                Text = "Saya menyetujui syarat dan ketentuan",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = COLOR_TEXT_DARK,
                BackColor = Color.Transparent,
                Location = new Point(contentStartX, currentY),
                AutoSize = true,
                Visible = false
            };
            _chkSetuju.CheckedState.BorderColor = COLOR_ACCENT_ORANGE;
            _chkSetuju.CheckedState.FillColor = COLOR_ACCENT_ORANGE;
            _mainPanel.Controls.Add(_chkSetuju);

            currentY += _chkSetuju.Height + 20;

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

            currentY += _btnDaftar.Height + 15;

            _lblAtauDaftar = new Label
            {
                Text = "Atau Daftar Dengan",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = COLOR_TEXT_LIGHT,
                BackColor = Color.Transparent,
                AutoSize = true,
                Visible = false
            };
            _lblAtauDaftar.Location = new Point((PANEL_WIDTH - _lblAtauDaftar.Width) / 2, currentY);
            _mainPanel.Controls.Add(_lblAtauDaftar);

            currentY += _lblAtauDaftar.Height + 8;

            int buttonSize = 35;
            int spacing = 18;
            int totalSocialWidth = buttonSize * 3 + spacing * 2;
            int socialStartX = (PANEL_WIDTH - totalSocialWidth) / 2;

            _btnGoogleReg = CreateSocialButton(socialStartX, currentY, "Icons/material-icon-theme_google.png", buttonSize);
            _btnAppleReg = CreateSocialButton(socialStartX + buttonSize + spacing, currentY, "Icons/streamline-logos_apple-logo-block.png", buttonSize);
            _btnFacebookReg = CreateSocialButton(socialStartX + (buttonSize + spacing) * 2, currentY, "Icons/devicon_facebook.png", buttonSize);

            _btnGoogleReg.Visible = false;
            _btnAppleReg.Visible = false;
            _btnFacebookReg.Visible = false;

            _mainPanel.Controls.Add(_btnGoogleReg);
            _mainPanel.Controls.Add(_btnAppleReg);
            _mainPanel.Controls.Add(_btnFacebookReg);
        }

        private void ShowLoginForm()
        {
            _mainPanel.Size = new Size(PANEL_WIDTH, PANEL_HEIGHT_LOGIN);
            _mainPanel.Location = new Point((this.Width - PANEL_WIDTH) / 2, (this.Height - PANEL_HEIGHT_LOGIN) / 2 + 30);
            _lblTitle.Text = "Hai, Sea-Mates!";
            _lblTitle.Location = new Point(_mainPanel.Left, _mainPanel.Top - 65);

            _btnTabMasuk.FillColor = COLOR_ACCENT_CYAN;
            _btnTabMasuk.ForeColor = Color.Black;
            _btnTabMasuk.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            _btnTabDaftar.FillColor = Color.Transparent;
            _btnTabDaftar.ForeColor = COLOR_TEXT_LIGHT;
            _btnTabDaftar.Font = new Font("Segoe UI", 11F);

            SetControlsVisibility(true, _txtEmail, _txtPassword, _chkIngatSaya, _lnkLupaSandi, _btnMasuk, _lblAtauMasuk, _btnGoogleLogin, _btnAppleLogin, _btnFacebookLogin);
            SetControlsVisibility(false, _txtRegNama, _txtRegEmail, _txtRegPassword, _chkSetuju, _btnDaftar, _lblAtauDaftar, _btnGoogleReg, _btnAppleReg, _btnFacebookReg);
        }

        private void ShowRegisterForm()
        {
            _mainPanel.Size = new Size(PANEL_WIDTH, PANEL_HEIGHT_REGISTER);
            _mainPanel.Location = new Point((this.Width - PANEL_WIDTH) / 2, (this.Height - PANEL_HEIGHT_REGISTER) / 2 + 30);
            _lblTitle.Text = "Searena";
            _lblTitle.Location = new Point(_mainPanel.Left, _mainPanel.Top - 65);

            _btnTabDaftar.FillColor = COLOR_ACCENT_CYAN;
            _btnTabDaftar.ForeColor = Color.Black;
            _btnTabDaftar.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            _btnTabMasuk.FillColor = Color.Transparent;
            _btnTabMasuk.ForeColor = COLOR_TEXT_LIGHT;
            _btnTabMasuk.Font = new Font("Segoe UI", 11F);

            SetControlsVisibility(false, _txtEmail, _txtPassword, _chkIngatSaya, _lnkLupaSandi, _btnMasuk, _lblAtauMasuk, _btnGoogleLogin, _btnAppleLogin, _btnFacebookLogin);
            SetControlsVisibility(true, _txtRegNama, _txtRegEmail, _txtRegPassword, _chkSetuju, _btnDaftar, _lblAtauDaftar, _btnGoogleReg, _btnAppleReg, _btnFacebookReg);
        }

        // === METHOD LOGIN & REGISTER DENGAN DATABASE ===
        private void Login()
        {
            // Validasi input
            if (string.IsNullOrWhiteSpace(_txtEmail.Text))
            {
                MessageBox.Show("Email harus diisi", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtEmail.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(_txtPassword.Text))
            {
                MessageBox.Show("Password harus diisi", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtPassword.Focus();
                return;
            }

            // Login dengan database
            var user = User.Login(_txtEmail.Text.Trim(), _txtPassword.Text);

            if (user != null)
            {
                MessageBox.Show($"Selamat datang, {user.NamaLengkap}!", "Login Berhasil",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                PindahKeDashboard();
            }
            else
            {
                MessageBox.Show("Email atau password salah", "Login Gagal",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Register()
        {
            // Validasi input
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

            if (!_chkSetuju.Checked)
            {
                MessageBox.Show("Harus menyetujui syarat dan ketentuan", "Validasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Registrasi dengan database
            var newUser = new User
            {
                NamaLengkap = _txtRegNama.Text.Trim(),
                Email = _txtRegEmail.Text.Trim(),
                Password = _txtRegPassword.Text,
                NoTelepon = ""
            };

            if (newUser.Save())
            {
                MessageBox.Show("Registrasi berhasil! Silakan login dengan akun Anda.", "Sukses",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Pindah ke form login
                ShowLoginForm();
                _txtEmail.Text = newUser.Email;
                _txtPassword.Focus();
            }
            else
            {
                MessageBox.Show("Registrasi gagal. Email mungkin sudah terdaftar.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PindahKeDashboard()
        {
            DashboardUtama dashboard = new DashboardUtama();
            dashboard.FormClosed += (s, args) => Application.Exit(); // Keluar aplikasi jika dashboard ditutup
            dashboard.Show();
            this.Hide(); // Sembunyikan form login, jangan Close()
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

        private Guna2ImageButton CreateSocialButton(int x, int y, string imagePath, int size)
        {
            var button = new Guna2ImageButton
            {
                Image = Image.FromFile(imagePath),
                ImageSize = new Size(28, 28),
                Location = new Point(x, y),
                Size = new Size(size, size),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                HoverState = { ImageSize = new Size(30, 30) },
                PressedState = { ImageSize = new Size(26, 26) }
            };

            button.Click += (s, e) =>
            {
                MessageBox.Show($"Login dengan {System.IO.Path.GetFileNameWithoutExtension(imagePath)} akan segera tersedia",
                    "Social Login");
            };

            return button;
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