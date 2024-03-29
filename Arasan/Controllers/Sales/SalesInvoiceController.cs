using Microsoft.AspNetCore.Mvc;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Collections.Generic;
using System.Xml.Linq;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Services.Sales;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Nest;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using AspNetCore.Reporting;
using Microsoft.Reporting.WebForms;
using QRCoder;
using System.Drawing.Imaging;
using System.Drawing;

using DocumentFormat.OpenXml.Drawing;

using ZXing;
using ZXing.Common;
using Intuit.Ipp.ReportService;


namespace Arasan.Controllers
{
    public class SalesInvoiceController : Controller
    {
        ISalesInvoiceService SalesInvoiceService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;

        private readonly IWebHostEnvironment _WebHostEnvironment;
        public SalesInvoiceController(ISalesInvoiceService _SalesInvoiceService, IConfiguration _configuratio, IWebHostEnvironment WebHostEnvironment)
        {
            SalesInvoiceService = _SalesInvoiceService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
            this._WebHostEnvironment = WebHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }
        public IActionResult SalesInvoice(string id)
        {
            SalesInvoice ca = new SalesInvoice();
            ca.Brlst = BindBranch();
            ca.Curlst = BindCurrency();
            ca.Currency = "1";
            ca.Loclst = GetLoc();
            ca.Invlst = BindInVoiceType();
            ca.Suplst = BindSupplier();
            ca.Typelst = BindType();
            ca.Orderlst = BindOrder();
            ca.Dislst = BindDispatchType();
            ca.Inspelst = BindInspect();
            ca.Doclst = BindDocument();
            ca.Voclst = BindVocher();
            ca.Tranlst = BindTrans();
            ca.Tnamelst = Tname();
            ca.Joblst = BindJob();
            ca.enterdby = Request.Cookies["Username"];
            List<SalesInvoiceItem> TData = new List<SalesInvoiceItem>();
            SalesInvoiceItem tda = new SalesInvoiceItem();
            List<TermsItem> TData1 = new List<TermsItem>();
            TermsItem tda1 = new TermsItem();
            List<AreaItem> TData2 = new List<AreaItem>();
            AreaItem tda2 = new AreaItem();

            if (id == null)
            {
                ca.Doc = "BY HAND";
                ca.Ordsam = "ORDER";
                ca.Dis = "Road";
                ca.Inspect = "OWN";
                ca.RecBy = "OWN";
                ca.Vocher = "R";
                ca.Branch = "10001000000001";
                ca.Location = "12423000000238";
                //ca.Narration = "Invoice to";
                string loc = ca.Location;
                ViewBag.locdisp = ca.Location;
                ca.InvDate = DateTime.Now.ToString("dd-MMM-yyyy");
                ca.RefDate = DateTime.Now.ToString("dd-MMM-yyyy");
                ca.ExRate = "1";
                DataTable dtv = datatrans.GetSequence("exinv", loc);
                if (dtv.Rows.Count > 0)
                {
                    ca.InvNo = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["LASTNO"].ToString();
                }
                for (int i = 0; i < 1; i++)
                {
                    tda = new SalesInvoiceItem();
                    tda.worklst = Bindempty();
                    tda.jobschlst = Bindempty();
                    tda.Itemlst = BindItemlst("");
                    tda.binlst = BindBin();
                    //tda.FrieghtItemId = "Frieght Charges";
                    //tda.FriQty = "1";
                    //tda.HSNcode = "996519";
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda1 = new TermsItem();

                    tda1.Termslst = BindTerms();
                    tda1.Isvalid = "Y";
                    TData1.Add(tda1);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda2 = new AreaItem();

                    tda2.Arealst = BindArea("");
                    tda2.Isvalid = "Y";
                    TData2.Add(tda2);
                }
            }
            else
            {

                // ca = directPurchase.GetDirectPurById(id);


                DataTable dt = new DataTable();
                double total = 0;
                dt = SalesInvoiceService.GetSalesInvoiceDeatils(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    ca.InvoType = dt.Rows[0]["INVTYPE"].ToString();
                    ca.InvNo = dt.Rows[0]["DOCID"].ToString();
                    ca.InvDate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.ID = id;
                    ca.RefNo = dt.Rows[0]["REFNO"].ToString();
                    ca.RefDate = dt.Rows[0]["REFDATE"].ToString();
                    ca.Party = dt.Rows[0]["PARTYID"].ToString();
                    ca.Vocher = dt.Rows[0]["VTYPE"].ToString();
                    ca.Customer = dt.Rows[0]["CUSTPO"].ToString();
                    ca.Ordsam = dt.Rows[0]["ORDERSAMPLE"].ToString();
                    ca.Sales = dt.Rows[0]["SALVAL"].ToString();
                    ca.RecBy = dt.Rows[0]["RECDBY"].ToString();
                    ca.Dis = dt.Rows[0]["DESPBY"].ToString();
                    ca.Inspect = dt.Rows[0]["INSPBY"].ToString();
                    ca.Trans = dt.Rows[0]["TRANSMODE"].ToString();
                    ca.Vno = dt.Rows[0]["VNO"].ToString();
                    ca.InvoiceD = dt.Rows[0]["INVDESC"].ToString();
                    ca.TranCharger = dt.Rows[0]["TRANSP"].ToString();
                    ca.Tname = dt.Rows[0]["TRANSNAME"].ToString();
                    ca.Distance = dt.Rows[0]["TRANSLIMIT"].ToString();
                    ca.Doc = dt.Rows[0]["DOCTHORUGH"].ToString();
                    ca.AinWords = dt.Rows[0]["AMTWORDS"].ToString();
                    ca.Serial = dt.Rows[0]["SERNO"].ToString();
                    ca.Narration = dt.Rows[0]["NARRATION"].ToString();
                    ca.Round = Convert.ToDouble(dt.Rows[0]["RNDOFF"].ToString() == "" ? "0" : dt.Rows[0]["RNDOFF"].ToString());
                    ca.Packing = Convert.ToDouble(dt.Rows[0]["PACKING"].ToString() == "" ? "0" : dt.Rows[0]["PACKING"].ToString());
                    ca.Gross = Convert.ToDouble(dt.Rows[0]["GROSS"].ToString() == "" ? "0" : dt.Rows[0]["GROSS"].ToString());
                    ca.Net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());

                }
                DataTable dt2 = new DataTable();
                dt2 = SalesInvoiceService.GetEditItemDetails(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new SalesInvoiceItem();
                        double toaamt = 0;
                        //tda.ItemGrouplst = BindItemGrplst();
                        //DataTable dt3 = new DataTable();
                        //dt3 = datatrans.GetItemSubGroup(dt2.Rows[i]["ITEMID"].ToString());
                        //if (dt3.Rows.Count > 0)
                        //{
                        //    tda.ItemGroupId = dt3.Rows[0]["SUBGROUPCODE"].ToString();
                        //}
                        tda.Itemlst = BindItemlst("");
                        tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();

                        DataTable dt4 = new DataTable();
                        dt4 = datatrans.GetItemDetails(tda.ItemId);
                        if (dt4.Rows.Count > 0)
                        {

                            tda.ConFac = dt4.Rows[0]["CF"].ToString();
                            tda.rate = Convert.ToDouble(dt4.Rows[0]["LATPURPRICE"].ToString());
                        }
                        tda.Quantity = Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                        toaamt = tda.rate * tda.Quantity;
                        total += toaamt;
                        //tda.QtyPrim= Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                        tda.Amount = toaamt;
                        tda.Unit = dt2.Rows[i]["UNITID"].ToString();
                        tda.FrigCharge = Convert.ToDouble(dt2.Rows[i]["FREIGHT"].ToString() == "" ? "0" : dt2.Rows[i]["FREIGHT"].ToString());
                        tda.DiscAmount = Convert.ToDouble(dt2.Rows[i]["DISCOUNT"].ToString() == "" ? "0" : dt2.Rows[i]["DISCOUNT"].ToString());
                        tda.CGSTP = Convert.ToDouble(dt2.Rows[i]["CGSTP"].ToString() == "" ? "0" : dt2.Rows[i]["CGSTP"].ToString());
                        tda.SGSTP = Convert.ToDouble(dt2.Rows[i]["SGSTP"].ToString() == "" ? "0" : dt2.Rows[i]["SGSTP"].ToString());
                        tda.IGSTP = Convert.ToDouble(dt2.Rows[i]["IGSTP"].ToString() == "" ? "0" : dt2.Rows[i]["IGSTP"].ToString());
                        tda.CGST = Convert.ToDouble(dt2.Rows[i]["CGST"].ToString() == "" ? "0" : dt2.Rows[i]["CGST"].ToString());
                        tda.SGST = Convert.ToDouble(dt2.Rows[i]["SGST"].ToString() == "" ? "0" : dt2.Rows[i]["SGST"].ToString());
                        tda.IGST = Convert.ToDouble(dt2.Rows[i]["IGST"].ToString() == "" ? "0" : dt2.Rows[i]["IGST"].ToString());
                        tda.IntroDiscount = Convert.ToDouble(dt2.Rows[i]["IDISC"].ToString() == "" ? "0" : dt2.Rows[i]["IDISC"].ToString());
                        tda.CashDiscount = Convert.ToDouble(dt2.Rows[i]["CDISC"].ToString() == "" ? "0" : dt2.Rows[i]["CDISC"].ToString());
                        tda.TradeDiscount = Convert.ToDouble(dt2.Rows[i]["TDISC"].ToString() == "" ? "0" : dt2.Rows[i]["TDISC"].ToString());
                        tda.AddDiscount = Convert.ToDouble(dt2.Rows[i]["ADISC"].ToString() == "" ? "0" : dt2.Rows[i]["ADISC"].ToString());
                        tda.SpecDiscount = Convert.ToDouble(dt2.Rows[i]["SDISC"].ToString() == "" ? "0" : dt2.Rows[i]["SDISC"].ToString());
                        tda.TotalAmount = Convert.ToDouble(dt2.Rows[i]["TOTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["TOTAMT"].ToString());





                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }

            }
            ca.SIlst = TData;
            ca.TermsItemlst = TData1;
            ca.AreaItemlst = TData2;
            return View(ca);
        }
        [HttpPost]
        public ActionResult SalesInvoice(SalesInvoice Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = SalesInvoiceService.DirectPurCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "SalesInvoice Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "SalesInvoice Updated Successfully...!";
                    }
                    return RedirectToAction("ListSalesInvoice");
                }

