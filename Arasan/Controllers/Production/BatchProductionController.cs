﻿using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Interface.Stores_Management;
using Arasan.Models;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class BatchProductionController : Controller
    {
        IBatchProduction IProductionEntry;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public BatchProductionController(IBatchProduction _IProductionEntry, IConfiguration _configuratio)
        {
            IProductionEntry = _IProductionEntry;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult BatchProduction()
        {
            ProductionEntry ca = new ProductionEntry();
            ca.Brlst = BindBranch();
            ca.Loclst = BindLocation();
            ca.Location = Request.Cookies["LocationId"];
            ca.Branch = Request.Cookies["BranchId"];
            ca.Shiftlst = BindShift();
            ca.Processlst = BindProcess();
            ca.ETypelst = BindEType();

            List<ProIn> TData = new List<ProIn>();
            ProIn tda = new ProIn();
            for (int i = 0; i < 1; i++)
            {
                tda = new ProIn();
                // tda.ItemGrouplst = BindItemGrplst();
                tda.Itemlst = BindItemlst("");
                tda.drumlst = Binddrum();
                tda.outputlst = Bindoutput();
                tda.Isvalid = "Y";
                TData.Add(tda);
            }

            List<ProInCons> TData1 = new List<ProInCons>();
            ProInCons tda1 = new ProInCons();
            for (int i = 0; i < 1; i++)
            {
                tda1 = new ProInCons();
                tda1.Itemlst = BindItemlst("");
                tda1.Isvalid = "Y";
                TData1.Add(tda1);
            }

            List<output> TData2 = new List<output>();
            output tda2 = new output();
            for (int i = 0; i < 1; i++)
            {
                tda2 = new output();
                tda2.Itemlst = BindItemlst("");
                tda2.drumlst = Binddrum();
                tda2.statuslst = BindStatus();
                tda2.loclst = BindLocation();
                tda2.Isvalid = "Y";
                TData2.Add(tda2);
            }

            List<wastage> TData3 = new List<wastage>();
            wastage tda3 = new wastage();
            for (int i = 0; i < 1; i++)
            {
                tda3 = new wastage();
                tda3.Itemlst = BindItemlst("");
                tda3.loclst = BindLocation();
                tda3.Isvalid = "Y";
                TData3.Add(tda3);
            }

            ca.inputlst = TData;
            ca.inconslst = TData1;
            ca.outlst = TData2;
            ca.wastelst = TData3;
            return View(ca);
        }

        public JsonResult GetItemJSON(string itemid)
        {
            EnqItem model = new EnqItem();
            model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst(itemid));
        }
        public JsonResult GetsearchItemJSON(string prefix)
        {
            return Json(BindSItemlst(prefix));
        }
        public JsonResult GetItemGrpJSON()
        {
            //EnqItem model = new EnqItem();
            //model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItemGrplst());
        }
        public JsonResult GetDrumJSON()
        {
            return Json(Binddrum());
        }

        public List<SelectListItem> BindItemlst(string value)
        {
            try
            {
                DataTable dtDesg = datatrans.GetItem(value);
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
        public List<SelectListItem> BindSItemlst(string term)
        {
            try
            {
                DataTable dtDesg = IProductionEntry.SeacrhItem(term);
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
                DataTable dtDesg = datatrans.GetItemSubGrp();
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
        public List<SelectListItem> Binddrum()
        {
            try
            {
                DataTable dtDesg = IProductionEntry.DrumDeatils();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DRUMNO"].ToString(), Value = dtDesg.Rows[i]["DRUMMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindShift()
        {
            try
            {
                DataTable dtDesg = IProductionEntry.ShiftDeatils();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SHIFTNO"].ToString(), Value = dtDesg.Rows[i]["SHIFTMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindProcess()
        {
            try
            {
                DataTable dtDesg = IProductionEntry.BindProcess();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PROCESSID"].ToString(), Value = dtDesg.Rows[i]["PROCESSMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindEType()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "BOTH", Value = "BOTH" });
                lstdesg.Add(new SelectListItem() { Text = "INPUT", Value = "INPUT" });
                lstdesg.Add(new SelectListItem() { Text = "OUTPUT", Value = "OUTPUT" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindStatus()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "PENDING", Value = "PENDING" });
                lstdesg.Add(new SelectListItem() { Text = "COMPLETED", Value = "COMPLETED" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> Bindoutput()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "YES", Value = "YES" });
                lstdesg.Add(new SelectListItem() { Text = "NO", Value = "NO" });
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
        public ActionResult GetStkqty(string branch, string loc, string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                string stkqty = "0";
                dt = IProductionEntry.Getstkqty(branch, loc, ItemId);
                if (dt.Rows.Count > 0)
                {
                    stkqty = dt.Rows[0]["QTY"].ToString();
                }
                if (stkqty == "")
                {
                    stkqty = "0";
                }
                var result = new { stkqty = stkqty };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetshiftDetail(string Shiftid)
        {
            try
            {
                DataTable dt = new DataTable();
                string fromtime = "";
                string totime = "";
                string tothrs = "";
                dt = datatrans.GetData("Select FROMTIME,TOTIME,SHIFTHRS from SHIFTMAST where SHIFTMASTID='" + Shiftid + "'");
                if (dt.Rows.Count > 0)
                {

                    fromtime = dt.Rows[0]["FROMTIME"].ToString();
                    totime = dt.Rows[0]["TOTIME"].ToString();
                    tothrs = dt.Rows[0]["SHIFTHRS"].ToString();
                }

                var result = new { fromtime = fromtime, totime = totime, tothrs = tothrs };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}