using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Arasan.Interface;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Models;
using Arasan.Services;
using AspNetCore.Reporting;
//using DocumentFormat.OpenXml.Vml;
//using DocumentFormat.OpenXml.Wordprocessing;
//using DocumentFormat.OpenXml.Office2010.Excel;


namespace Arasan.Controllers
{
    public class DirectPurchaseController : Controller
    {
        IDirectPurchase directPurchase;
        IConfiguration? _configuratio;
        private string? _connectionString;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        DataTransactions datatrans;
        public DirectPurchaseController(IDirectPurchase _directPurchase ,IConfiguration _configuratio, IWebHostEnvironment WebHostEnvironment)
        {
            directPurchase = _directPurchase;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
            this._WebHostEnvironment = WebHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }
        public IActionResult DirectPurchase(string id)
        {
            DirectPurchase ca = new DirectPurchase();
            ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];
            ca.user = Request.Cookies["UserName"];
            ca.Currency = "1";
            ca.Suplst = BindSupplier("AGAINST PURCHASE INDENT");
            ca.RefDate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.Curlst = BindCurrency();
            ca.Loclst = GetLoc();
            ca.putypelst = BindPuType();
            string loc = ca.Location;
            //ViewBag.locdisp = ca.Location;
            ca.Vocherlst = BindVocher();
            ca.Voucher = "Purchase";
            ca.DocDate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence1("dp", loc);
            if (dtv.Rows.Count > 0)
            {
                ca.DocNo = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["LASTNO"].ToString();
            }
            List<DirItem> TData = new List<DirItem>();
            DirItem tda = new DirItem();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new DirItem();
                    tda.Indentlst=BindEmpty();
                    //tda.ItemGrouplst = BindItemGrplst();
                    tda.Itemlst = BindItemlst("", "AGAINST PURCHASE INDENT");
                    tda.gstlst = Bindgstlst("");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {

                // ca = directPurchase.GetDirectPurById(id);


                DataTable dt = new DataTable();
                double total = 0;
                dt = directPurchase.GetDirectPurchase(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.Supplier = dt.Rows[0]["PARTYID"].ToString();
                    ca.DocNo = dt.Rows[0]["DOCID"].ToString();
                    ca.ID = id;
                    ca.Currency = dt.Rows[0]["MAINCURRENCY"].ToString();
                    ca.RefDate = dt.Rows[0]["REFDT"].ToString();
                    ca.Voucher = dt.Rows[0]["VOUCHER"].ToString();
                    ca.Location = dt.Rows[0]["LOCID"].ToString();
                    ca.Narration = dt.Rows[0]["NARR"].ToString();
                    ca.LRCha = Convert.ToDouble(dt.Rows[0]["LRCH"].ToString() == "" ? "0" : dt.Rows[0]["LRCH"].ToString());
                    ca.DelCh = Convert.ToDouble(dt.Rows[0]["DELCH"].ToString() == "" ? "0" : dt.Rows[0]["DELCH"].ToString());
                    ca.Other = Convert.ToDouble(dt.Rows[0]["OTHERCH"].ToString() == "" ? "0" : dt.Rows[0]["OTHERCH"].ToString());
                    ca.Frieghtcharge = Convert.ToDouble(dt.Rows[0]["FREIGHT"].ToString() == "" ? "0" : dt.Rows[0]["FREIGHT"].ToString());
                    ca.Disc = Convert.ToDouble(dt.Rows[0]["OTHERDISC"].ToString() == "" ? "0" : dt.Rows[0]["OTHERDISC"].ToString());
                    ca.Round = Convert.ToDouble(dt.Rows[0]["ROUNDM"].ToString() == "" ? "0" : dt.Rows[0]["ROUNDM"].ToString());

                    ca.Gross = Convert.ToDouble(dt.Rows[0]["GROSS"].ToString() == "" ? "0" : dt.Rows[0]["GROSS"].ToString());
                    ca.Net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());

                }
                DataTable dt2 = new DataTable();
                dt2 = directPurchase.GetDirectPurchaseItemDetails(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new DirItem();
                        double toaamt = 0;
                        tda.ItemGrouplst = BindItemGrplst();
                        DataTable dt3 = new DataTable();
                        dt3 = datatrans.GetItemSubGroup(dt2.Rows[i]["ITEMID"].ToString());
                        if (dt3.Rows.Count > 0)
                        {
                            tda.ItemGroupId = dt3.Rows[0]["SUBGROUPCODE"].ToString();
                        }
                        //tda.Itemlst = BindItemlst(tda.ItemGroupId);
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
                        //tda.PURLst = BindPurType();
                        //tda.unitprim= dt2.Rows[i]["UNITID"].ToString();

                        tda.Disc = Convert.ToDouble(dt2.Rows[i]["DISC"].ToString() == "" ? "0" : dt2.Rows[i]["DISC"].ToString());
                        tda.DiscAmount = Convert.ToDouble(dt2.Rows[i]["DISCAMOUNT"].ToString() == "" ? "0" : dt2.Rows[i]["DISCAMOUNT"].ToString());

                        tda.FrigCharge = Convert.ToDouble(dt2.Rows[i]["IFREIGHTCH"].ToString() == "" ? "0" : dt2.Rows[i]["IFREIGHTCH"].ToString());
                        tda.TotalAmount = Convert.ToDouble(dt2.Rows[i]["TOTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["TOTAMT"].ToString());
                        tda.CGSTP = Convert.ToDouble(dt2.Rows[i]["CGSTP"].ToString() == "" ? "0" : dt2.Rows[i]["CGSTP"].ToString());
                        tda.SGSTP = Convert.ToDouble(dt2.Rows[i]["SGSTP"].ToString() == "" ? "0" : dt2.Rows[i]["SGSTP"].ToString());
                        tda.IGSTP = Convert.ToDouble(dt2.Rows[i]["IGSTP"].ToString() == "" ? "0" : dt2.Rows[i]["IGSTP"].ToString());
                        tda.CGST = Convert.ToDouble(dt2.Rows[i]["CGST"].ToString() == "" ? "0" : dt2.Rows[i]["CGST"].ToString());
                        tda.SGST = Convert.ToDouble(dt2.Rows[i]["SGST"].ToString() == "" ? "0" : dt2.Rows[i]["SGST"].ToString());
                        tda.IGST = Convert.ToDouble(dt2.Rows[i]["IGST"].ToString() == "" ? "0" : dt2.Rows[i]["IGST"].ToString());
                        // tda.PurType = dt2.Rows[i]["PURTYPE"].ToString();
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }
                ca.Net = Math.Round(total, 2);

            }
            ca.DirLst = TData;
            return View(ca);

        }
        public IActionResult DirectPurchaseDetails(string id)
        {
            IEnumerable<DirItem> cmp = directPurchase.GetAllDirectPurItem(id);
            return View(cmp);
        }
        [HttpPost]
        public ActionResult DirectPurchase(DirectPurchase Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = directPurchase.DirectPurCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "DirectPurchase Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "DirectPurchase Updated Successfully...!";
                    }
                    return RedirectToAction("ListDirectPurchase");
                }

