using System.Collections.Generic;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Controllers.Master
{
    public class ExchangeRateController : Controller
    {

        IExchangeRateService exchangerateService;
        public ExchangeRateController(IExchangeRateService _exchangerateService)
        {
            exchangerateService = _exchangerateService;
        }
        public IActionResult ExchangeRate()
        {
            return View();
        }
        public IActionResult ListExchangeRate()
        {
            return View();
        }
    }
}
