using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class UnitController : Controller
    {
        IUnitService UnitService;
        IConfiguration? _configuration;
        private string? _connectionString;
        public UnitController(IUnitService _UnitService, IConfiguration _configuration)
        {
            UnitService = _UnitService;
          
        }
        public IActionResult Unit(string id)
        {
            Unit ca = new Unit();
            ca.createby = Request.Cookies["UserId"];
            if (id != null)
            {
                DataTable dt = new DataTable();
                double total = 0;
                dt = UnitService.GetUnit(id);
                if (dt.Rows.Count > 0)
                {
                    ca.UnitName = dt.Rows[0]["UNITID"].ToString();
                   
                }
            }
            return View(ca);
        }
        [HttpPost]
        public ActionResult Unit(Unit Cy, string id)
        {
           try
            {
                Cy.ID = id;
                string Strout = UnitService.UnitCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Unit Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Unit Updated Successfully...!";
                    }
                    return RedirectToAction("ListUnit");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Unit";
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
        public IActionResult ListUnit()
        {
            return View();
        }
        public ActionResult DeleteMR(string tag, string id)
        {

            string flag = UnitService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListUnit");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListUnit");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = UnitService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListUnit");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListUnit");
            }
        }

        public ActionResult MyListItemgrid(string strStatus)
        {
            List<Unitgrid> Reg = new List<Unitgrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = UnitService.GetAllUnit(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    EditRow = "<a href=Unit?id=" + dtUsers.Rows[i]["UNITMASTID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["UNITMASTID"].ToString() + "";
                }
                else
                {

                    EditRow = "";
                    DeleteRow = "Remove?tag=Del&id=" + dtUsers.Rows[i]["UNITMASTID"].ToString() + "";
                }
               
                Reg.Add(new Unitgrid
                {
                    id = dtUsers.Rows[i]["UNITMASTID"].ToString(),
                    unitname = dtUsers.Rows[i]["UNITID"].ToString(),
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
