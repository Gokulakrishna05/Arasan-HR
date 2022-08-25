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
    }
}