                else
                {
                    ViewBag.PageTitle = "Edit SalesInvoice";
                    TempData["notice"] = Strout;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Cy);
        }
        public ActionResult GetWO(string jobid)
        {
            try
            {
                DataTable dt = new DataTable();

                string location = "";
                string party = "";
              
                    dt = SalesInvoiceService.Getjobdetails(jobid);

                    if (dt.Rows.Count > 0)
                    {
                        party = dt.Rows[0]["PARTYID"].ToString();
                       location = dt.Rows[0]["LOCID"].ToString();
                    }


                var result = new { party = party, location = location };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult SalesInvoiceS(string id)
        {
            SalesInvoice ca = new SalesInvoice();
            ca.Brlst = BindBranch();
            ca.Curlst = BindCurrency();
            ca.Currency = "1";
            ca.Loclst = GetLoc();
            ca.Invlst = BindInVoiceType();
            ca.Suplst = BindSupplier();
            ca.Typelst = BindType();
            ca.Orderlst = BindOrder();
            ca.Dislst = BindDispatchType();
            ca.Inspelst = BindInspect();
            ca.Doclst = BindDocument();
            ca.Voclst = BindVocher();
            ca.Tranlst = BindTrans();
            ca.Tnamelst = Tname();
            ca.enterdby = Request.Cookies["Username"];
            List<SalesInvoiceItem> TData = new List<SalesInvoiceItem>();
            SalesInvoiceItem tda = new SalesInvoiceItem();
            List<TermsItem> TData1 = new List<TermsItem>();
            TermsItem tda1 = new TermsItem();
            List<AreaItem> TData2 = new List<AreaItem>();
            AreaItem tda2 = new AreaItem();

            if (id == null)
            {
                ca.Doc = "BY HAND";
                ca.Ordsam = "ORDER";
                ca.Dis = "Road";
                ca.Inspect = "OWN";
                ca.RecBy = "OWN";
                ca.Vocher = "R";
                ca.Branch = "10001000000001";
                ca.Location = "12418000000423";
                //ca.Narration = "Invoice to";
                string loc = ca.Location;
                ViewBag.locdisp = ca.Location;
                ca.InvDate = DateTime.Now.ToString("dd-MMM-yyyy");
                ca.RefDate = DateTime.Now.ToString("dd-MMM-yyyy");
                ca.ExRate = "1";
                DataTable dtv = datatrans.GetSequence("Deinv", loc);
                if (dtv.Rows.Count > 0)
                {
                    ca.InvNo = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["LASTNO"].ToString();
                }
                for (int i = 0; i < 1; i++)
                {
                    tda = new SalesInvoiceItem();
                    //tda.ItemGrouplst = BindItemGrplst();
                    tda.Itemlst = BindItemlst("");
                    tda.binlst = BindBin();
                    //tda.FrieghtItemId = "Frieght Charges";
                    //tda.FriQty = "1";
                    //tda.HSNcode = "996519";
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda1 = new TermsItem();

                    tda1.Termslst = BindTerms();
                    tda1.Isvalid = "Y";
                    TData1.Add(tda1);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda2 = new AreaItem();

                    tda2.Arealst = BindArea("");
                    tda2.Isvalid = "Y";
                    TData2.Add(tda2);
                }
            }
            else
            {

                // ca = directPurchase.GetDirectPurById(id);


                DataTable dt = new DataTable();
                double total = 0;
                dt = SalesInvoiceService.GetSalesInvoiceDeatils(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    ca.InvoType = dt.Rows[0]["INVTYPE"].ToString();
                    ca.InvNo = dt.Rows[0]["DOCID"].ToString();
                    ca.InvDate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.ID = id;
                    ca.RefNo = dt.Rows[0]["REFNO"].ToString();
                    ca.RefDate = dt.Rows[0]["REFDATE"].ToString();
                    ca.Party = dt.Rows[0]["PARTYID"].ToString();
                    ca.Vocher = dt.Rows[0]["VTYPE"].ToString();
                    ca.Customer = dt.Rows[0]["CUSTPO"].ToString();
                    ca.Ordsam = dt.Rows[0]["ORDERSAMPLE"].ToString();
                    ca.Sales = dt.Rows[0]["SALVAL"].ToString();
                    ca.RecBy = dt.Rows[0]["RECDBY"].ToString();
                    ca.Dis = dt.Rows[0]["DESPBY"].ToString();
                    ca.Inspect = dt.Rows[0]["INSPBY"].ToString();
                    ca.Trans = dt.Rows[0]["TRANSMODE"].ToString();
                    ca.Vno = dt.Rows[0]["VNO"].ToString();
                    ca.InvoiceD = dt.Rows[0]["INVDESC"].ToString();
                    ca.TranCharger = dt.Rows[0]["TRANSP"].ToString();
                    ca.Tname = dt.Rows[0]["TRANSNAME"].ToString();
                    ca.Distance = dt.Rows[0]["TRANSLIMIT"].ToString();
                    ca.Doc = dt.Rows[0]["DOCTHORUGH"].ToString();
                    ca.AinWords = dt.Rows[0]["AMTWORDS"].ToString();
                    ca.Serial = dt.Rows[0]["SERNO"].ToString();
                    ca.Narration = dt.Rows[0]["NARRATION"].ToString();
                    ca.Round = Convert.ToDouble(dt.Rows[0]["RNDOFF"].ToString() == "" ? "0" : dt.Rows[0]["RNDOFF"].ToString());
                    ca.Packing = Convert.ToDouble(dt.Rows[0]["PACKING"].ToString() == "" ? "0" : dt.Rows[0]["PACKING"].ToString());
                    ca.Gross = Convert.ToDouble(dt.Rows[0]["GROSS"].ToString() == "" ? "0" : dt.Rows[0]["GROSS"].ToString());
                    ca.Net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());

                }
                DataTable dt2 = new DataTable();
                dt2 = SalesInvoiceService.GetEditItemDetails(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new SalesInvoiceItem();
                        double toaamt = 0;
                        //tda.ItemGrouplst = BindItemGrplst();
                        //DataTable dt3 = new DataTable();
                        //dt3 = datatrans.GetItemSubGroup(dt2.Rows[i]["ITEMID"].ToString());
                        //if (dt3.Rows.Count > 0)
                        //{
                        //    tda.ItemGroupId = dt3.Rows[0]["SUBGROUPCODE"].ToString();
                        //}
                        tda.Itemlst = BindItemlst("");
                        tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();

                        DataTable dt4 = new DataTable();
                        dt4 = datatrans.GetItemDetails(tda.ItemId);
                        if (dt4.Rows.Count > 0)
                        {

                            tda.ConFac = dt4.Rows[0]["CF"].ToString();
                            tda.rate = Convert.ToDouble(dt4.Rows[0]["LATPURPRICE"].ToString());
                        }
                        tda.Quantity = Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                        toaamt = tda.rate * tda.Quantity;
                        total += toaamt;
                        //tda.QtyPrim= Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                        tda.Amount = toaamt;
                        tda.Unit = dt2.Rows[i]["UNITID"].ToString();
                        tda.FrigCharge = Convert.ToDouble(dt2.Rows[i]["FREIGHT"].ToString() == "" ? "0" : dt2.Rows[i]["FREIGHT"].ToString());
                        tda.DiscAmount = Convert.ToDouble(dt2.Rows[i]["DISCOUNT"].ToString() == "" ? "0" : dt2.Rows[i]["DISCOUNT"].ToString());
                        tda.CGSTP = Convert.ToDouble(dt2.Rows[i]["CGSTP"].ToString() == "" ? "0" : dt2.Rows[i]["CGSTP"].ToString());
                        tda.SGSTP = Convert.ToDouble(dt2.Rows[i]["SGSTP"].ToString() == "" ? "0" : dt2.Rows[i]["SGSTP"].ToString());
                        tda.IGSTP = Convert.ToDouble(dt2.Rows[i]["IGSTP"].ToString() == "" ? "0" : dt2.Rows[i]["IGSTP"].ToString());
                        tda.CGST = Convert.ToDouble(dt2.Rows[i]["CGST"].ToString() == "" ? "0" : dt2.Rows[i]["CGST"].ToString());
                        tda.SGST = Convert.ToDouble(dt2.Rows[i]["SGST"].ToString() == "" ? "0" : dt2.Rows[i]["SGST"].ToString());
                        tda.IGST = Convert.ToDouble(dt2.Rows[i]["IGST"].ToString() == "" ? "0" : dt2.Rows[i]["IGST"].ToString());
                        tda.IntroDiscount = Convert.ToDouble(dt2.Rows[i]["IDISC"].ToString() == "" ? "0" : dt2.Rows[i]["IDISC"].ToString());
                        tda.CashDiscount = Convert.ToDouble(dt2.Rows[i]["CDISC"].ToString() == "" ? "0" : dt2.Rows[i]["CDISC"].ToString());
                        tda.TradeDiscount = Convert.ToDouble(dt2.Rows[i]["TDISC"].ToString() == "" ? "0" : dt2.Rows[i]["TDISC"].ToString());
                        tda.AddDiscount = Convert.ToDouble(dt2.Rows[i]["ADISC"].ToString() == "" ? "0" : dt2.Rows[i]["ADISC"].ToString());
                        tda.SpecDiscount = Convert.ToDouble(dt2.Rows[i]["SDISC"].ToString() == "" ? "0" : dt2.Rows[i]["SDISC"].ToString());
                        tda.TotalAmount = Convert.ToDouble(dt2.Rows[i]["TOTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["TOTAMT"].ToString());





                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }

            }
            ca.SIlst = TData;
            ca.TermsItemlst = TData1;
            ca.AreaItemlst = TData2;
            return View(ca);
        }
        public IActionResult ListSalesInvoice(string id)
        {
            return View();
        }
        public IActionResult ListSIJSON(string id)
        {
            return View();
        }
        public IActionResult Jsonexport(string id)
        {
            DataTable dt = new DataTable();
            dt = SalesInvoiceService.GetSIDetails(id);
            string docdate= DateTime.Now.ToString("dd-MMM-yyyy");
            Jsonexport toDownload = new Jsonexport();
            string random = RandomString(7, false);
            string invno = "";
            string invdate = "";
            string jsonString = "[";
            toDownload.Version = "1.1";
            transdetail trna=new transdetail();
            trna.TaxSch = "GST";
            trna.SupTyp = "B2B";
            trna.RegRev = "N";
            trna.IgstOnIntra = "N";
            toDownload.TranDtls= trna;
            docdetails docdt = new docdetails();
            docdt.Typ = "INV";
            if (dt.Rows.Count > 0)
            {
                invno = dt.Rows[0]["INVNO"].ToString();
                invdate= dt.Rows[0]["invdate"].ToString();
                docdt.No = invno;
                docdt.Dt = invdate;
            }
            toDownload.DocDtls = docdt;
            sellerdetails sdt= new sellerdetails();
            sdt.Gstin = "33AAACT6771E1ZG";
            sdt.LglNm = "THE ARASAN ALUMINIUM INDUSTRIES (P) LTD";
            sdt.TrdNm = "THE ARASAN ALUMINIUM INDUSTRIES (P) LTD";
            sdt.Addr1 = "3/111 S.No 556 to 568, Melamathur Village";
            sdt.Addr2 = "Virudhunagar Road";
            sdt.Loc = "SIVAKSI";
            sdt.Stcd = "33";
            sdt.Pin = "626005";
            sdt.Em = "info@arasanaluminium.com";
            sdt.Ph = "917867000916";
            toDownload.SellerDtls= sdt;
            buyerdetails bdt= new buyerdetails();
            if (dt.Rows.Count > 0)
            {
                bdt.Gstin = dt.Rows[0]["GSTNO"].ToString();
                bdt.LglNm = dt.Rows[0]["PARTYID"].ToString();
                bdt.TrdNm = dt.Rows[0]["PARTYID"].ToString();
                bdt.pos = dt.Rows[0]["SCODE"].ToString();
                bdt.Addr1 = dt.Rows[0]["ADD1"].ToString();
                bdt.Addr2 = dt.Rows[0]["ADD2"].ToString();
                bdt.Loc = dt.Rows[0]["CITY"].ToString();
                bdt.Stcd = dt.Rows[0]["SCODE"].ToString();
                bdt.Pin = dt.Rows[0]["PINCODE"].ToString();
                bdt.Em = dt.Rows[0]["EMAIL"].ToString();
                bdt.Ph = dt.Rows[0]["PHONENO"].ToString();
            }
            toDownload.BuyerDtls = bdt;
            valuedtails vdt= new valuedtails();
            if (dt.Rows.Count > 0)
            {
                vdt.AssVal = dt.Rows[0]["GROSS"].ToString();
                vdt.CgstVal = dt.Rows[0]["BCGST"].ToString();
                vdt.SgstVal = dt.Rows[0]["BSGST"].ToString();
                vdt.IgstVal = dt.Rows[0]["BIGST"].ToString();
                vdt.CesVal = "0";
                vdt.StCesVal = "0";
                vdt.Discount = dt.Rows[0]["BDISCOUNT"].ToString();
                vdt.OthChrg = dt.Rows[0]["BFREIGHT"].ToString();
                vdt.RndOffAmt = dt.Rows[0]["ROFF"].ToString();
                vdt.TotInvVal = dt.Rows[0]["NET"].ToString();
                vdt.TotInvValFc = "0";
            }
            toDownload.ValDtls = vdt;
            shipdetails shdt = new shipdetails();
            if (dt.Rows.Count > 0)
            {
                shdt.Gstin = dt.Rows[0]["GSTNO"].ToString();
                shdt.LglNm = dt.Rows[0]["PARTYID"].ToString();
                shdt.TrdNm = dt.Rows[0]["PARTYID"].ToString();
                shdt.Addr1 = dt.Rows[0]["ADD1"].ToString();
                shdt.Addr2 = dt.Rows[0]["ADD2"].ToString();
                shdt.Loc = dt.Rows[0]["CITY"].ToString();
                shdt.Stcd = dt.Rows[0]["SCODE"].ToString();
                shdt.Pin = dt.Rows[0]["PINCODE"].ToString();
            }
            toDownload.ShipDtls = shdt;
            ewbdetails edt = new ewbdetails();
            DataTable dg = datatrans.GetData("Select PARTYID,GSTNO from PARTYMAST where PARTYMASTID='"+ dt.Rows[0]["TRANSP"].ToString() + "'");
            if (dt.Rows.Count > 0)
            {
                edt.TransId= dg.Rows[0]["GSTNO"].ToString();
                edt.TransName = dg.Rows[0]["PARTYID"].ToString();
                edt.TransMode = "1";
                edt.Distance = dt.Rows[0]["TDIST"].ToString();
                edt.VehNo = dt.Rows[0]["VNO"].ToString();
                edt.VehType = "R";
               
            }
            toDownload.EwbDtls = edt;
            DataTable dts=new DataTable();
            dts = SalesInvoiceService.GetSIITEMDetails(id);
            List<ItemDetails> TData = new List<ItemDetails>();
            ItemDetails tda = new ItemDetails();
            double gstrate = 0;
            if (dts.Rows.Count > 0)
            {
                for (int i = 0; i < dts.Rows.Count; i++)
                {
                    tda= new ItemDetails();
                    tda.SlNo = (i + 1).ToString();
                    tda.PrdDesc = dts.Rows[i]["ITEMSPEC"].ToString();
                    tda.IsServc = "N";
                    tda.HsnCd = dts.Rows[i]["HSN"].ToString();
                    
                    tda.Qty = dts.Rows[i]["Qty"].ToString();
                    tda.FreeQty = "";
                    tda.Unit = dts.Rows[i]["UNITID"].ToString();
                    tda.UnitPrice = dts.Rows[i]["RATE"].ToString();
                    tda.TotAmt = dts.Rows[i]["AMOUNT"].ToString();
                    tda.Discount = dts.Rows[i]["DISCOUNT"].ToString();
                    tda.PreTaxVal = dts.Rows[i]["AMOUNT"].ToString();
                    tda.AssAmt = dts.Rows[i]["AMOUNT"].ToString();
                    if(Convert.ToDouble(dts.Rows[i]["IGSTP"].ToString()) > 0)
                    {
                        gstrate = Convert.ToDouble(dts.Rows[i]["IGSTP"].ToString());
                    }
                    else
                    {
                        gstrate = Convert.ToDouble(dts.Rows[i]["CGSTP"].ToString()) + Convert.ToDouble(dts.Rows[i]["SGSTP"].ToString());
                    }
                    tda.GstRt = gstrate.ToString();
                    tda.IgstAmt = dts.Rows[i]["IGST"].ToString();
                    tda.CgstAmt = dts.Rows[i]["CGST"].ToString();
                    tda.SgstAmt = dts.Rows[i]["SGST"].ToString();
                    tda.OthChrg = "";
                    tda.TotItemVal = dts.Rows[i]["TOTAMT"].ToString();
                    tda.OrdLineRef = "";
                    tda.OrgCntry = "";
                    tda.PrdSlNo = dts.Rows[i]["DRUMDESC"].ToString();
                    TData.Add(tda);


                }
            }
            toDownload.ItemList = TData;
            jsonString += JsonSerializer.Serialize(toDownload);
            //string jsonString = JsonSerializer.Serialize(toDownload);
            jsonString += "]";
            var fileName = invno + "_" + invdate + "_" + random + "_" + docdate + ".json";
            var mimeType = "text/plain";
            var fileBytes = Encoding.ASCII.GetBytes(jsonString);
            return new FileContentResult(fileBytes, mimeType)
            {
                FileDownloadName = fileName
            };
        }
        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        public List<SelectListItem> BindJob()
        {
            try
            {
                DataTable dtDesg = SalesInvoiceService.GetJob();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["JOBASICID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult MyListSIGrid(string strStatus)
        {
            List<ListSIItems> Reg = new List<ListSIItems>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = SalesInvoiceService.GetAllSalesInvoice(strStatus);
            string EditRow = string.Empty;
            string DeleteRow = string.Empty;
            string jsonRow = string.Empty;
            string viewrow=string.Empty;
            string pdf = string.Empty;
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                //EditRow = "<a href=ProFormaInvoice?id=" + dtUsers.Rows[i]["PINVBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                //DeleteRow = "<a href=CloseQuote?id=" + dtUsers.Rows[i]["PINVBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                pdf = "<a href=Print?id=" + dtUsers.Rows[i]["EXINVBASICID"].ToString() + " target='_blank'><img src='../Images/pdficon.png' alt='View Details' width='20' /></a>";

                jsonRow = "<a href=Jsonexport?id=" + dtUsers.Rows[i]["EXINVBASICID"].ToString() + "><img src='../Images/json.png' alt='Edit' /></a>";
                viewrow = "<a href=ViewSI?id=" + dtUsers.Rows[i]["EXINVBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View' /></a>";
                Reg.Add(new ListSIItems
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["EXINVBASICID"].ToString()),
                    docno = dtUsers.Rows[i]["INVNO"].ToString(),
                    currency = dtUsers.Rows[i]["MAINCURR"].ToString(),
                    date = dtUsers.Rows[i]["INVDATE"].ToString(),
                    party = dtUsers.Rows[i]["PARTYID"].ToString(),
                    edit = EditRow,
                    delrow = DeleteRow,
                    jsonrow= jsonRow,
                    viewrow= viewrow,
                    pdf = pdf,
                    gross= dtUsers.Rows[i]["GROSS"].ToString(),
                    net= dtUsers.Rows[i]["NET"].ToString(),
                    cgst= dtUsers.Rows[i]["BCGST"].ToString(),
                    igst= dtUsers.Rows[i]["BIGST"].ToString(),
                    sgst = dtUsers.Rows[i]["BSGST"].ToString(),
                });
            }

            return Json(new
            {
                Reg
            });

        }
        public ActionResult ViewSI(string id)
        {
            SalesInvoice ca = new SalesInvoice();
            DataTable dt = SalesInvoiceService.ViewDepot(id);
            if (dt.Rows.Count > 0)
            {
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.InvoType = dt.Rows[0]["INVTYPE"].ToString();
                ca.InvNo = dt.Rows[0]["DOCID"].ToString();
                ca.InvDate = dt.Rows[0]["DOCDATE"].ToString();
                ca.ID = id;
                ca.Location = dt.Rows[0]["LOCID"].ToString();
                ca.Party = dt.Rows[0]["PARTYNAME"].ToString();
                ca.Vocher = dt.Rows[0]["VTYPE"].ToString();

                ca.Ordsam = dt.Rows[0]["EORDTYPE"].ToString();
                ca.Sales = dt.Rows[0]["SALVAL"].ToString();
                ca.RecBy = dt.Rows[0]["RECDBY"].ToString();
                ca.Dis = dt.Rows[0]["DESPBY"].ToString();
                ca.Inspect = dt.Rows[0]["INSPBY"].ToString();
                ca.Doc = dt.Rows[0]["DOCTHORUGH"].ToString();
                ca.AinWords = dt.Rows[0]["AMTWORDS"].ToString();
                ca.Serial = dt.Rows[0]["SERNO"].ToString();
                ca.Narration = dt.Rows[0]["NARRATION"].ToString();
                ca.Round = Convert.ToDouble(dt.Rows[0]["RNDOFF"].ToString() == "" ? "0" : dt.Rows[0]["RNDOFF"].ToString());
                //ca.Packing = Convert.ToDouble(dt.Rows[0]["PACKING"].ToString() == "" ? "0" : dt.Rows[0]["PACKING"].ToString());
                ca.Gross = Convert.ToDouble(dt.Rows[0]["GROSS"].ToString() == "" ? "0" : dt.Rows[0]["GROSS"].ToString());
                ca.Net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());
                ca.cgst = Convert.ToDouble(dt.Rows[0]["BCGST"].ToString() == "" ? "0" : dt.Rows[0]["BCGST"].ToString());
                ca.sgst = Convert.ToDouble(dt.Rows[0]["BSGST"].ToString() == "" ? "0" : dt.Rows[0]["BSGST"].ToString());
                ca.igst = Convert.ToDouble(dt.Rows[0]["BIGST"].ToString() == "" ? "0" : dt.Rows[0]["BIGST"].ToString());
                ca.Discount = Convert.ToDouble(dt.Rows[0]["BDISCOUNT"].ToString() == "" ? "0" : dt.Rows[0]["BDISCOUNT"].ToString());
                ca.FrightCharge = Convert.ToDouble(dt.Rows[0]["BFREIGHT"].ToString() == "" ? "0" : dt.Rows[0]["BFREIGHT"].ToString());
                ca.Trans = dt.Rows[0]["TRANSMODE"].ToString();
                ca.Distance = dt.Rows[0]["TRANSLIMIT"].ToString();

                //ca.Tname = dt.Rows[0]["TRANSNAME"].ToString();
                ca.Vno = dt.Rows[0]["VNO"].ToString();
                ca.InvoiceD = dt.Rows[0]["INVDESC"].ToString();
                ca.TranCharger = dt.Rows[0]["TRANSP"].ToString();

            }
            List<SalesInvoiceItem> TData = new List<SalesInvoiceItem>();
            SalesInvoiceItem tda = new SalesInvoiceItem();
            DataTable dtproin = SalesInvoiceService.Depotdetail(id);
            if (dtproin.Rows.Count > 0)
            {
                for (int i = 0; i < dtproin.Rows.Count; i++)
                {
                    tda = new SalesInvoiceItem();
                    tda.ItemId = dtproin.Rows[i]["ITEMID"].ToString();
                    tda.ItemType = dtproin.Rows[i]["ITEMTYPE"].ToString();
                    tda.ItemSpec = dtproin.Rows[i]["ITEMSPEC"].ToString();
                    tda.Quantity = dtproin.Rows[i]["QTY"].ToString() != "" ? Convert.ToDouble(dtproin.Rows[i]["QTY"].ToString()) : 0;
                    tda.CashDiscount = dtproin.Rows[i]["CDISC"].ToString() != "" ? Convert.ToDouble(dtproin.Rows[i]["CDISC"].ToString()) : 0;
                    tda.FrigCharge = dtproin.Rows[i]["FREIGHT"].ToString() != "" ? Convert.ToDouble(dtproin.Rows[i]["FREIGHT"].ToString()) : 0;
                    tda.CGST = dtproin.Rows[i]["CGST"].ToString() != "" ? Convert.ToDouble(dtproin.Rows[i]["CGST"].ToString()) : 0;
                    tda.SGST = dtproin.Rows[i]["SGST"].ToString() != "" ? Convert.ToDouble(dtproin.Rows[i]["SGST"].ToString()) : 0;
                    tda.IGST = dtproin.Rows[i]["IGST"].ToString() != "" ? Convert.ToDouble(dtproin.Rows[i]["IGST"].ToString()) : 0;
                    tda.DiscAmount = dtproin.Rows[i]["DISCOUNT"].ToString() != "" ? Convert.ToDouble(dtproin.Rows[i]["DISCOUNT"].ToString()) : 0;
                    tda.Unit = dtproin.Rows[i]["PRIUNIT"].ToString();
                    //tda.ConFac = dtproin.Rows[i]["CF"].ToString();
                    //tda.CurrentStock = dtproin.Rows[i]["IQTY"].ToString() != "" ? Convert.ToDouble(dtproin.Rows[i]["IQTY"].ToString()) : 0;
                    tda.binid = dtproin.Rows[i]["BINID"].ToString();
                    tda.rate = dtproin.Rows[i]["RATE"].ToString() != "" ? Convert.ToDouble(dtproin.Rows[i]["RATE"].ToString()) : 0;
                    tda.Amount = dtproin.Rows[i]["AMOUNT"].ToString() != "" ? Convert.ToDouble(dtproin.Rows[i]["AMOUNT"].ToString()) : 0;
                    tda.TotalAmount = dtproin.Rows[i]["TOTAMT"].ToString() != "" ? Convert.ToDouble(dtproin.Rows[i]["TOTAMT"].ToString()) : 0;
                    TData.Add(tda);
                }
            }
            List<TermsItem> TData1 = new List<TermsItem>();
            TermsItem tda1 = new TermsItem();
            DataTable dtProInCons = SalesInvoiceService.TermsDetail(id);
            if (dtProInCons.Rows.Count > 0)
            {
                for (int i = 0; i < dtProInCons.Rows.Count; i++)
                {
                    tda1 = new TermsItem();
                    tda1.Terms = dtProInCons.Rows[i]["TANDC"].ToString();

                    TData1.Add(tda1);
                }
            }
            List<AreaItem> TData2 = new List<AreaItem>();
            AreaItem tda2 = new AreaItem();
            DataTable dtproOut = SalesInvoiceService.AreaDetail(id);
            if (dtproOut.Rows.Count > 0)
            {
                for (int i = 0; i < dtproOut.Rows.Count; i++)
                {
                    tda2 = new AreaItem();
                    tda2.Areaid = dtproOut.Rows[i]["STYPE"].ToString();
                    tda2.Add1 = dtproOut.Rows[i]["SADD1"].ToString();
                    tda2.Add2 = dtproOut.Rows[i]["SADD2"].ToString();
                    tda2.Add3 = dtproOut.Rows[i]["SADD3"].ToString();
                    tda2.State = dtproOut.Rows[i]["SSTATE"].ToString();
                    tda2.City = dtproOut.Rows[i]["SCITY"].ToString();
                    tda2.Phone = dtproOut.Rows[i]["SPHONE"].ToString();
                    tda2.PinCode = dtproOut.Rows[i]["SPINCODE"].ToString();
                    tda2.Email = dtproOut.Rows[i]["SEMAIL"].ToString();
                    tda2.Receiver = dtproOut.Rows[i]["SNAME"].ToString();
                    tda2.Fax = dtproOut.Rows[i]["SFAX"].ToString();


                    TData2.Add(tda2);
                }
            }

            ca.SIlst = TData;
            ca.TermsItemlst = TData1;
            ca.AreaItemlst = TData2;


            return View(ca);
        }
        public ActionResult MyListSIJSONGrid(string strStatus)
        {
            List<ListSIItems> Reg = new List<ListSIItems>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = SalesInvoiceService.GetAllSalesInvoice(strStatus);
            string jsonRow = string.Empty;
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
             
                jsonRow = "<a href=Jsonexport?id=" + dtUsers.Rows[i]["EXINVBASICID"].ToString() + "><img src='../Images/json.png' alt='Edit' /></a>";

                Reg.Add(new ListSIItems
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["EXINVBASICID"].ToString()),
                    docno = dtUsers.Rows[i]["INVNO"].ToString(),
                    currency = dtUsers.Rows[i]["MAINCURR"].ToString(),
                    date = dtUsers.Rows[i]["INVDATE"].ToString(),
                    party = dtUsers.Rows[i]["PARTYID"].ToString(),
                    jsonrow = jsonRow

                });
            }

            return Json(new
            {
                Reg
            });

        }
        public ActionResult GridRecordsSave(string selectedRecord)
        {
            string docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            string random = RandomString(7, false);
            string jsonString = "";
            string[] words = selectedRecord.Split(',');

            foreach (string id in words)
            {
                DataTable dt = new DataTable();
                dt = SalesInvoiceService.GetSIDetails(id);
               
                Jsonexport toDownload = new Jsonexport();
                string invno = "";
                string invdate = "";
                jsonString += "[";
                toDownload.Version = "1.1";
                transdetail trna = new transdetail();
                trna.TaxSch = "GST";
                trna.SupTyp = "B2B";
                trna.RegRev = "N";
                trna.IgstOnIntra = "N";
                toDownload.TranDtls = trna;
                docdetails docdt = new docdetails();
                docdt.Typ = "INV";
                if (dt.Rows.Count > 0)
                {
                    invno = dt.Rows[0]["INVNO"].ToString();
                    invdate = dt.Rows[0]["invdate"].ToString();
                    docdt.No = invno;
                    docdt.Dt = invdate;
                }
                toDownload.DocDtls = docdt;
                sellerdetails sdt = new sellerdetails();
                sdt.Gstin = "33AAACT6771E1ZG";
                sdt.LglNm = "THE ARASAN ALUMINIUM INDUSTRIES (P) LTD";
                sdt.TrdNm = "THE ARASAN ALUMINIUM INDUSTRIES (P) LTD";
                sdt.Addr1 = "3/111 S.No 556 to 568, Melamathur Village";
                sdt.Addr2 = "Virudhunagar Road";
                sdt.Loc = "SIVAKSI";
                sdt.Stcd = "33";
                sdt.Pin = "626005";
                sdt.Em = "info@arasanaluminium.com";
                sdt.Ph = "917867000916";
                toDownload.SellerDtls = sdt;
                buyerdetails bdt = new buyerdetails();
                if (dt.Rows.Count > 0)
                {
                    bdt.Gstin = dt.Rows[0]["GSTNO"].ToString();
                    bdt.LglNm = dt.Rows[0]["PARTYID"].ToString();
                    bdt.TrdNm = dt.Rows[0]["PARTYID"].ToString();
                    bdt.pos = dt.Rows[0]["SCODE"].ToString();
                    bdt.Addr1 = dt.Rows[0]["ADD1"].ToString();
                    bdt.Addr2 = dt.Rows[0]["ADD2"].ToString();
                    bdt.Loc = dt.Rows[0]["CITY"].ToString();
                    bdt.Stcd = dt.Rows[0]["SCODE"].ToString();
                    bdt.Pin = dt.Rows[0]["PINCODE"].ToString();
                    bdt.Em = dt.Rows[0]["EMAIL"].ToString();
                    bdt.Ph = dt.Rows[0]["PHONENO"].ToString();
                }
                toDownload.BuyerDtls = bdt;
                valuedtails vdt = new valuedtails();
                if (dt.Rows.Count > 0)
                {
                    vdt.AssVal = dt.Rows[0]["GROSS"].ToString();
                    vdt.CgstVal = dt.Rows[0]["BCGST"].ToString();
                    vdt.SgstVal = dt.Rows[0]["BSGST"].ToString();
                    vdt.IgstVal = dt.Rows[0]["BIGST"].ToString();
                    vdt.CesVal = "0";
                    vdt.StCesVal = "0";
                    vdt.Discount = dt.Rows[0]["BDISCOUNT"].ToString();
                    vdt.OthChrg = dt.Rows[0]["BFREIGHT"].ToString();
                    vdt.RndOffAmt = dt.Rows[0]["ROFF"].ToString();
                    vdt.TotInvVal = dt.Rows[0]["NET"].ToString();
                    vdt.TotInvValFc = "0";
                }
                toDownload.ValDtls = vdt;
                shipdetails shdt = new shipdetails();
                if (dt.Rows.Count > 0)
                {
                    shdt.Gstin = dt.Rows[0]["GSTNO"].ToString();
                    shdt.LglNm = dt.Rows[0]["PARTYID"].ToString();
                    shdt.TrdNm = dt.Rows[0]["PARTYID"].ToString();
                    shdt.Addr1 = dt.Rows[0]["ADD1"].ToString();
                    shdt.Addr2 = dt.Rows[0]["ADD2"].ToString();
                    shdt.Loc = dt.Rows[0]["CITY"].ToString();
                    shdt.Stcd = dt.Rows[0]["SCODE"].ToString();
                    shdt.Pin = dt.Rows[0]["PINCODE"].ToString();
                }
                toDownload.ShipDtls = shdt;
                ewbdetails edt = new ewbdetails();
                DataTable dg = datatrans.GetData("Select PARTYID,GSTNO from PARTYMAST where PARTYMASTID='" + dt.Rows[0]["TRANSP"].ToString() + "'");
                if (dt.Rows.Count > 0)
                {
                    edt.TransId = dg.Rows[0]["GSTNO"].ToString();
                    edt.TransName = dg.Rows[0]["PARTYID"].ToString();
                    edt.TransMode = "1";
                    edt.Distance = dt.Rows[0]["TDIST"].ToString();
                    edt.VehNo = dt.Rows[0]["VNO"].ToString();
                    edt.VehType = "R";

                }
                toDownload.EwbDtls = edt;
                DataTable dts = new DataTable();
                dts = SalesInvoiceService.GetSIITEMDetails(id);
                List<ItemDetails> TData = new List<ItemDetails>();
                ItemDetails tda = new ItemDetails();
                double gstrate = 0;
                if (dts.Rows.Count > 0)
                {
                    for (int i = 0; i < dts.Rows.Count; i++)
                    {
                        tda = new ItemDetails();
                        tda.SlNo = (i + 1).ToString();
                        tda.PrdDesc = dts.Rows[i]["ITEMSPEC"].ToString();
                        tda.IsServc = "N";
                        tda.HsnCd = dts.Rows[i]["HSN"].ToString();

                        tda.Qty = dts.Rows[i]["Qty"].ToString();
                        tda.FreeQty = "";
                        tda.Unit = dts.Rows[i]["UNITID"].ToString();
                        tda.UnitPrice = dts.Rows[i]["RATE"].ToString();
                        tda.TotAmt = dts.Rows[i]["AMOUNT"].ToString();
                        tda.Discount = dts.Rows[i]["DISCOUNT"].ToString();
                        tda.PreTaxVal = dts.Rows[i]["AMOUNT"].ToString();
                        tda.AssAmt = dts.Rows[i]["AMOUNT"].ToString();
                        if (Convert.ToDouble(dts.Rows[i]["IGSTP"].ToString()) > 0)
                        {
                            gstrate = Convert.ToDouble(dts.Rows[i]["IGSTP"].ToString());
                        }
                        else
                        {
                            gstrate = Convert.ToDouble(dts.Rows[i]["CGSTP"].ToString()) + Convert.ToDouble(dts.Rows[i]["SGSTP"].ToString());
                        }
                        tda.GstRt = gstrate.ToString();
                        tda.IgstAmt = dts.Rows[i]["IGST"].ToString();
                        tda.CgstAmt = dts.Rows[i]["CGST"].ToString();
                        tda.SgstAmt = dts.Rows[i]["SGST"].ToString();
                        tda.OthChrg = "";
                        tda.TotItemVal = dts.Rows[i]["TOTAMT"].ToString();
                        tda.OrdLineRef = "";
                        tda.OrgCntry = "";
                        tda.PrdSlNo = dts.Rows[i]["DRUMDESC"].ToString();
                        TData.Add(tda);


                    }
                }
                toDownload.ItemList = TData;
                jsonString += JsonSerializer.Serialize(toDownload);
                //string jsonString = JsonSerializer.Serialize(toDownload);
                jsonString += "]";
                jsonString += "\n";
            }
            var fileName = "pendingeinv" + "_" + random + "_" + docdate + ".json";
            var mimeType = "text/plain";
            var fileBytes = Encoding.ASCII.GetBytes(jsonString);
            return new FileContentResult(fileBytes, mimeType)
            {
                FileDownloadName = fileName
            };


        }
        public JsonResult GetItemJSON(string locid)
        {
            DepotInvoiceItem model = new DepotInvoiceItem();
            model.Itemlst = BindItemlst(locid);
            return Json(BindItemlst(locid));

        }
        public JsonResult GetTeamsJSON()
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindTerms());
        }
        public JsonResult GetWoJSON(string partyid,string locid)
        {
            return Json(Bindworkorder(partyid, locid));
        }
        public JsonResult GetjobschJSON(string jobid)
        {
            return Json(BindJobSch(jobid));
        }
        public List<SelectListItem> BindTerms()
        {
            try
            {
                DataTable dtDesg = SalesInvoiceService.GetTerms();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["TANDC"].ToString(), Value = dtDesg.Rows[i]["TANDCDETAILID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindJobSch(string jobid)
        {
            try
            {
                DataTable dtDesg = SalesInvoiceService.GetSchedule(jobid);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SCHNO"].ToString(), Value = dtDesg.Rows[i]["JOSCHEDULEID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> Bindworkorder(string partyid,string locid)
        {
            try
            {
                DataTable dtDesg = datatrans.GetWOParty(partyid, locid);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["JOBASICID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindTrans()
        {
            try
            {

                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "NONE", Value = "NONE" });
                lstdesg.Add(new SelectListItem() { Text = "BY AUTO", Value = "BY AUTO" });
                lstdesg.Add(new SelectListItem() { Text = "BY VAN", Value = "BY VAN" });
                lstdesg.Add(new SelectListItem() { Text = "BY TRUCK", Value = "BY TRUCK" });
                lstdesg.Add(new SelectListItem() { Text = "BY LORRY", Value = "BY LORRY" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetItemDetail(string schid, string locid)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string unit = "";
                string CF = "";
                string price = "";
                string binno = "";
                string binname = "";
                string itemname = "";
                string rate = datatrans.GetDataString("select D.RATE from JODETAIL D,JOSCHEDULE S where S.PARENTRECORDID=D.JODETAILID AND S.JOSCHEDULEID='" + schid + "'");
                string qty= datatrans.GetDataString("select S.SCHQTY from JOSCHEDULE S where  S.JOSCHEDULEID='" + schid + "'");
                string ItemId = datatrans.GetDataString("select SCHITEMID from JOSCHEDULE WHERE JOSCHEDULEID='"+ schid + "'");
                dt = datatrans.GetItemDetails(ItemId);
                string stock = SalesInvoiceService.GetDrumStock(ItemId, locid);
                if (dt.Rows.Count > 0)
                {

                    unit = dt.Rows[0]["UNITID"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();
                    //binno = dt.Rows[0]["BINNO"].ToString();
                    //binname = datatrans.GetDataString("select BINID from BINBASIC where BINBASICId='" + dt.Rows[0]["BINNO"].ToString() + "'"); ;
                    dt1 = SalesInvoiceService.GetItemCF(ItemId, dt.Rows[0]["UNITMASTID"].ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        CF = dt1.Rows[0]["CF"].ToString();
                    }
                    itemname= dt.Rows[0]["ITEMID"].ToString();
                }

                var result = new { unit = unit, CF = CF, price = price, stock = stock , itemname = itemname, itemid= ItemId, rate= rate, qty= qty };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetItemRate(string ItemId, string custid, string ratec, string ordtype)
        {
            try
            {
                DataTable dt = new DataTable();
                string price = datatrans.GetDataString("Select nvl(sum(rate),0) rate , 1 AS SNO from (SELECT D.RATE FROM RATEBASIC B, RATEDETAIL D, ITEMMASTER I WHERE D.RCODE = '" + ratec + "' AND I.ITEMMASTERID = '" + ItemId + "' AND 'ORDER' ='" + ordtype + "' AND D.ITEMID = I.ITEMMASTERID AND B.RATEBASICID = D.RATEBASICID ANd B.VALIDFROM = (Select max(Validfrom) from Ratebasic R1 Where R1.RATECODE = '" + ratec + "' ANd R1.VALIDFROM <='" + DateTime.Now.ToString("dd-MMM-yyyy") + "') Union SELECT(-disc) FROM PARTYADVDISC WHERE PARTYMASTID ='" + custid + "' and active = 'Yes' and RATECODE = '" + ratec + "')");


                var result = new { price = price };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ListDrumSelection(string itemid, string locid)
        {
            List<DDrumdetails> EnqChkItem = new List<DDrumdetails>();
            DataTable dtEnq = new DataTable();
            dtEnq = SalesInvoiceService.GetDrumDetails(itemid, locid);
            for (int i = 0; i < dtEnq.Rows.Count; i++)
            {

                EnqChkItem.Add(new DDrumdetails
                {
                    lotno = dtEnq.Rows[i]["LOTNO"].ToString(),
                    drumno = dtEnq.Rows[i]["DRUMNO"].ToString(),
                    qty = dtEnq.Rows[i]["QTY"].ToString(),
                    rate = dtEnq.Rows[i]["RATE"].ToString(),
                    invid = dtEnq.Rows[i]["PLotmastID"].ToString()
                });
            }

            return Json(new
            {
                EnqChkItem
            });
        }
        public ActionResult DrumSelection(string schid, string rowid)
        {
            Drumdetailstable ca = new Drumdetailstable();
            List<DDrumdetails> TData = new List<DDrumdetails>();
            DDrumdetails tda = new DDrumdetails();
            DataTable dtEnq = new DataTable();
            dtEnq = SalesInvoiceService.GetDrumDetails(schid);
            for (int i = 0; i < dtEnq.Rows.Count; i++)
            {
                tda = new DDrumdetails();
                tda.lotno = dtEnq.Rows[i]["LOTNO"].ToString();
                tda.drumno = dtEnq.Rows[i]["DRUMNO"].ToString();
                tda.qty = dtEnq.Rows[i]["QTY"].ToString();
                tda.rate = dtEnq.Rows[i]["RATE"].ToString();
                //tda.invid = dtEnq.Rows[i]["PLotmastID"].ToString();
                TData.Add(tda);
            }
            ca.Drumlst = TData;
            return View(ca);
        }
        
        public JsonResult GetBinJSON()
        {
           return Json(BindBin());
        }
        public List<SelectListItem> Bindempty()
        {
            List<SelectListItem> lstdesg = new List<SelectListItem>();
            return lstdesg;
        }
            
        public List<SelectListItem> BindItemlst(string locid)
        {
            try
            {
                DataTable dtDesg = SalesInvoiceService.GetFGItem(locid);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["ITEMMASTERID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindItemGrplst()
        {
            try
            {
                DataTable dtDesg = datatrans.GetItemSubGrp();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SGCODE"].ToString(), Value = dtDesg.Rows[i]["ITEMSUBGROUPID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> Tname()
        {
            try
            {
                DataTable dtDesg = SalesInvoiceService.GetTname();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "NONE", Value = "0" });
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTYID"].ToString(), Value = dtDesg.Rows[i]["PARTYMASTID"].ToString() });
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
        public List<SelectListItem> BindBin()
        {
            try
            {
                DataTable dtDesg = datatrans.GetBinMaster();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["BINID"].ToString(), Value = dtDesg.Rows[i]["BINBASICID"].ToString() });
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
                DataTable dtDesg = datatrans.GetCustomer();
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
        public List<SelectListItem> BindInVoiceType()
        {
            try
            {

                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "GST SALES", Value = "GST SALES" });
                lstdesg.Add(new SelectListItem() { Text = "IGST SALES", Value = "IGST SALES" });
                lstdesg.Add(new SelectListItem() { Text = "SCRAP INVOICE", Value = "SCRAP INVOICE" });
                lstdesg.Add(new SelectListItem() { Text = "TRANSFER STOCK", Value = "TRANSFER STOCK" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindDispatchType()
        {
            try
            {

                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "Air", Value = "Air" });
                lstdesg.Add(new SelectListItem() { Text = "Ship", Value = "Ship" });
                lstdesg.Add(new SelectListItem() { Text = "Rail", Value = "Rail" });
                lstdesg.Add(new SelectListItem() { Text = "Road", Value = "Road" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindType()
        {
            try
            {

                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "EXPORT", Value = "EXPORT" });
                lstdesg.Add(new SelectListItem() { Text = "NEIGHBOUR", Value = "NEIGHBOUR" });
                lstdesg.Add(new SelectListItem() { Text = "COOLY INVOICE", Value = "COOLY INVOICE" });
                lstdesg.Add(new SelectListItem() { Text = "SALES", Value = "SALES" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindOrder()
        {
            try
            {

                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "ORDER", Value = "ORDER" });
                lstdesg.Add(new SelectListItem() { Text = "SAMPLE", Value = "SAMPLE" });
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

                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "R", Value = "R" });
                lstdesg.Add(new SelectListItem() { Text = "L", Value = "L" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindDocument()
        {
            try
            {

                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "BY HAND", Value = "BY HAND" });
                lstdesg.Add(new SelectListItem() { Text = "COURIER", Value = "COURIER" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindInspect()
        {
            try
            {

                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "THIRD PARTY", Value = "THIRD PARTY" });
                lstdesg.Add(new SelectListItem() { Text = "CUSTOMER", Value = "CUSTOMER" });
                lstdesg.Add(new SelectListItem() { Text = "OWN", Value = "OWN" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult DeleteMR(string tag, string id)
        {

            string flag = SalesInvoiceService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListSalesInvoice");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListSalesInvoice");
            }
        }
        public ActionResult GetGSTDetail(string ItemId, string custid)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string sgst = "";

                string hsn = "";
                if (ItemId == "1")
                {
                    hsn = "996519";
                }
                else
                {
                    dt = SalesInvoiceService.GetHsn(ItemId);
                    if (dt.Rows.Count > 0)
                    {
                        hsn = dt.Rows[0]["HSN"].ToString();
                    }
                }
                if (ItemId == "1")
                {
                    sgst = "18";
                }
                else
                {
                    dt1 = SalesInvoiceService.GetGSTDetails(hsn);
                    if (dt1.Rows.Count > 0)
                    {

                        sgst = dt1.Rows[0]["GSTP"].ToString();


                    }
                }

                string cmpstate = datatrans.GetDataString("select STATE from CONTROL");

                string type = "";

                string partystate = datatrans.GetDataString("select STATE from PARTYMAST where PARTYMASTID='" + custid + "'");
                if (partystate == cmpstate)
                {
                    type = "GST";
                }
                else
                {
                    type = "IGST";
                }

                var result = new { sgst = sgst, type = type, hsn = hsn };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Getdrumdetails(string schid)
        {
            try
            {
                string drumid = "";
               DataTable dtEnq = SalesInvoiceService.GetDrumDetails(schid);
                for (int i = 0; i < dtEnq.Rows.Count; i++)
                {
                   string dmid=dtEnq.Rows[i]["PLSTOCKID"].ToString();
                    drumid = String.Format("{0},{1}", dmid, drumid);
                }
                drumid = drumid.TrimEnd(',');

                var result = new { drumid = drumid };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetTrefficDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string treffic = "";

                dt = SalesInvoiceService.GetTrefficDetails(ItemId);
                if (dt.Rows.Count > 0)
                {
                    treffic = dt.Rows[0]["TARIFFID"].ToString();
                }


                var result = new { treffic = treffic };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetAddrtype(string custid)
        {
            try
            {
                DataTable dt = new DataTable();
                string cmpstate = datatrans.GetDataString("select STATE from CONTROL");

                string type = "";

                string partystate = datatrans.GetDataString("select STATE from PARTYMAST where PARTYMASTID='" + custid + "'");
                if (partystate == cmpstate)
                {
                    type = "GST";
                }
                else
                {
                    type = "IGST";
                }


                var result = new { type = type };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetItemSpecDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string type = "";
                string spec = "";

                dt = SalesInvoiceService.GetItemSpecDetails(ItemId);
                if (dt.Rows.Count > 0)
                {
                    type = dt.Rows[0]["ITEMTYPE"].ToString();
                    spec = dt.Rows[0]["ITEMSPEC"].ToString();
                }


                var result = new { type = type, spec = spec };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetAreaDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string reciver = "";
                //string address = "";
                string state = "";
                string city = "";
                string pincode = "";
                string phone = "";
                string email = "";
                string fax = "";
                string add1 = "";
                string add2 = "";
                string add3 = "";


                dt = SalesInvoiceService.GetAreaDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    reciver = dt.Rows[0]["ADDBOOKCOMPANY"].ToString();
                    //address = dt.Rows[0]["address"].ToString();
                    state = dt.Rows[0]["SSTATE"].ToString();
                    city = dt.Rows[0]["SCITY"].ToString();
                    pincode = dt.Rows[0]["SPINCODE"].ToString();
                    phone = dt.Rows[0]["SPHONE"].ToString();
                    email = dt.Rows[0]["SEMAIL"].ToString();
                    fax = dt.Rows[0]["SFAX"].ToString();
                    add1 = dt.Rows[0]["SADD1"].ToString();
                    add2 = dt.Rows[0]["SADD2"].ToString();
                    add3 = dt.Rows[0]["SADD3"].ToString();

                }

                var result = new { reciver = reciver, state = state, city = city, pincode = pincode, phone = phone, email = email, fax = fax, add1 = add1, add2 = add2, add3 = add3 };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetPartyaddrJSON(string custid)
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindArea(custid));
        }



        public List<SelectListItem> BindArea(string custid)
        {
            try
            {
                DataTable dtDesg = SalesInvoiceService.GetArea(custid);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ADDBOOKTYPE"].ToString(), Value = dtDesg.Rows[i]["ADDBOOKTYPE"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

        public ActionResult GetDocidDetail(string locid, string ordtype)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string doc = "";

                dt = datatrans.GetSequence("exinv", locid);
                if (dt.Rows.Count > 0)
                {
                    doc = dt.Rows[0]["doc"].ToString();
                }


                var result = new { doc = doc };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetAdvDetails(string custid)
        {
            try
            {
                DepotInvoice cy = new DepotInvoice();
                DataTable dtParty = datatrans.GetData("select distinct P.CREDITDAYS,P.CREDITLIMIT,P.GSTNO,P.PARTYNAME,P.ACCOUNTNAME,P.PartyGroup,A.ratecode,a.limit from PARTYMAST P,PartyAdvDisc A Where P.PartyMastID =A.PartyMastID(+) and A.active = 'Yes' and P.PARTYMASTID='" + custid + "' union select distinct P.CREDITDAYS,P.CREDITLIMIT,P.GSTNO,P.PARTYNAME,P.ACCOUNTNAME,P.PartyGroup,A.Bratecode,0 from PARTYMAST P,PartymastBRCode A Where P.PartyMastID =A.PartyMastID(+) and P.PARTYMASTID='" + custid + "' and 0=(select count(A.ratecode) from PARTYMAST P,PartyAdvDisc A Where P.PartyMastID =A.PartyMastID(+) and A.active = 'Yes' and P.PARTYMASTID='" + custid + "')");
                cy.arc = dtParty.Rows[0]["ratecode"].ToString();
                cy.crlimit = (long)Convert.ToDouble(dtParty.Rows[0]["CREDITLIMIT"].ToString());
                cy.crd = (long)Convert.ToDouble(dtParty.Rows[0]["CREDITDAYS"].ToString());
                cy.PartyG = dtParty.Rows[0]["PartyGroup"].ToString();
                cy.limit = (long)Convert.ToDouble(dtParty.Rows[0]["limit"].ToString());
                if (cy.limit > 0)
                {
                    DataTable psaledt = datatrans.GetData("Select nvl(sum(net),0) nets from ( Select sum(net) net from exinvbasic e, partymast p,partyadvdisc d where e.docdate between D.SD and D.ED and e.RateCode ='" + cy.arc + "' and e.partyid = P.PARTYMASTID and(P.PARTYMASTID = '" + custid + "' or(P.PARTYGROUP ='" + cy.PartyG + "' and 'None' <> '" + cy.PartyG + "')) and D.RATECODE = E.RATECODE and D.PARTYMASTID = P.PARTYMASTID  and e.EORDTYPE='ORDER' Union All Select sum(net) net from Depinvbasic e, partymast p,partyadvdisc d where e.docdate between D.SD and D.ED and e.RateCode = '" + cy.arc + "' and e.partyid = P.PARTYMASTID and(P.PARTYMASTID ='" + custid + "' or(P.PARTYGROUP = '" + cy.PartyG + "' and 'None' <> '" + cy.PartyG + "')) and D.RATECODE = E.RATECODE and D.PARTYMASTID = P.PARTYMASTID  and e.EORDTYPE='ORDER')");
                    cy.asale = (long)Convert.ToDouble(psaledt.Rows[0]["nets"].ToString());
                }
                else cy.asale = 0;

                var result = new { arc = cy.arc, partyg = cy.PartyG, limit = cy.limit, asale = cy.asale };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetNarrDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string narr = "";
                string narr1 = "";
                dt = SalesInvoiceService.GetNarr(ItemId);
                if (dt.Rows.Count > 0)
                {
                    narr = dt.Rows[0]["PARTYNAME"].ToString();
                }
                narr1 = "Invoiced To " + narr;

                var result = new { narr1 = narr1 };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult DeleteDI(string tag, string id)
        {

            string flag = SalesInvoiceService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListSalesInvoice");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListSalesInvoice");
            }
        }
    
        public async Task<IActionResult> Print(string id)
        {

            string mimtype = "";
            int extension = 1;

            System.Data.DataSet ds = new System.Data.DataSet();
            var path = $"{this._WebHostEnvironment.WebRootPath}\\Reports\\TaxInvoice.rdlc";
            Dictionary<string, string> Parameters = new Dictionary<string, string>();
            //  Parameters.Add("rp1", " Hi Everyone");
            var basic = await SalesInvoiceService.GetBasicItem(id);
            var Detail = await SalesInvoiceService.GetExinvItemDetail(id);
            //var terms = await ProFormaInvoiceService.GetPinvtermsDetail(id);
          
          
            AspNetCore.Reporting.LocalReport localReport = new AspNetCore.Reporting.LocalReport(path);
            localReport.AddDataSource("ExinvBasic", basic);
            localReport.AddDataSource("ExinvDetail", Detail);

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            string data = "QR Code";
            var qrCodeImage = GenerateQRCode(data);
            Parameters.Add("QRCodeImage", ConvertImageToBase64(qrCodeImage));

            var result = localReport.Execute(RenderType.Pdf, extension, Parameters, mimtype);

            return File(result.MainStream, "application/Pdf");

        }

        public Bitmap GenerateQRCode(string data)
        {
            int width = 20; int height = 20;
            var barcodeWriter = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = height,
                    Width = width
                }
            };

            var pixelData = barcodeWriter.Write(data);
            using (var bitmap = new Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            {
                var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                try
                {
                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }
                return bitmap;
            }
        }

        private string ConvertImageToBase64(Bitmap image)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, ImageFormat.Png);
                    byte[] byteImage = ms.ToArray();
                    return Convert.ToBase64String(byteImage);
                }
            }

        }
}
