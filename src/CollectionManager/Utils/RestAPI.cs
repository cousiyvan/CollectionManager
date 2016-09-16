using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CollectionManager.Utils
{
    public class RestAPI
    {
        #region Members
        private string _apiKey;
        private Uri _restUrl;
        private string _parameters;
        #endregion

        #region Constructors
        public RestAPI()
        {
            this._apiKey = "";
            this._restUrl = null;
            this._parameters = "";
        }

        public RestAPI(string apiKey, Uri restUrl, string parameters)
        {
            this._apiKey = apiKey;
            this._restUrl = restUrl;
            this._parameters = parameters;
        }
        #endregion


        public void DoCall()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = this._restUrl;

            // Add an Accept header for JSON format.
            // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(this._parameters).Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                var resp = response.Content.ReadAsAsync<IEnumerable<string>>().Result;
                foreach (var d in resp)
                {
                    Console.WriteLine("{0}", d);
                }
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }
    }
}
