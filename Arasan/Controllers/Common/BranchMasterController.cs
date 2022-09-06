using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Controllers.Common
{
    public class BranchMasterController : Controller
    {
        public IActionResult Branch_Master()
        {
            return View();
        }
    }
}
