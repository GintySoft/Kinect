using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetduinoAPI.Controllers
{
    public class DogController : Controller
    {
        //
        // GET: /Dog/

        public ActionResult Index()
        {
            return View();
        }

    }
}
