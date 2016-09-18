using CollectionManager.Models.Collection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollectionManager.Utils
{
    public class MapperGame
    {
        #region constructor
        public MapperGame()
        {

        }
        #endregion

        public void Mapping(JsonReader json, ref Game game, RestAPI restApi)
        {
            // From json object we get the values to fill our object
            JArray jArray = JArray.Load(json);
            foreach (var jElement in jArray)
            {
                // Values from first call
                game.Id = (int)jElement.SelectToken("id");
                game.Title = (string)jElement.SelectToken("name");
                game.OriginalTitle = game.Title;

                JArray nestJArray = (JArray)jElement.SelectToken("release_dates");
                foreach (var nestElement in nestJArray)
                {
                    int category = (int)jElement.SelectToken("category");
                    int plateform = (int)jElement.SelectToken("platform");
                    DateTime? releaseDate = (DateTime)jElement.SelectToken("date");
                    int region = (int)jElement.SelectToken("region");
                }
                // id 7346 name The Legend of Zelda: Breath of the Wild release_dates category 2 platform 41 date 1514674800000 region 8 
                // We get information from new call
                // restApi.Parameters = "";
            }
        }
    }
}
