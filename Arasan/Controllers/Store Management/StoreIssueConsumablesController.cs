using System.Collections.Generic;
using Arasan.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Models;
using System.Xml.Linq;
using Arasan.Services;

namespace Arasan.Controllers
{
    public class StoreIssueConsumablesController : Controller
    {
        IStoreIssueConsumables StoreIssService;
        private string? _connectionString;
        IConfiguration? _configuratio;
        DataTransactions datatrans;
        public StoreIssueConsumablesController(IStoreIssueConsumables _StoreIssService, IConfiguration _configuratio)
        {
            StoreIssService = _StoreIssService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult StoreIssueCons(string id)
        {

            StoreIssueConsumables ca = new StoreIssueConsumables();
            ca.Brlst = BindBranch();
            ca.Loclst = GetLoc();
            ca.EnqassignList = BindEmp();
            List<SICItem> TData = new List<SICItem>();
            SICItem tda = new SICItem();
            if (id == null)
            {

                for (int i = 0; i < 3; i++)
                {
                    tda = new SICItem();
                    tda.ItemGrouplst = BindItemGrplst();
                    tda.Itemlst = BindItemlst("");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                // ca = StoreIssService.GetLocationsById(id);
                DataTable dt = new DataTable();
                double total = 0;
                dt = StoreIssService.EditSICbyID(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    ca.DocNo = dt.Rows[0]["DOCID"].ToString();
                    ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.ReqNo = dt.Rows[0]["REQNO"].ToString();
                    ca.ID = id;
                    ca.ReqDate = dt.Rows[0]["REQDATE"].ToString();
                    ca.Location = dt.Rows[0]["TOLOCID"].ToString();
                    ca.LocCon = dt.Rows[0]["LOCIDCONS"].ToString();
                    ca.Process = dt.Rows[0]["PROCESSID"].ToString();
                    ca.MCNo = dt.Rows[0]["MCID"].ToString();
                    ca.MCNa = dt.Rows[0]["MCNAME"].ToString();
                    ca.Narr = dt.Rows[0]["NARRATION"].ToString();
                    ca.User = dt.Rows[0]["USERID"].ToString();
                    ca.Work = dt.Rows[0]["WCID"].ToString();
                }
                DataTable dt2 = new DataTable();
                dt2 = StoreIssService.GetSICItemDetails(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new SICItem();
                        double toaamt = 0;
                        tda.ItemGrouplst = BindItemGrplst();
                        DataTable dt3 = new DataTable();
                        dt3 = datatrans.GetItemSubGroup(dt2.Rows[i]["ITEMID"].ToString());
                        if (dt3.Rows.Count > 0)
                        {
                            tda.ItemGroupId = dt3.Rows[0]["SUBGROUPCODE"].ToString();
                        }
                        tda.Itemlst = BindItemlst(tda.ItemGroupId);
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
                     
                        //tda.DRLst = BindDrum();
                        //tda.SRLst = BindSerial();
                        //tda.Drum = dt2.Rows[i]["DRUMYN"].ToString();
                        //tda.Serial = dt2.Rows[i]["SERIALYN"].ToString();
                        //tda.unitprim= dt2.Rows[i]["UNITID"].ToString();
                        //tda.FromBin = Convert.ToDouble(dt2.Rows[i]["CGSTPER"].ToString() == "" ? "0" : dt2.Rows[i]["CGSTPER"].ToString());
                        tda.PendQty = Convert.ToDouble(dt2.Rows[i]["PENDQTY"].ToString() == "" ? "0" : dt2.Rows[i]["PENDQTY"].ToString());
                        tda.ReqQty = Convert.ToDouble(dt2.Rows[i]["REQQTY"].ToString() == "" ? "0" : dt2.Rows[i]["REQQTY"].ToString());
                        tda.Indp = Convert.ToDouble(dt2.Rows[i]["INDP"].ToString() == "" ? "0" : dt2.Rows[i]["INDP"].ToString());
                        //tda.SGSTAmt = Convert.ToDouble(dt2.Rows[i]["SGSTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["SGSTAMT"].ToString());
                        //tda.IGSTAmt = Convert.ToDouble(dt2.Rows[i]["IGSTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["IGSTAMT"].ToString());
                        //tda.DiscPer = Convert.ToDouble(dt2.Rows[i]["DISCPER"].ToString() == "" ? "0" : dt2.Rows[i]["DISCPER"].ToString());
                        //tda.DiscAmt = Convert.ToDouble(dt2.Rows[i]["DISCAMT"].ToString() == "" ? "0" : dt2.Rows[i]["DISCAMT"].ToString());
                        //tda.FrieghtAmt = Convert.ToDouble(dt2.Rows[i]["FREIGHTCHGS"].ToString() == "" ? "0" : dt2.Rows[i]["FREIGHTCHGS"].ToString());
                        //tda.TotalAmount = Convert.ToDouble(dt2.Rows[i]["TOTALAMT"].ToString() == "" ? "0" : dt2.Rows[i]["TOTALAMT"].ToString());
                        
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }
              
            }
            ca.SICLst = TData;
            return View(ca);
        }
        [HttpPost]
        public ActionResult StoreIssueConsumables(StoreIssueConsumables Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = StoreIssService.StoreIssueCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "StoreIssueCons Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "StoreIssueCons Updated Successfully...!";
                    }
                    return RedirectToAction("ListStoreIssueCons");
                }

                else
                {
                    ViewBag.PageTitle = "Edit StoreIssueCons";
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
        public List<SelectListItem> GetLoc()
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

        public List<SelectListItem> BindItemGrplst()
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
        //public List<SelectListItem> BindDrum()
        //{
            //    try
            //    {
            //        List<SelectListItem> lstdesg = new List<SelectListItem>();
            //        lstdesg.Add(new SelectListItem() { Text = "YES", Value = "YES" });
            //        lstdesg.Add(new SelectListItem() { Text = "NO", Value = "NO" });

            //        return lstdesg;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}
            //public List<SelectListItem> BindSerial()
            //{
            //    try
            //    {
            //        List<SelectListItem> lstdesg = new List<SelectListItem>();
            //        lstdesg.Add(new SelectListItem() { Text = "YES", Value = "YES" });
            //        lstdesg.Add(new SelectListItem() { Text = "NO", Value = "NO" });

            //        return lstdesg;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}
            public ActionResult GetItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                string Desc = "";
                string unit = "";
                string CF = "";
                string price = "";
                dt = datatrans.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    Desc = dt.Rows[0]["ITEMDESC"].ToString();
                    unit = dt.Rows[0]["UNITMASTID"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();
                    dt1 = StoreIssService.GetItemCF(ItemId, dt.Rows[0]["UNITMASTID"].ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        CF = dt1.Rows[0]["CF"].ToString();
                    }
                }

                var result = new { Desc = Desc, unit = unit, CF = CF, price = price };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetItemJSON(string itemid)
        {
            SICItem model = new SICItem();
            model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst(itemid));

        }
        public JsonResult GetItemGrpJSON()
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItemGrplst());
        }

        public IActionResult ListStoreIssueCons()
        {
            IEnumerable<StoreIssueConsumables> cmp = StoreIssService.GetAllStoreIssue();
            return View(cmp);
        }
    }
}
