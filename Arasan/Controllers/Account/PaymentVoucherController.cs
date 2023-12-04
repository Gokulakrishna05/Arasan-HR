using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Models;
using Oracle.ManagedDataAccess.Client;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

using Newtonsoft.Json.Linq;


namespace Arasan.Controllers
{ 
    public class PaymentVoucherController : Controller
    {
    IPaymentVoucher Voucher;
    IConfiguration? _configuratio;
    private string? _connectionString;

    DataTransactions datatrans;
    public PaymentVoucherController(IPaymentVoucher _Voucher, IConfiguration _configuratio)
    {
        Voucher = _Voucher;
        _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        datatrans = new DataTransactions(_connectionString);
    }
    public IActionResult PaymentVoucher(string id)
        {
            PaymentVoucher pv = new PaymentVoucher();
            var userId = Request.Cookies["UserId"];
            pv.Branch = Request.Cookies["BranchId"];
            DataTable dtv = datatrans.GetSequence("vouch"); 
            pv.Brlst = BindBranch();
            pv.Loclst = GetLoc(userId);
            pv.Curlst = BindCurrency();
            DataTable dtv1 = Voucher.GetVoucher();
            pv.VType = dtv1.Rows[0]["DESCRIPTION"].ToString();
            List<VoucherItem> TData = new List<VoucherItem>();
            VoucherItem tda = new VoucherItem();
            pv.Vdate = DateTime.Now.ToString("dd-MMM-yyyy");
            pv.RefDate=DateTime.Now.ToString("dd-MMM-yyyy");
            pv.Currency = "1";
            pv.ExRate = "1";
            pv.PType = "CASH";
            //DataTable dtParty = datatrans.GetData("select P.ACCOUNTNAME from GRNBLBASIC G,PARTYMAST P where G.PARTYID=P.PARTYMASTID AND G.GRNBLBASICID='" + grnid + "'");
            //string mid = dtParty.Rows[0]["ACCOUNTNAME"].ToString();
            //pv.Ledgername = dtParty.Rows[0]["ACCOUNTNAME"].ToString();
            //pv.LedgerId = mid;
            if (dtv.Rows.Count > 0)
            {
                pv.VoucherNo = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
            if (id == null)
            {
               
                //for (int i = 0; i < 3; i++)
                //{
                //    tda = new VoucherItem();
                //    tda.Creditlst = BindCredit();

                //    tda.Isvalid = "Y";
                //    TData.Add(tda);
                //}

            }
            else
            {
                
                DataTable dt = Voucher.EditVoucher(id);
                if (dt.Rows.Count > 0)
                {
                   
                    pv.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    pv.Supplier = dt.Rows[0]["PARTYNAME"].ToString();
                    pv.Grn = dt.Rows[0]["PO_OR_GRN"].ToString();
                    pv.RefNo= dt.Rows[0]["PO_OR_GRN"].ToString();
                    pv.ReqAmount = Convert.ToDouble(dt.Rows[0]["REQUESTAMOUNT"].ToString() == "" ? "0" : dt.Rows[0]["REQUESTAMOUNT"].ToString());
                    pv.TotalAmount = Convert.ToDouble(dt.Rows[0]["AMOUNT"].ToString() == "" ? "0" : dt.Rows[0]["AMOUNT"].ToString());
                }
                DataTable dt2 = new DataTable();
                dt2 = Voucher.GetVoucherDet(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new VoucherItem();
                       // tda.Creditlst = BindCredit();
                        tda.Acclst = BindLedger();
                        //tda.DrumNolst = BindDrumNo(ca.DrumLoc);
                        tda.Credit = dt2.Rows[i]["ACCTYPE"].ToString();
                        tda.Account = dt2.Rows[i]["ACCNAME"].ToString();
                        //tda.Batchlst = BindBatch(tda.DrumNo);


                        tda.Isvalid = "Y";
                        tda.CreditAmount = Convert.ToDouble(dt2.Rows[0]["CREDIT_AMOUNT"].ToString() == "" ? "0" : dt2.Rows[0]["CREDIT_AMOUNT"].ToString());
                        tda.DepitAmount = Convert.ToDouble(dt2.Rows[0]["DEPIT_AMOUNT"].ToString() == "" ? "0" : dt2.Rows[0]["DEPIT_AMOUNT"].ToString());
                     

                        tda.ID = id;
                        TData.Add(tda);
                    }

                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        tda = new VoucherItem();
                       // tda.Creditlst = BindCredit();
                        tda.Acclst = BindLedger();
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }
            }
            pv.VoucherLst=TData;
                return View(pv);
        }
        public IActionResult PaymentVoc(string id)
        {
            PaymentVoucher pv = new PaymentVoucher();
            var userId = Request.Cookies["UserId"];
            pv.Branch = Request.Cookies["BranchId"];
            DataTable dtv = datatrans.GetSequence("vouch");
            pv.Brlst = BindBranch();
            pv.Loclst = GetLoc(userId);
            pv.Curlst = BindCurrency();
            DataTable dtv1 = Voucher.GetVoucher();
            pv.VType = dtv1.Rows[0]["DESCRIPTION"].ToString();
            List<VoucherItem> TData = new List<VoucherItem>();
            VoucherItem tda = new VoucherItem();
            pv.Vdate = DateTime.Now.ToString("dd-MMM-yyyy");
            pv.Currency = "1";
            pv.ExRate = "1";
            pv.PType = "CASH";
            //DataTable dtParty = datatrans.GetData("select P.ACCOUNTNAME from GRNBLBASIC G,PARTYMAST P where G.PARTYID=P.PARTYMASTID AND G.GRNBLBASICID='" + grnid + "'");
            //string mid = dtParty.Rows[0]["ACCOUNTNAME"].ToString();
            //pv.Ledgername = dtParty.Rows[0]["ACCOUNTNAME"].ToString();
            //pv.LedgerId = mid;
            if (dtv.Rows.Count > 0)
            {
                pv.VoucherNo = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
            if (id == null)
            {

                //for (int i = 0; i < 3; i++)
                //{
                //    tda = new VoucherItem();
                //    tda.Creditlst = BindCredit();

                //    tda.Isvalid = "Y";
                //    TData.Add(tda);
                //}

            }
            else
            {

                DataTable dt = Voucher.EditVoucher(id);
                if (dt.Rows.Count > 0)
                {

                    pv.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    pv.Supplier = dt.Rows[0]["PARTYNAME"].ToString();
                    pv.Grn = dt.Rows[0]["PO_OR_GRN"].ToString();
                    pv.ReqAmount = Convert.ToDouble(dt.Rows[0]["REQUESTAMOUNT"].ToString() == "" ? "0" : dt.Rows[0]["REQUESTAMOUNT"].ToString());
                    pv.TotalAmount = Convert.ToDouble(dt.Rows[0]["AMOUNT"].ToString() == "" ? "0" : dt.Rows[0]["AMOUNT"].ToString());
                }
                DataTable dt2 = new DataTable();
                dt2 = Voucher.GetVoucherDet(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new VoucherItem();
                        // tda.Creditlst = BindCredit();
                        tda.Acclst = BindLedger();
                        //tda.DrumNolst = BindDrumNo(ca.DrumLoc);
                        tda.Credit = dt2.Rows[i]["ACCTYPE"].ToString();
                        tda.Account = dt2.Rows[i]["ACCNAME"].ToString();
                        //tda.Batchlst = BindBatch(tda.DrumNo);


                        tda.Isvalid = "Y";
                        tda.CreditAmount = Convert.ToDouble(dt2.Rows[0]["CREDIT_AMOUNT"].ToString() == "" ? "0" : dt2.Rows[0]["CREDIT_AMOUNT"].ToString());
                        tda.DepitAmount = Convert.ToDouble(dt2.Rows[0]["DEPIT_AMOUNT"].ToString() == "" ? "0" : dt2.Rows[0]["DEPIT_AMOUNT"].ToString());


                        tda.ID = id;
                        TData.Add(tda);
                    }

                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        tda = new VoucherItem();
                        // tda.Creditlst = BindCredit();
                        tda.Acclst = BindLedger();
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }
            }
            pv.VoucherLst = TData;
            return View(pv);
        }
        [HttpPost]
        public ActionResult PaymentVoucher(PaymentVoucher Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = Voucher.PaymentCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "PaymentVoucher Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "PaymentVoucher Updated Successfully...!";
                    }
                    return RedirectToAction("ListPaymentVoucher");
                }

                else
                {
                    ViewBag.PageTitle = "Edit PaymentVoucher";
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
        public List<SelectListItem> BindCredit()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "Dr", Value = "Dr" });
                lstdesg.Add(new SelectListItem() { Text = "Cr", Value = "Cr" });

                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindLedger()
        {
            try
            {
                DataTable dtDesg = datatrans.GetLedger();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DISPLAY_NAME"].ToString(), Value = dtDesg.Rows[i]["LEDGERID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
        public List<SelectListItem> GetLoc(string id)
        {
            try
            {
                DataTable dtDesg = Voucher.GetLocation(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOCID"].ToString(), Value = dtDesg.Rows[i]["loc"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetAccountJSON()
        {
           
            return Json(BindLedger());

        }
        public IActionResult ListPaymentVoucher()
        {
            IEnumerable<PaymentVoucher> cmp = Voucher.GetAllVoucher();
            return View(cmp);
        }
    }
}
