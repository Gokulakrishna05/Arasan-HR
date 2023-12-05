using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services.Master;
using DocumentFormat.OpenXml.Bibliography;
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
            return View();
        }

        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = ItemSubGroupService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListItemSubGroup");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListItemSubGroup");
            }
        } public ActionResult Remove(string tag, int id)
        {

            string flag = ItemSubGroupService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListItemSubGroup");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListItemSubGroup");
            }
        }

        public ActionResult MyListItemgrid(string strStatus)
        {
            List<ItemSubGrid> Reg = new List<ItemSubGrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = ItemSubGroupService.GetAllItemSubGroup(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    EditRow = "<a href=ItemSubGroup?id=" + dtUsers.Rows[i]["ITEMSUBGROUPID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["ITEMSUBGROUPID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                }
                else
                {

                    EditRow = "";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["ITEMSUBGROUPID"].ToString() + "><img src='../Images/close_icon.png' alt='Deactivate' /></a>";

                }

               
                Reg.Add(new ItemSubGrid
                {
                    id = dtUsers.Rows[i]["ITEMSUBGROUPID"].ToString(),
                    itemsubgroup = dtUsers.Rows[i]["SGCODE"].ToString(),
                    descreption = dtUsers.Rows[i]["SGDESC"].ToString(),
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
