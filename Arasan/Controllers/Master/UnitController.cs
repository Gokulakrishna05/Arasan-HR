using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;


namespace Arasan.Controllers.Master
{
    public class UnitController : Controller
    {
        IUnitService UnitService;
        public UnitController(IUnitService _UnitService)
        {
            UnitService = _UnitService;
        }
        public IActionResult Unit()
        {
            return View();
        }
        public IActionResult ListUnit()
        {
            return View();
        }

    }
}
