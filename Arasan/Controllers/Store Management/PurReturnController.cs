using System.Collections.Generic;
using Arasan.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Models;
using System.Xml.Linq;
using Arasan.Services.Master;

namespace Arasan.Controllers 
{
    public class PurReturnController : Controller
    {
        IPurchaseReturn PurReturn;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public PurReturnController(IPurchaseReturn _PurchaseReturn, IConfiguration _configuratio)
        {
            PurReturn = _PurchaseReturn;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult PurchaseRet(string id)
        {
            PurchaseReturn ca = new PurchaseReturn();
            ca.Brlst = BindBranch();
            ca.Suplst = BindSupplier();
            ca.Curlst = BindCurrency();
            ca.Loclst = GetLoc();
            ca.Satlst = GetSat();
            ca.Citylst = BindCity("");
            List<RetItem> TData = new List<RetItem>();
            RetItem tda = new RetItem();
           
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new RetItem();
                    tda.POlst = BindGRNlist();
                    //tda.Itemlst = BindItemlst();

                    TData.Add(tda);
                }
               
            }
            else
            {

                // ca = directPurchase.GetDirectPurById(id);


                DataTable dt = new DataTable();
              
                dt = PurReturn.GetPurchaseReturn(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();

                    ca.Supplier = dt.Rows[0]["PARTYID"].ToString();
                    ca.RetNo = dt.Rows[0]["DOCID"].ToString();
                    ca.RetDate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.ID = id;
                    ca.Currency = dt.Rows[0]["MAINCURRENCY"].ToString();
                    ca.ReqNo = dt.Rows[0]["REFNO"].ToString();
                    ca.ReqDate = dt.Rows[0]["REFDT"].ToString();
                    ca.Reason = dt.Rows[0]["REASONCODE"].ToString();
                    ca.Location = dt.Rows[0]["LOCID"].ToString();
                    ca.ExRate = dt.Rows[0]["EXCHANGERATE"].ToString();
                    ca.Rej = dt.Rows[0]["REJBY"].ToString();
                    ca.Temp = dt.Rows[0]["TEMPFIELD"].ToString();
                    ca.Trans = dt.Rows[0]["TRANSITLOCID"].ToString();
                    ca.Grn = dt.Rows[0]["RGRNNO"].ToString();
                    ca.Narration = dt.Rows[0]["NARR"].ToString();
                    //ca.LRCha = Convert.ToDouble(dt.Rows[0]["LRCH"].ToString() == "" ? "0" : dt.Rows[0]["LRCH"].ToString());
                    //ca.DelCh = Convert.ToDouble(dt.Rows[0]["DELCH"].ToString() == "" ? "0" : dt.Rows[0]["DELCH"].ToString());
                    //ca.Other = Convert.ToDouble(dt.Rows[0]["OTHERCH"].ToString() == "" ? "0" : dt.Rows[0]["OTHERCH"].ToString());
                    //ca.Frig = Convert.ToDouble(dt.Rows[0]["FREIGHT"].ToString() == "" ? "0" : dt.Rows[0]["FREIGHT"].ToString());
                    //ca.SpDisc = Convert.ToDouble(dt.Rows[0]["OTHERDISC"].ToString() == "" ? "0" : dt.Rows[0]["OTHERDISC"].ToString());

                    ca.Gross = Convert.ToDouble(dt.Rows[0]["GROSS"].ToString() == "" ? "0" : dt.Rows[0]["GROSS"].ToString());
                    ca.Net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());

                }
                DataTable dt2 = new DataTable();
                dt2 = PurReturn.GetPurchaseReturnDes(id);
                if (dt2.Rows.Count > 0)
                {
                 
                        ca.Addr = dt2.Rows[0]["SADD1"].ToString();

                        ca.City = dt2.Rows[0]["SCITY"].ToString();
                        ca.State = dt2.Rows[0]["SSTATE"].ToString();
                        ca.Pin = dt2.Rows[0]["SPINCODE"].ToString();
                        ca.ID = id;
                        ca.Phone = dt2.Rows[0]["SPHONE"].ToString();
                      
                }
                DataTable dt3 = new DataTable();
                dt3 = PurReturn.GetPurchaseReturnReason(id);
                if (dt3.Rows.Count > 0)
                {

                    ca.Reason = dt3.Rows[0]["REASON"].ToString();

                   
                   

                }
            }
           
                ca.RetLst = TData;
            return View(ca);

        }
        [HttpPost]
        public ActionResult PurchaseRet(PurchaseReturn Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = PurReturn.PurReturnCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "PurchaseRet Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "PurchaseRet Updated Successfully...!";
                    }
                    return RedirectToAction("ListPurchaseReturn");
                }

                else
                {
                    ViewBag.PageTitle = "Edit PurchaseRet";
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
        public JsonResult GetCityJSON(string ItemId)
        {
            PurchaseReturn model = new PurchaseReturn();
            model.Citylst = BindCity(ItemId);
            return Json(BindCity(ItemId));

        }
        public List<SelectListItem> BindCity(string ItemId)
        {
            try
            {
                DataTable dtDesg = PurReturn.GetCity(ItemId);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["CITYNAME"].ToString(), Value = dtDesg.Rows[i]["CITYID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindGRNlist()
        {
            try
            {
                DataTable dtDesg = PurReturn.GetGRN();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["GRNBLBASICID"].ToString() });
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

        public ActionResult GetGRNDetail(string POID)
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
             
               
                dt = PurReturn.GetGRNDetails(POID);
             
                if (dt.Rows.Count > 0)
                {

                    unit = dt.Rows[0]["UNITID"].ToString();
                    CF = dt.Rows[0]["CF"].ToString();
                    qty = dt.Rows[0]["QTY"].ToString();
                    rate = dt.Rows[0]["RATE"].ToString();
                    //dt1 = PurReturn.GetItemCF(ItemId, dt.Rows[0]["UNITMASTID"].ToString());
                    amount = dt.Rows[0]["AMOUNT"].ToString();

                    item = dt.Rows[0]["ITEMID"].ToString();
                    discAmount = dt.Rows[0]["DISC"].ToString();

                    // Disc = dt.Rows[0]["DISCPER"].ToString();

                    frig = dt.Rows[0]["IFREIGHTCH"].ToString();
                    cgs = dt.Rows[0]["CGSTPER"].ToString();
                    cgta = dt.Rows[0]["CGSTAMT"].ToString();
                    sgs = dt.Rows[0]["SGSTPER"].ToString();
                    sgta = dt.Rows[0]["SGSTAMT"].ToString();
                    igs = dt.Rows[0]["IGSTPER"].ToString();
                    igsta = dt.Rows[0]["IGSTAMT"].ToString();
                    totalAm = dt.Rows[0]["TOTAMT"].ToString();
                 
                }
               
                    var result = new { unit = unit, CF = CF, qty = qty, rate = rate, amount = amount, item = item, discAmount = discAmount, frig = frig, cgs = cgs, cgta = cgta, sgs = sgs, sgta = sgta, igs = igs, igsta = igsta, totalAm = totalAm };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult ListPurchaseReturn()
        {
            IEnumerable<PurchaseReturn> cmp = PurReturn.GetAllPurReturn();
            return View(cmp);
        }
        public JsonResult GetItemGrpJSON()
        {
            PurchaseReturn model = new PurchaseReturn();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(model);
        }
    }
}
