using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;


namespace Arasan.Controllers.Stores_Management
{
    public class StoresReturnController : Controller
    {
        public IActionResult StoresReturn()
        {
            return View();
        }
        public IActionResult Returnable_NonReturnable_Dc()
        {
            return View();
        }
        public IActionResult Stores_Acceptance()
        {
            return View();
        }
        public IActionResult Receipt_Against_Returnable_DC()
        {
            return View();
        }
        public IActionResult Stores_Issuse_Consumbables()
        {
            return View();
        }
        public IActionResult Purchase_Indent()
        {
            return View();
        }
        public IActionResult Stores_Issuse_Production()
        {
            return View();
        }
    }
}
