using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Controllers.Master
{
    public class ItemCategoryController : Controller
    {
        IItemCategoryService ItemCategoryService;
        public ItemCategoryController(IItemCategoryService _ItemCategoryService)
        {
            ItemCategoryService = _ItemCategoryService;
        }
        public IActionResult ItemCategory()
        {
            return View();
        }
    }
}
