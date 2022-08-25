using System.Collections.Generic;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;


namespace Arasan.Controllers.Master
{
    public class ItemGroupController : Controller
    {
        IItemGroupService itemGroupService;
        public ItemGroupController(IItemGroupService _itemGroupService)
        {
           itemGroupService = _itemGroupService;
        }

        public IActionResult ItemGroup()
        {
            return View();
        }
    }
}