                else
                {
                    ViewBag.PageTitle = "Edit DirectPurchase";
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
        public ActionResult MyListDirectPurchaseGrid(string strStatus)
        {
            List<DirectPurchaseItems> Reg = new List<DirectPurchaseItems>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)directPurchase.GetAllDirectPurchases(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string MailRow = string.Empty;
                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string MoveToGRN = string.Empty;
                string Print = string.Empty;
                string Account = string.Empty;
               
                MailRow = "<a href=DirectPurchase?tag=Del&id=" + dtUsers.Rows[i]["DPBASICID"].ToString() + "><img src='../Images/mail_icon.png' alt='Send Email' /></a>";
                EditRow = "<a href=DirectPurchase?id=" + dtUsers.Rows[i]["DPBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                Print = "<a href=Print?id=" + dtUsers.Rows[i]["DPBASICID"].ToString() + " target='_blank'><img src='../Images/pdficon.png' alt='View Details' width='20' /></a>";

                DeleteRow = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["DPBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                if(dtUsers.Rows[i]["STATUS"].ToString()== "GRN Generated")
                {
                    MoveToGRN = "<img src='../Images/tick.png' alt='View Details' width='20' />";
                }
                else
                {
                    MoveToGRN = "<a href=MoveToGRN?id=" + dtUsers.Rows[i]["DPBASICID"].ToString() + "><img src='../Images/move_quote.png' alt='View Details' width='20' /></a>";
                }
                
                    if (dtUsers.Rows[i]["IS_ACCOUNT"].ToString() == "N")
                {
                    Account = "<a href=DPACC?id=" + dtUsers.Rows[i]["DPBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/profit.png' alt='View Details' width='20' /></a>";
                }
                    //if (Reg.Status == "GRN Generated")
                //{
                //    @Html.DisplayFor(Reg => Reg.Status);
                //}
                //else
                //{
                //MoveToGRN = "<a href=MoveToGRN?id=" + dtUsers.Rows[i]["DPBASICID"].ToString() + "><img src='../Images/move_quote.png' alt='View Details' width='20' /></a>";

                //}
                Reg.Add(new DirectPurchaseItems
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["DPBASICID"].ToString()),
                    branch = dtUsers.Rows[i]["BRANCHID"].ToString(),
                    supplier = dtUsers.Rows[i]["PARTYNAME"].ToString(),
                    docNo = dtUsers.Rows[i]["DOCID"].ToString(),
                    docDate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    gross = dtUsers.Rows[i]["GROSS"].ToString(),
                    net = dtUsers.Rows[i]["NET"].ToString(),
                    mailrow = MailRow,
                    editrow = EditRow,
                    delrow = DeleteRow,
                    move = MoveToGRN,
                    print = Print,
                    account= Account

                });
            }

            return Json(new
            {
                Reg
            });

        }
        public IActionResult DPAccount(string id)
        {
            GRN grn = new GRN();
            DataTable dt = new DataTable();
            string SvSql = "select SUM(CGST) CGST,SUM(SGST) SGST,SUM(IGST) IGST,SUM(TOTAMT) AS NET,SUM(IFREIGHTCH * QTY) as FRIEGHT,SUM(IOTHERCH * QTY) otherch,SUM(AMOUNT) as GROSS,SUM(IPKNFDCH * QTY)PKNFDCH,SUM(DISCAMOUNT) as DISC from DPDETAIL where DPBASICID='" + id + "'";
            dt = datatrans.GetData(SvSql);
            grn.GRNID = id;
            grn.RefDate = DateTime.Now.ToString("dd-MMM-yyyy");
            grn.createdby = Request.Cookies["UserId"];
            DataTable dtnat = datatrans.GetData("select I.ITEMID,BL.QTY,U.UNITID from DPDETAIL BL,ITEMMASTER I,UNITMAST U where I.ITEMMASTERID=BL.ITEMID AND U.UNITMASTID=I.PRIUNIT AND DPBASICID='" + id + "'");
            grn.Vmemo = "BEING " + dtnat.Rows[0]["ITEMID"].ToString() + "-" + dtnat.Rows[0]["QTY"].ToString() + dtnat.Rows[0]["UNITID"].ToString() + "PURCHASED.";
            List<GRNAccount> TData = new List<GRNAccount>();
            GRNAccount tda = new GRNAccount();
            double totalcredit = 0;
            double totaldebit = 0;
            DataTable dtdet = datatrans.GetData("select ITEMACC,SUM(AMOUNT) as GROSS from DPDETAIL WHERE DPBASICID='"+ id + "' GROUP BY ITEMACC");
            DataTable dtacc = new DataTable();
            dtacc = datatrans.GetGRNconfig();
            string frieghtledger = "";
            string discledger = "";
            string roundoffledger = "";
            string cgstledger = "";
            string sgstledger = "";
            string igstledger = "";
            string packingledger = "";
            if (dtacc.Rows.Count > 0)
            {
                grn.ADCOMPHID = dtacc.Rows[0]["ADCOMPHID"].ToString();
                for (int i = 0; i < dtacc.Rows.Count; i++)
                {
                    if (dtacc.Rows[i]["ADTYPE"].ToString() == "FREIGHT CHARGES")
                    {
                        frieghtledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                    }
                    if (dtacc.Rows[i]["ADTYPE"].ToString().Contains("PACKING CHARGES"))
                    {
                        packingledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                    }
                    if (dtacc.Rows[i]["ADTYPE"].ToString() == "DISCOUNT")
                    {
                        discledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                    }
                    if (dtacc.Rows[i]["ADTYPE"].ToString() == "ROUND OFF")
                    {
                        roundoffledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                    }
                    if (dtacc.Rows[i]["ADTYPE"].ToString().Contains("CGST"))
                    {
                        cgstledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                    }
                    if (dtacc.Rows[i]["ADTYPE"].ToString().Contains("SGST"))
                    {
                        sgstledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                    }
                    if (dtacc.Rows[i]["ADTYPE"].ToString().Contains("IGST"))
                    {
                        igstledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                    }
                }
            }
            if (dt.Rows.Count > 0)
            {
               // grn.Roundminus = Convert.ToDouble(dt.Rows[0]["ROUND_OFF_MINUS"].ToString() == "" ? "0" : dt.Rows[0]["ROUND_OFF_MINUS"].ToString());
                //grn.Round = Convert.ToDouble(dt.Rows[0]["ROUND_OFF_PLUS"].ToString() == "" ? "0" : dt.Rows[0]["ROUND_OFF_PLUS"].ToString());
               // grn.otherdeduction = Convert.ToDouble(dt.Rows[0]["OTHER_DEDUCTION"].ToString() == "" ? "0" : dt.Rows[0]["OTHER_DEDUCTION"].ToString());
                grn.Othercharges = Convert.ToDouble(dt.Rows[0]["otherch"].ToString() == "" ? "0" : dt.Rows[0]["otherch"].ToString());
                grn.Packingcharges = Convert.ToDouble(dt.Rows[0]["PKNFDCH"].ToString() == "" ? "0" : dt.Rows[0]["PKNFDCH"].ToString());
                grn.Frieghtcharge = Convert.ToDouble(dt.Rows[0]["FRIEGHT"].ToString() == "" ? "0" : dt.Rows[0]["FRIEGHT"].ToString());

                grn.Gross = Convert.ToDouble(dt.Rows[0]["GROSS"].ToString() == "" ? "0" : dt.Rows[0]["GROSS"].ToString());
                grn.Net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());

                grn.DiscAmt = Convert.ToDouble(dt.Rows[0]["DISC"].ToString() == "" ? "0" : dt.Rows[0]["DISC"].ToString());
                grn.CGST = Convert.ToDouble(dt.Rows[0]["CGST"].ToString() == "" ? "0" : dt.Rows[0]["CGST"].ToString());
                grn.SGST = Convert.ToDouble(dt.Rows[0]["SGST"].ToString() == "" ? "0" : dt.Rows[0]["SGST"].ToString());
                grn.IGST = Convert.ToDouble(dt.Rows[0]["IGST"].ToString() == "" ? "0" : dt.Rows[0]["IGST"].ToString());
                //grn.TotalAmt= Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());
                DataTable dtParty = datatrans.GetData("select P.ACCOUNTNAME from DPBASIC G,PARTYMAST P where G.PARTYID=P.PARTYMASTID AND DPBASICID='" + id + "'");
                string mid = dtParty.Rows[0]["ACCOUNTNAME"].ToString();
                grn.mid = mid;
                if (grn.Net > 0)
                {
                    tda = new GRNAccount();
                    // tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.Ledgername = mid;
                    tda.CRAmount = grn.Net;
                    tda.DRAmount = 0;
                    tda.TypeName = "NET";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Cr";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    tda.symbol = "+";
                    TData.Add(tda);
                }
                if (grn.Gross > 0)
                {
                    for (int i = 0; i < dtdet.Rows.Count; i++)
                    {
                        tda = new GRNAccount();
                        tda.Ledgerlist = BindLedgerLst();
                        tda.Ledgername = dtdet.Rows[i]["ITEMACC"].ToString();
                        tda.CRAmount = 0;
                        tda.DRAmount = Convert.ToDouble(dtdet.Rows[i]["GROSS"].ToString() == "" ? "0" : dtdet.Rows[i]["GROSS"].ToString());
                        tda.TypeName = "GROSS";
                        tda.Isvalid = "Y";
                        tda.CRDR = "Dr";
                        tda.symbol = "-";
                        totalcredit += tda.CRAmount;
                        totaldebit += tda.DRAmount;
                        TData.Add(tda);
                    }

                }
                if (grn.CGST > 0)
                {
                    tda = new GRNAccount();
                    //tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.Ledgername = cgstledger;
                    tda.CRAmount = 0;
                    tda.DRAmount = grn.CGST;
                    tda.TypeName = "CGST";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Dr";
                    tda.symbol = "-";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    TData.Add(tda);
                }
                if (grn.SGST > 0)
                {
                    tda = new GRNAccount();
                    // tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.Ledgername = sgstledger;
                    tda.CRAmount = 0;
                    tda.DRAmount = grn.SGST;
                    tda.TypeName = "SGST";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Dr";
                    tda.symbol = "-";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    TData.Add(tda);
                }
                if (grn.IGST > 0)
                {
                    tda = new GRNAccount();
                    // tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.Ledgername = cgstledger;
                    tda.CRAmount = 0;
                    tda.DRAmount = grn.IGST;
                    tda.TypeName = "IGST";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Dr";
                    tda.symbol = "-";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    TData.Add(tda);
                }
                if (grn.DiscAmt > 0)
                {
                    tda = new GRNAccount();
                    // tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.CRAmount = grn.DiscAmt;
                    tda.DRAmount = 0;
                    tda.TypeName = "Discount";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Cr";
                    tda.symbol = "+";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    TData.Add(tda);
                }
                if (grn.Packingcharges > 0)
                {
                    tda = new GRNAccount();
                    // tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.Ledgername = packingledger;
                    tda.CRAmount = 0;
                    tda.DRAmount = grn.Packingcharges;
                    tda.TypeName = "PACKING CHARGES";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Dr";
                    tda.symbol = "-";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    TData.Add(tda);
                }
                if (grn.Frieghtcharge > 0)
                {
                    tda = new GRNAccount();
                    //  tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.Ledgername = frieghtledger;
                    tda.CRAmount = 0;
                    tda.DRAmount = grn.Frieghtcharge;
                    tda.TypeName = "FREIGHT CHARGES";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Dr";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    tda.symbol = "-";
                    TData.Add(tda);
                }
                if (grn.Othercharges > 0)
                {
                    tda = new GRNAccount();
                    // tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.CRAmount = 0;
                    tda.DRAmount = grn.Othercharges;
                    tda.TypeName = "Other charges";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Dr";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    tda.symbol = "-";
                    TData.Add(tda);
                }
                if (grn.Round > 0)
                {
                    tda = new GRNAccount();
                    // tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    // tda.Ledgername= packingledger
                    tda.CRAmount = 0;
                    tda.DRAmount = grn.Round;
                    tda.TypeName = "ROUND OFF";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Dr";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    tda.symbol = "-";
                    TData.Add(tda);
                }
               
                if (grn.otherdeduction > 0)
                {
                    tda = new GRNAccount();
                    //  tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.CRAmount = grn.otherdeduction;
                    tda.DRAmount = 0;
                    tda.TypeName = "Other Deduction";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Cr";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    tda.symbol = "+";
                    TData.Add(tda);
                }

            }
            grn.TotalCRAmt = totalcredit;
            grn.TotalDRAmt = totaldebit;
            grn.Acclst = TData;
            grn.Accconfiglst = BindAccconfig();
            return View(grn);
        }
        public IActionResult DPACC(string id)
        {
            GRN grn = new GRN();
            DataTable dt = new DataTable();
            string SvSql = "select SUM(CGST) CGST,SUM(SGST) SGST,SUM(IGST) IGST,SUM(TOTAMT) AS NET,SUM(IFREIGHT) as FRIEGHT,SUM(IOTHERCH * QTY) otherch,SUM(AMOUNT) as GROSS,SUM(IPKNFDCH * QTY)PKNFDCH,SUM(DISCAMOUNT) as DISC from DPDETAIL where DPBASICID='" + id + "'";
            dt = datatrans.GetData(SvSql);
            grn.GRNID = id;
            grn.RefDate = DateTime.Now.ToString("dd-MMM-yyyy");
            grn.createdby = Request.Cookies["UserId"];
            DataTable dtnat = datatrans.GetData("select I.ITEMID,BL.QTY,U.UNITID from DPDETAIL BL,ITEMMASTER I,UNITMAST U where I.ITEMMASTERID=BL.ITEMID AND U.UNITMASTID=I.PRIUNIT AND DPBASICID='" + id + "'");
            if(dtnat.Rows.Count > 0)
            {
                grn.Vmemo = "BEING " + dtnat.Rows[0]["ITEMID"].ToString() + "-" + dtnat.Rows[0]["QTY"].ToString() + dtnat.Rows[0]["UNITID"].ToString() + "PURCHASED.";
            }

            List<GRNAccount> TData = new List<GRNAccount>();
            GRNAccount tda = new GRNAccount();
            double totalcredit = 0;
            double totaldebit = 0;
            DataTable dtdet = datatrans.GetData("select ITEMACC,SUM(AMOUNT) as GROSS from DPDETAIL WHERE DPBASICID='" + id + "' GROUP BY ITEMACC");
            DataTable dtacc = new DataTable();
            dtacc = datatrans.GetGRNconfig();
            string frieghtledger = "";
            string discledger = "";
            string roundoffledger = "";
            string cgstledger = "";
            string sgstledger = "";
            string igstledger = "";
            string packingledger = "";
            if (dtacc.Rows.Count > 0)
            {
                grn.ADCOMPHID = dtacc.Rows[0]["ADCOMPHID"].ToString();
                for (int i = 0; i < dtacc.Rows.Count; i++)
                {
                    if (dtacc.Rows[i]["ADTYPE"].ToString() == "FREIGHT CHARGES")
                    {
                        frieghtledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                    }
                    if (dtacc.Rows[i]["ADTYPE"].ToString().Contains("PACKING CHARGES"))
                    {
                        packingledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                    }
                    if (dtacc.Rows[i]["ADTYPE"].ToString() == "DISCOUNT")
                    {
                        discledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                    }
                    if (dtacc.Rows[i]["ADTYPE"].ToString() == "ROUND OFF")
                    {
                        roundoffledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                    }
                    if (dtacc.Rows[i]["ADTYPE"].ToString().Contains("CGST"))
                    {
                        cgstledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                    }
                    if (dtacc.Rows[i]["ADTYPE"].ToString().Contains("SGST"))
                    {
                        sgstledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                    }
                    if (dtacc.Rows[i]["ADTYPE"].ToString().Contains("IGST"))
                    {
                        igstledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                    }
                }
            }
            if (dt.Rows.Count > 0)
            {
                // grn.Roundminus = Convert.ToDouble(dt.Rows[0]["ROUND_OFF_MINUS"].ToString() == "" ? "0" : dt.Rows[0]["ROUND_OFF_MINUS"].ToString());
                //grn.Round = Convert.ToDouble(dt.Rows[0]["ROUND_OFF_PLUS"].ToString() == "" ? "0" : dt.Rows[0]["ROUND_OFF_PLUS"].ToString());
                // grn.otherdeduction = Convert.ToDouble(dt.Rows[0]["OTHER_DEDUCTION"].ToString() == "" ? "0" : dt.Rows[0]["OTHER_DEDUCTION"].ToString());
                grn.Othercharges = Convert.ToDouble(dt.Rows[0]["otherch"].ToString() == "" ? "0" : dt.Rows[0]["otherch"].ToString());
                grn.Packingcharges = Convert.ToDouble(dt.Rows[0]["PKNFDCH"].ToString() == "" ? "0" : dt.Rows[0]["PKNFDCH"].ToString());
                grn.Frieghtcharge = Convert.ToDouble(dt.Rows[0]["FRIEGHT"].ToString() == "" ? "0" : dt.Rows[0]["FRIEGHT"].ToString());

                grn.Gross = Convert.ToDouble(dt.Rows[0]["GROSS"].ToString() == "" ? "0" : dt.Rows[0]["GROSS"].ToString());
                grn.Net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());

                grn.DiscAmt = Convert.ToDouble(dt.Rows[0]["DISC"].ToString() == "" ? "0" : dt.Rows[0]["DISC"].ToString());
                grn.CGST = Convert.ToDouble(dt.Rows[0]["CGST"].ToString() == "" ? "0" : dt.Rows[0]["CGST"].ToString());
                grn.SGST = Convert.ToDouble(dt.Rows[0]["SGST"].ToString() == "" ? "0" : dt.Rows[0]["SGST"].ToString());
                grn.IGST = Convert.ToDouble(dt.Rows[0]["IGST"].ToString() == "" ? "0" : dt.Rows[0]["IGST"].ToString());
                //grn.TotalAmt= Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());
                DataTable dtParty = datatrans.GetData("select P.ACCOUNTNAME from DPBASIC G,PARTYMAST P where G.PARTYID=P.PARTYMASTID AND DPBASICID='" + id + "'");
                string mid = dtParty.Rows[0]["ACCOUNTNAME"].ToString();
                grn.mid = mid;
                if (grn.Net > 0)
                {
                    tda = new GRNAccount();
                    // tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.Ledgername = mid;
                    tda.CRAmount = grn.Net;
                    tda.DRAmount = 0;
                    tda.TypeName = "NET";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Cr";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    tda.symbol = "+";
                    TData.Add(tda);
                }
                if (grn.Gross > 0)
                {
                    for (int i = 0; i < dtdet.Rows.Count; i++)
                    {
                        tda = new GRNAccount();
                        tda.Ledgerlist = BindLedgerLst();
                        tda.Ledgername = dtdet.Rows[i]["ITEMACC"].ToString();
                        tda.CRAmount = 0;
                        tda.DRAmount = Convert.ToDouble(dtdet.Rows[i]["GROSS"].ToString() == "" ? "0" : dtdet.Rows[i]["GROSS"].ToString());
                        tda.TypeName = "GROSS";
                        tda.Isvalid = "Y";
                        tda.CRDR = "Dr";
                        tda.symbol = "-";
                        totalcredit += tda.CRAmount;
                        totaldebit += tda.DRAmount;
                        TData.Add(tda);
                    }

                }
                if (grn.CGST > 0)
                {
                    tda = new GRNAccount();
                    //tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.Ledgername = cgstledger;
                    tda.CRAmount = 0;
                    tda.DRAmount = grn.CGST;
                    tda.TypeName = "CGST";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Dr";
                    tda.symbol = "-";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    TData.Add(tda);
                }
                if (grn.SGST > 0)
                {
                    tda = new GRNAccount();
                    // tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.Ledgername = sgstledger;
                    tda.CRAmount = 0;
                    tda.DRAmount = grn.SGST;
                    tda.TypeName = "SGST";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Dr";
                    tda.symbol = "-";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    TData.Add(tda);
                }
                if (grn.IGST > 0)
                {
                    tda = new GRNAccount();
                    // tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.Ledgername = cgstledger;
                    tda.CRAmount = 0;
                    tda.DRAmount = grn.IGST;
                    tda.TypeName = "IGST";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Dr";
                    tda.symbol = "-";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    TData.Add(tda);
                }
                if (grn.DiscAmt > 0)
                {
                    tda = new GRNAccount();
                    // tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.CRAmount = grn.DiscAmt;
                    tda.DRAmount = 0;
                    tda.TypeName = "Discount";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Cr";
                    tda.symbol = "+";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    TData.Add(tda);
                }
                if (grn.Packingcharges > 0)
                {
                    tda = new GRNAccount();
                    // tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.Ledgername = packingledger;
                    tda.CRAmount = 0;
                    tda.DRAmount = grn.Packingcharges;
                    tda.TypeName = "PACKING CHARGES";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Dr";
                    tda.symbol = "-";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    TData.Add(tda);
                }
                if (grn.Frieghtcharge > 0)
                {
                    tda = new GRNAccount();
                    //  tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.Ledgername = frieghtledger;
                    tda.CRAmount = 0;
                    tda.DRAmount = grn.Frieghtcharge;
                    tda.TypeName = "FREIGHT CHARGES";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Dr";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    tda.symbol = "-";
                    TData.Add(tda);
                }
                if (grn.Othercharges > 0)
                {
                    tda = new GRNAccount();
                    // tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.CRAmount = 0;
                    tda.DRAmount = grn.Othercharges;
                    tda.TypeName = "Other charges";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Dr";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    tda.symbol = "-";
                    TData.Add(tda);
                }
                if (grn.Round > 0)
                {
                    tda = new GRNAccount();
                    // tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    // tda.Ledgername= packingledger
                    tda.CRAmount = 0;
                    tda.DRAmount = grn.Round;
                    tda.TypeName = "ROUND OFF";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Dr";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    tda.symbol = "-";
                    TData.Add(tda);
                }

                if (grn.otherdeduction > 0)
                {
                    tda = new GRNAccount();
                    //  tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.CRAmount = grn.otherdeduction;
                    tda.DRAmount = 0;
                    tda.TypeName = "Other Deduction";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Cr";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    tda.symbol = "+";
                    TData.Add(tda);
                }

            }
            grn.TotalCRAmt = Math.Round(totalcredit, 2);
            grn.TotalDRAmt = Math.Round(totaldebit,2);
            grn.Acclst = TData;
            grn.Accconfiglst = BindAccconfig();
            return View(grn);
        }
        [HttpPost]
        public ActionResult DPACC(GRN Cy, string id)
        {

            try
            {
                string Strout = directPurchase.DPACC(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.GRNID == null)
                    {
                        TempData["notice"] = "GRN Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "GRN Updated Successfully...!";
                    }
                    return RedirectToAction("ListDirectPurchase");
                }

                else
                {
                    ViewBag.PageTitle = "Edit GRN";
                    TempData["notice"] = Strout;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Cy);
        }
        public List<SelectListItem> BindLedgerLst()
        {
            try
            {
                DataTable dtDesg = new DataTable();
                dtDesg = datatrans.GetData("select MASTERID,MNAME from master");
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["MNAME"].ToString(), Value = dtDesg.Rows[i]["MASTERID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindAccconfig()
        {
            try
            {
                DataTable dtDesg = new DataTable();
                string SvSql = "select ADSCHEME,ADCOMPHID from ADCOMPH where ADTRANSID='po' AND IS_ACTIVE='Y'";
                dtDesg = datatrans.GetData(SvSql);
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
        public IActionResult MoveToGRN(string id)
        {
            Models.DirectPurchase ca = new Models.DirectPurchase();
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            dt = directPurchase.GetDirectPurchaseGrn(id);
            if (dt.Rows.Count > 0)
            {
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                ca.Supplier = dt.Rows[0]["PARTYNAME"].ToString();
                ca.DocNo = dt.Rows[0]["DOCID"].ToString();
                ca.ID = id;
                ca.Currency = dt.Rows[0]["MAINCURR"].ToString();
                ca.RefDate = dt.Rows[0]["REFDT"].ToString();
                ca.Voucher = dt.Rows[0]["VOUCHER"].ToString();
                ca.Location = dt.Rows[0]["LOCID"].ToString();
                ca.Narration = dt.Rows[0]["NARR"].ToString();
                ca.LRCha = Convert.ToDouble(dt.Rows[0]["LRCH"].ToString() == "" ? "0" : dt.Rows[0]["LRCH"].ToString());
                ca.DelCh = Convert.ToDouble(dt.Rows[0]["DELCH"].ToString() == "" ? "0" : dt.Rows[0]["DELCH"].ToString());
                ca.Other = Convert.ToDouble(dt.Rows[0]["OTHERCH"].ToString() == "" ? "0" : dt.Rows[0]["OTHERCH"].ToString());
                ca.Frieghtcharge = Convert.ToDouble(dt.Rows[0]["FREIGHT"].ToString() == "" ? "0" : dt.Rows[0]["FREIGHT"].ToString());
                ca.Disc = Convert.ToDouble(dt.Rows[0]["OTHERDISC"].ToString() == "" ? "0" : dt.Rows[0]["OTHERDISC"].ToString());
                ca.Round = Convert.ToDouble(dt.Rows[0]["ROUNDM"].ToString() == "" ? "0" : dt.Rows[0]["ROUNDM"].ToString());
                ca.Gross = Convert.ToDouble(dt.Rows[0]["GROSS"].ToString() == "" ? "0" : dt.Rows[0]["GROSS"].ToString());
                ca.Net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());

            }
            List<DirItem> TData = new List<DirItem>();
            DirItem tda = new DirItem();
            double total = 0;
            dt2 = directPurchase.GetDirectPurchaseItemGRN(id);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new DirItem();
                    tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                    tda.Quantity = Convert.ToDouble(dt2.Rows[i]["QTY"].ToString() == "" ? "0" : dt2.Rows[i]["QTY"].ToString());
                    tda.rate = Convert.ToDouble(dt2.Rows[i]["RATE"].ToString() == "" ? "0" : dt2.Rows[i]["RATE"].ToString());
                    tda.ConFac = dt2.Rows[0]["CF"].ToString();
                    tda.Amount = tda.Quantity * tda.rate;
                    total += tda.Amount;

                    //toaamt = tda.rate * tda.Quantity;
                    //total += toaamt;
                   
                    //tda.Amount = toaamt;
                    tda.Unit = dt2.Rows[i]["UNITID"].ToString();
                   
                    tda.Disc = Convert.ToDouble(dt2.Rows[i]["DISC"].ToString() == "" ? "0" : dt2.Rows[i]["DISC"].ToString());
                    tda.DiscAmount = Convert.ToDouble(dt2.Rows[i]["DISCAMOUNT"].ToString() == "" ? "0" : dt2.Rows[i]["DISCAMOUNT"].ToString());

                    tda.FrigCharge = Convert.ToDouble(dt2.Rows[i]["IFREIGHTCH"].ToString() == "" ? "0" : dt2.Rows[i]["IFREIGHTCH"].ToString());
                    tda.TotalAmount = Convert.ToDouble(dt2.Rows[i]["TOTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["TOTAMT"].ToString());
                    tda.CGSTP = Convert.ToDouble(dt2.Rows[i]["CGSTP"].ToString() == "" ? "0" : dt2.Rows[i]["CGSTP"].ToString());
                    tda.SGSTP = Convert.ToDouble(dt2.Rows[i]["SGSTP"].ToString() == "" ? "0" : dt2.Rows[i]["SGSTP"].ToString());
                    tda.IGSTP = Convert.ToDouble(dt2.Rows[i]["IGSTP"].ToString() == "" ? "0" : dt2.Rows[i]["IGSTP"].ToString());
                    tda.CGST = Convert.ToDouble(dt2.Rows[i]["CGST"].ToString() == "" ? "0" : dt2.Rows[i]["CGST"].ToString());
                    tda.SGST = Convert.ToDouble(dt2.Rows[i]["SGST"].ToString() == "" ? "0" : dt2.Rows[i]["SGST"].ToString());
                    tda.IGST = Convert.ToDouble(dt2.Rows[i]["IGST"].ToString() == "" ? "0" : dt2.Rows[i]["IGST"].ToString());
                    // tda.PurType = dt2.Rows[i]["PURTYPE"].ToString();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
                ca.Net = Math.Round(total, 2);
            }
            //ca.Net = tot;
            ca.DirLst = TData;
            return View(ca);
        }
        [HttpPost]
        public ActionResult MoveToGRN(Models.DirectPurchase Cy, string id)
        {
            try
            {
                Cy.ID = id;
                string Strout = directPurchase.DirectPurchasetoGRN(Cy.ID);
                return RedirectToAction("ListDirectPurchase");
              

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("ListDirectPurchase");
        }
        public ActionResult DeleteItem(string tag, string id)
        {

            string flag = directPurchase.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListDirectPurchase");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListDirectPurchase");
            }
        }
        //public ActionResult DeleteItem(string tag, string id)
        //{
        //    //EnquiryList eqgl = new EnquiryList();
        //    //eqgl.StatusChange(tag, id);
        //    return RedirectToAction("ListDirectPurchase");

        //}
        public ActionResult GetDocidDetail(string locid)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string doc = "";
                if(locid== "10001000000827") { locid = "12423000000238"; }
                dt = datatrans.GetSequences("dp", locid);
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
        public IActionResult ListDirectPurchase()
        {
            //IEnumerable<DirectPurchase> cmp = directPurchase.GetAllDirectPur(status);
            return View();
        }
        public IActionResult ListDirectPurchaseGRN()
        {
            //IEnumerable<DirectPurchase> cmp = directPurchase.GetAllDirectPur(status);
            return View();
        }
        public ActionResult MyListDirectPurchaseGRN(string strStatus)
        {
            List<DirectPurchaseItems> Reg = new List<DirectPurchaseItems>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)directPurchase.GetAllDirectPurchases(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string MailRow = string.Empty;
                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string Account = string.Empty;
                string Print = string.Empty;
                //string Status = string.Empty;

                MailRow = "<a href=DirectPurchase?tag=Del&id=" + dtUsers.Rows[i]["DPBASICID"].ToString() + "><img src='../Images/mail_icon.png' alt='Send Email' /></a>";
                EditRow = "<a href=DirectPurchase?id=" + dtUsers.Rows[i]["DPBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                Print = "<a href=Print?id=" + dtUsers.Rows[i]["DPBASICID"].ToString() + " target='_blank'><img src='../Images/pdficon.png' alt='View Details' width='20' /></a>";
                DeleteRow = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["DPBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
               // Account = "<a href=GRNAccount?id=" + dtUsers.Rows[i]["GRNBLBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/profit.png' alt='View Details' width='20' /></a>";
                Reg.Add(new DirectPurchaseItems
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["DPBASICID"].ToString()),
                    branch = dtUsers.Rows[i]["BRANCHID"].ToString(),
                    supplier = dtUsers.Rows[i]["PARTYNAME"].ToString(),
                    docNo = dtUsers.Rows[i]["DOCID"].ToString(),
                    docDate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    mailrow = MailRow,
                    editrow = EditRow,
                    delrow = DeleteRow,
                    Accrow = Account,
                    print = Print,
                    //Status = Status,


                });
            }

            return Json(new
            {
                Reg
            });

        }
        
             public List<SelectListItem> BindEmpty()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
               
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
        public List<SelectListItem> BindVocher()
        {
            try
            {
                DataTable dtDesg = directPurchase.GetVocher();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DESCRIPTION"].ToString(), Value = dtDesg.Rows[i]["DESCRIPTION"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindSupplier(string supid)
        {
            try
            {
                DataTable dtDesg = datatrans.GetParty(supid);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
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
        public List<SelectListItem> BindIndent(string puid, string supid)
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                DataTable dtDesg = new DataTable();
                if (puid == "AGAINST PURCHASE INDENT") 
                {
                    dtDesg = datatrans.getindent();
                    for (int i = 0; i < dtDesg.Rows.Count; i++)
                    {
                        lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["DOCID"].ToString() });
                    }

                }
                //if (puid == "AGAINST EXCISE INVOICE")
                //{
                //    dtDesg = datatrans.getexinv(supid);
                //    for (int i = 0; i < dtDesg.Rows.Count; i++)
                //    {
                //        lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["EXINVBASICID"].ToString() });
                //    }

                //}
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindPuType()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "AGAINST PURCHASE INDENT", Value = "AGAINST PURCHASE INDENT" });
               // lstdesg.Add(new SelectListItem() { Text = "AGAINST EXCISE INVOICE", Value = "AGAINST EXCISE INVOICE" });
                lstdesg.Add(new SelectListItem() { Text = "AGAINST CONSUMABLES RETURN", Value = "AGAINST CONSUMABLES RETURN" });
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
                DataTable dtDesg = datatrans.GetFGLOC();

               
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
        public List<SelectListItem> Bindgstlst(string id)
        {
            try
            {

                DataTable dtDesg = directPurchase.GetTariff(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["TARIFFID"].ToString(), Value = dtDesg.Rows[i]["tariff"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public List<SelectListItem> BindPurType()
        //{
        //    try
        //    {

        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        lstdesg.Add(new SelectListItem() { Text = "CONSUMABLES PURCHASE", Value = "CONSUMABLES PURCHASE" });
        //        lstdesg.Add(new SelectListItem() { Text = "FIXED PURCHASE", Value = "FIXED PURCHASE" });
        //        lstdesg.Add(new SelectListItem() { Text = "MACHINERIES PURCHASE", Value = "MACHINERIES PURCHASE" });
        //        lstdesg.Add(new SelectListItem() { Text = "RAW MATERIAL", Value = "RAW MATERIAL" });
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public List<SelectListItem> BindItemlst(string value,string puid)
        {
            try
            {
                DataTable dtDesg = new DataTable();
                dtDesg = datatrans.GetindItem(value, puid);
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
        public List<SelectListItem> BindCItemlst()
        {
            try
            {
                DataTable dtDesg = new DataTable();
                dtDesg = datatrans.GetData("select ITEMID,ITEMMASTERID from ITEMMASTER WHERE SUBCATEGORY='PACK DRUM'");
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

        public ActionResult GetItemDetail(string ItemId,string indent)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                
                string unit = "";
                string CF = "";
                string price = "";
                string indqty = "";
                string inddetid = "";
                dt = datatrans.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                 
                    unit = dt.Rows[0]["UNITID"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();
                    dt1 = directPurchase.GetItemCF(ItemId, dt.Rows[0]["UNITMASTID"].ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        CF = dt1.Rows[0]["CF"].ToString();
                    }
                }
                string ind = datatrans.GetDataString("SELECT PINDBASICID FROM PINDBASIC WHERE DOCID='" + indent + "'");
                indqty = datatrans.GetDataString("SELECT QTY FROM PINDDETAIL WHERE PINDBASICID='"+ ind + "' AND ITEMID='"+ItemId+"'");
                inddetid = datatrans.GetDataString("SELECT PINDDETAILID FROM PINDDETAIL WHERE PINDBASICID='" + ind + "' AND ITEMID='"+ItemId+"'");
                var result = new { unit = unit, CF = CF, price = price, indqty= indqty , inddetid = inddetid };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetItemJSON(string indid, string puid)
        {
             return Json(BindItemlst(indid, puid));
        }
        public JsonResult GetCItemJSON()
        {
            return Json(BindCItemlst());
        }
        public JsonResult GetSupJSON(string puid)
        {
            return Json(BindSupplier(puid));
        }
        public JsonResult GetIndentJSON(string puid,string supid)
        {
            return Json(BindIndent(puid, supid));
        }
        
        public JsonResult GetItemGrpJSON()
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItemGrplst());
        }

        public IActionResult Index()
        {
            return View();
        }
        public ActionResult GetGSTDetail(string ItemId, string custid)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string per = "0";

                string hsn = "";
                string hsnid = "";
                string gst = "";
                
               
                //if (ItemId == "1")
                //{
                //    hsn = "996519";
                //}
                //else
                //{
                    dt = directPurchase.GetHsn(ItemId);
                    if (dt.Rows.Count > 0)
                    {
                        hsn = dt.Rows[0]["HSN"].ToString();
                    }
                dt1 = directPurchase.GethsnDetails(hsn);
                if (dt1.Rows.Count > 0)
                {

                    hsnid = dt1.Rows[0]["HSNCODEID"].ToString();


                }
                //}
                //if (ItemId == "1")
                //{
                //    sgst = "18";
                //}
                //else
                //{
                DataTable trff = new DataTable();
                trff = directPurchase.GetgstDetails(hsnid);
               
                if (trff.Rows.Count ==1)
                {
                    
                        gst = trff.Rows[0]["TARIFFID"].ToString();

                        DataTable percen = datatrans.GetData("Select PERCENTAGE from TARIFFMASTER where TARIFFMASTERID='" + gst + "'  ");
                        per = percen.Rows[0]["PERCENTAGE"].ToString();
                    

                }
                //}

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

                var result = new { per = per, type = type};
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetGSTItemJSON(string itemid, string custid)
        {
            return Json(BindTariff(itemid, custid));
        }
        public List<SelectListItem> BindTariff(string itemid, string custid)
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                DataTable dth = new DataTable();
                dth = datatrans.GetData("select GSTP from HSNMAST where HSCODE=(SELECT HSN FROM ITEMMASTER WHERE ITEMMASTERID='" + itemid + "')");
                string type = "";
                string cmpstate = datatrans.GetDataString("select STATE from CONTROL");
                string partystate = datatrans.GetDataString("select STATE from PARTYMAST where PARTYMASTID='" + custid + "'");
                if (partystate == cmpstate)
                {
                    type = "GST";
                }
                else
                {
                    type = "IGST";
                }
                DataTable dt = new DataTable();

                if (dth.Rows.Count > 0)
                {
                    for (int i = 0; i < dth.Rows.Count; i++)
                    {
                        string gper = dth.Rows[i]["GSTP"].ToString();
                        if (type == "GST")
                        {
                            dt = datatrans.GetData("select ETARIFFMASTERID,TARIFFID from ETARIFFMASTER where ((SGST)+(CGST))='" + gper + "' AND IS_ACTIVE='Y'");
                            if(dt.Rows.Count > 0)
                            {
                                lstdesg.Add(new SelectListItem() { Text = dt.Rows[0]["TARIFFID"].ToString(), Value = dt.Rows[0]["ETARIFFMASTERID"].ToString() });
                            }
                            
                        }
                        else
                        {
                            dt = datatrans.GetData("select ETARIFFMASTERID,TARIFFID from ETARIFFMASTER where IGST='" + gper + "' AND IS_ACTIVE='Y'");
                            if (dt.Rows.Count > 0)
                            {
                                lstdesg.Add(new SelectListItem() { Text = dt.Rows[0]["TARIFFID"].ToString(), Value = dt.Rows[0]["ETARIFFMASTERID"].ToString() });
                            }
                        }
                    }
                }
             
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetGSTPerDetail(string tarriffid)
        {
            try
            {
                string cgst = "";
                string sgst = "";
                string igst = "";
                DataTable dt = datatrans.GetData("select SGST,CGST,IGST from ETARIFFMASTER where ETARIFFMASTERID='"+ tarriffid + "'");
               if(dt.Rows.Count > 0)
                {
                    cgst = dt.Rows[0]["CGST"].ToString();
                    sgst = dt.Rows[0]["SGST"].ToString();
                    igst = dt.Rows[0]["IGST"].ToString();
                }


                var result = new { cgst = cgst, sgst = sgst, igst = igst };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetNarrDetail(string supid)
        {
            try
            {
                string type = "";

                string partystate = datatrans.GetDataString("select PARTYNAME from PARTYMAST where PARTYMASTID='" + supid + "'");

                
                    type = "Purchased From " +partystate;
                



                var result = new { type = type };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> Print(string id)
        {

            string mimtype = "";
            int extension = 1;
 
            System.Data.DataSet ds = new System.Data.DataSet();
            var path = $"{this._WebHostEnvironment.WebRootPath}\\Reports\\DirectPurchaseReport.rdlc";
            Dictionary<string, string> Parameters = new Dictionary<string, string>();
            //  Parameters.Add("rp1", " Hi Everyone");
            var dpitem = await directPurchase.GetdpItem(id);
            var dpdetitem = await directPurchase.GetdpdetItem(id);

            AspNetCore.Reporting.LocalReport localReport = new AspNetCore.Reporting.LocalReport(path);
            localReport.AddDataSource("DpBasic", dpitem);
            localReport.AddDataSource("DpDetail", dpdetitem);
            //localReport.AddDataSource("DataSet1_DataTable1", po);
            var result = localReport.Execute(RenderType.Pdf, extension, Parameters, mimtype);

            return File(result.MainStream, "application/Pdf");
          
        }
    }
}