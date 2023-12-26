using Arasan.Interface;
using Arasan.Interface.Store_Management;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Qualitycontrol;
using Arasan.Services.Store_Management;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Reflection;
using System.Xml.Linq;

namespace Arasan.Controllers.Store_Management
{
    public class SubContractingMaterialReceiptController : Controller
    {
        ISubContractingMaterialReceipt  SubContractingMaterialReceiptService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public SubContractingMaterialReceiptController(ISubContractingMaterialReceipt _SubContractingMaterialReceiptService, IConfiguration _configuratio)
        {
            SubContractingMaterialReceiptService = _SubContractingMaterialReceiptService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult SubContractingMaterialReceipt(string id)
        {
            SubContractingMaterialReceipt ca = new SubContractingMaterialReceipt();
            ca.Loc = BindLocation();
            ca.Brlst = BindBranch();
            ca.Suplst = BindSupplier("");
            ca.assignList = BindEmp();
            ca.DClst = BindDC();
            ca.Enterd = Request.Cookies["UserId"];
            ca.Branch = Request.Cookies["BranchId"];
            ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("submr");
            if (dtv.Rows.Count > 0)
            {
                ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }

            List<SubMaterialItem> TData = new List<SubMaterialItem>();
            SubMaterialItem tda = new SubMaterialItem();
            List<SubContractItem> TData1 = new List<SubContractItem>();
            SubContractItem tda1 = new SubContractItem();

            for (int i = 0; i < 1; i++)
            {
                tda = new SubMaterialItem();

                tda.Itemlst = BindItemlst();

                tda.Isvalid = "Y";
                TData.Add(tda);
            }
            for (int i = 0; i < 1; i++)
            {
                tda1 = new SubContractItem();
                tda1.Itemlst = BindItemlst();
                tda1.Isvalid = "Y";
                TData1.Add(tda1);
            }
            ca.Contlilst = TData1;
            ca.SubMatlilst = TData;

            return View(ca);
        }
        [HttpPost]
        public ActionResult SubContractingMaterialReceipt(SubContractingMaterialReceipt Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout =SubContractingMaterialReceiptService.SubContractingMaterialReceiptCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "SubContractingMaterialReceipt Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "SubContractingMaterialReceipt Updated Successfully...!";
                    }
                    return RedirectToAction("ListSubContractingMaterialReceipt");
                }

