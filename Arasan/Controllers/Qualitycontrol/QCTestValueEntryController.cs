using System.Collections.Generic;
using Arasan.Interface.Master;
using Arasan.Interface.Qualitycontrol;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;


namespace Arasan.Controllers.Qualitycontrol
{
    public class QCTestValueEntryController : Controller
    {
        IQCTestValueEntryService QCTestValueEntryService;
        public QCTestValueEntryController(IQCTestValueEntryService _QCTestValueEntryService)
        {
            QCTestValueEntryService = _QCTestValueEntryService;
        }
        public IActionResult QCTestValueEntry()
        {
            return View();
        }
        public IActionResult ListQCTestValueEntry()
        {
            return View();
        }
    }
}
