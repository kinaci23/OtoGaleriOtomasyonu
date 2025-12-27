using OtoGaleri.WinForms.AracYonetimi;
using System;
using System.Windows.Forms;

namespace OtoGaleri.WinForms
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            string kadi = txtKullaniciAdi.Text;
            string sifre = txtSifre.Text;

            if (kadi == "admin" && sifre == "1234")
            {
                FrmAracListesi anaForm = new FrmAracListesi();
                this.Hide();
                anaForm.ShowDialog();
                Application.Exit();
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre hatalı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Tasarım hataları için boş metotlar
        private void label2_Click(object sender, EventArgs e) { }
        private void txtKullaniciAdi_TextChanged(object sender, EventArgs e) { }
    }
}