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
        public IActionResult Sales_Enquiry()
        {
            return View();
        }
        public IActionResult Sales_Quotation()
        {
            return View();
        }
        public IActionResult Proforma_Invoice()
        {
            return View();
        }
        public IActionResult Excise_Invoice()
        {
            return View();
        }
        public IActionResult Supplimantry_Invoice()
        {
            return View();
        }
        public IActionResult Depot_Invoice()
        {
            return View();
        }

        public IActionResult Work_Order()
        {
            return View();
        }
    }
}
