﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Arasan.Interface.Production;
using Arasan.Models;
using System.Data;
using System.Security.Cryptography.Pkcs;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers.Production
{
    public class ProductionScheduleController : Controller
    {

        IProductionScheduleService ProductionScheduleService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public ProductionScheduleController(IProductionScheduleService _ProductionScheduleService, IConfiguration _configuratio)
        {
            ProductionScheduleService = _ProductionScheduleService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult ProductionSchedule(string id)
        {
            ProductionSchedule ca = new ProductionSchedule();
            ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Worklst = BindWorkCenter();
            ca.Processlst = BindProcess("");
            ca.Enterd = Request.Cookies["UserId"];
            ca.RecList = BindEmp();
            List<ProductionScheduleItem> TData = new List<ProductionScheduleItem>();
            ProductionScheduleItem tda = new ProductionScheduleItem();
            //List<ProductionItem> TData1 = new List<ProductionItem>();
            //ProductionItem tda1 = new ProductionItem();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new ProductionScheduleItem();
                    tda.ItemGrouplst = BindItemGrplst();
                    tda.Itemlst = BindItemlst("");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
                //for (int i = 0; i < 3; i++)
                //{
                //    tda1 = new ProductionItem();

                //    tda.PItemGrouplst = BindItemGrplstid();
                //    tda.PItemlst = BindItemlstid("");
                //    tda.Isvalid = "Y";
                //    TData.Add(tda);
                //}
            }
            else
            {
                //ca = QCResultService.GetQCResultById(id);

                DataTable dt = new DataTable();
                dt = ProductionScheduleService.GetProductionSchedule(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    ca.Type = dt.Rows[0]["SCHPLANTYPE"].ToString();
                    ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.WorkCenter = dt.Rows[0]["WCID"].ToString();
                    ca.Process = dt.Rows[0]["PROCESSID"].ToString();
                    ca.ID = id;
                    ca.Schdate = dt.Rows[0]["SCHDATE"].ToString();
                    ca.Formula = dt.Rows[0]["FORMULA"].ToString();
                    ca.Proddt = dt.Rows[0]["PDOCDT"].ToString();
                    ca.Itemid = dt.Rows[0]["OPITEMID"].ToString();
                    ca.Unit = dt.Rows[0]["OPUNIT"].ToString();
                    ca.Exprunhrs = dt.Rows[0]["EXPRUNHRS"].ToString();
                    ca.Refno = dt.Rows[0]["REFSCHNO"].ToString();
                    ca.Amdno = dt.Rows[0]["AMDSCHNO"].ToString();
                    ca.Entered = dt.Rows[0]["ENTEREDBY"].ToString();
                    
                }
                DataTable dt2 = new DataTable();
                dt2 = ProductionScheduleService.GetProductionScheduleDetail(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new ProductionScheduleItem();
                        tda.ItemGrouplst = BindItemGrplst();
                        DataTable dt3 = new DataTable();
                        dt3 = ProductionScheduleService.GetItemSubGroup(dt2.Rows[i]["ITEMID"].ToString());
                        if (dt3.Rows.Count > 0)
                        {
                            tda.ItemGroupId = dt3.Rows[0]["SUBGROUPCODE"].ToString();
                        }
                        tda.Itemlst = BindItemlst(tda.ItemGroupId);
                        tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();
                        DataTable dt4 = new DataTable();
                        dt4 = ProductionScheduleService.GetItemDetails(tda.ItemId);
                        if (dt4.Rows.Count > 0)
                        {
                            tda.Desc = dt4.Rows[0]["ITEMDESC"].ToString();
                        }
                        tda.Unit = dt2.Rows[i]["UNITID"].ToString();
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }

            }
            ca.PrsLst = TData;
            return View(ca);
        }
        [HttpPost]
        public ActionResult ProductionSchedule(ProductionSchedule Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = ProductionScheduleService.ProductionScheduleCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "ProductionSchedule Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "ProductionSchedule Updated Successfully...!";
                    }
                    return RedirectToAction("ListProductionSchedule");

                }
                else
                {
                    ViewBag.PageTitle = "Edit ProductionSchedule";
                    TempData["notice"] = Strout;
                    //return View();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Cy);
        }

       
        public IActionResult ListProductionSchedule()
        {
            IEnumerable<ProductionSchedule> cmp = ProductionScheduleService.GetProductionSchedule();
            return View();
        }
        //public List<SelectListItem> BindItemlstid(string value)
        //{
        //    try
        //    {
        //        DataTable dtDesg = ProductionScheduleService.GetItem(value);
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        for (int i = 0; i < dtDesg.Rows.Count; i++)
        //        {
        //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["ITEMMASTERID"].ToString() });
        //        }
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public List<SelectListItem> BindItemGrplstid()
        //{
        //    try
        //    {
        //        DataTable dtDesg = ProductionScheduleService.GetItemSubGrp();
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        for (int i = 0; i < dtDesg.Rows.Count; i++)
        //        {
        //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SGCODE"].ToString(), Value = dtDesg.Rows[i]["ITEMSUBGROUPID"].ToString() });
        //        }
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public List<SelectListItem> BindItemlst(string value)
        {
            try
            {
                DataTable dtDesg = ProductionScheduleService.GetItem(value);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["ITEMMASTERID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindItemGrplst()
        {
            try
            {
                DataTable dtDesg = ProductionScheduleService.GetItemSubGrp();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SGCODE"].ToString(), Value = dtDesg.Rows[i]["ITEMSUBGROUPID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dtDesg = datatrans.GetBranch();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["BRANCHID"].ToString(), Value = dtDesg.Rows[i]["BRANCHMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindEmp()
        {
            try
            {
                DataTable dtDesg = datatrans.GetEmp();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPNAME"].ToString(), Value = dtDesg.Rows[i]["EMPNAME"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindWorkCenter()
        {
            try
            {
                DataTable dtDesg = ProductionScheduleService.GetWorkCenter();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["WCID"].ToString(), Value = dtDesg.Rows[i]["WCBASICID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindProcess(string id)
        {
            try
            {
                DataTable dtDesg = ProductionScheduleService.GetProcess(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PROCESSID"].ToString(), Value = dtDesg.Rows[i]["WCBASICID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                string unit = "";
                string Desc = "";
                dt = datatrans.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    unit = dt.Rows[0]["UNITID"].ToString();
                    Desc = dt.Rows[0]["ITEMDESC"].ToString();
                }

                var result = new { unit = unit, Desc = Desc};
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetItemGrpJSON()
        {
            //ProductionScheduleItem model = new ProductionScheduleItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItemGrplst());
        }
        public JsonResult GetItemJSON(string itemid)
        {
            ProductionScheduleItem model = new ProductionScheduleItem();
            model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst(itemid));

        }
    }
}