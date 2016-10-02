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
        public IActionResult Games(int? id)
        {
            Uri gameApiRestUrl = null;
            string parameters = string.Empty;
            Dictionary<string, string> apiKey = new Dictionary<string, string>();
            apiKey.Add("X-Mashape-Key", _appSettings.ApiKey.Game);
            RestAPI restApi = null;
            JsonReader json;
            string restOutput = string.Empty;
            MapperGame mapper = new MapperGame();
            List<Game> games = null;

            if (Uri.TryCreate(_appSettings.ServicesSettings.Game, UriKind.Absolute, out gameApiRestUrl))
            {
                if (id != null)
                {
                    parameters = $"games/{id}?fields=*&limit=10";
                }
                else
                {
                    parameters = "games/?fields=*&limit=10&offset=0&order=release_dates.date%3Adesc&search=zelda";
                }
                restApi = new RestAPI(apiKey, gameApiRestUrl, parameters);
                //json = restApi.DoCall();

                //while (json.Read())
                //{
                //    if (json.Value != null)
                //    {
                //        ViewBag.Information += json.Value + " ";
                //    }
                //}

                json = restApi.DoCall();
                ApplicationUser user = null;
                if (User.Identity.Name != null)
                {
                    try
                    {
                        user = _userManager.FindByNameAsync(User.Identity.Name).Result;
                    }
                    catch (Exception exc)
                    {
                        _logger.LogError("Error during retrieval of user");
                        user = null;
                    }
                }
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
    }
}