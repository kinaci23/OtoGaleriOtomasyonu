using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtoGaleri.DataAccess
{
    /// <summary>
    /// Veritabanı bağlantı ve sorgu işlemlerini yöneten merkezi sınıf.
    /// </summary>
    public class SqlHelper
    {
        // [KURAL 8] private değişkenler class'ların ilk başında tanımlanmalı.
        private string _connectionString;

        /// <summary>
        /// Sınıfın kurucu methodu. Bağlantı cümlesini ayarlar.
        /// </summary>
        public SqlHelper()
        {
            // Bağlantı cümlesini App.config dosyasından okur.
            _connectionString = ConfigurationManager.ConnectionStrings["OtoGaleriBaglanti"].ConnectionString;
        }

        /// <summary>
        /// Select sorgularını çalıştırıp geriye DataTable döndüren method.
        /// [KURAL 5] Method isimleri Büyük harfle başlamalı.
        /// </summary>
        /// <param name="query">Çalıştırılacak SQL sorgusu</param>
        /// <returns>Sorgu sonucu dolu DataTable</returns>
        public DataTable GetDataTable(string query)
        {
            // [KURAL 35] try-catch blokları düzgün kullanılmalı
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                    {
                        DataTable dt = new DataTable();
                        con.Open();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama yapılabilir veya hata fırlatılabilir.
                throw new Exception("Veritabanı işlem hatası: " + ex.Message);
            }
        }

        /// <summary>
        /// Insert, Update, Delete işlemlerini yapan method.
        /// </summary>
        /// <param name="query">Çalıştırılacak SQL komutu</param>
        /// <returns>Etkilenen satır sayısı</returns>
        public int ExecuteQuery(string query)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Kayıt işlemi hatası: " + ex.Message);
            }
        }
    }
}