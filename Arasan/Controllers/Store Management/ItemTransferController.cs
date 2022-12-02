using Arasan.Interface.Master;
using Arasan.Interface.Store_Management;
using Arasan.Models;
using Arasan.Services.Store_Management;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Controllers.Store_Management
{
    public class ItemTransferController : Controller
    {
        IItemTransferService ItemTransferService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public ItemTransferController(IItemTransferService _ItemTransferService)
        {
            ItemTransferService = _ItemTransferService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult ItemTransfer(string id)
        {
            ItemTransfer st = new ItemTransfer();
            st.Loc = BindLocation();
            st.Brlst = BindBranch();
            List<Itemtran> TData = new List<Itemtran>();
            Itemtran tda = new Itemtran();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new Itemtran();
                    tda.Itlst = BindItem("");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                //st = ItemTransferService.GetItemTransferById(id);

                DataTable dt = new DataTable();
                double total = 0;
                dt = ItemTransferService.GetItemTransferDetails(id);
                if (dt.Rows.Count > 0)
                {
                    st.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    st.Location = dt.Rows[0]["FROMLOC"].ToString();
                    st.Toloc = dt.Rows[0]["DESTLOC"].ToString();
                    st.Docid = dt.Rows[0]["DOCID"].ToString();
                    st.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    st.Reason = dt.Rows[0]["REASONCODE"].ToString();
                    st.Gro = dt.Rows[0]["GROSS"].ToString();
                    st.Net = dt.Rows[0]["NET"].ToString();
                    st.Narr = dt.Rows[0]["NARRATION"].ToString();
                 


                }
                DataTable dt2 = new DataTable();
                dt2 = ItemTransferService.GetItemTransferItemDetails(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new Itemtran();
                        double toaamt = 0;
                        DataTable dt3 = new DataTable();
                        dt3 = datatrans.GetItemSubGroup(dt2.Rows[i]["ITEMID"].ToString());
                        if (dt3.Rows.Count > 0)
                            tda.Itlst = BindItem(tda.ItemId);
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


                        tda.FromBinID = Convert.ToDouble(dt2.Rows[i]["FROMBINID"].ToString() == "" ? "0" : dt2.Rows[i]["FROMBINID"].ToString());
                        tda.ToBinID = Convert.ToDouble(dt2.Rows[i]["TOBINID"].ToString() == "" ? "0" : dt2.Rows[i]["TOBINID"].ToString());
                        tda.Serial = Convert.ToDouble(dt2.Rows[i]["SERIALYN"].ToString() == "" ? "0" : dt2.Rows[i]["SERIALYN"].ToString());
                        tda.Lot = Convert.ToDouble(dt2.Rows[i]["PENDQTY"].ToString() == "" ? "0" : dt2.Rows[i]["LSTOCK"].ToString());
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }

            }
            st.Itlst = TData;
            return View(st);
        }
        [HttpPost]
        public ActionResult ItemTransfer(ItemTransfer ss, string id)
        {

            try
            {
                ss.ID = id;
                string Strout = ItemTransferService.ItemTransferCRUD(ss);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (ss.ID == null)
                    {
                        TempData["notice"] = " ItemTransfer Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " ItemTransfer Updated Successfully...!";
                    }
                    return RedirectToAction("ListItemTransfer");
                }

                else
                {
                    ViewBag.PageTitle = "Edit ItemTransfer";
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
        public IActionResult ListItemTransfer()
        {
            IEnumerable<ItemTransfer> sta = ItemTransferService.GetAllItemTransfer();
            return View(sta);
        }
        public List<SelectListItem> BindLocation()
        {
            try
            {
                DataTable dtDesg = ItemTransferService.GetLocation();
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
                DataTable dtDesg = ItemTransferService.GetItem(value);
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
                DataTable dtDesg = ItemTransferService.GetBranch();
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
                string unit = "";
                string CF = "";
                string price = "";
                dt = datatrans.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    Desc = dt.Rows[0]["ITEMDESC"].ToString();
                    unit = dt.Rows[0]["UNITID"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();
                    dt1 = ItemTransferService.GetItemCF(ItemId, dt.Rows[0]["UNITMASTID"].ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        CF = dt1.Rows[0]["CF"].ToString();
                    }
                }

                var result = new { Desc = Desc, Unit = unit, CF = CF, price = price };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetItemJSON(string itemid)
        {
            Itemtran model = new Itemtran();
            model.Itlst = BindItem(itemid);
            return Json(BindItem(itemid));
        }
    }
}

