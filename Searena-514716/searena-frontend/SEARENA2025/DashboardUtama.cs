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
            SetupEventHandlers();
            SetupScrollBars();
            SetupRatingComboBox();
            SetupDestinationPanels();
            LoadDestinasiFromDb();

            // Handle ketika form ditutup
            //this.FormClosed += (s, e) => Application.Exit();
        }

        private void SetupEventHandlers()
        {
            // Event handler untuk profile picture
            if (PctProfile != null)
                PctProfile.Click += (s, e) => OpenProfilePage();

            // Event handler untuk tombol hapus filter
            if (LblHapusFilter != null)
                LblHapusFilter.Click += (s, e) => ClearFilters();

            // TextBox search utama (Guna2TextBox)
            if (tbSearchDestinasi != null)
            {
                tbSearchDestinasi.TextChanged += TbSearchDestinasi_TextChanged; // update sugesti + filter panel
                tbSearchDestinasi.KeyDown += TbSearchDestinasi_KeyDown;         // navigasi ke list suggestion / enter
                tbSearchDestinasi.PlaceholderText = "Cari destinasi ...";
            }

            // ListBox sugesti
            if (lstSugesti != null)
            {
                lstSugesti.Visible = false;
                lstSugesti.DoubleClick += (s, e) => OpenSelectedSuggestion();
                lstSugesti.KeyDown += LstSugesti_KeyDown;
            }

            // Combo rating
            if (CmbRating != null)
                CmbRating.SelectedIndexChanged += (s, e) => ApplySorting();

            // Checkbox filter pulau
            if (PnlPulau != null)
            {
                foreach (Control control in PnlPulau.Controls)
                    if (control is Guna2CheckBox cb)
                        cb.CheckedChanged += (s, e) => ApplyFilters();
            }

            // Checkbox filter aktivitas
            if (PnlAktivitas != null)
            {
                foreach (Control control in PnlAktivitas.Controls)
                    if (control is Guna2CheckBox cb)
                        cb.CheckedChanged += (s, e) => ApplyFilters();
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
                        // Setup event handlers untuk textbox yang ditemukan
                        textBox.TextChanged += (s, e) => SearchDestinations();
                        textBox.Enter += (s, e) => {
                            if (textBox.Text == "Cari destinasi ...")
                            {
                                textBox.Text = "";
                                textBox.ForeColor = Color.Black;
                            }
                        };
                        textBox.Leave += (s, e) => {
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
            // Setup scrollbar untuk filter pulau (ScrollPulau)
            if (ScrollPulau != null && PnlPulau != null)
            {
                ScrollPulau.Minimum = 0;
                ScrollPulau.Maximum = Math.Max(0, PnlPulau.Height - 150);
                ScrollPulau.LargeChange = 50;
                ScrollPulau.SmallChange = 10;
                ScrollPulau.Visible = PnlPulau.Height > 150;

                ScrollPulau.Scroll += (s, e) =>
                {
                    PnlPulau.Top = -ScrollPulau.Value;
                };
            }
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

        // ===== NAVIGASI METHODS =====
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

        // ===== SEARCH & FILTER METHODS =====
        private void SearchDestinations()
        {
            string searchText = tbSearchDestinasi?.Text?.ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(searchText))
            {
                ShowAllDestinations();
                return;
            }

            FilterDestinationsBySearch(searchText);
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
            List<string> selectedPulau = GetSelectedPulau();
            List<string> selectedAktivitas = GetSelectedAktivitas();
            ApplyDestinationFiltering(selectedPulau, selectedAktivitas);
        }

        private void ApplySorting()
        {
            if (CmbRating == null) return;
            string sortBy = CmbRating.SelectedItem?.ToString() ?? "Populer";
            SortDestinations(sortBy);
        }

        // ===== FILTER LOGIC METHODS =====
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
        private void LoadDestinasiFromDb()
        {
            _allDestinasi.Clear();

            try
            {
                using (var conn = new NpgsqlConnection(AppDb.ConnString))
                {
                    conn.Open();

                    // GANTI nama tabel & kolom sesuai database kamu
                    string sql = @"SELECT destinasi_id, nama_destinasi, lokasi 
                           FROM destinasi";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            _allDestinasi.Add(new DestInfo
                            {
                                Id = rd.GetInt32(0),
                                Nama = rd.GetString(1),
                                Lokasi = rd.GetString(2)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data destinasi: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void ShowAllDestinations()
        {
            // Tampilkan semua panel destinasi
            foreach (Control control in this.Controls)
            {
                if (control is Guna2ShadowPanel panel && control.Name.StartsWith("panelDestinasi"))
                    panel.Visible = true;
            }

            // Cari di dalam container panel
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

            // Cari di semua kontrol form
            foreach (Control control in this.Controls)
            {
                if (control is Guna2ShadowPanel panel && control.Name.StartsWith("panelDestinasi"))
                {
                    bool found = SearchInDestinationPanel(panel, searchText);
                    panel.Visible = found;
                    if (found) foundCount++;
                }
            }

            // Cari di dalam container panel
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
            // Sorting logic
        }

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


        }

        private void ClearFilters()
        {
            if (CmbRating != null)
                CmbRating.SelectedIndex = 0;

            if (PnlPulau != null)
            {
                foreach (Control control in PnlPulau.Controls)
                    if (control is Guna2CheckBox checkBox)
                        checkBox.Checked = false;
            }

            if (PnlAktivitas != null)
            {
                foreach (Control control in PnlAktivitas.Controls)
                    if (control is Guna2CheckBox checkBox)
                        checkBox.Checked = false;
            }

            // Reset search
            if (tbSearchDestinasi != null)
                tbSearchDestinasi.Text = "";

            if (lstSugesti != null)
            {
                lstSugesti.Visible = false;
                lstSugesti.Items.Clear();
            }

            ShowAllDestinations();
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
            if (ScrollPulau != null && ScrollPulau.Visible)
            {
                int newValue = ScrollPulau.Value - (e.Delta / 2);
                newValue = Math.Max(ScrollPulau.Minimum, Math.Min(ScrollPulau.Maximum, newValue));
                ScrollPulau.Value = newValue;
                if (PnlPulau != null) PnlPulau.Top = -newValue;
            }
        }

        private void panelDestinasi1_MouseWheel(object sender, MouseEventArgs e)
        {

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

        private void OpenSelectedSuggestionFromFirst()
        {
            if (lstSugesti.Items.Count > 0)
            {
                lstSugesti.SelectedIndex = 0;
                OpenSelectedSuggestion();
            }
        }

        private void OpenSelectedSuggestion()
        {
            if (lstSugesti.SelectedItem is DestInfo info)
            {
                var detail = new DetailDestinasi(info.Id, info.Nama, info.Lokasi);
                detail.FormClosed += (s, args) => this.Show();
                detail.Show();
                this.Hide();

                lstSugesti.Visible = false;
            }
        }


        private void SetupDestinationPanels()
        {

            var allContainers = new List<Control>();
            allContainers.Add(this);
            allContainers.AddRange(this.Controls.OfType<Control>().Where(c => c is Guna2Panel || c is Panel));

            foreach (var container in allContainers)
            {
                foreach (Control c in container.Controls)
                {
                    if (c is Guna2ShadowPanel sp && c.Name.StartsWith("panelDestinasi"))
                    {
                        // TODO: mapping sesuai datamu (bisa dari label di dalam panel)
                        // Misal ambil nama & lokasi dari label di dalam panel:
                        string nama = FindTextIn(sp, "lblNamaDestinasi") ?? "Nama Destinasi";
                        string lokasi = FindTextIn(sp, "lblLokasiDestinasi") ?? "Lokasi";

                        // Id bisa kamu generate sementara atau ambil dari Tag panel (kalau sudah di-set di Designer)
                        int id = sp.Tag is int ? (int)sp.Tag : ExtractIdFromName(sp.Name); // contoh: panelDestinasi3 -> 3

                        sp.Tag = new DestInfo { Id = id, Nama = nama, Lokasi = lokasi };
                        sp.Cursor = Cursors.Hand;

                        // Klik di panel
                        sp.Click -= DestinationPanel_Click;
                        sp.Click += DestinationPanel_Click;

                        // Biar klik di child ikut membuka detail, wire-kan semua child
                        WireChildClicksTo(sp, DestinationPanel_Click);
                    }
                }
            }
        }


        private string FindTextIn(Control root, string controlName)
        {
            foreach (Control c in root.Controls)
            {
                if (c.Name == controlName)
                {
                    if (c is Label l) return l.Text;
                    if (c is Guna2HtmlLabel hl) return hl.Text;
                }
                var nested = FindTextIn(c, controlName);
                if (!string.IsNullOrEmpty(nested)) return nested;
            }
            return null;
        }

        private void WireChildClicksTo(Control root, EventHandler onClick)
        {
            foreach (Control c in root.Controls)
            {
                c.Click -= onClick;
                c.Click += onClick;
                if (c.HasChildren) WireChildClicksTo(c, onClick);
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



        private int ExtractIdFromName(string name)
        {
            var digits = new string(name.Where(char.IsDigit).ToArray());
            return int.TryParse(digits, out var id) ? id : 0;
        }

        private void DestinationPanel_Click(object sender, EventArgs e)
        {

            Control src = sender as Control;
            while (src != null && !(src is Guna2ShadowPanel && src.Name.StartsWith("panelDestinasi")))
                src = src.Parent;

            if (src is Guna2ShadowPanel panel && panel.Tag is DestInfo info)
            {

                var detail = new DetailDestinasi(info.Id, info.Nama, info.Lokasi);
                detail.FormClosed += (s, args) => this.Show();
                detail.Show();
                this.Hide();
            }
        }

        private void Navbar_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblProfile_Click(object sender, EventArgs e)
        {

        }
    }
}