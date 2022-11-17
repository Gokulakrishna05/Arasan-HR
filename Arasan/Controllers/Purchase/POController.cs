using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Arasan.Controllers
{
    public class POController : Controller
    {
        public IActionResult ListPO()
        {
            return View();
        }
    }
}
