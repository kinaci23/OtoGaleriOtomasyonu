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

        // =============================================================
        // BÖLÜM 1: LİSTELEME VE COMBOBOX VERİLERİ
        // =============================================================

        /// <summary>
        /// Veritabanındaki tüm markaları getirir.
        /// </summary>
        public DataTable GetMarkaListesi()
        {
            try
            {
                string query = "SELECT MarkaID, MarkaAdi FROM Tbl_Markalar ORDER BY MarkaAdi";
                return _sqlHelper.GetDataTable(query);
            }
            catch { return new DataTable(); }
        }

        /// <summary>
        /// Seçilen markaya ait modelleri getirir.
        /// </summary>
        public DataTable GetModelListesiByMarkaId(int markaId)
        {
            try
            {
                string query = $"SELECT ModelID, ModelAdi FROM Tbl_Modeller WHERE MarkaID = {markaId} ORDER BY ModelAdi";
                return _sqlHelper.GetDataTable(query);
            }
            catch { return new DataTable(); }
        }

        /// <summary>
        /// Seçilen modele ait donanım paketlerini getirir.
        /// </summary>
        public DataTable GetPaketListesiByModelId(int modelId)
        {
            try
            {
                string query = $"SELECT PaketID, PaketAdi FROM Tbl_Paketler WHERE ModelID = {modelId} ORDER BY PaketAdi";
                return _sqlHelper.GetDataTable(query);
            }
            catch { return new DataTable(); }
        }

        /// <summary>
        /// Araçları durumuna göre (Galeride veya Satıldı) filtreleyerek getirir.
        /// </summary>
        public DataTable GetAracListesiByDurum(bool satildiMi)
        {
            try
            {
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
                    WHERE A.SatisDurumu = {durumKodu}
                    ORDER BY A.KayitTarihi DESC";

                return _sqlHelper.GetDataTable(query);
            }
            catch { return new DataTable(); }
        }

        // =============================================================
        // BÖLÜM 2: ARAMA İŞLEMİ (EKSİK OLAN KISIM BURASIYDI)
        // =============================================================

        /// <summary>
        /// Marka, Model veya Yıl bilgisine göre arama yapar.
        /// </summary>
        public DataTable AracAra(string arananKelime, bool satildiMi)
        {
            try
            {
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
                    WHERE A.SatisDurumu = {durumKodu} 
                    AND (
                        M.MarkaAdi LIKE '%{arananKelime}%' OR 
                        Mo.ModelAdi LIKE '%{arananKelime}%' OR 
                        A.Yil LIKE '%{arananKelime}%'
                    )
                    ORDER BY A.KayitTarihi DESC";

                return _sqlHelper.GetDataTable(query);
            }
            catch (Exception)
            {
                return new DataTable();
            }
        }
        public DataTable GetAylikSatisDetay(int yil, int ayNo)
        {
            try
            {
                string query = $@"
                    SELECT 
                        M.MarkaAdi + ' ' + Mo.ModelAdi AS [Araç],
                        A.Yil,
                        A.MusteriAdSoyad AS [Müşteri],
                        A.MusteriTelefon AS [Telefon],
                        A.AlisFiyat AS [Alış],
                        A.SatisFiyat AS [Satış],
                        (A.SatisFiyat - A.AlisFiyat) AS [Kar]
                    FROM Tbl_Araclar A
                    INNER JOIN Tbl_Paketler P ON A.PaketID = P.PaketID
                    INNER JOIN Tbl_Modeller Mo ON P.ModelID = Mo.ModelID
                    INNER JOIN Tbl_Markalar M ON Mo.MarkaID = M.MarkaID
                    WHERE A.SatisDurumu = 1 
                    AND YEAR(A.SatisTarihi) = {yil} 
                    AND MONTH(A.SatisTarihi) = {ayNo}
                    ORDER BY A.SatisTarihi DESC";

                return _sqlHelper.GetDataTable(query);
            }
            catch
            {
                return new DataTable();
            }
        }

        // =============================================================
        // BÖLÜM 3: CRUD İŞLEMLERİ (EKLE - GÜNCELLE - SİL - GETİR)
        // =============================================================

        /// <summary>
        /// ID'si verilen aracın tüm bilgilerini getirir (Düzenleme ekranı için).
        /// </summary>
        public DataRow GetAracBilgi(int aracId)
        {
            try
            {
                string query = $@"
                    SELECT A.*, P.ModelID, Mo.MarkaID 
                    FROM Tbl_Araclar A
                    INNER JOIN Tbl_Paketler P ON A.PaketID = P.PaketID
                    INNER JOIN Tbl_Modeller Mo ON P.ModelID = Mo.ModelID
                    WHERE A.AracID = {aracId}";

                DataTable dt = _sqlHelper.GetDataTable(query);

                if (dt.Rows.Count > 0) return dt.Rows[0];
                else return null;
            }
            catch { return null; }
        }

        /// <summary>
        /// Yeni araç ekler.
        /// </summary>
        public string AracEkle(int paketId, string yil, int km, string renk, decimal alisFiyat, decimal satisFiyat, string aciklama)
        {
            string hata = null;
            try
            {
                string query = $@"INSERT INTO Tbl_Araclar 
                                (PaketID, Yil, Km, Renk, AlisFiyat, SatisFiyat, Aciklama, KayitTarihi, SatisDurumu) 
                                VALUES 
                                ({paketId}, '{yil}', {km}, '{renk}', {alisFiyat.ToString().Replace(',', '.')}, {satisFiyat.ToString().Replace(',', '.')}, '{aciklama}', GETDATE(), 0)";

                int result = _sqlHelper.ExecuteQuery(query);
                if (result <= 0) hata = "Kayıt eklenemedi.";
            }
            catch (Exception ex) { hata = ex.Message; }
            return hata;
        }

        /// <summary>
        /// Mevcut aracı günceller.
        /// </summary>
        public string AracGuncelle(int aracId, int paketId, string yil, int km, string renk, decimal alisFiyat, decimal satisFiyat, string aciklama)
        {
            string hata = null;
            try
            {
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
                if (result <= 0) hata = "Güncelleme yapılamadı.";
            }
            catch (Exception ex) { hata = ex.Message; }
            return hata;
        }

        /// <summary>
        /// Aracı siler.
        /// </summary>
        public string AracSil(int aracId)
        {
            string hata = null;
            try
            {
                string query = $"DELETE FROM Tbl_Araclar WHERE AracID = {aracId}";
                int result = _sqlHelper.ExecuteQuery(query);
                if (result <= 0) hata = "Silme işlemi başarısız.";
            }
            catch (Exception ex) { hata = ex.Message; }
            return hata;
        }

        // =============================================================
        // BÖLÜM 4: SATIŞ VE RAPORLAMA
        // =============================================================

        /// <summary>
        /// ID'si verilen aracı "Satıldı" yapar ve müşteri bilgisini kaydeder.
        /// </summary>
        public string AracSat(int aracId, string adSoyad, string telefon)
        {
            string hata = null;
            try
            {
                string query = $@"
                    UPDATE Tbl_Araclar SET 
                        SatisDurumu = 1, 
                        SatisTarihi = GETDATE(),
                        MusteriAdSoyad = '{adSoyad}',
                        MusteriTelefon = '{telefon}'
                    WHERE AracID = {aracId}";

                int result = _sqlHelper.ExecuteQuery(query);
                if (result <= 0) hata = "Satış işlemi gerçekleştirilemedi.";
            }
            catch (Exception ex) { hata = ex.Message; }
            return hata;
        }

        /// <summary>
        /// Satılan araçların toplam kar/zarar raporunu getirir.
        /// </summary>
        public DataRow GetSatisRaporu()
        {
            try
            {
                string query = @"
                    SELECT 
                        COUNT(*) as ToplamAdet, 
                        ISNULL(SUM(AlisFiyat), 0) as ToplamAlis, 
                        ISNULL(SUM(SatisFiyat), 0) as ToplamSatis 
                    FROM Tbl_Araclar 
                    WHERE SatisDurumu = 1";

                DataTable dt = _sqlHelper.GetDataTable(query);

                if (dt.Rows.Count > 0) return dt.Rows[0];
                else return null;
            }
            catch { return null; }
        }
        /// <summary>
        /// Satışları aylara göre gruplayarak detaylı rapor getirir.
        /// </summary>
        public DataTable GetAylikAnaliz()
        {
            try
            {
                // Ay numarasını (1,2..) İsime (Ocak, Şubat..) çeviren SQL yapısı:
                string query = @"
                    SELECT 
                        YEAR(SatisTarihi) AS [Yil],
                        CASE MONTH(SatisTarihi)
                            WHEN 1 THEN 'Ocak' WHEN 2 THEN 'Şubat' WHEN 3 THEN 'Mart'
                            WHEN 4 THEN 'Nisan' WHEN 5 THEN 'Mayıs' WHEN 6 THEN 'Haziran'
                            WHEN 7 THEN 'Temmuz' WHEN 8 THEN 'Ağustos' WHEN 9 THEN 'Eylül'
                            WHEN 10 THEN 'Ekim' WHEN 11 THEN 'Kasım' WHEN 12 THEN 'Aralık'
                        END AS [Ay],
                        MONTH(SatisTarihi) AS [AyNo], -- Sıralama yapmak için gizli numara tutuyoruz
                        COUNT(*) AS [Adet],
                        SUM(ISNULL(SatisFiyat, 0)) AS [Ciro],
                        SUM(ISNULL(SatisFiyat, 0) - ISNULL(AlisFiyat, 0)) AS [ToplamKar]
                    FROM Tbl_Araclar
                    WHERE SatisDurumu = 1 AND SatisTarihi IS NOT NULL
                    GROUP BY YEAR(SatisTarihi), MONTH(SatisTarihi)
                    ORDER BY Yil DESC, AyNo DESC"; // Numaraya göre sırala ama ismi göster

                return _sqlHelper.GetDataTable(query);
            }
            catch (Exception)
            {
                return new DataTable();
            }
        }
    }
}