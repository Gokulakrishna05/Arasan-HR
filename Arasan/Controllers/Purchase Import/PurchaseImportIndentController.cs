﻿using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Interface.Master;
//using DocumentFormat.OpenXml.Wordprocessing;

namespace Arasan.Controllers.Store_Management
{
    public class PurchaseImportIndentController : Controller
    {
        IPurchaseImportIndent PurIndent;
        private string? _connectionString;
        IConfiguration? _configuratio;
        DataTransactions datatrans;
        public PurchaseImportIndentController(IPurchaseImportIndent _PurService, IConfiguration _configuratio)
        {
            PurIndent = _PurService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);

        }
        public IActionResult PurchaseImportIndent()
        {
            PurchaseImportIndent ca = new PurchaseImportIndent();
            ca.Brlst = BindBranch();
            ca.SLoclst = GetStoreLoc();
            ca.PURLst = BindPurType();
            ca.ELst = BindErection();
            ca.EmpLst = BindEmp();
            ca.Branch = Request.Cookies["BranchId"];
            ca.user = Request.Cookies["UserId"];
            ca.IndentDate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.RefDate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("Pind");
            if (dtv.Rows.Count > 0)
            {
                ca.IndentId = dtv.Rows[0]["PREFIX"].ToString() + "" + dtv.Rows[0]["last"].ToString();
            }
            List<PIndentItem> TData = new List<PIndentItem>();
            PIndentItem tda = new PIndentItem();
            for (int i = 0; i < 3; i++)
            {
                tda = new PIndentItem();
                //tda.ItemGrouplst = BindItemGrplst();
                tda.Itemlst = BindItemlst();
                tda.loclst = GetLoc();
                tda.Isvalid = "Y";
                TData.Add(tda);
            }
            ca.PILst = TData;
            //ca.ID = "0";
            List<PIndentTANDC> TData1 = new List<PIndentTANDC>();
            PIndentTANDC tda1 = new PIndentTANDC();
            for (int i = 0; i < 1; i++)
            {
                tda1 = new PIndentTANDC();
                tda1.Isvalid = "Y";
                TData1.Add(tda1);
            }
            ca.TANDClst = TData1;

            return View(ca);
        }

        [HttpPost]
        public IActionResult PurchaseImportIndent(PurchaseImportIndent Cy, string id)
        {
            //if (ModelState.IsValid)
            //{
            try
            {
                Cy.ID = id;
                string Strout = PurIndent.IndentCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Indent Created Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Indent Updated Successfully...!";
                    }
                    return RedirectToAction("ListPurchaseImportIndent");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Indent";
                    TempData["notice"] = Strout;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //}
            return View(Cy);
        }

