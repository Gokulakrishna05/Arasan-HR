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
            ac.Schemelst = BindScheme();
            ac.ledlst = Bindledlst();

            //List<ConfigItem> TData = new List<ConfigItem>();
            //ConfigItem tda = new ConfigItem();
            if (id == null)
            {
                //for (int i = 0; i < 1; i++)
                //{
                //    tda = new ConfigItem();

                //    tda.ledlst = Bindledlst();
                //    tda.Isvalid = "Y";
                //    TData.Add(tda);
                //}
            }
            else
            {
                DataTable dt = new DataTable();

                dt = AccConfigService.GetAccConfig(id);
                if (dt.Rows.Count > 0)
                {
                    ac.Scheme = dt.Rows[0]["ADSCHEME"].ToString();
                    ac.TransactionName = dt.Rows[0]["TRANSDESC"].ToString();
                    ac.TransactionID = dt.Rows[0]["TRANSID"].ToString();
                    
                    //tda.saveledger = dt2.Rows[i]["ADACCOUNT"].ToString();

                    ac.Type = dt.Rows[0]["ADTYPE"].ToString();
                    ac.Tname = dt.Rows[0]["ADNAME"].ToString();
                    ac.Schname = dt.Rows[0]["ADSCHEMENAME"].ToString();
                    ac.ledger = dt.Rows[0]["ADACCOUNT"].ToString();

                    ac.ID = id;

                }
            }
            //DataTable dt2 = new DataTable();

            //dt2 = AccConfigService.GetAccConfigItem(id);
            //if (dt2.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dt2.Rows.Count; i++)
            //    {
            //        tda = new ConfigItem();
            //        double toaamt = 0;
            //        tda.ledlst = Bindledlst();

            //        tda.ledger = dt2.Rows[i]["ADACCOUNT"].ToString();
            //        tda.saveledger = dt2.Rows[i]["ADACCOUNT"].ToString();

            //        tda.Type = dt2.Rows[i]["ADTYPE"].ToString();
            //        tda.Tname = dt2.Rows[i]["ADNAME"].ToString();
            //        tda.Scheme = dt2.Rows[i]["ADSCHEMENAME"].ToString();
            //        TData.Add(tda);
            //    }
            //}

            //ac.Schemelst = TData;
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
                    return RedirectToAction("AccConfig");
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

        public IActionResult ListAccConfig()
        {
            IEnumerable<AccConfig> cmp = AccConfigService.GetAllAccConfig();
            return View(cmp);
        }


        public ActionResult GetSchemeDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string scheme = "";
                string transactionName = "";
                string transactionID = "";
                


                dt = AccConfigService.GetSchemeDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    scheme = dt.Rows[0]["ADSCHEME"].ToString();
                    transactionName = dt.Rows[0]["ADTRANSDESC"].ToString();
                    transactionID = dt.Rows[0]["ADTRANSID"].ToString();
                   
                   
                }

                var result = new { scheme = scheme, transactionName = transactionName, transactionID = transactionID };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetschemeJSON(string ItemId)
        {
            AccConfig model = new AccConfig();
            model.Schemelst = BindSchemelst(ItemId);
            return Json(BindSchemelst(ItemId));

        }
        public List<SelectListItem> BindSchemelst(string value)
        {
            try
            {
                DataTable dtDesg = AccConfigService.Getschemebyid(value);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ADSCHEME"].ToString(), Value = dtDesg.Rows[i]["ADCOMPHID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindScheme()
        {
            try
            {
                DataTable dtDesg = AccConfigService.GetConfig();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ADSCHEME"].ToString(), Value = dtDesg.Rows[i]["ADCOMPHID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
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
       
    }
}
