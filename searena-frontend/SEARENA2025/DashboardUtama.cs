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
using Npgsql;

namespace SEARENA2025
{
    public partial class DashboardUtama : Form
    {
        // === FIELD & MODEL ===
        // Panel destinasi lama (kalau ada di designer)
        private Guna2ShadowPanel panelDestinasi1;

        // Panel baru untuk kartu destinasi
        private FlowLayoutPanel flpDestinasi; // FlowLayoutPanel untuk menampung kartu destinasi
        private List<Destinasi> allDestinasi = new List<Destinasi>(); // Menyimpan semua destinasi

        // Untuk fitur sugesti search (listbox)
        private List<DestInfo> _allDestinasi = new List<DestInfo>();

        internal sealed class DestInfo
        {
            public int Id { get; set; }
            public string Nama { get; set; }
            public string Lokasi { get; set; }


            public override string ToString()
            {
                return $"{Nama} - {Lokasi}";
            }
        }

        public static class AppDb
        {
            // Satu tempat untuk connection string
            public static readonly string ConnString =
                "Host=aws-1-ap-southeast-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.eeqqiyfukvhbwystupei;Password=SearenaDB123";
        }

        public DashboardUtama()
        {
            InitializeComponent();

            // Sembunyikan panel destinasi manual dulu (yang hardcoded di designer)
            HideManualDestinationPanels();

            SetupEventHandlers();
            SetupScrollBars();
            SetupRatingComboBox();

            InitializeDestinasiPanel();

        }

        // ================== INIT & LAYOUT ==================

        private void HideManualDestinationPanels()
        {
            // Sembunyikan semua panel destinasi manual yang ada di designer
            foreach (Control control in this.Controls)
            {
                if (control is Guna2ShadowPanel panel && control.Name.StartsWith("panelDestinasi"))
                {
                    panel.Visible = false;
                }
            }

            // Cek juga di dalam container
            foreach (Control container in this.Controls)
            {
                if (container is Guna2Panel || container is Panel)
                {
                    foreach (Control control in container.Controls)
                    {
                        if (control is Guna2ShadowPanel panel && control.Name.StartsWith("panelDestinasi"))
                        {
                            panel.Visible = false;
                        }
                    }
                }
            }
        }

        private void InitializeDestinasiPanel()
        {
            // Cari FlowLayoutPanel yang sudah ada di designer
            flpDestinasi = this.Controls.OfType<FlowLayoutPanel>().FirstOrDefault(f => f.Name == "flpDestinasi");

            if (flpDestinasi == null)
            {
                // Cari di dalam container panels
                foreach (Control container in this.Controls)
                {
                    if (container is Guna2Panel || container is Panel)
                    {
                        flpDestinasi = container.Controls.OfType<FlowLayoutPanel>()
                            .FirstOrDefault(f => f.Name == "flpDestinasi");
                        if (flpDestinasi != null) break;
                    }
                }
            }

            if (flpDestinasi == null)
            {
                // Cari footer panel untuk menghitung tinggi yang tersedia (gunakan nama pasti PnlInformasi)
                var footerPanel = this.Controls.OfType<Control>()
                    .FirstOrDefault(c => c.Name == "PnlInformasi");

                // Cari posisi referensi dari control yang ada (navbar, filter panel, dll)
                int xPosition = 280;
                int yPosition = 100; // Posisi awal supaya kartu agak ke atas

                // Hitung tinggi yang tersedia (jangan sampai memotong footer)
                int availableHeight;
                const int safeMarginToFooter = 60; // margin aman supaya tidak menempel dengan footer

                if (footerPanel != null)
                {
                    // Tinggi panel destinasi = posisi atas footer - posisi Y panel destinasi - margin aman
                    availableHeight = Math.Max(200, footerPanel.Top - yPosition - safeMarginToFooter);
                }
                else
                {
                    // Fallback jika footer tidak ditemukan: gunakan nilai aman
                    availableHeight = 380; // dipendekkan supaya tidak memotong footer
                }

                // Coba cari panel filter untuk posisi relatif
                var filterPanel = this.Controls.OfType<Control>()
                    .FirstOrDefault(c => c.Name == "PnlFilter");

                if (filterPanel != null)
                {
                    xPosition = filterPanel.Right + 20;
                    // Pastikan panel destinasi tidak lebih rendah dari filter, tapi tetap minimal 100
                    yPosition = Math.Max(100, filterPanel.Top);

                    // Hitung ulang tinggi jika yPosition berubah
                    if (footerPanel != null)
                    {
                        availableHeight = Math.Max(200, footerPanel.Top - yPosition - safeMarginToFooter);
                    }
                }

                flpDestinasi = new FlowLayoutPanel
                {
                    Name = "flpDestinasi",
                    AutoScroll = true,
                    WrapContents = true,
                    FlowDirection = FlowDirection.LeftToRight,
                    Padding = new Padding(10, 5, 10, 10), // kecilkan padding atas
                    BackColor = Color.Transparent,
                    Location = new Point(xPosition, yPosition),
                    Size = new Size(this.Width - xPosition - 40, availableHeight),
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                };
                this.Controls.Add(flpDestinasi);
                flpDestinasi.BringToFront();
            }
            else
            {
                // FlowLayoutPanel sudah ada, pastikan visible dan di depan
                flpDestinasi.Visible = true;
                flpDestinasi.AutoScroll = true; // pastikan scroll aktif
                flpDestinasi.Padding = new Padding(10, 5, 10, 10);
                flpDestinasi.BringToFront();
            }

            // Load destinasi dari database
            LoadDestinasiFromDatabase();
        }

