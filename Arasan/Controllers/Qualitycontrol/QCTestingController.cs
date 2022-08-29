using Arasan.Interface;
using System.Collections.Generic;
using Arasan.Models;

using Microsoft.AspNetCore.Mvc;

namespace Arasan.Controllers
{
    public class QCTestingController : Controller
    {

        IQCTestingService QCTestingService;
        public QCTestingController(IQCTestingService _QCTestingService)
        {
            QCTestingService = _QCTestingService;
        }
        public IActionResult QCTesting()
        {
            return View();
        }
        public IActionResult QCResult()
        {
            return View();
        }
        public IActionResult QCTestValueEntry()
        {
            return View();
        }
        public IActionResult QCFinalValueEntry()
        {
            return View();
        }
        public IActionResult PackingQCFinalValueEntry()
        {
            return View();
        }
        public IActionResult ItemConversionEntry()
        {
            return View();
        }
        public IActionResult NCRelease()
        {
            return View();
        }
        public IActionResult ORSATEntry()
        {
            return View();
        }

    }
}
