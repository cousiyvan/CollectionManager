using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CollectionManager.Utils
{
    public class RestAPI
    {
        #region Members
        private Dictionary<string, string> _apiKey;
        private Uri _restUrl;
        private string _parameters;
        #endregion

        #region Properties
        public string Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }
        #endregion

        #region Constructors
        public RestAPI()
        {
            this._apiKey = new Dictionary<string, string>();
            this._restUrl = null;
            this._parameters = "";
        }

        public RestAPI(Dictionary<string, string> apiKey, Uri restUrl, string parameters)
        {
            this._apiKey = apiKey;
            this._restUrl = restUrl;
            this._parameters = parameters;
        }
        #endregion


        public JsonReader DoCall()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = this._restUrl;
            string output = string.Empty;
            JsonReader jsonReader = null;

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add(this._apiKey.Keys.First(), this._apiKey.Values.First());
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(this._parameters).Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                var resp = response.Content.ReadAsStringAsync().Result;
                
                jsonReader = new JsonTextReader(new StringReader(resp));
            }
            else
            {
                // output += string.Format("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return jsonReader;
        }

        public JToken GetSpecificValue(string call, string field)
        {
            this.Parameters = call;
            JsonReader json = this.DoCall();

            // From json object we get the values to fill our object
            JToken token = JToken.Load(json);
            // Values from first call

            return token.SelectToken(field);
        }
    }
}
