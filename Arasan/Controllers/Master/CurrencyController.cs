using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers.Master
{
    public class CurrencyController : Controller
    {
        ICurrencyService CurrencyService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public CurrencyController(ICurrencyService _CurrencyService, IConfiguration _configuratio)
        {
            CurrencyService = _CurrencyService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult Currency(string id)
        {
            Currency cu = new Currency();

            List<UsedCountries> TData = new List<UsedCountries>();
            UsedCountries tda = new UsedCountries();


            cu.createby = Request.Cookies["UserId"];
            if (id == null)
            {


                for (int i = 0; i < 1; i++)
                {
                    tda = new UsedCountries();
                    tda.Currencieslst = BindCountries();
                    tda.Isvalid = "Y";
                    TData.Add(tda);

                }
            }
            else
            {



                DataTable dtt = new DataTable();
                //DataTable dtt = new DataTable();
                dtt = CurrencyService.GetCurrencyEdit(id);
                if (dtt.Rows.Count > 0)
                {
                    cu.ID = dtt.Rows[0]["CURRENCYID"].ToString();
                    cu.CurrencyCode = dtt.Rows[0]["SYMBOL"].ToString();
                    cu.CurrencyName = dtt.Rows[0]["MAINCURR"].ToString();
                    cu.CurrencyCodes = dtt.Rows[0]["CURREP"].ToString();
                    cu.CurrencyInteger = dtt.Rows[0]["CURWIDTH"].ToString();

                  

                }
                DataTable dtt2 = new DataTable();

                dtt2 = datatrans.GetData("SELECT CURRENCYID, CONCODE,COUNTRY FROM CONCURR Where CURRENCYID='" + id + "'");

                if (dtt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt2.Rows.Count; i++)
                    {



                        tda = new UsedCountries();
                        tda.Currencieslst = BindCountries();
                        tda.ConCode = dtt2.Rows[0]["CONCODE"].ToString();
                        tda.Country = dtt2.Rows[0]["COUNTRY"].ToString();
                        //tda.Itemlst = 



                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }








                //  cu = CurrencyService.GetCurrencyById(id);
               




            }

            cu.Currencylst = TData;
            return View(cu);
        }


        public JsonResult GetCountryJSON()
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindCountries());
        }
        [HttpPost]
        public ActionResult Currency(Currency Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = CurrencyService.CurrencyCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Currency Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Currency Updated Successfully...!";
                    }
                    return RedirectToAction("ListCurrency");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Currency";
                    TempData["notice"] = Strout;
                    return View("Currency");
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Cy);
        }
        public IActionResult ListCurrency()
        {
            //IEnumerable<Currency> cmp = CurrencyService.GetAllCurrency(status);
            return View();
        }

        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = CurrencyService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListCurrency");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListCurrency");
            }
        } public ActionResult Remove(string tag, int id)
        {

            string flag = CurrencyService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListCurrency");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListCurrency");
            }
        }
        public ActionResult MyListItemgrid(string strStatus)
        {
            List<Currencygrid> Reg = new List<Currencygrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;

            dtUsers = CurrencyService.GetAllCurrencygrid(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string ViewRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    EditRow = "<a href=Currency?id=" + dtUsers.Rows[i]["CURRENCYID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";


                    ViewRow = "<a href=ViewCurrency?id=" + dtUsers.Rows[i]["CURRENCYID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='Waiting for approval' /></a>";




                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["CURRENCYID"].ToString() + "";
                }
                else
                {

                    EditRow = "";
                    DeleteRow = "Remove?tag=Del&id=" + dtUsers.Rows[i]["CURRENCYID"].ToString() + "";
                }

                Reg.Add(new Currencygrid
                {
                    id = dtUsers.Rows[i]["CURRENCYID"].ToString(),
                    currencycode = dtUsers.Rows[i]["SYMBOL"].ToString(),
                    currencyname = dtUsers.Rows[i]["MAINCURR"].ToString(),
                    editrow = EditRow,
                    viewrow = ViewRow,
                    delrow = DeleteRow,

                });
            }
            return Json(new
            {
                Reg
            });

        }

        public List<SelectListItem> BindCountries()
            {
                try
                {
                    DataTable dtDesg = datatrans.GetData("select COUNTRYCODE,CONMASTID From  CONMAST");
                    List<SelectListItem> lstdesg = new List<SelectListItem>();
                    for (int i = 0; i < dtDesg.Rows.Count; i++)
                    {
                        lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["CONCODE"].ToString(), Value = dtDesg.Rows[i]["CONMASTID"].ToString() });
                    }
                    return lstdesg;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        public ActionResult GetCountry(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string country = "";

                dt = datatrans.GetData("SELECT COUNTRY FROM CONMAST WHERE CONMASTID='" + ItemId+"'");

                if (dt.Rows.Count > 0)
                {

                    country = dt.Rows[0]["COUNTRY"].ToString();

                }

                var result = new { country = country };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public IActionResult ViewCurrency(string id)
        {
            List<UsedCountries> TData = new List<UsedCountries>();
            UsedCountries tda = new UsedCountries();
            List<UsedCountry> TDatab = new List<UsedCountry>();
            UsedCountry tdab = new UsedCountry();
            DataTable dt = new DataTable();

            dt = datatrans.GetData("Select CURRENCYID,SYMBOL,MAINCURR,CURREP,CURWIDTH from CURRENCY WHERE CURRENCYID='" + id + "'");

            //"Select IGROUP,ISUBGROUP,ITEMGROUP,SUBGROUPCODE,SUBCATEGORY,BINNO,BINYN,LOTYN,RHYN,RUNPERQTY,RUNHRS,COSTCATEGORY,AUTOCONSYN,QCT,DRUMYN,ITEMFROM,ETARIFFMASTER.TARIFFID,PURCAT,MAJORYN,to_char(LATPURDT,'dd-MON-yyyy')LATPURDT,ITEMID,ITEMDESC,REORDERQTY,REORDERLVL,MINSTK,UNITMAST.UNITID,MASTER.MNAME,HSN,SELLINGPRICE,EXPYN,VALMETHOD,SERIALYN,BSTATEMENTYN,TESTTBASIC.TEMPLATEID,QCCOMPFLAG,LATPURPRICE,REJRAWMATPER,RAWMATPER,ADD1PER,ADD1,RAWMATCAT,ITEMACC,PTEMPLATEID,CURINGDAY,AUTOINDENT from ITEMMASTER LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT LEFT OUTER JOIN MASTER ON MASTER.MASTERID=ITEMMASTER.ITEMACC LEFT OUTER JOIN TESTTBASIC ON TESTTBASIC.TESTTBASICID=ITEMMASTER.TEMPLATEID LEFT OUTER JOIN ETARIFFMASTER ON ETARIFFMASTER.ETARIFFMASTERID=ITEMMASTER.TARIFFID   where ITEMMASTERID=" + id + "");
            Currency cu = new Currency();
            if (dt.Rows.Count > 0)
            {
                cu.ID = dt.Rows[0]["CURRENCYID"].ToString();
                cu.CurrencyCode = dt.Rows[0]["SYMBOL"].ToString();
                cu.CurrencyName = dt.Rows[0]["MAINCURR"].ToString();
                cu.CurrencyCodes = dt.Rows[0]["CURREP"].ToString();
                cu.CurrencyInteger = dt.Rows[0]["CURWIDTH"].ToString();


            }
            DataTable dt2 = new DataTable();

            dt2 = datatrans.GetData("SELECT CURRENCYID, CONCURR.CONCODE,CONCURR.COUNTRY,CONMAST.CONCODE FROM CONCURR  left outer join CONMAST ON CONMASTID=CONCURR.CONCODE Where CURRENCYID='" + id + "'");

            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {


                    tda = new UsedCountries();
                    tda.Currencieslst = BindCountries();
                    tda.ConCode = dt2.Rows[0]["COUNTRYCODE"].ToString();
                    tda.Country = dt2.Rows[0]["COUNTRY"].ToString();
                    //tda.Itemlst = 



                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }



            DataTable dtt2 = new DataTable();

            dtt2 = datatrans.GetData("SELECT EXRATE,to_char(RATEDT,'dd-MON-yyyy')RATEDT FROM CRATE WHERE CURRENCYID='" + id +"' ");

            if (dtt2.Rows.Count > 0)
            {
                for (int i = 0; i < dtt2.Rows.Count; i++)
                {


                    tdab = new UsedCountry();
                    tdab.UsedCurrencylst = BindCountries();
                    tdab.exrate = dtt2.Rows[i]["EXRATE"].ToString();
                    tdab.ratedt = dtt2.Rows[i]["RATEDT"].ToString();
                    tdab.Isvalid = "Y";
                    TDatab.Add(tdab);
                }
            }
            cu.Currencylst = TData;
            cu.UsedCurrencieslst = TDatab;

            return View(cu);
            }

        }
}