                else
                {
                    ViewBag.PageTitle = "Edit SubContractingMaterialReceipt";
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
        public IActionResult ListSubContractingMaterialReceipt()
        {
            //IEnumerable<DirectPurchase> cmp = directPurchase.GetAllDirectPur(status);
            return View();
        }
        public JsonResult GetItemJSON()
        {
            SubMaterialItem model = new SubMaterialItem();
            model.Itemlst = BindItemlst();
            return Json(BindItemlst());

        }
        public JsonResult GetItemDelJSON()
        {
            SubContractItem model = new SubContractItem();
            model.Itemlst = BindItemlst();
            return Json(BindItemlst());

        }
        public ActionResult MyListSubContractingMaterialReceiptGrid(string strStatus)
        {
            List<MaterialRecItem> Reg = new List<MaterialRecItem>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)SubContractingMaterialReceiptService.GetAllSubContractingMaterialItem(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string DeleteRow = string.Empty;
                string ViewPen = string.Empty;
                string View = string.Empty;


                ViewPen = "<a href=ViewPendingSub?id=" + dtUsers.Rows[i]["SUBMRBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='Edit' /></a>";
                View = "<a href=ViewSub?id=" + dtUsers.Rows[i]["SUBMRBASICID"].ToString() + "><img src='../Images/view_icon.png' alt='Edit' /></a>";
                DeleteRow = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["SUBMRBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";



                Reg.Add(new MaterialRecItem
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["SUBMRBASICID"].ToString()),

                    supplier = dtUsers.Rows[i]["PARTYNAME"].ToString(),
                    docNo = dtUsers.Rows[i]["DOCID"].ToString(),
                    docDate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    loc = dtUsers.Rows[i]["LOCID"].ToString(),
                    viewpen = ViewPen,
                    view = View,

                    delrow = DeleteRow,



                });
            }

            return Json(new
            {
                Reg
            });

        }
        public JsonResult GetPartyJSON(string itemid)
        {
            SubContractingMaterialReceipt model = new SubContractingMaterialReceipt();
            model.Suplst = BindSupplier(itemid);
            return Json(BindSupplier(itemid));

        }
        public List<SelectListItem> BindDC()
        {
            try
            {
                DataTable dtDesg = SubContractingMaterialReceiptService.GetDC();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["SUBCONTDCBASICID"].ToString() });
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
                DataTable dtDesg = datatrans.GetItem();
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
        public List<SelectListItem> BindSupplier(string id)
        {
            try
            {
                DataTable dtDesg = SubContractingMaterialReceiptService.GetSupplier(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTYNAME"].ToString(), Value = dtDesg.Rows[i]["PARTYID"].ToString() });
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
        public ActionResult GetWCDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string work = "";
               
                dt = SubContractingMaterialReceiptService.GetWCDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    work = dt.Rows[0]["WCID"].ToString();
                  

                }

                var result = new { work = work};
                return Json(result);
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
                string price = "";
                string lot = "";
                dt = SubContractingMaterialReceiptService.GetItemDetails(ItemId);
               
                if (dt.Rows.Count > 0)
                {

                    unit = dt.Rows[0]["UNITID"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();
                    lot = dt.Rows[0]["LOTYN"].ToString();
                }

                var result = new { unit = unit, price = price, lot = lot };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetItem(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string unitid = "";
                string rate = "";
                

                dt = SubContractingMaterialReceiptService.GetItems(ItemId);

                if (dt.Rows.Count > 0)
                {

                    unitid = dt.Rows[0]["UNITID"].ToString();
                    rate = dt.Rows[0]["LATPURPRICE"].ToString();
                

                }

                var result = new { unitid = unitid, rate = rate  };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindDrum()
        {
            try
            {
                DataTable dtDesg = SubContractingMaterialReceiptService.GetDrum();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DRUMNO"].ToString(), Value = dtDesg.Rows[i]["DRUMMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult DrumSelection(string id,string rowid)
        {
            SubContractingMaterialReceipt ca = new SubContractingMaterialReceipt();
            List<DrumItemDeatil> TData = new List<DrumItemDeatil>();
            DrumItemDeatil tda = new DrumItemDeatil();

            DataTable dt2 = new DataTable();


            dt2 = SubContractingMaterialReceiptService.GetDrumItemDetails(id);
            if (dt2.Rows.Count > 0)
            {

                ca.item = dt2.Rows[0]["ITEMID"].ToString();
                ca.itemid = dt2.Rows[0]["item"].ToString();
                ca.qty = dt2.Rows[0]["QTY"].ToString();
                ca.rate = dt2.Rows[0]["RATE"].ToString();
            }

            for (int i = 0; i < 1; i++)
            {
                tda = new DrumItemDeatil();
                tda.drulist = BindDrum();
                tda.ID = id;
                tda.Isvalid = "Y";
                TData.Add(tda);
            }
            ca.drumlist = TData;
            return View(ca);



        }
        //public ActionResult GetDrumDetails(int ItemId, double rate, int qty)
        //{
        //    SubContractingMaterialReceipt ca = new SubContractingMaterialReceipt();
        //    List<DrumItemDeatil> TData = new List<DrumItemDeatil>();
        //    DrumItemDeatil tda = new DrumItemDeatil();

        //    int sqty = qty / ItemId;
        //    for (int i = 1; i <= sqty; i++)
        //    {
        //        tda = new DrumItemDeatil();

        //        tda.drulist = BindDrum();

        //        tda.qty = ItemId.ToString();
        //        tda.rate = rate.ToString();
        //        Double tamt = ItemId * rate;
        //        tda.amount = tamt.ToString();


        //        tda.Isvalid = "Y";

        //        TData.Add(tda);


        //    }


        //    ca.drumlist = TData;
        //    return Json(ca.drumlist);



        //}
        public ActionResult GetDrumDetails(int ItemId, int st, string pre)
        {
            SubContractingMaterialReceipt ca = new SubContractingMaterialReceipt();
            List<DrumItemDeatil> TData = new List<DrumItemDeatil>();
            DrumItemDeatil tda = new DrumItemDeatil();

            int count = ItemId - st;
            for (int i = 0; i <= count; i++)
            {
                tda = new DrumItemDeatil();


                int s = st;
                int legcode = Convert.ToInt32(s);
                //string code = GetNumberwithPrefix(legcode, 6);
                //int prefix = Convert.ToInt32(pre);
                tda.totaldrum = legcode.ToString();
                string drum = pre + "" + legcode;
                tda.drumno = drum.ToString();
                legcode++;
                st = legcode;
               


                TData.Add(tda);
            }

            ca.drumlist = TData;
            return Json(ca.drumlist);

        }

        //public static string GetNumberwithPrefix(int Ledgercode, int totalchar)
        //{
        //    string tempnumber = Ledgercode.ToString();
        //    while (tempnumber.Length < 6)
        //        tempnumber = "0" + tempnumber;
        //    return tempnumber;
        //}
        public JsonResult GetDrumJSON()
        {
            DrumItemDeatil model = new DrumItemDeatil();
            model.drulist = BindDrum();
            return Json(BindDrum());

        }
        public ActionResult GetSubDeliverItemDetail(string ItemId)
        {
            SubContractingMaterialReceipt ca = new SubContractingMaterialReceipt();
            List<SubMaterialItem> TData = new List<SubMaterialItem>();
            SubMaterialItem tda = new SubMaterialItem();

            DataTable dt2 = new DataTable();


            dt2 = SubContractingMaterialReceiptService.GetSubDelivItemDetails(ItemId);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new SubMaterialItem();
                    tda.item = dt2.Rows[i]["ITEMID"].ToString();
                    tda.itemid = dt2.Rows[i]["item"].ToString();
                    tda.unit = dt2.Rows[i]["UNIT"].ToString();
                    //tda.unitid = dt2.Rows[i]["UNITID"].ToString();
                    tda.qty = dt2.Rows[i]["QTY"].ToString();
                    tda.rate = dt2.Rows[i]["RATE"].ToString();
                    tda.amount = dt2.Rows[i]["AMOUNT"].ToString();
                    tda.supid = dt2.Rows[i]["SUBCONTDCDETAILID"].ToString();
                    tda.Isvalid = "Y";
                    tda.id = ItemId;
                    TData.Add(tda);


                }
            }

            ca.SubMatlilst = TData;
            return Json(ca.SubMatlilst);



        }
        public ActionResult GetSubRecivedDetail(string ItemId)
        {
            SubContractingMaterialReceipt ca = new SubContractingMaterialReceipt();
            List<SubContractItem> TData = new List<SubContractItem>();
            SubContractItem tda = new SubContractItem();

            DataTable dt2 = new DataTable();



            dt2 = SubContractingMaterialReceiptService.GetSubRecvItemDetails(ItemId);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new SubContractItem();




                    tda.item = dt2.Rows[i]["ITEMID"].ToString();
                    tda.itemid = dt2.Rows[i]["item"].ToString();

                    tda.unit = dt2.Rows[i]["UNITID"].ToString();
                    //tda.unitid = dt2.Rows[i]["UNITID"].ToString();
                    tda.qty = dt2.Rows[i]["BALANCE_QTY"].ToString();

                    tda.rate = dt2.Rows[i]["RATE"].ToString();
                    tda.amount = dt2.Rows[i]["AMOUNT"].ToString();
                    tda.detid = dt2.Rows[i]["TSOURCEID"].ToString();


                    tda.Isvalid = "Y";
                    tda.id = ItemId;
                    TData.Add(tda);


                }
            }

            ca.Contlilst = TData;
            return Json(ca.Contlilst);



        }
        public IActionResult ViewSub(string id)
        {
            SubContractingMaterialReceipt ca = new SubContractingMaterialReceipt();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();
            dt = SubContractingMaterialReceiptService.GetSubContract(id)
;
            if (dt.Rows.Count > 0)
            {
                
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.DocId = dt.Rows[0]["DOCID"].ToString();
                ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                ca.DCNo = dt.Rows[0]["T1SOURCEID"].ToString();
                ca.Supplier = dt.Rows[0]["PARTYNAME"].ToString();
                ca.WorkCenter = dt.Rows[0]["WCID"].ToString();
                ca.Location = dt.Rows[0]["LOCID"].ToString();
                ca.qtyrec = dt.Rows[0]["TOTRQTY"].ToString();
                ca.TotRecqty = dt.Rows[0]["TOTRCQTY"].ToString();
                ca.enterd = dt.Rows[0]["ENTEREDBY"].ToString();
                ca.RefNo = dt.Rows[0]["REFNO"].ToString();
                ca.RefDate = dt.Rows[0]["REFDATE"].ToString();
                ca.Narration = dt.Rows[0]["NARRATION"].ToString();
                ca.ID = id;
            }
            List<SubMaterialItem> TData = new List<SubMaterialItem>();
            SubMaterialItem tda = new SubMaterialItem();
           
            dtt = SubContractingMaterialReceiptService.GetMaterialReceipt(id)
;
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new SubMaterialItem();
                    tda.item = dtt.Rows[i]["ITEMID"].ToString();
                    tda.unit = dtt.Rows[i]["MUNIT"].ToString();
                    tda.qty = dtt.Rows[i]["MSUBQTY"].ToString();
                    tda.rate = dtt.Rows[i]["MRRATE"].ToString();
                    tda.amount = dtt.Rows[i]["MRAMOUNT"].ToString();
                    tda.supid = dtt.Rows[i]["SUBACTMRDETID"].ToString();
                    TData.Add(tda);
                }
            }
            List<SubContractItem> TData1 = new List<SubContractItem>();
            SubContractItem tda1 = new SubContractItem();

            dtt = SubContractingMaterialReceiptService.GetReceiptItem(id)
;
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda1 = new SubContractItem();
                    tda1.item = dtt.Rows[i]["ITEMID"].ToString();
                    tda1.unit = dtt.Rows[i]["UNIT"].ToString();
                    tda1.qty = dtt.Rows[i]["RECQTY"].ToString();
                    tda1.rate = dtt.Rows[i]["COSTRATE"].ToString();
                    tda1.amount = dtt.Rows[i]["AMOUNT"].ToString();

                    TData1.Add(tda1);
                }
            }

