using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using CollectionManager.Configuration;

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
            return View();
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