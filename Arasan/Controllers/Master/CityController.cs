using System.Collections.Generic;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Controllers.Master
{
    public class CityController : Controller
    {
        ICityService cityService;
        public CityController(ICityService _cityService)
        {
            cityService = _cityService;
        }
        public IActionResult City()
        {
            return View();
        }
    }
}
