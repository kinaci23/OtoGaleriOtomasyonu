using System;
using System.Windows.Forms;
using DevExpress.UserSkins;
using DevExpress.Skins;
using DevExpress.LookAndFeel;

namespace OtoGaleri.WinForms
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // 1. KOYU TEMA AKTİVASYONU
            SkinManager.EnableFormSkins();
            BonusSkins.Register();
            UserLookAndFeel.Default.SetSkinStyle("Office 2019 Black");

            // 2. Standart Windows Ayarları
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 3. Başlat
            Application.Run(new FrmLogin());
        }
    }
}