using System.Collections.Generic;
using Arasan.Interface.Production;
using Arasan.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Services.Production;
using System.Collections.Specialized;

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
            ca.ToLoc = BindLocation();
            ca.Type = BindType();
            ca.RecList = BindEmp();
            ca.Entered = Request.Cookies["UserId"];
            ca.Applst = BindEmp();
            ca.Itemlst = BindItemlst("");
            ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            List<DrumIssueEntryItem> TData = new List<DrumIssueEntryItem>();
            DrumIssueEntryItem tda = new DrumIssueEntryItem();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new DrumIssueEntryItem();
                    tda.FBinlst = BindBinID();
                    tda.TBinlst = BindBinID();
                    tda.drumlst = Binddrum("","");
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
                DataTable dt2 = new DataTable();

                dt2 = DrumIssueEntryService.GetDIEDetail(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new DrumIssueEntryItem();

                       // tda.FBinlst = BindBinID();
                        tda.FBinId = dt2.Rows[i]["FBINID"].ToString();

                      //  tda.TBinlst = BindBinID();
                        tda.TBinid = dt2.Rows[i]["TBINID"].ToString();

                        //tda.drumlst = Binddrum();
                        tda.drumlst = Binddrum(ca.FromLoc,ca.Itemid);
                        tda.Drum = dt2.Rows[i]["NDRUMNO"].ToString();
                        tda.Isvalid = "Y";
                        tda.Qty = dt2.Rows[i]["QTY"].ToString();
                        tda.Batch = dt2.Rows[i]["BATCHNO"].ToString();
                        tda.Rate = Convert.ToDouble(dt2.Rows[0]["BATCHRATE"].ToString() == "" ? "0" : dt2.Rows[0]["BATCHRATE"].ToString());
                        tda.Amount = Convert.ToDouble(dt2.Rows[0]["AMOUNT"].ToString() == "" ? "0" : dt2.Rows[0]["AMOUNT"].ToString());
                        tda.ID = id;
                        TData.Add(tda);
                    }

                }

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
            IEnumerable<DrumIssueEntry> cmp = DrumIssueEntryService.GetAllDrumIssueEntry();
            return View(cmp);
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
        public JsonResult GetDrumJSON(string id, string item)
        {
            DrumIssueEntryItem model = new DrumIssueEntryItem();
            model.drumlst = Binddrum(id, item);
            return Json(Binddrum(id, item));
        }
        public List<SelectListItem> Binddrum(string value,string item)
        {
            try
            {
                DataTable dtDesg = DrumIssueEntryService.DrumDeatils(value, item);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DRUM_NO"].ToString(), Value = dtDesg.Rows[i]["DRUM_ID"].ToString() });
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
            DrumIssueEntryItem model = new DrumIssueEntryItem();
            //model.Itemlst = BindItemlst(itemid);
            return Json(model);

        }
        public JsonResult GetItemGrpJSON(string id, string item)
        {
            DrumIssueEntryItem model = new DrumIssueEntryItem();
              model.drumlst = Binddrum(id, item);
            return Json(Binddrum(id , item));
        }
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
                DataTable dtDesg = DrumIssueEntryService.GetItem();
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
        public ActionResult GetDrumDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string batch = "";
                string qty = "";

                dt = DrumIssueEntryService.GetDrumDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    batch = dt.Rows[0]["BATCHNO"].ToString();
                    qty = dt.Rows[0]["QTY"].ToString();



                }

                var result = new { batch = batch , qty= qty };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
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

        public IActionResult ApproveDrumIssue(string DRUM)
        {
            DrumIssueEntry ca = new DrumIssueEntry();
            DataTable dt = DrumIssueEntryService.EditDrumIssue(DRUM);
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
                DataTable dtDrum = DrumIssueEntryService.EditDrumDetail(DRUM);
                for (int i = 0; i < dtDrum.Rows.Count; i++)
                {
                    tda = new DrumIssueEntryItem();

                    
                    tda.FBinId = dtDrum.Rows[i]["FBINID"].ToString();
 
                    tda.TBinid = dtDrum.Rows[i]["TBINID"].ToString();
 
                    tda.Drum = dtDrum.Rows[i]["DRUMNO"].ToString();

                    tda.Qty = dtDrum.Rows[i]["QTY"].ToString();
                    tda.Batch = dtDrum.Rows[i]["BATCHNO"].ToString();
                    tda.Rate = Convert.ToDouble(dtDrum.Rows[0]["BATCHRATE"].ToString() == "" ? "0" : dtDrum.Rows[0]["BATCHRATE"].ToString());
                    tda.Amount = Convert.ToDouble(dtDrum.Rows[0]["AMOUNT"].ToString() == "" ? "0" : dtDrum.Rows[0]["AMOUNT"].ToString());

                    TData.Add(tda);
                }


                ca.Drumlst = TData;
            }
            return View(ca);
        }
    }
}
