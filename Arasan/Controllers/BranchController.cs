using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Controllers
{
    public class BranchController : Controller
    {
        IBranchService branchService;
        public BranchController(IBranchService _branchService)
        {
            branchService = _branchService;
        }
        public ActionResult Index()
        {
            IEnumerable<Branch> branch = branchService.GetAllBranch();
            return View(branch);
        }
    }
}
