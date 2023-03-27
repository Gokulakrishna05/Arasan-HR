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
            if (id == null)
            {

            }
            else
            {
                //st = StateService.GetStateById(id);

            }
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
            //IEnumerable<QCFinalValueEntry> cmp = QCFinalValueEntryService.GetAllQCFinalValueEntry();
            return View();
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
