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
        public IActionResult AssetAddDed(string id)
        {
            AssetAddDed ca = new AssetAddDed();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Loc = BindLocation();
            ca.Typelst = Bindtype();
            ca.Entered = Request.Cookies["UserId"];
            ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
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
                    DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["ASADDBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
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
                    DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["ASDEDBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
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
    }
}