        private async void LoadDestinasiFromDatabase()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                // 1) Ambil data dari DB (di thread pool)
                allDestinasi = await Task.Run(() => Destinasi.GetAll());

                // 2) Siapkan list untuk sugesti pencarian
                _allDestinasi = allDestinasi
                    .Select(d => new DestInfo
                    {
                        Id = d.Id,
                        Nama = d.NamaDestinasi,
                        Lokasi = d.Lokasi
                    })
                    .ToList();

                // 3) Pastikan panel ada dan bersih
                if (flpDestinasi == null) return;
                flpDestinasi.SuspendLayout();
                flpDestinasi.Controls.Clear();

                // 4) Render
                if (allDestinasi.Count == 0)
                {
                    flpDestinasi.Controls.Add(new Label
                    {
                        Text = "Belum ada destinasi tersedia.\nTambahkan destinasi melalui halaman Admin.",
                        Font = new Font("Segoe UI", 12, FontStyle.Italic),
                        ForeColor = Color.Gray,
                        AutoSize = true,
                        TextAlign = ContentAlignment.MiddleCenter
                    });
                }
                else
                {
                    // Pilih salah satu:
                    // a) Terapkan filter/urutan saat ini:
                    ApplyFilters();

                    // b) ATAU kalau ingin render semua dulu:
                    // RebuildDestinasiCards(allDestinasi);
                }

