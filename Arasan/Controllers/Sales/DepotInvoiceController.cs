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
            ca.Branch = Request.Cookies["BranchId"];
            ca.Curlst = BindCurrency();
            ca.Loclst = GetLoc();
            ca.Invlst = BindInVoiceType();
            ca.Suplst = BindSupplier();
            ca.Typelst = BindType();
            ca.Orderlst = BindOrder();
            ca.Dislst = BindDispatchType();
            ca.Inspelst = BindInspect();
            ca.Doclst = BindDocument();
            ca.Voclst = BindVocher();

            if (id == null)
            {
                //for (int i = 0; i < 3; i++)
                //{
                //    tda = new DirItem();
                //    tda.ItemGrouplst = BindItemGrplst();
                //    tda.Itemlst = BindItemlst("");
                //    tda.Isvalid = "Y";
                //    TData.Add(tda);
                //}
            }
            else
            {

                // ca = directPurchase.GetDirectPurById(id);


                //DataTable dt = new DataTable();
                //double total = 0;
                //dt = directPurchase.GetDirectPurchase(id);
                //if (dt.Rows.Count > 0)
                //{
                //    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                //    ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                //    ca.Supplier = dt.Rows[0]["PARTYID"].ToString();
                //    ca.DocNo = dt.Rows[0]["DOCID"].ToString();
                //    ca.ID = id;
                //    ca.Currency = dt.Rows[0]["MAINCURRENCY"].ToString();
                //    ca.RefDate = dt.Rows[0]["REFDT"].ToString();
                //    ca.Voucher = dt.Rows[0]["VOUCHER"].ToString();
                //    ca.Location = dt.Rows[0]["LOCID"].ToString();
                //    ca.Narration = dt.Rows[0]["NARR"].ToString();
                //    ca.LRCha = Convert.ToDouble(dt.Rows[0]["LRCH"].ToString() == "" ? "0" : dt.Rows[0]["LRCH"].ToString());
                //    ca.DelCh = Convert.ToDouble(dt.Rows[0]["DELCH"].ToString() == "" ? "0" : dt.Rows[0]["DELCH"].ToString());
                //    ca.Other = Convert.ToDouble(dt.Rows[0]["OTHERCH"].ToString() == "" ? "0" : dt.Rows[0]["OTHERCH"].ToString());
                //    ca.Frig = Convert.ToDouble(dt.Rows[0]["FREIGHT"].ToString() == "" ? "0" : dt.Rows[0]["FREIGHT"].ToString());
                //    ca.SpDisc = Convert.ToDouble(dt.Rows[0]["OTHERDISC"].ToString() == "" ? "0" : dt.Rows[0]["OTHERDISC"].ToString());

                //    ca.Gross = Convert.ToDouble(dt.Rows[0]["GROSS"].ToString() == "" ? "0" : dt.Rows[0]["GROSS"].ToString());
                //    ca.net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());

                //}
            }
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
            //IEnumerable<DepotInvoice> cmp = DepotInvoiceService.GetAllDepotInvoice();
            return View();
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

    }
}
