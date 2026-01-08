using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using OtoGaleri.Service.AracYonetimi;

namespace OtoGaleri.WinForms.AracYonetimi
{
    public partial class FrmAracListesi : DevExpress.XtraEditors.XtraForm
    {
        private SAracYonetimi _sAracYonetimi;

        public FrmAracListesi()
        {
            InitializeComponent();
            _sAracYonetimi = new SAracYonetimi();
        }

        // FORM YÜKLENİRKEN
        private void FrmAracListesi_Load(object sender, EventArgs e)
        {
            Tasarim.Uygula(this);
            Listele();
            GridAyarlariniYap();
        }

        // LİSTELEME METODU 
        private void Listele()
        {
            
            if (tabControl1 == null) return;

            // 0. index: Galeridekiler (Satılmadı -> False)
            // 1. index: Satılanlar   (Satıldı   -> True)
            bool satildiMi = tabControl1.SelectedIndex == 1;

            // Servisteki yeni metoda "satildiMi" bilgisini gönderiyoruz
            dgvAraclar.DataSource = _sAracYonetimi.GetAracListesiByDurum(satildiMi);
        }

        // SEKME DEĞİŞTİĞİNDE ÇALIŞACAK METOD
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Sekme değişince arama kutusunu temizle
            // (Bu işlem tetiklendiğinde zaten txtArama_TextChanged çalışacak ve listeyi normal haliyle yükleyecektir)
            txtArama.Text = "";

            // Eğer Text zaten boşsa tetiklenmez, o yüzden manuel listeleme yapalım:
            if (string.IsNullOrEmpty(txtArama.Text))
            {
                Listele();
            }
        }

        // GRID AYARLARI
        private void GridAyarlariniYap()
        {
            if (dgvAraclar.DataSource == null) return;

            // --- TEMEL GÖRÜNÜM ---
            dgvAraclar.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAraclar.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAraclar.MultiSelect = false;
            dgvAraclar.ReadOnly = true;
            dgvAraclar.AllowUserToAddRows = false;
            dgvAraclar.RowHeadersVisible = false;
            dgvAraclar.BorderStyle = BorderStyle.None;

            // --- KOYU TEMA RENKLERİ (OKUNABİLİRLİK İÇİN) ---
            dgvAraclar.BackgroundColor = System.Drawing.Color.FromArgb(38, 38, 38); 
            dgvAraclar.GridColor = System.Drawing.Color.FromArgb(50, 50, 50);      

            // Satırların Rengi (Koyu Gri Zemin - Beyaz Yazı)
            dgvAraclar.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(38, 38, 38);
            dgvAraclar.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvAraclar.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(100, 100, 100); 
            dgvAraclar.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

            // Başlık (Header) Rengi
            dgvAraclar.EnableHeadersVisualStyles = false;
            dgvAraclar.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.Black; 
            dgvAraclar.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvAraclar.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.Black;
            dgvAraclar.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold);
            dgvAraclar.ColumnHeadersHeight = 40;

            // --- ÖZEL SÜTUN AYARLARI ---
            if (dgvAraclar.Columns["AracID"] != null)
                dgvAraclar.Columns["AracID"].Visible = false;

            if (dgvAraclar.Columns["Satış Fiyatı"] != null)
                dgvAraclar.Columns["Satış Fiyatı"].DefaultCellStyle.Format = "C2";
        }

        // YENİ ARAÇ EKLE
        private void btnYeniArac_Click(object sender, EventArgs e)
        {
            FrmAracEkle frm = new FrmAracEkle();
            frm.ShowDialog();
            Listele();
        }

        // ÇİFT TIKLAYINCA DÜZENLEME
        private void dgvAraclar_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            
            if (tabControl1.SelectedIndex == 1)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Satılmış araçlar üzerinde düzenleme yapılamaz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int secilenId = Convert.ToInt32(dgvAraclar.Rows[e.RowIndex].Cells["AracID"].Value);
            FrmAracEkle frm = new FrmAracEkle(secilenId);
            frm.ShowDialog();
            Listele();
        }

        // SİLME BUTONU
        private void btnSil_Click(object sender, EventArgs e)
        {
            if (dgvAraclar.SelectedRows.Count == 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Lütfen işlem yapılacak aracı seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult cevap = DevExpress.XtraEditors.XtraMessageBox.Show("Seçili kaydı silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (cevap == DialogResult.Yes)
            {
                int secilenId = Convert.ToInt32(dgvAraclar.SelectedRows[0].Cells["AracID"].Value);
                string sonuc = _sAracYonetimi.AracSil(secilenId);

                if (sonuc == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Kayıt silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Listele();
                }
                else
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Hata: " + sonuc, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // --- HATALARI SUSTURAN BOŞ METODLAR ---
        
        private void tabPage1_Click(object sender, EventArgs e) { }

        private void btnSatisYap_Click(object sender, EventArgs e)
        {
            // 1. Seçim Kontrolü
            if (dgvAraclar.SelectedRows.Count == 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Lütfen satışı yapılacak aracı seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Satılmış mı Kontrolü
            if (tabControl1.SelectedIndex == 1)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Bu araç zaten satılmış!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3. Seçilen verileri al
            int secilenId = Convert.ToInt32(dgvAraclar.SelectedRows[0].Cells["AracID"].Value);
            string fiyat = dgvAraclar.SelectedRows[0].Cells["Satış Fiyatı"].Value.ToString();

            // 4. Yeni Satış Formunu AÇ
            FrmSatisYap frm = new FrmSatisYap(fiyat);

            // Eğer kullanıcı "ONAYLA" butonuna bastıysa (DialogResult.OK döndüyse)
            if (frm.ShowDialog() == DialogResult.OK)
            {
                string musteriAd = frm.MusteriAdSoyad;
                string musteriTel = frm.MusteriTelefon;

                // Veritabanına kaydet
                string sonuc = _sAracYonetimi.AracSat(secilenId, musteriAd, musteriTel);

                if (sonuc == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show($"Hayırlı olsun! {musteriAd} adına satış gerçekleştirildi.", "Satış Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Listele();
                }
                else
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Hata: " + sonuc, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRapor_Click(object sender, EventArgs e)
        {
            
            FrmFinansalRapor frm = new FrmFinansalRapor();
            frm.ShowDialog();
        }

        private void txtArama_TextChanged(object sender, EventArgs e)
        {
            string aranan = txtArama.Text.Trim(); 

            // Hangi sekmedeyiz? (0: Galeride, 1: Satılanlar)
            bool satildiMi = tabControl1.SelectedIndex == 1;

            if (string.IsNullOrEmpty(aranan))
            {
                // Kutu boşsa normal listeyi getir
                dgvAraclar.DataSource = _sAracYonetimi.GetAracListesiByDurum(satildiMi);
            }
            else
            {
                // Kutu doluysa ARAMA metodunu çalıştır
                dgvAraclar.DataSource = _sAracYonetimi.AracAra(aranan, satildiMi);
            }
        }
    }
}