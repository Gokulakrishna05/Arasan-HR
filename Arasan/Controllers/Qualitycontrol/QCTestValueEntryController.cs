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
                if (tag == null)
                {
                    //ca = QCTestValueEntryService.GetQCTestValueEntryById(id);
                    DataTable dt = new DataTable();
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
                        ca.ID = id;


                    }
                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        tda = new QCTestValueEntryItem();
                        tda.Isvalid = "Y";
                        tda.APID = id;
                        TData.Add(tda);
                    }
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
                        }



                    }
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
    }
}
