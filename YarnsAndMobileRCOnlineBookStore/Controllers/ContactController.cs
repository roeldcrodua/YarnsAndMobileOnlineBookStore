using Microsoft.AspNetCore.Mvc;


namespace YarnsAndMobileRCOnlineBookStore.Controllers
{
    public class ContactController : Controller
    {

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
