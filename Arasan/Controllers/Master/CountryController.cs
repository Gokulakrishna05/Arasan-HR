using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Controllers.Master
{
    public class CountryController : Controller
    {

        ICountryService CountryService;
        public CountryController(ICountryService _CountryService)
        {
            CountryService = _CountryService;
        }
        public IActionResult Country()
        {
            return View();
        }
    }
}
