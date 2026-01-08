using System;
using System.Drawing;
using System.Windows.Forms;
using OtoGaleri.Service.AracYonetimi;

namespace OtoGaleri.WinForms
{
    public partial class FrmAylikDetay : DevExpress.XtraEditors.XtraForm
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
            Tasarim.Uygula(this);
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
            dgvDetay.BorderStyle = BorderStyle.None;

            // --- KOYU TEMA DÜZELTMESİ ---
            dgvDetay.BackgroundColor = System.Drawing.Color.FromArgb(38, 38, 38);
            dgvDetay.GridColor = System.Drawing.Color.FromArgb(50, 50, 50);

            dgvDetay.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(38, 38, 38);
            dgvDetay.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvDetay.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(100, 100, 100);
            dgvDetay.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

            // Başlık Rengi
            dgvDetay.EnableHeadersVisualStyles = false;
            dgvDetay.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.Black;
            dgvDetay.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvDetay.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.Black;
            dgvDetay.ColumnHeadersHeight = 35;

            // --- PARA FORMATLARI ---
            if (dgvDetay.Columns["Alış"] != null) dgvDetay.Columns["Alış"].DefaultCellStyle.Format = "C2";
            if (dgvDetay.Columns["Satış"] != null) dgvDetay.Columns["Satış"].DefaultCellStyle.Format = "C2";

            if (dgvDetay.Columns["Kar"] != null)
            {
                dgvDetay.Columns["Kar"].DefaultCellStyle.Format = "C2";
                dgvDetay.Columns["Kar"].DefaultCellStyle.ForeColor = System.Drawing.Color.LimeGreen;
                dgvDetay.Columns["Kar"].DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            }
        }

        // Yanlışlıkla oluşturulan tıklama olayını susturmak için:
        private void dgvDetay_CellDoubleClick(object sender, DataGridViewCellEventArgs e) { }
    }
}