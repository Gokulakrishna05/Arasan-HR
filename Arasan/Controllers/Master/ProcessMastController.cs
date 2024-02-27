using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers.Master
{
    public class ProcessMastController : Controller
    {
        IProcessMastService ProcessMastService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public ProcessMastController(IProcessMastService _ProcessMastService, IConfiguration _configuratio)
        {
            ProcessMastService = _ProcessMastService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult ProcessMast(String id)
        {

            ProcessMast pm = new ProcessMast();
            pm.Prodhrtypelst = BindProdhrtype();
            pm.Costtypelst = BindCosttype();


            //for edit & delete
            if (id != null)
            {
                DataTable dt = new DataTable();
                //double total = 0;
                dt = ProcessMastService.GetEditProcessMast(id);
                if (dt.Rows.Count > 0)
                {
                    pm.ProcessMastName = dt.Rows[0]["PROCESSID"].ToString();
                    pm.ProcessMastName = dt.Rows[0]["PROCESSNAME"].ToString();
                    pm.Batch = dt.Rows[0]["BATCHYN"].ToString();
                    pm.Qc = dt.Rows[0]["QCYN"].ToString();
                    pm.Sno = dt.Rows[0]["SNO"].ToString();
                    pm.Prodhrtype = dt.Rows[0]["PRODHRTYPE"].ToString();
                    pm.Costtype = dt.Rows[0]["BATCHORAVGCOST"].ToString();

                    pm.ID = id;

                }

            }
            else
            {
                pm.Qc = "N";
                pm.Batch = "N";
            }
            return View(pm);
        }
        public List<SelectListItem> BindProdhrtype()
        {
            try
            {
                List<SelectListItem> lstprodhr = new List<SelectListItem>();
                lstprodhr.Add(new SelectListItem() { Text = "CONTINUOUS", Value = "CONTINUOUS" });
                lstprodhr.Add(new SelectListItem() { Text = "FIXED", Value = "FIXED" });

                return lstprodhr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult ListProcessMast()
        {
            return View();
        }
        public List<SelectListItem> BindCosttype()
        {
            try
            {
                List<SelectListItem> lstcost = new List<SelectListItem>();
                lstcost.Add(new SelectListItem() { Text = "AVG COST", Value = "AVGCOST" });
                lstcost.Add(new SelectListItem() { Text = "BATCH COST", Value = "BATCHPOST" });

                return lstcost;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]

        public ActionResult ProcessMast(ProcessMast Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = ProcessMastService.ProcessMastCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Process Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Process Updated Successfully...!";
                    }
                    return RedirectToAction("ListProcessMast");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Process";
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

        public ActionResult MyListProcessMastgrid(string strStatus)
        {
            List<ProcessMastItem> Reg = new List<ProcessMastItem>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;

            dtUsers = ProcessMastService.GetAllProcessMast(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                EditRow = "<a href=ProcessMast?id=" + dtUsers.Rows[i]["PROCESSMASTID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                DeleteRow = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["PROCESSMASTID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";


                Reg.Add(new ProcessMastItem
                {
                    processmastid = dtUsers.Rows[i]["PROCESSID"].ToString(),
                    processmastname = dtUsers.Rows[i]["PROCESSNAME"].ToString(),
                    prodhrtype = dtUsers.Rows[i]["PRODHRTYPE"].ToString(),
                    batchoravg = dtUsers.Rows[i]["BATCHORAVGCOST"].ToString(),
                    editrow = EditRow,
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }
        public ActionResult DeleteItem(string tag, string id)
        {

            string flag = ProcessMastService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListProcessMast");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListProcessMast");
            }
        }
    }
}
