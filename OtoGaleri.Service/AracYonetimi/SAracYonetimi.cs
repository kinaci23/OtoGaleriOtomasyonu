using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OtoGaleri.DataAccess; // DataAccess katmanını kullanacağız

namespace OtoGaleri.Service.AracYonetimi
{
    /// <summary>
    /// Araç yönetimi ile ilgili iş mantığını (Business Logic) yürüten servis sınıfı.
    /// </summary>
    public class SAracYonetimi
    {
        // DataAccess katmanındaki yardımcı sınıfı çağırıyoruz.
        private SqlHelper _sqlHelper;

        public SAracYonetimi()
        {
            _sqlHelper = new SqlHelper();
        }

        /// <summary>
        /// Veritabanındaki tüm markaları getirir.
        /// </summary>
        /// <returns>Marka listesini içeren DataTable</returns>
        public DataTable GetMarkaListesi()
        {
            try
            {
                string query = "SELECT MarkaID, MarkaAdi FROM Tbl_Markalar ORDER BY MarkaAdi";
                return _sqlHelper.GetDataTable(query);
            }
            catch (Exception)
            {
                // Hata durumunda loglama yapılabilir, şimdilik boş tablo dönüyoruz.
                return new DataTable();
            }
        }

        /// <summary>
        /// Seçilen markaya ait modelleri getirir.
        /// </summary>
        /// <param name="markaId">Filtrelenecek Marka ID</param>
        /// <returns>Model listesini içeren DataTable</returns>
        public DataTable GetModelListesiByMarkaId(int markaId)
        {
            try
            {
                // Parametreli sorgu yerine string format kullandık, gerçek projede parametre kullanılmalı
                string query = $"SELECT ModelID, ModelAdi FROM Tbl_Modeller WHERE MarkaID = {markaId} ORDER BY ModelAdi";
                return _sqlHelper.GetDataTable(query);
            }
            catch (Exception)
            {
                return new DataTable();
            }
        }

        /// <summary>
        /// Seçilen modele ait donanım paketlerini getirir.
        /// </summary>
        /// <param name="modelId">Filtrelenecek Model ID</param>
        /// <returns>Paket listesini içeren DataTable</returns>
        public DataTable GetPaketListesiByModelId(int modelId)
        {
            try
            {
                string query = $"SELECT PaketID, PaketAdi FROM Tbl_Paketler WHERE ModelID = {modelId} ORDER BY PaketAdi";
                return _sqlHelper.GetDataTable(query);
            }
            catch (Exception)
            {
                return new DataTable();
            }
        }

        /// <summary>
        /// Yeni bir araç kaydını veritabanına ekler.
        /// </summary>
        public string AracEkle(int paketId, string yil, int km, string renk, decimal alisFiyat, decimal satisFiyat, string aciklama)
        {
            // Kural: string hata variable'ı null atanıp...
            string hata = null;

            try
            {
                // SQL Sorgusu (Basit string birleştirme ile)
                string query = $@"INSERT INTO Tbl_Araclar 
                                (PaketID, Yil, Km, Renk, AlisFiyat, SatisFiyat, Aciklama, KayitTarihi) 
                                VALUES 
                                ({paketId}, '{yil}', {km}, '{renk}', {alisFiyat.ToString().Replace(',', '.')}, {satisFiyat.ToString().Replace(',', '.')}, '{aciklama}', GETDATE())";

                int result = _sqlHelper.ExecuteQuery(query);

                if (result <= 0)
                {
                    hata = "Kayıt veritabanına eklenemedi.";
                }
            }
            catch (Exception ex)
            {
                // Kural: Exception'da message'a eşitlenmiş mi?
                hata = ex.Message;
            }

            // Kural: Tüm methodlar string döndürmeli (Hata yoksa null döner).
            return hata;
        }

        /// <summary>
        /// GridView'de göstermek üzere araçların detaylı listesini (Marka, Model, Paket isimleriyle birleştirerek) getirir.
        /// </summary>
        /// <returns>Özelleştirilmiş araç listesi tablosu</returns>
        /// <summary>
        /// Araçları durumuna göre (Galeride veya Satıldı) filtreleyerek getirir.
        /// </summary>
        /// <param name="satildiMi">false: Galeridekiler, true: Satılanlar</param>
        public DataTable GetAracListesiByDurum(bool satildiMi)
        {
            try
            {
                // Parametreden gelen true/false değerini veritabanındaki 1/0 formatına çeviriyoruz
                int durumKodu = satildiMi ? 1 : 0;

                string query = $@"
                    SELECT 
                        A.AracID,
                        M.MarkaAdi AS [Marka],
                        Mo.ModelAdi AS [Model],
                        P.PaketAdi AS [Paket / Donanım],
                        A.Yil AS [Yıl],
                        A.Km,
                        A.Renk,
                        A.SatisFiyat AS [Satış Fiyatı],
                        CASE 
                            WHEN A.SatisDurumu = 1 THEN 'SATILDI' 
                            ELSE 'GALERİDE' 
                        END AS [Durum]
                    FROM Tbl_Araclar A
                    INNER JOIN Tbl_Paketler P ON A.PaketID = P.PaketID
                    INNER JOIN Tbl_Modeller Mo ON P.ModelID = Mo.ModelID
                    INNER JOIN Tbl_Markalar M ON Mo.MarkaID = M.MarkaID
                    WHERE A.SatisDurumu = {durumKodu}  -- FİLTRE BURADA
                    ORDER BY A.KayitTarihi DESC";

                return _sqlHelper.GetDataTable(query);
            }
            catch (Exception)
            {
                return new DataTable();
            }
        }

