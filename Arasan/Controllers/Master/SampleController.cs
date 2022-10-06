using Microsoft.AspNetCore.Mvc;

namespace Arasan.Controllers.Master
{
    public class SampleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
