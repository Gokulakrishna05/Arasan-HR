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
        DataTransactions datatrans;
        IConfiguration? _configuratio;
        private string? _connectionString;
        public ItemGroupController(IItemGroupService _itemGroupService, IConfiguration _configuratio)
        {
            itemGroupService = _itemGroupService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult ItemGroup(string id)
        {
            ItemGroup ig = new ItemGroup();
            ig.createby = Request.Cookies["UserName"];
            ig.catlst = BindCategory();

            List<subgrp> TData = new List<subgrp>();
            subgrp tda = new subgrp();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new subgrp();
                    tda.consyn = "N";
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                DataTable dt = new DataTable();
                double total = 0;
                dt = itemGroupService.GetGroup(id);
                if (dt.Rows.Count > 0)
                {
                    
                    ig.ItemGroups = dt.Rows[0]["GROUPCODE"].ToString();
                    ig.ItemGroupDescription = dt.Rows[0]["GROUPDESC"].ToString();
                    ig.Type = dt.Rows[0]["GROUPTYPE"].ToString();
                    ig.ID = id;
                }
               DataTable dt2 = datatrans.GetData("Select SGCODE,SGDESC,SCONSYN,ITEMSUBGROUPID from ITEMSUBGROUP where ITEMGROUPID = '" + id + "'");
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new subgrp();
                        tda.subgrpname = dt2.Rows[i]["SGCODE"].ToString();
                        tda.subgrpdecs = dt2.Rows[i]["SGDESC"].ToString();
                        tda.consyn = dt2.Rows[i]["SCONSYN"].ToString();
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }
            }
            ig.Sublst = TData;
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
        public ActionResult DeleteMR(string tag, string id)
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
        
        public ActionResult Remove(string tag, string id)
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
                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["ITEMGROUPID"].ToString() + "";
                }
                else {

                    EditRow = "";
                    DeleteRow = "Remove?tag=Del&id=" + dtUsers.Rows[i]["ITEMGROUPID"].ToString() + "";

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
        public JsonResult GetItemGrpJSON()
        {
            ItemGroup model = new ItemGroup();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindCategory());
        }
    }
}