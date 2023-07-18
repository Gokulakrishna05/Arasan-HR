using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class GRNController : Controller
    {
        IGRN GRNService;
        private string? _connectionString;
        IConfiguration? _configuratio;
        DataTransactions datatrans;
        public GRNController(IGRN _GRNService, IConfiguration _configuratio)
        {
            GRNService = _GRNService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult GRN(string id)
        {
            GRN po = new GRN();
            po.Brlst = BindBranch();
            po.Suplst = BindSupplier();
            po.Curlst = BindCurrency();
            po.RecList = BindEmp();
            po.assignList = BindEmp();
        
            List<POItem> TData = new List<POItem>();
            POItem tda = new POItem();

            if (id == null)
            {

            }
            else
            {
                double total = 0;
                DataTable dt = new DataTable();
                dt = GRNService.EditGRNbyID(id);
                if (dt.Rows.Count > 0)
                {
                    po.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    po.BranchID = dt.Rows[0]["BRANCHID"].ToString();
                    po.GRNNo= dt.Rows[0]["DOCID"].ToString();
                    po.GRNdate = dt.Rows[0]["DOCDATE"].ToString();
                    po.Supplier = dt.Rows[0]["PARTYID"].ToString();
                    po.PONo = dt.Rows[0]["DOCID"].ToString();
                    po.ID = id;
                    po.PONo = dt.Rows[0]["PONO"].ToString();
                    po.Cur = dt.Rows[0]["MAINCURRENCY"].ToString();
                    po.ExRate = dt.Rows[0]["EXRATE"].ToString();
                    po.RefNo = dt.Rows[0]["REFNO"].ToString();
                    po.RefDate = dt.Rows[0]["REFDT"].ToString();

                    po.Narration = dt.Rows[0]["NARRATION"].ToString();
                    po.Roundminus = Convert.ToDouble(dt.Rows[0]["ROUND_OFF_MINUS"].ToString() == "" ? "0" : dt.Rows[0]["ROUND_OFF_MINUS"].ToString());
                    po.Round = Convert.ToDouble(dt.Rows[0]["ROUND_OFF_PLUS"].ToString() == "" ? "0" : dt.Rows[0]["ROUND_OFF_PLUS"].ToString());
                    po.otherdeduction = Convert.ToDouble(dt.Rows[0]["OTHER_DEDUCTION"].ToString() == "" ? "0" : dt.Rows[0]["OTHER_DEDUCTION"].ToString());
                    po.Othercharges = Convert.ToDouble(dt.Rows[0]["OTHER_CHARGES"].ToString() == "" ? "0" : dt.Rows[0]["OTHER_CHARGES"].ToString());
                    po.Packingcharges = Convert.ToDouble(dt.Rows[0]["PACKING_CHRAGES"].ToString() == "" ? "0" : dt.Rows[0]["PACKING_CHRAGES"].ToString());
                    po.Frieghtcharge = Convert.ToDouble(dt.Rows[0]["FREIGHT"].ToString() == "" ? "0" : dt.Rows[0]["FREIGHT"].ToString());

                    po.Gross = Convert.ToDouble(dt.Rows[0]["GROSS"].ToString() == "" ? "0" : dt.Rows[0]["GROSS"].ToString());
                    po.Net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());
                    po.LRno = dt.Rows[0]["LRNO"].ToString();
                    po.LRdate= dt.Rows[0]["LRDT"].ToString();
                    po.dispatchname= dt.Rows[0]["DESPTHRU"].ToString();
                    po.drivername= dt.Rows[0]["TRNSPNAME"].ToString();
                   po.truckno= dt.Rows[0]["truckno"].ToString();

                }
                DataTable dt2 = new DataTable();
                dt2 = GRNService.GetGRNItembyID(id);
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
                           
                        }
                        tda.rate = Convert.ToDouble(dt2.Rows[i]["RATE"].ToString());
                        tda.Quantity = Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                        //tda.PendingQty= Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                        tda.BillQty = Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                        tda.Goodqty = Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                        toaamt = tda.rate * tda.Quantity;
                        total += toaamt;
                        //tda.QtyPrim= Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                        tda.Amount = toaamt;
                        tda.Unit = dt2.Rows[i]["UNITID"].ToString();
                        tda.PURLst = BindPurType();
                        //tda.unitprim= dt2.Rows[i]["UNITID"].ToString();
                        tda.CGSTPer = Convert.ToDouble(dt2.Rows[i]["CGSTPER"].ToString() == "" ? "0" : dt2.Rows[i]["CGSTPER"].ToString());
                        tda.SGSTPer = Convert.ToDouble(dt2.Rows[i]["SGSTPER"].ToString() == "" ? "0" : dt2.Rows[i]["SGSTPER"].ToString());
                        tda.IGSTPer = Convert.ToDouble(dt2.Rows[i]["IGSTPER"].ToString() == "" ? "0" : dt2.Rows[i]["IGSTPER"].ToString());
                        tda.CGSTAmt = Convert.ToDouble(dt2.Rows[i]["CGSTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["CGSTAMT"].ToString());
                        tda.SGSTAmt = Convert.ToDouble(dt2.Rows[i]["SGSTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["SGSTAMT"].ToString());
                        tda.IGSTAmt = Convert.ToDouble(dt2.Rows[i]["IGSTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["IGSTAMT"].ToString());
                        tda.DiscPer = Convert.ToDouble(dt2.Rows[i]["DISCPER"].ToString() == "" ? "0" : dt2.Rows[i]["DISCPER"].ToString());
                        tda.DiscAmt = Convert.ToDouble(dt2.Rows[i]["DISC"].ToString() == "" ? "0" : dt2.Rows[i]["DISC"].ToString());
                        tda.TotalAmount = Convert.ToDouble(dt2.Rows[i]["TOTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["TOTAMT"].ToString());
                        tda.Purtype = dt2.Rows[i]["PURTYPE"].ToString();
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }
                po.PoItem = TData;

            }
            return View(po);
        }
        [HttpPost]
        public ActionResult GRN(GRN Cy, string id)
        {

            try
            {
                Cy.GRNID = id;
                string Strout = GRNService.GRNCRUD(Cy);
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
                    return RedirectToAction("ListGRN");
                }

                else
                {
                    ViewBag.PageTitle = "Edit GRN";
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
        public IActionResult GRNDetails(string id)
        {
            IEnumerable<POItem> cmp = GRNService.GetAllGRNItem(id);
            return View(cmp);
        }
        public IActionResult ListGRN(string status)
        {
            IEnumerable<GRN> cmp = GRNService.GetAllGRN(status);
            return View(cmp);
        }
        public IActionResult GRNAccount(string id)
        {
            GRN grn = new GRN();
            DataTable dt = new DataTable();
            dt = GRNService.FetchAccountRec(id);
            List<GRNAccount> TData = new List<GRNAccount>();
            GRNAccount tda = new GRNAccount();
            double totalcredit = 0;
            double totaldebit = 0;
            if (dt.Rows.Count > 0)
            {
                grn.Roundminus = Convert.ToDouble(dt.Rows[0]["ROUND_OFF_MINUS"].ToString() == "" ? "0" : dt.Rows[0]["ROUND_OFF_MINUS"].ToString());
                grn.Round = Convert.ToDouble(dt.Rows[0]["ROUND_OFF_PLUS"].ToString() == "" ? "0" : dt.Rows[0]["ROUND_OFF_PLUS"].ToString());
                grn.otherdeduction = Convert.ToDouble(dt.Rows[0]["OTHER_DEDUCTION"].ToString() == "" ? "0" : dt.Rows[0]["OTHER_DEDUCTION"].ToString());
                grn.Othercharges = Convert.ToDouble(dt.Rows[0]["OTHER_CHARGES"].ToString() == "" ? "0" : dt.Rows[0]["OTHER_CHARGES"].ToString());
                grn.Packingcharges = Convert.ToDouble(dt.Rows[0]["PACKING_CHRAGES"].ToString() == "" ? "0" : dt.Rows[0]["PACKING_CHRAGES"].ToString());
                grn.Frieghtcharge = Convert.ToDouble(dt.Rows[0]["FREIGHT"].ToString() == "" ? "0" : dt.Rows[0]["FREIGHT"].ToString());

                grn.Gross = Convert.ToDouble(dt.Rows[0]["GROSS"].ToString() == "" ? "0" : dt.Rows[0]["GROSS"].ToString());
                grn.Net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());

                grn.DiscAmt = Convert.ToDouble(dt.Rows[0]["TOT_DISC"].ToString() == "" ? "0" : dt.Rows[0]["TOT_DISC"].ToString());
                grn.CGST = Convert.ToDouble(dt.Rows[0]["CGST"].ToString() == "" ? "0" : dt.Rows[0]["CGST"].ToString());
                grn.SGST = Convert.ToDouble(dt.Rows[0]["SGST"].ToString() == "" ? "0" : dt.Rows[0]["SGST"].ToString());
                grn.IGST = Convert.ToDouble(dt.Rows[0]["IGST"].ToString() == "" ? "0" : dt.Rows[0]["IGST"].ToString());
                //grn.TotalAmt= Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());
                
                if (grn.Gross > 0)
                { 
                    tda = new GRNAccount();
                    tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist= BindLedgerLst();
                    tda.CRAmount = grn.Gross;
                    tda.DRAmount = 0;
                    tda.TypeName = "Gross Amount";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Cr";
                    tda.symbol = "+";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    TData.Add(tda);
                }
                if (grn.CGST > 0)
                {
                    tda = new GRNAccount();
                    tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.CRAmount = grn.CGST;
                    tda.DRAmount = 0;
                    tda.TypeName = "CGST";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Cr";
                    tda.symbol = "+";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    TData.Add(tda);
                }
                if (grn.SGST > 0)
                {
                    tda = new GRNAccount();
                    tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.CRAmount = grn.SGST;
                    tda.DRAmount = 0;
                    tda.TypeName = "SGST";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Cr";
                    tda.symbol = "+";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    TData.Add(tda);
                }
                if (grn.IGST > 0)
                {
                    tda = new GRNAccount();
                    tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.CRAmount = grn.IGST;
                    tda.DRAmount = 0;
                    tda.TypeName = "IGST";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Cr";
                    tda.symbol = "+";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    TData.Add(tda);
                }
                if (grn.DiscAmt > 0)
                {
                    tda = new GRNAccount();
                    tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.CRAmount = 0;
                    tda.DRAmount = grn.DiscAmt;
                    tda.TypeName = "Discount";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Dr";
                    tda.symbol = "-";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    TData.Add(tda);
                }
                if (grn.Packingcharges > 0)
                {
                    tda = new GRNAccount();
                    tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.CRAmount = grn.Packingcharges;
                    tda.DRAmount = 0;
                    tda.TypeName = "Packing charges";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Cr";
                    tda.symbol = "+";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    TData.Add(tda);
                }
                if (grn.Frieghtcharge > 0)
                {
                    tda = new GRNAccount();
                    tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.CRAmount = grn.Frieghtcharge;
                    tda.DRAmount = 0;
                    tda.TypeName = "Frieght charges";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Cr";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    tda.symbol = "+";
                    TData.Add(tda);
                }
                if (grn.Othercharges > 0)
                {
                    tda = new GRNAccount();
                    tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.CRAmount = grn.Othercharges;
                    tda.DRAmount = 0;
                    tda.TypeName = "Other charges";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Cr";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    tda.symbol = "+";
                    TData.Add(tda);
                }
                if (grn.Round > 0)
                {
                    tda = new GRNAccount();
                    tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.CRAmount = grn.Round;
                    tda.DRAmount = 0;
                    tda.TypeName = "Round Off Plus";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Cr";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    tda.symbol = "+";
                    TData.Add(tda);
                }
                if (grn.Roundminus > 0)
                {
                    tda = new GRNAccount();
                    tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.CRAmount = 0;
                    tda.DRAmount = grn.Round;
                    tda.TypeName = "Round Off minus";
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
                    tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.CRAmount = 0;
                    tda.DRAmount = grn.otherdeduction;
                    tda.TypeName = "Other Deduction";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Dr";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    tda.symbol = "-";
                    TData.Add(tda);
                }
                if (grn.Net > 0)
                {
                    tda = new GRNAccount();
                    tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.CRAmount = 0;
                    tda.DRAmount = grn.Net;
                    tda.TypeName = "Total Debit";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Dr";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    tda.symbol = "-";
                    TData.Add(tda);
                }
            }
            grn.TotalCRAmt = totalcredit;
            grn.TotalDRAmt = totaldebit;
            grn.Acclst = TData;
            return View(grn);
        }
        public List<SelectListItem> BindCRDRLst()
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
        public List<SelectListItem> BindLedgerLst()
        {
            try
            {
                DataTable dtDesg = GRNService.LedgerList();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DISPNAME"].ToString(), Value = dtDesg.Rows[i]["MASTERID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetItemGrpJSON()
        {
               return Json(BindLedgerLst());
        }

        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = GRNService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListGRN");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListGRN");
            }
        }

    }
}
