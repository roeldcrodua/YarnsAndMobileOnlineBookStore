using Microsoft.AspNetCore.Mvc;


namespace YarnsAndMobileRCOnlineBookStore.Controllers
{
    public class ContactController : Controller
    {

        [Route("Contact")]
        public IActionResult Contact()
        {
            return View();
        }
    }
}
