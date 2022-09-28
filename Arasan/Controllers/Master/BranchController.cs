



using System.Collections.Generic;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Controllers.Master
{
    public class BranchController : Controller
    {
        IBranchService branchService;
            public BranchController(IBranchService _branchService)
            {
                branchService = _branchService;
            }
            public IActionResult Branch()
        {
            return View();
        }
    }
}
