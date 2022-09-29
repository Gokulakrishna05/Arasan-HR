using Arasan.Models;
using System.Collections.Generic;
using System.Collections; 
using Arasan.Interface.Master;

using Microsoft.AspNetCore.Mvc;

namespace Arasan.Controllers.Master
{
    public class HSNcodeController : Controller
    {
        IHSNcodeService HSNcodeService;
        public HSNcodeController(IHSNcodeService _HSNcodeService)
        {
            HSNcodeService = _HSNcodeService;
        }
        public IActionResult HSNcode() 
        {
            return View();
        }
        public IActionResult ListHSNcode()
        {
            return View();
        }
    }
}
        