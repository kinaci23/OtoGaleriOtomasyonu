using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using OtoGaleri.Service.AracYonetimi;

namespace OtoGaleri.WinForms.AracYonetimi
{
    public partial class FrmAracListesi : Form
    {
        // Servis katmanını çağıracağımız değişken
        private SAracYonetimi _sAracYonetimi;

        public FrmAracListesi()
        {
            InitializeComponent();
            _sAracYonetimi = new SAracYonetimi();
        }

        // FORM YÜKLENİRKEN ÇALIŞACAK ANA METOD
        private void FrmAracListesi_Load(object sender, EventArgs e)
        {
            Listele();           // Verileri veritabanından çek
            GridAyarlariniYap(); // Tablonun görsel ayarlarını yap
        }

        // Veritabanından veriyi çekip Grid'e dolduran metod
        private void Listele()
        {
            dgvAraclar.DataSource = _sAracYonetimi.GetAracListesiDetayli();
        }

        // GridView (Tablo) Görsel Ayarları
        private void GridAyarlariniYap()
        {
            if (dgvAraclar.DataSource == null) return;

            // 1. Genel Görünüm ve Davranış
            dgvAraclar.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAraclar.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAraclar.MultiSelect = false;
            dgvAraclar.ReadOnly = true;
            dgvAraclar.AllowUserToAddRows = false;
            dgvAraclar.RowHeadersVisible = false;
            dgvAraclar.BackgroundColor = SystemColors.Control;
            dgvAraclar.BorderStyle = BorderStyle.None;

            // Satır Seçim Renkleri (Hafif Gri)
            dgvAraclar.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 200, 200);
            dgvAraclar.DefaultCellStyle.SelectionForeColor = Color.Black;

            // 2. Başlık Ayarları
            dgvAraclar.EnableHeadersVisualStyles = false; // Windows temasını kapat

            // Başlık Normal Rengi
            dgvAraclar.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
            dgvAraclar.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            // [YENİ] Başlık "Seçili" Rengi (Normal ile aynı yapıyoruz ki mavi olmasın)
            dgvAraclar.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(45, 45, 48);
            dgvAraclar.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;

            dgvAraclar.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvAraclar.ColumnHeadersHeight = 35;

            // 3. ID Sütununu Gizleme
            if (dgvAraclar.Columns["AracID"] != null)
                dgvAraclar.Columns["AracID"].Visible = false;

            // 4. Para Birimi Formatı
            if (dgvAraclar.Columns["Satış Fiyatı"] != null)
                dgvAraclar.Columns["Satış Fiyatı"].DefaultCellStyle.Format = "C2";
        }

        // YENİ ARAÇ EKLE BUTONU
        private void btnYeniArac_Click(object sender, EventArgs e)
        {
            FrmAracEkle frm = new FrmAracEkle();
            frm.ShowDialog(); // Form açılır ve kapanana kadar kod burada bekler.

            // Form kapandığında (Kayıt işlemi bitince) listeyi yenile
            Listele();
        }

        private void dgvAraclar_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Başlığa tıklanırsa işlem yapma
            if (e.RowIndex < 0) return;

            // Seçili satırdan ID'yi al (AracID sütunu gizli ama verisi orada duruyor)
            int secilenId = Convert.ToInt32(dgvAraclar.Rows[e.RowIndex].Cells["AracID"].Value);

            // Formu ID ile aç (Güncelleme Modu)
            FrmAracEkle frm = new FrmAracEkle(secilenId);
            frm.ShowDialog();

            // Form kapanınca listeyi yenile ki değişiklik görünsün
            Listele();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            // 1. Seçili satır var mı kontrol et
            if (dgvAraclar.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen silinecek aracı listeden seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Kullanıcıdan son onayı al (Kritik Adım!)
            DialogResult cevap = MessageBox.Show("Seçili aracı silmek istediğinize emin misiniz?\nBu işlem geri alınamaz!", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (cevap == DialogResult.Yes)
            {
                // 3. Seçili ID'yi al
                int secilenId = Convert.ToInt32(dgvAraclar.SelectedRows[0].Cells["AracID"].Value);

                // 4. Servise gönder ve sil
                string sonuc = _sAracYonetimi.AracSil(secilenId);

                if (sonuc == null)
                {
                    MessageBox.Show("Kayıt silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Listele(); // Listeyi yenile ki silinen araç gitsin
                }
                else
                {
                    MessageBox.Show("Hata: " + sonuc, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}