using System.Collections.Generic;
using System.Data;
 using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
 using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers

{
    public class ExchangeRateController : Controller
    {

        IExchangeRateService exchangerateService;
        IConfiguration? _configuration;
        private string? _connectionString;
        DataTransactions datatrans;

        public ExchangeRateController(IExchangeRateService _exchangerateService, IConfiguration _configuration)
        {
            exchangerateService = _exchangerateService;
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult ExchangeRate(string id)
        {
            ExchangeRate ic = new ExchangeRate();
            //ic.createby = Request.Cookies["UserId"];
            ic.Symlst = BindSymbol();
            ic.Rtlst = BindRateType();

            

            if (id == null)
            {
               
            }
            else
            {
                DataTable dt = new DataTable();

                //double total = 0;
                dt = exchangerateService.GetEditExchangeDetail(id);
                if (dt.Rows.Count > 0)
                {
                    ic.ID = dt.Rows[0]["CRATEID"].ToString();
                    ic.CurrencySymbol = dt.Rows[0]["CURRID"].ToString();
                    ic.CurrencyName = dt.Rows[0]["CURRNAME"].ToString();
                    ic.Exchange = dt.Rows[0]["EXRATE"].ToString();                  
                    ic.RateType = dt.Rows[0]["RTYPE"].ToString();
                    ic.ExchangeDate = dt.Rows[0]["RATEDT"].ToString();

                }

            }
           
            return View(ic);
        }
        public ActionResult MyListExchangegrid(string strStatus)
        {
            List<Exchangegrid> Reg = new List<Exchangegrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = exchangerateService.GetAllExchangeGRID(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    EditRow = "<a href=ExchangeRate?id=" + dtUsers.Rows[i]["CRATEID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["CRATEID"].ToString() + "";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "Remove?tag=Del&id=" + dtUsers.Rows[i]["CRATEID"].ToString() + "";
                }


                Reg.Add(new Exchangegrid
                {
                    id = dtUsers.Rows[i]["CRATEID"].ToString(),
                    csym = dtUsers.Rows[i]["CURRID"].ToString(),
                    cname = dtUsers.Rows[i]["CURRNAME"].ToString(),
                    rtype = dtUsers.Rows[i]["RTYPE"].ToString(),
                    erate = dtUsers.Rows[i]["EXRATE"].ToString(),
                    date = dtUsers.Rows[i]["RATEDT"].ToString(),
                    editrow = EditRow,
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }
        public ActionResult Getcurrencydetails(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string sym = "";

                dt = datatrans.GetData("SELECT MAINCURR FROM CURRENCY WHERE CURRENCYID='" + ItemId+"'");

                if (dt.Rows.Count > 0)
                {

                    sym = dt.Rows[0]["MAINCURR"].ToString();

                }

                var result = new { sym = sym };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindSymbol()
        {
            try
            {
                DataTable dtDesg = exchangerateService.GetSym();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SYMBOL"].ToString(), Value = dtDesg.Rows[i]["CURRENCYID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

  
        public List<SelectListItem> BindRateType()
        {
            try
            {
                List<SelectListItem> lstprodhr = new List<SelectListItem>();
                lstprodhr.Add(new SelectListItem() { Text = "SALES", Value = "SALES" });
                lstprodhr.Add(new SelectListItem() { Text = "PURCHASE", Value = "PURCHASE" });
                lstprodhr.Add(new SelectListItem() { Text = "CUSTOMS", Value = "CUSTOMS" });

                return lstprodhr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult ExchangeRate(ExchangeRate Ic, string id)
        {

            try
            {
                Ic.ID = id;
                string Strout = exchangerateService.ExchangeRateCRUD(Ic);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Ic.ID == null)
                    {
                        TempData["notice"] = "ExchangeRate Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "ExchangeRate Updated Successfully...!";
                    }
                    return RedirectToAction("ListExchangeRate");
                }

                else
                {
                    ViewBag.PageTitle = "Edit ExchangeRate";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Ic);
        }
        public ActionResult DeleteMR(string tag, string id)
        {

            string flag = exchangerateService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListExchangeRate");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListExchangeRate");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = exchangerateService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListExchangeRate");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListExchangeRate");
            }
        }
      
        public IActionResult ListExchangeRate()
        {
            return View();
        }
    }
}
