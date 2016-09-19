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

namespace CollectionManager.Controllers
{
    public class CollectionController : Controller
    {
        #region Members
        private readonly AppSettings _appSettings;
        #endregion

        #region Constructors
        public CollectionController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
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
        public IActionResult Games()
        {
            Uri gameApiRestUrl = null;
            string parameters = string.Empty;
            Dictionary<string, string> apiKey = new Dictionary<string, string>();
            apiKey.Add("X-Mashape-Key", "MtG6phh7A5mshHpRiU1ooJK3glr9p1S1VsejsnX9JomArwQspE");
            RestAPI restApi = null;
            JsonReader json;
            string restOutput = string.Empty;
            MapperGame mapper = new MapperGame();
            Game game = new Game();

            if (Uri.TryCreate("https://igdbcom-internet-game-database-v1.p.mashape.com/", UriKind.Absolute, out gameApiRestUrl))
            {
                parameters = "games/?fields=*&limit=10&offset=0&order=release_dates.date%3Adesc&search=zelda"; ;
                restApi = new RestAPI(apiKey, gameApiRestUrl, parameters);
                json = restApi.DoCall();

                while (json.Read())
                {
                    if (json.Value != null)
                    {
                        ViewBag.Information += json.Value + " ";
                    }
                }

                json = restApi.DoCall();
                mapper.Mapping(json, ref game, restApi);
            }
            return View(game);
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