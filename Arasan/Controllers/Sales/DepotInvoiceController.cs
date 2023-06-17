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
using Arasan.Services.Store_Management;

namespace Arasan.Controllers.Sales
{
    public class DepotInvoiceController : Controller
    {
        IDepotInvoiceService DepotInvoiceService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public DepotInvoiceController(IDepotInvoiceService _DepotInvoiceService, IConfiguration _configuratio)
        {
            DepotInvoiceService = _DepotInvoiceService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult DepotInvoice(string id)
        {
            DepotInvoice ca = new DepotInvoice();
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
            List<DepotInvoiceItem> TData = new List<DepotInvoiceItem>();
            DepotInvoiceItem tda = new DepotInvoiceItem();

            if (id == null)
            {
                for (int i = 0; i < 2; i++)
                {
                    tda = new DepotInvoiceItem();
                    tda.ItemGrouplst = BindItemGrplst();
                    tda.Itemlst = BindItemlst("");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {

                // ca = directPurchase.GetDirectPurById(id);


                DataTable dt = new DataTable();
                double total = 0;
                dt = DepotInvoiceService.GetDepotInvoiceDeatils(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    ca.InvoType = dt.Rows[0]["INVTYPE"].ToString();
                    ca.InvNo = dt.Rows[0]["DOCID"].ToString();
                    ca.InvDate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.ID = id;
                    ca.RefNo = dt.Rows[0]["REFNO"].ToString();
                    ca.RefDate = dt.Rows[0]["REFDATE"].ToString();
                    ca.Currency = dt.Rows[0]["MAINCURRENCY"].ToString();
                    ca.ExRate = dt.Rows[0]["EXRATE"].ToString();
                    ca.Party = dt.Rows[0]["PARTYID"].ToString();
                    ca.Vocher = dt.Rows[0]["VTYPE"].ToString();
                    ca.Customer = dt.Rows[0]["CUSTPO"].ToString();
                    ca.Type = dt.Rows[0]["TYPE"].ToString();
                    ca.Ordsam = dt.Rows[0]["ORDERSAMPLE"].ToString();
                    ca.Sales = dt.Rows[0]["SALVAL"].ToString();
                    ca.RecBy = dt.Rows[0]["RECDBY"].ToString();
                    ca.Dis = dt.Rows[0]["DESPBY"].ToString();
                    ca.Inspect = dt.Rows[0]["INSPBY"].ToString();
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
                dt2 = DepotInvoiceService.GetEditItemDetails(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new DepotInvoiceItem();
                        double toaamt = 0;
                        tda.ItemGrouplst = BindItemGrplst();
                        DataTable dt3 = new DataTable();
                        dt3 = datatrans.GetItemSubGroup(dt2.Rows[i]["ITEMID"].ToString());
                        if (dt3.Rows.Count > 0)
                        {
                            tda.ItemGroupId = dt3.Rows[0]["SUBGROUPCODE"].ToString();
                        }
                        tda.Itemlst = BindItemlst(tda.ItemGroupId);
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
            ca.Depotlst = TData;
            return View(ca);

        }
        [HttpPost]
        public ActionResult DepotInvoice(DepotInvoice Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = DepotInvoiceService.DirectPurCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "DepotInvoice Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "DepotInvoice Updated Successfully...!";
                    }
                    return RedirectToAction("ListDepotInvoice");
                }

                else
                {
                    ViewBag.PageTitle = "Edit DepotInvoice";
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
        public IActionResult ListDepotInvoice()
        {
            IEnumerable<DepotInvoice> cmp = DepotInvoiceService.GetAllDepotInvoice();
            return View(cmp);
        }
        public JsonResult GetItemJSON(string itemid)
        {
            DepotInvoiceItem model = new DepotInvoiceItem();
            model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst(itemid));

        }
        public ActionResult GetItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string unit = "";
                string CF = "";
                string price = "";
                dt = datatrans.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    unit = dt.Rows[0]["UNITID"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();
                    dt1 = DepotInvoiceService.GetItemCF(ItemId, dt.Rows[0]["UNITMASTID"].ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        CF = dt1.Rows[0]["CF"].ToString();
                    }
                }

                var result = new { unit = unit, CF = CF, price = price };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetItemGrpJSON()
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItemGrplst());
        }
        public List<SelectListItem> BindItemlst(string value)
        {
            try
            {
                DataTable dtDesg = datatrans.GetItem(value);
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
        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = DepotInvoiceService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListDepotInvoice");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListDepotInvoice");
            }
        }

    }
}
