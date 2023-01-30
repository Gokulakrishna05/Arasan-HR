using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Master;
using Arasan.Services.Store_Management;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Controllers.Store_Management
{
    public class StoreAccController : Controller
    {
        IStoreAccService StoreAccService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public StoreAccController(IStoreAccService _StoreAccService, IConfiguration _configuratio)
        {
            StoreAccService = _StoreAccService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult StoreAcc(string id)
        {
            StoreAcc st = new StoreAcc();
            st.Loc = BindLocation();
            st.Brlst = BindBranch();
            List<StoItem> TData = new List<StoItem>();
            StoItem tda = new StoItem();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new StoItem();
                    tda.ItemGrouplst = BindItemGrplst();
                    tda.Itlst = BindItemlst("");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                //st = StoreAccService.GetStoreAccById(id);

                DataTable dt = new DataTable();
                double total = 0;
                dt = StoreAccService.GetStoreAccDetails(id);
                if (dt.Rows.Count > 0)
                {
                    st.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    st.Location = dt.Rows[0]["LOCATIONID"].ToString();
                    st.Docid = dt.Rows[0]["DOCID"].ToString();
                    st.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    st.Refno = dt.Rows[0]["REFNO"].ToString();
                    st.Refdate = dt.Rows[0]["REFDATE"].ToString();
                    st.ID = id;
                    st.Retno = dt.Rows[0]["RETNO"].ToString();
                    st.Retdate = dt.Rows[0]["RETDATE"].ToString();
                    st.Narr = dt.Rows[0]["NARRATION"].ToString();


                }
                DataTable dt2 = new DataTable();
                dt2 = StoreAccService.GetStoreAccItemDetails(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new StoItem();
                        double toaamt = 0;
                        tda.ItemGrouplst = BindItemGrplst();
                        DataTable dt3 = new DataTable();
                        dt3 = datatrans.GetItemSubGroup(dt2.Rows[i]["ITEMID"].ToString());
                        if (dt3.Rows.Count > 0)
                        {
                            tda.ItemGroupId = dt3.Rows[0]["SUBGROUPCODE"].ToString();
                        }
                        tda.Itlst = BindItemlst(tda.ItemId);
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
                        tda.Unit = dt2.Rows[i]["UNIT"].ToString();

                        
                        tda.FromBinID = Convert.ToDouble(dt2.Rows[i]["FROMBINID"].ToString() == "" ? "0" : dt2.Rows[i]["FROMBINID"].ToString());
                        tda.ToBinID = Convert.ToDouble(dt2.Rows[i]["TOBINID"].ToString() == "" ? "0" : dt2.Rows[i]["TOBINID"].ToString());
                        tda.Serial = Convert.ToDouble(dt2.Rows[i]["SERIALYN"].ToString() == "" ? "0" : dt2.Rows[i]["SERIALYN"].ToString());
                        tda.PendQty = Convert.ToDouble(dt2.Rows[i]["PENDQTY"].ToString() == "" ? "0" : dt2.Rows[i]["PENDQTY"].ToString());
                        tda.RejQty = Convert.ToDouble(dt2.Rows[i]["REJQTY"].ToString() == "" ? "0" : dt2.Rows[i]["REJQTY"].ToString());
                        tda.AccQty = Convert.ToDouble(dt2.Rows[i]["ACCQTY"].ToString() == "" ? "0" : dt2.Rows[i]["ACCQTY"].ToString());
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }

            }
            st.Stolst = TData;
            return View(st);
        }
        [HttpPost]
        public ActionResult StoreAcc(StoreAcc ss, string id)
        {

            try
            {
                ss.ID = id;
                string Strout = StoreAccService.StoreAccCRUD(ss);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (ss.ID == null)
                    {
                        TempData["notice"] = " StoreAccepatence Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " StoreAccepatence Updated Successfully...!";
                    }
                    return RedirectToAction("List_StoreAcc");
                }

                else
                {
                    ViewBag.PageTitle = "Edit HSNcode";
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
        public IActionResult List_StoreAcc()
        {
            IEnumerable<StoreAcc> sta = StoreAccService.GetAllStoreAcc();
            return View(sta);
        }
        public List<SelectListItem> BindLocation()
        {
            try
            {
                DataTable dtDesg = StoreAccService.GetLocation();
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
                DataTable dtDesg = StoreAccService.GetBranch();
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
           
                string Unit = "";
                string CF = "";
                string price = "";
                dt = datatrans.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                  
                    Unit = dt.Rows[0]["UNITID"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();
                    dt1 = StoreAccService.GetItemCF(ItemId, dt.Rows[0]["UNITMASTID"].ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        CF = dt1.Rows[0]["CF"].ToString();
                    }
                }

                var result = new {  Unit = Unit, CF = CF, price = price };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetItemJSON(string itemid)
        {
            StoItem model = new StoItem();
            //model.Itlst = BindItem(itemid);
            return Json(BindItemlst(itemid));
        }
    }
}
