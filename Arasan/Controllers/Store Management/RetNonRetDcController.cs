using Microsoft.AspNetCore.Mvc;
using Arasan.Models;
using Arasan.Interface.Sales;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Collections.Generic;
using System.Xml.Linq;
using AspNetCore.Reporting;
using System.Reflection;
using Arasan.Interface;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using Arasan.Services.Production;


namespace Arasan.Controllers
{
    public class RetNonRetDcController : Controller
    {
        IRetNonRetDc RetNonRetDcService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        DataTransactions datatrans;
        public RetNonRetDcController(IRetNonRetDc _RetNonRetDcService, IConfiguration _configuratio)
        {

            RetNonRetDcService = _RetNonRetDcService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult RetNonRetDc(string id)
        {
            RetNonRetDc ca = new RetNonRetDc();

            ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];
            ca.DDate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.Partylst = BindParty();
            ca.Stocklst = BindStock();
            ca.typelst = Bindtype();
            ca.applst = BindEmp();
            ca.apprlst = BindEmp();
            ca.Loclst = BindLoclst();
            DataTable dtv = datatrans.GetSequence("RNrDC");
            if (dtv.Rows.Count > 0)
            {
                ca.Did = dtv.Rows[0]["PREFIX"].ToString() + dtv.Rows[0]["last"].ToString();
            }

            List<RetNonRetDcItem> TData = new List<RetNonRetDcItem>();
            RetNonRetDcItem tda = new RetNonRetDcItem();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new RetNonRetDcItem();
                    tda.Sublst = BindSublst();
                    tda.Itemlst = BindItemlst("");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                double total = 0;
                dt = RetNonRetDcService.GetReturnable(id);
                if (dt.Rows.Count > 0)
                {

                    ca.Location = dt.Rows[0]["FROMLOCID"].ToString();
                    ca.Did = dt.Rows[0]["DOCID"].ToString();
                    ca.DDate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.DcType = dt.Rows[0]["DELTYPE"].ToString();
                    ca.Through = dt.Rows[0]["THROUGH"].ToString();
                    ca.Party = dt.Rows[0]["PARTYNAME"].ToString();
                    ca.Stock = dt.Rows[0]["STKTYPE"].ToString();
                    ca.Ref = dt.Rows[0]["REFNO"].ToString();
                    ca.RefDate = dt.Rows[0]["REFDATE"].ToString();
                    ca.Delivery = dt.Rows[0]["DELDATE"].ToString();
                    ca.Narration = dt.Rows[0]["NARRATION"].ToString();
                    ca.Approved = dt.Rows[0]["APPBY"].ToString();
                    ca.Approval2 = dt.Rows[0]["APPBY2"].ToString();

                    dt1 = RetNonRetDcService.GetPartyDetails(dt.Rows[0]["PARTYNAME"].ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        ca.Add1 = dt1.Rows[0]["ADD1"].ToString();

                        ca.Add2 = dt1.Rows[0]["ADD2"].ToString();
                        ca.City = dt1.Rows[0]["CITY"].ToString();
                    }
                    ca.ID = id;
                }
                DataTable dt2 = new DataTable();
                dt2 = RetNonRetDcService.GetReturnableItems(id);
                if (dt2.Rows.Count > 0)
                {

                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new RetNonRetDcItem();
                        double toaamt = 0;
                        tda.Sublst = BindSublst();
                        DataTable dt3 = new DataTable();
                        dt3 = datatrans.GetItemSubGroup(dt2.Rows[i]["ITEMID"].ToString());
                        if (dt3.Rows.Count > 0)
                        {
                            tda.subgrp = dt3.Rows[0]["SUBGROUPCODE"].ToString();
                        }
                        DataTable dt4 = new DataTable();
                        dt4 = RetNonRetDcService.GetRetItemDetail(dt2.Rows[i]["ITEMID"].ToString());
                        if (dt3.Rows.Count > 0)
                        {
                            tda.Unit = dt4.Rows[0]["UNITID"].ToString();
                            tda.PurRate = dt4.Rows[0]["LATPURPRICE"].ToString();
                        }
                        tda.Itemlst = BindItemlst(tda.subgrp);
                        tda.item = dt2.Rows[i]["ITEMID"].ToString();
                        tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();
                        
                        //tda.Unit = dt2.Rows[i]["UNIT"].ToString();
                        //tda.Unit = dt2.Rows[i]["UNIT"].ToString();
                        tda.Current = dt2.Rows[i]["CLSTOCK"].ToString();
                        tda.Qty = dt2.Rows[i]["QTY"].ToString();
                        tda.Transaction = dt2.Rows[i]["PURFTRN"].ToString();
                        tda.Rate = dt2.Rows[i]["RATE"].ToString();
                        tda.Amount = dt2.Rows[i]["AMOUNT"].ToString();

                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }
            }
            
