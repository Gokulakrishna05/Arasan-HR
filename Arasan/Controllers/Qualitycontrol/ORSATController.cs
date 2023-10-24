using Arasan.Interface;
using System.Collections.Generic;
using Arasan.Models;

using Microsoft.AspNetCore.Mvc;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using Arasan.Services.Qualitycontrol;
using Arasan.Interface.Qualitycontrol;

namespace Arasan.Controllers.Qualitycontrol
{
    public class ORSATController : Controller
    {

        IORSAT ORSATService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public ORSATController(IORSAT _ORSATService, IConfiguration _configuratio)
        {
            ORSATService = _ORSATService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult ORSAT(string id, string tag)
        {
            ORSAT ca = new ORSAT();
            ca.Brlst = BindBranch();
            ca.Doclst = BindDoc();

            List<ORSATdetails> TData = new List<ORSATdetails>();
            ORSATdetails tda = new ORSATdetails();
            List<ORSATdetails> TData1 = new List<ORSATdetails>();
            ORSATdetails tda1 = new ORSATdetails();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new ORSATdetails();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {

            }
            ca.OBSATlst = TData;
            
            return View(ca);
        }

        [HttpPost]
        //public ActionResult ORSAT(ORSAT Cy, string id)
        //{

        //    try
        //    {
        //        Cy.ID = id;
        //        string Strout = ORSATService.ORSATCRUD(Cy);
        //        if (string.IsNullOrEmpty(Strout))
        //        {
        //            if (Cy.ID == null)
        //            {
        //                TempData["notice"] = " ORSAT Inserted Successfully...!";
        //            }
        //            else
        //            {
        //                TempData["notice"] = " ORSAT Updated Successfully...!";
        //            }
        //            return RedirectToAction("ListORSAT");
        //        }

        //        else
        //        {
        //            ViewBag.PageTitle = "Edit ORSAT";
        //            TempData["notice"] = Strout;
        //            //return View();
        //        }

                // }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return View(Cy);
        //}
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
        
            public List<SelectListItem> BindDoc()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "JCE*300571", Value = "JCE*300571" });


                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult ListORSAT()
        {
            return View();
        }
    }
}
