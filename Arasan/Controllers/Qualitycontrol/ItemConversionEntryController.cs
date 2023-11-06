using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Interface.Qualitycontrol;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Production;
using Arasan.Services.Qualitycontrol;
using Arasan.Services.Sales;
using Arasan.Services.Store_Management;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers.Qualitycontrol
{
    public class ItemConversionEntryController : Controller
    {
        IItemConversionEntryService ItemConversionEntryService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public ItemConversionEntryController(IItemConversionEntryService _ItemConversionEntryService, IConfiguration _configuratio)
        {
            ItemConversionEntryService = _ItemConversionEntryService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult ItemConversionEntry(string id)
        {
            ItemConversionEntry ca = new ItemConversionEntry();
            ca.Loc = BindLocation();
            ca.Itemlst = BindItemlst("");
            ca.TItemlst = BindItemlst();
            ca.Entlst = BindEmp();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Approvelst = BindEmp();
            //DataTable dtv = datatrans.GetSequence("Qinin");
            ca.DocDate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("ICE");
            if (dtv.Rows.Count > 0)
            {
                ca.Docid = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
            List<ConEntryItem> TData = new List<ConEntryItem>();
            ConEntryItem tda = new ConEntryItem();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new ConEntryItem();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                //ca = QCResultService.GetQCResultById(id);

                //DataTable dt = new DataTable();
                //dt = ItemConversionEntryService.GetEditDeatils(id);
                //if (dt.Rows.Count > 0)
                //{
                //    ca.Docid = dt.Rows[0]["DOCID"].ToString();
                //    ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                //    ca.Location = dt.Rows[0]["FROMLOC"].ToString();
                //    ca.Itemlst = BindItemlst(ca.Location);
                //    ca.Item = dt.Rows[0]["ITEMID"].ToString();
                //    ca.ToItem = dt.Rows[0]["TITEMID"].ToString();
                //    ca.Purpose = dt.Rows[0]["CPURPOSE"].ToString();
                //    ca.Unit = dt.Rows[0]["FUNIT"].ToString();
                //    ca.Total = dt.Rows[0]["TOTQTY"].ToString();
                //    ca.Entered = dt.Rows[0]["ENTEREDBY"].ToString();
                //    ca.Approved = dt.Rows[0]["APPROVEDBY"].ToString();
                //    ca.Remarks = dt.Rows[0]["REMARKS"].ToString();
                //    ca.ID = id;
                //}
                //DataTable dt2 = new DataTable();
                //dt2 = ItemConversionEntryService.GetEditItemDetails(id);
                //if (dt2.Rows.Count > 0)
                //{

                //    for (int i = 0; i < dt2.Rows.Count; i++)
                //    {
                //        tda = new ConEntryItem();
                //        ViewBag.Item = dt.Rows[0]["ITEMID"].ToString();
                //        tda.drum = dt2.Rows[0]["DRUMNO"].ToString();
                //        tda.batchno = dt2.Rows[i]["BATCHNO"].ToString();
                //        tda.batchno = dt2.Rows[0]["FBINID"].ToString();
                //        tda.qty = dt2.Rows[0]["QTY"].ToString();
                //        tda.Isvalid = "Y";
                //        TData.Add(tda);
                //    }

                //}

            }
            ca.ICEILst = TData;
            return View(ca);
        }
        [HttpPost]
        public ActionResult ItemConversionEntry(ItemConversionEntry Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = ItemConversionEntryService.ItemConversionEntryCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "ItemConversionEntry Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "ItemConversionEntry Updated Successfully...!";
                    }
                    return RedirectToAction("ListItemConversionEntry");

                }
                else
                {
                    ViewBag.PageTitle = "Edit ItemConversionEntry";
                    TempData["notice"] = Strout;
                    //return View();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Cy);
        }
        public IActionResult ListItemConversionEntry(string st, string ed)
        {

            IEnumerable<ItemConversionEntry> cmp = ItemConversionEntryService.GetAllItemConversionEntry(st,ed);
            return View(cmp);
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
        public List<SelectListItem> BindItemlst(string value)
        {
            try
            {
                DataTable dtDesg = ItemConversionEntryService.GetItem(value);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["Item"].ToString() });
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
                DataTable dtDesg = ItemConversionEntryService.GetItem();
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
            ItemConversionEntry model = new ItemConversionEntry();
            model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst(itemid));

        }
        public ActionResult GetStockDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                string stock = "";
                string unit = "";


                dt = ItemConversionEntryService.GetStockDetails(ItemId);
                if (dt.Rows.Count > 0)
                {
                    stock = dt.Rows[0]["BALANCE_QTY"].ToString();

                }
                dt1 = ItemConversionEntryService.GetUnitDeatils(ItemId);
                if (dt1.Rows.Count > 0)
                {
                    unit = dt1.Rows[0]["UNITID"].ToString();

                }

                var result = new { stock = stock , unit = unit };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetDrumStockDetails(string id, string item)
        {
            ItemConversionEntry model = new ItemConversionEntry();
            DataTable dtt = new DataTable();
            List<ConEntryItem> Data = new List<ConEntryItem>();
            ConEntryItem tda = new ConEntryItem();
            dtt = ItemConversionEntryService.GetDrumStockDetail(id,item);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new ConEntryItem();
                    tda.drum = dtt.Rows[i]["DRUM_NO"].ToString();
                    tda.qty = dtt.Rows[i]["BALANCE_QTY"].ToString();
                    tda.sid = dtt.Rows[i]["DRUM_STOCK_ID"].ToString();
                    DataTable dtt2 = new DataTable();
                    dtt2 = ItemConversionEntryService.GetBatchDetails(tda.sid);
                    tda.batchno = dtt2.Rows[i]["LOTNO"].ToString();
                    DataTable dtt3 = new DataTable();
                    dtt3 = ItemConversionEntryService.GetBinDetails(id);
                    tda.binid = dtt3.Rows[i]["BINID"].ToString();
                    //tda.binid = "0";
                    tda.rate = dtt3.Rows[i]["LATPURPRICE"].ToString();
                    tda.total = "0";
                    Data.Add(tda);
                }
            }
            model.ICEILst = Data;
            return Json(model.ICEILst);

        }
    }
}
