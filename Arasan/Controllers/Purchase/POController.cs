﻿using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Arasan.Controllers
{
    public class POController : Controller
    {
        IPO PoService;
        private string? _connectionString;
        IConfiguration? _configuratio;
        DataTransactions datatrans;
        public POController(IPO _PoService, IConfiguration _configuratio)
        {
            PoService = _PoService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        
        public IActionResult PurchaseOrder(string id)
        {
            PO po = new PO();
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
                    po.POdate = dt.Rows[0]["DOCDATE"].ToString();
                    po.Supplier = dt.Rows[0]["PARTYID"].ToString();
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
                        tda.CGSTPer= Convert.ToDouble(dt2.Rows[i]["CGSTPER"].ToString() == "" ? "0" : dt2.Rows[i]["CGSTPER"].ToString());
                        tda.SGSTPer = Convert.ToDouble(dt2.Rows[i]["SGSTPER"].ToString() == "" ? "0" : dt2.Rows[i]["SGSTPER"].ToString());
                        tda.IGSTPer = Convert.ToDouble(dt2.Rows[i]["IGSTPER"].ToString() == "" ? "0" : dt2.Rows[i]["IGSTPER"].ToString());
                        tda.CGSTAmt = Convert.ToDouble(dt2.Rows[i]["CGSTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["CGSTAMT"].ToString());
                        tda.SGSTAmt = Convert.ToDouble(dt2.Rows[i]["SGSTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["SGSTAMT"].ToString());
                        tda.IGSTAmt = Convert.ToDouble(dt2.Rows[i]["IGSTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["IGSTAMT"].ToString());
                        tda.DiscPer = Convert.ToDouble(dt2.Rows[i]["DISCPER"].ToString() == "" ? "0" : dt2.Rows[i]["DISCPER"].ToString());
                        tda.DiscAmt = Convert.ToDouble(dt2.Rows[i]["DISCAMT"].ToString() == "" ? "0" : dt2.Rows[i]["DISCAMT"].ToString());
                        tda.FrieghtAmt = Convert.ToDouble(dt2.Rows[i]["FREIGHTCHGS"].ToString() == "" ? "0" : dt2.Rows[i]["FREIGHTCHGS"].ToString());
                        tda.TotalAmount = Convert.ToDouble(dt2.Rows[i]["TOTALAMT"].ToString() == "" ? "0" : dt2.Rows[i]["TOTALAMT"].ToString());
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
        public ActionResult PurchaseOrder(PO Cy, string id)
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
        public IActionResult ListPO()
        {
            IEnumerable<PO> cmp = PoService.GetAllPO();
            return View(cmp);
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
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTY"].ToString(), Value = dtDesg.Rows[i]["PARTYMASTID"].ToString() });
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
        public IActionResult PODetails(string id)
        {
            IEnumerable<POItem> cmp = PoService.GetAllPOItem(id);
            return View(cmp);
        }
        public IActionResult ViewPO(string id)
        {
            PO ca = new PO();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();
            dt = PoService.GetPObyID(id);
            if (dt.Rows.Count > 0)
            {
                ca.Supplier = dt.Rows[0]["PARTY"].ToString();
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
                    tda.CGSTPer = Convert.ToDouble(dtt.Rows[i]["CGSTPER"].ToString() == "" ? "0" : dtt.Rows[i]["CGSTPER"].ToString());
                    tda.SGSTPer = Convert.ToDouble(dtt.Rows[i]["SGSTPER"].ToString() == "" ? "0" : dtt.Rows[i]["SGSTPER"].ToString());
                    tda.IGSTPer = Convert.ToDouble(dtt.Rows[i]["IGSTPER"].ToString() == "" ? "0" : dtt.Rows[i]["IGSTPER"].ToString());
                    tda.CGSTAmt = Convert.ToDouble(dtt.Rows[i]["CGSTAMT"].ToString() == "" ? "0" : dtt.Rows[i]["CGSTAMT"].ToString());
                    tda.SGSTAmt = Convert.ToDouble(dtt.Rows[i]["SGSTAMT"].ToString() == "" ? "0" : dtt.Rows[i]["SGSTAMT"].ToString());
                    tda.IGSTAmt = Convert.ToDouble(dtt.Rows[i]["IGSTAMT"].ToString() == "" ? "0" : dtt.Rows[i]["IGSTAMT"].ToString());
                    tda.DiscPer = Convert.ToDouble(dtt.Rows[i]["DISCPER"].ToString() == "" ? "0" : dtt.Rows[i]["DISCPER"].ToString());
                    tda.DiscAmt = Convert.ToDouble(dtt.Rows[i]["DISCAMT"].ToString() == "" ? "0" : dtt.Rows[i]["DISCAMT"].ToString());
                    tda.FrieghtAmt = Convert.ToDouble(dtt.Rows[i]["FREIGHTCHGS"].ToString() == "" ? "0" : dtt.Rows[i]["FREIGHTCHGS"].ToString());
                    tda.TotalAmount = Convert.ToDouble(dtt.Rows[i]["TOTALAMT"].ToString() == "" ? "0" : dtt.Rows[i]["TOTALAMT"].ToString());
                    Data.Add(tda);
                }
            }
            //ca.Net = tot;
            ca.PoItem = Data;
            return View(ca);
        }
        [HttpPost]
        public ActionResult ViewPO(PO Cy, string id)
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
    }
}