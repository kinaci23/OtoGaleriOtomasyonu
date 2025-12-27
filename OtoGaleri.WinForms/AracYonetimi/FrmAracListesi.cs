using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using OtoGaleri.Service.AracYonetimi;

namespace OtoGaleri.WinForms.AracYonetimi
{
    public partial class FrmAracListesi : Form
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
            Listele();
            GridAyarlariniYap();
        }

        // LİSTELEME METODU (ARTIK SEKME DURUMUNA GÖRE ÇALIŞIYOR)
        private void Listele()
        {
            // Eğer TabControl eklenmediyse veya hata veriyorsa, bu satır patlamasın diye kontrol:
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

            dgvAraclar.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAraclar.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAraclar.MultiSelect = false;
            dgvAraclar.ReadOnly = true;
            dgvAraclar.AllowUserToAddRows = false;
            dgvAraclar.RowHeadersVisible = false;
            dgvAraclar.BackgroundColor = SystemColors.Control;
            dgvAraclar.BorderStyle = BorderStyle.None;

            // Renk Ayarları
            dgvAraclar.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 200, 200);
            dgvAraclar.DefaultCellStyle.SelectionForeColor = Color.Black;

            dgvAraclar.EnableHeadersVisualStyles = false;
            dgvAraclar.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
            dgvAraclar.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvAraclar.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(45, 45, 48); // Mavi olmasın
            dgvAraclar.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvAraclar.ColumnHeadersHeight = 35;

            // Gizlenecek Sütun
            if (dgvAraclar.Columns["AracID"] != null)
                dgvAraclar.Columns["AracID"].Visible = false;

            // Para Formatı
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

            // Eğer "Satılanlar" sekmesindeysek düzenlemeye izin vermeyelim (Mantıken satılmış araç değişmez)
            if (tabControl1.SelectedIndex == 1)
            {
                MessageBox.Show("Satılmış araçlar üzerinde düzenleme yapılamaz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Lütfen işlem yapılacak aracı seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult cevap = MessageBox.Show("Seçili kaydı silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (cevap == DialogResult.Yes)
            {
                int secilenId = Convert.ToInt32(dgvAraclar.SelectedRows[0].Cells["AracID"].Value);
                string sonuc = _sAracYonetimi.AracSil(secilenId);

                if (sonuc == null)
                {
                    MessageBox.Show("Kayıt silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Listele();
                }
                else
                {
                    MessageBox.Show("Hata: " + sonuc, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // --- HATALARI SUSTURAN BOŞ METODLAR ---
        // Daha önce yanlışlıkla çift tıklanan eventlerin hata vermemesi için:
        private void tabPage1_Click(object sender, EventArgs e) { }

        private void btnSatisYap_Click(object sender, EventArgs e)
        {
            // 1. Seçim Kontrolü
            if (dgvAraclar.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen satışı yapılacak aracı seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Satılmış mı Kontrolü
            if (tabControl1.SelectedIndex == 1)
            {
                MessageBox.Show("Bu araç zaten satılmış!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    MessageBox.Show($"Hayırlı olsun! {musteriAd} adına satış gerçekleştirildi.", "Satış Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Listele();
                }
                else
                {
                    MessageBox.Show("Hata: " + sonuc, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRapor_Click(object sender, EventArgs e)
        {
            // 1. Servisten verileri çek
            DataRow rapor = _sAracYonetimi.GetSatisRaporu();

            if (rapor != null)
            {
                // 2. Verileri değişkenlere al
                int satilanAdet = Convert.ToInt32(rapor["ToplamAdet"]);
                decimal toplamAlis = Convert.ToDecimal(rapor["ToplamAlis"]);
                decimal toplamSatis = Convert.ToDecimal(rapor["ToplamSatis"]);

                // 3. Kar hesapla (Satış - Alış)
                decimal toplamKar = toplamSatis - toplamAlis;

                // 4. Sonucu Göster (String Format ile para birimi ekliyoruz: C2)
                string mesaj = $"--- FİNANSAL DURUM RAPORU ---\n\n" +
                               $"Toplam Satılan Araç: {satilanAdet} Adet\n" +
                               $"-----------------------------------\n" +
                               $"Toplam Maliyet (Gider): {toplamAlis:C2}\n" +
                               $"Toplam Ciro (Gelir):    {toplamSatis:C2}\n" +
                               $"-----------------------------------\n" +
                               $"NET KAR: {toplamKar:C2}";

                MessageBox.Show(mesaj, "Finansal Analiz", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Rapor alınırken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtArama_TextChanged(object sender, EventArgs e)
        {
            string aranan = txtArama.Text.Trim(); // Başındaki sonundaki boşlukları al

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