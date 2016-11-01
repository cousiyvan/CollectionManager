using CollectionManager.Data;
using CollectionManager.Models;
using CollectionManager.Models.Collection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollectionManager.Utils
{
    public class MapperMovie
    {
        #region constructor
        public MapperMovie()
        {

        }
        #endregion

        public Movie Mapping(JsonReader json, RestAPI restApi, MovieContext context, ApplicationUser user, ILogger logger)
        {
            Movie movie = new Movie();

            try
            {
                // From json object we get the values to fill our object
                JToken jtoken = JToken.Load(json);

                // Values from first call
                movie.Id = (int)jtoken.SelectToken("id");
                movie.Budget = (int)jtoken.SelectToken("budget");
                movie.Description = (string)jtoken.SelectToken("overview");
                movie.ReleaseDate = new Dictionary<string, DateTime>() { { "", (DateTime)jtoken.SelectToken("release_date") } };
                movie.Title= (string)jtoken.SelectToken("title");
                movie.Runtime = (TimeSpan)jtoken.SelectToken("runtime");
                movie.OriginalTitle = (string)jtoken.SelectToken("original_title");
                movie.Benefits = (int)jtoken.SelectToken("revenue");

                if (jtoken.SelectTokens("genres") != null)
                {
                    List<string> genres = jtoken.SelectTokens("genres").Select(s => (string)s.SelectToken("name")).ToList<string>();
                    movie.Genres.AddRange(genres);
                }

                if (jtoken.SelectTokens("production_companies") != null)
                {
                    List<string> companies = jtoken.SelectTokens("production_companies").Select(s => (string)s.SelectToken("name")).ToList<string>();
                    movie.Publishers.AddRange(companies);
                }
                movie.Websites = new List<Uri>() {
                    new Uri($"http://www.imdb.com/title/{jtoken.SelectToken("imdb_id")}")
                };

                movie.Poster = new Uri($"https://image.tmdb.org/t/p/w300_and_h450_bestv2/{jtoken.SelectToken("poster_path")}");
                //game.Title = (string)jElement.SelectToken("name");
                //game.OriginalTitle = game.Title;
                //game.Description = (string)jElement.SelectToken("summary");
                //game.Storyline = (string)jElement.SelectToken("storyline");

                //List<string> developers = new List<string>();
                //JToken jToken = jElement.SelectToken("developers");
                //if (jToken != null)
                //{
                //    developers = jElement.SelectToken("developers").Select(s => (string)s).ToList<string>();
                //    developers = Utils.CheckDbContentExistence(Game.CallType.Companie, restApi, developers, context);
                //    game.Developers.AddRange(developers);
                //}

                //List<string> publishers = new List<string>();
                //jToken = jElement.SelectToken("publishers");
                //if (jToken != null)
                //{
                //    publishers = jElement.SelectToken("publishers").Select(s => (string)s).ToList<string>();
                //    publishers = Utils.CheckDbContentExistence(Game.CallType.Companie, restApi, publishers, context);
                //    game.Publishers.AddRange(publishers);
                //}

                //int category = (int)jElement.SelectToken("category");

                //List<string> gameModes = new List<string>();
                //jToken = jElement.SelectToken("game_modes");
                //if (jToken != null)
                //{
                //    gameModes = jElement.SelectToken("game_modes").Select(s => (string)s).ToList<string>();
                //    gameModes = Utils.CheckDbContentExistence(Game.CallType.GameModes, restApi, gameModes, context);
                //    game.GameModes.AddRange(gameModes);
                //}

                //List<string> keywords = new List<string>();
                //jToken = jElement.SelectToken("keywords");
                //if (jToken != null)
                //{
                //    keywords = jElement.SelectToken("keywords").Select(s => (string)s).ToList<string>();
                //    keywords = Utils.CheckDbContentExistence(Game.CallType.Keyword, restApi, keywords, context);
                //    game.Keywords.AddRange(keywords);
                //}

                //List<string> themes = new List<string>();
                //jToken = jElement.SelectToken("themes");
                //if (jToken != null)
                //{
                //    themes = jElement.SelectToken("themes").Select(s => (string)s).ToList<string>();
                //    themes = Utils.CheckDbContentExistence(Game.CallType.Theme, restApi, themes, context);
                //    game.Themes.AddRange(themes);
                //}

                //List<string> genres = new List<string>();
                //jToken = jElement.SelectToken("genres");
                //if (jToken != null)
                //{
                //    genres = jElement.SelectToken("genres").Select(s => (string)s).ToList<string>();
                //    genres = Utils.CheckDbContentExistence(Game.CallType.Genres, restApi, genres, context);
                //    game.Genres.AddRange(genres);
                //}

                //foreach (var token in jElement.SelectTokens("release_dates"))
                //{
                //    string platform = (string)token.Children()["platform"].ToList()[0];
                //    platform = Utils.GetContent(restApi, Game.CallType.Platform, new List<string>() { platform })[0];
                //    game.ReleaseDate.Add(platform, Utils.UnixTimeStampToDateTime((double)token.Children()["date"].ToList()[0]));
                //}

                //if (!string.IsNullOrEmpty((string)jElement.SelectToken("cover.cloudinary_id")))
                //    game.Poster = new Uri(string.Format("https://res.cloudinary.com/igdb/image/upload/t_{0}/{1}.jpg", "cover_big", (string)jElement.SelectToken("cover.cloudinary_id")));
                //else
                //    game.Poster = new Uri("/images/NotAvailable.png", UriKind.Relative);
                //if (jElement.SelectToken("screenshots") != null)
                //{
                //    List<string> pictures = jElement.SelectToken("screenshots").Select(s => (string)s.SelectToken("cloudinary_id")).ToList();
                //    foreach (var p in pictures)
                //    {
                //        if (!string.IsNullOrEmpty(p))
                //            game.Pictures.Add(new Uri(string.Format("https://res.cloudinary.com/igdb/image/upload/t_{0}/{1}.jpg", "screenshot_med", p)));
                //        else
                //            game.Pictures.Add(new Uri("/images/NotAvailable.png", UriKind.Relative));
                //    }
                //}

                //if (jElement.SelectToken("videos") != null)
                //{
                //    List<string> videos = jElement.SelectToken("videos").Select(s => (string)s.SelectToken("video_id")).ToList();
                //    foreach (var v in videos)
                //    {
                //        game.Videos.Add(new Uri(string.Format("https://www.youtube.com/embed/{0}", v)));
                //    }
                //}

                ////JArray nestJArray = (JArray)jElement.SelectToken("release_dates");
                ////foreach (var nestElement in nestJArray)
                ////{
                ////    int? category = (int)nestElement.SelectToken("category");
                ////    int? plateform = (int)nestElement.SelectToken("platform");
                ////    long? releaseDate = (long)nestElement.SelectToken("date");
                ////    int? region = (int)nestElement.SelectToken("region");
                ////}
                //// id 7346 name The Legend of Zelda: Breath of the Wild release_dates category 2 platform 41 date 1514674800000 region 8 
                //// We get information from new call
                //// restApi.Parameters = "";

                //var linqForGame = from g in context.GameDbMapping.ToList()
                //                  where g.GameId == game.Id && (user != null && user.Id == g.UserId)
                //                  select g;

                //if (linqForGame.ToList().Count > 0)
                //    game.gameDb = linqForGame.First();
                //else
                //    game.gameDb = new Models.DB.GameDbMapping();
            }
            catch (Exception exc)
            {
                logger.LogError($"Error during mapping: {exc.Message}");
            }

            return movie;
        }
    }
}
