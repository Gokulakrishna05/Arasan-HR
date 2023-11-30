using Arasan.Interface;
using Arasan.Interface.Qualitycontrol;
using Arasan.Interface.Sales;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Qualitycontrol;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Reflection;

namespace Arasan.Controllers.Sales
{
    public class DebitNoteBillController : Controller
    {
        IDebitNoteBillService DebitNoteBillService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public DebitNoteBillController(IDebitNoteBillService _DebitNoteBillService, IConfiguration _configuratio)
        {
            DebitNoteBillService = _DebitNoteBillService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult DebitNoteBill(string id, string tag)
        {
            DebitNoteBill ca = new DebitNoteBill();
            ca.Brlst = BindBranch();
            ca.Partylst = BindGParty();
            ca.Vocherlst = BindVocher();
            ca.Vocher = "R";
            DataTable dtv = datatrans.GetSequence("Dbnot");
            if (dtv.Rows.Count > 0)
            {
                ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
            ca.Location = Request.Cookies["LocationId"];
            ca.Branch = Request.Cookies["BranchId"];
            ca.RefDate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            List<DebitNoteItem> TData = new List<DebitNoteItem>();
            DebitNoteItem tda = new DebitNoteItem();
            if (id == null)
            {
                tda = new DebitNoteItem();
                tda.Grnlst = BindGrnlst("");
                tda.Itemlst = BindItemlst("");
                tda.Invdate = DateTime.Now.ToString("dd-MMM-yyyy");
                //tda.CGSTP = "9";
                //tda.SGSTP = "9";
                //tda.IGSTP = "0";
                tda.Isvalid = "Y";
                TData.Add(tda);

            }
            else
            {
                if (tag == "")
                {
                    DataTable dt = new DataTable();
                    dt = DebitNoteBillService.GetDebitNoteBillDetail(id);
                    if (dt.Rows.Count > 0)
                    {
                        ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                        ca.Vocher = dt.Rows[0]["VTYPE"].ToString();
                        ca.DocId = dt.Rows[0]["DOCID"].ToString();
                        ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                        ca.grnid= id;
                        //ca.RefNo = dt.Rows[0]["REFNO"].ToString();
                        // ca.RefDate = dt.Rows[0]["REFDT"].ToString();
                        ca.Party = dt.Rows[0]["PARTYID"].ToString();
                        ca.Gross = dt.Rows[0]["GROSS"].ToString();
                        ca.Net = dt.Rows[0]["NET"].ToString();
                        ca.PartyBal= dt.Rows[0]["NET"].ToString();
                        ca.Amount = dt.Rows[0]["AMTINWRD"].ToString();
                        //ca.Bigst = Convert.ToDouble(dt2.Rows[0]["CGST"].ToString() == "" ? "0" : dt2.Rows[0]["CGST"].ToString());
                        //ca.Bsgst = Convert.ToDouble(dt2.Rows[0]["SGST"].ToString() == "" ? "0" : dt2.Rows[0]["SGST"].ToString());
                        //ca.Bcgst = Convert.ToDouble(dt2.Rows[0]["CGST"].ToString() == "" ? "0" : dt2.Rows[0]["CGST"].ToString());
                        ca.Narration = dt.Rows[0]["NARRATION"].ToString();



                    }
                    DataTable dt2 = new DataTable();

                    dt2 = DebitNoteBillService.GetDebitNoteBillItem(id);
                    if (dt2.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            tda = new DebitNoteItem();
                            tda.Grnlst = BindGrnlst("");
                            tda.Grnlst = BindGrnlst(ca.Party);
                            tda.InvNo = dt2.Rows[i]["INVNO"].ToString();
                            tda.Invdate = dt2.Rows[i]["INVDT"].ToString();
                            tda.Itemlst = BindItemlst(tda.InvNo);
                            tda.Item = dt2.Rows[i]["ITEMID"].ToString();
                            tda.Cf = dt2.Rows[i]["CONVFACTOR"].ToString();
                            tda.Unit = dt2.Rows[i]["PRIUNIT"].ToString();
                            tda.Qty = dt2.Rows[i]["QTY"].ToString();
                            tda.Rate = dt2.Rows[i]["RATE"].ToString();
                            tda.Amount = dt2.Rows[i]["AMOUNT"].ToString();
                            tda.CGST = dt2.Rows[i]["CGST"].ToString();
                            tda.SGST = dt2.Rows[i]["SGST"].ToString();
                            tda.IGST = dt2.Rows[i]["IGST"].ToString();
                            tda.Total = dt2.Rows[i]["TOTAMT"].ToString();
                            tda.Isvalid = "Y";
                            tda.ID = id;
                            TData.Add(tda);
                        }

                    }
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt = DebitNoteBillService.GetPurRet(id);
                    if (dt.Rows.Count > 0)
                    {
                        ca.Branch = dt.Rows[0]["BRANCHID"].ToString();


                        ca.grnid = id;
                        ca.Party = dt.Rows[0]["PARTYNAME"].ToString();
                        ca.Partyid = dt.Rows[0]["PARTYID"].ToString();
                        ca.Gross = dt.Rows[0]["GROSS"].ToString();
                        ca.Net = dt.Rows[0]["NET"].ToString();
                    //  ca.RefNo= dt.Rows[0]["RGRNNO"].ToString();
                      //  ca.RefDate= dt.Rows[0]["DOCDAT"].ToString();
                        ca.PartyBal= dt.Rows[0]["NET"].ToString();
                        //ca.Bigst = dt.Rows[0]["BIGST"].ToString();
                        //ca.Bsgst = dt.Rows[0]["BSGST"].ToString();
                        //ca.Bcgst = dt.Rows[0]["BCGST"].ToString();




                    }
                    DataTable dt2 = new DataTable();

                    dt2 = DebitNoteBillService.GetPurRetDetail(id);
                    if (dt2.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            tda = new DebitNoteItem();
                            //DataTable dt3 = new DataTable();

                            //dt3 = DebitNoteBillService.GetPurRetDoc(id);
                            //if(dt3.Rows.Count > 0)
                            //{
                            //tda.Grnlst = BindGrnlst("");

                            //tda.InvNo = dt3.Rows[0]["RGRNNO"].ToString();
                            //tda.Invdate = dt3.Rows[0]["DOCDAT"].ToString();
                            //}

                            tda.InvNo= dt.Rows[0]["RGRNNO"].ToString();
                            tda.Invdate= dt.Rows[0]["DOCDAT"].ToString();
                            tda.Item = dt2.Rows[i]["ITEMID"].ToString();
                            tda.Itemid = dt2.Rows[i]["item"].ToString();
                            tda.Cf = dt2.Rows[i]["CF"].ToString();
                            tda.Unit = dt2.Rows[i]["UNITID"].ToString();
                            tda.InQty = dt2.Rows[i]["QTY"].ToString();
                            tda.Rate = dt2.Rows[i]["RATE"].ToString();
                            tda.Amount = dt2.Rows[i]["AMOUNT"].ToString();
                            tda.CGST = dt2.Rows[i]["CGST"].ToString();
                            tda.SGST = dt2.Rows[i]["SGST"].ToString();
                            tda.IGST = dt2.Rows[i]["IGST"].ToString();
                            tda.Total = dt2.Rows[i]["TOTAMT"].ToString();
                            tda.CGSTP = dt2.Rows[i]["CGSTP"].ToString();
                            tda.SGSTP = dt2.Rows[i]["SGSTP"].ToString();
                            tda.IGSTP = dt2.Rows[i]["IGSTP"].ToString();

                            //double ig = ca.Bigst;
                            //double sg = ca.Bsgst;
                            //double cg = ca.Bcgst;
                            //if (dt2.Rows.Count > 1)
                            //{
                            //    i = 0;
                            //    ca.Bigst = Convert.ToDouble(dt2.Rows[i]["CGST"].ToString() == "" ? "0" : dt2.Rows[i]["CGST"].ToString());
                            //    ca.Bsgst = Convert.ToDouble(dt2.Rows[i]["SGST"].ToString() == "" ? "0" : dt2.Rows[i]["SGST"].ToString());
                            //    ca.Bcgst = Convert.ToDouble(dt2.Rows[i]["CGST"].ToString() == "" ? "0" : dt2.Rows[i]["CGST"].ToString());
                            //    double igtot = ig + ca.Bigst;
                            //    double sgtot = sg + ca.Bsgst;
                            //    double cgtot = cg + ca.Bcgst;
                            //    ca.Bigst = igtot;
                            //    ca.Bsgst = sgtot;
                            //    ca.Bcgst = cgtot;
                            //}
                            tda.Isvalid = "Y";
                            tda.ID = id;
                            TData.Add(tda);
                        }

                    }
                }


            }
            DataTable ap = datatrans.GetData("select SUM(CGST) as cgst,SUM(SGST) as sgst,SUM(IGST) as igst from PRETDETAIL WHERE  PRETDETAIL.PRETBASICID='" + id + "'");
            ca.Bigst = Convert.ToDouble(ap.Rows[0]["igst"].ToString() == "" ? "0" : ap.Rows[0]["igst"].ToString());
            ca.Bsgst = Convert.ToDouble(ap.Rows[0]["sgst"].ToString() == "" ? "0" : ap.Rows[0]["sgst"].ToString());
            ca.Bcgst = Convert.ToDouble(ap.Rows[0]["cgst"].ToString() == "" ? "0" : ap.Rows[0]["cgst"].ToString());
            ca.Depitlst = TData;
            return View(ca);
        }
        [HttpPost]
        public ActionResult DebitNoteBill(DebitNoteBill Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = DebitNoteBillService.DebitNoteBillCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "DebitNoteBill Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "DebitNoteBill Updated Successfully...!";
                    }
                    return RedirectToAction("ListDebitNoteBill");
                }

