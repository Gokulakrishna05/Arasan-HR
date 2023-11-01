using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCore.Reporting;
using NuGet.Packaging.Signing;
using System.Net.Mail;
using Arasan.Services.Qualitycontrol;

namespace Arasan.Controllers
{
    public class PO : Controller
    {
        IPO PoService;
        private string? _connectionString;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        IConfiguration? _configuratio;
        DataTransactions datatrans;
        public PO(IPO _PoService, IConfiguration _configuratio, IWebHostEnvironment WebHostEnvironment)
        {
            this._WebHostEnvironment = WebHostEnvironment;
            PoService = _PoService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            //object value = InitializeComponent();
        }

        public IActionResult PurchaseOrder(string id)
        {
            Models.PO po = new Models.PO();
            po.Brlst = BindBranch();
            po.Suplst = BindSupplier();
            po.Curlst = BindCurrency();
            po.RecList = BindEmp();
            po.assignList = BindEmp();
            po.Paymenttermslst = BindPaymentterms();
            po.deltermlst = Binddeliveryterms();
            po.warrantytermslst = Bindwarrantyterms();
            po.desplst = Binddespatch();
            List<POItem> TData = new List<POItem>();
            POItem tda = new POItem();

            if (id == null)
            {

            }
            else
            {
                double total = 0;
                DataTable dt = new DataTable();
                dt = PoService.EditPObyID(id);
                if (dt.Rows.Count > 0)
                {
                    po.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    po.BranchId = dt.Rows[0]["BRANCHID"].ToString();
                    po.POdate = dt.Rows[0]["DOCDATE"].ToString();
                    po.Supplier = dt.Rows[0]["PARTYID"].ToString();
                    po.SuppId = dt.Rows[0]["PARTYID"].ToString();
                    po.PONo = dt.Rows[0]["DOCID"].ToString();
                    po.ID = id;
                    po.QuoteNo = dt.Rows[0]["Quotno"].ToString();
                    po.Cur = dt.Rows[0]["MAINCURRENCY"].ToString();
                    po.ExRate = dt.Rows[0]["EXRATE"].ToString();
                    po.RefNo = dt.Rows[0]["REFNO"].ToString();
                    po.RefDate = dt.Rows[0]["REFDT"].ToString();

                    po.Paymentterms = dt.Rows[0]["PAYTERMS"].ToString();
                    po.delterms = dt.Rows[0]["DELTERMS"].ToString();
                    po.desp = dt.Rows[0]["DESP"].ToString();
                    po.warrantyterms = dt.Rows[0]["WARRTERMS"].ToString();

                    po.Narration = dt.Rows[0]["NARRATION"].ToString();
                    po.Roundminus = Convert.ToDouble(dt.Rows[0]["ROUND_OFF_MINUS"].ToString() == "" ? "0" : dt.Rows[0]["ROUND_OFF_MINUS"].ToString());
                    po.Round = Convert.ToDouble(dt.Rows[0]["ROUND_OFF_PLUS"].ToString() == "" ? "0" : dt.Rows[0]["ROUND_OFF_PLUS"].ToString());
                    po.otherdeduction = Convert.ToDouble(dt.Rows[0]["OTHER_DEDUCTION"].ToString() == "" ? "0" : dt.Rows[0]["OTHER_DEDUCTION"].ToString());
                    po.Othercharges = Convert.ToDouble(dt.Rows[0]["OTHER_CHARGES"].ToString() == "" ? "0" : dt.Rows[0]["OTHER_CHARGES"].ToString());
                    po.Packingcharges = Convert.ToDouble(dt.Rows[0]["PACKING_CHRAGES"].ToString() == "" ? "0" : dt.Rows[0]["PACKING_CHRAGES"].ToString());
                    po.Frieghtcharge = Convert.ToDouble(dt.Rows[0]["FREIGHT"].ToString() == "" ? "0" : dt.Rows[0]["FREIGHT"].ToString());

                    po.Gross = Convert.ToDouble(dt.Rows[0]["GROSS"].ToString() == "" ? "0" : dt.Rows[0]["GROSS"].ToString());
                    po.Net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());

                }
                DataTable dt2 = new DataTable();
                dt2 = PoService.GetPOItemDetails(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new POItem();
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
                            tda.Desc = dt4.Rows[0]["ITEMDESC"].ToString();
                            tda.Conversionfactor = dt4.Rows[0]["CF"].ToString();
                            tda.rate = Convert.ToDouble(dt4.Rows[0]["LATPURPRICE"].ToString());
                        }
                        tda.Quantity = Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                        toaamt = tda.rate * tda.Quantity;
                        total += toaamt;
                        //tda.QtyPrim= Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                        tda.Amount = toaamt;
                        tda.Unit = dt2.Rows[i]["UNITID"].ToString();
                        tda.PURLst = BindPurType();
                        //tda.unitprim= dt2.Rows[i]["UNITID"].ToString();
                        tda.CGSTPer = Convert.ToDouble(dt2.Rows[i]["CGSTP"].ToString() == "" ? "0" : dt2.Rows[i]["CGSTP"].ToString());
                        tda.SGSTPer = Convert.ToDouble(dt2.Rows[i]["SGSTP"].ToString() == "" ? "0" : dt2.Rows[i]["SGSTP"].ToString());
                        tda.IGSTPer = Convert.ToDouble(dt2.Rows[i]["IGSTP"].ToString() == "" ? "0" : dt2.Rows[i]["IGSTP"].ToString());
                        tda.CGSTAmt = Convert.ToDouble(dt2.Rows[i]["CGST"].ToString() == "" ? "0" : dt2.Rows[i]["CGST"].ToString());
                        tda.SGSTAmt = Convert.ToDouble(dt2.Rows[i]["SGST"].ToString() == "" ? "0" : dt2.Rows[i]["SGST"].ToString());
                        tda.IGSTAmt = Convert.ToDouble(dt2.Rows[i]["IGST"].ToString() == "" ? "0" : dt2.Rows[i]["IGST"].ToString());
                        tda.DiscPer = Convert.ToDouble(dt2.Rows[i]["DISCPER"].ToString() == "" ? "0" : dt2.Rows[i]["DISCPER"].ToString());
                        tda.DiscAmt = Convert.ToDouble(dt2.Rows[i]["DISCAMT"].ToString() == "" ? "0" : dt2.Rows[i]["DISCAMT"].ToString());
                        tda.FrieghtAmt = Convert.ToDouble(dt2.Rows[i]["FREIGHTCHGS"].ToString() == "" ? "0" : dt2.Rows[i]["FREIGHTCHGS"].ToString());
                        tda.TotalAmount = Convert.ToDouble(dt2.Rows[i]["TOTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["TOTAMT"].ToString());
                        tda.Purtype = dt2.Rows[i]["PURTYPE"].ToString();
                        tda.Duedate = dt2.Rows[i]["DUEDATE"].ToString();
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }
                po.PoItem = TData;

            }
            return View(po);
        }
        [HttpPost]
        public ActionResult PurchaseOrder(Models.PO Cy, string id)
        {

            try
            {
                Cy.POID = id;
                string Strout = PoService.PurOrderCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.POID == null)
                    {
                        TempData["notice"] = "PO Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "PO Updated Successfully...!";
                    }
                    return RedirectToAction("ListPO");
                }

