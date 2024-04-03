using Microsoft.AspNetCore.Mvc;
using Arasan.Models;
using Arasan.Interface.Sales;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Collections.Generic;
using System.Xml.Linq;
using Arasan.Services.Sales;
using Arasan.Interface;
using Arasan.Services.Master;
using Arasan.Services;
using AspNetCore.Reporting;

namespace Arasan.Controllers.Sales
{
    public class ProFormaInvoiceController : Controller
    {
        IProFormaInvoiceService ProFormaInvoiceService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public ProFormaInvoiceController(IProFormaInvoiceService _ProFormaInvoiceService, IConfiguration _configuratio, IWebHostEnvironment WebHostEnvironment)
        {
            ProFormaInvoiceService = _ProFormaInvoiceService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
            this._WebHostEnvironment = WebHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }
        public IActionResult ProFormaInvoice(string id ,int tag)
        {
            ProFormaInvoice ca = new ProFormaInvoice();
            ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Curlst = BindCurrency();
            ca.Suplst = BindSupplier();
            ca.Joblst = BindJob();
            ca.Loclst = GetLoc();
            ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("PInv");
            if (dtv.Rows.Count > 0)
            {
                ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
            List<ProformaInvoiceItem> TData = new List<ProformaInvoiceItem>();
            ProformaInvoiceItem tda = new ProformaInvoiceItem();
            List<PTermsItem> TData1 = new List<PTermsItem>();
            PTermsItem tda1 = new PTermsItem();
            List<PAreaItem> TData2 = new List<PAreaItem>();
            PAreaItem tda2 = new PAreaItem();
            if (id == null)
            {
                ca.Currency = "1";
                ca.ExRate = "1";
                for (int i = 0; i < 1; i++)
                {
                    tda = new ProformaInvoiceItem();
                    tda.Itemlst = BindEmpty();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else if(id != null && tag != 0)
            {


                DataTable dt1 = new DataTable();
                //double total = 0;
                dt1 = ProFormaInvoiceService.GetDrumParty(id);
                if (dt1.Rows.Count > 0)
                {
                     
                        ca.Party = dt1.Rows[0]["CUSTOMERID"].ToString();
                        
                    }
                 DataTable dtt = new DataTable();

                dtt = ProFormaInvoiceService.GetDrumAll(id);
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda = new ProformaInvoiceItem();
                        tda.Itemlst = BindEmpty();
                        //tda.itemid = dtt.Rows[i]["ITEMID"].ToString();
                        //tda.item = dtt.Rows[i]["item"].ToString();
                        //DataTable dtt1 = new DataTable();
                        //dtt1 = datatrans.GetItemDetails(tda.item);
                        //if (dtt1.Rows.Count > 0)
                        //{
                  
                        //    tda.unit = dtt1.Rows[0]["UNITID"].ToString();
                        //    tda.itemdes = dtt1.Rows[i]["ITEMDESC"].ToString();
                        //}
                        //    tda.qty = dtt.Rows[i]["totqty"].ToString();
                        //tda.rate = dtt.Rows[i]["totrate"].ToString();
                       
                        //tda.BaID = id;
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                      
                    }
                }
            }
            else
            {
                DataTable dt = new DataTable();
                //double total = 0;
                dt = ProFormaInvoiceService.GetEditProFormaInvoice(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    //ca.WorkCenter = dt.Rows[0]["WORKORDER"].ToString();
                    ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.RefNo = dt.Rows[0]["REFNO"].ToString();
                    ca.RefDate = dt.Rows[0]["REFDATE"].ToString();
                    ca.Currency = dt.Rows[0]["MAINCURR"].ToString();
                    ca.ExRate = dt.Rows[0]["EXRATE"].ToString();
                    ca.Party = dt.Rows[0]["PARTYID"].ToString();
                    //ca.SalesValue = dt.Rows[0]["SALESVALUE"].ToString();
                    ca.Gross = dt.Rows[0]["GROSS"].ToString();
                    ca.Net = dt.Rows[0]["NET"].ToString();
                    ca.Amount = dt.Rows[0]["AMTWORDS"].ToString();
                    //ca.BankName = dt.Rows[0]["BANKNAME"].ToString();
                    ca.AcNo = dt.Rows[0]["ACNO"].ToString();
                    ca.Address = dt.Rows[0]["SHIPADDRESS"].ToString();
                    ca.Narration = dt.Rows[0]["NARRATION"].ToString();
                    ca.ID = id;


                }
              
            }
            for (int i = 0; i < 1; i++)
            {
                tda1 = new PTermsItem();

                tda1.Termslst = BindTerms();
                tda1.Isvalid = "Y";
                TData1.Add(tda1);
            }
            for (int i = 0; i < 1; i++)
            {
                tda2 = new PAreaItem();

                tda2.Arealst = BindArea("");
                tda2.Isvalid = "Y";
                TData2.Add(tda2);
            }
            ca.TermsItemlst = TData1;
            ca.AreaItemlst = TData2;
            ca.ProFormalst = TData;
            return View(ca);

        }
        [HttpPost]
        public ActionResult ProFormaInvoice(ProFormaInvoice Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = ProFormaInvoiceService.ProFormaInvoiceCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "ProFormaInvoice Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "ProFormaInvoice Updated Successfully...!";
                    }
                    return RedirectToAction("ListProFormaInvoice");
                }

