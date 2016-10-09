﻿using CollectionManager.Data;
using CollectionManager.Models;
using CollectionManager.Models.Collection;
using Microsoft.AspNetCore.Identity;
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

        public List<Game> Mapping(JsonReader json, RestAPI restApi, GameContext context, ApplicationUser user)
        {
            List<Game> games = new List<Game>();

            // From json object we get the values to fill our object
            JArray jArray = JArray.Load(json);
            foreach (var jElement in jArray)
            {
                Game game = new Game();
                // Values from first call
                game.Id = (int)jElement.SelectToken("id");
                game.Title = (string)jElement.SelectToken("name");
                game.OriginalTitle = game.Title;
                game.Description = (string)jElement.SelectToken("summary");
                game.Storyline = (string)jElement.SelectToken("storyline");

                List<string> developers = new List<string>();
                JToken jToken = jElement.SelectToken("developers");
                if (jToken != null)
                {
                    developers = jElement.SelectToken("developers").Select(s => (string)s).ToList<string>();
                    game.Developers.AddRange(Utils.GetContent(restApi, Game.CallType.Companie, developers));
                }

                List<string> publishers = new List<string>();
                jToken = jElement.SelectToken("publishers");
                if (jToken != null)
                {
                    publishers = jElement.SelectToken("publishers").Select(s => (string)s).ToList<string>();
                    game.Publishers.AddRange(Utils.GetContent(restApi, Game.CallType.Companie, publishers));
                }

                int category = (int)jElement.SelectToken("category");

                List<string> gameModes = new List<string>();
                jToken = jElement.SelectToken("game_modes");
                if (jToken != null)
                {
                    gameModes = jElement.SelectToken("game_modes").Select(s => (string)s).ToList<string>();
                    game.GameModes.AddRange(Utils.GetContent(restApi, Game.CallType.GameModes, gameModes));
                }

                List<string> keywords = new List<string>();
                jToken = jElement.SelectToken("keywords");
                if (jToken != null)
                {
                    keywords = jElement.SelectToken("keywords").Select(s => (string)s).ToList<string>();
                    game.Keywords.AddRange(Utils.GetContent(restApi, Game.CallType.Keyword, keywords));
                }

                List<string> themes = new List<string>();
                jToken = jElement.SelectToken("themes");
                if (jToken != null)
                {
                    themes = jElement.SelectToken("themes").Select(s => (string)s).ToList<string>();
                    game.Themes.AddRange(Utils.GetContent(restApi, Game.CallType.Theme, themes));
                }

                List<string> genres = new List<string>();
                jToken = jElement.SelectToken("genres");
                if (jToken != null)
                {
                    genres = jElement.SelectToken("genres").Select(s => (string)s).ToList<string>();
                    game.Genres.AddRange(Utils.GetContent(restApi, Game.CallType.Genres, genres));
                }

                foreach (var token in jElement.SelectTokens("release_dates"))
                {
                    string platform = (string)token.Children()["platform"].ToList()[0];
                    platform = Utils.GetContent(restApi, Game.CallType.Platform, new List<string>() { platform })[0];
                    game.ReleaseDate.Add(platform, Utils.UnixTimeStampToDateTime((double)token.Children()["date"].ToList()[0]));
                }
                
                game.Poster = new Uri(string.Format("https://res.cloudinary.com/igdb/image/upload/t_{0}/{1}.jpg", "cover_big", (string)jElement.SelectToken("cover.cloudinary_id")));
                if (jElement.SelectToken("screenshots") != null)
                {
                    List<string> pictures = jElement.SelectToken("screenshots").Select(s => (string)s.SelectToken("cloudinary_id")).ToList();
                    foreach (var p in pictures)
                    {
                        game.Pictures.Add(new Uri(string.Format("https://res.cloudinary.com/igdb/image/upload/t_{0}/{1}.jpg", "screenshot_med", p)));
                    }
                }

                if (jElement.SelectToken("videos") != null)
                {
                    List<string> videos = jElement.SelectToken("videos").Select(s => (string)s.SelectToken("video_id")).ToList();
                    foreach (var v in videos)
                    {
                        game.Videos.Add(new Uri(string.Format("https://www.youtube.com/embed/{0}", v)));
                    }
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

                var linqForGame = from g in context.GameDbMapping.ToList()
                                  where g.GameId == game.Id && (user != null && user.Id == g.UserId)
                                  select g;

                if (linqForGame.ToList().Count > 0)
                    game.gameDb = linqForGame.First();
                else
                    game.gameDb = new Models.DB.GameDbMapping();

                games.Add(game);
            }

            return games;
        }
    }
}
