using System.Collections.Generic;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
namespace Arasan.Controllers.Master
{
    public class ItemSubGroupController : Controller
    {
        IItemSubGroupService  itemSubGroupService;
        public ItemSubGroupController(IItemSubGroupService _itemSubGroupService)
        {
            itemSubGroupService = _itemSubGroupService;
        }

        public IActionResult ListItemSubGroup()
        {
            return View();
        }
        public IActionResult ItemSubGroup()
        {
            return View();
        }
    }
}