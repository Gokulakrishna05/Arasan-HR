using System.Collections.Generic;
using Arasan.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Models;
using System.Xml.Linq;

namespace Arasan.Controllers
{
    public class StoreIssueProductionController : Controller
    {
        IStoreIssueProduction StoreIssue ;
        public StoreIssueProductionController(IStoreIssueProduction _StoreIssue)
        {
            StoreIssue = _StoreIssue;
        }
        public IActionResult StoreIssuePro()
        {
            return View();
        }
        public IActionResult ListStoreIssuePro()
        {
            return View();
        }
    }
}