        public ActionResult MyListIndentgrid()
        {
            List<IndentBindList> Reg = new List<IndentBindList>();
            DataTable dtUsers = new DataTable();
            //strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = PurIndent.GetIndent();
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                EditRow = "<a href=PurchaseImportIndent?id=" + dtUsers.Rows[i]["IPINDBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                DeleteRow = "<a href=DeleteIndent?tag=Del&id=" + dtUsers.Rows[i]["IPINDBASICID"].ToString() + " onclick='return confirm(" + "\"Are you sure you want to Disable this record...?\"" + ")'><img src='../Images/Inactive.png' alt='Deactivate' /></a>";

                Reg.Add(new IndentBindList
                {
                    piid = Convert.ToInt64(dtUsers.Rows[i]["IPINDBASICID"].ToString()),
                    branch = dtUsers.Rows[i]["BRANCHID"].ToString(),
                    indentno = dtUsers.Rows[i]["DOCID"].ToString(),
                    indentdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    EditRow = EditRow,
                    DelRow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }
        public ActionResult ListIndentItemgrid(string PRID)
        {
            List<IndentItemImportBindList> EnqChkItem = new List<IndentItemImportBindList>();
            DataTable dtEnq = new DataTable();
            dtEnq = PurIndent.GetIndentItem(PRID);
            for (int i = 0; i < dtEnq.Rows.Count; i++)
            {
                EnqChkItem.Add(new IndentItemImportBindList
                {
                    indentid = Convert.ToInt64(dtEnq.Rows[i]["IPINDDETAILID"].ToString()),
                    piid = Convert.ToInt64(dtEnq.Rows[i]["IPINDBASICID"].ToString()),
                    itemname = dtEnq.Rows[i]["ITEMID"].ToString(),
                    unit = dtEnq.Rows[i]["UNITID"].ToString(),
                    quantity = dtEnq.Rows[i]["QTY"].ToString(),
                    location = dtEnq.Rows[i]["LOCID"].ToString(),
                    duedate = dtEnq.Rows[i]["DUEDATE"].ToString(),
                });
            }

            return Json(new
            {
                EnqChkItem
            });
        }

        public ActionResult ListIndentItemgridApproval(string PRID)
        {
            List<IndentItemImportBindList> EnqChkItem = new List<IndentItemImportBindList>();
            DataTable dtEnq = new DataTable();
            dtEnq = PurIndent.GetIndentItemApprove();
            for (int i = 0; i < dtEnq.Rows.Count; i++)
            {
                string Approval = string.Empty;
                string DisApproval = string.Empty;
                Approval = "IndentApproved?id=" + dtEnq.Rows[i]["IPINDDETAILID"].ToString() + "";
                DisApproval = "IndentDisApproved?id=" + dtEnq.Rows[i]["IPINDDETAILID"].ToString() + "";
                EnqChkItem.Add(new IndentItemImportBindList
                {
                    indentid = Convert.ToInt64(dtEnq.Rows[i]["IPINDDETAILID"].ToString()),
                    piid = Convert.ToInt64(dtEnq.Rows[i]["IPINDBASICID"].ToString()),
                    itemname = dtEnq.Rows[i]["ITEMID"].ToString(),
                    unit = dtEnq.Rows[i]["UNITID"].ToString(),
                    quantity = dtEnq.Rows[i]["QTY"].ToString(),
                    location = dtEnq.Rows[i]["LOCID"].ToString(),
                    duedate = dtEnq.Rows[i]["DUEDATE"].ToString(),
                    indentno = dtEnq.Rows[i]["DOCID"].ToString(),
                    indentdate = dtEnq.Rows[i]["DOCDATE"].ToString(),
                    approval = Approval,
                    disapproval = DisApproval
                });
            }

            return Json(new
            {
                EnqChkItem
            });
        }

        public ActionResult ListIndentItemgridApproved(string PRID)
        {
            List<IndentItemImportBindList> EnqChkItem = new List<IndentItemImportBindList>();
            DataTable dtEnq = new DataTable();
            dtEnq = PurIndent.GetIndentItemApproved();
            for (int i = 0; i < dtEnq.Rows.Count; i++)
            {
                EnqChkItem.Add(new IndentItemImportBindList
                {
                    indentid = Convert.ToInt64(dtEnq.Rows[i]["IPINDDETAILID"].ToString()),
                    piid = Convert.ToInt64(dtEnq.Rows[i]["IPINDBASICID"].ToString()),
                    itemname = dtEnq.Rows[i]["ITEMID"].ToString(),
                    unit = dtEnq.Rows[i]["UNITID"].ToString(),
                    quantity = dtEnq.Rows[i]["QTY"].ToString(),
                    location = dtEnq.Rows[i]["LOCID"].ToString(),
                    duedate = dtEnq.Rows[i]["DUEDATE"].ToString(),
                    indentno = dtEnq.Rows[i]["DOCID"].ToString(),
                    indentdate = dtEnq.Rows[i]["DOCDATE"].ToString(),
                });
            }

            return Json(new
            {
                EnqChkItem
            });
        }

        public ActionResult ListIndentItemgridSupp(string PRID)
        {
            List<IndentItemImportBindList> EnqChkItem = new List<IndentItemImportBindList>();
            DataTable dtEnq = new DataTable();
            dtEnq = PurIndent.GetIndentItemSupp();
            string assign = string.Empty;

            for (int i = 0; i < dtEnq.Rows.Count; i++)
            {
                assign = "ImportIndentSupAllocation?id=" + dtEnq.Rows[i]["ITEMMASTERID"].ToString() + "";
                EnqChkItem.Add(new IndentItemImportBindList
                {

                    itemname = dtEnq.Rows[i]["ITEMID"].ToString(),
                    itemid = dtEnq.Rows[i]["ITEMMASTERID"].ToString(),
                    quantity = dtEnq.Rows[i]["QTY"].ToString(),
                    assign = assign,

                });
            }

            return Json(new
            {
                EnqChkItem
            });
        }

        public ActionResult ListIndentItemgridSuppDetails()
        {
            List<IndentItemImportBindList> EnqChkItem = new List<IndentItemImportBindList>();
            DataTable dtEnq = new DataTable();
            dtEnq = PurIndent.GetIndentItemSuppDetail();
            for (int i = 0; i < dtEnq.Rows.Count; i++)
            {
                EnqChkItem.Add(new IndentItemImportBindList
                {
                    indentid = Convert.ToInt64(dtEnq.Rows[i]["IPINDDETAILID"].ToString()),
                    piid = Convert.ToInt64(dtEnq.Rows[i]["IPINDBASICID"].ToString()),
                    itemid = dtEnq.Rows[i]["ITEMMASTERID"].ToString(),
                    itemname = dtEnq.Rows[i]["ITEMID"].ToString(),
                    unit = dtEnq.Rows[i]["UNITID"].ToString(),
                    quantity = dtEnq.Rows[i]["QTY"].ToString(),
                    location = dtEnq.Rows[i]["LOCID"].ToString(),
                    duedate = dtEnq.Rows[i]["DUEDATE"].ToString(),
                    indentno = dtEnq.Rows[i]["DOCID"].ToString(),
                    indentdate = dtEnq.Rows[i]["DOCDATE"].ToString(),
                });
            }

            return Json(new
            {
                EnqChkItem
            });
        }
        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dtDesg = PurIndent.GetBranch();
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

        public List<SelectListItem> BindEmp()
        {
            try
            {
                DataTable dtDesg = PurIndent.GetEmp();
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
        public List<SelectListItem> BindErection()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "Regular Consumption", Value = "Regular Consumption" });
                lstdesg.Add(new SelectListItem() { Text = "Erection Work", Value = "Erection Work" });
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
        public List<SelectListItem> GetStoreLoc()
        {
            try
            {
                DataTable dtDesg = PurIndent.GetSLocation();
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
        public JsonResult GetItemJSON()
        {
            EnqItem model = new EnqItem();
            model.Itemlst = BindItemlst();
            return Json(BindItemlst());

        }
        //public JsonResult GetSuppJSON(string partytype)
        //{
        //    IndentSuppAllocate model = new IndentSuppAllocate();
        //    model.Partylst = BindItemlst(itemid);
        //    return Json(BindItemlst(itemid));

        //}
        public JsonResult GetItemGrpJSON()
        {
            //EnqItem model = new EnqItem();
            //model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItemGrplst());
        }
        public JsonResult GetSupJSON()
        {
            return Json(BindSupplier());
        }
        public JsonResult GetexistingSupJSON(string itemid)
        {
            return Json(BindOldSupp(itemid));
        }

        public List<SelectListItem> BindSupplier()
        {
            try
            {
                DataTable dtDesg = PurIndent.GetSupplier();
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
        public JsonResult GetLocGSON()
        {
            return Json(GetLoc());
        }

        public List<SelectListItem> GetLoc()
        {
            try
            {
                DataTable dtDesg = PurIndent.GetLocation();
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
        public List<SelectListItem> BindItemlst()
        {
            try
            {
                DataTable dtDesg = datatrans.GetRawItem();
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
        public ActionResult IndentApproved(string id)
        {
            datatrans = new DataTransactions(_connectionString);
            bool result = datatrans.UpdateStatus("UPDATE IPINDDETAIL SET APPROVED1='YES',APPROVAL1U='SRRAJAN',APP1DT='" + DateTime.Now.ToString("dd-MMM-yyyy") + "' Where IPINDDETAILID='" + id + "'");
            return RedirectToAction("List_PI_Approval");
        }
        public ActionResult IndentDisApproved(string id)
        {
            string user = Request.Cookies["UserId"];
            datatrans = new DataTransactions(_connectionString);
            bool result = datatrans.UpdateStatus("UPDATE IPINDDETAIL SET APPROVED1='NO',MODIFYBY='" + user + "',MODIFY_ON='" + DateTime.Now.ToString("dd-MMM-yyyy") + "' Where IPINDDETAILID='" + id + "'");
            return RedirectToAction("List_PI_Approval");
        }
        public List<SelectListItem> BindItemGrplst()
        {
            try
            {
                DataTable dtDesg = PurIndent.GetItemSubGrp();
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
        public ActionResult GetItemDetail(string ItemId, string loc, string branch)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                string QC = "";
                string unit = "";
                string unitid = "";
                string indentqty = "0";
                string stk = "";
                string totalstock = "";
                string item = "";
                dt = PurIndent.GetItemDetails(ItemId);
                dt1 = PurIndent.GetIndetnPlacedDetails(ItemId);
                dt2 = PurIndent.GetSTKDetails(ItemId, loc, branch);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["QCYNTEMP"].ToString() == "" || string.IsNullOrEmpty(dt.Rows[0]["QCYNTEMP"].ToString()))
                    {
                        QC = "NO";
                    }
                    else
                    {
                        QC = dt.Rows[0]["QCYNTEMP"].ToString();
                    }

                    unit = dt.Rows[0]["UNITID"].ToString();
                    unitid = dt.Rows[0]["UNITMASTID"].ToString();
                }
                if (dt1.Rows.Count > 0)
                {
                    indentqty = dt1.Rows[0]["QTY"].ToString();
                }
                if (dt2.Rows.Count > 0)
                {
                    stk = dt2.Rows[0]["QTY"].ToString();
                }
                DataTable stock = datatrans.GetData("Select SUM(BALANCE_QTY) as qty from INVENTORY_ITEM where ITEM_ID='" + ItemId + "' AND BALANCE_QTY > 0 AND LOCATION_ID NOT IN '" + loc + "' AND BRANCH_ID='" + branch + "'  ");
                if (stock.Rows.Count > 0)
                {
                    totalstock = stock.Rows[0]["qty"].ToString();
                }
                item = ItemId;
                var result = new { QC = QC, unit = unit, unitid = unitid, indentqty = indentqty, stk = stk, totalstock = totalstock, item = item };
                return Json(result);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetSupp(string ItemId, string SuppId)
        {
            try
            {
                string rate = "";
                string unit = "";
                string lpprice = "";
                string lpdate = "";
                string qty = "";
                DataTable ds = new DataTable();
                ds = PurIndent.GetSuppPurchaseDetails(SuppId, ItemId);

                if (ds.Rows.Count > 0)
                {
                    rate = ds.Rows[0]["RATE"].ToString();
                    unit = ds.Rows[0]["PUNIT"].ToString();
                    lpprice = ds.Rows[0]["TOTAMT"].ToString();
                    lpdate = ds.Rows[0]["DOCDATE"].ToString();
                    qty = ds.Rows[0]["QTY"].ToString();
                }
                else
                {
                    rate = "New";
                    unit = "New";
                    lpprice = "New";
                    lpdate = "New";
                    qty = "New";
                }

                var result = new { rate = rate, unit = unit, lpprice = lpprice, lpdate = lpdate, qty = qty };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult WholeStock(string id)
        {
            PurchaseImportIndent MR = new PurchaseImportIndent();

            List<ImportTotalStockItem> TData = new List<ImportTotalStockItem>();
            ImportTotalStockItem tda = new ImportTotalStockItem();
            DataTable dtt = datatrans.GetData("Select ITEMMASTER.ITEMID,ITEM_ID,LOCDETAILS.LOCID,INVENTORY_ITEM_ID,LOCATION_ID,to_char(GRN_DATE,'dd-MON-yyyy')GRN_DATE,ITEM_ID,BALANCE_QTY from INVENTORY_ITEM left outer join ITEMMASTER ON ITEMMASTERID=INVENTORY_ITEM.ITEM_ID left outer join LOCDETAILS ON LOCDETAILSID=INVENTORY_ITEM.LOCATION_ID where ITEM_ID=" + id + " AND BALANCE_QTY > 0 AND LOCATION_ID NOT IN '10001000000827' AND BRANCH_ID='10001000000001'  ");

            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new ImportTotalStockItem();
                    tda.item = dtt.Rows[i]["ITEMID"].ToString();
                    tda.itemid = dtt.Rows[i]["ITEM_ID"].ToString();
                    tda.invid = dtt.Rows[i]["INVENTORY_ITEM_ID"].ToString();

                    tda.location = dtt.Rows[i]["LOCID"].ToString();
                    tda.locationid = dtt.Rows[i]["LOCATION_ID"].ToString();
                    tda.docDate = dtt.Rows[i]["GRN_DATE"].ToString();
                    tda.qty = dtt.Rows[i]["BALANCE_QTY"].ToString();

                    TData.Add(tda);
                }
            }
            MR.stklst = TData;
            return View(MR);
        }
        public IActionResult ListPurchaseImportIndent()
        {
            return View();
        }
        public IActionResult List_PI_Approval()
        {
            return View();
        }

        public IActionResult List_PI_Approved()
        {
            return View();
        }
        public IActionResult List_PI_Supp_Allocation()
        {
            return View();
        }
        public IActionResult List_PI_MSupp_Allocation()
        {
            ImportIndentSuppMultipleAllocate PI = new ImportIndentSuppMultipleAllocate();
            PI.Partylst = BindSupplier();
            return View(PI);
        }
        public ActionResult ImportIndentSupAllocation(string id)
        {
            ImportIndentSupAllocation ca = new ImportIndentSupAllocation();
            List<ImportIndentSupAllocationList> TData = new List<ImportIndentSupAllocationList>();
            ImportIndentSupAllocationList tda = new ImportIndentSupAllocationList();
            DataTable dt = new DataTable();
            dt = PurIndent.GetHistory(id);
            DataTable da = new DataTable();
            da = PurIndent.GetIndentItemSuppEnq(id);
            if (da.Rows.Count > 0)
            {
                ca.ItemName = da.Rows[0]["ITEMID"].ToString();
                ca.TotalQty = da.Rows[0]["QTY"].ToString();
                ca.ItemId = da.Rows[0]["ITEMMASTERID"].ToString();
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                tda = new ImportIndentSupAllocationList();
                tda.PartyName = dt.Rows[i]["PARTYNAME"].ToString();
                tda.Rate = dt.Rows[i]["RATE"].ToString();
                tda.Unit = dt.Rows[i]["PUNIT"].ToString();
                ca.UnitP = tda.Unit;
                tda.LastPurchsePrice = dt.Rows[i]["TOTAMT"].ToString();
                tda.LastPurchaseDate = dt.Rows[i]["DOCDATE"].ToString();
                tda.Qty = dt.Rows[i]["QTY"].ToString();
                TData.Add(tda);
            }
            ca.HSupLst = TData;


            List<ImportIndentSuppAllocate> TDataa = new List<ImportIndentSuppAllocate>();
            ImportIndentSuppAllocate tdaa = new ImportIndentSuppAllocate();
            DataTable dt1 = new DataTable();
            dt1 = PurIndent.GetLasttwoSupp(id);
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                tdaa = new ImportIndentSuppAllocate();
                tdaa.Partytype = "Existing";
                tdaa.Partylst = BindOldSupp(id);
                tdaa.PartyName = dt1.Rows[i]["PARTYID"].ToString();
                DataTable ds = new DataTable();
                ds = PurIndent.GetSuppPurchaseDetails(dt1.Rows[i]["PARTYID"].ToString(), id);
                if (ds.Rows.Count > 0)
                {
                    tdaa.Rate = ds.Rows[0]["RATE"].ToString();
                    tdaa.Unit = ds.Rows[0]["PUNIT"].ToString();
                    tdaa.LastPurchsePrice = ds.Rows[0]["TOTAMT"].ToString();
                    tdaa.LastPurchaseDate = ds.Rows[0]["DOCDATE"].ToString();
                    tdaa.Qty = ds.Rows[0]["QTY"].ToString();
                }
                tdaa.EnquiryQty = ca.TotalQty;
                tdaa.Isvalid = "Y";
                TDataa.Add(tdaa);
            }
            ca.indentSuppAllocates = TDataa;
            return View(ca);
        }
        public JsonResult GridRecordsSave(string[] selectedRecord, string supid)
        {
            string user = Request.Cookies["UserId"];
            string Strout = PurIndent.GenerateEnquiry(selectedRecord, supid, user);

            return Json(Strout);
        }
        public ActionResult MultipleSupAllocation(string selectedRecord)
        {
            string se = selectedRecord.Replace('[', ' ');
            string ses = se.Replace(']', ' ');
            string sed = ses.Replace('"', ' ').Trim();
            foreach (string number in sed.Split(','))
            {
                string nu = number;
            }
            ImportIndentSuppAllocate ca = new ImportIndentSuppAllocate();
            List<ImportIndentSuppAllocate> TDataa = new List<ImportIndentSuppAllocate>();
            ImportIndentSuppAllocate tdaa = new ImportIndentSuppAllocate();
            DataTable dt1 = new DataTable();
            //dt1 = PurIndent.GetLasttwoSupp(id);
            foreach (string number in sed.Split(','))
            {
                string nu = number.Trim();
                tdaa = new ImportIndentSuppAllocate();
                tdaa.Partylst = BindSupplier();
                //tdaa.ItemName=
                DataTable dr = new DataTable();
                dr = PurIndent.GetIndentItemSuppEnq(nu);
                if (dr.Rows.Count > 0)
                {
                    tdaa.EnquiryQty = dr.Rows[0]["QTY"].ToString();
                    tdaa.ItemName = dr.Rows[0]["ITEMID"].ToString();
                    tdaa.ItemId = dr.Rows[0]["ITEMMASTERID"].ToString();
                    tdaa.Unit = dr.Rows[0]["UNITID"].ToString();
                }
                tdaa.Isvalid = "Y";
                TDataa.Add(tdaa);
            }
            ca.indentSuppAllocates = TDataa;
            return View(ca);
        }
        public List<SelectListItem> BindOldSupp(string id)
        {
            try
            {
                DataTable dt1 = new DataTable();
                dt1 = PurIndent.GetLasttwoSupp(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dt1.Rows[i]["PARTYNAME"].ToString(), Value = dt1.Rows[i]["PARTYID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}