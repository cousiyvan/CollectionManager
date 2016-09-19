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
                game.Description = (string)jElement.SelectToken("summary");
                List<string> developers = jElement.SelectToken("developers").Select(s => (string)s).ToList<string>();
                game.Developers.AddRange(developers);
                List<string> publishers = jElement.SelectToken("publishers").Select(s => (string)s).ToList<string>();
                game.Publishers.AddRange(publishers);
                int category = (int)jElement.SelectToken("category");
                List<string> gameModes = jElement.SelectToken("game_modes").Select(s => (string)s).ToList<string>();
                game.GameModes.AddRange(gameModes);
                List<string> keywords = jElement.SelectToken("keywords").Select(s => (string)s).ToList<string>();
                game.Keywords.AddRange(keywords);
                List<string> themes = jElement.SelectToken("themes").Select(s => (string)s).ToList<string>();
                game.Themes.AddRange(themes);
                List<string> genres = jElement.SelectToken("genres").Select(s => (string)s).ToList<string>();
                game.Genres.AddRange(genres);
                game.ReleaseDate.Add((string)jElement.SelectToken("release_dates[0].platform"), Utils.UnixTimeStampToDateTime((double)jElement.SelectToken("release_dates[0].date")));
                game.Poster = new Uri(string.Format("https://res.cloudinary.com/igdb/image/upload/t_{0}/{1}.jpg", "cover_big", (string)jElement.SelectToken("cover.cloudinary_id")));
                List<string> pictures = jElement.SelectToken("screenshots").Select(s => (string)s.SelectToken("cloudinary_id")).ToList();
                foreach (var p in pictures)
                {
                    game.Pictures.Add(new Uri(string.Format("https://res.cloudinary.com/igdb/image/upload/t_{0}/{1}.jpg", "screenshot_med", p)));
                }
                List<string> videos = jElement.SelectToken("videos").Select(s => (string)s.SelectToken("video_id")).ToList();
                foreach (var v in videos)
                {
                    game.Videos.Add(new Uri(string.Format("https://youtu.be/{0}", v)));
                }

                //JArray nestJArray = (JArray)jElement.SelectToken("release_dates");
                //foreach (var nestElement in nestJArray)
                //{
                //    int? category = (int)nestElement.SelectToken("category");
                //    int? plateform = (int)nestElement.SelectToken("platform");
                //    long? releaseDate = (long)nestElement.SelectToken("date");
                //    int? region = (int)nestElement.SelectToken("region");
                //}
                // id 7346 name The Legend of Zelda: Breath of the Wild release_dates category 2 platform 41 date 1514674800000 region 8 
                // We get information from new call
                // restApi.Parameters = "";

                break;
            }
        }
    }
}
