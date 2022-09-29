using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;

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
        public IActionResult ItemCategory(string id)
        {
            ItemCategory ic = new ItemCategory();
            if (id == null)
            {

            }
            else
            {
                ic = ItemCategoryService.GetCategoryById(id);

            }
            return View(ic);
        }
        [HttpPost]
        public ActionResult ItemCategory(ItemCategory Ic, string id)
        {

            try
            {
                Ic.ID = id;
                string Strout = ItemCategoryService.CategoryCRUD(Ic);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Ic.ID == null)
                    {
                        TempData["notice"] = "Category Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Category Updated Successfully...!";
                    }
                    return RedirectToAction("ListItemCategory");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Category";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Ic);
        }
      
        public IActionResult ListItemCategory()
        {
            IEnumerable<ItemCategory> ic = ItemCategoryService.GetAllItemCategory();
            return View(ic);
        }
    }
}
