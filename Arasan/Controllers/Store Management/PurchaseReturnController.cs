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
        public PurchaseReturnController(IPurchaseReturn _PurchaseReturn)
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
            ca.Satlst = GetSat();
            List<RetItem> TData = new List<RetItem>();
            RetItem tda = new RetItem();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new RetItem();
                    tda.POlst = BindPOlist();
                    //tda.Itemlst = BindItemlst();
               
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
        public List<SelectListItem> GetSat()
        {
            try
            {
                DataTable dtDesg = PurReturn.GetState();


                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["STATE"].ToString(), Value = dtDesg.Rows[i]["STATEMASTID"].ToString() });
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
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["POBASICID"].ToString() });
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

        public ActionResult GetPODetail(string POID)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dtt = new DataTable();

                string unit = "";
                string CF = "";
                string rate = "";
                string item = "";
                string qty = "";
                string amount = "";
                //string Disc = "";
                string discAmount = "";
                string frig = "";
                string cgs = "";
                string cgta = "";
                string sgs = "";
                string sgta = "";
                string igs = "";
                string igsta = "";
                string totalAm = "";
             
               
                dt = PurReturn.GetPODetails(POID);
             
                if (dt.Rows.Count > 0)
                {

                    unit = dt.Rows[0]["UNITID"].ToString();
                    CF = dt.Rows[0]["CF"].ToString();
                    qty = dt.Rows[0]["QTY"].ToString();
                    rate = dt.Rows[0]["RATE"].ToString();
                    //dt1 = PurReturn.GetItemCF(ItemId, dt.Rows[0]["UNITMASTID"].ToString());
                    amount = dt.Rows[0]["AMOUNT"].ToString();

                    item = dt.Rows[0]["ITEMID"].ToString();
                    discAmount = dt.Rows[0]["DISCAMT"].ToString();

                    // Disc = dt.Rows[0]["DISCPER"].ToString();

                    frig = dt.Rows[0]["FREIGHTCHGS"].ToString();
                    cgs = dt.Rows[0]["CGSTPER"].ToString();
                    cgta = dt.Rows[0]["CGSTAMT"].ToString();
                    sgs = dt.Rows[0]["SGSTPER"].ToString();
                    sgta = dt.Rows[0]["SGSTAMT"].ToString();
                    igs = dt.Rows[0]["IGSTPER"].ToString();
                    igsta = dt.Rows[0]["IGSTAMT"].ToString();
                    totalAm = dt.Rows[0]["TOTALAMT"].ToString();
                 
                }
               
                    var result = new { unit = unit, CF = CF, qty = qty, rate = rate, amount = amount, item = item, discAmount = discAmount, frig = frig, cgs = cgs, cgta = cgta, sgs = sgs, sgta = sgta, igs = igs, igsta = igsta, totalAm = totalAm };
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
