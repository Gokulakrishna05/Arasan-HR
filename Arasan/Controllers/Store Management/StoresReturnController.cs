using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Interface.Stores_Management;
using Arasan.Models;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers.Stores_Management
{
    public class StoresReturnController : Controller
    {
        IStoresReturnService StoresReturnService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public StoresReturnController(IStoresReturnService _StoresReturnService, IConfiguration _configuratio)
        {
            StoresReturnService = _StoresReturnService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult StoresReturn(string id)
        {
            StoresReturn ca = new StoresReturn();
            ca.Brlst = BindBranch();
            ca.Loc = BindLocation();
            DataTable dtv = datatrans.GetSequence("sRet");
            if (dtv.Rows.Count > 0)
            {
                ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
            ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.Branch = Request.Cookies["BranchId"];
            List<StoreItem> TData = new List<StoreItem>();
            StoreItem tda = new StoreItem();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new StoreItem();
                   
                    tda.Itemlst = BindItemlst("");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                //ca = LocationService.GetLocationsById(id);


                DataTable dt = new DataTable();
                double total = 0;
                dt = StoresReturnService.GetStoresReturn(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    ca.RefNo = dt.Rows[0]["REFNO"].ToString();
                    ca.RefDate = dt.Rows[0]["REFDATE"].ToString();
                    //ca.ID = id;
                    //ca.Currency = dt.Rows[0]["MAINCURRENCY"].ToString();
                    //ca.RefDate = dt.Rows[0]["REFDT"].ToString();
                    //ca.Voucher = dt.Rows[0]["VOUCHER"].ToString();
                    ca.Location = dt.Rows[0]["LOCID"].ToString();
                    //ca.Narration = dt.Rows[0]["NARR"].ToString();

                }
                DataTable dt2 = new DataTable();
                dt2 = StoresReturnService.GetSRItemDetails(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new StoreItem();
                        double toaamt = 0;
                         
                        tda.Itemlst = BindItemlst(ca.Location);
                        tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();

                        DataTable dt4 = new DataTable();
                        dt4 = datatrans.GetItemDetails(tda.ItemId);
                        if (dt4.Rows.Count > 0)
                        {
                            
                            tda.ConFac = dt4.Rows[0]["CF"].ToString();
                            tda.rate = Convert.ToDouble(dt4.Rows[0]["LATPURPRICE"].ToString());
                        }
                        tda.Quantity = Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                        toaamt = tda.rate * tda.Quantity;
                        total += toaamt;
                        //tda.QtyPrim= Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                        tda.Amount = toaamt;
                        tda.Unit = dt2.Rows[i]["UNITID"].ToString();

                        //tda.FromBin = Convert.ToDouble(dt2.Rows[i]["FROMBINID"].ToString() == "" ? "0" : dt2.Rows[i]["FROMBINID"].ToString());
                        //tda.ToBin = Convert.ToDouble(dt2.Rows[i]["TOBINID"].ToString() == "" ? "0" : dt2.Rows[i]["TOBINID"].ToString());

                        //tda.BinID = Convert.ToDouble(dt2.Rows[i]["BINID"].ToString() == "" ? "0" : dt2.Rows[i]["BINID"].ToString());
                        //tda.Process = Convert.ToDouble(dt2.Rows[i]["PROCESSID"].ToString() == "" ? "0" : dt2.Rows[i]["PROCESSID"].ToString());


                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }
            }
            ca.StrLst = TData;
            return View(ca);
        }
        public IActionResult StoresReturnDetails(string id)
        {
            IEnumerable<StoreItem> cmp = StoresReturnService.GetAllStoresReturnItem(id);
            return View(cmp);
        }
        [HttpPost]
        public ActionResult StoresReturn(StoresReturn Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = StoresReturnService.StoresReturnCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "StoresReturn Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "StoresReturn Updated Successfully...!";
                    }
                    return RedirectToAction("ListStoresReturn");
                }

                else
                {
                    ViewBag.PageTitle = "Edit StoresReturn";
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
        public IActionResult ListStoresReturn(string st, string ed)
        {
            IEnumerable<StoresReturn> cmp = StoresReturnService.GetAllStoresReturn(st,ed);
            return View(cmp);
        }
        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dtDesg = StoresReturnService.GetBranch();
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
        public List<SelectListItem> BindLocation()
        {
            try
            {
                DataTable dtDesg = StoresReturnService.GetLocation();
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
        public List<SelectListItem> BindItemlst(string value)
        {
            try
            {
                DataTable dtDesg = StoresReturnService.GetItem(value);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["ITEM_ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public ActionResult GetItemDetail(string ItemId,string loc,string branch)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();

                string unit = "";
                string unitid = "";
                string CF = "";
                string price = "";
                string stk = "";
                string lot = "";
                dt = datatrans.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    unit = dt.Rows[0]["UNITID"].ToString();
                    unitid = dt.Rows[0]["UNITMASTID"].ToString();

                    price = dt.Rows[0]["LATPURPRICE"].ToString();
                    dt1 = StoresReturnService.GetItemCF(ItemId, dt.Rows[0]["UNITMASTID"].ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        CF = dt1.Rows[0]["CF"].ToString();
                    }
                }
                dt2 = StoresReturnService.Getstkqty(ItemId, loc, branch);
                if (dt2.Rows.Count > 0)
                {
                    stk = dt2.Rows[0]["QTY"].ToString();
                    lot = dt2.Rows[0]["LOT_NO"].ToString();
                }
                if (stk == "")
                {
                    stk = "0";
                }

                var result = new { unit = unit, CF = CF, price = price, stk= stk , lot = lot, unitid= unitid };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetItemJSON(string itemid)
        {
            StoreItem model = new StoreItem();
            model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst(itemid));

        }
        public JsonResult GetItemGrpJSON(string itemid)
        {
            //StoreItem model = new StoreItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItemlst(itemid));
        }

        public ActionResult GetLocDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string narr = "";
                string narr1 = "";
                dt = StoresReturnService.Getloc(ItemId);
                if (dt.Rows.Count > 0)
                {
                    narr = dt.Rows[0]["LOCID"].ToString();
                }
                narr1 = "Returned From " + narr;

                var result = new {  narr1 = narr1 };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}