            ca.Contlilst = TData1;
            ca.SubMatlilst = TData;
            return View(ca);
        }
        public IActionResult ViewDrum(string id)
        {
            SubContractingMaterialReceipt ca = new SubContractingMaterialReceipt();
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            List<DrumItemDeatil> TData = new List<DrumItemDeatil>();
            DrumItemDeatil tda = new DrumItemDeatil();
            dt = SubContractingMaterialReceiptService.GetDrumdetails(id)
;
            dt2 = SubContractingMaterialReceiptService.GetMaterialReceipt(id);
            if (dt2.Rows.Count > 0)
            {

                ca.item = dt2.Rows[0]["ITEMID"].ToString();

                ca.qty = dt2.Rows[0]["MSUBQTY"].ToString();

            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                tda = new DrumItemDeatil();
                tda.drumno = dt.Rows[i]["ACTUALDRUM"].ToString();
                tda.qty = dt.Rows[i]["MLQTY"].ToString();
                tda.rate = dt.Rows[i]["MLRATE"].ToString();
                tda.amount = dt.Rows[i]["MLAMOUNT"].ToString();

                TData.Add(tda);
            }
            ca.drumlist = TData;


            return View(ca);
        }
        public IActionResult ViewPendingSub(string id)
        {
            SubContractingMaterialReceipt ca = new SubContractingMaterialReceipt();
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            List<pendingitem> TData = new List<pendingitem>();
            pendingitem tda = new pendingitem();
            string coid = datatrans.GetDataString("Select SUBCONTEDETID from SUBMRDETAIL where SUBMRBASICID='" + id + "'");

            dt = datatrans.GetData("Select ITEMMASTER.ITEMID,DCQTY,PENDQTY,RECQTY,SUBMRBASICID from SUBMRDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=SUBMRDETAIL.FGITEMID  where SUBCONTEDETID='" + coid + "'");

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tda = new pendingitem();
                    tda.item = dt.Rows[i]["ITEMID"].ToString();
                    tda.baid = dt.Rows[i]["SUBMRBASICID"].ToString();
                    tda.qty = dt.Rows[i]["DCQTY"].ToString();
                    tda.penqty = dt.Rows[i]["PENDQTY"].ToString();
                    tda.recqty = dt.Rows[i]["RECQTY"].ToString();

                    dt2 = datatrans.GetData("Select DOCID,to_char(DOCDATE,'dd-MM-yy')DOCDATE from SUBMRBASIC  where SUBMRBASICID='" + tda.baid + "'");
                    if (dt2.Rows.Count > 0)
                    {
                        tda.docNo = dt2.Rows[0]["DOCID"].ToString();
                        tda.docDate = dt2.Rows[0]["DOCDATE"].ToString();
                    }
                    TData.Add(tda);
                }
            }
            ca.penlst = TData;


            return View(ca);
        }
        public IActionResult SubMatReceipt(string id)
        {
            SubContractingMaterialReceipt ca = new SubContractingMaterialReceipt();
            ca.Loc = BindLocation();
            ca.Brlst = BindBranch();
            ca.Suplst = BindSupplier("");
            ca.assignList = BindEmp();
            ca.DClst = BindDC();
            ca.Enterd = Request.Cookies["UserId"];
            ca.Branch = Request.Cookies["BranchId"];
            ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("submr");
            if (dtv.Rows.Count > 0)
            {
                ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }

            List<SubMaterialItem> TData = new List<SubMaterialItem>();
            SubMaterialItem tda = new SubMaterialItem();
            List<SubContractItem> TData1 = new List<SubContractItem>();
            SubContractItem tda1 = new SubContractItem();

            for (int i = 0; i < 1; i++)
            {
                tda = new SubMaterialItem();

                tda.Itemlst = BindItemlst();

                tda.Isvalid = "Y";
                TData.Add(tda);
            }
            for (int i = 0; i < 1; i++)
            {
                tda1 = new SubContractItem();
                tda1.Itemlst = BindItemlst();
                tda1.Isvalid = "Y";
                TData1.Add(tda1);
            }
            ca.Contlilst = TData1;
            ca.SubMatlilst = TData;

            return View(ca);
        }
    }
}
