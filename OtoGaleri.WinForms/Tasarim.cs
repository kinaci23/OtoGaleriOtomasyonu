using System;
using System.Drawing;
using System.Windows.Forms;

namespace OtoGaleri.WinForms
{
    public static class Tasarim
    {
        public static void Uygula(Form frm)
        {
            
            try
            {
                
                Bitmap resim = Properties.Resources.carIcon;
                IntPtr hIcon = resim.GetHicon(); 
                frm.Icon = Icon.FromHandle(hIcon); 
            }
            catch
            {
                
            }

            // --- 2. EKRAN AYARLARI ---
            frm.StartPosition = FormStartPosition.CenterScreen; 

            // --- 3. ORTAK YAZI TİPİ ---
            frm.Font = new Font("Segoe UI", 10, FontStyle.Regular);

            // --- 4. KONTROLLERİ GEZ VE DÜZENLE ---
            foreach (Control c in frm.Controls)
            {
                Ozellestir(c);
            }
        }

        private static void Ozellestir(Control c)
        {
            
            if (c.HasChildren)
            {
                foreach (Control child in c.Controls)
                {
                    Ozellestir(child);
                }
            }

            // --- BUTONLAR ---
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
                lbl.ForeColor = Color.White; 
            }

            // --- TEXTBOX (YAZI KUTUSU) AYARLARI ---
            if (c is TextBox txt)
            {
                // Arka plan: Koyu Gri 
                txt.BackColor = Color.FromArgb(50, 50, 50);
                // Yazı Rengi: Beyaz
                txt.ForeColor = Color.White;
                // Çerçeve: FixedSingle 
                txt.BorderStyle = BorderStyle.FixedSingle;
            }

            // --- COMBOBOX (AÇILIR LİSTE) AYARLARI ---
            if (c is ComboBox cmb)
            {
                cmb.BackColor = Color.FromArgb(50, 50, 50);
                cmb.ForeColor = Color.White;
                cmb.FlatStyle = FlatStyle.Flat; 
            }
        }
    }
}