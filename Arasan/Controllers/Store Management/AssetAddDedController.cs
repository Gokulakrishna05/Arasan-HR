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
using Nest;
using Arasan.Services;
using Arasan.Services.Store_Management;

namespace Arasan.Controllers 
{
    public class AssetAddDedController : Controller
    {
        IAssetAddDed Asset;
        IConfiguration? _configuratio;
        private string? _connectionString;
        

        DataTransactions datatrans;
        public AssetAddDedController(IAssetAddDed _Asset, IConfiguration _configuratio )
        {
             
            Asset = _Asset;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult AssetAddDed(string id,string tag)
        {
            AssetAddDed ca = new AssetAddDed();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Loc = BindLocation();
            ca.Typelst = Bindtype();
            ca.Entered = Request.Cookies["UserId"];
            ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.close = tag;
            List<AdDeItem> TData = new List<AdDeItem>();
            AdDeItem tda = new AdDeItem();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new AdDeItem();
                    
                    tda.Itemlst = BindItemlst();
                    
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            ca.Itlst = TData;
            return View(ca);
        }

        [HttpPost]
        public ActionResult AssetAddDed(AssetAddDed Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = Asset.AssetAddDedCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.Type == "DIRECT ADDITION")
                    {
                        if (Cy.ID == null)
                        {
                            TempData["notice"] = "Asset Addition Inserted Successfully...!";
                        }
                        else
                        {
                            TempData["notice"] = "AssetAddDed Updated Successfully...!";
                        }
                        return RedirectToAction("ListAssetAddition");
                    }
                    if (Cy.Type == "DIRECT DEDUCTION")
                    {
                        if (Cy.ID == null)
                        {
                            TempData["notice"] = "Asset Deduction Inserted Successfully...!";
                        }
                        else
                        {
                            TempData["notice"] = "AssetAddDed Updated Successfully...!";
                        }
                        return RedirectToAction("ListAssetDeduction");
                    }
                }