        // --- YENİ EKLENEN METOTLAR (GÜNCELLEME İÇİN) ---

        /// <summary>
        /// ID'si verilen aracın tüm bilgilerini (Marka ve Model ID'leri dahil) getirir.
        /// Formu düzenleme modunda açarken kutuları doldurmak için kullanılır.
        /// </summary>
        public DataRow GetAracBilgi(int aracId)
        {
            try
            {
                // Aracı getirirken Paket -> Model -> Marka ilişkisiyle üst ID'leri de çekiyoruz.
                // Bu sayede ComboBox'ları doğru seçili getirebileceğiz.
                string query = $@"
                    SELECT 
                        A.*, 
                        P.ModelID, 
                        Mo.MarkaID 
                    FROM Tbl_Araclar A
                    INNER JOIN Tbl_Paketler P ON A.PaketID = P.PaketID
                    INNER JOIN Tbl_Modeller Mo ON P.ModelID = Mo.ModelID
                    WHERE A.AracID = {aracId}";

                DataTable dt = _sqlHelper.GetDataTable(query);

                if (dt.Rows.Count > 0)
                    return dt.Rows[0]; // Tek bir satır (DataRow) döndür
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Mevcut bir araç kaydını günceller.
        /// </summary>
        public string AracGuncelle(int aracId, int paketId, string yil, int km, string renk, decimal alisFiyat, decimal satisFiyat, string aciklama)
        {
            string hata = null;
            try
            {
                // UPDATE sorgusu
                string query = $@"
                    UPDATE Tbl_Araclar SET 
                        PaketID = {paketId},
                        Yil = '{yil}',
                        Km = {km},
                        Renk = '{renk}',
                        AlisFiyat = {alisFiyat.ToString().Replace(',', '.')},
                        SatisFiyat = {satisFiyat.ToString().Replace(',', '.')},
                        Aciklama = '{aciklama}'
                    WHERE AracID = {aracId}";

                int result = _sqlHelper.ExecuteQuery(query);

                if (result <= 0)
                    hata = "Güncelleme işlemi yapılamadı.";
            }
            catch (Exception ex)
            {
                hata = ex.Message;
            }
            return hata;
        }

        /// <summary>
        /// ID'si verilen aracı veritabanından siler.
        /// </summary>
        public string AracSil(int aracId)
        {
            string hata = null;
            try
            {
                string query = $"DELETE FROM Tbl_Araclar WHERE AracID = {aracId}";
                int result = _sqlHelper.ExecuteQuery(query);

                if (result <= 0)
                    hata = "Silme işlemi başarısız oldu veya kayıt bulunamadı.";
            }
            catch (Exception ex)
            {
                hata = ex.Message; // İlişkisel veriler varsa (örn: Satış tablosu) SQL hatası dönebilir.
            }
            return hata;
        }

        /// <summary>
        /// ID'si verilen aracı "Satıldı" olarak işaretler.
        /// </summary>
        public string AracSat(int aracId)
        {
            string hata = null;
            try
            {
                // SatisDurumu = 1 (Satıldı) yapıyoruz.
                // Satış Tarihini de şu an (GETDATE) olarak güncelliyoruz ki raporlarda lazım olacak.
                string query = $"UPDATE Tbl_Araclar SET SatisDurumu = 1, SatisTarihi = GETDATE() WHERE AracID = {aracId}";

                int result = _sqlHelper.ExecuteQuery(query);

                if (result <= 0)
                    hata = "Satış işlemi gerçekleştirilemedi.";
            }
            catch (Exception ex)
            {
                hata = ex.Message;
            }
            return hata;
        }
        /// <summary>
        /// Satılan araçların toplam alış, toplam satış ve adet bilgisini getirir.
        /// </summary>
        public DataRow GetSatisRaporu()
        {
            try
            {
                // SQL'in SUM fonksiyonu ile veritabanında toplama işlemi yapıyoruz.
                // ISNULL: Eğer hiç satış yoksa NULL yerine 0 gelmesini sağlar.
                string query = @"
                    SELECT 
                        COUNT(*) as ToplamAdet, 
                        ISNULL(SUM(AlisFiyat), 0) as ToplamAlis, 
                        ISNULL(SUM(SatisFiyat), 0) as ToplamSatis 
                    FROM Tbl_Araclar 
                    WHERE SatisDurumu = 1"; // Sadece satılanları topla

                DataTable dt = _sqlHelper.GetDataTable(query);

                if (dt.Rows.Count > 0)
                    return dt.Rows[0];
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}