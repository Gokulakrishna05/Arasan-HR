using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Interface.Store_Management;
using Arasan.Models;
using Arasan.Services.Store_Management;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers.Store_Management
{
    public class DirectDeductionController : Controller
    {
        IDirectDeductionService DirectDeductionService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public DirectDeductionController(IDirectDeductionService _DirectDeductionService, IConfiguration _configuratio)
        {
            DirectDeductionService = _DirectDeductionService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult DirectDeduction(string id)
        {
            DirectDeduction st = new DirectDeduction();
            st.Loc = BindLocation();
            st.Brlst = BindBranch();
            List<DeductionItem> TData = new List<DeductionItem>();
            DeductionItem tda = new DeductionItem();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new DeductionItem();
                    tda.Itlst = BindItem("");
                    tda.ItemGrouplst = BindItemGrplst();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                //st = DirectDeductionService.GetDirectDeductionById(id);

                DataTable dt = new DataTable();
                double total = 0;
                dt = DirectDeductionService.GetDirectDeductionDetails(id);
                if (dt.Rows.Count > 0)
                {
                    st.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    st.Location = dt.Rows[0]["LOCID"].ToString();
                    st.DocId = dt.Rows[0]["DOCID"].ToString();
                    st.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    st.Dcno = dt.Rows[0]["DCNO"].ToString();
                    st.Reason = dt.Rows[0]["REASON"].ToString();
                    st.Gro = dt.Rows[0]["GROSS"].ToString();
                    st.Entered = dt.Rows[0]["ENTBY"].ToString();
                    st.Narr = dt.Rows[0]["NARRATION"].ToString();
                    st.NoDurms = dt.Rows[0]["NOOFD"].ToString();
                   

                }
                DataTable dt2 = new DataTable();
                dt2 = DirectDeductionService.GetDDItemDetails(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new DeductionItem();
                        double toaamt = 0;
                        tda.ItemGrouplst = BindItemGrplst();
                        DataTable dt3 = new DataTable();
                        dt3 = datatrans.GetItemSubGroup(dt2.Rows[i]["ITEMID"].ToString());
                        if (dt3.Rows.Count > 0)
                        {
                            tda.ItemGroupId = dt3.Rows[0]["SUBGROUPCODE"].ToString();
                        }
                        tda.Itlst = BindItem(tda.ItemGroupId);
                        tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();
                        DataTable dt4 = new DataTable();
                        dt4 = datatrans.GetItemDetails(tda.ItemId);
                        if (dt4.Rows.Count > 0)
                        {

                            tda.ConFac = dt4.Rows[0]["CF"].ToString();
                            tda.Rate = Convert.ToDouble(dt4.Rows[0]["LATPURPRICE"].ToString());
                        }
                        tda.Quantity = Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                        toaamt = tda.Rate * tda.Quantity;
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
                        tda.BinID = Convert.ToDouble(dt2.Rows[i]["BINID"].ToString() == "" ? "0" : dt2.Rows[i]["BINID"].ToString());
                        tda.Process = Convert.ToDouble(dt2.Rows[i]["PROCESSID"].ToString() == "" ? "0" : dt2.Rows[i]["PROCESSID"].ToString());
                        //tda.Indp = Convert.ToDouble(dt2.Rows[i]["INDP"].ToString() == "" ? "0" : dt2.Rows[i]["INDP"].ToString());
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
            st.Itlst = TData;
            return View(st);
        }
        [HttpPost]
        public ActionResult DirectDeduction(DirectDeduction ss, string id)
        {

            try
            {
                ss.ID = id;
                string Strout = DirectDeductionService.DirectDeductionCRUD(ss);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (ss.ID == null)
                    {
                        TempData["notice"] = " DirectDeduction Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " DirectDeduction Updated Successfully...!";
                    }
                    return RedirectToAction("ListDirectDeduction");
                }

                else
                {
                    ViewBag.PageTitle = "Edit DirectDeduction";
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
        public IActionResult ListDirectDeduction()
        {
            IEnumerable<DirectDeduction> sta = DirectDeductionService.GetAllDirectDeduction();
            return View(sta);
        }
        public List<SelectListItem> BindLocation()
        {
            try
            {
                DataTable dtDesg = DirectDeductionService.GetLocation();
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
        public List<SelectListItem> BindItem(string value)
        {
            try
            {
                DataTable dtDesg = DirectDeductionService.GetItem(value);
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
                DataTable dtDesg = DirectDeductionService.GetItemGrp();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["GROUPCODE"].ToString(), Value = dtDesg.Rows[i]["ITEMGROUPID"].ToString() });
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
                DataTable dtDesg = DirectDeductionService.GetBranch();
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
        public ActionResult GetItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                string Desc = "";
                string Unit = "";
                string CF = "";
                string price = "";
                dt = datatrans.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    Desc = dt.Rows[0]["ITEMDESC"].ToString();
                    Unit = dt.Rows[0]["UNITID"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();
                    dt1 = DirectDeductionService.GetItemCF(ItemId, dt.Rows[0]["UNITMASTID"].ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        CF = dt1.Rows[0]["CF"].ToString();
                    }
                }
                
                var result = new { Desc = Desc, Unit = Unit, CF = CF, price = price };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetItemJSON(string itemid)
        {
            DeductionItem model = new DeductionItem();
             model.Itlst = BindItem(itemid);
            return Json(BindItem(itemid));
        }
        public JsonResult GetItemGrpJSON()
        {
            DeductionItem model = new DeductionItem();
             model.ItemGrouplst = BindItemGrplst();
            return Json(BindItemGrplst());
        }
    }

}

