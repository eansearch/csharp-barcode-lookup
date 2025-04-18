using System;
using System.Net;
using System.Web;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace BarcodeLookup
{
    /// <summary>
    /// Provides functionality to interact with the EAN barcode lookup API.
    /// </summary>
    public class BarcodeLookup
    {
        private readonly string _apiUrl;
        private int _timeout;
        private const int MAX_API_TRIES = 3;

        /// <summary>
        /// Initializes a new instance of the <see cref="BarcodeLookup"/> class with the provided API token.
        /// </summary>
        /// <param name="token">API token for authentication.</param>
        public BarcodeLookup(string token)
        {
            _apiUrl = $"https://api.ean-search.org/api?token={token}&format=json";
            _timeout = 180;
        }

        /// <summary>
        /// Sets the timeout duration for API requests.
        /// </summary>
        /// <param name="seconds">Timeout duration in seconds.</param>
        public void SetTimeout(int seconds)
        {
            _timeout = seconds;
        }

        /// <summary>
        /// Retrieves the product name associated with the given GTIN/Barcode.
        /// </summary>
        /// <param name="gtin">The GTIN/Barcode number.</param>
        /// <param name="lang">Preferred language code for the response (default is 1 = English).</param>
        /// <returns>The product name, or null if an error occurs.</returns>
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

        /// <summary>
        /// Retrieves the book title associated with the given ISBN.
        /// </summary>
        /// <param name="isbn">The ISBN number.</param>
        /// <returns>The book title, or null if an error occurs.</returns>
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

        /// <summary>
        /// Retrieves detailed product information associated with the given 13-digit GTIN/EAN Barcode.
        /// </summary>
        /// <param name="gtin">The GTIN/Barcode number.</param>
        /// <param name="lang">Language code for the response (default is 1 = English).</param>
        /// <returns>A dictionary containing product details, or null if an error occurs.</returns>
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

        /// <summary>
        /// Retrieves detailed product information associated with the given 12-digit UPC Barcode.
        /// </summary>
        /// <param name="gtin">The GTIN/Barcode number.</param>
        /// <param name="lang">Language code for the response (default is 1 = English).</param>
        /// <returns>A dictionary containing product details, or null if an error occurs.</returns>
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

        /// <summary>
        /// Verify the checksum of the given 13-digit GTIN/EAN Barcode.
        /// </summary>
        /// <param name="gtin">The GTIN/Barcode number.</param>
        /// <returns>True or false.</returns>
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

        /// <summary>
        /// Retrieves detailed product information for products matching the given search term.
        /// </summary>
        /// <param name="name">The search term.</param>
        /// <param name="page">Page number for the results (default is 0).</param>
        /// <param name="lang">Language code for the response (default is 1 = English).</param>
        /// <returns>A List of dictionaries containing product details.</returns>
        public List<Dictionary<string, object>> ProductSearch(string name, int page = 0, int lang = 1)
        {
            name = Quote(name);
            string url = $"{_apiUrl}&op=product-search&name={name}&page={page}&language={lang}";
            string contents = UrlOpen(url);
            var data = JsonSerializer.Deserialize<Dictionary<string, List<Dictionary<string, object>>>>(contents);
            return data?["productlist"];
        }

        /// <summary>
        /// Retrieves detailed product information for products similar to the given search term.
        /// </summary>
        /// <param name="name">The search term.</param>
        /// <param name="page">Page number for the results (default is 0).</param>
        /// <param name="lang">Language code for the response (default is 1 = English).</param>
        /// <returns>A List of dictionaries containing product details.</returns>
        public List<Dictionary<string, object>> SimilarProductSearch(string name, int page = 0, int lang = 1)
        {
            name = Quote(name);
            string url = $"{_apiUrl}&op=similar-product-search&name={name}&page={page}&language={lang}";
            string contents = UrlOpen(url);
            var data = JsonSerializer.Deserialize<Dictionary<string, List<Dictionary<string, object>>>>(contents);
            return data?["productlist"];
        }

        /// <summary>
        /// Retrieves detailed product information for products from a specific product category, matching to the given search term.
        /// </summary>
        /// <param name="category">The category code.</param>
        /// <param name="name">The search term.</param>
        /// <param name="page">Page number for the results (default is 0).</param>
        /// <param name="lang">Language code for the response (default is 1 = English).</param>
        /// <returns>A List of dictionaries containing product details.</returns>
        public List<Dictionary<string, object>> CategorySearch(int category, string name = "", int page = 0, int lang = 1)
        {
            name = Quote(name);
            string url = $"{_apiUrl}&op=category-search&category={category}&name={name}&page={page}&language={lang}";
            string contents = UrlOpen(url);
            var data = JsonSerializer.Deserialize<Dictionary<string, List<Dictionary<string, object>>>>(contents);
            return data?["productlist"];
        }

        /// <summary>
        /// Retrieves detailed product information for products for all products with a barcode starting with the given prefix.
        /// </summary>
        /// <param name="prefix">The barcode prefix.</param>
        /// <param name="page">Page number for the results (default is 0).</param>
        /// <param name="lang">Language code for the response (default is 1 = English).</param>
        public List<Dictionary<string, object>> BarcodePrefixSearch(string prefix, int page = 0, int lang = 1)
        {
            string url = $"{_apiUrl}&op=barcode-prefix-search&prefix={prefix}&page={page}&language={lang}";
            string contents = UrlOpen(url);
            var data = JsonSerializer.Deserialize<Dictionary<string, List<Dictionary<string, object>>>>(contents);
            return data?["productlist"];
        }

        /// <summary>
        /// Retrieves the issuing country for a given barcode.
        /// </summary>
        /// <param name="ean">The barcode.</param>
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

        /// <summary>
        /// Generate a PNG barcode image for a given 13-digit GTIN/EAN barcode.
        /// </summary>
        /// <param name="ean">The barcode.</param>
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

