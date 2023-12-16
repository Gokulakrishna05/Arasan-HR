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
    public class ContToAssetController : Controller
    {

        IContToAsset Asset;
        IConfiguration? _configuratio;
        private string? _connectionString;
        

        DataTransactions datatrans;
        public ContToAssetController(IContToAsset _Asset, IConfiguration _configuratio )
        {
             
            Asset = _Asset;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult ContToAsset(string id)
        {
            ContToAsset ca = new ContToAsset();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Loc = BindLocation();
           
            ca.Entered = Request.Cookies["UserId"];
            ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("Stoas");
            if (dtv.Rows.Count > 0)
            {
                ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + dtv.Rows[0]["last"].ToString();
            }
            List<ConItem> TData = new List<ConItem>();
            ConItem tda = new ConItem();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new ConItem();

                    tda.Itemlst = BindItemlst();

                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            ca.Itlst = TData;
            return View(ca);
            
        }
        [HttpPost]
        public ActionResult ContToAsset(ContToAsset Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = Asset.ConstoassetCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                   
                        if (Cy.ID == null)
                        {
                            TempData["notice"] = "ContToAsset Addition Inserted Successfully...!";
                        }
                        else
                        {
                            TempData["notice"] = "ContToAsset Updated Successfully...!";
                        }
                        return RedirectToAction("ListContToAsset");
                    
                }

                else
                {
                    ViewBag.PageTitle = "Edit ContToAsset";
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
        public List<SelectListItem> BindItemlst()
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

                stock = datatrans.GetData("Select SUM(BALANCE_QTY) as qty from INVENTORY_ITEM where ITEM_ID='" + ItemId + "' AND BALANCE_QTY > 0 AND LOCATION_ID= '" + loc + "'  ");
                totalstock = stock.Rows[0]["qty"].ToString();
            

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
        public JsonResult GetItemGrpJSON()
        {
            //DirectItem model = new DirectItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItemlst());
        }
        public ActionResult MyListContoassetItemgrid(string strStatus)
        {
            List<ConGrid> Reg = new List<ConGrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = Asset.GetAllConAsset(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {



                string ViewRow = string.Empty;

                string DeleteRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {


                    ViewRow = "<a href=ViewAssadd?id=" + dtUsers.Rows[i]["CONTOASSETBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View Details' width='20' /></a>";
                    DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["CONTOASSETBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                }
                else
                {


                    ViewRow = "";

                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["CONTOASSETBASICID"].ToString() + "><img src='../Images/close_icon.png' alt='Deactivate' /></a>";

                }

                Reg.Add(new ConGrid
                {
                    id = dtUsers.Rows[i]["CONTOASSETBASICID"].ToString(),
                    did = dtUsers.Rows[i]["DOCID"].ToString(),
                    ddate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    loc = dtUsers.Rows[i]["LOCID"].ToString(),
                    toloc = dtUsers.Rows[i]["location"].ToString(),
                    reason = dtUsers.Rows[i]["REASONCODE"].ToString(),




                    viewrow = ViewRow,

                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }
        public IActionResult ListConToAsset()
        {
            return View();
        }
    }
}
