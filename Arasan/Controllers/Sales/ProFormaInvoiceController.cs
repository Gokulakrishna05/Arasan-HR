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
        public IActionResult ProFormaInvoice(string id)
        {
            ProFormaInvoice ca = new ProFormaInvoice();
            ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];
            //ca.Curlst = BindCurrency();
            //ca.Suplst = BindSupplier();
            ca.Joblst = BindJob();
            List<ProFormaInvoiceDetail> TData = new List<ProFormaInvoiceDetail>();
            ProFormaInvoiceDetail tda = new ProFormaInvoiceDetail();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new ProFormaInvoiceDetail();

                    tda.Isvalid = "Y";
                    TData.Add(tda);
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
                    ca.WorkCenter = dt.Rows[0]["WORKORDER"].ToString();
                    ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.RefNo = dt.Rows[0]["REFNO"].ToString();
                    ca.RefDate = dt.Rows[0]["REFDATE"].ToString();
                    ca.Currency = dt.Rows[0]["MAINCURR"].ToString();
                    ca.ExRate = dt.Rows[0]["EXRATE"].ToString();
                    ca.Party = dt.Rows[0]["PARTYID"].ToString();
                    ca.SalesValue = dt.Rows[0]["SALESVALUE"].ToString();
                    ca.Gross = dt.Rows[0]["GROSS"].ToString();
                    ca.Net = dt.Rows[0]["NET"].ToString();
                    ca.Amount = dt.Rows[0]["AMTWORDS"].ToString();
                    ca.BankName = dt.Rows[0]["BANKNAME"].ToString();
                    ca.AcNo = dt.Rows[0]["ACNO"].ToString();
                    ca.Address = dt.Rows[0]["SHIPADDRESS"].ToString();
                    ca.Narration = dt.Rows[0]["NARRATION"].ToString();
                    ca.ID = id;


                }
                //ca = SalesQuotationService.GetLocationsById(id);
                //DataTable dt2 = new DataTable();
                //dt2 = SalesQuotationService.GetSalesQuotationItemDetails(id);
                //if (dt2.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dt2.Rows.Count; i++)
                //    {
                //        tda = new QuoItem();
                //        double toaamt = 0;
                //        tda.ItemGrouplst = BindItemGrplst();
                //        DataTable dt3 = new DataTable();
                //        dt3 = datatrans.GetItemSubGroup(dt2.Rows[i]["ITEMID"].ToString());
                //        if (dt3.Rows.Count > 0)
                //        {
                //            tda.ItemGroupId = dt3.Rows[0]["SUBGROUPCODE"].ToString();
                //        }
                //        tda.Itlst = BindItemlst(tda.ItemId);
                //        tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                //        tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();
                //        DataTable dt4 = new DataTable();
                //        dt4 = datatrans.GetItemDetails(tda.ItemId);
                //        if (dt4.Rows.Count > 0)
                //        {
                //            tda.Des = dt4.Rows[0]["ITEMDESC"].ToString();
                //            tda.ConFac = dt4.Rows[0]["CF"].ToString();
                //            tda.Rate = Convert.ToDouble(dt4.Rows[0]["LATPURPRICE"].ToString());
                //        }
                //        tda.Quantity = Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                //        toaamt = tda.Rate * tda.Quantity;
                //        total += toaamt;
                //        //tda.QtyPrim= Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                //        tda.Amount = toaamt;
                //        tda.Unit = dt2.Rows[i]["UNIT"].ToString();
                //        //tda.unitprim= dt2.Rows[i]["UNITID"].ToString();
                //        tda.Disc = Convert.ToDouble(dt2.Rows[i]["DISC"].ToString() == "" ? "0" : dt2.Rows[i]["DISC"].ToString());
                //        tda.DiscAmount = Convert.ToDouble(dt2.Rows[i]["DISCAMOUNT"].ToString() == "" ? "0" : dt2.Rows[i]["DISCAMOUNT"].ToString());
                //        tda.FrigCharge = Convert.ToDouble(dt2.Rows[i]["IFREIGHTCH"].ToString() == "" ? "0" : dt2.Rows[i]["IFREIGHTCH"].ToString());
                //        tda.TotalAmount = Convert.ToDouble(dt2.Rows[i]["TOTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["TOTAMT"].ToString());
                //        tda.CGSTP = Convert.ToDouble(dt2.Rows[i]["CGSTPER"].ToString() == "" ? "0" : dt2.Rows[i]["CGSTPER"].ToString());
                //        tda.SGSTP = Convert.ToDouble(dt2.Rows[i]["SGSTPER"].ToString() == "" ? "0" : dt2.Rows[i]["SGSTPER"].ToString());
                //        tda.IGSTP = Convert.ToDouble(dt2.Rows[i]["IGSTPER"].ToString() == "" ? "0" : dt2.Rows[i]["IGSTPER"].ToString());
                //        tda.CGST = Convert.ToDouble(dt2.Rows[i]["CGSTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["CGSTAMT"].ToString());
                //        tda.SGST = Convert.ToDouble(dt2.Rows[i]["SGSTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["SGSTAMT"].ToString());
                //        tda.IGST = Convert.ToDouble(dt2.Rows[i]["IGSTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["IGSTAMT"].ToString());
                //        tda.Isvalid = "Y";
                //        TData.Add(tda);
                //    }
                //}
                //ca.Net = Math.Round(total, 2);
                //ca.QuoLst = Data;
            }
            //ca.QuoLst = TData;
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

                string  currency= "";
                string  party = "";
                if (ItemId != "edit")
                {
                    dt = ProFormaInvoiceService.GetProFormaInvoiceDetails(ItemId);

                    if (dt.Rows.Count > 0)
                    {

                        currency = dt.Rows[0]["MAINCURR"].ToString();
                        party = dt.Rows[0]["PARTY"].ToString();


                    }
                }

                var result = new { currency = currency, party = party};
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
                        tda.amount = dtt1.Rows[i]["AMOUNT"].ToString();
                        tda.discount = dtt1.Rows[i]["DISCOUNT"].ToString();
                        tda.itrodis = dtt1.Rows[i]["IDISC"].ToString();
                        tda.tradedis = dtt1.Rows[i]["TDISC"].ToString();
                        tda.cashdisc = dtt1.Rows[i]["CDISC"].ToString();
                        tda.tradedis = dtt1.Rows[i]["TDISC"].ToString();

                        tda.additionaldis = dtt1.Rows[i]["ADISC"].ToString();
                        tda.dis = dtt1.Rows[i]["SDISC"].ToString();
                        tda.frieght = dtt1.Rows[i]["FREIGHT"].ToString();
                        tda.tariff = dtt1.Rows[i]["TARIFFID"].ToString();
                        tda.CGST = dtt1.Rows[i]["CGST"].ToString();
                        tda.Isvalid = "Y";
                        tda.SGST = dtt1.Rows[i]["SGST"].ToString();
                        tda.IGST = dtt1.Rows[i]["IGST"].ToString();
                        tda.totamount = dtt1.Rows[i]["TOTEXAMT"].ToString();

                        Data.Add(tda);
                    }
                }
            }
            else
            {
                dtt = ProFormaInvoiceService.GetWorkOrderDetail(id);
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda = new ProFormaInvoiceDetail();

                        tda.itemid = dtt.Rows[i]["ITEMID"].ToString();
                        tda.itemdes = dtt.Rows[i]["ITEMSPEC"].ToString();
                        tda.unit = dtt.Rows[i]["UNITID"].ToString();
                        tda.qty = dtt.Rows[i]["QTY"].ToString();
                        tda.rate = dtt.Rows[i]["RATE"].ToString();
                        tda.amount = dtt.Rows[i]["AMOUNT"].ToString();
                        tda.discount = dtt.Rows[i]["DISCOUNT"].ToString();
                        tda.itrodis = dtt.Rows[i]["IDISC"].ToString();
                        tda.cashdisc = dtt.Rows[i]["CDISC"].ToString();
                        tda.tradedis = dtt.Rows[i]["TDISC"].ToString();
                        tda.additionaldis = dtt.Rows[i]["ADISC"].ToString();
                        tda.dis = dtt.Rows[i]["SDISC"].ToString();
                        tda.frieght = dtt.Rows[i]["FREIGHT"].ToString();
                        //tda.tariff = dtt.Rows[i]["TARIFFID"].ToString();
                        //tda.totamount = dtt.Rows[i]["TOTEXAMT"].ToString();
                        tda.ID = id;

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
            IEnumerable<ProFormaInvoice> cmp = ProFormaInvoiceService.GetAllProFormaInvoice(status);
            return View(cmp);
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
        public ActionResult CloseQuote(string tag, int id)
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
