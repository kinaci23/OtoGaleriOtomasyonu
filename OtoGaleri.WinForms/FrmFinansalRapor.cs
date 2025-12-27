using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using OtoGaleri.Service.AracYonetimi;

namespace OtoGaleri.WinForms
{
    public partial class FrmFinansalRapor : Form
    {
        private SAracYonetimi _sAracYonetimi;

        public FrmFinansalRapor()
        {
            InitializeComponent();
            _sAracYonetimi = new SAracYonetimi();
        }

        private void FrmFinansalRapor_Load(object sender, EventArgs e)
        {
            RaporlariYukle();
            GridAyarlariniYap();
        }

        private void RaporlariYukle()
        {
            DataRow genelRapor = _sAracYonetimi.GetSatisRaporu();
            if (genelRapor != null)
            {
                // Eğer tasarımda isimleri farklıysa burası hata verebilir, kontrol et.
                if (label3 != null) label3.Text = "Toplam Satış: " + genelRapor["ToplamAdet"].ToString() + " Adet";

                decimal ciro = Convert.ToDecimal(genelRapor["ToplamSatis"]);
                decimal maliyet = Convert.ToDecimal(genelRapor["ToplamAlis"]);
                decimal netKar = ciro - maliyet;

                if (label2 != null) label2.Text = "Toplam Ciro: " + ciro.ToString("C2");
                if (lblGenelKar != null) lblGenelKar.Text = "NET KAR: " + netKar.ToString("C2");
            }

            DataTable dtAylik = _sAracYonetimi.GetAylikAnaliz();
            dgvRapor.DataSource = dtAylik;
        }

        private void GridAyarlariniYap()
        {
            if (dgvRapor.DataSource == null) return;

            // --- TEMEL AYARLAR ---
            dgvRapor.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRapor.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRapor.MultiSelect = false;
            dgvRapor.ReadOnly = true;
            dgvRapor.RowHeadersVisible = false;
            dgvRapor.BackgroundColor = SystemColors.Control;
            dgvRapor.BorderStyle = BorderStyle.None;

            // --- RENK AYARLARI (MAVİLERİ YOK ETME) ---
            // 1. Satır seçilince GRİ olsun (Mavi değil)
            dgvRapor.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 200, 200);
            dgvRapor.DefaultCellStyle.SelectionForeColor = Color.Black;

            // 2. Başlık (Header) Rengi
            dgvRapor.EnableHeadersVisualStyles = false; // Bunu demezsek renkler değişmez!
            dgvRapor.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48); // Koyu Gri
            dgvRapor.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRapor.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(45, 45, 48); // Başlığa tıklayınca renk değişmesin
            dgvRapor.ColumnHeadersHeight = 35;

            // --- SÜTUN GİZLEME VE FORMATLAMA ---
            if (dgvRapor.Columns["AyNo"] != null)
                dgvRapor.Columns["AyNo"].Visible = false;

            if (dgvRapor.Columns["Ciro"] != null)
                dgvRapor.Columns["Ciro"].DefaultCellStyle.Format = "C2";

            if (dgvRapor.Columns["ToplamKar"] != null)
            {
                dgvRapor.Columns["ToplamKar"].DefaultCellStyle.Format = "C2";
                dgvRapor.Columns["ToplamKar"].DefaultCellStyle.ForeColor = Color.SeaGreen;
                dgvRapor.Columns["ToplamKar"].DefaultCellStyle.SelectionForeColor = Color.DarkGreen; // Seçilince de yeşil kalsın
                dgvRapor.Columns["ToplamKar"].DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            }
        }

        // --- ÇİFT TIKLAMA İLE DETAY AÇMA ---
        private void dgvRapor_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // 1. Başlığa tıklandıysa işlem yapma
            if (e.RowIndex < 0) return;

            // 2. HATA ÇÖZÜMÜ: Değerleri güvenli değişkenlere al
            var yilDegeri = dgvRapor.Rows[e.RowIndex].Cells["Yil"].Value;
            var ayNoDegeri = dgvRapor.Rows[e.RowIndex].Cells["AyNo"].Value;

            // 3. Kontrol Et: Eğer veritabanından boş (DBNull) geldiyse işlem yapma
            if (yilDegeri == DBNull.Value || yilDegeri == null || ayNoDegeri == DBNull.Value || ayNoDegeri == null)
            {
                MessageBox.Show("Bu satışın tarih bilgisi eksik olduğu için detayları görüntülenemiyor.", "Eksik Veri", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 4. Artık güvenle sayıya çevirebiliriz
            int yil = Convert.ToInt32(yilDegeri);
            int ayNo = Convert.ToInt32(ayNoDegeri);

            // Ay hücresi boş olsa bile ToString() hata vermez, boş metin döner.
            string ayAdi = dgvRapor.Rows[e.RowIndex].Cells["Ay"].Value.ToString();

            // 5. Detay formunu aç
            FrmAylikDetay detayFormu = new FrmAylikDetay(yil, ayNo, ayAdi);
            detayFormu.StartPosition = FormStartPosition.CenterParent;
            detayFormu.ShowDialog();
        }

        // --- TASARIMCI HATASINI KURTARAN BOŞ METODLAR ---
        // (Ekran görüntüsündeki hatayı bu çözecek)
        private void lblGenelKar_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void lblToplamSatis_Click(object sender, EventArgs e) { }
    }
}