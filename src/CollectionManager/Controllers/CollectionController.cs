using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CollectionManager.Controllers
{
    public class CollectionController : Controller
    {
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