            ca.RetLst = TData;
            return View(ca);
        }

        [HttpPost]
        public ActionResult RetNonRetDc(RetNonRetDc Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = RetNonRetDcService.RetNonRetDcCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "RetNonRetDc Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "RetNonRetDc Updated Successfully...!";
                    }
                    return RedirectToAction("ListRetNonRetDc");
                }

                else
                {
                    ViewBag.PageTitle = "Edit RetNonRetDc";
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

        public List<SelectListItem> BindLoclst()
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
        }  public List<SelectListItem> BindEmp()
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
        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dtDesg = RetNonRetDcService.GetBranch();
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

        public List<SelectListItem> BindParty()
        {
            try
            {
                DataTable dtDesg = RetNonRetDcService.GetParty();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTYID"].ToString(), Value = dtDesg.Rows[i]["PARTYMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindStock()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "Stock", Value = "Stock" });
                lstdesg.Add(new SelectListItem() { Text = "Asset", Value = "Asset" });
               
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public List<SelectListItem> Bindtype()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "Returnable DC", Value = "Returnable DC" });
                lstdesg.Add(new SelectListItem() { Text = "Non-Returnable", Value = "Non-Returnable" });
                lstdesg.Add(new SelectListItem() { Text = "Condemn", Value = "Condemn" });
               
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindSublst()
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
        public JsonResult GetItemJSON(string itemid)
        {
            RetNonRetDcItem model = new RetNonRetDcItem();
            model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst(itemid));

        }
      

        public JsonResult GetGrpitemJSON()
        {
            //CreditorDebitNote model = new CreditorDebitNote();
            //model.Grouplst = BindGrouplst(ItemId);
            return Json(BindSublst());

        }
        public IActionResult ListRetNonRetDc()
        {
            return View();
        }

        public ActionResult GetPartyDetails(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string add = "";
                string address = "";
                string city = "";
              

                dt = RetNonRetDcService.GetPartyDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    add = dt.Rows[0]["ADD1"].ToString();

                    address = dt.Rows[0]["ADD2"].ToString();
                    city = dt.Rows[0]["CITY"].ToString();
                   
                }

                var result = new { add = add, address = address , city = city };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetRetItemDetail(string ItemId,string loc,string branch,string type)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable stock = new DataTable();
               

                string unit = "";
                string purrate = "";
                string totalstock = "";
                string asseststockp = "";
                string asseststockm = "";
               
                dt = RetNonRetDcService.GetRetItemDetail(ItemId);

                if (dt.Rows.Count > 0)
                {

                    unit = dt.Rows[0]["UNITID"].ToString();
                    purrate = dt.Rows[0]["LATPURPRICE"].ToString();
                    
                }
                if (type == "Stock")
                {
                     stock = datatrans.GetData("Select SUM(BALANCE_QTY) as qty from INVENTORY_ITEM where ITEM_ID='" + ItemId + "' AND BALANCE_QTY > 0 AND LOCATION_ID= '" + loc + "' AND BRANCH_ID='" + branch + "'  ");
                    totalstock = stock.Rows[0]["qty"].ToString();
                }

                else
                {
                    asseststockp = datatrans.GetDataString("Select SUM(QTY) as qty from ASSTOCKVALUE where ITEMID='" + ItemId + "' AND LOCID= '" + loc + "' AND PLUSORMINUS ='p' ");

                    asseststockm = datatrans.GetDataString("Select SUM(QTY) as qty from ASSTOCKVALUE where ITEMID='" + ItemId + "' AND LOCID= '" + loc + "' AND PLUSORMINUS ='m' ");
                    if(asseststockp=="")
                    {
                        asseststockp = "0";
                    }
                    if (asseststockm == "")
                    {
                        asseststockm = "0";
                    }
                    double pstock =Convert.ToDouble(asseststockp);
                    double pmstock =Convert.ToDouble(asseststockm);
                    double Totpmstock = pstock- pmstock;
                    totalstock = Totpmstock.ToString();
                }
                if (totalstock=="")
                {
                    totalstock = "0";
                }
               
                var result = new { unit = unit, purrate = purrate, totalstock= totalstock };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = RetNonRetDcService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListRetNonRetDc");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListRetNonRetDc");
            }
        }

        public ActionResult Remove(string tag, int id)
        {

            string flag = RetNonRetDcService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListRetNonRetDc");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListRetNonRetDc");
            }
        }

        public IActionResult ViewRetNonRetDc(string id)
        {

            RetNonRetDc ca = new RetNonRetDc();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            dt = RetNonRetDcService.ViewGetReturnable(id);
            if (dt.Rows.Count > 0)
            {
                ca.Location = dt.Rows[0]["FROMLOCID"].ToString();
                ca.Did = dt.Rows[0]["DOCID"].ToString();
                ca.DDate = dt.Rows[0]["DOCDATE"].ToString();
                ca.DcType = dt.Rows[0]["DELTYPE"].ToString();
                ca.Through = dt.Rows[0]["THROUGH"].ToString();
                ca.Party = dt.Rows[0]["PARTYID"].ToString();
                ca.Stock = dt.Rows[0]["STKTYPE"].ToString();
                ca.Ref = dt.Rows[0]["REFNO"].ToString();
                ca.RefDate = dt.Rows[0]["REFDATE"].ToString();
                ca.Delivery = dt.Rows[0]["DELDATE"].ToString();
                ca.Narration = dt.Rows[0]["NARRATION"].ToString();
                ca.Approved = dt.Rows[0]["EMPNAME"].ToString();
                ca.Approval2 = dt.Rows[0]["EMPNAME"].ToString();

                dt1 = RetNonRetDcService.GetPartyDetails(dt.Rows[0]["PARTYID"].ToString());
                if (dt1.Rows.Count > 0)
                {
                    ca.Add1 = dt1.Rows[0]["ADD1"].ToString();

                    ca.Add2 = dt1.Rows[0]["ADD2"].ToString();
                    ca.City = dt1.Rows[0]["CITY"].ToString();
                }
                ca.ID = id;

                List<RetNonRetDcItem> Data = new List<RetNonRetDcItem>();
                RetNonRetDcItem tda = new RetNonRetDcItem();
                //double tot = 0;

                dt2 = RetNonRetDcService.GetReturnableItems(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        DataTable dt3 = new DataTable();
                        dt3 = datatrans.GetItemSubGroup(dt2.Rows[i]["ITEMID"].ToString());
                        if (dt3.Rows.Count > 0)
                        {
                            tda.subgrp = dt3.Rows[0]["SUBGROUPCODE"].ToString();
                        }
                        DataTable dt4 = new DataTable();
                        dt4 = RetNonRetDcService.GetRetItemDetail(dt2.Rows[i]["ITEMID"].ToString());
                        if (dt3.Rows.Count > 0)
                        {
                            tda.Unit = dt4.Rows[0]["UNITID"].ToString();
                            tda.PurRate = dt4.Rows[0]["LATPURPRICE"].ToString();
                        }
                        tda.Itemlst = BindItemlst(tda.subgrp);
                        tda.item = dt2.Rows[i]["ITEMID"].ToString();
                        tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.Current = dt2.Rows[i]["CLSTOCK"].ToString();
                        tda.Qty = dt2.Rows[i]["QTY"].ToString();
                        tda.Transaction = dt2.Rows[i]["PURFTRN"].ToString();
                        tda.Rate = dt2.Rows[i]["RATE"].ToString();
                        tda.Amount = dt2.Rows[i]["AMOUNT"].ToString();

                        Data.Add(tda);
                    }
                }

                ca.RetLst = Data;

            }
            return View(ca);
        }

        public IActionResult ApproveRetNonRetDc(string id)
        {

            RetNonRetDc ca = new RetNonRetDc();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            dt = RetNonRetDcService.ViewGetReturnable(id);
            if (dt.Rows.Count > 0)
            {
                ca.Location = dt.Rows[0]["LOCID"].ToString();
                ca.Locationid = dt.Rows[0]["FROMLOCID"].ToString();
                ca.Did = dt.Rows[0]["DOCID"].ToString();
                ca.DDate = dt.Rows[0]["DOCDATE"].ToString();
                ca.ADDate = DateTime.Now.ToString("dd-MMM-yyyy");
                ca.DcType = dt.Rows[0]["DELTYPE"].ToString();
                ca.Through = dt.Rows[0]["THROUGH"].ToString();
                ca.Party = dt.Rows[0]["PARTYID"].ToString();
                ca.Stock = dt.Rows[0]["STKTYPE"].ToString();
                ca.Ref = dt.Rows[0]["REFNO"].ToString();
                ca.RefDate = dt.Rows[0]["REFDATE"].ToString();
                ca.Delivery = dt.Rows[0]["DELDATE"].ToString();
                ca.Narration = dt.Rows[0]["NARRATION"].ToString();
                ca.Approved = dt.Rows[0]["EMPNAME"].ToString();
                ca.Approval2 = dt.Rows[0]["EMPNAME"].ToString();

                dt1 = RetNonRetDcService.GetPartyDetails(dt.Rows[0]["PARTYID"].ToString());
                if (dt1.Rows.Count > 0)
                {
                    ca.Add1 = dt1.Rows[0]["ADD1"].ToString();

                    ca.Add2 = dt1.Rows[0]["ADD2"].ToString();
                    ca.City = dt1.Rows[0]["CITY"].ToString();
                }
                ca.ID = id;

                List<RetNonRetDcItem> Data = new List<RetNonRetDcItem>();
                RetNonRetDcItem tda = new RetNonRetDcItem();
                //double tot = 0;

                dt2 = RetNonRetDcService.GetReturnableItems(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                       
                        DataTable dt4 = new DataTable();
                        dt4 = RetNonRetDcService.GetRetItemDetail(dt2.Rows[i]["CITEMID"].ToString());
                        if (dt4.Rows.Count > 0)
                        {
                            tda.Unit = dt4.Rows[0]["UNITID"].ToString();
                            tda.PurRate = dt4.Rows[0]["LATPURPRICE"].ToString();
                        }
                       
                        tda.item = dt2.Rows[i]["ITEMID"].ToString();
                        tda.detid = dt2.Rows[i]["RDELDETAILID"].ToString();
                        tda.saveItemId = dt2.Rows[i]["CITEMID"].ToString();
                        tda.Current = dt2.Rows[i]["CLSTOCK"].ToString();
                        tda.Qty = dt2.Rows[i]["QTY"].ToString();
                        tda.Transaction = dt2.Rows[i]["PURFTRN"].ToString();
                        tda.Rate = dt2.Rows[i]["RATE"].ToString();
                        tda.Amount = dt2.Rows[i]["AMOUNT"].ToString();

                        Data.Add(tda);
                    }
                }

                ca.RetLst = Data;

            }
            return View(ca);
        }

        [HttpPost]
        public ActionResult ApproveRetNonRetDc(RetNonRetDc Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = RetNonRetDcService.ApproveRetNonRetDcCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    
                        TempData["notice"] = "RetNonRetDc Approved Successfully...!";
                    
                    return RedirectToAction("ListRetNonRetDc");
                }

                else
                {
                    ViewBag.PageTitle = "Edit RetNonRetDc";
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
        public ActionResult MyListItemgrid(string strStatus)
        {
            List<RetNonRetDcGrid> Reg = new List<RetNonRetDcGrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = RetNonRetDcService.GetAllReturn(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                
                string approve = string.Empty;
                string ViewRow = string.Empty;
                string EditRow = string.Empty;
                string DeleteRow = string.Empty;



                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    if (dtUsers.Rows[i]["STATUS"].ToString() == "Approve")
                    {
                        approve = "";
                        EditRow = "";
                    }
                    else
                    {
                        approve = "<a href=ApproveRetNonRetDc?id=" + dtUsers.Rows[i]["RDELBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/move_quote.png' alt='View Details' width='20' /></a>";
                        EditRow = "<a href=RetNonRetDc?id=" + dtUsers.Rows[i]["RDELBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";

                    }
                    ViewRow = "<a href=ViewRetNonRetDc?id=" + dtUsers.Rows[i]["RDELBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View Details' width='20' /></a>";
                    DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["RDELBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                }
                else
                {

                    approve = "";
                    ViewRow = "";
                    EditRow = "";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["RDELBASICID"].ToString() + "><img src='../Images/close_icon.png' alt='Deactivate' /></a>";

                }

                Reg.Add(new RetNonRetDcGrid
                {
                    id = dtUsers.Rows[i]["RDELBASICID"].ToString(),
                    did = dtUsers.Rows[i]["DOCID"].ToString(),
                    ddate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    dctype = dtUsers.Rows[i]["DELTYPE"].ToString(),
                    party = dtUsers.Rows[i]["PARTYID"].ToString(),


                    approve = approve,
                    viewrow = ViewRow,
                    editrow = EditRow,
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }

    }
}
