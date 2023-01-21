using System.Collections.Generic;
using System.Data;
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
        public IActionResult QCTestValueEntry(string id)
        {
            QCTestValueEntry ca = new QCTestValueEntry();
            ca.Brlst = BindBranch();
            ca.assignList = BindEmp();
            if (id == null)
            {

            }
            else
            {
                //ca = QCTestValueEntryService.GetQCTestValueEntryById(id);
                DataTable dt = new DataTable();
                double total = 0;
                dt = QCTestValueEntryService.GetQCTestValueEntryDetails(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.Work = dt.Rows[0]["WCID"].ToString();
                    ca.Shift = dt.Rows[0]["SHIFTNO"].ToString();
                    ca.Process = dt.Rows[0]["PROCESSLOTNO"].ToString();
                    ca.Drum = dt.Rows[0]["DRUMNO"].ToString();
                    ca.Prodate = dt.Rows[0]["PRODDATE"].ToString();
                    ca.Sample = dt.Rows[0]["DSAMPLE"].ToString();
                    ca.Sampletime = dt.Rows[0]["DSAMPLETIME"].ToString();
                    ca.Item = dt.Rows[0]["ITEMID"].ToString();
                    ca.Entered = dt.Rows[0]["ENTEREDBY"].ToString();
                    ca.Remarks = dt.Rows[0]["REMARKS"].ToString();



                }
            }
            return View(ca);
        }
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
                    return RedirectToAction("ListQCTestValueEntry");
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
        public IActionResult ListQCTestValueEntry()
        {
            IEnumerable<QCTestValueEntry> sta = QCTestValueEntryService.GetAllQCTestValueEntry();
            return View();
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
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPNAME"].ToString(), Value = dtDesg.Rows[i]["EMPMASTID"].ToString() });
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
