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
        IAccConfig Config;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public AccConfigController(IAccConfig _Config, IConfiguration _configuratio)
        {
            Config = _Config;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult AccConfig(string id)
        {
            AccConfig ac = new AccConfig();
            
            
            List<ConfigItem> TData = new List<ConfigItem>();
            ConfigItem tda = new ConfigItem();
            if (id==null)
            {
                DataTable dt = new DataTable();
                dt = Config.GetConfig();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        tda = new ConfigItem();
                        tda.Type = dt.Rows[i]["TYPE"].ToString();
                        tda.Ledlst = BindLedger();
                        tda.Ledger = dt.Rows[i]["LEDGERID"].ToString();
                        tda.ID = dt.Rows[i]["ACCOUNTCONFIGID"].ToString();
                        TData.Add(tda);
                        //sq.ID = id;
                    }
                }

                //for (int i = 0; i < 8; i++)
                //{
                //    tda = new ConfigItem();
                //    tda.Ledlst = BindLedger();

               
                //    TData.Add(tda);

                   
                //    //TData.Add(tda);
                //}
            }
            else
            {
                //DataTable dt = new DataTable();
                //dt = Config.GetConfig();
                //if (dt.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        tda = new ConfigItem();
                //        tda.Type = dt.Rows[i]["TYPE"].ToString();
                //        tda.Ledlst = BindLedger();
                //        tda.Ledger = dt.Rows[i]["LEDGERID"].ToString();
                //        TData.Add(tda);
                //        //sq.ID = id;
                //    }
                //}
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
                string Strout = Config.ConfigCRUD(Cy);
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
        public List<SelectListItem> BindLedger()
        {
            try
            {
                DataTable dtDesg = Config.GetLedger();
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
