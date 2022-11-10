using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Controllers.Master
{
    public class EmployeeController : Controller
    {
        public IActionResult Employee()
        {
            Employee E = new Employee();
            return View(E);
        }
    }
}