                else
                {
                    ViewBag.PageTitle = "Edit PO";
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
        public IActionResult ListPO(string status)
        {
            IEnumerable<Models.PO> cmp = PoService.GetAllPO(status);
            return View(cmp);
        }
        public IActionResult ListGateInWard(string fromdate, string todate)
        {
            GateInward cmp = new GateInward();
            DateTime now = DateTime.Now;
            if (string.IsNullOrEmpty(fromdate) && string.IsNullOrEmpty(todate))
            {
                fromdate = now.ToString("dd-MMM-yyyy");
                todate = now.ToString("dd-MMM-yyyy");
            }
            DataTable dt = new DataTable();
            dt = PoService.GetAllGateInward(fromdate, todate);
            cmp.fromdate = fromdate;
            cmp.todate = todate;
            List<GateInwardItem> TData = new List<GateInwardItem>();
            GateInwardItem tda = new GateInwardItem();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tda = new GateInwardItem();
                    tda.Supplier = dt.Rows[i]["PARTYNAME"].ToString();
                    tda.Status = dt.Rows[i]["STATUS"].ToString();
                    tda.GateInDate = dt.Rows[i]["GATE_IN_DATE"].ToString();
                    tda.GateInTime = dt.Rows[i]["GATE_IN_TIME"].ToString();
                    tda.PONo = dt.Rows[i]["DOCID"].ToString();
                    tda.POID = dt.Rows[i]["GATE_IN_ID"].ToString();
                    tda.TotalQty = dt.Rows[i]["TOTAL_QTY"].ToString() == "" ? 0 : Convert.ToDouble(dt.Rows[i]["TOTAL_QTY"].ToString());
                    TData.Add(tda);
                }
            }
            cmp.GateInlst = TData;
            return View(cmp);
        }
        public IActionResult GateInward()
        {
            GateInward GI = new GateInward();
            GI.Suplst = BindSupplier();
            GI.GateInDate = DateTime.Now.ToString("dd-MMM-yyyy");
            GI.GateInTime = DateTime.Now.ToString("h:mm");
            return View(GI);
        }

