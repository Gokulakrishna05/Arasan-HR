using System.Collections.Generic;
using System.Security.Cryptography.Pkcs;
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
        public IActionResult Work_Order_Amedment()
        {
            return View();
        }
        public IActionResult Work_Orde_ShortClose()
        {
            return View();
        }
        public IActionResult Sales_Return()
        {
            return View();
        }
        public IActionResult Debit_Note_Bill()
        {
            return View();
        }
        public IActionResult Credit_Note_Bill()
        {
            return View();
        }
        public IActionResult Credit_Note_Approval()
        {
            return View();
        }
        public IActionResult Sales_Forecasting()
        {
            return View();
        }
    }
}
