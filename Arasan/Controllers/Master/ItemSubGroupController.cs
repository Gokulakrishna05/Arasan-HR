using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers.Master
{
    public class ItemSubGroupController : Controller
    {
        IItemSubGroupService ItemSubGroupService;
        public ItemSubGroupController(IItemSubGroupService _ItemSubGroupService)
        {
            ItemSubGroupService = _ItemSubGroupService;
        }
        public IActionResult ItemSubGroup(string id)
        {
            ItemSubGroup sg = new ItemSubGroup();
            if (id == null)
            {

            }
            else
            {
                sg = ItemSubGroupService.GetItemSubGroupById(id);

            }
            return View(sg);
        }
        [HttpPost]
        public ActionResult ItemSubGroup(ItemSubGroup sub, string id)
        {

            try
            {
                sub.ID = id;
                string Strout = ItemSubGroupService.ItemSubGroupCRUD(sub);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (sub.ID == null)
                    {
                        TempData["notice"] = " ItemSubGroup Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " ItemSubGroup Updated Successfully...!";
                    }
                    return RedirectToAction("ListItemSubGroup");
                }

                else
                {
                    ViewBag.PageTitle = "Edit ItemSubGroup";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(sub);
        }
        public IActionResult ListItemSubGroup()
        {
            IEnumerable<ItemSubGroup> itg = ItemSubGroupService.GetAllItemSubGroup();
            return View(itg);
        }



    }
}