                flpDestinasi.ResumeLayout();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading destinasi:\n{ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        // Event handler terpisah untuk mencegah multiple subscription
        private void Card_OnCardClicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is DestinasiCard card && card.Tag is Destinasi destinasi)
                {
                    OpenDetailDestinasi(destinasi);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error handling card click: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenDetailDestinasi(Destinasi destinasi)
        {
            try
            {
                var detailForm = new DetailDestinasi(
                    destinasi.Id,
                    destinasi.NamaDestinasi,
                    destinasi.Lokasi
                );
                
                // PERBAIKI: Close form saat ini sebelum show form baru
                detailForm.FormClosed += (s, args) => this.Show();
                detailForm.Show();
                this.Hide(); // Hide bukan Close agar bisa di-show lagi
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error membuka detail destinasi:\n{ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================== EVENT HANDLERS SETUP ==================

        private void SetupEventHandlers()
        {
            // Profile
            if (PctProfile != null)
                PctProfile.Click += (s, e) => OpenProfilePage();

            // Hapus filter
            if (LblHapusFilter != null)
                LblHapusFilter.Click += (s, e) => ClearFilters();

            // Textbox search lama (txtCariDestinasi) -> filter kartu
            if (txtCariDestinasi != null)
            {
                txtCariDestinasi.TextChanged += (s, e) => SearchDestinations();
                txtCariDestinasi.Enter += (s, e) =>
                {
                    if (txtCariDestinasi.Text == "Cari destinasi ...")
                    {
                        txtCariDestinasi.Text = "";
                        txtCariDestinasi.ForeColor = Color.Black;
                    }
                };
                txtCariDestinasi.Leave += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(txtCariDestinasi.Text))
                    {
                        txtCariDestinasi.Text = "Cari destinasi ...";
                        txtCariDestinasi.ForeColor = Color.Gray;
                    }
                };
            }
            else
            {
                // Kalau textbox di dalam panel search
                FindAndSetupSearchTextBox();
            }

            // TextBox search baru + suggestion (tbSearchDestinasi + lstSugesti)
            if (tbSearchDestinasi != null)
            {
                tbSearchDestinasi.TextChanged -= TbSearchDestinasi_TextChanged; // Hindari duplikasi
                tbSearchDestinasi.TextChanged += TbSearchDestinasi_TextChanged;
                tbSearchDestinasi.KeyDown -= TbSearchDestinasi_KeyDown;
                tbSearchDestinasi.KeyDown += TbSearchDestinasi_KeyDown;
                tbSearchDestinasi.PlaceholderText = "Cari destinasi ...";
            }

            if (lstSugesti != null)
            {
                lstSugesti.Visible = false;
                lstSugesti.DoubleClick -= OpenSelectedSuggestion_Handler; // Hindari duplikasi
                lstSugesti.DoubleClick += OpenSelectedSuggestion_Handler;
                lstSugesti.KeyDown -= LstSugesti_KeyDown;
                lstSugesti.KeyDown += LstSugesti_KeyDown;
                lstSugesti.Font = new Font("Segoe UI", 9f);
                
                // PENTING: Pastikan lstSugesti berada di parent yang sama dengan tbSearchDestinasi
                if (tbSearchDestinasi != null && lstSugesti.Parent != this)
                {
                    lstSugesti.Parent?.Controls.Remove(lstSugesti);
                    this.Controls.Add(lstSugesti);
                    lstSugesti.BringToFront();
                }
            }

            // Combo rating
            if (CmbRating != null)
            {
                CmbRating.SelectedIndexChanged += (s, e) => ApplyFilters();
            }

            // Checkbox filter pulau
            if (PnlPulau != null)
            {
                foreach (Control control in PnlPulau.Controls)
                {
                    if (control is Guna2CheckBox checkBox)
                    {
                        checkBox.CheckedChanged += (s, e) => ApplyFilters();
                    }
                }
            }

            // Checkbox filter aktivitas
            if (PnlAktivitas != null)
            {
                foreach (Control control in PnlAktivitas.Controls)
                {
                    if (control is Guna2CheckBox checkBox)
                    {
                        checkBox.CheckedChanged += (s, e) => ApplyFilters();
                    }
                }
            }
        }

        private void FindAndSetupSearchTextBox()
        {
            // Cari textbox search di dalam PnlSearch
            if (PnlSearch != null)
            {
                foreach (Control control in PnlSearch.Controls)
                {
                    if (control is Guna2TextBox textBox)
                    {
                        textBox.TextChanged += (s, e) => SearchDestinations();
                        textBox.Enter += (s, e) =>
                        {
                            if (textBox.Text == "Cari destinasi ...")
                            {
                                textBox.Text = "";
                                textBox.ForeColor = Color.Black;
                            }
                        };
                        textBox.Leave += (s, e) =>
                        {
                            if (string.IsNullOrWhiteSpace(textBox.Text))
                            {
                                textBox.Text = "Cari destinasi ...";
                                textBox.ForeColor = Color.Gray;
                            }
                        };
                        break;
                    }
                }
            }
        }

        private void SetupScrollBars()
        {
           
        }

        private void SetupRatingComboBox()
        {
            if (CmbRating != null)
            {
                CmbRating.Items.Clear();
                CmbRating.Items.Add("Populer");
                CmbRating.Items.Add("Rating Tertinggi");
                CmbRating.Items.Add("Rating Terendah");
                CmbRating.Items.Add("Rekomendasi");
                CmbRating.SelectedIndex = 0;
            }
        }

        // ================== NAVIGASI SIMPLE ==================

        private void ScrollToTop()
        {
            this.ActiveControl = null;
        }

        private void ScrollToDestinations()
        {
            if (panelDestinasi1 != null)
                panelDestinasi1.Focus();
        }

        private void ScrollToAbout()
        {
            if (PnlInformasi != null)
            {
                PnlInformasi.Visible = true;
                PnlInformasi.BringToFront();
            }
        }

        private void ScrollToContact()
        {
            if (PnlInformasi != null)
            {
                PnlInformasi.Visible = true;
                PnlInformasi.BringToFront();
            }
        }

        private void OpenProfilePage()
        {
            try
            {
                Form2 profileForm = new Form2(this); // PERBAIKI: Kirim reference parent form
                profileForm.FormClosed += (s, args) => this.Show();
                profileForm.Show();
                this.Hide(); // Hide saja, jangan Close
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error membuka halaman profile: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================== SEARCH & FILTER (KARTU) ==================

        private void SearchDestinations()
        {
            string searchText = "";

            if (tbSearchDestinasi != null && !string.IsNullOrEmpty(tbSearchDestinasi.Text))
            {
                searchText = tbSearchDestinasi.Text.ToLower();
            }
            else if (txtCariDestinasi != null)
            {
                searchText = txtCariDestinasi.Text.ToLower();
            }
            else
            {
                searchText = GetSearchTextFromPnlSearch();
            }

            if (searchText == "cari destinasi ..." || string.IsNullOrWhiteSpace(searchText))
            {
                ShowAllDestinasiCards();
                return;
            }

            FilterDestinasiCardsBySearch(searchText);
        }

        private void ShowAllDestinasiCards()
        {
            if (flpDestinasi == null) return;

            foreach (Control control in flpDestinasi.Controls)
            {
                control.Visible = true;
            }
        }

        private void FilterDestinasiCardsBySearch(string searchText)
        {
            if (flpDestinasi == null) return;

            int foundCount = 0;

            foreach (Control control in flpDestinasi.Controls)
            {
                if (control is DestinasiCard card)
                {
                    bool found = card.NamaDestinasi.ToLower().Contains(searchText) ||
                                card.Lokasi.ToLower().Contains(searchText) ||
                                card.Pulau.ToLower().Contains(searchText);

                    card.Visible = found;
                    if (found) foundCount++;
                }
            }

            if (foundCount == 0)
            {
                MessageBox.Show($"Tidak ditemukan destinasi dengan kata kunci: '{searchText}'",
                              "Pencarian", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string GetSearchTextFromPnlSearch()
        {
            if (PnlSearch != null)
            {
                foreach (Control control in PnlSearch.Controls)
                {
                    if (control is Guna2TextBox textBox)
                    {
                        return textBox.Text.ToLower();
                    }
                }
            }
            return "";
        }

        private void ApplyFilters()
        {
            if (flpDestinasi == null) return;

            // 1. Ambil filter dari checkbox
            var selectedPulau = GetSelectedPulau();        // sudah ada fungsinya
            var selectedAktivitas = GetSelectedAktivitas(); // juga sudah ada

            // 2. Ambil pilihan urutan
            string sortBy = CmbRating?.SelectedItem?.ToString() ?? "Populer";

            // 3. Mulai dari semua destinasi (data dari DB)
            IEnumerable<Destinasi> query = allDestinasi;

            // --- FILTER PULAU (Bali, Jawa, dll) ---
            if (selectedPulau.Any())
            {
                // pastikan teks di checkbox sama dengan nilai di kolom pulau (Bali, Jawa, dst)
                query = query.Where(d => selectedPulau.Contains(d.Pulau));
            }

            // --- FILTER AKTIVITAS (Sunset, Snorkeling, Diving, Camping) ---
            if (selectedAktivitas.Any())
            {
                query = query.Where(d =>
                    !string.IsNullOrWhiteSpace(d.Activity) &&          // nama properti sesuaikan dengan model Destinasi kamu
                    selectedAktivitas.Any(a =>
                        d.Activity
                            .Split(',')                                // "Snorkeling,Sunset" → ["Snorkeling","Sunset"]
                            .Select(x => x.Trim())
                            .Contains(a)
                    )
                );
            }

            // 4. SORTING
            switch (sortBy)
            {
                case "Rating Tertinggi":
                    query = query.OrderByDescending(d => d.RatingAvg);
                    break;
                case "Rating Terendah":
                    query = query.OrderBy(d => d.RatingAvg);
                    break;
                case "Rekomendasi":
                    // contoh: rating tinggi + review banyak
                    query = query
                        .OrderByDescending(d => d.RatingAvg)
                        .ThenByDescending(d => d.TotalReview);
                    break;
                case "Populer":
                default:
                    // paling banyak review dianggap paling populer
                    query = query.OrderByDescending(d => d.TotalReview);
                    break;
            }

            var result = query.ToList();

            // 5. Bangun ulang kartu di FlowLayoutPanel
            RebuildDestinasiCards(result);
        }

        private void RebuildDestinasiCards(List<Destinasi> sortedDestinasi)
        {
            if (flpDestinasi == null) return;

            flpDestinasi.SuspendLayout();
            flpDestinasi.Controls.Clear();

            foreach (var destinasi in sortedDestinasi)
            {
                var card = new DestinasiCard(destinasi);
                card.Margin = new Padding(10);
                card.CardClicked += (s, e) => OpenDetailDestinasi(destinasi);
                flpDestinasi.Controls.Add(card);
            }

            flpDestinasi.ResumeLayout();
        }

        // FILTER LOGIC
        private List<string> GetSelectedPulau()
        {
            List<string> selectedPulau = new List<string>();
            if (PnlPulau == null) return selectedPulau;

            foreach (Control control in PnlPulau.Controls)
            {
                if (control is Guna2CheckBox checkBox && checkBox.Checked)
                {
                    selectedPulau.Add(checkBox.Text);
                }
            }
            return selectedPulau;
        }

        private List<string> GetSelectedAktivitas()
        {
            List<string> selectedAktivitas = new List<string>();
            if (PnlAktivitas == null) return selectedAktivitas;

            foreach (Control control in PnlAktivitas.Controls)
            {
                if (control is Guna2CheckBox checkBox && checkBox.Checked)
                {
                    selectedAktivitas.Add(checkBox.Text);
                }
            }
            return selectedAktivitas;
        }

        // ================== SEARCH SUGGESTION SUPPORT ==================
        private void TbSearchDestinasi_TextChanged(object sender, EventArgs e)
        {
            if (tbSearchDestinasi == null || lstSugesti == null) return;

            string text = tbSearchDestinasi.Text.Trim();

            if (string.IsNullOrEmpty(text))
            {
                lstSugesti.Visible = false;
                lstSugesti.Items.Clear();
            }
            else
            {
                // Cari destinasi yang cocok dengan input
                var matches = _allDestinasi
                    .Where(d =>
                        d.Nama.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        d.Lokasi.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0)
                    .Take(5) // Batasi 5 suggestion
                    .ToList();

                lstSugesti.BeginUpdate();
                lstSugesti.Items.Clear();

                foreach (var d in matches)
                    lstSugesti.Items.Add(d);

                lstSugesti.EndUpdate();
                lstSugesti.Visible = matches.Count > 0;

                // PERBAIKI: Atur posisi lstSugesti tepat di bawah tbSearchDestinasi
                if (lstSugesti.Visible && tbSearchDestinasi != null)
                {
                    // Konversi koordinat tbSearchDestinasi ke koordinat form
                    Point searchBoxLocation = tbSearchDestinasi.PointToScreen(Point.Empty);
                    Point formLocation = this.PointToScreen(Point.Empty);
                    
                    // Hitung posisi relatif terhadap form
                    int relativeX = searchBoxLocation.X - formLocation.X;
                    int relativeY = searchBoxLocation.Y - formLocation.Y + tbSearchDestinasi.Height + 2;
                    
                    lstSugesti.Location = new Point(relativeX, relativeY);
                    lstSugesti.Width = tbSearchDestinasi.Width;
                    lstSugesti.Height = 120; // Tinggi tetap untuk 5 suggestion
                    lstSugesti.BringToFront();
                }
            }

            // Tetap jalankan search untuk filter cards
            SearchDestinations();
        }

        private void TbSearchDestinasi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && lstSugesti != null && lstSugesti.Visible && lstSugesti.Items.Count > 0)
            {
                lstSugesti.Focus();
                lstSugesti.SelectedIndex = 0;
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                OpenSelectedSuggestionFromFirst();
                e.Handled = true;
            }
        }

        private void LstSugesti_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                OpenSelectedSuggestion();
                e.Handled = true;
            }
        }

        private void OpenSelectedSuggestionFromFirst()
        {
            if (lstSugesti != null && lstSugesti.Items.Count > 0)
            {
                lstSugesti.SelectedIndex = 0;
                OpenSelectedSuggestion();
            }
        }

        private void OpenSelectedSuggestion()
        {
            if (lstSugesti?.SelectedItem is DestInfo info)
            {
                // Cari Destinasi asli berdasarkan Id
                var dest = allDestinasi.FirstOrDefault(d => d.Id == info.Id);
                if (dest != null)
                {
                    OpenDetailDestinasi(dest);
                }

                // Sembunyikan suggestion list
                lstSugesti.Visible = false;
                if (tbSearchDestinasi != null)
                    tbSearchDestinasi.Text = info.Nama;
            }
        }

        // Handler untuk DoubleClick event
        private void OpenSelectedSuggestion_Handler(object sender, EventArgs e)
        {
            OpenSelectedSuggestion();
        }

        // ================== CLEAR FILTERS SUPPORT ==================
        private void ClearFilters()
        {
            if (CmbRating != null)
                CmbRating.SelectedIndex = 0;
            if (PnlPulau != null)
            {
                foreach (Control control in PnlPulau.Controls)
                {
                    if (control is Guna2CheckBox cb) cb.Checked = false;
                }
            }
            if (PnlAktivitas != null)
            {
                foreach (Control control in PnlAktivitas.Controls)
                {
                    if (control is Guna2CheckBox cb) cb.Checked = false;
                }
            }
            if (tbSearchDestinasi != null) tbSearchDestinasi.Text = string.Empty;
            if (lstSugesti != null) { lstSugesti.Visible = false; lstSugesti.Items.Clear(); }
            ShowAllDestinasiCards();
        }

        // ====== EVENT HANDLERS REQUIRED BY DESIGNER (STUBS) ======
        private void guna2ShadowPanel2_Paint(object sender, PaintEventArgs e) { }
        private void guna2HtmlLabel25_Click(object sender, EventArgs e) { }
        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e) { }
        private void CheckBoxSulawesi_CheckedChanged(object sender, EventArgs e) { }
        private void guna2HtmlLabel26_Click(object sender, EventArgs e) { }
        private void CheckBoxSunset_CheckedChanged(object sender, EventArgs e) { }
        private void guna2HtmlLabel7_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel3_Click(object sender, EventArgs e) { }
        private void tbSearchDestinasi_TextChanged_1(object sender, EventArgs e) { }
        private void Navbar_Paint(object sender, PaintEventArgs e) { }
        private void lblProfile_Click(object sender, EventArgs e) { }
        private void DashboardUtama_Load(object sender, EventArgs e)
        {
            if (txtCariDestinasi != null)
                txtCariDestinasi.ForeColor = Color.Gray;
            if (lstSugesti != null)
                lstSugesti.Visible = false;
        }

        private void guna2CheckBox1_CheckedChanged_1(object sender, EventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void lstSugesti_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
