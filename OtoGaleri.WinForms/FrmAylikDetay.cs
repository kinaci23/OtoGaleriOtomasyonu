using System;
using System.Drawing;
using System.Windows.Forms;
using OtoGaleri.Service.AracYonetimi;

namespace OtoGaleri.WinForms
{
    public partial class FrmAylikDetay : Form
    {
        private int _yil;
        private int _ayNo;
        private string _ayAdi;
        private SAracYonetimi _service;

        public FrmAylikDetay(int yil, int ayNo, string ayAdi)
        {
            InitializeComponent();
            _yil = yil;
            _ayNo = ayNo;
            _ayAdi = ayAdi;
            _service = new SAracYonetimi();
        }

        private void FrmAylikDetay_Load(object sender, EventArgs e)
        {
            this.Text = $"{_ayAdi} {_yil} - Satış Detayları";

            // Verileri dgvDetay tablosuna yükle
            dgvDetay.DataSource = _service.GetAylikSatisDetay(_yil, _ayNo);
            GridAyarlariniYap();
        }

        private void GridAyarlariniYap()
        {
            if (dgvDetay.DataSource == null) return;

            // --- TEMEL AYARLAR ---
            dgvDetay.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetay.RowHeadersVisible = false;
            dgvDetay.ReadOnly = true;
            dgvDetay.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetay.BackgroundColor = SystemColors.Control;
            dgvDetay.BorderStyle = BorderStyle.None;

            // --- RENK AYARLARI (MAVİLERİ YOK ETME) ---
            // 1. Satır seçilince GRİ olsun
            dgvDetay.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 200, 200);
            dgvDetay.DefaultCellStyle.SelectionForeColor = Color.Black;

            // 2. Başlık (Header) Rengi
            dgvDetay.EnableHeadersVisualStyles = false;
            dgvDetay.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
            dgvDetay.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvDetay.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(45, 45, 48);
            dgvDetay.ColumnHeadersHeight = 35;

            // --- PARA BİRİMİ VE RENKLENDİRME ---
            if (dgvDetay.Columns["Alış"] != null) dgvDetay.Columns["Alış"].DefaultCellStyle.Format = "C2";
            if (dgvDetay.Columns["Satış"] != null) dgvDetay.Columns["Satış"].DefaultCellStyle.Format = "C2";

            if (dgvDetay.Columns["Kar"] != null)
            {
                dgvDetay.Columns["Kar"].DefaultCellStyle.Format = "C2";
                dgvDetay.Columns["Kar"].DefaultCellStyle.ForeColor = Color.SeaGreen;
                dgvDetay.Columns["Kar"].DefaultCellStyle.SelectionForeColor = Color.DarkGreen;
                dgvDetay.Columns["Kar"].DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            }
        }

        // Yanlışlıkla oluşturulan tıklama olayını susturmak için:
        private void dgvDetay_CellDoubleClick(object sender, DataGridViewCellEventArgs e) { }
    }
}