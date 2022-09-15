using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;


namespace Arasan.Controllers.Master
{
    public class CurrencyController : Controller
    {
        ICurrencyService CurrencyService;
        public CurrencyController(ICurrencyService _CurrencyService)
        {
            CurrencyService = _CurrencyService;
        }
        public IActionResult Currency()
        {
            return View();
        }
        public IActionResult ListCurrency()
        {
            return View();
        }
    }
}
