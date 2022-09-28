using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Controllers
{
    public class CompanyController : Controller
    {
        ICompanyService CompanyService;
        public CompanyController (ICompanyService _CompanyService)
        {
            CompanyService = _CompanyService;
        }
        public IActionResult Company()
        {
            return View();
        }
    }
}



