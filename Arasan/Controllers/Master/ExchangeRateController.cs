using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class ExchangeRateController : Controller
    {

        IExchangeRateService exchangerateService;
        IConfiguration? _configuration;
        private string? _connectionString;
        public ExchangeRateController(IExchangeRateService _exchangerateService, IConfiguration _configuration)
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
