using System;
using System.Net;
using System.Web;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace BarcodeLookup
{

    public class BarcodeLookup
    {
        private readonly string _apiUrl;
        private int _timeout;
        private const int MAX_API_TRIES = 3;

        public BarcodeLookup(string token)
        {
            _apiUrl = $"https://api.ean-search.org/api?token={token}&format=json";
            _timeout = 180;
        }

        public void SetTimeout(int seconds)
        {
            _timeout = seconds;
        }

        public string GTINName(string gtin, int lang = 1)
        {
            string url = $"{_apiUrl}&op=barcode-lookup&ean={gtin}&language={lang}";
            string contents = UrlOpen(url);
            var data = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(contents);

            if (data != null && data[0].ContainsKey("error"))
            {
                return null;
            }
            return data?[0]["name"].ToString();
        }

        public string Isbn(string isbn)
        {
            string url = $"{_apiUrl}&op=barcode-lookup&isbn={isbn}";
            string contents = UrlOpen(url);
            var data = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(contents);

            if (data != null && data[0].ContainsKey("error"))
            {
                return null;
            }
            return data?[0]["name"].ToString();
        }

        public Dictionary<string, object> GTIN(string gtin, int lang = 1)
        {
            string url = $"{_apiUrl}&op=barcode-lookup&ean={gtin}&language={lang}";
            string contents = UrlOpen(url);
            var data = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(contents);

            if (data != null && data[0].ContainsKey("error"))
            {
                return null;
            }
            return data?[0];
        }

        public Dictionary<string, object> UPC(string upc, int lang = 1)
        {
            string url = $"{_apiUrl}&op=barcode-lookup&ean={upc}&language={lang}";
            string contents = UrlOpen(url);
            var data = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(contents);

            if (data != null && data[0].ContainsKey("error"))
            {
                return null;
            }
            return data?[0];
        }

        public bool? VerifyChecksum(string ean)
        {
            string url = $"{_apiUrl}&op=verify-checksum&ean={ean}";
            string contents = UrlOpen(url);
            var data = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(contents);

            if (data != null && data[0].ContainsKey("error"))
            {
                return null;
            }
            return bool.Parse(data?[0]["valid"].ToString());
        }

        public List<Dictionary<string, object>> ProductSearch(string name, int page = 0, int lang = 1)
        {
            name = Quote(name);
            string url = $"{_apiUrl}&op=product-search&name={name}&page={page}&language={lang}";
            string contents = UrlOpen(url);
            var data = JsonSerializer.Deserialize<Dictionary<string, List<Dictionary<string, object>>>>(contents);
            return data?["productlist"];
        }

        public List<Dictionary<string, object>> SimilarProductSearch(string name, int page = 0, int lang = 1)
        {
            name = Quote(name);
            string url = $"{_apiUrl}&op=similar-product-search&name={name}&page={page}&language={lang}";
            string contents = UrlOpen(url);
            var data = JsonSerializer.Deserialize<Dictionary<string, List<Dictionary<string, object>>>>(contents);
            return data?["productlist"];
        }

        public List<Dictionary<string, object>> CategorySearch(string category, string name = "", int page = 0, int lang = 1)
        {
            name = Quote(name);
            string url = $"{_apiUrl}&op=category-search&category={category}&name={name}&page={page}&language={lang}";
            string contents = UrlOpen(url);
            var data = JsonSerializer.Deserialize<Dictionary<string, List<Dictionary<string, object>>>>(contents);
            return data?["productlist"];
        }

        public List<Dictionary<string, object>> BarcodePrefixSearch(string prefix, int page = 0, int lang = 1)
        {
            string url = $"{_apiUrl}&op=barcode-prefix-search&prefix={prefix}&page={page}&language={lang}";
            string contents = UrlOpen(url);
            var data = JsonSerializer.Deserialize<Dictionary<string, List<Dictionary<string, object>>>>(contents);
            return data?["productlist"];
        }

        public string IssuingCountryLookup(string ean)
        {
            string url = $"{_apiUrl}&op=issuing-country&ean={ean}";
            string contents = UrlOpen(url);
            var data = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(contents);

            if (data != null && data[0].ContainsKey("error"))
            {
                return null;
            }
            return data?[0]["issuingCountry"].ToString();
        }

        public string BarcodeImage(string ean)
        {
            string url = $"{_apiUrl}&op=barcode-image&ean={ean}";
            string contents = UrlOpen(url);
            var data = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(contents);

            if (data != null && data[0].ContainsKey("error"))
            {
                return null;
            }
            return data?[0]["barcode"].ToString();
        }

        private string Quote(string param)
        {
            return HttpUtility.UrlEncode(param);
        }

        private string UrlOpen(string url, int tries = 1)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = _timeout * 1000;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex) when (tries < MAX_API_TRIES && ((HttpWebResponse)ex.Response)?.StatusCode == (HttpStatusCode)429)
            {
                System.Threading.Thread.Sleep(1000);
                return UrlOpen(url, tries + 1);
            }
        }
    }

}

