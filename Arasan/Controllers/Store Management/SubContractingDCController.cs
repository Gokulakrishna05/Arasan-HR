using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Interface.Store_Management;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Store_Management;
using AspNetCore.Reporting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Controllers.Store_Management
{
    public class SubContractingDCController : Controller
    {
        ISubContractingDC SubContractingDCService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        DataTransactions datatrans;
        public SubContractingDCController(ISubContractingDC _SubContractingDCService, IConfiguration _configuratio,IWebHostEnvironment WebHostEnvironment)
        {
            SubContractingDCService = _SubContractingDCService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
            this._WebHostEnvironment = WebHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }
        public IActionResult SubContractingDC(string id)
        {
            SubContractingDC st = new SubContractingDC();
            st.Loc = BindLocation();
            st.Brlst = BindBranch();
            st.Suplst = BindSupplier();
            st.assignList = BindEmp();
            st.Branch = Request.Cookies["BranchId"];
            st.Entered = Request.Cookies["UserId"];
            st.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("subdc");
            if (dtv.Rows.Count > 0)
            {
                st.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
            List<SubContractingItem> TData = new List<SubContractingItem>();
            SubContractingItem tda = new SubContractingItem();
            List<ReceiptDetailItem> TData1 = new List<ReceiptDetailItem>();
            ReceiptDetailItem tda1 = new ReceiptDetailItem();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new SubContractingItem();
                    tda.Itemlst = BindItemlst("");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda1 = new ReceiptDetailItem();
                    tda1.Itemlist = BindItemlist("");
                    tda1.Isvalid1 = "Y";
                    TData1.Add(tda1);
                }
            }
            else
            {
                //st = StoreAccService.GetStoreAccById(id);
                DataTable dt = new DataTable();
                double total = 0;
                dt = SubContractingDCService.GetSubContractingDCDeatils(id);
                if (dt.Rows.Count > 0)
                {
                    st.ID = id;
                    st.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    st.DocId = dt.Rows[0]["DOCID"].ToString();
                    st.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    //st.Suplst = BindSupplier();
                    st.Supplier = dt.Rows[0]["PARTYID"].ToString();
                    st.Add1 = dt.Rows[0]["ADD1"].ToString();
                    st.Add2 = dt.Rows[0]["ADD2"].ToString();
                    st.City = dt.Rows[0]["CITY"].ToString();
                    st.Location = dt.Rows[0]["LOCID"].ToString();
                    st.Through = dt.Rows[0]["THROUGH"].ToString();
                    st.Entered = dt.Rows[0]["ENTEREDBY"].ToString();
                    st.TotalQty = dt.Rows[0]["TOTQTY"].ToString();
                    st.Narration = dt.Rows[0]["NARRATION"].ToString();
                    //ca.Net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());

                }
                DataTable dt2 = new DataTable();
                dt2 = SubContractingDCService.GetEditItemDetails(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new SubContractingItem();

                        tda.Itemlst = BindItemlst("");
                        //tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.Unit = dt2.Rows[i]["UNIT"].ToString();
                        tda.Quantity = dt2.Rows[i]["QTY"].ToString();
                        tda.rate = dt2.Rows[i]["RATE"].ToString();
                        tda.Amount = dt2.Rows[i]["AMOUNT"].ToString();
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }
                DataTable dt3 = new DataTable();
                dt3 = SubContractingDCService.GetEditReceiptDetailItem(id);
                if (dt3.Rows.Count > 0)
                {
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        tda1 = new ReceiptDetailItem();

                        tda1.Itemlist = BindItemlst("");
                        //tda1.saveItemId = dt3.Rows[i]["ITEMID"].ToString();
                        tda1.ItemId = dt3.Rows[i]["RITEM"].ToString();
                        tda1.Unit = dt3.Rows[i]["RUNIT"].ToString();
                        tda1.Quantity = dt3.Rows[i]["ERQTY"].ToString();
                        tda1.rate = dt3.Rows[i]["ERATE"].ToString();
                        tda1.Amount = dt3.Rows[i]["EAMOUNT"].ToString();
                        tda1.Isvalid1 = "Y";
                        TData1.Add(tda1);
                    }
                }
            }
            st.SCDIlst = TData;
            st.RECDlst = TData1;
            return View(st);
        }
        [HttpPost]
        public ActionResult SubContractingDC(SubContractingDC ss, string id)
        {

            try
            {
                ss.ID = id;
                string Strout = SubContractingDCService.SubContractingDCCRUD(ss);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (ss.ID == null)
                    {
                        TempData["notice"] = " SubContractingDC Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " SubContractingDC Updated Successfully...!";
                    }
                    return RedirectToAction("ListSubContractingDC");
                }

                else
                {
                    ViewBag.PageTitle = "Edit SubContractingDC";
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
        public IActionResult ListSubContractingDC()
        {
            //IEnumerable<DirectAddition> sta = DirectAdditionService.GetAllDirectAddition(st, ed);
            return View();
        }
        public ActionResult MyListSubContractingDCGrid(string strStatus)
        {
            List<ListSubContractingDCItem> Reg = new List<ListSubContractingDCItem>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)SubContractingDCService.GetAllListSubContractingDCItems(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string pack = string.Empty;
                //string approve = string.Empty;
                string View = string.Empty;
                string EditRow = string.Empty;
                string DeleteRow = string.Empty;
                string recept = string.Empty;
                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    if (dtUsers.Rows[i]["STATUS"].ToString() == "Approve")
                    {
                        //approve = "";
                        View = "<a href=ViewSubContractingDC?id=" + dtUsers.Rows[i]["SUBCONTDCBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View Details' width='20' /></a>";
                        recept = "<a href=SubConDcRec?id=" + dtUsers.Rows[i]["SUBCONTDCBASICID"].ToString() + "><img src='../Images/pdficon.png' alt='View Details' width='20' /></a>";

                       // EditRow = "";
                    }
                    else
                    {
                        //approve = "<a href=ApproveSubContractingDC?id=" + dtUsers.Rows[i]["SUBCONTDCBASICID"].ToString() + " ><img src='../Images/move_quote.png' alt='View Details' width='20' /></a>";
                        pack = "<a href=PackingMatSubContractingDC?id=" + dtUsers.Rows[i]["SUBCONTDCBASICID"].ToString() + " ><img src='../Images/view_icon.png' alt='View Details' width='20' /></a>";
                        View = "<a href=ViewSubContractingDC?id=" + dtUsers.Rows[i]["SUBCONTDCBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View Details' width='20' /></a>";
                        recept = "<a href=SubConDcRec?id=" + dtUsers.Rows[i]["SUBCONTDCBASICID"].ToString() + " target='_blank'><img src='../Images/pdficon.png' alt='View Details' width='20' /></a>";

                       // EditRow = "<a href=SubContractingDC?id=" + dtUsers.Rows[i]["SUBCONTDCBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";

                    }
                }
                DeleteRow = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["SUBCONTDCBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";

                Reg.Add(new ListSubContractingDCItem
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["SUBCONTDCBASICID"].ToString()),
                    branch = dtUsers.Rows[i]["BRANCHID"].ToString(),
                    docid = dtUsers.Rows[i]["DOCID"].ToString(),
                    docDate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    loc = dtUsers.Rows[i]["LOCID"].ToString(),
                    tot = dtUsers.Rows[i]["TOTQTY"].ToString(),
                    //approve = approve,
                    pack = pack,
                    view = View,
                    recept = recept,
                   // editrow = EditRow,
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

            string flag = SubContractingDCService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListSubContractingDC");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListSubContractingDC");
            }
        }
        public List<SelectListItem> BindItemlst(string value)
        {
            try
            {
                DataTable dtDesg = SubContractingDCService.GetItem(value);
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
        public List<SelectListItem> BindPackItemlst( )
        {
            try
            {
                DataTable dtDesg = SubContractingDCService.GetPackItem();
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
        public List<SelectListItem> BindItemlist(string id)
        {
            try
            {
                DataTable dtDesg = SubContractingDCService.GetPartyItem(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["WIPITEMID"].ToString() });
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
                DataTable dtDesg = SubContractingDCService.GetLocation();


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
        public List<SelectListItem> BindSupplier()
        {
            try
            {
                DataTable dtDesg = SubContractingDCService.GetSupplier();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PartyID"].ToString(), Value = dtDesg.Rows[i]["PARTYMASTID"].ToString() });
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
        public ActionResult GetPartyDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string add1 = "";
                string add2 = "";
                string city = "";
                dt = SubContractingDCService.GetPartyDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    add1 = dt.Rows[0]["ADD1"].ToString();
                    add2 = dt.Rows[0]["ADD2"].ToString();
                    city = dt.Rows[0]["CITY"].ToString();
                }

                var result = new { add1 = add1, add2 = add2, city = city };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetItemGrpJSON(string id)
        {
            SubContractingItem model = new SubContractingItem();
            model.Itemlst = BindItemlst(id);
            return Json(BindItemlst(id));

        }
        public JsonResult GetItemJSON(string ItemId)
        {
          ReceiptDetailItem model = new ReceiptDetailItem();
           model.Itemlist = BindItemlist(ItemId);
           return Json(BindItemlist(ItemId));

        }
        public JsonResult GetStockItemJSON(string ItemId)
        {
            SubContractingItem model = new SubContractingItem();
            model.Itemlst = BindItemlst(ItemId);
            return Json(BindItemlst(ItemId));

        }
        public JsonResult GetPackItemJSON(string ItemId)
        {
            PackMatItem model = new PackMatItem();
            model.Itemlst = BindPackItemlst( );
            return Json(BindPackItemlst( ));

        }
        public ActionResult GetItemDetail(string ItemId, string loc)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string unit = "";
                string cf = "";
                string price = "";
                string lot = "";
                string group = "";
                string stock = "";
               //string binno = "";
               //string binname = "";
               dt = SubContractingDCService.GetItemDetails(ItemId);
                string type = datatrans.GetDataString("SELECT LOTYN FROM ITEMMASTER WHERE ITEMMASTERID='" + ItemId + "'");

                if (type == "YES")
                {
                    stock = datatrans.GetDataString("select SUM(S.PLUSQTY-S.MINUSQTY) as QTY  from LSTOCKVALUE S  where S.LOCID='" + loc + "' AND S.ITEMID='" + ItemId + "' HAVING SUM(S.PLUSQTY-S.MINUSQTY) > 0");
                }
                else
                {
                    stock = datatrans.GetDataString("select SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) as QTY  from STOCKVALUE S  where S.LOCID='" + loc + "' AND S.ITEMID='" + ItemId + "' HAVING SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) > 0  ");
                }
                if (dt.Rows.Count > 0)
                {

                    unit = dt.Rows[0]["UNITID"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();
                    lot = dt.Rows[0]["LOTYN"].ToString();
                   
                        dt1 = SubContractingDCService.GetItemCF(ItemId, dt.Rows[0]["UNITMASTID"].ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        cf = dt1.Rows[0]["CF"].ToString();
                    }
                }

                var result = new { unit = unit, cf = cf, price = price, lot = lot, stock = stock  };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetPackItemDetail(string ItemId, string loc)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string unit = "";
                string cf = "";
                string price = "";
                string lot = "";
                string group = "";
                string stock = "";
                //string binno = "";
                //string binname = "";
                dt = SubContractingDCService.GetItemDetails(ItemId);
                 stock = datatrans.GetDataString("Select SUM(BALANCE_QTY) from INVENTORY_ITEM where ITEM_ID='" + ItemId + "' AND BALANCE_QTY > 0 AND LOCATION_ID= '" + loc + "'  ");

                if (dt.Rows.Count > 0)
                {

                    unit = dt.Rows[0]["UNITID"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();
                    
                  
                    dt1 = SubContractingDCService.GetItemCF(ItemId, dt.Rows[0]["UNITMASTID"].ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        cf = dt1.Rows[0]["CF"].ToString();
                    }
                }

                var result = new { unit = unit, cf = cf, price = price , stock = stock  };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult ListSubContractDrumSelection(string id)
        {
            SubContractDDDrumdetailstable ca = new SubContractDDDrumdetailstable();
            List<SubContractDDrumdetails> TData = new List<SubContractDDrumdetails>();
            SubContractDDrumdetails tda = new SubContractDDrumdetails();
           DataTable dtEnq = SubContractingDCService.ViewSubContractDrumDetails(id);
            for (int i = 0; i < dtEnq.Rows.Count; i++)
            {
                tda = new SubContractDDrumdetails();

                tda.drumno = dtEnq.Rows[i]["DRUMNO"].ToString();
                tda.qty = dtEnq.Rows[i]["BQTY"].ToString();
               
                tda.lotno = dtEnq.Rows[0]["TLOT"].ToString();
                tda.rate = dtEnq.Rows[0]["BRATE"].ToString();
                tda.amount = dtEnq.Rows[0]["BAMOUNT"].ToString();
                //tda.invid = dtEnq.Rows[i]["PLotmastID"].ToString();
                TData.Add(tda);
            }
            ca.SUBDDrumlst = TData;
            return View(ca);
        }
        public ActionResult SubContractDrumSelection(string itemid, string rowid, string loc ,string type)
        {
            SubContractDDDrumdetailstable ca = new SubContractDDDrumdetailstable();
            List<SubContractDDrumdetails> TData = new List<SubContractDDrumdetails>();
            SubContractDDrumdetails tda = new SubContractDDrumdetails();
            DataTable dtEnq = new DataTable();
            type = datatrans.GetDataString("SELECT LOTYN FROM ITEMMASTER WHERE ITEMMASTERID='"+itemid+"'");
            if (type == "YES")
            {
                dtEnq = SubContractingDCService.GetSubContractDrumDetails(itemid, loc);
                for (int i = 0; i < dtEnq.Rows.Count; i++)
                {
                    tda = new SubContractDDrumdetails();

                    tda.drumno = dtEnq.Rows[i]["DRUMNO"].ToString();
                    tda.qty = dtEnq.Rows[i]["QTY"].ToString();
                    tda.reqqty = dtEnq.Rows[i]["QTY"].ToString();
                    //tda.stkid = dtEnq.Rows[i]["DRUM_STOCK_ID"].ToString();
                    

                    tda.lotno = dtEnq.Rows[0]["LOTNO"].ToString();
                    //tda.rate = dtEnq.Rows[0]["RATE"].ToString();
                    //tda.invid = dtEnq.Rows[i]["PLotmastID"].ToString();
                    TData.Add(tda);
                }
            }
            else
            {
                
                dtEnq = datatrans.GetData("Select SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) as QTY,SS.RATE  from STOCKVALUE S,STOCKVALUE2 SS  where S.STOCKVALUEID=SS.STOCKVALUEID and S.LOCID='" + loc + "' AND S.ITEMID='" + itemid + "' HAVING SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) > 0 GROUP BY SS.RATE");
                if (dtEnq.Rows.Count > 0)
                {
                    for (int i = 0; i < dtEnq.Rows.Count; i++)
                    {
                        tda = new SubContractDDrumdetails();
                        //tda.invid = dtEnq.Rows[i]["INVENTORY_ITEM_ID"].ToString();
                        //tda.lotno = dtEnq.Rows[i]["LOT_NO"].ToString();
                        tda.qty = dtEnq.Rows[i]["QTY"].ToString();
                        tda.reqqty = dtEnq.Rows[i]["QTY"].ToString();
                        tda.rate = dtEnq.Rows[i]["RATE"].ToString();



                        TData.Add(tda);
                    }
                }
            }
            ca.SUBDDrumlst = TData;
            return View(ca);
        }
      
        public IActionResult ViewSubContractingDC(string id)
        {
            SubContractingDC st = new SubContractingDC();
            DataTable dt = new DataTable();
            dt = SubContractingDCService.GetSubViewDeatils(id);
            if (dt.Rows.Count > 0)
            {
                st.ID = id;
                st.Branch = dt.Rows[0]["BRANCHID"].ToString();
                st.DocId = dt.Rows[0]["DOCID"].ToString();
                st.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                //st.Suplst = BindSupplier();
                st.Supplier = dt.Rows[0]["PARTYNAME"].ToString();
                st.Add1 = dt.Rows[0]["ADD1"].ToString();
                st.Add2 = dt.Rows[0]["ADD2"].ToString();
                st.City = dt.Rows[0]["CITY"].ToString();
                st.Location = dt.Rows[0]["LOCID"].ToString();
                st.Through = dt.Rows[0]["THROUGH"].ToString();
                st.Entered = dt.Rows[0]["ENTEREDBY"].ToString();
                st.TotalQty = dt.Rows[0]["TOTQTY"].ToString();
                st.Narration = dt.Rows[0]["NARRATION"].ToString();
                //ca.Net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());

            }
            List<SubContractingItem> TData = new List<SubContractingItem>();
            SubContractingItem tda = new SubContractingItem();
            DataTable dt2 = new DataTable();
            dt2 = SubContractingDCService.GetSubContractViewDetails(id);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new SubContractingItem();

                    tda.Itemlst = BindItemlst("");
                    //tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();
                    tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                    tda.Unit = dt2.Rows[i]["UNIT"].ToString();
                    tda.detid = dt2.Rows[i]["SUBCONTDCDETAILID"].ToString();
                    tda.Quantity = dt2.Rows[i]["QTY"].ToString();
                    tda.rate = dt2.Rows[i]["RATE"].ToString();
                    tda.Amount = dt2.Rows[i]["AMOUNT"].ToString();
                  
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            List<ReceiptDetailItem> TData1 = new List<ReceiptDetailItem>();
            ReceiptDetailItem tda1 = new ReceiptDetailItem();
            DataTable dt3 = new DataTable();
            dt3 = SubContractingDCService.GetReceiptViewDetail(id);
            if (dt3.Rows.Count > 0)
            {
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    tda1 = new ReceiptDetailItem();

                    tda1.Itemlist = BindItemlst("");
                    //tda1.saveItemId = dt3.Rows[i]["ITEMID"].ToString();
                    tda1.ItemId = dt3.Rows[i]["ITEMID"].ToString();
                    tda1.Unit = dt3.Rows[i]["RUNIT"].ToString();
                    tda1.Quantity = dt3.Rows[i]["ERQTY"].ToString();
                    tda1.rate = dt3.Rows[i]["ERATE"].ToString();
                    tda1.Amount = dt3.Rows[i]["EAMOUNT"].ToString();
                    tda1.Isvalid1 = "Y";
                    TData1.Add(tda1);
                }
            }
            List<PackMatItem> TData2 = new List<PackMatItem>();
            PackMatItem tda2 = new PackMatItem();
            DataTable dt4 = new DataTable();
            string baid = datatrans.GetDataString("Select RDELBASICID from RDELBASIC where DCREFID='" + id + "'");
            dt4 = SubContractingDCService.GetPackMatViewDetail(baid);
            DataTable dcno = datatrans.GetData("Select DOCID,to_char(DOCDATE,'dd-MM-yy')DOCDATE from RDELBASIC where RDELBASICID='" + baid + "'");
            if (dcno.Rows.Count > 0)
            {
                st.NDcNo =dcno.Rows[0]["DOCID"].ToString();
                st.dcDate =dcno.Rows[0]["DOCDATE"].ToString();
            }
            if (dt4.Rows.Count > 0)
            {
                for (int i = 0; i < dt4.Rows.Count; i++)
                {
                    tda2 = new PackMatItem();

                   
                    //tda1.saveItemId = dt3.Rows[i]["ITEMID"].ToString();
                    tda2.ItemId = dt4.Rows[i]["ITEMID"].ToString();
                    tda2.Unit = dt4.Rows[i]["UNIT"].ToString();
                    tda2.Quantity = dt4.Rows[i]["QTY"].ToString();
                    tda2.rate = dt4.Rows[i]["RATE"].ToString();
                    tda2.Amount = dt4.Rows[i]["AMOUNT"].ToString();
                    tda2.stock = dt4.Rows[i]["CLSTOCK"].ToString();

                    tda2.Isvalid = "Y";
                    TData2.Add(tda2);
                }
            }

            st.SCDIlst = TData;
            st.RECDlst = TData1;
            st.packlst = TData2;
            return View(st);
        }
        public IActionResult ApproveSubContractingDC(string id)
        {
            SubContractingDC st = new SubContractingDC();
            DataTable dt = new DataTable();
            dt = SubContractingDCService.GetSubViewDeatils(id);
            if (dt.Rows.Count > 0)
            {
                st.ID = id;
                st.Branch = dt.Rows[0]["BRANCHID"].ToString();
                st.Branchid = dt.Rows[0]["BRANCH"].ToString();
                st.DocId = dt.Rows[0]["DOCID"].ToString();
                st.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                //st.Suplst = BindSupplier();
                st.Supplier = dt.Rows[0]["PARTYNAME"].ToString();
                st.party = dt.Rows[0]["PARTYID"].ToString();
                st.Add1 = dt.Rows[0]["ADD1"].ToString();
                st.Add2 = dt.Rows[0]["ADD2"].ToString();
                st.City = dt.Rows[0]["CITY"].ToString();
                st.Location = dt.Rows[0]["LOCID"].ToString();
                st.Locationid = dt.Rows[0]["loc"].ToString();
                st.Through = dt.Rows[0]["THROUGH"].ToString();
                st.Entered = dt.Rows[0]["ENTEREDBY"].ToString();
                st.TotalQty = dt.Rows[0]["TOTQTY"].ToString();
                st.Narration = dt.Rows[0]["NARRATION"].ToString();
                st.Enterd = Request.Cookies["UserId"];
                //ca.Net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());

            }
            List<SubContractingItem> TData = new List<SubContractingItem>();
            SubContractingItem tda = new SubContractingItem();
            DataTable dt2 = new DataTable();
            dt2 = SubContractingDCService.GetSubContractViewDetails(id);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new SubContractingItem();

                    tda.Itemlst = BindItemlst("");
                    //tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();
                    tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                    tda.item = dt2.Rows[i]["item"].ToString();
                    tda.Unit = dt2.Rows[i]["UNIT"].ToString();
                    tda.Quantity = dt2.Rows[i]["QTY"].ToString();
                    tda.rate = dt2.Rows[i]["RATE"].ToString();
                    tda.Amount = dt2.Rows[i]["AMOUNT"].ToString();
                    tda.detid = dt2.Rows[i]["SUBCONTDCDETAILID"].ToString();
                    DataTable drum = datatrans.GetData("Select BITEMID,TLOT,DRUMNO,BQTY,BRATE,BAMOUNT from SUBCONTDCBATCH where PARENTRECORDID='" + tda.detid + "'");
                    if (drum.Rows.Count > 0)
                    {
                        for (int j = 0; j < drum.Rows.Count; j++)
                        {
                            tda.Drumsdesc = drum.Rows[j]["DRUMNO"].ToString();
                            tda.dqty = drum.Rows[j]["BQTY"].ToString();
                            tda.drate = drum.Rows[j]["BRATE"].ToString();
                            tda.Lotno = drum.Rows[j]["TLOT"].ToString();
                        }

                    }
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            List<ReceiptDetailItem> TData1 = new List<ReceiptDetailItem>();
            ReceiptDetailItem tda1 = new ReceiptDetailItem();
            DataTable dt3 = new DataTable();
            dt3 = SubContractingDCService.GetReceiptViewDetail(id);
            if (dt3.Rows.Count > 0)
            {
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    tda1 = new ReceiptDetailItem();

                    tda1.Itemlist = BindItemlst("");
                    //tda1.saveItemId = dt3.Rows[i]["ITEMID"].ToString();
                    tda1.ItemId = dt3.Rows[i]["ITEMID"].ToString();
                    tda1.item = dt3.Rows[i]["RITEM"].ToString();
                    tda1.detid = dt3.Rows[i]["SUBCONTEDETID"].ToString();
                    tda1.Unit = dt3.Rows[i]["RUNIT"].ToString();
                    tda1.Quantity = dt3.Rows[i]["ERQTY"].ToString();
                    tda1.rate = dt3.Rows[i]["ERATE"].ToString();
                    tda1.Amount = dt3.Rows[i]["EAMOUNT"].ToString();
                    tda1.Isvalid1 = "Y";
                    TData1.Add(tda1);
                }
            }


            st.SCDIlst = TData;
            st.RECDlst = TData1;
            return View(st);
        }
        [HttpPost]
        public ActionResult ApproveSubContractingDC(SubContractingDC ss, string id)
        {

            try
            {
                ss.ID = id;
                string Strout = SubContractingDCService.ApproveSubContractingDCCRUD(ss);
                if (string.IsNullOrEmpty(Strout))
                { 
                        TempData["notice"] = " SubContractingDC Approved Successfully...!";
                    
                    return RedirectToAction("ListSubContractingDC");
                }

                else
                {
                    ViewBag.PageTitle = "Edit SubContractingDC";
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
        public IActionResult PackingMatSubContractingDC(string id)
        {
            SubContractingDC st = new SubContractingDC();
            DataTable dt = new DataTable();
            st.Loc = BindLocation();
            dt = SubContractingDCService.GetSubViewDeatils(id);
            if (dt.Rows.Count > 0)
            {
                st.pakid = id;
                st.Branch = dt.Rows[0]["BRANCHID"].ToString();
                st.Branchid = dt.Rows[0]["BRANCH"].ToString();
                st.DocId = dt.Rows[0]["DOCID"].ToString();
                st.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
                //st.Suplst = BindSupplier();
                st.Supplier = dt.Rows[0]["PARTYNAME"].ToString();
                st.party = dt.Rows[0]["PARTYID"].ToString();
                st.Add1 = dt.Rows[0]["ADD1"].ToString();
                st.Add2 = dt.Rows[0]["ADD2"].ToString();
                st.City = dt.Rows[0]["CITY"].ToString();
                st.Location = dt.Rows[0]["LOCID"].ToString();
                st.Locationid = dt.Rows[0]["loc"].ToString();
               
                st.Entered = dt.Rows[0]["ENTEREDBY"].ToString();
                st.TotalQty = dt.Rows[0]["TOTQTY"].ToString();
                st.Narration = dt.Rows[0]["NARRATION"].ToString();
                st.Enterd = Request.Cookies["UserId"];
                //ca.Net = Convert.ToDouble(dt.Rows[0]["GetItemGrpJSON"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());

            }
            List<PackMatItem> TData3 = new List<PackMatItem>();
            PackMatItem tda3 = new PackMatItem();

            for (int i = 0; i < 1; i++)
            {
                tda3 = new PackMatItem();
                tda3.Itemlst = BindPackItemlst();
                tda3.Isvalid = "Y";
                TData3.Add(tda3);
            }
            List<SubContractingItem> TData = new List<SubContractingItem>();
            SubContractingItem tda = new SubContractingItem();
            DataTable dt2 = new DataTable();
            dt2 = SubContractingDCService.GetSubContractViewDetails(id);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new SubContractingItem();

                    tda.Itemlst = BindItemlst("");
                    //tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();
                    tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                    tda.item = dt2.Rows[i]["item"].ToString();
                    tda.Unit = dt2.Rows[i]["UNIT"].ToString();
                    tda.Quantity = dt2.Rows[i]["QTY"].ToString();
                    tda.rate = dt2.Rows[i]["RATE"].ToString();
                    tda.Amount = dt2.Rows[i]["AMOUNT"].ToString();
                    tda.detid = dt2.Rows[i]["SUBCONTDCDETAILID"].ToString();
                    DataTable drum = datatrans.GetData("Select BITEMID,TLOT,DRUMNO,BQTY,BRATE,BAMOUNT from SUBCONTDCBATCH where PARENTRECORDID='" + tda.detid + "'");
                    if (drum.Rows.Count > 0)
                    {
                        for (int j = 0; j < drum.Rows.Count; j++)
                        {
                            tda.Drumsdesc = drum.Rows[j]["DRUMNO"].ToString();
                            tda.dqty = drum.Rows[j]["BQTY"].ToString();
                            tda.drate = drum.Rows[j]["BRATE"].ToString();
                            tda.Lotno = drum.Rows[j]["TLOT"].ToString();
                        }

                    }
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            List<ReceiptDetailItem> TData1 = new List<ReceiptDetailItem>();
            ReceiptDetailItem tda1 = new ReceiptDetailItem();
            DataTable dt3 = new DataTable();
            dt3 = SubContractingDCService.GetReceiptViewDetail(id);
            if (dt3.Rows.Count > 0)
            {
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    tda1 = new ReceiptDetailItem();

                    tda1.Itemlist = BindItemlst("");
                    //tda1.saveItemId = dt3.Rows[i]["ITEMID"].ToString();
                    tda1.ItemId = dt3.Rows[i]["ITEMID"].ToString();
                    tda1.item = dt3.Rows[i]["RITEM"].ToString();
                    tda1.detid = dt3.Rows[i]["SUBCONTEDETID"].ToString();
                    tda1.Unit = dt3.Rows[i]["RUNIT"].ToString();
                    tda1.Quantity = dt3.Rows[i]["ERQTY"].ToString();
                    tda1.rate = dt3.Rows[i]["ERATE"].ToString();
                    tda1.Amount = dt3.Rows[i]["EAMOUNT"].ToString();
                    tda1.Isvalid1 = "Y";
                    TData1.Add(tda1);
                }
            }


            st.SCDIlst = TData;
            st.RECDlst = TData1;
            st.packlst = TData3;
            return View(st);
        }
        [HttpPost]
        public ActionResult PackingMatSubContractingDC(SubContractingDC ss, string id)
        {

            try
            {
                ss.ID = id;
                string Strout = SubContractingDCService.PackMatSubConDCCRUD(ss);
                if (string.IsNullOrEmpty(Strout))
                {
                    TempData["notice"] = " SubContractingDC Packing Mat Approved Successfully...!";

                    return RedirectToAction("ListSubContractingDC");
                }

                else
                {
                    ViewBag.PageTitle = "Edit SubContractingDC";
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

        public async Task<IActionResult> SubConDcRec(string id)
        {

            string mimtype = "";
            int extension = 1;
            

            System.Data.DataSet ds = new System.Data.DataSet();
            var path = $"{this._WebHostEnvironment.WebRootPath}\\Reports\\SubConDc.rdlc";
            Dictionary<string, string> Parameters = new Dictionary<string, string>();
            //  Parameters.Add("rp1", " Hi Everyone");
            var subcondc = await SubContractingDCService.GetSubcondc(id);
            var subcondcDet = await SubContractingDCService.GetSubcondcdet(id);

            AspNetCore.Reporting.LocalReport localReport = new AspNetCore.Reporting.LocalReport(path);
            localReport.AddDataSource("SubConDc", subcondc);
            localReport.AddDataSource("SubConDet", subcondcDet);
            //localReport.AddDataSource("DataSet1_DataTable1", po);
            var result = localReport.Execute(RenderType.Pdf, extension, Parameters, mimtype);

            return File(result.MainStream, "application/Pdf");
            
        }
    }
}
