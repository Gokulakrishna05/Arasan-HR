using System.Collections.Generic;
using Arasan.Interface.Production;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
namespace Arasan.Controllers.Production
{
    public class ProcessCostEntryController : Controller
    {
        IProcessCostEntryService  ProcessCostEntryService;
        public ProcessCostEntryController(IProcessCostEntryService _ProcessCostEntryService)
        {
            ProcessCostEntryService = _ProcessCostEntryService;
        }
        public IActionResult ProcessCostEntry()
        {
            return View();
        }
        public IActionResult BatchCreation()
        {
            return View();
        }
    }
}
