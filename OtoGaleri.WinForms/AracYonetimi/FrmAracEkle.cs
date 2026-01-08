using OtoGaleri.Service.AracYonetimi;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace OtoGaleri.WinForms.AracYonetimi
{
    public partial class FrmAracEkle : DevExpress.XtraEditors.XtraForm
    {
        private SAracYonetimi _sAracYonetimi;

        
        private int _gelenAracId = 0;

        
        public FrmAracEkle(int aracId = 0)
        {
            InitializeComponent();
            _sAracYonetimi = new SAracYonetimi();
            _gelenAracId = aracId;
        }

        private void FrmAracEkle_Load(object sender, EventArgs e)
        {
            Tasarim.Uygula(this);
            
            YapayZekaButonuEkle();
            MarkalariYukle();

            
            if (_gelenAracId > 0)
            {
                VerileriDoldur();
                btnKaydet.Text = "GÜNCELLE"; // Butonun adını değiştir
                this.Text = "Araç Düzenle";  // Form başlığını değiştir
            }
        }

        private void YapayZekaButonuEkle()
        {
            // Buton Özellikleri
            Button btnAI = new Button();
            btnAI.Text = "✨ Yapay Zeka ile Oluştur"; 
            btnAI.Size = new Size(180, 30); 
            btnAI.BackColor = Color.FromArgb(0, 120, 215);
            btnAI.ForeColor = Color.White;
            btnAI.FlatStyle = FlatStyle.Flat;
            btnAI.FlatAppearance.BorderSize = 0;
            btnAI.Cursor = Cursors.Hand;
            btnAI.Font = new Font("Segoe UI", 9, FontStyle.Bold);

            
            Control[] bulunanlar = Controls.Find("txtAciklama", true);

            if (bulunanlar.Length > 0)
            {
                Control kutu = bulunanlar[0];

                int xKonumu = kutu.Location.X + kutu.Width - btnAI.Size.Width;

                
                int yKonumu = kutu.Location.Y - btnAI.Size.Height - 5;

                btnAI.Location = new Point(xKonumu, yKonumu);

               
                btnAI.Click += BtnAI_Click;

                this.Controls.Add(btnAI);
                btnAI.BringToFront();
            }
        }

        // --- BUTONA BASINCA ÇALIŞACAK KOD ---
        private async void BtnAI_Click(object sender, EventArgs e)
        {
            

            // Basit kontrol: Boş mu?
            if (string.IsNullOrEmpty(cmbMarka.Text) || string.IsNullOrEmpty(cmbModel.Text))
            {
                MessageBox.Show("Lütfen önce Marka ve Model bilgilerini seçin.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            Button btn = (Button)sender;
            btn.Text = "Yazılıyor... ⏳";
            btn.Enabled = false;

            try
            {
                
                YapayZeka ai = new YapayZeka();

                // Formdaki verileri gönder
                string sonuc = await ai.AciklamaUret(
                    cmbMarka.Text,
                    cmbModel.Text,
                    cmbPaket.Text,
                    txtYil.Text,
                    txtKm.Text,
                    txtRenk.Text
                );

                
                txtAciklama.Text = sonuc;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                
                btn.Text = "✨ Yapay Zeka ile Açıklama Yaz";
                btn.Enabled = true;
            }
        }

        private void MarkalariYukle()
        {
            
            cmbModel.DataSource = null;
            cmbPaket.DataSource = null;

            DataTable dt = _sAracYonetimi.GetMarkaListesi();
            cmbMarka.DisplayMember = "MarkaAdi";
            cmbMarka.ValueMember = "MarkaID";
            cmbMarka.DataSource = dt;
            cmbMarka.SelectedIndex = -1; 
        }

        
        private void VerileriDoldur()
        {
            DataRow row = _sAracYonetimi.GetAracBilgi(_gelenAracId);

            if (row != null)
            {
                
                cmbMarka.SelectedValue = row["MarkaID"];

                
                cmbModel.SelectedValue = row["ModelID"];

                
                cmbPaket.SelectedValue = row["PaketID"];

                
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
                DevExpress.XtraEditors.XtraMessageBox.Show("Lütfen araç model ve paketini eksiksiz seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                DevExpress.XtraEditors.XtraMessageBox.Show("İşlem başarıyla tamamlandı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Hata oluştu: " + sonuc, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- HATALARI GİDERMEK İÇİN EKLENEN BOŞ METOTLAR ---
        

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