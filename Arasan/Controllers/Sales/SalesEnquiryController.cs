using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
namespace Arasan.Controllers
{
    public class SalesEnquiryController : Controller
    {
        ISalesEnq SalesenqService;
        public SalesEnquiryController(ISalesEnq _SalesenqService)
        {
            SalesenqService = _SalesenqService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
