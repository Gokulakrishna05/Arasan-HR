using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Controllers
{
    public class PurchaseEnqController : Controller
    {
        IPurchaseEnqService PurenqService;
        public PurchaseEnqController(IPurchaseEnqService _PurenqService)
        {
            PurenqService = _PurenqService;
        }
        public IActionResult PurchaseEnquiry()
        {
            return View();
        }

        public IActionResult ListPurchaseEnquiry()
        {
            return View();
        }

        public IActionResult Direct_Purchase()
        {
            return View();
        }

        public IActionResult Purchase_Quotations()
        {
            return View();
        }

        public IActionResult GRN_CUM_BILL()
        {
            return View();
        }
        public IActionResult Purchase_Order()
        {
            return View();
        }
        public IActionResult Purchse_Order_close()
        {
            return View();
        }
        public IActionResult Purchse_Indent()
        {
            return View();
        }

        public IActionResult Purchase_Order_ament()
        {
            return View();
        }
    }
}
