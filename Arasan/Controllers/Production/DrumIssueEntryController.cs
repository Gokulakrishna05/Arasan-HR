using System.Collections.Generic;
using Arasan.Interface.Production;
using Arasan.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Services.Production;
using System.Collections.Specialized;
using Arasan.Interface;
using Arasan.Services;

namespace Arasan.Controllers.Production
{
    public class DrumIssueEntryController : Controller
    {
        IDrumIssueEntryService DrumIssueEntryService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public DrumIssueEntryController(IDrumIssueEntryService _DrumIssueEntryService, IConfiguration _configuratio)
        {
            DrumIssueEntryService = _DrumIssueEntryService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult DrumIssueEntry(string id)
        {
            DrumIssueEntry ca = new DrumIssueEntry();
            ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Bin = BindBinID();
            ca.ToBin = BindBinID();
            ca.Loc = BindLocation();
            ca.ToLoc = BindLoc();
            ca.Type = BindType();
            ca.RecList = BindEmp();
            ca.Entered = Request.Cookies["UserId"];
            ca.Applst = BindEmp();
            ca.Itemlst = BindItemlst("");
            ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("DIE");
            if (dtv.Rows.Count > 0)
            {
                ca.Docid = dtv.Rows[0]["PREFIX"].ToString() + "" + dtv.Rows[0]["last"].ToString();
            }
            List<DrumIssueEntryItem> TData = new List<DrumIssueEntryItem>();
            DrumIssueEntryItem tda = new DrumIssueEntryItem();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new DrumIssueEntryItem();
                    tda.FBinlst = BindBinID();
                    tda.TBinlst = BindBinID();
                    //tda.drumlst = Binddrum("","");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                //st = DrumIssueEntryService.Get DrumIssueEntryById(id);

                DataTable dt = new DataTable();
                dt = DrumIssueEntryService.GetDrumIssuseDetails(id);
                if (dt.Rows.Count > 0)
                {
                    ca.ID = id;
                    ca.Docid = dt.Rows[0]["DOCID"].ToString();
                    ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.Branch = dt.Rows[0]["BRANCH"].ToString();
                    ca.Itemid = dt.Rows[0]["ITEMID"].ToString();
                    ca.FromLoc = dt.Rows[0]["FROMLOC"].ToString();
                    ca.Toloc = dt.Rows[0]["TOLOC"].ToString();
                    ca.Frombin = dt.Rows[0]["FROMBINID"].ToString();
                    ca.Tobin = dt.Rows[0]["TOBINID"].ToString();
                    ca.Unit = dt.Rows[0]["UNIT"].ToString();
                    ca.Stock = dt.Rows[0]["LOTSTOCK"].ToString();
                    ca.type = dt.Rows[0]["TYPE"].ToString();
                    ca.Drum = dt.Rows[0]["STOCK"].ToString();
                    ca.Entered = dt.Rows[0]["ENTEREDBY"].ToString();
                    ca.Approved = dt.Rows[0]["APPROVEDBY"].ToString();
                    ca.Qty = Convert.ToDouble(dt.Rows[0]["TOTQTY"].ToString() == "" ? "0" : dt.Rows[0]["TOTQTY"].ToString());
                    ca.Purpose = dt.Rows[0]["REMARKS"].ToString();
                    ca.IRate = Convert.ToDouble(dt.Rows[0]["ISSRATE"].ToString() == "" ? "0" : dt.Rows[0]["ISSRATE"].ToString());
                    ca.IValue = Convert.ToDouble(dt.Rows[0]["ISSVALUE"].ToString() == "" ? "0" : dt.Rows[0]["ISSVALUE"].ToString());

                }
                //DataTable dt2 = new DataTable();

                //dt2 = DrumIssueEntryService.GetDIEDetail(id);
                //if (dt2.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dt2.Rows.Count; i++)
                //    {
                //        tda = new DrumIssueEntryItem();

                //       // tda.FBinlst = BindBinID();
                //        tda.FBinId = dt2.Rows[i]["FBINID"].ToString();

                //      //  tda.TBinlst = BindBinID();
                //        tda.TBinid = dt2.Rows[i]["TBINID"].ToString();

                //        //tda.drumlst = Binddrum();
                //        tda.drumlst = Binddrum(ca.FromLoc,ca.Itemid);
                //        tda.Drum = dt2.Rows[i]["NDRUMNO"].ToString();
                //        tda.Isvalid = "Y";
                //        tda.Qty = dt2.Rows[i]["QTY"].ToString();
                //        tda.Batch = dt2.Rows[i]["BATCHNO"].ToString();
                //        tda.Rate = Convert.ToDouble(dt2.Rows[0]["BATCHRATE"].ToString() == "" ? "0" : dt2.Rows[0]["BATCHRATE"].ToString());
                //        tda.Amount = Convert.ToDouble(dt2.Rows[0]["AMOUNT"].ToString() == "" ? "0" : dt2.Rows[0]["AMOUNT"].ToString());
                //        tda.ID = id;
                //        TData.Add(tda);
                //    }

                //}

            }
            ca.Drumlst = TData;
            return View(ca);
        }
        [HttpPost]
        public ActionResult DrumIssueEntry(DrumIssueEntry Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = DrumIssueEntryService.DrumIssueEntryCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "  DrumIssueEntry Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "  DrumIssueEntry Updated Successfully...!";
                    }
                    return RedirectToAction("ListDrumIssueEntry");
                }

