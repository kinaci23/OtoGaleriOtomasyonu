using System;
using System.Data;
using System.Windows.Forms;
using OtoGaleri.Service.AracYonetimi;

namespace OtoGaleri.WinForms.AracYonetimi
{
    public partial class FrmAracEkle : DevExpress.XtraEditors.XtraForm
    {
        private SAracYonetimi _sAracYonetimi;

        // Bu değişken formun modunu belirleyecek (0: Yeni Kayıt, >0: Güncelleme)
        private int _gelenAracId = 0;

        // Constructor: Artık dışarıdan ID alabiliyor (Varsayılan 0)
        public FrmAracEkle(int aracId = 0)
        {
            InitializeComponent();
            _sAracYonetimi = new SAracYonetimi();
            _gelenAracId = aracId;
        }

        private void FrmAracEkle_Load(object sender, EventArgs e)
        {
            Tasarim.Uygula(this);
            // Önce listeleri hazırlayalım
            MarkalariYukle();

            // Eğer düzenleme modundaysak (ID geldiyse), verileri kutulara doldur
            if (_gelenAracId > 0)
            {
                VerileriDoldur();
                btnKaydet.Text = "GÜNCELLE"; // Butonun adını değiştir
                this.Text = "Araç Düzenle";  // Form başlığını değiştir
            }
        }

        private void MarkalariYukle()
        {
            // Temizlik
            cmbModel.DataSource = null;
            cmbPaket.DataSource = null;

            DataTable dt = _sAracYonetimi.GetMarkaListesi();
            cmbMarka.DisplayMember = "MarkaAdi";
            cmbMarka.ValueMember = "MarkaID";
            cmbMarka.DataSource = dt;
            cmbMarka.SelectedIndex = -1; // Seçimi temizle
        }

        // DÜZENLEME MODUNDA KUTULARI DOLDURAN METOT
        private void VerileriDoldur()
        {
            DataRow row = _sAracYonetimi.GetAracBilgi(_gelenAracId);

            if (row != null)
            {
                // 1. Önce ComboBox Zincirini Tetikleyerek Dolduruyoruz
                // Markayı seçtiğimiz an otomatik olarak Model listesi yüklenir
                cmbMarka.SelectedValue = row["MarkaID"];

                // Modeli seçtiğimiz an Paket listesi yüklenir
                cmbModel.SelectedValue = row["ModelID"];

                // Son olarak Paketi seçiyoruz
                cmbPaket.SelectedValue = row["PaketID"];

                // 2. Diğer Kutucuklar
                txtYil.Text = row["Yil"].ToString();
                txtKm.Text = row["Km"].ToString();
                txtRenk.Text = row["Renk"].ToString();
                txtAlisFiyat.Text = row["AlisFiyat"].ToString();
                txtSatisFiyat.Text = row["SatisFiyat"].ToString();
                txtAciklama.Text = row["Aciklama"].ToString();
            }
        }

        private void cmbMarka_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMarka.SelectedValue != null && int.TryParse(cmbMarka.SelectedValue.ToString(), out int markaId))
            {
                DataTable dt = _sAracYonetimi.GetModelListesiByMarkaId(markaId);
                cmbModel.DisplayMember = "ModelAdi";
                cmbModel.ValueMember = "ModelID";
                cmbModel.DataSource = dt;

                if (cmbModel.Items.Count > 0) cmbModel.SelectedIndex = -1;

                cmbPaket.DataSource = null;
            }
            else
            {
                cmbModel.DataSource = null;
                cmbPaket.DataSource = null;
            }
        }

        private void cmbModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbModel.SelectedValue != null && int.TryParse(cmbModel.SelectedValue.ToString(), out int modelId))
            {
                DataTable dt = _sAracYonetimi.GetPaketListesiByModelId(modelId);
                cmbPaket.DisplayMember = "PaketAdi";
                cmbPaket.ValueMember = "PaketID";
                cmbPaket.DataSource = dt;

                if (cmbPaket.Items.Count > 0) cmbPaket.SelectedIndex = -1;
            }
            else
            {
                cmbPaket.DataSource = null;
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (cmbPaket.SelectedValue == null)
            {
                MessageBox.Show("Lütfen araç model ve paketini eksiksiz seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int paketId = Convert.ToInt32(cmbPaket.SelectedValue);
            string yil = txtYil.Text;
            string renk = txtRenk.Text;
            string aciklama = txtAciklama.Text;

            int.TryParse(txtKm.Text, out int km);
            decimal.TryParse(txtAlisFiyat.Text, out decimal alisFiyat);
            decimal.TryParse(txtSatisFiyat.Text, out decimal satisFiyat);

            string sonuc;

            // KARAR ANI: YENİ Mİ YOKSA GÜNCELLEME Mİ?
            if (_gelenAracId > 0)
            {
                sonuc = _sAracYonetimi.AracGuncelle(_gelenAracId, paketId, yil, km, renk, alisFiyat, satisFiyat, aciklama);
            }
            else
            {
                sonuc = _sAracYonetimi.AracEkle(paketId, yil, km, renk, alisFiyat, satisFiyat, aciklama);
            }

            if (sonuc == null)
            {
                MessageBox.Show("İşlem başarıyla tamamlandı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Hata oluştu: " + sonuc, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- HATALARI GİDERMEK İÇİN EKLENEN BOŞ METOTLAR ---
        // Tasarımda yanlışlıkla çift tıklanan label ve text'lerin kod karşılıkları:

        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void label7_Click(object sender, EventArgs e) { }
        private void label8_Click(object sender, EventArgs e) { }

        private void cmbPaket_SelectedIndexChanged(object sender, EventArgs e) { }
        private void txtYil_TextChanged(object sender, EventArgs e) { }
    }
}