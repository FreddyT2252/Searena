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
        private Guna2ShadowPanel panelDestinasi1;

        private FlowLayoutPanel flpDestinasi; 
        private List<Destinasi> allDestinasi = new List<Destinasi>(); 

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
            public static readonly string ConnString =
                DotNetEnv.Env.GetString("DB_CONNECTION");
        }

        public DashboardUtama()
        {
            InitializeComponent();

            HideManualDestinationPanels();

            SetupEventHandlers();
            SetupScrollBars();
            SetupRatingComboBox();

            InitializeDestinasiPanel();

        }

        // Layout dan inisialisasi panel destinasi

        private void HideManualDestinationPanels()
        {
            foreach (Control control in this.Controls)
            {
                if (control is Guna2ShadowPanel panel && control.Name.StartsWith("panelDestinasi"))
                {
                    panel.Visible = false;
                }
            }

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
                var footerPanel = this.Controls.OfType<Control>()
                    .FirstOrDefault(c => c.Name == "PnlInformasi");

                int xPosition = 280;
                int yPosition = 100;

                int availableHeight;
                const int safeMarginToFooter = 60; 

                if (footerPanel != null)
                {
                    availableHeight = Math.Max(200, footerPanel.Top - yPosition - safeMarginToFooter);
                }
                else
                {
                    availableHeight = 380; 
                }

                var filterPanel = this.Controls.OfType<Control>()
                    .FirstOrDefault(c => c.Name == "PnlFilter");

                if (filterPanel != null)
                {
                    xPosition = filterPanel.Right + 20;
                    yPosition = Math.Max(100, filterPanel.Top);

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
                    Padding = new Padding(10, 5, 10, 10), 
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
                flpDestinasi.Visible = true;
                flpDestinasi.AutoScroll = true; 
                flpDestinasi.Padding = new Padding(10, 5, 10, 10);
                flpDestinasi.BringToFront();
            }

            LoadDestinasiFromDatabase();
        }

        private async void LoadDestinasiFromDatabase()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                allDestinasi = await Task.Run(() => Destinasi.GetAll());

                _allDestinasi = allDestinasi
                    .Select(d => new DestInfo
                    {
                        Id = d.Id,
                        Nama = d.NamaDestinasi,
                        Lokasi = d.Lokasi
                    })
                    .ToList();

                if (flpDestinasi == null) return;
                flpDestinasi.SuspendLayout();
                flpDestinasi.Controls.Clear();

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
                    ApplyFilters();
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
                string fullLocation;
                if (string.IsNullOrWhiteSpace(destinasi.Lokasi))
                    fullLocation = destinasi.Pulau;                     
                else
                    fullLocation = $"{destinasi.Lokasi}, {destinasi.Pulau}";

                var detailForm = new DetailDestinasi(
                    destinasi.Id,
                    destinasi.NamaDestinasi,
                    fullLocation
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

        // Setup event handlers untuk controls

        private void SetupEventHandlers()
        {
            // Profile
            if (PctProfile != null)
                PctProfile.Click += (s, e) => OpenProfilePage();

            // Hapus filter
            if (LblHapusFilter != null)
                LblHapusFilter.Click += (s, e) => ClearFilters();

            var btnReload = this.Controls.Find("btnReload", true).FirstOrDefault();
            if (btnReload != null)
            {
                if (btnReload is Guna.UI2.WinForms.Guna2Button gunaBtn)
                {
                    gunaBtn.Click -= BtnReload_Click;
                    gunaBtn.Click += BtnReload_Click;
                }
                else if (btnReload is Button btn)
                {
                    btn.Click -= BtnReload_Click;
                    btn.Click += BtnReload_Click;
                }
            }

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
                FindAndSetupSearchTextBox();
            }

            // TextBox search baru + suggestion (tbSearchDestinasi + lstSugesti)
            if (tbSearchDestinasi != null)
            {
                tbSearchDestinasi.TextChanged -= TbSearchDestinasi_TextChanged; 
                tbSearchDestinasi.TextChanged += TbSearchDestinasi_TextChanged;
                tbSearchDestinasi.KeyDown -= TbSearchDestinasi_KeyDown;
                tbSearchDestinasi.KeyDown += TbSearchDestinasi_KeyDown;
                tbSearchDestinasi.PlaceholderText = "Cari destinasi ...";
            }

            if (lstSugesti != null)
            {
                lstSugesti.Visible = false;
                lstSugesti.DoubleClick -= OpenSelectedSuggestion_Handler; 
                lstSugesti.DoubleClick += OpenSelectedSuggestion_Handler;
                lstSugesti.KeyDown -= LstSugesti_KeyDown;
                lstSugesti.KeyDown += LstSugesti_KeyDown;
                lstSugesti.Font = new Font("Segoe UI", 9f);
                
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

        // Navigasi antar section

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
                Form2 profileForm = new Form2(this); 
                
                profileForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error membuka halaman profile: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Search dan filter logic

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

            // Ambil filter dari checkbox
            var selectedPulau = GetSelectedPulau();       
            var selectedAktivitas = GetSelectedAktivitas(); 

            // Ambil pilihan urutan
            string sortBy = CmbRating?.SelectedItem?.ToString() ?? "Populer";
            
            IEnumerable<Destinasi> query = allDestinasi;

            // Filter pulau
            if (selectedPulau.Any())
            {
                query = query.Where(d => selectedPulau.Contains(d.Pulau));
            }

            // Filter aktivitas (snorkeling, sunset, dll)
            if (selectedAktivitas.Any())
            {
                query = query.Where(d =>
                    !string.IsNullOrWhiteSpace(d.Activity) &&          
                    selectedAktivitas.Any(a =>
                        d.Activity
                            .Split(',')                               
                            .Select(x => x.Trim())
                            .Contains(a)
                    )
                );
            }

            // Urutkan berdasarkan pilihan
            switch (sortBy)
            {
                case "Rating Tertinggi":
                    query = query.OrderByDescending(d => d.RatingAvg);
                    break;
                case "Rating Terendah":
                    query = query.OrderBy(d => d.RatingAvg);
                    break;
                case "Rekomendasi":
                    // yang rating tinggi dan review banyak
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

        // Logic untuk mendapatkan filter dari checkbox
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

        // Cari dengan suggestion support
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
                    .Take(5) 
                    .ToList();

                lstSugesti.BeginUpdate();
                lstSugesti.Items.Clear();

                foreach (var d in matches)
                    lstSugesti.Items.Add(d);

                lstSugesti.EndUpdate();
                lstSugesti.Visible = matches.Count > 0;

                if (lstSugesti.Visible && tbSearchDestinasi != null)
                {
                    Point searchBoxLocation = tbSearchDestinasi.PointToScreen(Point.Empty);
                    Point formLocation = this.PointToScreen(Point.Empty);

                    int relativeX = searchBoxLocation.X - formLocation.X;
                    int relativeY = searchBoxLocation.Y - formLocation.Y + tbSearchDestinasi.Height + 2;
                    
                    lstSugesti.Location = new Point(relativeX, relativeY);
                    lstSugesti.Width = tbSearchDestinasi.Width;
                    lstSugesti.Height = 120; 
                    lstSugesti.BringToFront();
                }
            }

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
                var dest = allDestinasi.FirstOrDefault(d => d.Id == info.Id);
                if (dest != null)
                {
                    OpenDetailDestinasi(dest);
                }

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

        // Clear filters
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

        //Reload destinasi dan status cuaca
        private async void BtnReload_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (sender is Guna.UI2.WinForms.Guna2Button gunaBtn)
                {
                    gunaBtn.Enabled = false;
                    gunaBtn.Text = "Memuat...";
                }
                else if (sender is Button btn)
                {
                    btn.Enabled = false;
                    btn.Text = "Memuat...";
                }

                await ReloadDestinasiAsync();

                MessageBox.Show("Destinasi dan cuaca berhasil diperbarui!", "Reload Berhasil",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saat reload:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                
                if (sender is Guna.UI2.WinForms.Guna2Button gunaBtn)
                {
                    gunaBtn.Enabled = true;
                    gunaBtn.Text = "Reload";
                }
                else if (sender is Button btn)
                {
                    btn.Enabled = true;
                    btn.Text = "Reload";
                }
            }
        }

        private async Task ReloadDestinasiAsync()
        {
            // Ambil data destinasi terbaru dari database
            allDestinasi = await Task.Run(() => Destinasi.GetAll());

            // Update list untuk sugesti pencarian
            _allDestinasi = allDestinasi
                .Select(d => new DestInfo
                {
                    Id = d.Id,
                    Nama = d.NamaDestinasi,
                    Lokasi = d.Lokasi
                })
                .ToList();

            if (flpDestinasi == null) return;
            flpDestinasi.SuspendLayout();
            flpDestinasi.Controls.Clear();
            
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
                // Terapkan filter/urutan saat ini
                ApplyFilters();
                await ReloadAllWeatherAsync();
            }

            flpDestinasi.ResumeLayout();
        }

        // Method untuk reload cuaca 
        private async Task ReloadAllWeatherAsync()
        {
            if (flpDestinasi == null) return;
            var cards = flpDestinasi.Controls.OfType<DestinasiCard>().ToList();
            var tasks = cards.Select(card => card.ReloadWeatherAsync());
            await Task.WhenAll(tasks);
        }
        // event handlers kosong untuk mencegah error designer
        private void guna2ShadowPanel2_Paint(object sender, PaintEventArgs e) { }
        private void guna2HtmlLabel25_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel26_Click(object sender, EventArgs e) { }
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

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void lstSugesti_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void guna2CheckBox2_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