        [HttpPost]
        public ActionResult GateInward(GateInward Cy, string id)
        {

            try
            {
                string Strout = PoService.GateInwardCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Gate Inward Entered Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Gate Inward Updated Successfully...!";
                    }
                    return RedirectToAction("GateInward");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Gate Inward";
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
        public JsonResult GetSuppJSON(string supid)
        {
            GateInward model = new GateInward();
            model.POlst = BindPOlst(supid);
            return Json(BindPOlst(supid));

        }
        public JsonResult GetPOJSON(string poid)
        {
            GateInward model = new GateInward();
            DataTable dtt = new DataTable();
            List<POGateItem> Data = new List<POGateItem>();
            POGateItem tda = new POGateItem();
            dtt = PoService.GetPOItembyID(poid);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new POGateItem();
                    tda.itemid = dtt.Rows[i]["Itemi"].ToString();
                    tda.itemname = dtt.Rows[i]["ITEMID"].ToString();
                    tda.unit = dtt.Rows[i]["UNITID"].ToString();
                    tda.quantity = Convert.ToDouble(dtt.Rows[i]["QTY"].ToString());
                    DataTable dt4 = new DataTable();
                    dt4 = datatrans.GetItemDetails(tda.itemid);
                    if (dt4.Rows.Count > 0)
                    {
                        tda.Conversionfactor = dt4.Rows[0]["CF"].ToString();
                        tda.qc = dt4.Rows[0]["QCCOMPFLAG"].ToString();
                    }
                    Data.Add(tda);
                }
            }
            //ca.Net = tot;
            model.PoItem = Data;
            return Json(model.PoItem);

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
        public List<SelectListItem> BindPurType()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "CONSUMABLES PURCHASE", Value = "CONSUMABLES PURCHASE" });
                lstdesg.Add(new SelectListItem() { Text = "FIXED PURCHASE", Value = "FIXED PURCHASE" });
                lstdesg.Add(new SelectListItem() { Text = "MACHINERIES PURCHASE", Value = "MACHINERIES PURCHASE" });
                lstdesg.Add(new SelectListItem() { Text = "RAW MATERIAL", Value = "RAW MATERIAL" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindPaymentterms()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "30 Days  Credit", Value = "30 Days  Credit" });
                lstdesg.Add(new SelectListItem() { Text = "10 Days  Credit", Value = "10 Days  Credit" });
                lstdesg.Add(new SelectListItem() { Text = "Against Proforma Invoice", Value = "Against Proforma Invoice" });

                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> Binddespatch()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "ALAGAR SAMY PARCEL SERVICE", Value = "ALAGAR SAMY PARCEL SERVICE" });
                lstdesg.Add(new SelectListItem() { Text = "By Lorry", Value = "By Lorry" });
                lstdesg.Add(new SelectListItem() { Text = "R.G. Road Lines", Value = "R.G. Road Lines" });

                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> Bindwarrantyterms()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "One Year Free service", Value = "One Year Free service" });
                lstdesg.Add(new SelectListItem() { Text = "3 Years On Site Warrenty", Value = "3 Years On Site Warrenty" });

                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> Binddeliveryterms()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "immediate (Indimate despatch details by mail)", Value = "immediate (Indimate despatch details by mail)" });
                lstdesg.Add(new SelectListItem() { Text = "20 Days (Indimate despatch details by mail)", Value = "20 Days (Indimate despatch details by mail)" });
                lstdesg.Add(new SelectListItem() { Text = "7 to 10 Days (Indimate despatch details by mail)", Value = "7 to 10 Days (Indimate despatch details by mail)" });
                lstdesg.Add(new SelectListItem() { Text = "Within 14 Days only ", Value = "Within 14 Days only " });
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
        public List<SelectListItem> BindEmp()
        {
            try
            {
                DataTable dtDesg = datatrans.GetEmp();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPNAME"].ToString(), Value = dtDesg.Rows[i]["EMPMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
        public List<SelectListItem> BindPOlst(string value)
        {
            try
            {
                DataTable dtDesg = PoService.GetPObySuppID(value);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["POBASICID"].ToString() });
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
        public IActionResult PODetails(string id)
        {
            IEnumerable<POItem> cmp = PoService.GetAllPOItem(id);
            return View(cmp);
        }
        public IActionResult GateInwardDetails(string id)
        {
            IEnumerable<POItem> cmp = PoService.GetAllGateInwardItem(id);
            return View(cmp);
        }
        public IActionResult ViewPO(string id)
        {
            Models.PO ca = new Models.PO();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();
            dt = PoService.GetPObyID(id);
            if (dt.Rows.Count > 0)
            {
                ca.Supplier = dt.Rows[0]["PARTYNAME"].ToString();
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.PONo = dt.Rows[0]["DOCID"].ToString();
                ca.POdate = dt.Rows[0]["DOCDATE"].ToString();
                ca.QuoteNo = dt.Rows[0]["Quotno"].ToString();
                ca.QuoteDate = dt.Rows[0]["Quotedate"].ToString();
                ca.Roundminus = Convert.ToDouble(dt.Rows[0]["ROUND_OFF_MINUS"].ToString() == "" ? "0" : dt.Rows[0]["ROUND_OFF_MINUS"].ToString());
                ca.Round = Convert.ToDouble(dt.Rows[0]["ROUND_OFF_PLUS"].ToString() == "" ? "0" : dt.Rows[0]["ROUND_OFF_PLUS"].ToString());
                ca.otherdeduction = Convert.ToDouble(dt.Rows[0]["OTHER_DEDUCTION"].ToString() == "" ? "0" : dt.Rows[0]["OTHER_DEDUCTION"].ToString());
                ca.Othercharges = Convert.ToDouble(dt.Rows[0]["OTHER_CHARGES"].ToString() == "" ? "0" : dt.Rows[0]["OTHER_CHARGES"].ToString());
                ca.Packingcharges = Convert.ToDouble(dt.Rows[0]["PACKING_CHRAGES"].ToString() == "" ? "0" : dt.Rows[0]["PACKING_CHRAGES"].ToString());
                ca.Frieghtcharge = Convert.ToDouble(dt.Rows[0]["FREIGHT"].ToString() == "" ? "0" : dt.Rows[0]["FREIGHT"].ToString());

                ca.Gross = Convert.ToDouble(dt.Rows[0]["GROSS"].ToString() == "" ? "0" : dt.Rows[0]["GROSS"].ToString());
                ca.Net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());
                ca.ID = id;
            }
            List<POItem> Data = new List<POItem>();
            POItem tda = new POItem();
            double tot = 0;
            dtt = PoService.GetPOItembyID(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new POItem();
                    tda.ItemId = dtt.Rows[i]["ITEMID"].ToString();
                    tda.Unit = dtt.Rows[i]["UNITID"].ToString();
                    tda.Quantity = Convert.ToDouble(dtt.Rows[i]["QTY"].ToString());
                    tda.rate = Convert.ToDouble(dtt.Rows[i]["RATE"].ToString());
                    tda.Amount = tda.Quantity * tda.rate;
                    tot += tda.Amount;
                    tda.CGSTPer = Convert.ToDouble(dtt.Rows[i]["CGSTP"].ToString() == "" ? "0" : dtt.Rows[i]["CGSTP"].ToString());
                    tda.SGSTPer = Convert.ToDouble(dtt.Rows[i]["SGSTP"].ToString() == "" ? "0" : dtt.Rows[i]["SGSTP"].ToString());
                    tda.IGSTPer = Convert.ToDouble(dtt.Rows[i]["IGSTP"].ToString() == "" ? "0" : dtt.Rows[i]["IGSTP"].ToString());
                    tda.CGSTAmt = Convert.ToDouble(dtt.Rows[i]["CGST"].ToString() == "" ? "0" : dtt.Rows[i]["CGST"].ToString());
                    tda.SGSTAmt = Convert.ToDouble(dtt.Rows[i]["SGST"].ToString() == "" ? "0" : dtt.Rows[i]["SGST"].ToString());
                    tda.IGSTAmt = Convert.ToDouble(dtt.Rows[i]["IGST"].ToString() == "" ? "0" : dtt.Rows[i]["IGST"].ToString());
                    tda.DiscPer = Convert.ToDouble(dtt.Rows[i]["DISCPER"].ToString() == "" ? "0" : dtt.Rows[i]["DISCPER"].ToString());
                    tda.DiscAmt = Convert.ToDouble(dtt.Rows[i]["DISCAMT"].ToString() == "" ? "0" : dtt.Rows[i]["DISCAMT"].ToString());
                    tda.FrieghtAmt = Convert.ToDouble(dtt.Rows[i]["FREIGHTCHGS"].ToString() == "" ? "0" : dtt.Rows[i]["FREIGHTCHGS"].ToString());
                    tda.TotalAmount = Convert.ToDouble(dtt.Rows[i]["TOTAMT"].ToString() == "" ? "0" : dtt.Rows[i]["TOTAMT"].ToString());
                    Data.Add(tda);
                }
            }
            //ca.Net = tot;
            ca.PoItem = Data;
            return View(ca);
        }
        [HttpPost]
        public ActionResult ViewPO(Models.PO Cy, string id)
        {
            try
            {
                Cy.ID = id;
                string Strout = PoService.POtoGRN(Cy.ID);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "GRN Generated Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "GRN Generated Successfully...!";
                    }
                    return RedirectToAction("ListPO");
                }

                else
                {
                    ViewBag.PageTitle = "Edit PO";
                    TempData["notice"] = Strout;
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("ListPO");
        }

        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = PoService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListPO");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListPO");
            }
        }
        public async Task<IActionResult> Print(string id)
        {

            string mimtype = "";
            int extension = 1;
            string DrumID = datatrans.GetDataString("Select PARTYID from POBASIC where POBASICID='" + id + "' ");
            DataSet ds = new DataSet();
            var path = $"{this._WebHostEnvironment.WebRootPath}\\Reports\\Basic.rdlc";
            Dictionary<string, string> Parameters = new Dictionary<string, string>();
            //  Parameters.Add("rp1", " Hi Everyone");
            var Poitem = await PoService.GetPOItem(id, DrumID);
        
            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("DataSet1", Poitem);
            //localReport.AddDataSource("DataSet1_DataTable1", po);
            var result = localReport.Execute(RenderType.Pdf, extension, Parameters, mimtype);

            return File(result.MainStream, "application/Pdf");
        }

     
         public ActionResult SendMail(string id)
        {


            try
            {
                datatrans = new DataTransactions(_connectionString);
                MailRequest requestwer = new MailRequest();

                requestwer.ToEmail = "deepa@icand.in";
                requestwer.Subject = "Purchase Order";
                //string Content = "";

                string Content = "Attached my file";
               

                requestwer.Body = Content;
                //request.Attachments = "No";
                datatrans.sendemailpo("Test mail", Content, "gokulakrishna76@gmail.com", "kesavanmoorthi70@gmail.com", "spabnjcirlfipjco", "587", "true", "smtp.gmail.com", "IcanD");
                return Ok();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public IActionResult ViewGateInward(string id)
        {
            GateInward ca = new GateInward();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();

            dt = PoService.GetViewGateInward(id);
            if (dt.Rows.Count > 0)
            {
                ca.Supplier = dt.Rows[0]["PARTYNAME"].ToString();
                ca.POId = dt.Rows[0]["DOCID"].ToString();
                ca.GateInDate = dt.Rows[0]["GATE_IN_DATE"].ToString();
                ca.GateInTime = dt.Rows[0]["GATE_IN_TIME"].ToString();
                ca.ID = id;
                ca.TotalQty = Convert.ToDouble(dt.Rows[0]["TOTAL_QTY"].ToString());
                ca.Narration = dt.Rows[0]["NARRATION"].ToString();

                List<GateInwardItem> Data = new List<GateInwardItem>();
                GateInwardItem tda = new GateInwardItem();
                //double tot = 0;

                dtt = PoService.GetViewGateItems(id);
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda.ItemId = dtt.Rows[0]["ITEMID"].ToString();
                        tda.QC = dtt.Rows[0]["QCT"].ToString();
                        tda.Unit = dtt.Rows[0]["UNITID"].ToString();
                        tda.Quantity = dtt.Rows[0]["QCFLAG"].ToString();
                        tda.inQuantity = dtt.Rows[0]["IN_QTY"].ToString();

                        Data.Add(tda);
                    }
                }

                ca.GateInlst = Data;
            }
            return View(ca);
        }

    }
}
