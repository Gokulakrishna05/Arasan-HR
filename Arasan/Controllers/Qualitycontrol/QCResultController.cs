﻿using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Interface.Qualitycontrol;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Store_Management;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers.Qualitycontrol
{
    public class QCResultController : Controller
    {
        IQCResultService QCResultService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public QCResultController(IQCResultService _QCResultService, IConfiguration _configuratio)
        {
            QCResultService = _QCResultService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult QCResult(string id)
        {
            QCResult ca = new QCResult();
            ca.lst = BindGRNlist();
            ca.assignList = BindEmp();
            ca.Loc = BindLocation();
            List<QCResultItem> TData = new List<QCResultItem>();
            QCResultItem tda = new QCResultItem();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new QCResultItem();
                    tda.Itemlst = BindItemlst("");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                //ca = QCResultService.GetQCResultById(id);

                DataTable dt = new DataTable();

                dt = QCResultService.GetQCResult(id);
                if (dt.Rows.Count > 0)
                {
                    ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.GRNNo = dt.Rows[0]["GRNNO"].ToString();
                    ca.GRNDate = dt.Rows[0]["GRNDATE"].ToString();          
                    ca.ID = id;
                    ca.Party = dt.Rows[0]["PARTY"].ToString();
                    ca.Location = dt.Rows[0]["LOCATION"].ToString();
                    ca.Remarks = dt.Rows[0]["REMARKS"].ToString();
                    ca.QcLocation = dt.Rows[0]["QCLOCATION"].ToString();
                    ca.TestedBy = dt.Rows[0]["TESTEDBY"].ToString();
                }
                DataTable dt2 = new DataTable();
                dt2 = QCResultService.GetQCResultDetail(id);
                if (dt2.Rows.Count > 0)
                {

                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new QCResultItem();
                        tda.ItemId = dt2.Rows[0]["ITEMID"].ToString();
                        tda.Itemlst = BindItemlst(ca.GRNNo);
                        tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.GrnQty = dt2.Rows[0]["GRNQTY"].ToString();
                        //tda.InsQty = dt2.Rows[0]["INSQTY"].ToString();
                        tda.RejQty = dt2.Rows[0]["REJQTY"].ToString();
                        tda.AccQty = dt2.Rows[0]["ACCQTY"].ToString();
                        tda.CostRate = dt2.Rows[0]["COSTRATE"].ToString();
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }

                }

            }
            ca.QCRLst = TData;
            return View(ca);
        }
        [HttpPost]
        public ActionResult QCResult(QCResult Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = QCResultService.QCResultCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "QCResult Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "QCResult Updated Successfully...!";
                    }
                    return RedirectToAction("ListQCResult");

                }
                else
                {
                    ViewBag.PageTitle = "Edit QCResult";
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
        public IActionResult ListQCResult()
        {

            IEnumerable<QCResult> cmp = QCResultService.GetAllQCResult();
            return View(cmp);
        }
        public List<SelectListItem> BindGRNlist()
        {
            try
            {
                DataTable dtDesg = QCResultService.GetGRN();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                   
                        lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["GRNBLBASICID"].ToString() });
                 
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetItemJSON()
        {
            QCResult model = new QCResult();
            //model.Itemlst = BindItemlst(itemid);
            return Json(model);

        }
        //public List<SelectListItem> BindType()
        //{
        //    try
        //    {
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        lstdesg.Add(new SelectListItem() { Text = "GRN", Value = "GRN" });
        //        lstdesg.Add(new SelectListItem() { Text = "PO", Value = "PO" });


        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public ActionResult GetGRNDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                string grndate = "";

                dt = QCResultService.GetGRNDetails(ItemId);
                if (dt.Rows.Count > 0)
                {
                    grndate = dt.Rows[0]["DOCDATE"].ToString();

                }

                var result = new { grndate = grndate };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetGRNItemJSON(string supid)
        {
            QCResultItem model = new QCResultItem();
            model.Itemlst = BindItemlst(supid);
            return Json(BindItemlst(supid));

        }
        public JsonResult GetGRNSuppJSON(string suppid)
        {
            QCResult model = new QCResult();
            model.Supplst = BindSupplst(suppid);
            return Json(BindSupplst(suppid));

        }
        public List<SelectListItem> BindItemlst(string value)
        {
            try
            {
                DataTable dtDesg = QCResultService.GetItembyId(value);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["GRNBLDETAILID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindSupplst(string value)
        {
            try
            {
                DataTable dtDesg = QCResultService.GetParty(value);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTY"].ToString(), Value = dtDesg.Rows[i]["GRNBLBASICID"].ToString() });
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
                DataTable dtDesg = datatrans.GetLocation();
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
        public List<SelectListItem> BindEmp()
        {
            try
            {
                DataTable dtDesg = datatrans.GetEmp();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPNAME"].ToString(), Value = dtDesg.Rows[i]["EMPMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetGRNItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
            

             
                string qty = "";
                string accqty = "";

                string rejqty = "";
                string cost = "";
                //string Disc = "";



                dt = QCResultService.GetGRNItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                   
                    qty = dt.Rows[0]["QTY"].ToString();
                    accqty = dt.Rows[0]["ACCQTY"].ToString();
                    rejqty = dt.Rows[0]["REJQTY"].ToString();
                    //dt1 = PurReturn.GetItemCF(ItemId, dt.Rows[0]["UNITMASTID"].ToString());
                    cost = dt.Rows[0]["COSTRATE"].ToString();


                  

                }

                var result = new {  qty = qty, accqty = accqty, rejqty = rejqty, cost = cost };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}