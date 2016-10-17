using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using CollectionManager.Configuration;
using CollectionManager.Utils;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using CollectionManager.Models.Collection;
using CollectionManager.Data;
using Microsoft.AspNetCore.Identity;
using CollectionManager.Models;
using Microsoft.Extensions.Logging;
using CollectionManager.Models.DB;

namespace CollectionManager.Controllers
{
    public class CollectionController : Controller
    {
        #region Members
        private readonly AppSettings _appSettings;
        private readonly GameContext _gameContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;
        private readonly int MaxElements = 4;
        private readonly int MaxPages = 5;
        private readonly int SummaryMaxCharacters = 150;
        private readonly string AlertSuccessClass = "alert alert-success";
        private readonly string AlertWarningClass = "alert alert-warning";
        private readonly string AlertInfoClass = "alert alert-info";
        private readonly string AlertErrorClass = "alert alert-danger";

        private enum GameElement
        {
            Collection = 0,
            Wishlist,
            Favorites
        }
        #endregion

        #region Constructors
        public CollectionController(
            IOptions<AppSettings> appSettings,
            GameContext gameContext,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILoggerFactory loggerFactory)
        {
            _appSettings = appSettings.Value;
            _gameContext = gameContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }
        #endregion

        /// <summary>
        /// Controller to the index view
        /// </summary>
        /// <returns>The view to be displayed</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Controller to the Games view
        /// </summary>
        /// <returns>The view to be displayed</returns>
        public IActionResult Games(int? id, int offset = 0, int currentPage = 0)
        {
            // init variables
            Uri gameApiRestUrl = null;
            string parameters = string.Empty;
            Dictionary<string, string> apiKey = new Dictionary<string, string>();
            apiKey.Add("X-Mashape-Key", _appSettings.ApiKey.Game);
            RestAPI restApi = null;
            JsonReader json;
            string restOutput = string.Empty;
            MapperGame mapper = new MapperGame();
            List<Game> games = null;

            // init some ViewData values
            ViewData["MaxPages"] = this.MaxPages;
            ViewData["offset"] = offset;
            ViewData["CurrentPage"] = currentPage;
            ViewData["MaxElements"] = this.MaxElements;
            ViewData["SummaryMaxCharacters"] = this.SummaryMaxCharacters;

            if (Uri.TryCreate(_appSettings.ServicesSettings.Game, UriKind.Absolute, out gameApiRestUrl))
            {
                restApi = new RestAPI(apiKey, gameApiRestUrl, parameters);

                if (id != null)
                {
                    parameters = $"games/{id}?fields=*&limit={this.MaxElements}&offset={offset}";
                }
                else
                {
                    JToken count = restApi.GetSpecificValue($"games/count", "count");
                    ViewData["count"] = count.Value<int>();
                    parameters = $"games/?fields=*&limit={MaxElements}&offset={offset}&order=release_dates.date%3Adesc";
                }
                //json = restApi.DoCall();

                //while (json.Read())
                //{
                //    if (json.Value != null)
                //    {
                //        ViewBag.Information += json.Value + " ";
                //    }
                //}

                try
                {
                    restApi = new RestAPI(apiKey, gameApiRestUrl, parameters);
                    json = restApi.DoCall();
                    ApplicationUser user = this.GetConnectedUser();
                    games = mapper.Mapping(json, restApi, _gameContext, user, _logger);

                    // We get user data if he is connected
                    if (user != null)
                    {
                        var linqUserGames = from g in _gameContext.GameDbMapping.ToList()
                                            where user.Id == g.UserId
                                            select g;
                        string collectionId = string.Empty, wishlistId = string.Empty, favoritesId = string.Empty;
                        foreach (var userGame in linqUserGames)
                        {
                            collectionId += (userGame.Collection) ? $"{userGame.GameId.ToString()}," : string.Empty;
                            wishlistId += (userGame.Wishlist) ? $"{userGame.GameId.ToString()}," : string.Empty;
                            favoritesId += (userGame.Favorite) ? $"{userGame.GameId.ToString()}," : string.Empty;
                        }

                        if (!string.IsNullOrEmpty(collectionId))
                        {
                            restApi.Parameters = $"games/{collectionId.Substring(0, collectionId.Length - 1)}?fields=*&limit={MaxElements}&offset={offset}&order=release_dates.date%3Adesc";
                            json = restApi.DoCall();
                            ViewData["MyCollectionGames"] = mapper.Mapping(json, restApi, _gameContext, user, _logger);
                        }

                        if (!string.IsNullOrEmpty(wishlistId))
                        {
                            restApi.Parameters = $"games/{wishlistId.Substring(0, wishlistId.Length - 1)}?fields=*&limit={MaxElements}&offset={offset}&order=release_dates.date%3Adesc";
                            json = restApi.DoCall();
                            ViewData["MyWishlistGames"] = mapper.Mapping(json, restApi, _gameContext, user, _logger);
                        }

                        if (!string.IsNullOrEmpty(favoritesId))
                        {
                            restApi.Parameters = $"games/{favoritesId.Substring(0, favoritesId.Length - 1)}?fields=*&limit={MaxElements}&offset={offset}&order=release_dates.date%3Adesc";
                            json = restApi.DoCall();
                            ViewData["MyFavoritesGames"] = mapper.Mapping(json, restApi, _gameContext, user, _logger);
                        }
                    }
                }
                catch (Exception exc)
                {
                    _logger.LogError($"Error during call: {exc.Message}");
                    ViewData["AlertMessage"] = $"Error during process: {exc.Message}";
                    ViewData["AlertClass"] = this.AlertErrorClass;
                    games = null;
                }
            }
            if (games == null)
                games = new List<Game>();

            ViewResult view = null;
            if (games.Count > 1)
            {
                view = View(games);
            }
            else if (games.Count == 1)
            {
                view = View("~/Views/Collection/GameDetails.cshtml", games[0]);
            }
            else
            {
                view = new ViewResult();
            }

            if (!string.IsNullOrEmpty((string)TempData["AlertMessage"]))
            {
                ViewData["AlertMessage"] = (string)TempData["AlertMessage"];
                ViewData["AlertClass"] = (string)TempData["AlertClass"];

                TempData["AlertMessage"] = null;
                TempData["AlertClass"] = null;
            }

            return view;
        }

