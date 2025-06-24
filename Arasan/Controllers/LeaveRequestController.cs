using Microsoft.AspNetCore.Mvc;

namespace Arasan.Controllers
{
    public class LeaveRequestController : Controller
    {
        public IActionResult LeaveRequest()
        {
            return View();
        }

        public IActionResult ListLeaveRequest()
        {
            return View();
        }
    }
}
