using System.Drawing;
using System.Windows.Forms;

namespace OtoGaleri.WinForms
{
    public static class Tasarim
    {
        public static void Uygula(Form frm)
        {
            // 1. ORTAK YAZI TİPİ
            frm.Font = new Font("Segoe UI", 10, FontStyle.Regular);

            // Formun kendi arka plan rengini de koyu yapalım (Garanti olsun)
            // frm.BackColor = Color.FromArgb(38, 38, 38); 

            // 2. KONTROLLERİ GEZ VE DÜZENLE
            foreach (Control c in frm.Controls)
            {
                Ozellestir(c);
            }
        }

        private static void Ozellestir(Control c)
        {
            // İç içe paneller varsa onları da gez (Recursive)
            if (c.HasChildren)
            {
                foreach (Control child in c.Controls)
                {
                    Ozellestir(child);
                }
            }

            // --- BUTONLARI MODERNLEŞTİR ---
            if (c is Button btn)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Cursor = Cursors.Hand;
                btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            }

            // --- LABEL (ETİKET) RENKLERİ ---
            if (c is Label lbl)
            {
                lbl.ForeColor = Color.White; // Yazılar Beyaz
            }

            // --- TEXTBOX (YAZI KUTUSU) AYARLARI ---
            if (c is TextBox txt)
            {
                // Arka plan: Koyu Gri (Siyah değil, yumuşak bir gri)
                txt.BackColor = Color.FromArgb(50, 50, 50);
                // Yazı Rengi: Beyaz
                txt.ForeColor = Color.White;
                // Çerçeve: FixedSingle (3D görünümü kapatır, düz çizgi yapar)
                txt.BorderStyle = BorderStyle.FixedSingle;
            }

            // --- COMBOBOX (AÇILIR LİSTE) AYARLARI ---
            if (c is ComboBox cmb)
            {
                cmb.BackColor = Color.FromArgb(50, 50, 50);
                cmb.ForeColor = Color.White;
                cmb.FlatStyle = FlatStyle.Flat; // Modern görünüm
            }
        }
    }
}