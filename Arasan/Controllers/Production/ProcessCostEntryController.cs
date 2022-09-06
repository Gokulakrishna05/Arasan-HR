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
        public IActionResult PackingNote()
        {
            return View();
        }
        public IActionResult DrumIssueEntry()
        {
            return View();
        }
        public IActionResult DrumStockReconcilation()
        {
            return View();
        }
        public IActionResult MaterialSplittingEntry()
        {
            return View();
        }
        public IActionResult FiredDrums()
        {
            return View();
        }
        public IActionResult FiredDrumsRelease()
        {
            return View();
        }
        public IActionResult ProductionForecasting()
        {
            return View();
        }
        public IActionResult PackingIssueEntry()
        {
            return View();
        }
        public IActionResult DrumLoadEntry()
        {
            return View();
        }
        public IActionResult ProductionEntry()
        {
            return View();
        }
        public IActionResult BatchProductionEntry()
        {
            return View();
        }
        public IActionResult MonthlyProductionEntry()
        {
            return View();
        }
         public IActionResult ProductionLog()
        {
            return View();
        }
        public IActionResult DrumChangeUnPackingEntry()
        {
            return View();
        }
        public IActionResult ProductionSchedule()
        {
            return View();
        }
        public IActionResult PackingEntry()
        {
            return View();
        }
    }
}
