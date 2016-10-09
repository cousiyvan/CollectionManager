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

        private enum GameElement
        {
            Collection =0,
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
        public IActionResult Games(int? id, int offset = 0)
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

                restApi = new RestAPI(apiKey, gameApiRestUrl, parameters);
                json = restApi.DoCall();
                ApplicationUser user = this.GetConnectedUser();
                games = mapper.Mapping(json, restApi, _gameContext, user);
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

            return view;
        }

        public IActionResult AddRemoveGameCollection(int id)
        {
            ApplicationUser user = this.GetConnectedUser();
            this.AddElement(GameElement.Collection, id, user);
            
            return RedirectToAction(nameof(CollectionController.Games), "Collection", new { offset = ViewData["offset"] });
        }

        public IActionResult AddRemoveGameWishlist(int id)
        {
            ApplicationUser user = this.GetConnectedUser();
            this.AddElement(GameElement.Wishlist, id, user);

            return RedirectToAction(nameof(CollectionController.Games), "Collection", new { offset = ViewData["offset"] });
        }

        public IActionResult AddRemoveGameFavorites(int id)
        {
            ApplicationUser user = this.GetConnectedUser();
            this.AddElement(GameElement.Favorites, id, user);

            return RedirectToAction(nameof(CollectionController.Games), "Collection", new { offset = ViewData["offset"] });
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

        private void AddElement(GameElement gameElement, int id, ApplicationUser user)
        {
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
                        objectExists.ToList()[0].Collection = !objectExists.ToList()[0].Collection;
                    else if (gameElement == GameElement.Favorites)
                        objectExists.ToList()[0].Favorite= !objectExists.ToList()[0].Favorite;
                    else if (gameElement == GameElement.Wishlist)
                        objectExists.ToList()[0].Wishlist = !objectExists.ToList()[0].Wishlist;

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

                    this._gameContext.GameDbMapping.Add(gameDbMapping);
                    this._gameContext.SaveChanges();
                }
            }
        }
        #endregion
    }
}