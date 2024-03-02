 using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.Pkcs;
using System.Xml.Linq;
using Arasan.Interface;
using Arasan.Models;
using Arasan.Services.Sales;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Arasan.Controllers.Sales
{
    public class SalesReturnController : Controller
    {
        ISalesReturn SRInterface;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public SalesReturnController(ISalesReturn _SalesReturnService, IConfiguration _configuratio)
        {
            SRInterface = _SalesReturnService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult SalesReturn(string id)
        {
            SalesReturn SR = new SalesReturn();
            SR.invoicelst = BindInvoice();
            SR.vlst = BindVtype();
            SR.Loc = BindLocation();
            SR.Branch = Request.Cookies["BranchId"];
            DataTable dtv = datatrans.GetSequence("SalRt");
            if (dtv.Rows.Count > 0)
            {
                SR.DocId = dtv.Rows[0]["PREFIX"].ToString() + "" + dtv.Rows[0]["last"].ToString();
            }
            //SR.Vtype = "1";
            SR.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            List<SalesReturnItem> TData = new List<SalesReturnItem>();
            SalesReturnItem tda = new SalesReturnItem();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new SalesReturnItem();
                   
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else {
                DataTable dt = new DataTable();

                dt = SRInterface.GetSalesRet(id);
                if (dt.Rows.Count > 0)
                {
                    SR.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    SR.Docdate = dt.Rows[0]["DOCDATE"].ToString();

                    SR.DocId = dt.Rows[0]["DOCID"].ToString();
                    SR.custname = dt.Rows[0]["PARTYNAME"].ToString();
                    SR.ID = id;
                    SR.invoiceid = dt.Rows[0]["INVOICENO"].ToString();
                    SR.transitlocation = dt.Rows[0]["TRANSITLOCID"].ToString();
                    SR.invoicedate = dt.Rows[0]["INVOICEDATE"].ToString();
                    SR.Vtype = dt.Rows[0]["TYPE"].ToString();
                    SR.RefNo = dt.Rows[0]["REFNO"].ToString();
                    SR.RefDate = dt.Rows[0]["REFDT"].ToString();
                    SR.location = dt.Rows[0]["LOCID"].ToString();

                    SR.gross = Convert.ToDouble(dt.Rows[0]["GROSS"].ToString() == "" ? "0" : dt.Rows[0]["GROSS"].ToString());
                    SR.net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());
                    ViewBag.invoiceid = id;
                }

            }
            
            return View(SR);
        }
        [HttpPost]
        public ActionResult SalesReturn(SalesReturn Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = SRInterface.SalesReturnCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "SalesReturn Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "SalesReturn Updated Successfully...!";
                    }
                    return RedirectToAction("ListSalesReturn");
                }

                else
                {
                    ViewBag.PageTitle = "Edit SalesReturn";
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
        public IActionResult ListSalesReturn()
        {
            //IEnumerable<SalesReturn> cmp = SRInterface.GetAllSalesReturn(status);
            return View();
        }
        public ActionResult MyListSalesReturnGrid(string strStatus)
        {
            List<ListSalesReturnItems> Reg = new List<ListSalesReturnItems>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)SRInterface.GetAllListSalesReturnItems(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string View = string.Empty;
                string DeleteRow = string.Empty;

                if (dtUsers.Rows[i]["STATUS"].ToString() == "INACTIVE")
                {
                    View = "";
                    DeleteRow = "";
                }
                else
                {
                    View = "<a href=ViewSalesReturn?id=" + dtUsers.Rows[i]["SALERETBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View Deatils' /></a>";
                    DeleteRow = "<a href=CloseQuote?id=" + dtUsers.Rows[i]["SALERETBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";

                }

                Reg.Add(new ListSalesReturnItems
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["SALERETBASICID"].ToString()),
                    //branch = dtUsers.Rows[i]["BRANCHID"].ToString(),
                    docid = dtUsers.Rows[i]["DOCID"].ToString(),
                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    custname = dtUsers.Rows[i]["PARTYNAME"].ToString(),
                    view = View,
                    delete = DeleteRow,



                });
            }

            return Json(new
            {
                Reg
            });

        }
        public List<SelectListItem> BindLocation()
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
        public List<SelectListItem> BindInvoice()
        {
            try
            {
                DataTable dtDesg = SRInterface.GetInvoice();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["PINVBASICID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindVtype()
        {
            try
            {
                DataTable dtDesg = datatrans.GetVType();
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

        public ActionResult GetInvoice(string id )
        {
            try
            {
                DataTable dt = new DataTable();
               
                string gross = "";
                string net = "";
                string partyname = "";
                string invdate = "";
                if (id != "edit")
                {
                    dt = SRInterface.GetInvoiceDetails(id);

                    if (dt.Rows.Count > 0)
                    {
                        gross = dt.Rows[0]["GROSS"].ToString();
                        net = dt.Rows[0]["NET"].ToString();
                        partyname = dt.Rows[0]["PARTYNAME"].ToString();
                        invdate = dt.Rows[0]["DOCDATE"].ToString();
                    }
                }
                //else
                //{
                //    DataTable dt1 = new DataTable();
                //    dt1 = SRInterface.GetSalesReturn(id);
                //    if (dt1.Rows.Count > 0)
                //    {
                //        partyname = dt1.Rows[0]["PARTYNAME"].ToString();
                //        invdate = dt1.Rows[0]["INVOICEDATE"].ToString();
                //        gross = dt1.Rows[0]["GROSS"].ToString();
                //        net = dt1.Rows[0]["NET"].ToString();
                //    }
                //}

                var result = new {  gross = gross, net = net, partyname = partyname , invdate = invdate };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetSRJSON(string invoiceid,string jobid)
        {
            SalesReturn model = new SalesReturn();
            DataTable dtt = new DataTable();
            List<SalesReturnItem> Data = new List<SalesReturnItem>();
            SalesReturnItem tda = new SalesReturnItem();
            if (invoiceid == "edit")
            {
                DataTable dt = new DataTable();

                dt = SRInterface.GetSalesRet(jobid);
                if (dt.Rows.Count > 0)
                {

                    model.custname = dt.Rows[0]["PARTYNAME"].ToString();
                   
                   
                    model.invoicedate = dt.Rows[0]["INVOICEDATE"].ToString();


                    model.gross = Convert.ToDouble(dt.Rows[0]["GROSS"].ToString() == "" ? "0" : dt.Rows[0]["GROSS"].ToString());
                    model.net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());
                    
                }

                DataTable dtt1 = new DataTable();
                dtt1 = SRInterface.GetSalesRetDetails(jobid);
                if (dtt1.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt1.Rows.Count; i++)
                    {
                        tda = new SalesReturnItem();
                        tda.itemname = dtt1.Rows[i]["ITEMID"].ToString();
                        tda.itemid = dtt1.Rows[i]["item"].ToString();
                        tda.unit = dtt1.Rows[i]["UNITID"].ToString();
                        tda.soldqty = dtt1.Rows[i]["QTYSOLD"].ToString();

                        tda.rate = dtt1.Rows[i]["RATE"].ToString();
                        tda.amount = dtt1.Rows[i]["AMOUNT"].ToString();
                        
                        tda.disc = Convert.ToDouble(dtt1.Rows[i]["DISECOUNT"].ToString() == "" ? "0" : dtt1.Rows[i]["DISECOUNT"].ToString());
                        tda.exicetype = dtt1.Rows[i]["EXCISETYPE"].ToString();
                        tda.quantity = dtt1.Rows[i]["QTY"].ToString();
                        tda.totalamount = Convert.ToDouble(dtt1.Rows[i]["TOTAMT"].ToString() == "" ? "0" : dtt1.Rows[i]["TOTAMT"].ToString());
                        tda.traiffid = dtt1.Rows[i]["TARIFFID"].ToString();

                        tda.cgstper = Convert.ToDouble(dtt1.Rows[i]["CGSTPER"].ToString() == "" ? "0" : dtt1.Rows[i]["CGSTPER"].ToString());
                        tda.cgstamt = Convert.ToDouble(dtt1.Rows[i]["CGSTAMT"].ToString() == "" ? "0" : dtt1.Rows[i]["CGSTAMT"].ToString());
                        tda.sgstamt = Convert.ToDouble(dtt1.Rows[i]["SGSTAMT"].ToString() == "" ? "0" : dtt1.Rows[i]["SGSTAMT"].ToString());
                        tda.sgstper = Convert.ToDouble(dtt1.Rows[i]["SGSTPER"].ToString() == "" ? "0" : dtt1.Rows[i]["SGSTPER"].ToString());
                        tda.igstamt = Convert.ToDouble(dtt1.Rows[i]["IGSTAMT"].ToString() == "" ? "0" : dtt1.Rows[i]["IGSTAMT"].ToString());
                        tda.igstper = Convert.ToDouble(dtt1.Rows[i]["IGSTPER"].ToString() == "" ? "0" : dtt1.Rows[i]["IGSTPER"].ToString());
                        tda.Isvalid = "Y";
                        

                        Data.Add(tda);
                    }
                }
            }
            else
            {
                dtt = SRInterface.GetInvoiceItem(invoiceid);
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda = new SalesReturnItem();
                        tda.itemid = dtt.Rows[i]["itemi"].ToString();
                        tda.itemname = dtt.Rows[i]["ITEMID"].ToString();
                        tda.unit = dtt.Rows[i]["UNITID"].ToString();
                        tda.quantity = dtt.Rows[i]["QTY"].ToString();
                        tda.soldqty = dtt.Rows[i]["QTY"].ToString();
                        tda.Isvalid = "Y";
                        //tda.confac = dtt.Rows[i]["CF"].ToString();
                        tda.rate = dtt.Rows[i]["RATE"].ToString();
                        tda.amount = dtt.Rows[i]["AMOUNT"].ToString();
                        tda.disc = dtt.Rows[i]["DISCPER"].ToString() == "" ? 0 : Convert.ToDouble(dtt.Rows[i]["DISCPER"].ToString());
                        tda.discamount = dtt.Rows[i]["DISCOUNT"].ToString() == "" ? 0 : Convert.ToDouble(dtt.Rows[i]["DISCOUNT"].ToString());
                        tda.frigcharge = dtt.Rows[i]["FREIGHT"].ToString() == "" ? 0 : Convert.ToDouble(dtt.Rows[i]["FREIGHT"].ToString());
                        tda.cgstamt = Convert.ToDouble(dtt.Rows[i]["CGST"].ToString());
                        tda.cgstper = Convert.ToDouble(dtt.Rows[i]["CGSTP"].ToString());
                        tda.sgstamt = Convert.ToDouble(dtt.Rows[i]["SGST"].ToString());
                        tda.sgstper = Convert.ToDouble(dtt.Rows[i]["SGSTP"].ToString());
                        tda.igstamt = Convert.ToDouble(dtt.Rows[i]["IGST"].ToString());
                        tda.igstper = Convert.ToDouble(dtt.Rows[i]["IGSTP"].ToString());
                        tda.totalamount = dtt.Rows[i]["TOTAMT"].ToString() == "" ? 0 : Convert.ToDouble(dtt.Rows[i]["TOTAMT"].ToString());
                        tda.exicetype = dtt.Rows[i]["EXCISETYPE"].ToString();
                        tda.traiffid = dtt.Rows[i]["TARIFFID"].ToString();
                        //tda.binid = dtt.Rows[i]["BINID"].ToString();
                        tda.unitid = dtt.Rows[i]["UNIT"].ToString();
                        DataTable dt = new DataTable();
                        //dt = PurReturn.Getstkqty(grnid, loc, branch);
                        //if (dt.Rows.Count > 0)
                        //{
                        //    tda.stkqty = dt.Rows[0]["QTY"].ToString();
                        //}
                        Data.Add(tda);
                    }
                }
            }
            model.returnlist = Data;
            return Json(model.returnlist);

        }

        public ActionResult CloseQuote(string tag, int id)
        {

            string flag = SRInterface.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListSalesReturn");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListSalesReturn");
            }
        }
        public IActionResult ViewSalesReturn(string id)
        {
            SalesReturn SR = new SalesReturn();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();

            dt = SRInterface.GetRetByName(id);
            if (dt.Rows.Count > 0)
            {
                SR.Branch = dt.Rows[0]["BRANCHID"].ToString();
                SR.Docdate = dt.Rows[0]["DOCDATE"].ToString();

                SR.DocId = dt.Rows[0]["DOCID"].ToString();
                SR.custname = dt.Rows[0]["PARTYNAME"].ToString();
                SR.ID = id;
                SR.invoiceid = dt.Rows[0]["INVOICENO"].ToString();
                SR.transitlocation = dt.Rows[0]["TRANSITLOCID"].ToString();
                SR.invoicedate = dt.Rows[0]["INVOICEDATE"].ToString();
                SR.Vtype = dt.Rows[0]["TYPE"].ToString();
                SR.RefNo = dt.Rows[0]["REFNO"].ToString();
                SR.RefDate = dt.Rows[0]["REFDT"].ToString();
                SR.location = dt.Rows[0]["LOCID"].ToString();

                SR.gross = Convert.ToDouble(dt.Rows[0]["GROSS"].ToString());
                SR.net = Convert.ToDouble(dt.Rows[0]["NET"].ToString());
                SR.Narr = dt.Rows[0]["NARRATION"].ToString(); 

                SR.ID = id;


                List<SalesReturnItem> Data = new List<SalesReturnItem>();
                SalesReturnItem tda = new SalesReturnItem();
                //double tot = 0;

                dtt = SRInterface.GetRetItem(id);
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda.itemname = dtt.Rows[i]["ITEMID"].ToString();
                        tda.unit = dtt.Rows[i]["UNITID"].ToString();
                        tda.quantity = dtt.Rows[i]["QTY"].ToString();
                        tda.soldqty = dtt.Rows[i]["QTY"].ToString();
                        tda.rate = dtt.Rows[i]["RATE"].ToString();
                        tda.amount = dtt.Rows[i]["AMOUNT"].ToString();
                        //tda.disc = Convert.ToDouble(dtt.Rows[i]["DISCPER"].ToString());
                        tda.discamount = Convert.ToDouble(dtt.Rows[i]["DISCOUNT"].ToString());
                        tda.cgstamt = Convert.ToDouble(dtt.Rows[i]["CGSTAMT"].ToString());
                        tda.cgstper = Convert.ToDouble(dtt.Rows[i]["CGSTPER"].ToString());
                        tda.sgstamt = Convert.ToDouble(dtt.Rows[i]["SGSTAMT"].ToString());
                        tda.sgstper = Convert.ToDouble(dtt.Rows[i]["SGSTPER"].ToString());
                        tda.igstamt = Convert.ToDouble(dtt.Rows[i]["IGSTAMT"].ToString());
                        tda.igstper = Convert.ToDouble(dtt.Rows[i]["IGSTPER"].ToString());
                        tda.totalamount =  Convert.ToDouble(dtt.Rows[i]["TOTAMT"].ToString());
                        tda.exicetype = dtt.Rows[i]["EXCISETYPE"].ToString();
                        tda.traiffid = dtt.Rows[i]["TARIFFID"].ToString();
                        Data.Add(tda);
                    }
                }
               
                SR.returnlist = Data;
            }
            return View(SR);
        }



        //[HttpPost]
        //public ActionResult ViewQuote(SalesEnquiry Cy, string id)
        //{
        //    try
        //    {
        //        Cy.ID = id;
        //        string Strout = Sales.EnquirytoQuote(Cy.ID);
        //        if (string.IsNullOrEmpty(Strout))
        //        {
        //            if (Cy.ID == null)
        //            {
        //                TempData["notice"] = "SalesQuotation Generated Successfully...!";
        //            }
        //            else
        //            {
        //                TempData["notice"] = "SalesQuotation Generated Successfully...!";
        //            }
        //            return RedirectToAction("ListSalesEnquiry");
        //        }

        //        else
        //        {
        //            ViewBag.PageTitle = "Edit Sales_Enquiry";
        //            TempData["notice"] = Strout;
        //        }

        //        // }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return RedirectToAction("ListSalesEnquiry");
        //}
    }
}
