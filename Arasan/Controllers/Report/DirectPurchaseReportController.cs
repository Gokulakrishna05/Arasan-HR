﻿using Microsoft.AspNetCore.Mvc;
using Arasan.Interface;
using Arasan.Interface.Report;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Report;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Controllers.Report
{
    public class DirectPurchaseReportController : Controller
    {
        IDirectPurchaseReportService DirectPurchaseReportService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public DirectPurchaseReportController(IDirectPurchaseReportService _DirectPurchaseReportService, IConfiguration _configuratio)
        {
            DirectPurchaseReportService = _DirectPurchaseReportService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult DirectPurchaseReport(string strfrom, string strTo)
        {
            try
            {
                DirectPurchaseReport objR = new DirectPurchaseReport();
                objR.Brlst = BindBranch();
                objR.Suplst = BindSupplier();
                objR.ItemGrouplst = BindItemGrplst();
                objR.Itemlst = BindItemlst("");
                objR.dtFrom = strfrom;
                objR.dtTo = strTo;
                return View(objR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public IActionResult ListDirectPurchaseReport()
        {
            return View();
        }
        public List<SelectListItem> BindItemlst(string id)
        {
            try
            {
                DataTable dtDesg = DirectPurchaseReportService.GetItem(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["ITEMID"].ToString() });
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

        public List<SelectListItem> BindSupplier()
        {
            try
            {
                DataTable dtDesg = datatrans.GetSupplier();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTYNAME"].ToString(), Value = dtDesg.Rows[i]["PARTYNAME"].ToString() });
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
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["BRANCHID"].ToString(), Value = dtDesg.Rows[i]["BRANCHID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetItemJSON(string itemid)
        {
            DirectPurchaseReport model = new DirectPurchaseReport();
            model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst(itemid));

        }
        public ActionResult MyListDirectPurchaseReportGrid(string dtFrom, string dtTo, string Branch, string Item, string Customer)
        {
            List<DirectPurchaseReportItems> Reg = new List<DirectPurchaseReportItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)DirectPurchaseReportService.GetAllDPReport(dtFrom, dtTo, Branch, Item, Customer);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                Reg.Add(new DirectPurchaseReportItems
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["DPBASICID"].ToString()),
                    branch = dtUsers.Rows[i]["BRANCHID"].ToString(),
                    docNo = dtUsers.Rows[i]["DOCID"].ToString(),
                    docDate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    party = dtUsers.Rows[i]["PARTYID"].ToString(),
                    item = dtUsers.Rows[i]["ITEMID"].ToString(),
                    loc = dtUsers.Rows[i]["LOCID"].ToString(),
                    unit = dtUsers.Rows[i]["UNITID"].ToString(),
                    qty = dtUsers.Rows[i]["QTY"].ToString(),
                    rate = dtUsers.Rows[i]["RATE"].ToString(),
                    amount = dtUsers.Rows[i]["AMOUNT"].ToString(),





                });
            }

            return Json(new
            {
                Reg
            });

        }
    }
}