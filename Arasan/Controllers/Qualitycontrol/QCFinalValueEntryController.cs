using System.Collections.Generic;
using Arasan.Interface.Master;
using Arasan.Interface.Qualitycontrol;
using Arasan.Models;
using System.Security.Cryptography.Pkcs;
using System.Xml.Linq;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Controllers.Qualitycontrol
{
    public class QCFinalValueEntryController : Controller
    {
        IQCFinalValueEntryService QCFinalValueEntryService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public QCFinalValueEntryController(IQCFinalValueEntryService _QCFinalValueEntryService, IConfiguration _configuratio)
        {
            QCFinalValueEntryService = _QCFinalValueEntryService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult QCFinalValueEntry(string id)
        {
            QCFinalValueEntry ca = new QCFinalValueEntry();
            ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Worklst = BindWorkCenter();
            ca.Enterd = Request.Cookies["UserId"];
            ca.RecList = BindEmp();
            ca.Processlst = BindProcess("");
            ca.drumlst = Binddrum();
            ca.Itemlst = BindItemlst("");
            ca.Batchlst = BindBatch("");
            List<QCFVItem> TData = new List<QCFVItem>();
            QCFVItem tda = new QCFVItem();
            List<QCFVItemDeatils> TData1 = new List<QCFVItemDeatils>();
            QCFVItemDeatils tda1 = new QCFVItemDeatils();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new QCFVItem();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
                for (int i = 0; i < 3; i++)
                {
                    tda1 = new QCFVItemDeatils();
                    tda1.Isvalid = "Y";
                    TData1.Add(tda1);
                }

            }
            else
            {
                DataTable dt = new DataTable();
                dt = QCFinalValueEntryService.GetQCFVDeatil(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Branch = dt.Rows[0]["BRANCH"].ToString();
                    ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.WorkCenter = dt.Rows[0]["WCID"].ToString();
                    ca.Process = dt.Rows[0]["PROCESSID"].ToString();
                    ca.ID = id;
                    ca.DrumNo = dt.Rows[0]["DRUMNO"].ToString();
                    ca.Batch = dt.Rows[0]["BATCH"].ToString();
                    ca.BatchNo = dt.Rows[0]["BATCHNO"].ToString();
                    ca.Itemid = dt.Rows[0]["ITEMID"].ToString();
                    ca.ProNo = dt.Rows[0]["PRODID"].ToString();
                    ca.Rate = dt.Rows[0]["RATEPHR"].ToString();
                    ca.ProDate = dt.Rows[0]["PRODDATE"].ToString();
                    ca.SampleNo = dt.Rows[0]["SAMPLENO"].ToString();
                    ca.NozzleNo = dt.Rows[0]["NOZZLENO"].ToString();
                    ca.AirPress = dt.Rows[0]["AIRPRESS"].ToString();
                    ca.Additive = dt.Rows[0]["ADDCH"].ToString();
                    ca.Stime = dt.Rows[0]["STIME"].ToString();
                    ca.CTemp = dt.Rows[0]["BCT"].ToString();
                    ca.FResult = dt.Rows[0]["FINALRESULT"].ToString();
                    ca.RType = dt.Rows[0]["RESULTTYPE"].ToString();
                    ca.Enterd = dt.Rows[0]["ENTEREDBY"].ToString();
                    ca.Reamarks = dt.Rows[0]["REMARKS"].ToString();

                }
                DataTable dt2 = new DataTable();
                dt2 = QCFinalValueEntryService.GetQCFVResultDetail(id);
                if (dt2.Rows.Count > 0)
                {

                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new QCFVItem();
                        tda.Des = dt2.Rows[0]["TDESC"].ToString();
                        tda.Value = dt2.Rows[0]["VALUEORMANUAL"].ToString();
                        tda.Unit = dt2.Rows[0]["UNIT"].ToString();
                        tda.Sta = dt2.Rows[0]["STARTVALUE"].ToString();
                        tda.En = dt2.Rows[0]["ENDVALUE"].ToString();
                        tda.Test = dt2.Rows[0]["TESTVALUE"].ToString();
                        tda.Manual = dt2.Rows[0]["MANUALVALUE"].ToString();
                        tda.Actual = dt2.Rows[0]["ACTTESTVALUE"].ToString();
                        tda.Result = dt2.Rows[0]["TESTRESULT"].ToString();
                        TData.Add(tda);
                    }
                }
                DataTable dt3 = new DataTable();
                dt3 = QCFinalValueEntryService.GetQCFVGasDetail(id);

                if (dt3.Rows.Count > 0)
                {

                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        tda1 = new QCFVItemDeatils();
                        tda1.Time = dt3.Rows[0]["MINS"].ToString();
                        tda1.Vol = dt3.Rows[0]["VOL25C"].ToString();
                        tda1.Volat = dt3.Rows[0]["VOL35C"].ToString();
                        tda1.Volc = dt3.Rows[0]["VOL45C"].ToString();
                        tda1.Stp = dt3.Rows[0]["VOLSTP"].ToString();
                        TData1.Add(tda1);
                    }
                }
                else
                {
                    for (int i = 0; i < 1; i++)
                    {
                        tda1 = new QCFVItemDeatils();
                        tda1.Isvalid = "Y";
                        TData1.Add(tda1);
                    }
                }
                //st = StateService.GetStateById(id);

            }
            ca.QCFVLst = TData;
            ca.QCFVDLst = TData1;
            return View(ca);
        }
        [HttpPost]
        public ActionResult QCFinalValueEntry(QCFinalValueEntry Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = QCFinalValueEntryService.QCFinalValueEntryCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = " QCFinalValueEntry Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " QCFinalValueEntry Updated Successfully...!";
                    }
                    return RedirectToAction("ListQCFinalValueEntry");
                }

                else
                {
                    ViewBag.PageTitle = "Edit QCFinalValueEntry";
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
        public IActionResult ListQCFinalValueEntry()
        {
            IEnumerable<QCFinalValueEntry> cmp = QCFinalValueEntryService.GetAllQCFinalValueEntry();
            return View(cmp);
        }
        //public JsonResult GetGRNItemJSON(string supid)
        //{
        //    QCFinalValueEntry model = new QCFinalValueEntry();
        //    model.Itemlst = BindItemlst(supid);
        //    return Json(BindItemlst(supid));

        //}
        public JsonResult GetItemJSON(string supid)
        {
            QCFinalValueEntry model = new QCFinalValueEntry();
            model.Itemlst = BindItemlst(supid);
            return Json(BindItemlst(supid));

        }
        //public JsonResult GetDrumJSON(string Drmid)
        //{
        //    QCFinalValueEntry model = new QCFinalValueEntry();
        //    model.drumlst = Binddrum(Drmid); 
        //    return Json(Binddrum(Drmid));

        //}
        public JsonResult GetBatchJSON(string Batchid)
        {
            QCFinalValueEntry model = new QCFinalValueEntry();
            model.Batchlst = BindBatch(Batchid);
            return Json(BindBatch(Batchid));

        }
        public List<SelectListItem> Binddrum()
        {
            try
            {
                DataTable dtDesg = QCFinalValueEntryService.DrumDeatils();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DRUMNO"].ToString(), Value = dtDesg.Rows[i]["ODRUMNO"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindBatch(string value)
        {
            try
            {
                DataTable dtDesg = QCFinalValueEntryService.BatchDeatils(value);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["NBATCHNO"].ToString(), Value = dtDesg.Rows[i]["NBATCHNO"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindItemlst(string value)
        {
            try
            {
                DataTable dtDesg = QCFinalValueEntryService.GetItem(value);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["OITEMID"].ToString() });
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
                DataTable dtDesg = QCFinalValueEntryService.GetProcess();
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
                DataTable dtDesg = QCFinalValueEntryService.GetWorkCenter();
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

    }
}