                else
                {
                    ViewBag.PageTitle = "Edit  DrumIssueEntry";
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
        public IActionResult ListDrumIssueEntry()
        {
            //IEnumerable<DrumIssueEntry> cmp = DrumIssueEntryService.GetAllDrumIssueEntry(st, ed);
            return View();
        }
        public ActionResult MyListDrumIssueGrid(string strStatus)
        {
            List<DrumIssueitem> Reg = new List<DrumIssueitem>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)DrumIssueEntryService.GetAllDrumIssueItems(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string View = string.Empty;
                
                string DeleteRow = string.Empty;

                View = "<a href=ApproveDrumIssue?id=" + dtUsers.Rows[i]["DIEBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View Details' width='20' /></a>";
                
                DeleteRow = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["DIEBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                Reg.Add(new DrumIssueitem
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["DIEBASICID"].ToString()),
                    docno = dtUsers.Rows[i]["DOCID"].ToString(),
                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    fromloc = dtUsers.Rows[i]["LOCID"].ToString(),
                    toloc = dtUsers.Rows[i]["toloc"].ToString(),
                    item = dtUsers.Rows[i]["ITEMID"].ToString(),
                    
                    view = View,
                   
                    delrow = DeleteRow,
                });
            }

            return Json(new
            {
                Reg
            });

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
        public ActionResult GetItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string unit = "";

