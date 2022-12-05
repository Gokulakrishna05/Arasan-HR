using System.Collections.Generic;
using Arasan.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Models;
using System.Xml.Linq;

namespace Arasan.Controllers 
{
    public class PurchaseReturnController : Controller
    {
        IPurchaseReturn PurReturn;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public PurchaseReturnController(IPurchaseReturn _PurchaseReturn, IConfiguration _configuratio)
        {
            PurReturn = _PurchaseReturn;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult PurchaseReturn(string id)
        {
            PurchaseReturn ca = new PurchaseReturn();
            ca.Brlst = BindBranch();
            ca.Suplst = BindSupplier();
            ca.Curlst = BindCurrency();
            ca.Loclst = GetLoc();
            List<RetItem> TData = new List<RetItem>();
            RetItem tda = new RetItem();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new RetItem();
                    tda.POlst = BindPOlist();
                    //tda.Itemlst = BindItemlst();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            ca.RetLst = TData;
            return View(ca);

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

        public List<SelectListItem> BindCurrency()
        {
            try
            {
                DataTable dtDesg = datatrans.GetCurency();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["Cur"].ToString(), Value = dtDesg.Rows[i]["CURRENCYID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindSupplier()
        {
            try
            {
                DataTable dtDesg = datatrans.GetSupplier();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTY"].ToString(), Value = dtDesg.Rows[i]["PARTYMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> GetLoc()
        {
            try
            {
                DataTable dtDesg = datatrans.GetLocation();


                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOCID"].ToString(), Value = dtDesg.Rows[i]["LOCDETAILSID"].ToString() });
                }
                return lstdesg;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindPOlist()
        {
            try
            {
                DataTable dtDesg = PurReturn.GetPO();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["POBASICID"].ToString(), Value = dtDesg.Rows[i]["POBASICID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public List<SelectListItem> BindItemlst()
        //{
        //    try
        //    {
        //        DataTable dtDesg = PurReturn.GetItem();
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        for (int i = 0; i < dtDesg.Rows.Count; i++)
        //        {
        //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["ITEMMASTERID"].ToString() });
        //        }
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public ActionResult GetItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string unit = "";
                string CF = "";
                string rate = "";
                string item = "";
                string Qty = "";
                string Amount = "";
                string Disc = "";
                string DiscAmount = "";
                string Frig = "";
                string CGS = "";
                string CGTA = "";
                string SGS = "";
                string SGTA = "";
                string IGS = "";
                string IGSTA = "";
                string TotalAm = "";
                dt = PurReturn.GetPODetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    unit = dt.Rows[0]["UNIT"].ToString();
                    rate = dt.Rows[0]["RATE"].ToString();
                    //dt1 = PurReturn.GetItemCF(ItemId, dt.Rows[0]["UNITMASTID"].ToString());
                   
                        CF = dt.Rows[0]["CF"].ToString();
                    item = dt.Rows[0]["ITEMID"].ToString();
                    Qty = dt.Rows[0]["QTY"].ToString();
                    Amount = dt.Rows[0]["AMOUNT"].ToString();
                    Disc = dt.Rows[0]["DISCPER"].ToString();
                    DiscAmount = dt.Rows[0]["DISCAMT"].ToString();
                    Frig = dt.Rows[0]["FREIGHTCHGS"].ToString();
                    CGS = dt.Rows[0]["CGSTPER"].ToString();
                    CGTA = dt.Rows[0]["CGSTAMT"].ToString();
                    SGS = dt.Rows[0]["SGSTPER"].ToString();
                    SGTA = dt.Rows[0]["SGSTAMT"].ToString();
                    IGS = dt.Rows[0]["IGSTPER"].ToString();
                    IGSTA = dt.Rows[0]["IGSTAMT"].ToString();
                    TotalAm = dt.Rows[0]["TOTALAMT"].ToString();
                }

                var result = new { unit = unit, CF = CF, rate = rate, item = item, Qty = Qty, Amount = Amount , Disc = Disc, DiscAmount = DiscAmount, Frig = Frig, CGS = CGS, CGTA = CGTA, SGS = SGS, SGTA = SGTA, IGS = IGS, IGSTA = IGSTA, TotalAm = TotalAm };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
