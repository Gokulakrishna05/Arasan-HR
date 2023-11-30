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
            ca.Branch = Request.Cookies["BranchId"];
            ca.Location = Request.Cookies["LocationId"];
            ca.Loclst = GetLoc();
            ca.Satlst = GetSat();
            ca.assignList = BindEmp();
            ca.Citylst = BindCity("");
            ca.POlst = BindGRNlist();
            ca.Partylst = Bindpartylist();
            ca.currlst = Bindcurrlist();
          
           
            ca.ReqDate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.RetDate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("PURRE");
            if (dtv.Rows.Count > 0)
            {
                ca.RetNo = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
            List<RetItem> TData = new List<RetItem>();
            RetItem tda = new RetItem();

            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new RetItem();

                    //tda.Itemlst = BindItemlst("");

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

                    ca.Trans = dt.Rows[0]["TRANSITLOCID"].ToString();
                    ca.Grn = dt.Rows[0]["RGRNNO"].ToString();
                    ca.Narration = dt.Rows[0]["NARR"].ToString();
                    //ca.LRCha = Convert.ToDouble(dt.Rows[0]["LRCH"].ToString() == "" ? "0" : dt.Rows[0]["LRCH"].ToString());
                    //ca.DelCh = Convert.ToDouble(dt.Rows[0]["DELCH"].ToString() == "" ? "0" : dt.Rows[0]["DELCH"].ToString());
                    //ca.Other = Convert.ToDouble(dt.Rows[0]["OTHERCH"].ToString() == "" ? "0" : dt.Rows[0]["OTHERCH"].ToString());
                    //ca.Frig = Convert.ToDouble(dt.Rows[0]["FREIGHT"].ToString() == "" ? "0" : dt.Rows[0]["FREIGHT"].ToString());
                    //ca.SpDisc = Convert.ToDouble(dt.Rows[0]["OTHERDISC"].ToString() == "" ? "0" : dt.Rows[0]["OTHERDISC"].ToString());

                    //ca.Gross = Convert.ToDouble(dt.Rows[0]["GROSS"].ToString() == "" ? "0" : dt.Rows[0]["GROSS"].ToString());
                    //ca.Net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());
                    ca.Gross = dt.Rows[0]["GROSS"].ToString();
                    ca.Net = dt.Rows[0]["NET"].ToString();
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
                DataTable dt4 = new DataTable();





                }
                //DataTable dt4 = new DataTable();

                //dt4 = PurReturn.GetPurchaseReturnDetail(id);
                //if (dt4.Rows.Count > 0)
                //{

                //    for (int i = 0; i < dt4.Rows.Count; i++)
                //    {
                //        tda = new RetItem();

                //        //tda.Itemlst = BindItemlst(ca.Grn);
                //        tda.ItemId = dt4.Rows[i]["ITEMID"].ToString();
                //        tda.saveItemId = dt4.Rows[i]["ITEMID"].ToString();
                //        tda.rate = dt4.Rows[i]["RATE"].ToString();
                //        tda.Amount = dt4.Rows[i]["AMOUNT"].ToString();
                //        tda.Quantity = dt4.Rows[i]["QTY"].ToString();
                //        tda.binid = dt4.Rows[i]["BINID"].ToString();
                //        tda.Current = dt4.Rows[i]["CLSTOCK"].ToString();
                //        tda.Return = dt4.Rows[i]["PRIQTY"].ToString();
                //        tda.TotalAmount = dt4.Rows[i]["TOTAMT"].ToString();
                //        tda.ConFac = dt4.Rows[i]["CF"].ToString();
                //        tda.Unit = dt4.Rows[i]["UNITID"].ToString();
                //        tda.CGSTPer = Convert.ToDouble(dt4.Rows[i]["CGSTPER"].ToString() == "" ? "0" : dt4.Rows[i]["CGSTPER"].ToString());
                //        tda.SGSTPer = Convert.ToDouble(dt4.Rows[i]["SGSTPER"].ToString() == "" ? "0" : dt4.Rows[i]["SGSTPER"].ToString());
                //        tda.IGSTPer = Convert.ToDouble(dt4.Rows[i]["IGSTPER"].ToString() == "" ? "0" : dt4.Rows[i]["IGSTPER"].ToString());
                //        tda.CGSTAmt = Convert.ToDouble(dt4.Rows[i]["CGSTAMT"].ToString() == "" ? "0" : dt4.Rows[i]["CGSTAMT"].ToString());
                //        tda.SGSTAmt = Convert.ToDouble(dt4.Rows[i]["SGSTAMT"].ToString() == "" ? "0" : dt4.Rows[i]["SGSTAMT"].ToString());
                //        tda.IGSTAmt = Convert.ToDouble(dt4.Rows[i]["IGSTAMT"].ToString() == "" ? "0" : dt4.Rows[i]["IGSTAMT"].ToString());
                //        tda.Isvalid = "Y";
                //        TData.Add(tda);
                //    }
                //}
 
            
 
            //}
 
           
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

        public JsonResult GetGRNCurrencyJSON(string suppid)
        {
            PurchaseReturn model = new PurchaseReturn();
            model.Curlst = BindCurrency(suppid);
            return Json(BindCurrency(suppid));

        }
        public List<SelectListItem> BindCurrency(string id)
        {
            try
            {
                DataTable dtDesg = PurReturn.GetCurrency(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["MAINCURR"].ToString(), Value = dtDesg.Rows[i]["CURRENCYID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetGRNSuppJSON(string suppid)
        {
            PurchaseReturn model = new PurchaseReturn();
            model.Suplst = BindSupplier(suppid);
            return Json(BindSupplier(suppid));

        }
        public List<SelectListItem> BindSupplier(string id)
        {
            try
            {
                DataTable dtDesg = PurReturn.GetSupplier(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTYNAME"].ToString(), Value = dtDesg.Rows[i]["PARTYMASTID"].ToString() });
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
        public List<SelectListItem> Bindpartylist()
        {
            try
            {
                DataTable dtDesg = PurReturn.Getparty();


                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTYNAME"].ToString(), Value = dtDesg.Rows[i]["PARTYMASTID"].ToString() });
                }
                return lstdesg;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        } public List<SelectListItem> Bindcurrlist()
        {
            try
            {
                DataTable dtDesg = PurReturn.Getcurr();


                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["MAINCURR"].ToString(), Value = dtDesg.Rows[i]["CURRENCYID"].ToString() });
                }
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
        //public JsonResult GetGRNItemJSON(string supid)
        //{
        //    RetItem model = new RetItem();
        //    model.Itemlst = BindItemlst(supid);
        //    return Json(BindItemlst(supid));

        //}
        public JsonResult GetPRJSON(string grnid,string branch,string loc)
        {
            PurchaseReturn model = new PurchaseReturn();
            DataTable dtt = new DataTable();
            List<ReturnItem> Data = new List<ReturnItem>();
            ReturnItem tda = new ReturnItem();
            dtt = PurReturn.GetGRNDetails(grnid);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new ReturnItem();
                    tda.itemid = dtt.Rows[i]["itemi"].ToString();
                    tda.itemname = dtt.Rows[i]["ITEMID"].ToString();
                    tda.unit = dtt.Rows[i]["UNITID"].ToString();
                    tda.quantity = dtt.Rows[i]["QTY"].ToString();
                    tda.rqty= dtt.Rows[i]["QTY"].ToString();
                    tda.confac = dtt.Rows[i]["CF"].ToString();
                    tda.rate= dtt.Rows[i]["RATE"].ToString();
                    tda.amount = dtt.Rows[i]["AMOUNT"].ToString();
                    tda.disc= Convert.ToDouble(dtt.Rows[i]["DISCPER"].ToString());
                    tda.discAmount= Convert.ToDouble(dtt.Rows[i]["DISC"].ToString());
                    tda.frigcharge= Convert.ToDouble( dtt.Rows[i]["IFREIGHTCH"].ToString() == "" ? "0" :dtt.Rows[i]["IFREIGHTCH"].ToString());
                    tda.cgstamt= Convert.ToDouble(dtt.Rows[i]["CGST"].ToString() == "" ? "0" : dtt.Rows[i]["CGST"].ToString());
                    tda.cgstper= Convert.ToDouble(dtt.Rows[i]["CGSTP"].ToString() == "" ? "0" : dtt.Rows[i]["CGSTP"].ToString());
                    tda.sgstamt= Convert.ToDouble( dtt.Rows[i]["SGST"].ToString() == "" ? "0" : dtt.Rows[i]["SGST"].ToString());
                    tda.sgstper= Convert.ToDouble(dtt.Rows[i]["SGSTP"].ToString() == "" ? "0" : dtt.Rows[i]["SGSTP"].ToString());
                    tda.igstamt= Convert.ToDouble(dtt.Rows[i]["IGST"].ToString() == "" ? "0" : dtt.Rows[i]["IGST"].ToString());
                    tda.igstper= Convert.ToDouble( dtt.Rows[i]["IGSTP"].ToString() == "" ? "0" : dtt.Rows[i]["IGSTP"].ToString());
                    tda.totalamount= Convert.ToDouble(dtt.Rows[i]["TOTAMT"].ToString()=="" ? "0" : dtt.Rows[i]["TOTAMT"].ToString());
                    tda.binid= dtt.Rows[i]["BINID"].ToString();
                    tda.binid = "0";
                    
                    tda.unitid= dtt.Rows[i]["UNIT"].ToString();
                   
                    DataTable dt = new DataTable();
                    dt = PurReturn.Getstkqty(grnid, "10001000000827", branch);
                    if(dt.Rows.Count > 0)
                    {
                        tda.stkqty = dt.Rows[0]["QTY"].ToString();
                    }
                    if(tda.stkqty=="")
                    {
                        tda.stkqty = "0";
                    }
                   
                    Data.Add(tda);
                }
            }
            model.returnlist = Data;
            return Json(model.returnlist);

        }
       

        public ActionResult GetStkqty(string grnid,string loc,string branch)
        {
            try
            {
                DataTable dt = new DataTable();

                string stkqty = "";

                dt = PurReturn.Getstkqty(grnid, loc, branch);
                if (dt.Rows.Count > 0)
                {
                    stkqty = dt.Rows[0]["QTY"].ToString();
                }

                var result = new { stkqty = stkqty };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetGRNBL(string GRNID)
        {
            try
            {
                DataTable dt = new DataTable();

                string party = "";
                string currency = "";
                string currencyid = "";
                string ex = "";
                string frig = "";
                string other = "";

                string roundoffplus = "";
                string roundoffmin = "";
            
                string otherdedu = "";
                string gross = "";
                string net = "";
                string packing = "";


                dt = PurReturn.GetGRNBlDetails(GRNID);

                if (dt.Rows.Count > 0)
                {
                    party = dt.Rows[0]["PARTYNAME"].ToString();
                    currency = dt.Rows[0]["MAINCURR"].ToString();
                    currencyid = dt.Rows[0]["MAINCURRENCY"].ToString();
                    ex = dt.Rows[0]["EXRATE"].ToString();
                    frig = dt.Rows[0]["FREIGHT"].ToString();
                    other = dt.Rows[0]["OTHER_CHARGES"].ToString();
                    roundoffplus = dt.Rows[0]["ROUND_OFF_PLUS"].ToString();
                    //dt1 = PurReturn.GetItemCF(ItemId, dt.Rows[0]["UNITMASTID"].ToString());
                    roundoffmin = dt.Rows[0]["ROUND_OFF_MINUS"].ToString();
                    packing= dt.Rows[0]["PACKING_CHRAGES"].ToString();
                    otherdedu = dt.Rows[0]["OTHER_DEDUCTION"].ToString();
                    gross = dt.Rows[0]["GROSS"].ToString();
                    net = dt.Rows[0]["NET"].ToString();
                   

                }

                var result = new { ex = ex, frig = frig, other = other, roundoffplus = roundoffplus, roundoffmin = roundoffmin, otherdedu = otherdedu, gross = gross, net = net, packing = packing, party= party, currency= currency , currencyid = currencyid };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetGRNDetail(string POID)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dtt = new DataTable();

                string unit = "";
                string CF = "";
                string rate = "";
              
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
                string packing = "";


                dt = PurReturn.GetGRNDetails(POID);
             
                if (dt.Rows.Count > 0)
                {

                    unit = dt.Rows[0]["UNITID"].ToString();
                    CF = dt.Rows[0]["CF"].ToString();
                    qty = dt.Rows[0]["QTY"].ToString();
                    rate = dt.Rows[0]["RATE"].ToString();
                    //dt1 = PurReturn.GetItemCF(ItemId, dt.Rows[0]["UNITMASTID"].ToString());
                    amount = dt.Rows[0]["AMOUNT"].ToString();
                    packing= dt.Rows[0]["AMOUNT"].ToString();

                    //discAmount = dt.Rows[0]["DISC"].ToString();

                    // Disc = dt.Rows[0]["DISCPER"].ToString();

                    //frig = dt.Rows[0]["IFREIGHTCH"].ToString();
                    cgs = dt.Rows[0]["CGSTPER"].ToString();
                    cgta = dt.Rows[0]["CGSTAMT"].ToString();
                    sgs = dt.Rows[0]["SGSTPER"].ToString();
                    sgta = dt.Rows[0]["SGSTAMT"].ToString();
                    igs = dt.Rows[0]["IGSTPER"].ToString();
                    igsta = dt.Rows[0]["IGSTAMT"].ToString();
                    totalAm = dt.Rows[0]["TOTAMT"].ToString();
                 
                }
               
                    var result = new { unit = unit, CF = CF, qty = qty, rate = rate, amount = amount, discAmount = discAmount, frig = frig, cgs = cgs, cgta = cgta, sgs = sgs, sgta = sgta, igs = igs, igsta = igsta, totalAm = totalAm , packing = packing };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult ListPurchaseReturn()
        {
            //IEnumerable<PurchaseReturn> cmp = PurReturn.GetAllPurReturn);
            return View();
        }
        public ActionResult MyListPurchaseReturnGrid(string strStatus)
        {
            List<PurchaseReturnItems> Reg = new List<PurchaseReturnItems>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)PurReturn.GetAllPurchaseReturnItems(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
               
                string Create = string.Empty;
                string View = string.Empty;
                //string EditRow = string.Empty;
                string DeleteRow = string.Empty;

                
                if (dtUsers.Rows[i]["STATUS"].ToString()=="DN RAISED")
                {
                    Create = "";
                }
                else
                {
                    Create = "<a href=/DebitNoteBill/DebitNoteBill?id=" + dtUsers.Rows[i]["PRETBASICID"].ToString() + " tag='1'><img src='../Images/move_quote.png' alt='View Details' width='20' /></a>";
                }
                View = "<a href=viewPurchaseReturn?id=" + dtUsers.Rows[i]["PRETBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View Details' width='20' /></a>";
                //EditRow = "<a href=PurchaseRet?id=" + dtUsers.Rows[i]["PRETBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                DeleteRow = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["PRETBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";

                Reg.Add(new PurchaseReturnItems
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["PRETBASICID"].ToString()),
                    branch = dtUsers.Rows[i]["BRANCHID"].ToString(),
                    docNo = dtUsers.Rows[i]["DOCID"].ToString(),
                    docDate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    curr = dtUsers.Rows[i]["MAINCURR"].ToString(),
                    create = Create,
                    view = View,
                    //editrow = EditRow,
                    delrow = DeleteRow,



                }); 
            }

            return Json(new
            {
                Reg
            });

        }
        public ActionResult DeleteItem(string tag, string id)
        {

            string flag = PurReturn.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListPurchaseReturn");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListPurchaseReturn");
            }
        }
        public JsonResult GetItemGrpJSON()
        {
            //RetItem model = new RetItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindGRNlist());
        }
      

        public IActionResult viewPurchaseReturn(string id)
        {
            PurchaseReturn ca = new PurchaseReturn();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();

            dt = PurReturn.GetviewPurchaseReturn(id);
            if (dt.Rows.Count > 0)
            {
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();

                ca.Supplier = dt.Rows[0]["PARTYNAME"].ToString();
                ca.RetNo = dt.Rows[0]["DOCID"].ToString();
                ca.RetDate = dt.Rows[0]["DOCDATE"].ToString();
                ca.ID = id;
                ca.Currency = dt.Rows[0]["MAINCURR"].ToString();
                ca.ReqNo = dt.Rows[0]["REFNO"].ToString();
                ca.ReqDate = dt.Rows[0]["REFDT"].ToString();
                ca.Reason = dt.Rows[0]["REASONCODE"].ToString();
                ca.Location = dt.Rows[0]["LOCID"].ToString();
                ca.ExRate = dt.Rows[0]["EXCHANGERATE"].ToString();
                ca.Rej = dt.Rows[0]["EMPNAME"].ToString();
               

                ca.Trans = dt.Rows[0]["LOCID"].ToString();
                ca.Grn = dt.Rows[0]["RGRNNO"].ToString();
                ca.Narration = dt.Rows[0]["NARR"].ToString();
                ca.Addr = dt.Rows[0]["AREA"].ToString();
                ca.Address = dt.Rows[0]["ADDRESS"].ToString();
                ca.City = dt.Rows[0]["CITY"].ToString();
                ca.State = dt.Rows[0]["STATE"].ToString();
                ca.Pin = dt.Rows[0]["PINCODE"].ToString();
                ca.Phone = dt.Rows[0]["PHONE"].ToString();
                
                //ca.Gross = Convert.ToDouble(dt.Rows[0]["GROSS"].ToString());
                //ca.Net = Convert.ToDouble(dt.Rows[0]["NET"].ToString());

                ca.Gross = dt.Rows[0]["GROSS"].ToString();
                ca.Net = dt.Rows[0]["NET"].ToString();
                ca.ID = id;

                List<RetItem> Data = new List<RetItem>();
                RetItem tda = new RetItem();
                double tot = 0;

                dtt = PurReturn.GetviewPurchaseReturnDetail(id);
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda = new RetItem();
                        tda.ItemId = dtt.Rows[i]["ITEMID"].ToString();
                        tda.rate = dtt.Rows[i]["RATE"].ToString();
                        tda.Amount = dtt.Rows[i]["AMOUNT"].ToString();
                        tda.Quantity = dtt.Rows[i]["QTY"].ToString();
                        tda.binid = dtt.Rows[i]["BINID"].ToString();
                        tda.Current = dtt.Rows[i]["CLSTOCK"].ToString();
                        tda.Return = dtt.Rows[i]["PRIQTY"].ToString();
                        tda.TotalAmount = dtt.Rows[i]["TOTAMT"].ToString();
                        tda.ConFac = dtt.Rows[i]["CF"].ToString();
                        tda.Unit = dtt.Rows[i]["UNITID"].ToString();
                        
                        tda.CGSTPer = Convert.ToDouble(dtt.Rows[i]["CGSTP"].ToString() == "" ? "0" : dtt.Rows[i]["CGSTP"].ToString());
                        tda.SGSTPer = Convert.ToDouble(dtt.Rows[i]["SGSTP"].ToString() == "" ? "0" : dtt.Rows[i]["SGSTP"].ToString());
                        tda.IGSTPer = Convert.ToDouble(dtt.Rows[i]["IGSTP"].ToString() == "" ? "0" : dtt.Rows[i]["IGSTP"].ToString());
                        tda.CGSTAmt = Convert.ToDouble(dtt.Rows[i]["CGST"].ToString() == "" ? "0" : dtt.Rows[i]["CGST"].ToString());
                        tda.SGSTAmt = Convert.ToDouble(dtt.Rows[i]["SGST"].ToString() == "" ? "0" : dtt.Rows[i]["SGST"].ToString());
                        tda.IGSTAmt = Convert.ToDouble(dtt.Rows[i]["IGST"].ToString() == "" ? "0" : dtt.Rows[i]["IGST"].ToString());


                        Data.Add(tda);
                    }
                }

                ca.RetLst = Data;
            }
            return View(ca);
        }
    }
}