        public IActionResult AddRemoveGameCollection(int id, int offset = 0, int currentPage = 0)
        {
            ApplicationUser user = this.GetConnectedUser();
            bool added = this.AddElement(GameElement.Collection, id, user);

            string resultString = added ? "added" : "removed";
            TempData["AlertMessage"] = $"Game {resultString} to collection";
            TempData["AlertClass"] = this.AlertSuccessClass;

            return RedirectToAction(nameof(CollectionController.Games), "Collection", new { offset = offset, currentPage = currentPage });
        }

        public IActionResult AddRemoveGameWishlist(int id, int offset = 0, int currentPage = 0)
        {
            ApplicationUser user = this.GetConnectedUser();
            bool added =this.AddElement(GameElement.Wishlist, id, user);

            string resultString = added ? "added" : "removed";
            TempData["AlertMessage"] = $"Game {resultString} to wishlist";
            TempData["AlertClass"] = this.AlertSuccessClass;

            return RedirectToAction(nameof(CollectionController.Games), "Collection", new { offset = offset, currentPage = currentPage });
        }

        public IActionResult AddRemoveGameFavorites(int id, int offset = 0, int currentPage = 0)
        {
            ApplicationUser user = this.GetConnectedUser();
            bool added = this.AddElement(GameElement.Favorites, id, user);

            string resultString = added ? "added" : "removed";
            TempData["AlertMessage"] = $"Game {resultString} to favorites";
            TempData["AlertClass"] = this.AlertSuccessClass;

            return RedirectToAction(nameof(CollectionController.Games), "Collection", new { offset = offset, currentPage = currentPage });
        }

        /// <summary>
        /// Controller to the Books view
        /// </summary>
        /// <returns>The view to be displayed</returns>
        public IActionResult Books()
        {
            return View();
        }

        /// <summary>
        /// Controller to the Musc view
        /// </summary>
        /// <returns>The view to be displayed</returns>
        public IActionResult Music()
        {
            return View();
        }

        /// <summary>
        /// Controller to the Series view
        /// </summary>
        /// <returns>The view to be displayed</returns>
        public IActionResult Series()
        {
            return View();
        }

        /// <summary>
        /// Controller to the Movies view
        /// </summary>
        /// <returns>The view to be displayed</returns>
        public IActionResult Movies()
        {
            return View();
        }

        #region Utils
        private ApplicationUser GetConnectedUser()
        {
            ApplicationUser user = null;
            if (User.Identity.Name != null)
            {
                try
                {
                    user = _userManager.FindByNameAsync(User.Identity.Name).Result;
                }
                catch (Exception exc)
                {
                    _logger.LogError("Error during retrieval of user:" + exc.InnerException);
                    user = null;
                }
            }

            return user;
        }

        private bool AddElement(GameElement gameElement, int id, ApplicationUser user)
        {
            bool added = false;
            if (id == 0)
                new ViewResult();
            else
            {
                var objectExists = from gameDb in _gameContext.GameDbMapping
                                   where gameDb.GameId == id
                                   where gameDb.UserId == user.Id
                                   select gameDb;

                if (objectExists.ToList().Count > 0)
                {
                    if (gameElement == GameElement.Collection)
                    {
                        objectExists.ToList()[0].Collection = !objectExists.ToList()[0].Collection;
                        added = objectExists.ToList()[0].Collection;
                    }
                    else if (gameElement == GameElement.Favorites)
                    {
                        objectExists.ToList()[0].Favorite = !objectExists.ToList()[0].Favorite;
                        added = objectExists.ToList()[0].Favorite;
                    }
                    else if (gameElement == GameElement.Wishlist)
                    {
                        objectExists.ToList()[0].Wishlist = !objectExists.ToList()[0].Wishlist;
                        added = objectExists.ToList()[0].Wishlist;
                    }
                    this._gameContext.SaveChanges();
                }
                else
                {
                    GameDbMapping gameDbMapping = new GameDbMapping();
                    if (gameElement == GameElement.Collection)
                        gameDbMapping = new GameDbMapping
                        {
                            Collection = true,
                            Favorite = false,
                            Wishlist = false,
                            GameId = id,
                            UserId = user.Id
                        };
                    else if (gameElement == GameElement.Favorites)
                        gameDbMapping = new GameDbMapping
                        {
                            Collection = false,
                            Favorite = true,
                            Wishlist = false,
                            GameId = id,
                            UserId = user.Id
                        };
                    else if (gameElement == GameElement.Wishlist)
                        gameDbMapping = new GameDbMapping
                        {
                            Collection = false,
                            Favorite = false,
                            Wishlist = true,
                            GameId = id,
                            UserId = user.Id
                        };
                    added = true;

                    this._gameContext.GameDbMapping.Add(gameDbMapping);
                    this._gameContext.SaveChanges();
                }
            }
            return added;
        }
        #endregion
    }
}