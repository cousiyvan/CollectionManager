using CollectionManager.Models.Collection;
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
        
        #endregion
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static List<string> GetContent(RestAPI restApi, Game.CallType callType, List<string> ids)
        {
            JsonReader json;
            List<string> restOutput = new List<string>();
            string callParam = string.Empty;

            switch (callType)
            {
                case Game.CallType.Companie:
                    callParam = "/companies/";
                    break;
                case Game.CallType.Franchise:
                    callParam = "/franchises/";
                    break;
                case Game.CallType.GamePerspective:
                    callParam = "/player_perspectives/";
                    break;
                case Game.CallType.Genres:
                    callParam = "/genres/";
                    break;
                case Game.CallType.Keyword:
                    callParam = "/keywords/";
                    break;
                case Game.CallType.Platform:
                    callParam = "/platforms/";
                    break;
                case Game.CallType.Serie:
                    callParam = "/series/";
                    break;
                case Game.CallType.Theme:
                    callParam = "/themes/";
                    break;
                case Game.CallType.GameModes:
                    callParam = "/game_modes/";
                    break;
                default:
                    callParam = "/games/";
                    break;
            }

            // Covnert from list to single string splitted with ","
            string idSplitted = string.Empty;
            if (ids.Count > 1)
            {
                ids.ForEach(x => idSplitted += string.Format("{0},", x));
                idSplitted = idSplitted.Remove(idSplitted.Length-1);
            }
            else
                ids.ForEach(x => idSplitted += string.Format("{0}", x));

            restApi.Parameters = string.Format("{0}{1}?fields=*&limit=40", callParam, idSplitted);

            json = restApi.DoCall();

            JArray jArray = JArray.Load(json);
            foreach (var jElement in jArray)
            {
                // Values from first call
                restOutput.Add((string)jElement.SelectToken("name"));
            }

            return restOutput;
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