                else
                {
                    ViewBag.PageTitle = "Edit AssetAddDed";
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
        public List<SelectListItem> BindItemlst( )
        {
            try
            {
                DataTable dtDesg = datatrans.GetItem();
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
        public ActionResult GetItemDetail(string ItemId, string loc)
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

                dt = datatrans.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    unit = dt.Rows[0]["UNITID"].ToString();
                    purrate = dt.Rows[0]["LATPURPRICE"].ToString();

                }
               
                    asseststockp = datatrans.GetDataString("Select SUM(QTY) as qty from ASSTOCKVALUE where ITEMID='" + ItemId + "' AND LOCID= '" + loc + "' AND PLUSORMINUS ='p' ");

                    asseststockm = datatrans.GetDataString("Select SUM(QTY) as qty from ASSTOCKVALUE where ITEMID='" + ItemId + "' AND LOCID= '" + loc + "' AND PLUSORMINUS ='m' ");
                    if (asseststockp == "")
                    {
                        asseststockp = "0";
                    }
                    if (asseststockm == "")
                    {
                        asseststockm = "0";
                    }
                    double pstock = Convert.ToDouble(asseststockp);
                    double pmstock = Convert.ToDouble(asseststockm);
                    double Totpmstock = pstock - pmstock;
                    totalstock = Totpmstock.ToString();
                
                if (totalstock == "")
                {
                    totalstock = "0";
                }

                var result = new { unit = unit, purrate = purrate, totalstock = totalstock };
                return Json(result);
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
                lstdesg.Add(new SelectListItem() { Text = "Direct Addition", Value = "DIRECT ADDITION" });
                lstdesg.Add(new SelectListItem() { Text = "Direct Deduction", Value = "DIRECT DEDUCTION" });
                

                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetDocDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string seq = "";
                string sequnce = "";
                string lasto = "";


                if (ItemId == "DIRECT ADDITION")
                {

                    DataTable dtv = datatrans.GetSequence("DSdd");
                    if (dtv.Rows.Count > 0)
                    {
                        seq = dtv.Rows[0]["PREFIX"].ToString() + dtv.Rows[0]["last"].ToString();
                    }
                }
                if (ItemId == "DIRECT DEDUCTION")
                {

                    DataTable dtv = datatrans.GetSequence("Assdd");
                    if (dtv.Rows.Count > 0)
                    {
                        seq = dtv.Rows[0]["PREFIX"].ToString() + dtv.Rows[0]["last"].ToString();
                    }
                }

                var result = new { seq = seq };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult MyListAddItemgrid(string strStatus)
        {
            List<AddGrid> Reg = new List<AddGrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = Asset.GetAllAddition(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                
                string ViewRow = string.Empty;
                
                string DeleteRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                   
                    ViewRow = "<a href=ViewAssadd?id=" + dtUsers.Rows[i]["ASADDBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View Details' width='20' /></a>";
                    DeleteRow = "<a href=DeleteAdd?tag=Del&id=" + dtUsers.Rows[i]["ASADDBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                }
                else
                {

                   
                    ViewRow = "";
                    
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["ASADDBASICID"].ToString() + "><img src='../Images/close_icon.png' alt='Deactivate' /></a>";

                }

                Reg.Add(new AddGrid
                {
                    id = dtUsers.Rows[i]["ASADDBASICID"].ToString(),
                    did = dtUsers.Rows[i]["DOCID"].ToString(),
                    ddate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    loc = dtUsers.Rows[i]["LOCID"].ToString(),
                    reason = dtUsers.Rows[i]["REASON"].ToString(),



                   
                    viewrow = ViewRow,
                    
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }
        public ActionResult MyListDedItemgrid(string strStatus)
        {
            List<DedGrid> Reg = new List<DedGrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = Asset.GetAllDeduction(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                string approve = string.Empty;
 
                string ViewRow = string.Empty;
                 
                string DeleteRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                   

                    ViewRow = "<a href=ViewAssded?id=" + dtUsers.Rows[i]["ASDEDBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View Details' width='20' /></a>";
                    DeleteRow = "<a href=DeleteDed?tag=Del&id=" + dtUsers.Rows[i]["ASDEDBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                }
                else
                {


                    

                    ViewRow = "";
                    
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["ASDEDBASICID"].ToString() + "><img src='../Images/close_icon.png' alt='Deactivate' /></a>";

                }

                Reg.Add(new DedGrid
                {
                    id = dtUsers.Rows[i]["ASDEDBASICID"].ToString(),
                    did = dtUsers.Rows[i]["DOCID"].ToString(),
                    ddate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    loc = dtUsers.Rows[i]["LOCID"].ToString(),
                    reason = dtUsers.Rows[i]["REASON"].ToString(),



                  
                    viewrow = ViewRow,
                    
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }
        public IActionResult ListAssetAddition()
        {
            return View();
        }
        public IActionResult ListAssetDeduction()
        {
            return View();
        }
        public JsonResult GetItemGrpJSON()
        {
            //DirectItem model = new DirectItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItemlst());
        }
        public IActionResult ViewAssadd(string id)
        {

            AssetAddDed ca = new AssetAddDed();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            dt = Asset.ViewAssadd(id);
            if (dt.Rows.Count > 0)
            {
                ca.Location = dt.Rows[0]["LOCID"].ToString();
                ca.bin = dt.Rows[0]["BINYN"].ToString();
                ca.DocId = dt.Rows[0]["DOCID"].ToString();
                ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                ca.Reason = dt.Rows[0]["REASON"].ToString();
                ca.Type = dt.Rows[0]["STOCKTRANSTYPE"].ToString();
                ca.Narr = dt.Rows[0]["NARRATION"].ToString();
                ca.Gro = Convert.ToDouble(dt.Rows[0]["GROSS"].ToString() == "" ? "0" : dt.Rows[0]["GROSS"].ToString());
                ca.Net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());


                ca.ID = id;

                List<AdDeItem> Data = new List<AdDeItem>();
                AdDeItem tda = new AdDeItem();
                //double tot = 0;

                dt2 = Asset.ViewAssaddDet(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new AdDeItem();

                        tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                       


                        //DataTable dt3 = new DataTable();
                        //dt3 = datatrans.GetItemSubGroup(dt2.Rows[i]["ITEMID"].ToString());
                        //if (dt3.Rows.Count > 0)
                        //{
                        //    tda.itemname = dt3.Rows[0]["SUBGROUPCODE"].ToString();
                        //}

                        tda.rate = Convert.ToDouble(dt2.Rows[i]["RATE"].ToString() == "" ? "0" : dt2.Rows[i]["RATE"].ToString());
                        tda.Quantity = Convert.ToDouble(dt2.Rows[i]["QTY"].ToString() == "" ? "0" : dt2.Rows[i]["QTY"].ToString());
                        tda.Amount = Convert.ToDouble(dt2.Rows[i]["AMOUNT"].ToString() == "" ? "0" : dt2.Rows[i]["AMOUNT"].ToString());
                        tda.Unit = dt2.Rows[i]["UNIT"].ToString();
                      

                        Data.Add(tda);
                    }
                }

                ca.Itlst = Data;

            }
            return View(ca);
        }
        public IActionResult ViewAssded(string id)
        {

            AssetAddDed ca = new AssetAddDed();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            dt = Asset.ViewAssded(id);
            if (dt.Rows.Count > 0)
            {
                ca.Location = dt.Rows[0]["LOCID"].ToString();
                ca.bin = dt.Rows[0]["BINYN"].ToString();
                ca.DocId = dt.Rows[0]["DOCID"].ToString();
                ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                ca.Reason = dt.Rows[0]["REASON"].ToString();
                ca.Type = "Direct Deduction";
                ca.Narr = dt.Rows[0]["NARRATION"].ToString();
                ca.Gro = Convert.ToDouble(dt.Rows[0]["GROSS"].ToString() ==""? "0" :dt.Rows[0]["GROSS"].ToString());
                ca.Net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());
                

                
                ca.ID = id;

                List<AdDeItem> Data = new List<AdDeItem>();
                AdDeItem tda = new AdDeItem();
                //double tot = 0;

                dt2 = Asset.ViewAssdedDet(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new AdDeItem();

                        tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                   
                        tda.Stock = dt2.Rows[i]["CLSTK"].ToString();


                        //DataTable dt3 = new DataTable();
                        //dt3 = datatrans.GetItemSubGroup(dt2.Rows[i]["ITEMID"].ToString());
                        //if (dt3.Rows.Count > 0)
                        //{
                        //    tda.itemname = dt3.Rows[0]["SUBGROUPCODE"].ToString();
                        //}

                        tda.rate = Convert.ToDouble(dt2.Rows[i]["RATE"].ToString() == "" ? "0" : dt2.Rows[i]["RATE"].ToString());
                        tda.Quantity = Convert.ToDouble(dt2.Rows[i]["QTY"].ToString() == "" ? "0" : dt2.Rows[i]["QTY"].ToString());
                        tda.Amount = Convert.ToDouble(dt2.Rows[i]["AMOUNT"].ToString() == "" ? "0" : dt2.Rows[i]["AMOUNT"].ToString());
                        tda.Unit = dt2.Rows[i]["UNIT"].ToString();


                        Data.Add(tda);
                    }
                }

                ca.Itlst = Data;

            }
            return View(ca);
        }
        public ActionResult DeleteAdd(string id)
        {
            datatrans = new DataTransactions(_connectionString);
            bool result = datatrans.UpdateStatus("UPDATE ASADDBASIC SET IS_ACTIVE='N'  Where ASADDBASIC.ASADDBASICID='" + id + "'");
            return RedirectToAction("ListAssetAddition");
        }
        public ActionResult DeleteDed(string id)
        {
            datatrans = new DataTransactions(_connectionString);
            bool result = datatrans.UpdateStatus("UPDATE ASDEDBASIC SET IS_ACTIVE='N'  Where ASDEDBASIC.ASDEDBASICID='" + id + "'");
            return RedirectToAction("ListAssetDeduction");
        }
    }
}