                else
                {
                    ViewBag.PageTitle = "Edit ProFormaInvoice";
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
        public ActionResult GetProFormaInvoiceDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string currency = "";
                string party = "";
               
                if (ItemId != "edit")
                {
                    dt = ProFormaInvoiceService.GetProFormaInvoiceDetails(ItemId);

                    if (dt.Rows.Count > 0)
                    {

                         
                        party = dt.Rows[0]["PARTYNAME"].ToString();


                    }
                    
                }

                var result = new {  party = party };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetWorkOrderDetails(string id,string jobid)
        {
            ProFormaInvoice model = new ProFormaInvoice();
            DataTable dtt = new DataTable();
            List<ProFormaInvoiceDetail> Data = new List<ProFormaInvoiceDetail>();
            ProFormaInvoiceDetail tda = new ProFormaInvoiceDetail();
            if (id == "edit")
            {
                DataTable dtt1 = new DataTable();
                dtt1 = ProFormaInvoiceService.EditProFormaInvoiceDetails(jobid);
                if (dtt1.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt1.Rows.Count; i++)
                    {
                        tda = new ProFormaInvoiceDetail();
                        tda.itemid = dtt1.Rows[i]["ITEMID"].ToString();
                        tda.itemdes = dtt1.Rows[i]["ITEMSPEC"].ToString();
                        tda.unit = dtt1.Rows[i]["UNIT"].ToString();
                        tda.qty = dtt1.Rows[i]["QTY"].ToString();

                        tda.rate = dtt1.Rows[i]["RATE"].ToString();
                        tda.amount = Convert.ToDouble(dtt1.Rows[i]["AMOUNT"].ToString());
                        tda.discount = dtt1.Rows[i]["DISCOUNT"].ToString();
                        tda.itrodis = dtt1.Rows[i]["IDISC"].ToString();
                        tda.tradedis = dtt1.Rows[i]["TDISC"].ToString();
                        tda.cashdisc = dtt1.Rows[i]["CDISC"].ToString();
                        tda.tradedis = dtt1.Rows[i]["TDISC"].ToString();

                        tda.additionaldis = dtt1.Rows[i]["ADISC"].ToString();
                        tda.dis = dtt1.Rows[i]["SDISC"].ToString();
                        tda.frieght = dtt1.Rows[i]["FREIGHT"].ToString();
                        tda.tariff = dtt1.Rows[i]["TARIFFID"].ToString();
                        tda.cgst = dtt1.Rows[i]["CGST"].ToString();
                        tda.Isvalid = "Y";
                        tda.sgst = dtt1.Rows[i]["SGST"].ToString();
                        tda.igst = dtt1.Rows[i]["IGST"].ToString();
                        tda.totamount = Convert.ToDouble(dtt1.Rows[i]["TOTEXAMT"].ToString());

                        Data.Add(tda);
                    }
                }
            }
            else
            {

                string detid = datatrans.GetDataString("select JODRUMALLOCATIONBASICID FROM JODRUMALLOCATIONBASIC WHERE JOPID='" + id + "'");
               
              
                dtt = ProFormaInvoiceService.GetWorkOrderDetail(detid);
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda = new ProFormaInvoiceDetail();

                        tda.itemid = dtt.Rows[i]["ITEMID"].ToString();
                        tda.itemdes = dtt.Rows[i]["ITEMDESC"].ToString();
                        tda.unit = dtt.Rows[i]["UNITID"].ToString();
                        tda.qty = dtt.Rows[i]["qty"].ToString();
                        tda.rate =(dtt.Rows[i]["RATE"].ToString() == "" ? "0" : dtt.Rows[0]["RATE"].ToString());
                        //double rate = Convert.ToDouble(tda.rate);
                        //double quatity = Convert.ToDouble(tda.qty);
                        //double amt= quatity * rate;
                        //tda.amount = amt ;


                       

                        string hsnid = "";

                        string hsn = "";
                        string sgstp ="";
                        string cgstp = "";
                        string igstp ="";
                        double cgsta = 0;
                        double sgsta = 0;
                        double igsta = 0;
                        double pers = 0;
                        string gst = "";
                        string itemid = datatrans.GetDataString("SELECT ITEMID FROM JODRUMALLOCATIONDETAIL  WHERE JODRUMALLOCATIONBASICID='" + detid + "' GROUP BY ITEMID");
                        hsn = datatrans.GetDataString("select HSN from ITEMMASTER WHERE ITEMMASTERID = '" + itemid + "'");
                        
                        hsnid = datatrans.GetDataString("select HSNCODEID from HSNCODE WHERE HSNCODE='" + hsn + "'");

                        
                        DataTable trff = new DataTable();
                        trff = ProFormaInvoiceService.GetgstDetails(hsnid);
                        // tda.gstlst = bindgst(hsnid);
                        if (trff.Rows.Count > 0)
                        {
                            for (int j = 0; j < trff.Rows.Count; j++)
                            {

                                gst = trff.Rows[j]["TARIFFID"].ToString();

                                DataTable per = datatrans.GetData("Select PERCENTAGE from TARIFFMASTER where TARIFFMASTERID='" + gst + "'  ");
                                pers = Convert.ToDouble(per.Rows[0]["PERCENTAGE"].ToString());
                            }

                        }
                        //}

                        string cmpstate = datatrans.GetDataString("select STATE from CONTROL");

                        string type = "";
                       DataTable dt = ProFormaInvoiceService.GetProFormaInvoiceDetails(detid);
                        string party = "";
                        if (dt.Rows.Count > 0)
                        {


                            party = dt.Rows[0]["PARTYNAME"].ToString();


                        }
                        string partystate = datatrans.GetDataString("select STATE from PARTYMAST where PARTYNAME='" + party + "'");
                        if (trff.Rows.Count == 1)
                        {
                            if (partystate == cmpstate)
                            {
                                double cgst = pers / 2;
                                double sgst = pers / 2;
                                sgstp = sgst.ToString();
                                cgstp = cgst.ToString();
                                if(sgstp == "" && cgstp == "")
                                {
                                    sgstp = "0";
                                    cgstp = "0";
                                }
                                tda.sgstp = sgstp ;
                                tda.cgstp = cgstp ;
                                //double cgstperc = tda.amount / 100 * sgstp;
                                //double sgstperc = tda.amount / 100 * cgstp;
                                //tda.cgst = cgstperc ;
                                //tda.sgst = cgstperc ;
                                //tda.totamount = tda.cgst + tda.sgst + tda.igst;
                               // po.Net = tda.TotalAmount;
                            }
                            else
                            {
                                igstp =pers.ToString();
                                if (igstp == "" ) 
                                {
                                    igstp = "0";
                                    
                                }
                                tda.igstp= igstp ;
                                //tda.igst = tda.amount / 100 * tda.igstp;
                                //tda.totamount = tda.igst + tda.amount;
                                 
                            }
                            if (sgstp == "" && cgstp == "")
                            {
                                sgstp = "0";
                                cgstp = "0";
                            }
                            tda.sgstp = sgstp;
                            tda.cgstp = cgstp;
                        }
                        tda.ID = id;
                        tda.Isvalid = "Y";
                        Data.Add(tda);
                    }
                }
            }
            model.ProFormavlst = Data;
            return Json(model.ProFormavlst);

        }
        public ActionResult GetItemDetail(string ItemId, string locid)
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
                string spec = "";
                dt = datatrans.GetItemDetails(ItemId);
                string stock = ProFormaInvoiceService.GetDrumStock(ItemId, locid);
                if (dt.Rows.Count > 0)
                {

                    unit = dt.Rows[0]["UNITID"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();
                    //binno = dt.Rows[0]["BINNO"].ToString();
                    //binname = datatrans.GetDataString("select BINID from BINBASIC where BINBASICId='" + dt.Rows[0]["BINNO"].ToString() + "'"); ;
                    dt1 = ProFormaInvoiceService.GetItemCF(ItemId, dt.Rows[0]["UNITMASTID"].ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        CF = dt1.Rows[0]["CF"].ToString();
                    }
                    spec= dt.Rows[0]["ITEMDESC"].ToString();
                }

                var result = new { unit = unit, CF = CF, price = price, binno = binno, binname = binname, stock = stock ,spec=spec};
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

                dt = ProFormaInvoiceService.GetTrefficDetails(ItemId);
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
        public IActionResult ListProFormaInvoice(string status)
        {

            //HttpContext.Session.SetString("SalesStatus", "Y");
            //IEnumerable<ProFormaInvoice> cmp = ProFormaInvoiceService.GetAllProFormaInvoice(status);
            return View();
        }
        public ActionResult MyListProFormaInvoiceGrid(string strStatus)
        {
            List<ListProFormaInvoiceItems> Reg = new List<ListProFormaInvoiceItems>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)ProFormaInvoiceService.GetAllListProFormaInvoiceItems(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string EditRow = string.Empty;
                string DeleteRow = string.Empty;
                string pdf = string.Empty;

                //if (dtUsers.Rows[i]["STATUS"].ToString() == "INACTIVE")
                //{
                //    View = "";
                //    DeleteRow = "";
                //}
                //else
                //{
                pdf = "<a href=Print?id=" + dtUsers.Rows[i]["PINVBASICID"].ToString() + " target='_blank'><img src='../Images/pdficon.png' alt='View Details' width='20' /></a>";

                EditRow = "<a href=ProFormaInvoice?id=" + dtUsers.Rows[i]["PINVBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                DeleteRow = "<a href=CloseQuote?id=" + dtUsers.Rows[i]["PINVBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";

                //}

                Reg.Add(new ListProFormaInvoiceItems
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["PINVBASICID"].ToString()),
                    //branch = dtUsers.Rows[i]["BRANCHID"].ToString(),
                    enqno = dtUsers.Rows[i]["DOCID"].ToString(),
                    refno = dtUsers.Rows[i]["REFNO"].ToString(),
                    date = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    party = dtUsers.Rows[i]["PARTYNAME"].ToString(),
                    edit = EditRow,
                    pdf = pdf,
                    delrow = DeleteRow,



                });
            }

            return Json(new
            {
                Reg
            });

        }
        public JsonResult GetItemJSON(string locid)
        {
            return Json(BindItemlst(locid));

        }
        public List<SelectListItem> BindItemlst(string locid)
        {
            try
            {
                DataTable dtDesg = ProFormaInvoiceService.GetFGItem(locid);
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
        public List<SelectListItem> BindEmpty()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
               lstdesg.Add(new SelectListItem() { Text = "", Value = "" });
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
        public List<SelectListItem> BindJob()
        {
            try
            {
                DataTable dtDesg = ProFormaInvoiceService.GetJob();
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
        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dtDesg = ProFormaInvoiceService.GetBranch();
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
        public ActionResult CloseQuote(string tag, string id)
        {

            string flag = ProFormaInvoiceService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {
                return RedirectToAction("ListProFormaInvoice");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListProFormaInvoice");
            }
        }
        public List<SelectListItem> BindTerms()
        {
            try
            {
                DataTable dtDesg = ProFormaInvoiceService.GetTerms();
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
        public List<SelectListItem> BindArea(string custid)
        {
            try
            {
                DataTable dtDesg = ProFormaInvoiceService.GetArea(custid);
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
        public ActionResult DrumSelectionView(string id)
        {
            ViewDrumdetailstable ca = new ViewDrumdetailstable();
            List<DDrumdetailsView> TData = new List<DDrumdetailsView>();
            DDrumdetailsView tda = new DDrumdetailsView();
            string detid = datatrans.GetDataString("select JODRUMALLOCATIONBASICID FROM JODRUMALLOCATIONBASIC WHERE JOPID='" + id + "'");

            DataTable dtEnq = new DataTable();
            dtEnq = ProFormaInvoiceService.GetDrumDetails(detid);
            for (int i = 0; i < dtEnq.Rows.Count; i++)
            {
                tda = new DDrumdetailsView();
                tda.lotno = dtEnq.Rows[i]["LOTNO"].ToString();
                tda.drumno = dtEnq.Rows[i]["DRUMNO"].ToString();
                tda.qty = dtEnq.Rows[i]["QTY"].ToString();
                tda.rate = dtEnq.Rows[i]["RATE"].ToString();
               
                TData.Add(tda);
            }
            ca.Drumlst = TData;
            return View(ca);
        }
        //public ActionResult AssignSession(string status)
        //{
        //    try
        //    {
        //        HttpContext.Session.SetString("SalesStatus", status);
        //        string result = "";
        //        if (!string.IsNullOrEmpty(status))
        //        {
        //            result = status;
        //            //HttpContext.Session.SetString("SalesStatus", status);
        //        }
        //        return Json(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
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
                    dt = datatrans.GetHsn(ItemId);
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
                    dt1 = datatrans.GetGSTDetails(hsn);
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
        public ActionResult GetItemRate(string ItemId, string custid, string ratec)
        {
            try
            {
                DataTable dt = new DataTable();
                string price = datatrans.GetDataString("Select nvl(sum(rate),0) rate , 1 AS SNO from (SELECT D.RATE FROM RATEBASIC B, RATEDETAIL D, ITEMMASTER I WHERE D.RCODE = '" + ratec + "' AND I.ITEMMASTERID = '" + ItemId + "'  AND D.ITEMID = I.ITEMMASTERID AND B.RATEBASICID = D.RATEBASICID ANd B.VALIDFROM = (Select max(Validfrom) from Ratebasic R1 Where R1.RATECODE = '" + ratec + "' ANd R1.VALIDFROM <='" + DateTime.Now.ToString("dd-MMM-yyyy") + "') Union SELECT(-disc) FROM PARTYADVDISC WHERE PARTYMASTID ='" + custid + "' and active = 'Yes' and RATECODE = '" + ratec + "')");


                var result = new { price = price };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetPartyaddrJSON(string custid)
        {
                       return Json(BindArea(custid));
        }
        public ActionResult GetNarrDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string narr = "";
                string narr1 = "";
                dt = datatrans.GetNarr(ItemId);
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
        public async Task<IActionResult> Print(string id)
        {

            string mimtype = "";
            int extension = 1;

            System.Data.DataSet ds = new System.Data.DataSet();
            var path = $"{this._WebHostEnvironment.WebRootPath}\\Reports\\Pro-Forma.rdlc";
            Dictionary<string, string> Parameters = new Dictionary<string, string>();
            //  Parameters.Add("rp1", " Hi Everyone");
            var basic = await ProFormaInvoiceService.GetBasicItem(id);
            var Detail = await ProFormaInvoiceService.GetPinvItemDetail(id);
            var terms = await ProFormaInvoiceService.GetPinvtermsDetail(id);

            AspNetCore.Reporting.LocalReport localReport = new AspNetCore.Reporting.LocalReport(path);
            localReport.AddDataSource("PinvBasic", basic);
            localReport.AddDataSource("PinvDetail", Detail);
            localReport.AddDataSource("PinvTandC", terms);
             
            var result = localReport.Execute(RenderType.Pdf, extension, Parameters, mimtype);

            return File(result.MainStream, "application/Pdf");

        }
    }
}
