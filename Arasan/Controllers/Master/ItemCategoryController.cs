using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;

using Microsoft.AspNetCore.Mvc;
using System.Data;

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

            ic.createby = Request.Cookies["UserId"];

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
                    ViewBag.PageTitle = "Edit ItemCategory";
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
            return View();
        }

        public ActionResult DeleteMR(string tag, string id)
        {

            string flag = ItemCategoryService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListItemCategory");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListItemCategory");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = ItemCategoryService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListItemCategory");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListItemCategory");
            }
        }

        public ActionResult MyListItemgrid(string strStatus)
        {
            List<ItemCategoryGrid> Reg = new List<ItemCategoryGrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = ItemCategoryService.GetAllItemCategory(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    EditRow = "<a href=ItemCategory?id=" + dtUsers.Rows[i]["ITEMCATEGORYID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["ITEMCATEGORYID"].ToString() + "";
                }
                else
                {

                    EditRow = "";
                    DeleteRow = "Remove?tag=Del&id=" + dtUsers.Rows[i]["ITEMCATEGORYID"].ToString() + "";
                }
                
                Reg.Add(new ItemCategoryGrid
                {
                    id = dtUsers.Rows[i]["ITEMCATEGORYID"].ToString(),
                    category = dtUsers.Rows[i]["CATEGORY"].ToString(),
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
