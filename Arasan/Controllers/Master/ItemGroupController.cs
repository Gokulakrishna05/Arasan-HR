using System.Collections.Generic;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            ig.createby = Request.Cookies["UserId"];
            ig.catlst = BindCategory();
            if (id != null)
            {
                DataTable dt = new DataTable();
                double total = 0;
                dt = itemGroupService.GetGroup(id);
                if (dt.Rows.Count > 0)
                {
                    ig.ItemCat = dt.Rows[0]["CATEGORY"].ToString();
                    ig.ItemGroups = dt.Rows[0]["GROUPCODE"].ToString();
                    ig.ItemGroupDescription = dt.Rows[0]["GROUPDESC"].ToString();
                    ig.Type = dt.Rows[0]["GROUPTYPE"].ToString();
                }
            }
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

        public List<SelectListItem> BindCategory()
        {
            try
            {
                DataTable dtDesg = itemGroupService.GetCategory();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["CATEGORY"].ToString(), Value = dtDesg.Rows[i]["ITEMCATEGORYID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }

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
        }
        
        public ActionResult Remove(string tag, int id)
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

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    EditRow = "<a href=ItemGroup?id=" + dtUsers.Rows[i]["ITEMGROUPID"].ToString() + "><img src='../Images/edit.png' alt='Edit'/></a>";
                    DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["ITEMGROUPID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                }
                else {

                    EditRow = "";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["ITEMGROUPID"].ToString() + "><img src='../Images/close_icon.png' alt='Deactivate' /></a>";

                }

                Reg.Add(new ItemGroupGrid
                {
                    id = dtUsers.Rows[i]["ITEMGROUPID"].ToString(),
                    itemgroup = dtUsers.Rows[i]["GROUPCODE"].ToString(),
                    itemcat = dtUsers.Rows[i]["CATEGORY"].ToString(),
                    itemgroupdescription = dtUsers.Rows[i]["GROUPDESC"].ToString(),
                    type = dtUsers.Rows[i]["GROUPTYPE"].ToString(),
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