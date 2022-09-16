using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using Arasan.Interface.Master;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Controllers.Master
{
    public class PartyMasterController : Controller
    {

        IPartyMasterService PartyMasterService;
        public PartyMasterController(IPartyMasterService _PartyMasterService)
        {
            PartyMasterService = _PartyMasterService;
        }
        public IActionResult PartyMaster()
        {
            return View();
        }
    }
}
