using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApi2.Models;

namespace WebApi2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            HomeServices srv = new HomeServices();

            HomeModels ltdDetail = new HomeModels();

            ltdDetail = srv.ReadContentHtmlPage(ltdDetail);

            return View("Index", ltdDetail);
        }
    }
}
