using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Interface.Store_Management;
using Arasan.Models;
using Arasan.Services.Store_Management;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Controllers.Store_Management
{
    public class DirectAdditionController : Controller
    {
        IDirectAddition DirectAdditionService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public DirectAdditionController(IDirectAddition _DirectAdditionService, IConfiguration _configuratio)
        {
            DirectAdditionService = _DirectAdditionService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult DirectAddition(string id)
        {
            DirectAddition st = new DirectAddition();
            st.Loc = BindLocation();
            st.Brlst = BindBranch();
            st.assignList = BindEmp();
            List<DirectItem> TData = new List<DirectItem>();
            DirectItem tda = new DirectItem();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new DirectItem();
                    tda.ItemGrouplst = BindItemGrplst();
                    tda.Itemlst = BindItemlst("");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                //st = StoreAccService.GetStoreAccById(id);

                DataTable dt = new DataTable();
                double total = 0;
                dt = DirectAdditionService.GetDirectAdditionDetails(id);
                if (dt.Rows.Count > 0)
                {
                    st.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    st.Location = dt.Rows[0]["LOCID"].ToString();
                    st.DocId = dt.Rows[0]["DOCID"].ToString();
                    st.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    st.ChellanNo = dt.Rows[0]["DCNO"].ToString();
                    st.Reason = dt.Rows[0]["REASON"].ToString();     
                    //st.Gro = dt.Rows[0]["GROSS"].ToString();
                    st.Entered = dt.Rows[0]["ENTBY"].ToString();
                    st.Narr = dt.Rows[0]["NARRATION"].ToString();

                    st.Gro = Convert.ToDouble(dt.Rows[0]["GROSS"].ToString() == "" ? "0" : dt.Rows[0]["GROSS"].ToString());
                    st.Net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());
                }
                DataTable dt2 = new DataTable();
                dt2 = DirectAdditionService.GetDAItemDetails(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new DirectItem();
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
        public ActionResult DirectAddition(DirectAddition ss, string id)
        {

            try
            {
                ss.ID = id;
                string Strout = DirectAdditionService.DirectAdditionCRUD(ss);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (ss.ID == null)
                    {
                        TempData["notice"] = " DirectAddition Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " DirectAddition Updated Successfully...!";
                    }
                    return RedirectToAction("ListDirectAddition");
                }

                else
                {
                    ViewBag.PageTitle = "Edit DirectAddition";
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
        public IActionResult ListDirectAddition(string status)
        {
            IEnumerable<DirectAddition> sta = DirectAdditionService.GetAllDirectAddition(status);
            return View(sta);
        }
        public List<SelectListItem> BindLocation()
        {
            try
            {
                DataTable dtDesg = DirectAdditionService.GetLocation();
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
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPNAME"].ToString(), Value = dtDesg.Rows[i]["EMPMASTID"].ToString() });
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
        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dtDesg = DirectAdditionService.GetBranch();
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

                string unit = "";
                string CF = "";
                string price = "";
                dt = datatrans.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    unit = dt.Rows[0]["UNITID"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();
                    dt1 = DirectAdditionService.GetItemCF(ItemId, dt.Rows[0]["UNITMASTID"].ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        CF = dt1.Rows[0]["CF"].ToString();
                    }
                }

                var result = new { unit = unit, CF = CF, price = price };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetItemJSON(string itemid)
        {
            DirectItem model = new DirectItem();
            model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst(itemid));

        }
        public JsonResult GetItemGrpJSON()
        {
            //DirectItem model = new DirectItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItemGrplst());
        }

        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = DirectAdditionService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListDirectAddition");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListDirectAddition");
            }
        }

    }
    
}
