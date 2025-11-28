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

            // Inisialisasi panel destinasi dari database (pakai FlowLayoutPanel + DestinasiCard)
            InitializeDestinasiPanel();

            // Handle ketika form ditutup
            //this.FormClosed += (s, e) => Application.Exit();
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
                // Cari footer panel untuk menghitung tinggi yang tersedia
                var footerPanel = this.Controls.OfType<Control>()
                    .FirstOrDefault(c => c.Name.ToLower().Contains("footer") || c.Name.ToLower().Contains("kontak") || c.Name.ToLower().Contains("tentang"));

                // Cari posisi referensi dari control yang ada (navbar, filter panel, dll)
                int xPosition = 280;  // Default X position
                int yPosition = 150;  // Lebih ke atas lagi (dari 180 ke 150)

                // Hitung tinggi yang tersedia (jangan sampai memotong footer)
                int availableHeight = this.Height - yPosition - 150; // Reserve 150px untuk footer

                if (footerPanel != null)
                {
                    // Jika ada footer, pastikan tidak memotong
                    availableHeight = footerPanel.Top - yPosition - 20; // 20px margin
                }

                // Coba cari panel filter atau navbar untuk posisi relatif
                var filterPanel = this.Controls.OfType<Control>()
                    .FirstOrDefault(c => c.Name.Contains("Filter") || c.Name.Contains("Pulau"));

                if (filterPanel != null)
                {
                    // Posisi di sebelah kanan filter panel
                    xPosition = filterPanel.Right + 20;
                    yPosition = Math.Max(150, filterPanel.Top); // Minimal Y = 150
                }

                // Jika belum ada, buat baru dengan posisi yang lebih baik
                flpDestinasi = new FlowLayoutPanel
                {
                    Name = "flpDestinasi",
                    AutoScroll = true,
                    WrapContents = true,
                    FlowDirection = FlowDirection.LeftToRight,
                    Padding = new Padding(20),
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

                await Task.Run(() =>
                {
                    allDestinasi = Destinasi.GetAll();
                });

                // Build list untuk fitur sugesti (ListBox)
                _allDestinasi = allDestinasi
                    .Select(d => new DestInfo
                    {
                        Id = d.Id,
                        Nama = d.NamaDestinasi,
                        Lokasi = d.Lokasi
                    })
                    .ToList();

                MessageBox.Show($"Ditemukan {allDestinasi.Count} destinasi di database", "Info Loading",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (flpDestinasi != null)
                {
                    flpDestinasi.Controls.Clear();
                }

                if (allDestinasi.Count == 0)
                {
                    var lblNoData = new Label
                    {
                        Text = "Belum ada destinasi tersedia.\nTambahkan destinasi melalui halaman Admin.",
                        Font = new Font("Segoe UI", 12, FontStyle.Italic),
                        ForeColor = Color.Gray,
                        AutoSize = true,
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    flpDestinasi.Controls.Add(lblNoData);
                }
                else
                {
                    if (allDestinasi.Count == 0)
                    {
                        flpDestinasi.Controls.Clear();
                        flpDestinasi.Controls.Add(new Label { Text = "Belum ada destinasi...", AutoSize = true });
                    }
                    else
                    {
                        ApplyFilters();
                    }
                }
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

        private void OpenDetailDestinasi(Destinasi destinasi)
        {
            try
            {
                var detailForm = new DetailDestinasi(
                    destinasi.Id,
                    destinasi.NamaDestinasi,
                    destinasi.Lokasi
                );
                detailForm.FormClosed += (s, args) => this.Show();
                detailForm.Show();
                this.Hide();
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
                tbSearchDestinasi.TextChanged += TbSearchDestinasi_TextChanged;
                tbSearchDestinasi.KeyDown += TbSearchDestinasi_KeyDown;
                tbSearchDestinasi.PlaceholderText = "Cari destinasi ...";
            }

            if (lstSugesti != null)
            {
                lstSugesti.Visible = false;
                lstSugesti.DoubleClick += (s, e) => OpenSelectedSuggestion();
                lstSugesti.KeyDown += LstSugesti_KeyDown;
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
                Form2 profileForm = new Form2();
                profileForm.FormClosed += (s, args) => this.Show();
                profileForm.Show();
                this.Hide();
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

        private void ApplyDestinasiCardFiltering(List<string> selectedPulau, List<string> selectedAktivitas)
        {
            if (flpDestinasi == null) return;

            bool hasPulauFilter = selectedPulau.Count > 0;
            bool hasAktivitasFilter = selectedAktivitas.Count > 0;

            if (!hasPulauFilter && !hasAktivitasFilter)
            {
                ShowAllDestinasiCards();
                return;
            }

            foreach (Control control in flpDestinasi.Controls)
            {
                if (control is DestinasiCard card)
                {
                    bool showDestination = true;

                    if (hasPulauFilter)
                    {
                        showDestination = showDestination && selectedPulau.Contains(card.Pulau);
                    }

                    // Filter aktivitas belum di-implement di DestinasiCard, jadi skip dulu
                    card.Visible = showDestination;
                }
            }
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

        // ================== PANEL MANUAL (KALAU MASIH DIPAKAI) ==================
        // Fungsi-fungsi di bawah masih pakai Guna2ShadowPanel lama,
        // dibiarkan hidup kalau nanti mau dipakai lagi.

        private void ShowAllDestinations()
        {
            foreach (Control control in this.Controls)
            {
                if (control is Guna2ShadowPanel panel && control.Name.StartsWith("panelDestinasi"))
                    panel.Visible = true;
            }

            foreach (Control container in this.Controls)
            {
                if (container is Guna2Panel)
                {
                    foreach (Control control in container.Controls)
                    {
                        if (control is Guna2ShadowPanel panel && control.Name.StartsWith("panelDestinasi"))
                            panel.Visible = true;
                    }
                }
            }
        }

        private void FilterDestinationsBySearch(string searchText)
        {
            int foundCount = 0;

            foreach (Control control in this.Controls)
            {
                if (control is Guna2ShadowPanel panel && control.Name.StartsWith("panelDestinasi"))
                {
                    bool found = SearchInDestinationPanel(panel, searchText);
                    panel.Visible = found;
                    if (found) foundCount++;
                }
            }

            foreach (Control container in this.Controls)
            {
                if (container is Guna2Panel)
                {
                    foreach (Control control in container.Controls)
                    {
                        if (control is Guna2ShadowPanel panel && control.Name.StartsWith("panelDestinasi"))
                        {
                            bool found = SearchInDestinationPanel(panel, searchText);
                            panel.Visible = found;
                            if (found) foundCount++;
                        }
                    }
                }
            }

            if (foundCount == 0)
            {
                MessageBox.Show($"Tidak ditemukan destinasi dengan kata kunci: '{searchText}'",
                              "Pencarian", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool SearchInDestinationPanel(Guna2ShadowPanel panel, string searchText)
        {
            foreach (Control innerControl in panel.Controls)
            {
                if (innerControl is Label label && label.Text.ToLower().Contains(searchText))
                    return true;
                if (innerControl is Guna2HtmlLabel htmlLabel && htmlLabel.Text.ToLower().Contains(searchText))
                    return true;
                if (innerControl is Guna2Panel innerPanel)
                {
                    foreach (Control childControl in innerPanel.Controls)
                    {
                        if (childControl is Label childLabel && childLabel.Text.ToLower().Contains(searchText))
                            return true;
                        if (childControl is Guna2HtmlLabel childHtmlLabel && childHtmlLabel.Text.ToLower().Contains(searchText))
                            return true;
                    }
                }
            }
            return false;
        }

        private void ApplyDestinationFiltering(List<string> selectedPulau, List<string> selectedAktivitas)
        {
            bool hasPulauFilter = selectedPulau.Count > 0;
            bool hasAktivitasFilter = selectedAktivitas.Count > 0;

            if (!hasPulauFilter && !hasAktivitasFilter)
            {
                ShowAllDestinations();
                return;
            }

            foreach (Control control in this.Controls)
            {
                if (control is Guna2ShadowPanel panel && control.Name.StartsWith("panelDestinasi"))
                {
                    bool showDestination = true;

                    if (hasPulauFilter)
                    {
                        string destinationPulau = GetDestinationPulau(panel);
                        showDestination = showDestination && selectedPulau.Contains(destinationPulau);
                    }

                    if (hasAktivitasFilter && showDestination)
                    {
                        List<string> destinationAktivitas = GetDestinationAktivitas(panel);
                        showDestination = showDestination && selectedAktivitas.Any(a => destinationAktivitas.Contains(a));
                    }

                    panel.Visible = showDestination;
                }
            }
        }

        private string GetDestinationPulau(Guna2ShadowPanel destinationPanel)
        {
            foreach (Control control in destinationPanel.Controls)
            {
                if (control is Guna2HtmlLabel label)
                {
                    if (label.Text.Contains("Papua")) return "Papua";
                    if (label.Text.Contains("Bali")) return "Bali";
                    if (label.Text.Contains("Jawa")) return "Jawa";
                    if (label.Text.Contains("Sulawesi")) return "Sulawesi";
                }
            }
            return "Unknown";
        }

        private List<string> GetDestinationAktivitas(Guna2ShadowPanel destinationPanel)
        {
            List<string> aktivitas = new List<string>();

            foreach (Control control in destinationPanel.Controls)
            {
                if (control is Guna2HtmlLabel label)
                {
                    if (label.Text.Contains("Sunset")) aktivitas.Add("Sunset");
                    if (label.Text.Contains("Diving")) aktivitas.Add("Diving");
                    if (label.Text.Contains("Snorkeling")) aktivitas.Add("Snorkeling");
                }
            }

            return aktivitas;
        }

        private void SortDestinations(string sortBy)
        {
            // kalau mau sorting panel manual, bisa diisi
        }

        // ================== SEARCH SUGGESTION (tbSearchDestinasi + lstSugesti) ==================

        private void TbSearchDestinasi_TextChanged(object sender, EventArgs e)
        {
            if (tbSearchDestinasi == null || lstSugesti == null) return;

            string text = tbSearchDestinasi.Text.Trim();

            if (string.IsNullOrEmpty(text))
            {
                lstSugesti.Visible = false;
            }
            else
            {
                var matches = _allDestinasi
                    .Where(d =>
                        d.Nama.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        d.Lokasi.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

                lstSugesti.BeginUpdate();
                lstSugesti.Items.Clear();

                foreach (var d in matches)
                    lstSugesti.Items.Add(d);

                lstSugesti.EndUpdate();
                lstSugesti.Visible = matches.Count > 0;
            }

            // sekalian filter kartu
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
                // Cari Destinasi asli berdasarkan Id, supaya pake kartu & data lengkap
                var dest = allDestinasi.FirstOrDefault(d => d.Id == info.Id);
                if (dest != null)
                {
                    OpenDetailDestinasi(dest);
                }
                else
                {
                    // fallback kalau tidak ketemu: buka DetailDestinasi pakai info minimal
                    var detail = new DetailDestinasi(info.Id, info.Nama, info.Lokasi);
                    detail.FormClosed += (s, args) => this.Show();
                    detail.Show();
                    this.Hide();
                }

                lstSugesti.Visible = false;
            }
        }

        // ================== MISC / EVENT DEFAULT ==================

        private void ClearFilters()
        {
            if (CmbRating != null)
                CmbRating.SelectedIndex = 0;

            if (PnlPulau != null)
            {
                foreach (Control control in PnlPulau.Controls)
                {
                    if (control is Guna2CheckBox checkBox)
                        checkBox.Checked = false;
                }
            }

            if (PnlAktivitas != null)
            {
                foreach (Control control in PnlAktivitas.Controls)
                {
                    if (control is Guna2CheckBox checkBox)
                        checkBox.Checked = false;
                }
            }

            // Reset search lama
            if (txtCariDestinasi != null)
            {
                txtCariDestinasi.Text = "Cari destinasi ...";
                txtCariDestinasi.ForeColor = Color.Gray;
            }

            // Reset search baru + sugesti
            if (tbSearchDestinasi != null)
                tbSearchDestinasi.Text = "";

            if (lstSugesti != null)
            {
                lstSugesti.Visible = false;
                lstSugesti.Items.Clear();
            }

            ShowAllDestinasiCards();
        }

        private void guna2HtmlLabel3_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel7_Click(object sender, EventArgs e) { }
        private void panelDestinasi1_Paint(object sender, PaintEventArgs e) { }
        private void guna2ShadowPanel2_Paint(object sender, PaintEventArgs e) { }
        private void guna2HtmlLabel25_Click(object sender, EventArgs e) { }
        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e) { }
        private void guna2HtmlLabel26_Click(object sender, EventArgs e) { }

        private void DashboardUtama_Load(object sender, EventArgs e)
        {
            if (txtCariDestinasi != null)
                txtCariDestinasi.ForeColor = Color.Gray;

            if (lstSugesti != null)
                lstSugesti.Visible = false;
        }

        private void PnlPulau_MouseEnter(object sender, EventArgs e)
        {
            if (PnlPulau != null) PnlPulau.Focus();
        }

        private void PnlAktivitas_MouseEnter(object sender, EventArgs e)
        {
            if (PnlAktivitas != null) PnlAktivitas.Focus();
        }

        private void panelDestinasi1_MouseEnter(object sender, EventArgs e)
        {
            if (panelDestinasi1 != null) panelDestinasi1.Focus();
        }

        private void PnlPulau_MouseWheel(object sender, MouseEventArgs e)
        {
           
        }

        private void panelDestinasi1_MouseWheel(object sender, MouseEventArgs e)
        {
        }

        private void Navbar_Paint(object sender, PaintEventArgs e)
        {
        }

        private void lblProfile_Click(object sender, EventArgs e)
        {
        }

        private void guna2CheckBox1_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void CheckBoxSunset_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void CheckBoxSulawesi_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
