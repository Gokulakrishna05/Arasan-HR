﻿using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Interface.Store_Management;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Store_Management;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Controllers.Store_Management
{
    public class SubContractingDCController : Controller
    {
        ISubContractingDC SubContractingDCService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public SubContractingDCController(ISubContractingDC _SubContractingDCService, IConfiguration _configuratio)
        {
            SubContractingDCService = _SubContractingDCService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult SubContractingDC(string id)
        {
            SubContractingDC st = new SubContractingDC();
            st.Loc = BindLocation();
            st.Brlst = BindBranch();
            st.Suplst = BindSupplier();
            st.assignList = BindEmp();
            st.Branch = Request.Cookies["BranchId"];
            st.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("subdc");
            if (dtv.Rows.Count > 0)
            {
                st.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
            List<SubContractingItem> TData = new List<SubContractingItem>();
            SubContractingItem tda = new SubContractingItem();
            List<ReceiptDetailItem> TData1 = new List<ReceiptDetailItem>();
            ReceiptDetailItem tda1 = new ReceiptDetailItem();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new SubContractingItem();
                    tda.Itemlst = BindItemlst();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda1 = new ReceiptDetailItem();
                    tda1.Itemlist = BindItemlst();
                    tda1.Isvalid1 = "Y";
                    TData1.Add(tda1);
                }
            }
            else
            {
                //st = StoreAccService.GetStoreAccById(id);
                DataTable dt = new DataTable();
                double total = 0;
                dt = SubContractingDCService.GetSubContractingDCDeatils(id);
                if (dt.Rows.Count > 0)
                {
                    st.ID = id;
                    st.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    st.DocId = dt.Rows[0]["DOCID"].ToString();
                    st.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    //st.Suplst = BindSupplier();
                    st.Supplier = dt.Rows[0]["PARTYID"].ToString();
                    st.Add1 = dt.Rows[0]["ADD1"].ToString();
                    st.Add2 = dt.Rows[0]["ADD2"].ToString();
                    st.City = dt.Rows[0]["CITY"].ToString();
                    st.Location = dt.Rows[0]["LOCID"].ToString();
                    st.Through = dt.Rows[0]["THROUGH"].ToString();
                    st.Entered = dt.Rows[0]["ENTEREDBY"].ToString();
                    st.TotalQty = dt.Rows[0]["TOTQTY"].ToString();
                    st.Narration = dt.Rows[0]["NARRATION"].ToString();
                    //ca.Net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());

                }
                DataTable dt2 = new DataTable();
                dt2 = SubContractingDCService.GetEditItemDetails(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new SubContractingItem();

                        tda.Itemlst = BindItemlst();
                        //tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.Unit = dt2.Rows[i]["UNIT"].ToString();
                        tda.Quantity = dt2.Rows[i]["QTY"].ToString();
                        tda.rate = dt2.Rows[i]["RATE"].ToString();
                        tda.Amount = dt2.Rows[i]["AMOUNT"].ToString();
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }
                DataTable dt3 = new DataTable();
                dt3 = SubContractingDCService.GetEditReceiptDetailItem(id);
                if (dt3.Rows.Count > 0)
                {
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        tda1 = new ReceiptDetailItem();

                        tda1.Itemlist = BindItemlst();
                        //tda1.saveItemId = dt3.Rows[i]["ITEMID"].ToString();
                        tda1.ItemId = dt3.Rows[i]["RITEM"].ToString();
                        tda1.Unit = dt3.Rows[i]["RUNIT"].ToString();
                        tda1.Quantity = dt3.Rows[i]["ERQTY"].ToString();
                        tda1.rate = dt3.Rows[i]["ERATE"].ToString();
                        tda1.Amount = dt3.Rows[i]["EAMOUNT"].ToString();
                        tda1.Isvalid1 = "Y";
                        TData1.Add(tda1);
                    }
                }
            }
            st.SCDIlst = TData;
            st.RECDlst = TData1;
            return View(st);
        }
        [HttpPost]
        public ActionResult SubContractingDC(SubContractingDC ss, string id)
        {

            try
            {
                ss.ID = id;
                string Strout = SubContractingDCService.SubContractingDCCRUD(ss);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (ss.ID == null)
                    {
                        TempData["notice"] = " SubContractingDC Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " SubContractingDC Updated Successfully...!";
                    }
                    return RedirectToAction("ListSubContractingDC");
                }

                else
                {
                    ViewBag.PageTitle = "Edit SubContractingDC";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(ss);
        }
        public IActionResult ListSubContractingDC()
        {
            //IEnumerable<DirectAddition> sta = DirectAdditionService.GetAllDirectAddition(st, ed);
            return View();
        }
        public ActionResult MyListSubContractingDCGrid(string strStatus)
        {
            List<ListSubContractingDCItem> Reg = new List<ListSubContractingDCItem>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)SubContractingDCService.GetAllListSubContractingDCItems(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string View = string.Empty;
                string EditRow = string.Empty;
                string DeleteRow = string.Empty;

                View = "<a href=ViewSubContractingDC?id=" + dtUsers.Rows[i]["SUBCONTDCBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View Details' width='20' /></a>";
                EditRow = "<a href=SubContractingDC?id=" + dtUsers.Rows[i]["SUBCONTDCBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                DeleteRow = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["SUBCONTDCBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";

                Reg.Add(new ListSubContractingDCItem
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["SUBCONTDCBASICID"].ToString()),
                    branch = dtUsers.Rows[i]["BRANCHID"].ToString(),
                    docid = dtUsers.Rows[i]["DOCID"].ToString(),
                    docDate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    loc = dtUsers.Rows[i]["LOCID"].ToString(),
                    tot = dtUsers.Rows[i]["TOTQTY"].ToString(),
                    view = View,
                    editrow = EditRow,
                    delrow = DeleteRow,



                });
            }

            return Json(new
            {
                Reg
            });

        }
        public ActionResult DeleteItem(string tag, String id)
        {

            string flag = SubContractingDCService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListSubContractingDC");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListSubContractingDC");
            }
        }
        public List<SelectListItem> BindItemlst()
        {
            try
            {
                DataTable dtDesg = SubContractingDCService.GetItem();
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
        //public List<SelectListItem> BindItemlist(string id)
        //{
        //    try
        //    {
        //        DataTable dtDesg = SubContractingDCService.GetPartyItem(id);
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        for (int i = 0; i < dtDesg.Rows.Count; i++)
        //        {
        //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["WIPITEMID"].ToString(), Value = dtDesg.Rows[i]["WCBASICID"].ToString() });
        //        }
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
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
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPNAME"].ToString(), Value = dtDesg.Rows[i]["EMPNAME"].ToString() });
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
        public ActionResult GetPartyDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string add1 = "";
                string add2 = "";
                string city = "";
                dt = SubContractingDCService.GetPartyDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    add1 = dt.Rows[0]["ADD1"].ToString();
                    add2 = dt.Rows[0]["ADD2"].ToString();
                    city = dt.Rows[0]["CITY"].ToString();
                }

                var result = new { add1 = add1, add2 = add2, city = city };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetItemGrpJSON()
        {
            SubContractingItem model = new SubContractingItem();
            model.Itemlst = BindItemlst();
            return Json(BindItemlst());

        }
        //public JsonResult GetItemJSON(string ItemId)
        //{
        //    ReceiptDetailItem model = new ReceiptDetailItem();
        //    model.Itemlist = BindItemlist(ItemId);
        //    return Json(BindItemlist(ItemId));

        //}
        public ActionResult GetItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string unit = "";
                string cf = "";
                string price = "";
                string lot = "";
                //string binno = "";
                //string binname = "";
                dt = SubContractingDCService.GetItemDetails(ItemId);
                string stock = SubContractingDCService.GetDrumStock(ItemId);
                if (dt.Rows.Count > 0)
                {

                    unit = dt.Rows[0]["UNITID"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();
                    lot = dt.Rows[0]["LOTYN"].ToString();
                    dt1 = SubContractingDCService.GetItemCF(ItemId, dt.Rows[0]["UNITMASTID"].ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        cf = dt1.Rows[0]["CF"].ToString();
                    }
                }

                var result = new { unit = unit, cf = cf, price = price, lot = lot, stock = stock };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult ListSubContractDrumSelection(string itemid)
        {
            List<SubContractDDrumdetails> EnqChkItem = new List<SubContractDDrumdetails>();
            DataTable dtEnq = new DataTable();
            dtEnq = SubContractingDCService.GetSubContractDrumDetails(itemid);
            for (int i = 0; i < dtEnq.Rows.Count; i++)
            {

                EnqChkItem.Add(new SubContractDDrumdetails
                {
                    lotno = dtEnq.Rows[i]["LOTNO"].ToString(),
                    drumno = dtEnq.Rows[i]["DRUM"].ToString(),
                    qty = dtEnq.Rows[i]["QTY"].ToString(),
                    rate = dtEnq.Rows[i]["RATE"].ToString(),
                    //invid = dtEnq.Rows[i]["PLotmastID"].ToString()
                });
            }

            return Json(new
            {
                EnqChkItem
            });
        }
        public ActionResult SubContractDrumSelection(string itemid, string rowid)
        {
            SubContractDDDrumdetailstable ca = new SubContractDDDrumdetailstable();
            List<SubContractDDrumdetails> TData = new List<SubContractDDrumdetails>();
            SubContractDDrumdetails tda = new SubContractDDrumdetails();
            DataTable dtEnq = new DataTable();
            dtEnq = SubContractingDCService.GetSubContractDrumDetails(itemid);
            for (int i = 0; i < dtEnq.Rows.Count; i++)
            {
                tda = new SubContractDDrumdetails();
                tda.lotno = dtEnq.Rows[i]["LOTNO"].ToString();
                tda.drumno = dtEnq.Rows[i]["DRUM"].ToString();
                tda.qty = dtEnq.Rows[i]["QTY"].ToString();
                tda.rate = dtEnq.Rows[i]["RATE"].ToString();
                //tda.invid = dtEnq.Rows[i]["PLotmastID"].ToString();
                TData.Add(tda);
            }
            ca.SUBDDrumlst = TData;
            return View(ca);
        }
        public IActionResult ViewSubContractingDC(string id)
        {
            SubContractingDC st = new SubContractingDC();
            DataTable dt = new DataTable();
            dt = SubContractingDCService.GetSubViewDeatils(id);
            if (dt.Rows.Count > 0)
            {
                st.ID = id;
                st.Branch = dt.Rows[0]["BRANCHID"].ToString();
                st.DocId = dt.Rows[0]["DOCID"].ToString();
                st.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                //st.Suplst = BindSupplier();
                st.Supplier = dt.Rows[0]["PARTYNAME"].ToString();
                st.Add1 = dt.Rows[0]["ADD1"].ToString();
                st.Add2 = dt.Rows[0]["ADD2"].ToString();
                st.City = dt.Rows[0]["CITY"].ToString();
                st.Location = dt.Rows[0]["LOCID"].ToString();
                st.Through = dt.Rows[0]["THROUGH"].ToString();
                st.Entered = dt.Rows[0]["ENTEREDBY"].ToString();
                st.TotalQty = dt.Rows[0]["TOTQTY"].ToString();
                st.Narration = dt.Rows[0]["NARRATION"].ToString();
                //ca.Net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());

            }
            List<SubContractingItem> TData = new List<SubContractingItem>();
            SubContractingItem tda = new SubContractingItem();
            DataTable dt2 = new DataTable();
            dt2 = SubContractingDCService.GetSubContractViewDetails(id);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new SubContractingItem();

                    tda.Itemlst = BindItemlst();
                    //tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();
                    tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                    tda.Unit = dt2.Rows[i]["UNIT"].ToString();
                    tda.Quantity = dt2.Rows[i]["QTY"].ToString();
                    tda.rate = dt2.Rows[i]["RATE"].ToString();
                    tda.Amount = dt2.Rows[i]["AMOUNT"].ToString();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            List<ReceiptDetailItem> TData1 = new List<ReceiptDetailItem>();
            ReceiptDetailItem tda1 = new ReceiptDetailItem();
            DataTable dt3 = new DataTable();
            dt3 = SubContractingDCService.GetReceiptViewDetail(id);
            if (dt3.Rows.Count > 0)
            {
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    tda1 = new ReceiptDetailItem();

                    tda1.Itemlist = BindItemlst();
                    //tda1.saveItemId = dt3.Rows[i]["ITEMID"].ToString();
                    tda1.ItemId = dt3.Rows[i]["ITEMID"].ToString();
                    tda1.Unit = dt3.Rows[i]["RUNIT"].ToString();
                    tda1.Quantity = dt3.Rows[i]["ERQTY"].ToString();
                    tda1.rate = dt3.Rows[i]["ERATE"].ToString();
                    tda1.Amount = dt3.Rows[i]["EAMOUNT"].ToString();
                    tda1.Isvalid1 = "Y";
                    TData1.Add(tda1);
                }
            }


            st.SCDIlst = TData;
            st.RECDlst = TData1;
            return View(st);
        }
    }
}