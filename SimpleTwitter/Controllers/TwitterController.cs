using System;
using System.Web.Mvc;

namespace SimpleTwitter.Controllers
{
    public class TwitterController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}