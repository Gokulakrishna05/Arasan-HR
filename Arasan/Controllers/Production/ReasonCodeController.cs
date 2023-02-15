using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Interface.Production;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using Arasan.Services;

namespace Arasan.Controllers.Production
{

    public class ReasonCodeController : Controller
    {
        IReasonCodeService ReasonCodeService;

        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public ReasonCodeController(IReasonCodeService _ReasonCodeService, IConfiguration _configuratio)
        {
            ReasonCodeService = _ReasonCodeService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult ReasonCode(string id)
        {
            ReasonCode ca = new ReasonCode();
            ca.assignList = BindEmp();
            //ca.Categorylst = BindCategory();
            List<ReasonItem> TData = new List<ReasonItem>();
            ReasonItem tda = new ReasonItem();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new ReasonItem();
                    tda.Categorylst = BindCategory();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                DataTable dt = new DataTable();

                dt = ReasonCodeService.GetReasonCode(id);
                if (dt.Rows.Count > 0)
                {
                    ca.ModBy = dt.Rows[0]["MODBY"].ToString();
                    //ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();

                    
                }

            }
            ca.ReLst = TData;
            return View(ca);
        }
        [HttpPost]
        public ActionResult ReasonCode(ReasonCode Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = ReasonCodeService.ReasonCodeCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "ReasonCode Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "ReasonCode Updated Successfully...!";
                    }
                    return RedirectToAction("ListReasonCode");
                }

                else
                {
                    ViewBag.PageTitle = "Edit ReasonCode";
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
        public List<SelectListItem> BindCategory()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "IDLE-TIME", Value = "IDLE-TIME" });
                lstdesg.Add(new SelectListItem() { Text = "DOWN-TIME", Value = "DOWN-TIME" });
                lstdesg.Add(new SelectListItem() { Text = "RUN-TIME", Value = "RUN-TIME" });

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
        public IActionResult ListReasonCode()
        {
            IEnumerable<ReasonCode> cmp = ReasonCodeService.GetAllReasonCode();
            return View(cmp);
        }
        public JsonResult GetItemJSON()
        {
            ReasonItem model = new ReasonItem();
            //model.Itemlst = BindItemlst(itemid);
            return Json(model);

        }
        //public JsonResult GetItemGrpJSON()
        //{
        //    //DirectItem model = new DirectItem();
        //    //  model.ItemGrouplst = BindItemGrplst(value);
        //    return Json(BindItemGrplst());
        //}
    }
}
