using System.Collections.Generic;
using System.Data;
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
    public class QCTestValueEntryController : Controller
    {
        IQCTestValueEntryService QCTestValueEntryService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public QCTestValueEntryController(IQCTestValueEntryService _QCTestValueEntryService, IConfiguration _configuratio)
        {
            QCTestValueEntryService = _QCTestValueEntryService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult QCTestValueEntry(string id,string tag)
        {
            QCTestValueEntry ca = new QCTestValueEntry();
            ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];
            ca.assignList = BindEmp();
            ca.Worklst = BindWorkCenter();
            ca.itemlst = BindItem();
            ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.Shiftlst = BindShift();
            DataTable dtv = datatrans.GetSequence("QTVE");
            if (dtv.Rows.Count > 0)
            {
                ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
            List<QCTestValueEntryItem> TData = new List<QCTestValueEntryItem>();
            QCTestValueEntryItem tda = new QCTestValueEntryItem();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new QCTestValueEntryItem();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }

            }
            else
            {
                DataTable dt = new DataTable();
                if (tag == null)
                {
                    //ca = QCTestValueEntryService.GetQCTestValueEntryById(id);
                    //DataTable dt = new DataTable();
                    dt = QCTestValueEntryService.GetQCTestValueEntryDetails(id);
                    if (dt.Rows.Count > 0)
                    {
                        ca.Branch = dt.Rows[0]["BRANCH"].ToString();
                        ca.DocId = dt.Rows[0]["DOCID"].ToString();
                        ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                        ca.Work = dt.Rows[0]["WCID"].ToString();
                        ca.Shift = dt.Rows[0]["SHIFTNO"].ToString();
                        ca.Process = dt.Rows[0]["PROCESSLOTNO"].ToString();
                        ca.Drum = dt.Rows[0]["CDRUMNO"].ToString();
                        ca.Prodate = dt.Rows[0]["PRODDATE"].ToString();
                        ca.Sample = dt.Rows[0]["SAMPLENO"].ToString();
                        ca.Sampletime = dt.Rows[0]["STIME"].ToString();
                        ca.Item = dt.Rows[0]["ITEMID"].ToString();
                        ca.Entered = dt.Rows[0]["ENTEREDBY"].ToString();
                        ca.Remarks = dt.Rows[0]["REMARKS"].ToString();
                        ca.Rate = dt.Rows[0]["RATEPHR"].ToString();
                        ca.Nozzle = dt.Rows[0]["NOZZLENO"].ToString();
                        ca.Air = dt.Rows[0]["AIRPRESS"].ToString();
                        ca.AddCharge = dt.Rows[0]["ADDCH"].ToString();
                        ca.Ctemp = dt.Rows[0]["CDRUMNO"].ToString();
                        ca.ID = id;


                    }
                    DataTable dt1 = new DataTable();
                    dt1 = QCTestValueEntryService.GetQCTestDetails(id);
                    if (dt1.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {

                            tda = new QCTestValueEntryItem();
                            //ViewBag.Item = dt.Rows[0]["ITEMID"].ToString();
                            tda.description = dt1.Rows[i]["TDESC"].ToString();
                            tda.startvalue = dt1.Rows[i]["STARTVALUE"].ToString();
                            tda.actual = dt1.Rows[i]["ACTTESTVALUE"].ToString();
                            tda.test = dt1.Rows[i]["TESTVALUE"].ToString();
                            tda.testresult = dt1.Rows[i]["TESTRESULT"].ToString();
                            tda.endvalue = dt1.Rows[i]["ENDVALUE"].ToString();
                            tda.unit = dt1.Rows[i]["UNIT"].ToString();
                            tda.value = dt1.Rows[i]["VALUEORMANUAL"].ToString();
                            tda.manual = dt1.Rows[i]["MANUALVALUE"].ToString();

                            tda.apid = id;
                            TData.Add(tda);
                        }
                    }
                }
                else
                {
                  
                    DataTable dt1 = new DataTable();
                    dt1 = QCTestValueEntryService.GetAPOutDetails(id);
                    if (dt1.Rows.Count > 0)
                    {



                        ca.Work = dt1.Rows[0]["WCID"].ToString();
                        ca.Shift = dt1.Rows[0]["SHIFT"].ToString();
                        ca.Prodate = dt1.Rows[0]["DOCDATE"].ToString();
                        ca.APID = id;
                        DataTable dtt1 = new DataTable();
                        dtt1 = QCTestValueEntryService.GetAPOutItemDetails(id);
                        if (dtt1.Rows.Count > 0)
                        {
                            ca.Drum = dtt1.Rows[0]["DRUMNO"].ToString();


                            ca.Sampletime = dtt1.Rows[0]["FROMTIME"].ToString();
                            ca.Item = dtt1.Rows[0]["ITEMID"].ToString();
                            ca.ItemId = dtt1.Rows[0]["item"].ToString();
                            ViewBag.Item = dtt1.Rows[0]["ITEMID"].ToString();
                        }

                    }
                    DataTable dtt = new DataTable();

                    List<QCTestValueEntryItem> Data = new List<QCTestValueEntryItem>();
                    QCTestValueEntryItem tda1 = new QCTestValueEntryItem();
                    //string itemid = datatrans.GetDataString(" SELECT ITEMMASTERID FROM ITEMMASTER WHERE ITEMID='" + ca.ItemId + "'");
                    string temp = datatrans.GetDataString(" SELECT TEMPLATEID FROM ITEMMASTER WHERE ITEMMASTERID='" + ca.ItemId + "'");
                    dtt = QCTestValueEntryService.GetItemDetail(temp);
                    if (dtt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtt.Rows.Count; i++)
                        {
                            tda1 = new QCTestValueEntryItem();

                            tda1.description = dtt.Rows[i]["TESTDESC"].ToString();
                            tda1.value = dtt.Rows[i]["VALUEORMANUAL"].ToString();
                            tda1.unit = dtt.Rows[i]["UNITID"].ToString();
                            tda1.startvalue = dtt.Rows[i]["STARTVALUE"].ToString();
                            tda1.endvalue = dtt.Rows[i]["ENDVALUE"].ToString();

                            Data.Add(tda1);
                        }
                    }
                    ca.QCTestLst = Data;
                }
                
            }
            ca.QCTestLst = TData;
            return View(ca);
        }
        [HttpPost]
        public ActionResult QCTestValueEntry(QCTestValueEntry Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = QCTestValueEntryService.QCTestValueEntryCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "QCTestValueEntry Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "QCTestValueEntry Updated Successfully...!";
                    }
                    return RedirectToAction("ListQCTestValueEntry", new { Cy.APID});
                }

                else
                {
                    ViewBag.PageTitle = "Edit QCTestValueEntry";
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
        public IActionResult ListQCTestValueEntry(string st, string ed)
        {
            IEnumerable<QCTestValueEntry> sta = QCTestValueEntryService.GetAllQCTestValueEntry(st,ed);
            return View(sta);
        }
        public JsonResult GetItemGrpJSON()
        {
            QCTestValueEntryItem model = new QCTestValueEntryItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(model);
        }
        public List<SelectListItem> BindShift()
        {
            try
            {
                DataTable dtDesg = QCTestValueEntryService.ShiftDeatils();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SHIFTNO"].ToString(), Value = dtDesg.Rows[i]["SHIFTNO"].ToString() });
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
                DataTable dtDesg = QCTestValueEntryService.GetWorkCenter();
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
        public List<SelectListItem> BindItem()
        {
            try
            {
                DataTable dtDesg = QCTestValueEntryService.GetItem();
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
        public ActionResult DeleteQC(string tag, string id)
        {

            string flag = QCTestValueEntryService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListQCTestValueEntry");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListQCTestValueEntry");
            }
        }
        public ActionResult GetItemDetails(string id)
        {
            QCTestValueEntry model = new QCTestValueEntry();
            DataTable dtt = new DataTable();
            
            List<QCTestValueEntryItem> Data = new List<QCTestValueEntryItem>();
            QCTestValueEntryItem tda = new QCTestValueEntryItem();
            string itemid = datatrans.GetDataString(" SELECT ITEMMASTERID FROM ITEMMASTER WHERE ITEMID='" + id + "'");
            string temp = datatrans.GetDataString(" SELECT TEMPLATEID FROM ITEMMASTER WHERE ITEMMASTERID='"+ itemid + "'");
            dtt = QCTestValueEntryService.GetItemDetail(temp);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new QCTestValueEntryItem();

                    tda.description = dtt.Rows[i]["TESTDESC"].ToString();
                    tda.value = dtt.Rows[i]["VALUEORMANUAL"].ToString();
                    tda.unit = dtt.Rows[i]["UNITID"].ToString();
                    tda.startvalue = dtt.Rows[i]["STARTVALUE"].ToString();
                    tda.endvalue = dtt.Rows[i]["ENDVALUE"].ToString();

                    Data.Add(tda);
                }
            }
            model.QCTestLst = Data;
            return Json(model.QCTestLst);
        }

        public IActionResult ViewQCTestValueEntry(string id)
        {

            QCTestValueEntry ca = new QCTestValueEntry();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();

            dt = QCTestValueEntryService.GetViewQCTestValueEntry(id);
            if (dt.Rows.Count > 0)
            {
                ca.Branch = dt.Rows[0]["BRANCH"].ToString();
                ca.DocId = dt.Rows[0]["DOCID"].ToString();
                ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                ca.Work = dt.Rows[0]["WCID"].ToString();
                ca.Shift = dt.Rows[0]["SHIFTNO"].ToString();
                ca.Process = dt.Rows[0]["PROCESSLOTNO"].ToString();
                ca.Drum = dt.Rows[0]["CDRUMNO"].ToString();
                ca.Prodate = dt.Rows[0]["PRODDATE"].ToString();
                ca.Sample = dt.Rows[0]["SAMPLENO"].ToString();
                ca.Sampletime = dt.Rows[0]["STIME"].ToString();
                ca.Item = dt.Rows[0]["ITEMID"].ToString();
                ca.Entered = dt.Rows[0]["ENTEREDBY"].ToString();
                ca.Remarks = dt.Rows[0]["REMARKS"].ToString();
                ca.Rate = dt.Rows[0]["RATEPHR"].ToString();
                ca.Nozzle = dt.Rows[0]["NOZZLENO"].ToString();
                ca.Air = dt.Rows[0]["AIRPRESS"].ToString();
                ca.AddCharge = dt.Rows[0]["ADDCH"].ToString();
                ca.Ctemp = dt.Rows[0]["CDRUMNO"].ToString();
                ca.ID = id;



                List<QCTestValueEntryItem> Data = new List<QCTestValueEntryItem>();
                QCTestValueEntryItem tda = new QCTestValueEntryItem();
                //double tot = 0;

                dtt = QCTestValueEntryService.GetViewQCTestDetails(id);
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda.description = dtt.Rows[i]["TDESC"].ToString();
                        tda.value = dtt.Rows[i]["VALUEORMANUAL"].ToString();
                        tda.unit = dtt.Rows[i]["UNIT"].ToString();
                        tda.startvalue = dtt.Rows[i]["STARTVALUE"].ToString();
                        tda.endvalue = dtt.Rows[i]["ENDVALUE"].ToString();
                        tda.test = dtt.Rows[i]["TESTVALUE"].ToString();
                        tda.manual = dtt.Rows[i]["MANUALVALUE"].ToString();
                        tda.actual = dtt.Rows[i]["ACTTESTVALUE"].ToString();
                        tda.testresult = dtt.Rows[i]["TESTRESULT"].ToString();

                        Data.Add(tda);
                    }
                }

                ca.QCTestLst = Data;
            }
            return View(ca);
        }

    }
    }
