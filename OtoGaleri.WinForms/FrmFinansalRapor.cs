using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using OtoGaleri.Service.AracYonetimi;

namespace OtoGaleri.WinForms
{
    public partial class FrmFinansalRapor : DevExpress.XtraEditors.XtraForm
    {
        private SAracYonetimi _sAracYonetimi;

        public FrmFinansalRapor()
        {
            InitializeComponent();
            _sAracYonetimi = new SAracYonetimi();
        }

        private void FrmFinansalRapor_Load(object sender, EventArgs e)
        {
            Tasarim.Uygula(this);
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
            dgvRapor.BorderStyle = BorderStyle.None;

            // --- KOYU TEMA DÜZELTMESİ ---
            dgvRapor.BackgroundColor = System.Drawing.Color.FromArgb(38, 38, 38);
            dgvRapor.GridColor = System.Drawing.Color.FromArgb(50, 50, 50);

            dgvRapor.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(38, 38, 38);
            dgvRapor.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvRapor.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(100, 100, 100);
            dgvRapor.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

            // Başlık Rengi
            dgvRapor.EnableHeadersVisualStyles = false;
            dgvRapor.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.Black;
            dgvRapor.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvRapor.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.Black;
            dgvRapor.ColumnHeadersHeight = 35;

            // --- SÜTUN GİZLEME VE FORMATLAMA ---
            if (dgvRapor.Columns["AyNo"] != null) dgvRapor.Columns["AyNo"].Visible = false;
            if (dgvRapor.Columns["Ciro"] != null) dgvRapor.Columns["Ciro"].DefaultCellStyle.Format = "C2";

            if (dgvRapor.Columns["ToplamKar"] != null)
            {
                dgvRapor.Columns["ToplamKar"].DefaultCellStyle.Format = "C2";
                // Kar sütunu dikkat çeksin diye Yeşil yapıyoruz ama Koyu zeminde okunması için "LimeGreen" seçtim
                dgvRapor.Columns["ToplamKar"].DefaultCellStyle.ForeColor = System.Drawing.Color.LimeGreen;
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
                DevExpress.XtraEditors.XtraMessageBox.Show("Bu satışın tarih bilgisi eksik olduğu için detayları görüntülenemiyor.", "Eksik Veri", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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