                dt = datatrans.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    unit = dt.Rows[0]["UNITID"].ToString();

                }

                var result = new { unit = unit };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public JsonResult GetDrumJSON(string id, string item)
        //{
        //    DrumIssueEntryItem model = new DrumIssueEntryItem();
        //    model.drumlst = Binddrum(id, item);
        //    return Json(Binddrum(id, item));
        //}
        //public List<SelectListItem> Binddrum(string value,string item)
        //{
        //    try
        //    {
        //        DataTable dtDesg = DrumIssueEntryService.DrumDeatils(value, item);
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        for (int i = 0; i < dtDesg.Rows.Count; i++)
        //        {
        //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DRUM_NO"].ToString(), Value = dtDesg.Rows[i]["DRUM_ID"].ToString() });
        //        }
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public JsonResult GetItemJSON(string ItemId)
        {
            //DrumIssueEntry model = new DrumIssueEntry();
            //model.Itemlst = BindItemlst(ItemId);
            return Json(BindItemlst(ItemId));

        }
        //public JsonResult GetItemGrpJSON(string id, string item)
        //{
        //    DrumIssueEntryItem model = new DrumIssueEntryItem();
        //      model.drumlst = Binddrum(id, item);
        //    return Json(Binddrum(id , item));
        //}
        public List<SelectListItem> BindType()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "FOR INTERNAL", Value = "FOR INTERNAL" });
                lstdesg.Add(new SelectListItem() { Text = "FOR UNBACKING", Value = "FOR UNBACKING" });


                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindItemlst(string id)
        {
            try
            {
                DataTable dtDesg = DrumIssueEntryService.GetItem(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["item"].ToString() });
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
                DataTable dtDesg = DrumIssueEntryService.GetBranch();
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
        public List<SelectListItem> BindBinID()
        {
            try
            {
                DataTable dtDesg = DrumIssueEntryService.BindBinID();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["BINID"].ToString(), Value = dtDesg.Rows[i]["BINBASICID"].ToString() });
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
                DataTable dtDesg = DrumIssueEntryService.GetLocation();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOCID"].ToString(), Value = dtDesg.Rows[i]["loc"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindLoc()
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
        //public ActionResult GetDrumDetail(string ItemId)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();

        //        string batch = "";
        //        string qty = "";

        //        dt = DrumIssueEntryService.GetDrumDetails(ItemId);

        //        if (dt.Rows.Count > 0)
        //        {

        //            batch = dt.Rows[0]["BATCHNO"].ToString();
        //            qty = dt.Rows[0]["QTY"].ToString();



        //        }

        //        var result = new { batch = batch , qty= qty };
        //        return Json(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public ActionResult GetStockDetail(string ItemId ,string items)
        {
            try
            {
                DataTable dt = new DataTable();

                string drum = "";
                string stock = "";

                dt = DrumIssueEntryService.GetStockDetails(ItemId, items);

                if (dt.Rows.Count > 0)
                {

                    drum = dt.Rows[0]["SUM_QTY"].ToString();
                    stock = dt.Rows[0]["SUM_QTY"].ToString();



                }

                var result = new { drum = drum, stock = stock };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult DrumSelection(string itemid,string locid)
        {
            DrumIssueEntry model = new DrumIssueEntry();
            DataTable dtt = new DataTable();
            List<DrumIssueEntryItem> Data = new List<DrumIssueEntryItem>();
            DrumIssueEntryItem tda = new DrumIssueEntryItem();
            dtt = DrumIssueEntryService.GetDrumStockDetail(itemid, locid);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new DrumIssueEntryItem();
                   tda.drum = dtt.Rows[i]["DRUMNO"].ToString();

                    tda.qty = dtt.Rows[i]["QTY"].ToString();
                    tda.batchno = dtt.Rows[i]["LOTNO"].ToString();

                    Data.Add(tda);
                }
            }
            model.Drumlst = Data;
            return View(model);
        }
        //public ActionResult GetItemJSON(string ItemId)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        DataTable dt1 = new DataTable();
        //        string item = "";
        //        string unit = "";

        //        dt = DrumIssueEntryService.GetItemDetails(ItemId);

        //        if (dt.Rows.Count > 0)
        //        {

        //            item = dt.Rows[0]["ITEMID"].ToString();
        //            dt1 = datatrans.GetItemDetails(item);
        //            unit = dt1.Rows[0]["QTY"].ToString();



        //        }

        //        var result = new { item = item, qty = unit };
        //        return Json(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public IActionResult ApproveDrumIssue(string id)
        {
            DrumIssueEntry ca = new DrumIssueEntry();
            DataTable dt = DrumIssueEntryService.EditDrumIssue(id);
            if (dt.Rows.Count > 0)
            {
                ca.Docid = dt.Rows[0]["DOCID"].ToString();
                ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.Itemid = dt.Rows[0]["ITEMID"].ToString();
                ca.FromLoc = dt.Rows[0]["LOCID"].ToString();
                ca.Toloc = dt.Rows[0]["loc"].ToString();
                ca.Frombin = dt.Rows[0]["FROMBINID"].ToString();
                ca.Tobin = dt.Rows[0]["TOBINID"].ToString();
                ca.Unit = dt.Rows[0]["UNIT"].ToString();
                ca.Stock = dt.Rows[0]["LOTSTOCK"].ToString();
                ca.type = dt.Rows[0]["TYPE"].ToString();
                ca.Drum = dt.Rows[0]["STOCK"].ToString();
                ca.Entered = dt.Rows[0]["ENTEREDBY"].ToString();
                ca.Approved = dt.Rows[0]["APPROVEDBY"].ToString();
                ca.Qty = Convert.ToDouble(dt.Rows[0]["TOTQTY"].ToString() == "" ? "0" : dt.Rows[0]["TOTQTY"].ToString());
                ca.IRate = Convert.ToDouble(dt.Rows[0]["ISSRATE"].ToString() == "" ? "0" : dt.Rows[0]["ISSRATE"].ToString());
                ca.IValue = Convert.ToDouble(dt.Rows[0]["ISSVALUE"].ToString() == "" ? "0" : dt.Rows[0]["ISSVALUE"].ToString());
                //ca.Purpose = dt.Rows[0]["REMARKS"].ToString();
                //ViewBag.entrytype = ca.EntryType;
                List<DrumIssueEntryItem> TData = new List<DrumIssueEntryItem>();
                DrumIssueEntryItem tda = new DrumIssueEntryItem();
                DataTable dtDrum = DrumIssueEntryService.EditDrumDetail(id);
                for (int i = 0; i < dtDrum.Rows.Count; i++)
                {
                    tda = new DrumIssueEntryItem();
                    tda.FBinId = dtDrum.Rows[i]["FBINID"].ToString();
                    tda.TBinid = dtDrum.Rows[i]["TBINID"].ToString();
                    tda.drum = dtDrum.Rows[i]["DRUMNO"].ToString();
                    tda.qty = dtDrum.Rows[i]["QTY"].ToString();
                    tda.batch = dtDrum.Rows[i]["BATCHNO"].ToString();
                    tda.Rate = Convert.ToDouble(dtDrum.Rows[0]["BATCHRATE"].ToString() == "" ? "0" : dtDrum.Rows[0]["BATCHRATE"].ToString());
                    tda.Amount = Convert.ToDouble(dtDrum.Rows[0]["AMOUNT"].ToString() == "" ? "0" : dtDrum.Rows[0]["AMOUNT"].ToString());
                    TData.Add(tda);
                }


                ca.Drumlst = TData;
            }
            return View(ca);
        }
        public ActionResult GetDrumStockDetails(string id,string item)
        {
            DrumIssueEntry model = new DrumIssueEntry();
            DataTable dtt = new DataTable();
            List<DrumIssueEntryItem> Data = new List<DrumIssueEntryItem>();
            DrumIssueEntryItem tda = new DrumIssueEntryItem();
            dtt = DrumIssueEntryService.GetDrumStockDetail(id, item);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new DrumIssueEntryItem();

                    tda.drum = dtt.Rows[i]["DRUMNO"].ToString();
                  
                    tda.qty = dtt.Rows[i]["QTY"].ToString();
                    tda.batchno = dtt.Rows[i]["LOTNO"].ToString();
                   

                    Data.Add(tda);
                }
            }
            model.Drumlst = Data;
            return Json(model.Drumlst);

        }

        //public IActionResult ApprovePyroProduction(string id)
        //{
        //    DrumIssueEntry ca = new DrumIssueEntry();
        //    DataTable dt = DrumIssueEntryService.EditDrumIssue(id);
        //    if (dt.Rows.Count > 0)
        //    {
        //        ca.Docid = dt.Rows[0]["DOCID"].ToString();
        //        ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
        //        ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
        //        ca.Itemid = dt.Rows[0]["ITEMID"].ToString();
        //        ca.FromLoc = dt.Rows[0]["LOCID"].ToString();
        //        ca.Toloc = dt.Rows[0]["loc"].ToString();
        //        //ca.Frombin = dt.Rows[0]["FROMBINID"].ToString();
        //        //ca.Tobin = dt.Rows[0]["TOBINID"].ToString();
        //        ca.Unit = dt.Rows[0]["UNIT"].ToString();
        //        ca.Stock = dt.Rows[0]["LOTSTOCK"].ToString();
        //        ca.type = dt.Rows[0]["TYPE"].ToString();
        //        ca.Drum = dt.Rows[0]["STOCK"].ToString();
        //        ca.Entered = dt.Rows[0]["ENTEREDBY"].ToString();
        //        ca.Approved = dt.Rows[0]["APPROVEDBY"].ToString();
        //        ca.Qty = Convert.ToDouble(dt.Rows[0]["TOTQTY"].ToString() == "" ? "0" : dt.Rows[0]["TOTQTY"].ToString());
               
        //        //ca.Purpose = dt.Rows[0]["REMARKS"].ToString();
        //        //ViewBag.entrytype = ca.EntryType;
        //        List<DrumIssueEntryItem> TData = new List<DrumIssueEntryItem>();
        //        DrumIssueEntryItem tda = new DrumIssueEntryItem();
        //        DataTable dtDrum = DrumIssueEntryService.EditDrumDetail(id);
        //        for (int i = 0; i < dtDrum.Rows.Count; i++)
        //        {
        //            tda = new DrumIssueEntryItem();
                 
        //            tda.drum = dtDrum.Rows[i]["DRUMNO"].ToString();
        //            tda.qty = dtDrum.Rows[i]["QTY"].ToString();
                  
                    
        //            TData.Add(tda);
        //        }


        //        ca.Drumlst = TData;
        //    }

        //    return View(ca);

        //}
        //[HttpPost]
        //public IActionResult IssueToIndent(StockIn Cy, string id)
        //{
           
        //    try
        //    {
        //        Cy.ID = id;
        //        string Strout = DrumIssueEntryService.IssueToStockCRUD(Cy);
        //        if (string.IsNullOrEmpty(Strout))
        //        {
        //            if (Cy.ID == null)
        //            {
        //                TempData["notice"] = "Stock Issued Successfully...!";
        //            }
        //            else
        //            {
        //                TempData["notice"] = "Stock Issued Successfully...!";
        //            }
        //            return RedirectToAction("ListStockIn");
        //        }

        //        else
        //        {
        //            ViewBag.PageTitle = "Edit Indent";
        //            TempData["notice"] = Strout;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    //}
        //    return View(Cy);
        //}
    }
}
