﻿using System.Collections.Generic;
using System.Data;
using System.Runtime.Intrinsics.Arm;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Master;
//using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers.Master
{

    public class DesignationController : Controller
    {
        IDesignation DesignationService;
        public DesignationController(IDesignation _DesignationService)
        {
            DesignationService = _DesignationService;
        }
        public IActionResult Designation(string id)
        {
            Designation Dp = new Designation();
            Dp.createby = Request.Cookies["UserId"];

            Dp.DeptNamelst = BindDeptName();
            if (id == null)
            {
                
            }
            else
            {
                DataTable dt = new DataTable();
                double total = 0;
                dt = DesignationService.GetDesignation(id);
                if (dt.Rows.Count > 0)
                {
                    
                    Dp.Design = dt.Rows[0]["DESIGNATION"].ToString();
                    Dp.DeptName = dt.Rows[0]["DEPARTMENT_NAME"].ToString();

                }

            }
            return View(Dp);
        }
        public IActionResult PDesig(string id)
        {
            Designation Dp = new Designation();
            Dp.createby = Request.Cookies["UserId"];

            Dp.DeptNamelst = BindPDeptName();
            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();
                double total = 0;
                dt = DesignationService.GetPDesignation(id);
                if (dt.Rows.Count > 0)
                {

                    Dp.Design = dt.Rows[0]["DESIGNATION"].ToString();
                    Dp.DeptName = dt.Rows[0]["PDEPTID"].ToString();
                    Dp.Pos = dt.Rows[0]["POSITION"].ToString();
                }

            }
            return View(Dp);
        }

        [HttpPost]
        public IActionResult Designation(Designation Dp, string id)
        {
            try
            {
                Dp.ID = id;
                string Strout = DesignationService.DesignationCRUD(Dp);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Dp.ID == null)
                    {
                        TempData["notice"] = "Designation Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Designation Updated Successfully...!";
                    }
                    return RedirectToAction("ListDesignation");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Designation";
                    TempData["notice"] = Strout;
                   
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }



            return View(Dp);
        }
        [HttpPost]
        public IActionResult PDesig(Designation Dp, string id)
        {
            try
            {
                Dp.ID = id;
                string Strout = DesignationService.PDesignationCRUD(Dp);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Dp.ID == null)
                    {
                        TempData["notice"] = "Designation Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Designation Updated Successfully...!";
                    }
                    return RedirectToAction("ListPDesg");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Designation";
                    TempData["notice"] = Strout;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Dp);
        }
        public List<SelectListItem> BindDeptName()
        {
            try
            {
                DataTable dtDesg = DesignationService.GetDeptName();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DEPARTMENT_NAME"].ToString(), Value = dtDesg.Rows[i]["DEPARTMENTMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindPDeptName()
        {
            try
            {
                DataTable dtDesg = DesignationService.GetPDeptName();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DEPARTMENT"].ToString(), Value = dtDesg.Rows[i]["PDEPTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult ListDesignation()
        {
            return View();
        }
        public IActionResult ListPDesg()
        {
            return View();
        }

        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = DesignationService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListDesignation");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListDesignation");
            }
        }   public ActionResult Remove(string tag, int id)
        {

            string flag = DesignationService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListDesignation");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListDesignation");
            }
        }

        public ActionResult MyListItemgrid(string strStatus)
        {
            List<DesignationGrid> Reg = new List<DesignationGrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = DesignationService.GetAllDESIGNATION(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    EditRow = "<a href=Designation?id=" + dtUsers.Rows[i]["DESIGNATIONMASTID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["DESIGNATIONMASTID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                }
                else
                {

                    EditRow = "";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["DESIGNATIONMASTID"].ToString() + "><img src='../Images/close_icon.png' alt='Deactivate' /></a>";

                }
                
                Reg.Add(new DesignationGrid
                {
                    id = dtUsers.Rows[i]["DESIGNATIONMASTID"].ToString(),
                    design = dtUsers.Rows[i]["DESIGNATION"].ToString(),
                    deptname = dtUsers.Rows[i]["DEPARTMENT_NAME"].ToString(),
                    editrow = EditRow,
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }
        public ActionResult MyListPdesig(string strStatus)
        {
            List<DesignationGrid> Reg = new List<DesignationGrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = DesignationService.GetAllPDESIGNATION(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    EditRow = "<a href=PDesig?id=" + dtUsers.Rows[i]["PDESGID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "<a href=DeletePDesig?tag=Del&id=" + dtUsers.Rows[i]["PDESGID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                }
                else
                {

                    EditRow = "";
                    DeleteRow = "<a href=DeletePDesig?tag=Del&id=" + dtUsers.Rows[i]["PDESGID"].ToString() + "><img src='../Images/close_icon.png' alt='Deactivate' /></a>";

                }

                Reg.Add(new DesignationGrid
                {
                    id = dtUsers.Rows[i]["PDESGID"].ToString(),
                    design = dtUsers.Rows[i]["DESIGNATION"].ToString(),
                    deptname = dtUsers.Rows[i]["DEPARTMENT"].ToString(),
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
