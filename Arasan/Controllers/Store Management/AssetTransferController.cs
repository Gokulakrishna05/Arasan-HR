using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Interface.Store_Management;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Store_Management;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Controllers.Store_Management
{
    public class AssetTransferController : Controller
    {
        IAssetTransfer AssetTransferService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public AssetTransferController(IAssetTransfer _AssetTransferService, IConfiguration _configuratio)
        {
            AssetTransferService = _AssetTransferService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult AssetTransfer(string id)
        {
            AssetTransfer st = new AssetTransfer();
            st.Loc = BindLocation();
            st.ToLoc = BindLocation();
            st.Bin = BindBinID();
            st.ToBin = BindBinID();
            st.Brlst = BindBranch();
            st.Branch = Request.Cookies["BranchId"];
            st.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("ASTFR");
            if (dtv.Rows.Count > 0)
            {
                st.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
            List<AssetTransferItem> TData = new List<AssetTransferItem>();
            AssetTransferItem tda = new AssetTransferItem();

            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new AssetTransferItem();
                    tda.Itemlst = BindItemlst();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }

            }
            else
            {
            //    //st = StoreAccService.GetStoreAccById(id);
            //    DataTable dt = new DataTable();
            //    double total = 0;
            //    dt = SubContractingDCService.GetSubContractingDCDeatils(id);
            //    if (dt.Rows.Count > 0)
            //    {
            //        st.ID = id;
            //        st.Branch = dt.Rows[0]["BRANCHID"].ToString();
            //        st.DocId = dt.Rows[0]["DOCID"].ToString();
            //        st.Docdate = dt.Rows[0]["DOCDATE"].ToString();
            //        //st.Suplst = BindSupplier();
            //        st.Supplier = dt.Rows[0]["PARTYID"].ToString();
            //        st.Add1 = dt.Rows[0]["ADD1"].ToString();
            //        st.Add2 = dt.Rows[0]["ADD2"].ToString();
            //        st.City = dt.Rows[0]["CITY"].ToString();
            //        st.Location = dt.Rows[0]["LOCID"].ToString();
            //        st.Through = dt.Rows[0]["THROUGH"].ToString();
            //        st.Entered = dt.Rows[0]["ENTEREDBY"].ToString();
            //        st.TotalQty = dt.Rows[0]["TOTQTY"].ToString();
            //        st.Narration = dt.Rows[0]["NARRATION"].ToString();
            //        //ca.Net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());

            //    }
            //    DataTable dt2 = new DataTable();
            //    dt2 = SubContractingDCService.GetEditItemDetails(id);
            //    if (dt2.Rows.Count > 0)
            //    {
            //        for (int i = 0; i < dt2.Rows.Count; i++)
            //        {
            //            tda = new SubContractingItem();

            //            tda.Itemlst = BindItemlst();
            //            //tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();
            //            tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
            //            tda.Unit = dt2.Rows[i]["UNIT"].ToString();
            //            tda.Quantity = dt2.Rows[i]["QTY"].ToString();
            //            tda.rate = dt2.Rows[i]["RATE"].ToString();
            //            tda.Amount = dt2.Rows[i]["AMOUNT"].ToString();
            //            tda.Isvalid = "Y";
            //            TData.Add(tda);
            //        }
            //    }
               
            }
            st.Assetlst = TData;
            //st.RECDlst = TData1;
            return View(st);
        }
        [HttpPost]
        public ActionResult AssetTransfer(AssetTransfer ss, string id)
        {

            try
            {
                ss.ID = id;
                string Strout = AssetTransferService.AssetTransferCRUD(ss);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (ss.ID == null)
                    {
                        TempData["notice"] = " AssetTransfer Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " AssetTransfer Updated Successfully...!";
                    }
                    return RedirectToAction("ListAssetTransfer");
                }

                else
                {
                    ViewBag.PageTitle = "Edit AssetTransfer";
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
        public IActionResult ListAssetTransfer()
        {
            //IEnumerable<DirectAddition> sta = DirectAdditionService.GetAllDirectAddition(st, ed);
            return View();
        }
        public ActionResult MyAssetTransferGrid(string strStatus)
        {
            List<ListAssetTransferItem> Reg = new List<ListAssetTransferItem>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)AssetTransferService.GetAllListAssetTransferItemItems(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string View = string.Empty;
              
                string DeleteRow = string.Empty;

                View = "<a href=ViewAssetTransfer?id=" + dtUsers.Rows[i]["ASITEMTRANLOCID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View Details' width='20' /></a>";
                DeleteRow = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["ASITEMTRANLOCID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";

                Reg.Add(new ListAssetTransferItem
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["ASITEMTRANLOCID"].ToString()),
                    branch = dtUsers.Rows[i]["BRANCHID"].ToString(),
                    docid = dtUsers.Rows[i]["DOCID"].ToString(),
                    docDate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    loc = dtUsers.Rows[i]["LOCID"].ToString(),
                    view = View,
                    delrow = DeleteRow,



                });
            }

            return Json(new
            {
                Reg
            });

        }
        public ActionResult DeleteItem(string tag, String id)
        {

            string flag = AssetTransferService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListAssetTransfer");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListAssetTransfer");
            }
        }
        public List<SelectListItem> BindItemlst()
        {
            try
            {
                DataTable dtDesg = AssetTransferService.GetItem();
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
        public List<SelectListItem> BindBinID()
        {
            try
            {
                DataTable dtDesg = AssetTransferService.BindBinID();
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
        public ActionResult GetNarrDetail(string ItemId,string loc)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string narr = "";
             

                string floc = datatrans.GetDataString("SELECT LOCID FROM LOCDETAILS WHERE LOCDETAILSID ='" + loc + "'");
                string toloc = datatrans.GetDataString("SELECT LOCID FROM LOCDETAILS WHERE LOCDETAILSID ='" + ItemId + "'");


                narr = "Transferred from " + floc + " to " + toloc;
 
                var result = new { narr = narr };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetItemDetail(string ItemId,string loc)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable stock = new DataTable();

                string unit = "";
                string price = "";
                string totalstock = "";
                string asseststockp = "";
                string asseststockm = "";

                dt = AssetTransferService.GetItemDetails(ItemId);
               
                if (dt.Rows.Count > 0)
                {

                    unit = dt.Rows[0]["UNITID"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();

                    
                }
                asseststockp = datatrans.GetDataString("Select SUM(QTY) as qty from ASSTOCKVALUE where ITEMID='" + ItemId + "' AND LOCID= '" + loc + "' AND PLUSORMINUS ='p' ");

                asseststockm = datatrans.GetDataString("Select SUM(QTY) as qty from ASSTOCKVALUE where ITEMID='" + ItemId + "' AND LOCID= '" + loc + "' AND PLUSORMINUS ='m' ");
                if (asseststockp == "")
                {
                    asseststockp = "0";
                }
                if (asseststockm == "")
                {
                    asseststockm = "0";
                }
                double pstock = Convert.ToDouble(asseststockp);
                double pmstock = Convert.ToDouble(asseststockm);
                double Totpmstock = pstock - pmstock;
                totalstock = Totpmstock.ToString();
            
                if (totalstock == "")
                {
                totalstock = "0";
                }

            var result = new { unit = unit, price = price , totalstock = totalstock };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetItemJSON()
        {
            AssetTransferItem model = new AssetTransferItem();
            model.Itemlst = BindItemlst();
            return Json(BindItemlst());

        }
        public IActionResult ViewAssetTransfer(string id)
        {
            AssetTransfer ca = new AssetTransfer();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();
            dt = AssetTransferService.GetAssetTransfer(id)
;
            if (dt.Rows.Count > 0)
            {
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.DocId = dt.Rows[0]["DOCID"].ToString();
                ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                ca.Location = dt.Rows[0]["LOCID"].ToString();
                ca.ToLocation = dt.Rows[0]["Loc"].ToString();
                ca.BinId = dt.Rows[0]["BINID"].ToString();
                ca.ToBinId = dt.Rows[0]["Bin"].ToString();
                ca.Reason = dt.Rows[0]["REASONCODE"].ToString();
                ca.Order = dt.Rows[0]["BROWSEORDER"].ToString();
                ca.Gross = dt.Rows[0]["GROSS"].ToString();
                ca.Net = dt.Rows[0]["NET"].ToString();
                ca.Narration = dt.Rows[0]["NARRATION"].ToString();
                ca.ID = id;
            }
            List<AssetTransferItem> TData = new List<AssetTransferItem>();
            AssetTransferItem tda = new AssetTransferItem();

            dtt = AssetTransferService.GetAssetTransferItem(id)
;
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new AssetTransferItem();
                    tda.ItemId = dtt.Rows[i]["ITEMID"].ToString();
                    tda.Unit = dtt.Rows[i]["UNIT"].ToString();
                    tda.Current = dtt.Rows[i]["CLSTK"].ToString();
                    tda.Quantity = dtt.Rows[i]["QTY"].ToString();
                    tda.rate = dtt.Rows[i]["FCOSTRATE"].ToString();
                    tda.Amount = dtt.Rows[i]["AMOUNT"].ToString();
                    TData.Add(tda);
                }
            }

            ca.Assetlst = TData;
            return View(ca);
        }
    }
}
