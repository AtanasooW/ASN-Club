using Microsoft.AspNetCore.Mvc;

namespace ASNClub.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult All()
        {
            return View();
        }
    }
}
