using Microsoft.AspNetCore.Mvc;

namespace Store.ECommerce.Controllers
{
    public class HomeController :
        Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}