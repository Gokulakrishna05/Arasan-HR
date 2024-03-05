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

namespace Arasan.Controllers.Sales
{
    public class ProFormaInvoiceController : Controller
    {
        IProFormaInvoiceService ProFormaInvoiceService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public ProFormaInvoiceController(IProFormaInvoiceService _ProFormaInvoiceService, IConfiguration _configuratio)
        {
            ProFormaInvoiceService = _ProFormaInvoiceService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult ProFormaInvoice(string id ,int tag)
        {
            ProFormaInvoice ca = new ProFormaInvoice();
            ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Curlst = BindCurrency();
            ca.Suplst = BindSupplier();
            ca.Joblst = BindJob();
            ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("PInv");
            if (dtv.Rows.Count > 0)
            {
                ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
            List<ProFormaInvoiceDetail> TData = new List<ProFormaInvoiceDetail>();
            ProFormaInvoiceDetail tda = new ProFormaInvoiceDetail();
            List<PTermsItem> TData1 = new List<PTermsItem>();
            PTermsItem tda1 = new PTermsItem();
            List<PAreaItem> TData2 = new List<PAreaItem>();
            PAreaItem tda2 = new PAreaItem();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new ProFormaInvoiceDetail();

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
                        tda = new ProFormaInvoiceDetail();
                        tda.itemid = dtt.Rows[i]["ITEMID"].ToString();
                        tda.item = dtt.Rows[i]["item"].ToString();
                        DataTable dtt1 = new DataTable();
                        dtt1 = datatrans.GetItemDetails(tda.item);
                        if (dtt1.Rows.Count > 0)
                        {
                  
                            tda.unit = dtt1.Rows[0]["UNITID"].ToString();
                            tda.itemdes = dtt1.Rows[i]["ITEMDESC"].ToString();
                        }
                            tda.qty = dtt.Rows[i]["totqty"].ToString();
                        tda.rate = dtt.Rows[i]["totrate"].ToString();
                        //tda.amount = dtt.Rows[i]["REFNO"].ToString();
                        tda.BaID = id;
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
                        double rate = Convert.ToDouble(tda.rate);
                        double quatity = Convert.ToDouble(tda.qty);
                        double amt= quatity * rate;
                        tda.amount = amt ;


                       

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
            model.ProFormalst = Data;
            return Json(model.ProFormalst);

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

                //if (dtUsers.Rows[i]["STATUS"].ToString() == "INACTIVE")
                //{
                //    View = "";
                //    DeleteRow = "";
                //}
                //else
                //{
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
                    delrow = DeleteRow,



                });
            }

            return Json(new
            {
                Reg
            });

        }
        public List<SelectListItem> BindSupplier()
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
    }
}
