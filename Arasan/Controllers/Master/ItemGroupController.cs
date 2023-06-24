using System.Collections.Generic;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services.Master;
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
        public IActionResult ItemGroup(string id)
        {
            ItemGroup ig = new ItemGroup();
            if (id == null)
            {

            }
            else
            {
                ig = itemGroupService.GetItemGroupById(id);

            }
           // return View();
            return View(ig);
        }
        [HttpPost]
        public ActionResult ItemGroup(ItemGroup by, string id)
        {

            try
            {
                by.ID = id;
                string Strout = itemGroupService.ItemGroupCRUD(by);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (by.ID == null)
                    {
                        TempData["notice"] = "ItemGroupInserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "ItemGroup Updated Successfully...!";
                    }
                    return RedirectToAction("ListItemGroup");
                }

                else
                {
                    ViewBag.PageTitle = "Edit ItemGroup";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception it)
            {
                throw it;
            }

            return View(by);
        }
        public IActionResult ListItemGroup(string status)
        {
            IEnumerable<ItemGroup> itg = itemGroupService.GetAllItemGroup(status);
            return View(itg);
        }
        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = itemGroupService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListItemGroup");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListItemGroup");
            }
        }public ActionResult Remove(string tag, int id)
        {

            string flag = itemGroupService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListItemGroup");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListItemGroup");
            }
        }
    }
}