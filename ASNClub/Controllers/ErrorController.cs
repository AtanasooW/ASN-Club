using Microsoft.AspNetCore.Mvc;

namespace ASNClub.Controllers
{
    
    public class ErrorController : Controller
    {
        public IActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
        public IActionResult InternalServerError()
        {
            Response.StatusCode = 500;
            return View();
        }
        public IActionResult BadRequest()
        {
            Response.StatusCode = 400;
            return View();
        }
    }
}
