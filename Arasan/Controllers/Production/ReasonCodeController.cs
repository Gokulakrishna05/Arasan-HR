using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Interface.Production;

namespace Arasan.Controllers.Production
{

    public class ReasonCodeController : Controller
    {
        IReasonCodeService ReasonCodeService;
        public ReasonCodeController(IReasonCodeService _ReasonCodeService)
        {
            ReasonCodeService = _ReasonCodeService;
        }
        public IActionResult ReasonCode()
        {
            return View();
        }
        public IActionResult ListReasonCode()
        {
            return View();
        }
    }
}