                else
                {
                    ViewBag.PageTitle = "Edit DebitNoteBill";
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
        [HttpPost]
        public ActionResult Credit_Note_Approval(DebitNoteBill Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = DebitNoteBillService.CreditNoteStock(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    TempData["notice"] = "CreditNote Approved Successfully...!";
                    return RedirectToAction("ListDebitNoteBill");
                }

                else
                {
                    ViewBag.PageTitle = "Edit DebitNoteBill";
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

        public IActionResult DN_Approval(string PROID)
        {
            DebitNoteBill grn = new DebitNoteBill();
            grn.RefDate = DateTime.Now.ToString("dd-MMM-yyyy");
            grn.createdby = Request.Cookies["UserId"];
            DataTable dt = new DataTable();
            dt = DebitNoteBillService.GetDNDetails(PROID);
            List<GRNAccount> TData = new List<GRNAccount>();
            GRNAccount tda = new GRNAccount();
            DataTable dtParty = datatrans.GetData("select P.ACCOUNTNAME from PARTYMAST P where P.PARTYMASTID='" + dt.Rows[0]["PARTYID"].ToString() + "'");
            string mid = dtParty.Rows[0]["ACCOUNTNAME"].ToString();
            grn.mid = mid;
            double net= Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString()); 
            double gross= Convert.ToDouble(dt.Rows[0]["GROSS"].ToString() == "" ? "0" : dt.Rows[0]["GROSS"].ToString());
            double cgst= Convert.ToDouble(dt.Rows[0]["BCGST"].ToString() == "" ? "0" : dt.Rows[0]["BCGST"].ToString());
            double sgst = Convert.ToDouble(dt.Rows[0]["BSGST"].ToString() == "" ? "0" : dt.Rows[0]["BSGST"].ToString());
            double igst = Convert.ToDouble(dt.Rows[0]["BIGST"].ToString() == "" ? "0" : dt.Rows[0]["BIGST"].ToString());
            double totalcredit = 0;
            double totaldebit = 0;
            if (net > 0)
            {
                tda = new GRNAccount();
                tda.Ledgerlist = BindLedgerLst();
                tda.Ledgername = mid;
                tda.CRAmount = net;
                tda.DRAmount = 0;
                tda.TypeName = "NET";
                tda.Isvalid = "Y";
                tda.CRDR = "Dr";
                totalcredit += tda.CRAmount;
                totaldebit += tda.DRAmount;
                tda.symbol = "-";
                TData.Add(tda);
            }
            if (gross > 0)
            {
                tda = new GRNAccount();
                tda.Ledgerlist = BindLedgerLst();
                tda.Ledgername = mid;
                tda.CRAmount = 0;
                tda.DRAmount = gross;
                tda.TypeName = "GROSS";
                tda.Isvalid = "Y";
                tda.CRDR = "Cr";
                totalcredit += tda.CRAmount;
                totaldebit += tda.DRAmount;
                tda.symbol = "+";
                TData.Add(tda);
            }
            if (cgst > 0)
            {
                tda = new GRNAccount();
                tda.Ledgerlist = BindLedgerLst();
                tda.Ledgername = mid;
                tda.CRAmount = 0;
                tda.DRAmount = cgst;
                tda.TypeName = "CGST";
                tda.Isvalid = "Y";
                tda.CRDR = "Cr";
                totalcredit += tda.CRAmount;
                totaldebit += tda.DRAmount;
                tda.symbol = "+";
                TData.Add(tda);
            }
            if (sgst > 0)
            {
                tda = new GRNAccount();
                tda.Ledgerlist = BindLedgerLst();
                tda.Ledgername = mid;
                tda.CRAmount = 0;
                tda.DRAmount = net;
                tda.TypeName = "SGST";
                tda.Isvalid = "Y";
                tda.CRDR = "Cr";
                totalcredit += tda.CRAmount;
                totaldebit += tda.DRAmount;
                tda.symbol = "+";
                TData.Add(tda);
            }
            if (igst > 0)
            {
                tda = new GRNAccount();
                tda.Ledgerlist = BindLedgerLst();
                tda.Ledgername = mid;
                tda.CRAmount = 0;
                tda.DRAmount = igst;
                tda.TypeName = "IGST";
                tda.Isvalid = "Y";
                tda.CRDR = "Cr";
                totalcredit += tda.CRAmount;
                totaldebit += tda.DRAmount;
                tda.symbol = "+";
                TData.Add(tda);
            }
            grn.TotalCRAmt = totalcredit;
            grn.TotalDRAmt = totaldebit;
            grn.Acclst = TData;
            grn.Accconfiglst = BindAccconfig();
            return View(grn);
        }
        public List<SelectListItem> BindAccconfig()
        {
            try
            {
                DataTable dtDesg = DebitNoteBillService.AccconfigLst();
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
        public List<SelectListItem> BindLedgerLst()
        {
            try
            {
                DataTable dtDesg = datatrans.LedgerList();
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
        public IActionResult Credit_Note_Approval(string PROID)
        {

            DebitNoteBill ca = new DebitNoteBill();
            ca.RecList = BindEmp();
            ca.Currency = "1";
            ca.Exchange = "1";
            ca.Curlst = BindCurrency();
            List<CreditItem> TData = new List<CreditItem>();
            CreditItem tda = new CreditItem();
            tda = new CreditItem();
            tda.Isvalid = "Y";
            tda.Crlst = BindCredit();
            tda.grplst = BindAccGroup();
            tda.Acclst = BindLedger("");
            TData.Add(tda);

            DataTable dt = DebitNoteBillService.EditProEntry(PROID);
            if (dt.Rows.Count > 0)
               {
                    ca.ID = PROID;
                    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
                    ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    ca.Bra = dt.Rows[0]["BRA"].ToString();
                    ca.Net = dt.Rows[0]["NET"].ToString();
                    ca.Partyid = dt.Rows[0]["PARTYID"].ToString();
                    ca.Location = Request.Cookies["Locationname"];
                    ca.Loc = Request.Cookies["Locationid"];
                ca.Vocher = "Debit Note";
                DataTable dt1 = DebitNoteBillService.GetPartyLedger(ca.Partyid);
                ca.ledger = dt1.Rows[0]["DISPLAY_NAME"].ToString();

            }
            
            ca.Creditlst = TData;
            return View(ca);
        }
        public List<SelectListItem> BindItemlst(string value)
        {
            try
            {
               // string grn = datatrans.GetDataString("Select GRNBLBASICID from GRNBLBASIC where DOCID='" + value + "' ");
                DataTable dtDesg = DebitNoteBillService.GetItem(value);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["item"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindLedger(string id)
        {
            try
            {
                DataTable dtDesg = DebitNoteBillService.GetLedger(id);
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
        public List<SelectListItem> BindAccGroup()
        {
            try
            {
                DataTable dtDesg = DebitNoteBillService.GetAccGrp();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ACCOUNTGROUP"].ToString(), Value = dtDesg.Rows[i]["ACCGROUPID"].ToString() });
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
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPNAME"].ToString(), Value = dtDesg.Rows[i]["EMPNAME"].ToString() });
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
        public List<SelectListItem> BindVocher()
        {
            try
            {
                DataTable dtDesg = DebitNoteBillService.GetVocher();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DESCRIPTION"].ToString(), Value = dtDesg.Rows[i]["VCHTYPEID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetPartyJSON(string supid)
        {
            //string CityID = datatrans.GetDataString("Select STATEMASTID from STATEMAST where STATE='" + supid + "' ");
            DebitNoteItem model = new DebitNoteItem();
            model.Grnlst = BindGrnlst(supid);
            return Json(BindGrnlst(supid));

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
        public JsonResult GetAccountJSON(string id)
        {

            return Json(BindLedger(id));

        }
        public JsonResult GetAccGroupJSON(string itemid)
        {
            //string CityID = datatrans.GetDataString("Select STATEMASTID from STATEMAST where STATE='" + supid + "' ");
            CreditItem model = new CreditItem();
            model.Acclst = BindLedger(itemid);
            return Json(BindLedger(itemid));

        }
        public ActionResult GetItemDetail(string ItemId,string grnid)
        {
            try
            {
                DataTable dt = new DataTable();


                //string invDate = "";
                //string item = "";
                //string ItemType = "";
                //string ItemSpec = "";
                string cf = "";
                string unit = "";
                string qty = "";
                string rate = "";
                string amount = "";
                string cgstp = "";
                string sgstp = "";
                string igstp = "";

                string cgst = "";
                string sgst = "";
                string igst = "";
                string total = "";

                dt = DebitNoteBillService.GetItemDetails(ItemId, grnid);

                if (dt.Rows.Count > 0)
                {


                    ////invDate = dt.Rows[0]["DOCDATE"].ToString();
                    //item = dt.Rows[0]["ITEMID"].ToString();
                    //ItemType = dt.Rows[0]["UNITID"].ToString();
                    //ItemSpec = dt.Rows[0]["UNITID"].ToString();
                    cf = dt.Rows[0]["CF"].ToString();
                    unit = dt.Rows[0]["PUNIT"].ToString();
                    qty = dt.Rows[0]["QTY"].ToString();
                    rate = dt.Rows[0]["RATE"].ToString();
                    amount = dt.Rows[0]["AMOUNT"].ToString();
                    cgstp = dt.Rows[0]["CGSTP"].ToString();
                    sgstp = dt.Rows[0]["SGSTP"].ToString();
                    igstp = dt.Rows[0]["IGSTP"].ToString();
                    cgst = dt.Rows[0]["CGST"].ToString();
                    sgst = dt.Rows[0]["SGST"].ToString();
                    igst = dt.Rows[0]["IGST"].ToString();
                    total = dt.Rows[0]["TOTAMT"].ToString();

                }

                var result = new { cf = cf, unit = unit, qty = qty, rate = rate, amount = amount, cgstp= cgstp, sgstp= sgstp, igstp= igstp, cgst = cgst, sgst = sgst, igst = igst, total = total };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public ActionResult GetInvoDate(string ItemId)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();


        //       string invDate = "";
               

        //        dt = DebitNoteBillService.GetInvoDates(ItemId);

        //        if (dt.Rows.Count > 0)
        //        {

        //            invDate = dt.Rows[0]["DOCDATE"].ToString();
        //        }

        //        var result = new { invDate = invDate };
        //        return Json(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public IActionResult ListDebitNoteBill()
        {
            IEnumerable<DebitNoteBill> sta = DebitNoteBillService.GetAllDebitNoteBill();
            return View(sta);
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
        public List<SelectListItem> BindGParty()
        {
            try
            {
                DataTable dtDesg = datatrans.GetSupplier();
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
        public List<SelectListItem> BindGrnlst(string id)
        {
            try
            {
                DataTable dtDesg = DebitNoteBillService.GetGrn(id);
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
        public JsonResult GetItemJSON(string itemid)
        {
            DebitNoteItem model = new DebitNoteItem();
            model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst(itemid));

        }
    }
}
