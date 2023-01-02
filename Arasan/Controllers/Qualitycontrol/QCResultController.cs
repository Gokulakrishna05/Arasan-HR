using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Interface.Qualitycontrol;
using Arasan.Models;
using Arasan.Services;
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
            ca.Typlst = BindType();
            ca.lst = BindGRNlist();
            ca.assignList = BindEmp();
            //ca.lst = BindGRNlist("");
            if (id == null)
            {

            }
            else
            {

                DataTable dt = new DataTable();

                dt = QCResultService.GetQCResult(id);
                if (dt.Rows.Count > 0)
                {
                    ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.GRNNo = dt.Rows[0]["GRNNO"].ToString();
                    ca.GRNDate = dt.Rows[0]["GRNDATE"].ToString();
                    ca.ID = id;
                    ca.Party = dt.Rows[0]["PARTYID"].ToString();

                }

            }
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
                    return View();
                }

              //}
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
        public JsonResult GetItemJSON(string itemid)
        {
            QCResult model = new QCResult();
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
                dt = QCResultService.GetGRNDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    grn = dt.Rows[0]["DOCID"].ToString();
                    grndate = dt.Rows[0]["DOCDATE"].ToString();
                    party = dt.Rows[0]["PARTY"].ToString();


                }

                var result = new { grn = grn, grndate = grndate, party = party };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetGRNItemJSON(string supid)
        {
            QCResult model = new QCResult();
            model.Itemlst = BindItemlst(supid);
            return Json(BindItemlst(supid));

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
    }
}
