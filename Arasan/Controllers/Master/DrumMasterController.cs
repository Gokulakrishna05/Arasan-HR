﻿using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Master;
//using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class DrumMasterController : Controller
    {
        IDrumMaster DrumMasterService;

        IConfiguration? _configuration;
        private string? _connectionString;

        DataTransactions datatrans;

        public DrumMasterController(IDrumMaster _DrumMasterService, IConfiguration _configuration)
        {
            DrumMasterService = _DrumMasterService;
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        //For form dropdowns
        public IActionResult DrumMaster(string id)
        {
            DrumMaster DM = new DrumMaster();
            DM.createby = Request.Cookies["UserId"];
            DM.Categorylst = BindCategory();
            DM.Locationlst = BindLocation();
            DM.DrumTypelst = BindDrumType();

            //for edit & delete
            if (id != null)
            {
                DataTable dt = new DataTable();
                double total = 0;
                dt = DrumMasterService.GetDrumMaster(id);
                if (dt.Rows.Count > 0)
                {
                    DM.DrumNo = dt.Rows[0]["DRUMNO"].ToString();
                    DM.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                    DM.Category = dt.Rows[0]["CATEGORY"].ToString();
                    DM.Location = dt.Rows[0]["LOCATION"].ToString();
                    DM.ID = id;
                    DM.DrumType = dt.Rows[0]["DRUMTYPE"].ToString();
                    DM.TargetWeight = dt.Rows[0]["TAREWT"].ToString();
                }
            }
            return View(DM);
        }

        public List<SelectListItem> BindCategory()
        {
            try
            {
                DataTable dtDesg = DrumMasterService.GetCategory();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["COLUMN1"].ToString(), Value = dtDesg.Rows[i]["COLUMN1"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<SelectListItem> BindLocation()
        {
            try
            {
                // for access the data form datatrans page

                DataTable dtDesg = new DataTable();
                dtDesg = datatrans.GetLocation();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOCID"].ToString(), Value = dtDesg.Rows[i]["LOCDETAILSID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindDrumType()
        {
            try
            {
                DataTable dtDesg = DrumMasterService.GetDrumType();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["COLUMN1"].ToString(), Value = dtDesg.Rows[i]["COLUMN1"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost] // for same name

        //For procedure_db query
        public IActionResult DrumMaster(DrumMaster DM, string id)
        {

            try
            {
                DM.ID = id;
                string Strout = DrumMasterService.DrumMasterCRUD(DM);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (DM.ID == null)
                    {
                        TempData["notice"] = "DrumMaster Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "DrumMaster Updated Successfully...!";
                    }
                    return RedirectToAction("ListDrumMaster");
                }

                else
                {
                    ViewBag.PageTitle = "Edit DrumMaster";
                    TempData["notice"] = Strout;
                    //return View();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(DM);
        }

        public IActionResult ListDrumMaster()
        {
            return View();
        }

        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = DrumMasterService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListDrumMaster");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListDrumMaster");
            }
        } 
        public ActionResult Remove(string tag, int id)
        {

            string flag = DrumMasterService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListDrumMaster");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListDrumMaster");
            }
        }

        public ActionResult MyListItemgrid(string strStatus)
        {
            List<DrumMastergrid> Reg = new List<DrumMastergrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = DrumMasterService.GetAllDrummast(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    EditRow = "<a href=DrumMaster?id=" + dtUsers.Rows[i]["DRUMMASTID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["DRUMMASTID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                }
                else
                {

                    EditRow = "";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["DRUMMASTID"].ToString() + "><img src='../Images/close_icon.png' alt='Deactivate' /></a>";

                }

               
                Reg.Add(new DrumMastergrid
                {
                    id = dtUsers.Rows[i]["DRUMMASTID"].ToString(),
                    drumnno = dtUsers.Rows[i]["DRUMNO"].ToString(),
                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    category = dtUsers.Rows[i]["CATEGORY"].ToString(),
                    location = dtUsers.Rows[i]["LOCID"].ToString(),
                    drumtype = dtUsers.Rows[i]["DRUMTYPE"].ToString(),
                    targetweight = dtUsers.Rows[i]["TAREWT"].ToString(),
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