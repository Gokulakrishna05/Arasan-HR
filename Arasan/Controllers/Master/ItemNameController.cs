using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Controllers.Master
{
    public class ItemNameController : Controller
    {

        IItemNameService ItemNameService;
        public ItemNameController(IItemNameService _ItemNameService)
        {
            ItemNameService = _ItemNameService;
        }
        public IActionResult ItemName()
        {
            return View();
        }
        public IActionResult ItemList()
        {
            return View();
        }

    }
}
