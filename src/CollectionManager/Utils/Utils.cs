using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollectionManager.Utils
{
    public static class Utils {
        #region Properties
        public static Dictionary<int, string> GamesGenres = new Dictionary<int, string>();
        public static Dictionary<int, string> GamesCompanies = new Dictionary<int, string>();
        public static Dictionary<int, string> GamesPlatforms = new Dictionary<int, string>();
        public static Dictionary<int, string> GamesKeywords = new Dictionary<int, string>();
        public static Dictionary<int, string> GamesMode = new Dictionary<int, string>();
        public static Dictionary<int, string> GamesFranchises = new Dictionary<int, string>();
        public static Dictionary<int, string> GamesSeries = new Dictionary<int, string>();
        public static Dictionary<int, string> GamesThemes = new Dictionary<int, string>();
        public static Dictionary<int, string> GamesPerspectives = new Dictionary<int, string>();
        #endregion
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static void FillBasicLists()
        {
            Uri gameApiRestUrl = null;
            string parameters = string.Empty;
            Dictionary<string, string> apiKey = new Dictionary<string, string>();
            apiKey.Add("X-Mashape-Key", "MtG6phh7A5mshHpRiU1ooJK3glr9p1S1VsejsnX9JomArwQspE");
            RestAPI restApi = null;
            JsonReader json;
            string restOutput = string.Empty;
            MapperGame mapper = new MapperGame();

            if (Uri.TryCreate("https://igdbcom-internet-game-database-v1.p.mashape.com/", UriKind.Absolute, out gameApiRestUrl))
            {
                // Genres
                parameters = "genres/?fields=*&limit=50&offset=0"; ;
                restApi = new RestAPI(apiKey, gameApiRestUrl, parameters);

                json = restApi.DoCall();
                JArray jArray = JArray.Load(json);
                foreach (var jElement in jArray)
                {
                    GamesGenres.Add((int)jElement.SelectToken("id"), (string)jElement.SelectToken("name"));
                }

                // Platforms
                restApi.Parameters = "platforms/?fields=*&limit=50&offset=0";

                json = restApi.DoCall();
                jArray = JArray.Load(json);
                foreach (var jElement in jArray)
                {
                    GamesPlatforms.Add((int)jElement.SelectToken("id"), (string)jElement.SelectToken("name"));
                }

                // Keywords
                restApi.Parameters = "keywords/?fields=*&limit=50&offset=0";

                json = restApi.DoCall();
                jArray = JArray.Load(json);
                foreach (var jElement in jArray)
                {
                    GamesKeywords.Add((int)jElement.SelectToken("id"), (string)jElement.SelectToken("name"));
                }

                // Companies
                restApi.Parameters = "companies/?fields=*&limit=50&offset=0";

                json = restApi.DoCall();
                jArray = JArray.Load(json);
                foreach (var jElement in jArray)
                {
                    GamesCompanies.Add((int)jElement.SelectToken("id"), (string)jElement.SelectToken("name"));
                }

                // Modes
                restApi.Parameters = "game_modes/?fields=*&limit=50&offset=0";

                json = restApi.DoCall();
                jArray = JArray.Load(json);
                foreach (var jElement in jArray)
                {
                    GamesMode.Add((int)jElement.SelectToken("id"), (string)jElement.SelectToken("name"));
                }

                // Franchises
                restApi.Parameters = "franchises/?fields=*&limit=50&offset=0";

                json = restApi.DoCall();
                jArray = JArray.Load(json);
                foreach (var jElement in jArray)
                {
                    GamesFranchises.Add((int)jElement.SelectToken("id"), (string)jElement.SelectToken("name"));
                }

                // Series
                restApi.Parameters = "collections/?fields=*&limit=50&offset=0";

                json = restApi.DoCall();
                jArray = JArray.Load(json);
                foreach (var jElement in jArray)
                {
                    GamesSeries.Add((int)jElement.SelectToken("id"), (string)jElement.SelectToken("name"));
                }

                // Perspective
                restApi.Parameters = "player_perspectives/?fields=*&limit=50&offset=0";

                json = restApi.DoCall();
                jArray = JArray.Load(json);
                foreach (var jElement in jArray)
                {
                    GamesPerspectives.Add((int)jElement.SelectToken("id"), (string)jElement.SelectToken("name"));
                }
            }
        }

        public static List<T> ConvertFromIdToElement<T>(Dictionary<int, T> mappingTable, List<T> elements)
        {
            List<T> output = new List<T>();
            foreach (var e in elements)
            {
                var linqCall = from m in mappingTable
                               where m.Key == int.Parse(e.ToString())
                               select m.Value;
                output.AddRange(linqCall.ToList<T>());
            }

            return output;
        }
    }
}
