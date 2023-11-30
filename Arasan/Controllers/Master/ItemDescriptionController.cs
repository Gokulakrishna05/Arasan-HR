using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers.Master
{
    public class ItemDescriptionController : Controller
    {
        IItemDescriptionService ItemDescriptionService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public ItemDescriptionController(IItemDescriptionService _ItemDescriptionService, IConfiguration _configuratio)
        {
            ItemDescriptionService = _ItemDescriptionService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult ItemDescription(string id)
        {
            ItemDescription br = new ItemDescription();
            br.Unitlst = BindUnit();
            if (id != null)
            {
                DataTable dt = new DataTable();
                dt = ItemDescriptionService.GetEditItemDescription(id);
                if (dt.Rows.Count > 0)
                {
                    br.Des = dt.Rows[0]["TESTDESC"].ToString();
                    br.Unit = dt.Rows[0]["UNITID"].ToString();
                    br.Value = dt.Rows[0]["VALUEORMANUAL"].ToString();
                    br.ID = id;
                }
            }
            return View(br);

        }
        [HttpPost]
        public ActionResult ItemDescription(ItemDescription Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = ItemDescriptionService.ItemDescriptionCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Branch ItemDescription Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Branch ItemDescription Successfully...!";
                    }
                    return RedirectToAction("ListItemDescription");
                }

                else
                {
                    ViewBag.PageTitle = "Edit ItemDescription";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Cy);
        }
        public IActionResult ListItemDescription()
        {
            return View();
        }
        //public List<SelectListItem> BindDes()
        //{
        //    try
        //    {
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        lstdesg.Add(new SelectListItem() { Text = "MANUAL", Value = "MANUAL" });
        //        lstdesg.Add(new SelectListItem() { Text = "VALUE", Value = "VALUE" });
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public List<SelectListItem> BindUnit()
        {
            try
            {
                DataTable dtDesg = ItemDescriptionService.GetUnit();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["UNITID"].ToString(), Value = dtDesg.Rows[i]["UNITMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = ItemDescriptionService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListItemDescription");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListItemDescription");
            }
        }

        public ActionResult MyListItemgrid(string strStatus)
        {
            List<ItemDescriptiongrid> Reg = new List<ItemDescriptiongrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = ItemDescriptionService.GetAllItemDescription(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                EditRow = "<a href=ItemDescription?id=" + dtUsers.Rows[i]["TESTDESCMASTERID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["TESTDESCMASTERID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";

                Reg.Add(new ItemDescriptiongrid
                {
                    id = dtUsers.Rows[i]["TESTDESCMASTERID"].ToString(),
                    des = dtUsers.Rows[i]["TESTDESC"].ToString(),
                    unit = dtUsers.Rows[i]["UNITID"].ToString(),
                    value = dtUsers.Rows[i]["VALUEORMANUAL"].ToString(),
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
