using Arasan.Interface;
using System.Collections.Generic;
using Arasan.Models;

using Microsoft.AspNetCore.Mvc;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Controllers
{
    public class QCTestingController : Controller
    {
        IQCTestingService QCTestingService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public QCTestingController(IQCTestingService _QCTestingService, IConfiguration _configuratio)
        {
            QCTestingService = _QCTestingService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult QCTesting(string id)
        {
            QCTesting ca = new QCTesting();
            ca.Typlst = BindType();
            ca.lst = BindGRNlist();
         
            //ca.lst = BindGRNlist("");
            if (id == null)
            {

            }
            else
            {
              

            }
            return View(ca);
        }
        public IActionResult ListQCTesting()
        {
            return View();
        }
        public List<SelectListItem> BindType()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "PO", Value = "PO" });
                lstdesg.Add(new SelectListItem() { Text = "GRN", Value = "GRN" });

                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindGRNlist()
        {
            try
            {
                DataTable dtDesg = QCTestingService.GetGRN();
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
        //public List<SelectListItem> BindGRNlist(string value)
        //{
        //    try
        //    {
        //        DataTable dtDesg = QCTestingService.GetGRN(value);
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        for (int i = 0; i < dtDesg.Rows.Count; i++)
        //        {
        //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["POBASICID"].ToString() });
        //        }
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public JsonResult GetItemJSON(string itemid)
        {
            QCTesting model = new QCTesting();
            //model.Itemlst = BindItemlst(itemid);
            return Json(model);

        }
        public ActionResult GetGRNDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string grn = "";
                string grndate = "";
                string party = "";
                dt = QCTestingService.GetGRNDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    grn = dt.Rows[0]["UNITID"].ToString();
                    grndate = dt.Rows[0]["LATPURPRICE"].ToString();
                    party = dt.Rows[0]["LATPURPRICE"].ToString();


                }

                var result = new { grn = grn, grndate = grndate, party = party };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult QCResult()
        {
            return View();
        }
        public IActionResult QCTestValueEntry()
        {
            return View();
        }
        public IActionResult QCFinalValueEntry()
        {
            return View();
        }
        public IActionResult PackingQCFinalValueEntry()
        {
            return View();
        }
        public IActionResult ItemConversionEntry()
        {
            return View();
        }
        public IActionResult NCRelease()
        {
            return View();
        }
        public IActionResult ORSATEntry()
        {
            return View();
        }
    }
}
