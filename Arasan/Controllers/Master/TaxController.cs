using System.Collections.Generic;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;


namespace Arasan.Controllers.Master
{
    public class TaxController : Controller
    {
        ITaxService taxService;
        public TaxController(ITaxService _taxService)
        {
            taxService = _taxService;
        }

        public IActionResult Tax()
        {
            return View();
        }
        public IActionResult ListTax()
        {
            return View();
        }
    }
}
