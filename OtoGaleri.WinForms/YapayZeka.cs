using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OtoGaleri.WinForms
{
    public class YapayZeka
    {

        private const string ApiKey = "BURAYA_KEY_GELECEK";

        // Model Adresi (Gemini 2.5 Flash 
        private const string ApiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key=";

        public async Task<string> AciklamaUret(string marka, string model, string paket, string yil, string km, string renk)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Yapay Zekaya Gönderilecek Emir
                    string prompt = $@"Aşağıdaki araç için müşteriyi cezbeden, samimi, bol emojili, 
                                    satış odaklı kısa bir galeri ilan açıklaması yaz (maksimum 3 cümle).
                                    ÖNEMLİ KURAL: Asla kesme işareti (') veya tek tırnak kullanma. 
                                    Bunun yerine kelimeleri bitişik yaz veya cümleyi değiştir.
                                    Araç: {marka} {model} {paket}, Yıl: {yil}, KM: {km}, Renk: {renk}.";

                    // Gemini API'nin istediği özel JSON formatı
                    var requestBody = new
                    {
                        contents = new[]
                        {
                            new
                            {
                                parts = new[]
                                {
                                    new { text = prompt }
                                }
                            }
                        }
                    };

                    string jsonGonderilen = JsonConvert.SerializeObject(requestBody);
                    var content = new StringContent(jsonGonderilen, Encoding.UTF8, "application/json");

                    
                    HttpResponseMessage response = await client.PostAsync(ApiUrl + ApiKey, content);

                    
                    string jsonGelen = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        
                        dynamic data = JsonConvert.DeserializeObject(jsonGelen);

                        if (data.candidates != null && data.candidates.Count > 0)
                        {
                            return data.candidates[0].content.parts[0].text;
                        }
                        return "Yapay zeka boş bir cevap döndürdü.";
                    }
                    else
                    {
                        
                        return $"BAĞLANTI HATASI ({response.StatusCode}): {jsonGelen}";
                    }
                }
            }
            catch (Exception ex)
            {
                return "KRİTİK PROGRAM HATASI: " + ex.Message;
            }
        }
    }
}