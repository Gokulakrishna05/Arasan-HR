using System.Collections.Generic;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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
                        TempData["notice"] = "ItemGroup Inserted Successfully...!";
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
        public IActionResult ListItemGroup()
        {
            return View();
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

        public ActionResult MyListItemgrid(string strStatus)
        {
            List<ItemGroupGrid> Reg = new List<ItemGroupGrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = itemGroupService.GetAllItemGroup(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                EditRow = "<a href=ItemGroup?id=" + dtUsers.Rows[i]["ITEMGROUPID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["ITEMGROUPID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";

                Reg.Add(new ItemGroupGrid
                {
                    id = dtUsers.Rows[i]["ITEMGROUPID"].ToString(),
                    itemGroup = dtUsers.Rows[i]["GROUPCODE"].ToString(),
                    itemgroupdescription = dtUsers.Rows[i]["GROUPDESC"].ToString(),
                    editrow = EditRow,
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }
    }
}