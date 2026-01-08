using System;
using System.Windows.Forms;

namespace OtoGaleri.WinForms
{
    public partial class FrmSatisYap : DevExpress.XtraEditors.XtraForm
    {
        // Ana formun bu bilgilere ulaşabilmesi için değişkenler tanımlıyoruz
        public string MusteriAdSoyad { get; private set; }
        public string MusteriTelefon { get; private set; }

        // Constructor (Yapıcı Metot): Form açılırken fiyatı dışarıdan alır
        public FrmSatisYap(string fiyatBilgisi)
        {
            InitializeComponent();
            lblFiyat.Text = fiyatBilgisi; // Fiyatı etikete yazdır
        }

        // ONAYLA BUTONU
        private void btnOnayla_Click(object sender, EventArgs e)
        {
            // 1. Boş Alan Kontrolü
            if (string.IsNullOrWhiteSpace(txtAdSoyad.Text) || string.IsNullOrWhiteSpace(txtTelefon.Text))
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Lütfen müşteri adı ve telefon bilgisini giriniz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Bilgileri Kaydet
            MusteriAdSoyad = txtAdSoyad.Text;
            MusteriTelefon = txtTelefon.Text;

            // 3. Formun sonucunu "TAMAM" olarak işaretle ve kapat
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // İPTAL BUTONU
        private void btnIptal_Click(object sender, EventArgs e)
        {
            // Formun sonucunu "İPTAL" olarak işaretle ve kapat
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // --- HATALARI GİDERMEK İÇİN EKLENEN BOŞ METOTLAR ---
        // Tasarımda yanlışlıkla çift tıklanan label'ların kod karşılıkları:
        private void lblFiyat_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
    }
}