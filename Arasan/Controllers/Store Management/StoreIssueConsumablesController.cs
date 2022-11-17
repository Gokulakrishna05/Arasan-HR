using System.Collections.Generic;
using Arasan.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Models;
using System.Xml.Linq;

namespace Arasan.Controllers
{
    public class StoreIssueConsumablesController : Controller
    {
        IStoreIssueConsumables StoreIssService;
        public StoreIssueConsumablesController(IStoreIssueConsumables _StoreIssService)
        {
            StoreIssService = _StoreIssService;
        }
        public IActionResult StoreIssueCons()
        {
            return View();
        }
        public IActionResult ListStoreIssueCons()
        {
            return View();
        }
    }
}
