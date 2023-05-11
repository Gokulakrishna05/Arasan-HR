using System.Collections.Generic;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services.Master;
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
        public IActionResult Company(string id)
        {
            Company ca = new Company();
            if (id == null)
            {

            }
            else
            {
                //ca = TaxService.GetTaxById(id);

            }
            return View(ca);
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
