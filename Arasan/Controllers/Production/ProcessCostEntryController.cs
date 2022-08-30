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
        public IActionResult ProcessLostEntry()
        {
            return View();
        }
        public IActionResult ReasonCode()
        {
            return View();
        }
        public IActionResult CuringInward()
        {
            return View();
        }
        public IActionResult CuringOutward()
        {
            return View();
        }
        public IActionResult CuringEStatus()
        {
            return View();
        }
        public IActionResult PackingDrumAllocation()
        {
            return View();
        }
        public IActionResult DrumChange()
        {
            return View();
        }
        public IActionResult MTOOutputCalculator()
        {
            return View();
        }
    }
}
