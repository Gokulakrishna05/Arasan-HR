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
            ca.lst = BindGRNlist("");
            ca.assignList = BindEmp();
            //ca.lst = BindGRNlist("");
            List<QCItem> TData = new List<QCItem>();
            QCItem tda = new QCItem();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new QCItem();
                 
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }

            }
            else
            {

                DataTable dt = new DataTable();

                dt = QCTestingService.GetQCTesting(id);
                if (dt.Rows.Count > 0)
                {
                    ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.GRNNo = dt.Rows[0]["DOCID"].ToString();
                    ca.GRNDate = dt.Rows[0]["GRNDATE"].ToString();
                    ca.ID = id;
                    ca.Party = dt.Rows[0]["PARTY"].ToString();
                    ca.ItemId = dt.Rows[0]["ITEMID"].ToString();
                    ca.SNo = dt.Rows[0]["SLNO"].ToString();
                    ca.LotNo = dt.Rows[0]["LOTSERIALNO"].ToString();
                    ca.TestResult = dt.Rows[0]["TESTRESULT"].ToString();
                    ca.TestedBy = dt.Rows[0]["TESTEDBY"].ToString();
                    ca.Remarks = dt.Rows[0]["REMARKS"].ToString();
                    ca.ClassCode = dt.Rows[0]["CLASSCODE"].ToString();
                }
                DataTable dt2 = new DataTable();

                dt2 = QCTestingService.GetQCDetail(id);
                if (dt2.Rows.Count > 0)
                {

                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new QCItem();
                        tda.TestDec = dt2.Rows[0]["TESTDESC"].ToString();
                        tda.TestValue = dt2.Rows[0]["TESTVALUE"].ToString();
                        tda.Result = dt2.Rows[0]["RESULT"].ToString();
                        tda.AcTestValue = dt2.Rows[0]["ACTTESTVALUE"].ToString();
                        tda.AccVale = dt2.Rows[0]["ACVAL"].ToString();
                        tda.ManualValue = dt2.Rows[0]["MANUALVALUE"].ToString();
                        TData.Add(tda);
                    } 
                    
                }
               
            }
            ca.QCLst = TData;
            return View(ca);
        }
        [HttpPost]
        public ActionResult QCTesting(QCTesting Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = QCTestingService.QCTestingCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "QCTesting Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "QCTesting Updated Successfully...!";
                    }
                    return RedirectToAction("ListQCTesting");
                }

                else
                {
                    ViewBag.PageTitle = "Edit QCTesting";
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
        public IActionResult ListQCTesting()
        {

            IEnumerable<QCTesting> cmp = QCTestingService.GetAllQCTesting();
            return View(cmp);
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
        public JsonResult GetTypeJSON(string GPID)
        {
            QCTesting model = new QCTesting();
            model.Typlst = BindGRNlist(GPID);
            return Json(BindGRNlist(GPID));

        }

        public List<SelectListItem> BindGRNlist(string value)
        {
            try

            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                if (value == "GRN")
                {
                    DataTable dtDesg = QCTestingService.GetGRN(value);

                    for (int i = 0; i < dtDesg.Rows.Count; i++)
                    {
                        lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["GRNBLBASICID"].ToString() });
                    }
                }

                else
                {
                    DataTable dtDesg = QCTestingService.GetPO(value);

                    for (int i = 0; i < dtDesg.Rows.Count; i++)
                    {
                        lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["POBASICID"].ToString() });
                    }
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
     
        public JsonResult GetItemJSON()
        {
            QCItem model = new QCItem();
            //model.Itemlst = BindItemlst(itemid);
            return Json(model);

        }
        public ActionResult GetGRNDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                string grndate = "";
               
                dt = QCTestingService.GetGRNDetails(ItemId);
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
            QCTesting model = new QCTesting();
            model.Itemlst = BindItemlst(supid);
            return Json(BindItemlst(supid));

        }
        public JsonResult GetGRNSuppJSON(string suppid)
        {
            QCTesting model = new QCTesting();
            model.Supplst = BindSupplst(suppid);
            return Json(BindSupplst(suppid));

        }
        public List<SelectListItem> BindItemlst(string value)
        {
            try
            {

                List<SelectListItem> lstdesg = new List<SelectListItem>();
                //if (value == "GRN")
                //{
                    DataTable dtDesg = QCTestingService.GetItembyId(value);
                    for (int i = 0; i < dtDesg.Rows.Count; i++)
                    {
                        lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["GRNBLDETAILID"].ToString() });

                    }
                //}
                //else
                //{
                //    DataTable dtDesg = QCTestingService.GetPOItembyId(value);
                //    for (int i = 0; i < dtDesg.Rows.Count; i++)
                //    {
                //        lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["PODETAILID"].ToString() });

                //    }
                //}
               
               
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
                DataTable dtDesg = QCTestingService.GetParty(value);
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
