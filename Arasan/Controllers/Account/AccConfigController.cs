using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.Pkcs;
using System.Xml.Linq;
using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Production;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class AccConfigController : Controller
    {
        IAccConfig AccConfigService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public AccConfigController(IAccConfig _AccConfigService, IConfiguration _configuratio)
        {
            AccConfigService = _AccConfigService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult AccConfig(string id)
        {
            AccConfig ac = new AccConfig();
            ac.CreatBy = Request.Cookies["UserId"];
            //ac.CreatOn = Request.Cookies["Date"];
            ac.Branch = Request.Cookies["BRANCHID"];
           
            List<ConfigItem> TData = new List<ConfigItem>();
            ConfigItem tda = new ConfigItem();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new ConfigItem();

                    tda.ledlst = Bindledlst();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }

            }
            else
            {
                DataTable dt = new DataTable();

                dt = AccConfigService.GetAccConfig(id);
                if (dt.Rows.Count > 0)
                {
                    ac.SchemeDes = dt.Rows[0]["ADSCHEMEDESC"].ToString();
                    ac.Scheme = dt.Rows[0]["ADSCHEME"].ToString();
                    ac.ID = id;
                    ac.TransactionName = dt.Rows[0]["ADTRANSDESC"].ToString();
                    ac.TransactionID = dt.Rows[0]["ADTRANSID"].ToString();
                    
                }

                DataTable dt2 = new DataTable();

                dt2 = AccConfigService.GetAccConfigItem(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new ConfigItem();
                        //double toaamt = 0;
                        //tda.ledlst = Bindledlst();


                        tda.Type = dt2.Rows[i]["ADTYPE"].ToString();
                        tda.Tname = dt2.Rows[i]["ADNAME"].ToString();
                        tda.Schname = dt2.Rows[i]["ADSCHEMENAME"].ToString();
                        tda.ledger = dt2.Rows[i]["ADACCOUNT"].ToString();
                        

                        TData.Add(tda);
                    }
                }
            }
            ac.ConfigLst = TData;
            return View(ac);
        }
        [HttpPost]
        public ActionResult AccConfig(AccConfig Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = AccConfigService.ConfigCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "AccConfig Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "AccConfig Updated Successfully...!";
                    }
                    return RedirectToAction("ListAccConfig");
                }

                else
                {
                    ViewBag.PageTitle = "Edit AccConfig";
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

        public IActionResult ListAccConfig(string Active)
        {
            IEnumerable<AccConfig> cmp = AccConfigService.GetAllAccConfig(Active);
            return View(cmp);
        }


        //public ActionResult GetSchemeDetail(string ItemId)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        //string scheme = "";
        //        string transactionname = "";
        //        string transactionid = "";
                

        //        dt = AccConfigService.GetSchemeDetails(ItemId);

        //        if (dt.Rows.Count > 0)
        //        {
        //            //scheme = dt.Rows[0]["ADSCHEME"].ToString();
        //            transactionname = dt.Rows[0]["ADTRANSDESC"].ToString();
        //            transactionid = dt.Rows[0]["ADTRANSID"].ToString();
                   
        //        }

        //        var result = new { transactionname = transactionname, transactionid = transactionid };
        //        return Json(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        public JsonResult GetledgerJSON()
        {
            //DeductionItem model = new DeductionItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(Bindledlst());
        }

        //public JsonResult GetschemeJSON(string ItemId)
        //{
        //    AccConfig model = new AccConfig();
        //    model.Schemelst = BindSchemelst(ItemId);
        //    return Json(BindSchemelst(ItemId));

        //}

        //public List<SelectListItem> BindSchemelst(string value)
        //{
        //    try
        //    {
        //        DataTable dtDesg = AccConfigService.Getschemebyid(value);
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        for (int i = 0; i < dtDesg.Rows.Count; i++)
        //        {
        //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ADSCHEME"].ToString(), Value = dtDesg.Rows[i]["ADCOMPHID"].ToString() });
        //        }
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public List<SelectListItem> BindAction()
        //{
        //    try
        //    {
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        lstdesg.Add(new SelectListItem() { Text = "Debited", Value = "Debited" });
        //        lstdesg.Add(new SelectListItem() { Text = "Credited", Value = "Credited" });
                

        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public List<SelectListItem> Bindledlst()
        {
            try
            {
                DataTable dtDesg = AccConfigService.Getledger();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LEDNAME"].ToString(), Value = dtDesg.Rows[i]["LEDGERID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = AccConfigService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListAccConfig");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListAccConfig");
            }
        }

        public ActionResult Remove(string tag, int id)
        {

            string flag = AccConfigService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListAccConfig");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListAccConfig");
            }
        }
    }
}
