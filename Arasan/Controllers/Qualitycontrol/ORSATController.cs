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
            ca.shiftlst = Bindshift();
            ca.worklst = Bindwork();

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
            ca.ORSATlst = TData;
            
            return View(ca);
        }

        [HttpPost]
        public ActionResult ORSAT(ORSAT Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = ORSATService.ORSATCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = " ORSAT Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " ORSAT Updated Successfully...!";
                    }
                    return RedirectToAction("ListORSAT");
                }

                else
                {
                    ViewBag.PageTitle = "Edit ORSAT";
                    TempData["notice"] = Strout;
                    //return View();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Cy);
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
        
        

        public List<SelectListItem> Bindshift()
        {
            try
            {
                DataTable dtDesg = ORSATService.Getshift();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SHIFTNO"].ToString(), Value = dtDesg.Rows[i]["SHIFTMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> Bindwork()
        {
            try
            {
                DataTable dtDesg = ORSATService.Getwork();
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

        public IActionResult ListORSAT(string st, string ed)
        {
           IEnumerable<ORSAT> cmp = ORSATService.GetAllORSAT(st, ed);
           return View(cmp);
        }

        public JsonResult GetItemGrpJSON()
        {
            ORSATdetails model = new ORSATdetails();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(model);
        }

        public IActionResult ViewORSAT(string id)
        {
            ORSAT ca = new ORSAT();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();

            dt = ORSATService.GetViewORSAT(id);
            if (dt.Rows.Count > 0)
            {
                ca.Branch = dt.Rows[0]["BRANCH"].ToString();
                ca.docid = dt.Rows[0]["DOCID"].ToString();
                ca.docdate = dt.Rows[0]["DOCDATE"].ToString();
                ca.shift = dt.Rows[0]["SHIFTNO"].ToString();
                ca.ID = id;
                ca.work = dt.Rows[0]["WCID"].ToString();
                ca.entry = dt.Rows[0]["ENTDATE"].ToString();
                ca.time = dt.Rows[0]["ETIME"].ToString();
                ca.remarks = dt.Rows[0]["REMARKS"].ToString();

                List<ORSATdetails> Data = new List<ORSATdetails>();
                ORSATdetails tda = new ORSATdetails();
                //double tot = 0;

                dtt = ORSATService.GetViewORSATDetail(id);
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda.para = dtt.Rows[0]["PARAMETERS"].ToString();
                        tda.value = dtt.Rows[0]["PARAMVAL"].ToString();
                        
                        Data.Add(tda);
                    }
                }

                ca.ORSATlst = Data;
            }
            return View(ca);
        }


        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = ORSATService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListORSAT");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListORSAT");
            }
        }

    }
}
