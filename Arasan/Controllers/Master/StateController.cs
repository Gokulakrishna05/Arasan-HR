using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Controllers.Master
{
    public class StateController : Controller
    {
        IStateService StateService;
        public StateController(IStateService _StateService)
        {
            StateService = _StateService;
        }
        public IActionResult State()
        {
            return View();
        }
    }
}
