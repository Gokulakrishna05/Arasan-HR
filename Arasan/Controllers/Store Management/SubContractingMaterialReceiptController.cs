using Arasan.Interface;
using Arasan.Interface.Store_Management;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Qualitycontrol;
using Arasan.Services.Store_Management;
using AspNetCore.Reporting;
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
        private readonly IWebHostEnvironment _WebHostEnvironment;
        DataTransactions datatrans;
        public SubContractingMaterialReceiptController(ISubContractingMaterialReceipt _SubContractingMaterialReceiptService, IConfiguration _configuratio, IWebHostEnvironment WebHostEnvironment)
        {
            SubContractingMaterialReceiptService = _SubContractingMaterialReceiptService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
            this._WebHostEnvironment = WebHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }
        public IActionResult SubContractingMaterialReceipt(string id)
        {
            SubContractingMaterialReceipt ca = new SubContractingMaterialReceipt();
            ca.Loc = BindLocation();
            ca.Brlst = BindBranch();
            ca.Suplst = BindSupplier();
            ca.assignList = BindEmp();
            ca.DClst = BindDC();
            ca.Enterd = Request.Cookies["UserId"];
            ca.Branch = Request.Cookies["BranchId"];
            ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("submr");
            if (dtv.Rows.Count > 0)
            {
                ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + "" + dtv.Rows[0]["last"].ToString();
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
                tda1.Itemlst = BindItemlist("");
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
         
        public JsonResult GetItemDelJSON(string ItemId)
        {
            SubContractItem model = new SubContractItem();
            model.Itemlst = BindItemlist(ItemId);
            return Json(BindItemlist(ItemId));

        }
        public List<SelectListItem> BindItemlist(string id)
        {
            try
            {
                DataTable dtDesg = SubContractingMaterialReceiptService.GetPartyItem(id);
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
                string recept = string.Empty;
                string Account = string.Empty;

                recept = "<a href=SubConMatDcRec?id=" + dtUsers.Rows[i]["SUBMRBASICID"].ToString() + " target='_blank'><img src='../Images/pdficon.png' alt='View Details' width='20' /></a>";
                Account = "<a href=SubMatAccount?id=" + dtUsers.Rows[i]["SUBMRBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/profit.png' alt='View Details' width='20' /></a>";

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
                    recept = recept,
                    account = Account,

                    delrow = DeleteRow,



                });
            }

            return Json(new
            {
                Reg
            });

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
        public List<SelectListItem> BindSupplier()
        {
            try
            {
                DataTable dtDesg = SubContractingMaterialReceiptService.GetSupplier();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTYID"].ToString(), Value = dtDesg.Rows[i]["PartyMASTID"].ToString() });
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


            string item = datatrans.GetDataString("select WIPITEMID from WCBASIC where PARTYID='" + id + "'");
           
            string rate = datatrans.GetDataString("select ERATE from SUBCONTEDET where RITEM='" + item + "'ORDER BY SUBCONTEDETID DESC ");
            //if (dt2.Rows.Count > 0)
            //{

            //    ca.item = dt2.Rows[0]["ITEMID"].ToString();
            //    ca.itemid = dt2.Rows[0]["item"].ToString();
            //    ca.qty = dt2.Rows[0]["QTY"].ToString();
            //    ca.rate = dt2.Rows[0]["RATE"].ToString();
            //}

            for (int i = 0; i < 1; i++)
            {
                tda = new DrumItemDeatil();
                tda.drulist = BindDrum();
                //tda.ID = id;
                tda.drumrate = rate;
                tda.Isvalid = "Y";
                tda.uniqueid = "pre-" + i;
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
        public ActionResult GetDrumDetails([FromBody] DrumItemDeatil[] model)
        {
            SubContractingMaterialReceipt ca = new SubContractingMaterialReceipt();
            List<DrumItemDeatil> TData = new List<DrumItemDeatil>();
            DrumItemDeatil tda = new DrumItemDeatil();
            foreach (DrumItemDeatil emp in model)
            {
                int st = Convert.ToInt32(emp.Start);
                int ed = Convert.ToInt32(emp.End);
                string prifix = emp.Pri;
                string qty = emp.drumqty;
                int count = ed - st;
                for (int i = 0; i <= count; i++)
                {
                    tda = new DrumItemDeatil();


                    int s = st;
                    int legcode = Convert.ToInt32(s);
                    //string code = GetNumberwithPrefix(legcode, 6);
                    //int prefix = Convert.ToInt32(pre);
                    tda.totaldrum = legcode.ToString();
                    string drum = prifix + "" + legcode;
                    tda.drumno = drum.ToString();
                    tda.prefix = prifix;
                    tda.qty = qty;
                    legcode++;
                    st = legcode;



                    TData.Add(tda);
                }
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
                tda.item = dt.Rows[i]["ITEMID"].ToString();
               
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

            if(coid != "0")
            {
                dt = datatrans.GetData("Select ITEMMASTER.ITEMID,DCQTY,PENDQTY,RECQTY,SUBMRBASICID from SUBMRDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=SUBMRDETAIL.FGITEMID  where SUBCONTEDETID='" + coid + "'");
            }
           

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
            ca.Suplst = BindSupplier();
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
        public async Task<IActionResult> SubConMatDcRec(string id)
        {

            string mimtype = "";
            int extension = 1;


            System.Data.DataSet ds = new System.Data.DataSet();
            var path = $"{this._WebHostEnvironment.WebRootPath}\\Reports\\SubConMatDc.rdlc";
            Dictionary<string, string> Parameters = new Dictionary<string, string>();
            //  Parameters.Add("rp1", " Hi Everyone");
            var subcondc = await SubContractingMaterialReceiptService.GetSubMrdc(id);
            var subcondcDet = await SubContractingMaterialReceiptService.GetSubMrdcdet(id);
            var subActcondcDet = await SubContractingMaterialReceiptService.GetSubActMrdcdet(id);

            AspNetCore.Reporting.LocalReport localReport = new AspNetCore.Reporting.LocalReport(path);
            localReport.AddDataSource("SubMat", subcondc);
            localReport.AddDataSource("SubMatDet", subcondcDet);
            localReport.AddDataSource("SubActMr", subActcondcDet);
           
            var result = localReport.Execute(RenderType.Pdf, extension, Parameters, mimtype);

            return File(result.MainStream, "application/Pdf");

        }
        public IActionResult SubMatAccount(string id)
        {
            SubContractingMaterialReceipt grn = new SubContractingMaterialReceipt();
            DataTable dt = new DataTable();
            dt = SubContractingMaterialReceiptService.FetchAccountRec(id);
            grn.Subid = id;
            grn.RefDate = DateTime.Now.ToString("dd-MMM-yyyy");
            grn.Enterd = Request.Cookies["UserId"];
            DataTable dtnat = datatrans.GetData("select I.ITEMID,BL.QTY,U.UNITID from GRNBLDETAIL BL,ITEMMASTER I,UNITMAST U where I.ITEMMASTERID=BL.ITEMID AND U.UNITMASTID=I.PRIUNIT AND GRNBLBASICID='" + id + "'");
            //grn.Vmemo = "BEING " + dtnat.Rows[0]["ITEMID"].ToString() + "-" + dtnat.Rows[0]["QTY"].ToString() + dtnat.Rows[0]["UNITID"].ToString() + "PURCHASED.";
            List<SubMatAccount> TData = new List<SubMatAccount>();
            SubMatAccount tda = new SubMatAccount();
            double totalcredit = 0;
            double totaldebit = 0;
            DataTable dtdet = datatrans.GetData("select I.ITEMACC,(BRATE*MRQTY)  as GROSS,G.MITEMID from SUBACTMRDET G,ITEMMASTER I  where G.MITEMID=I.ITEMMASTERID AND G.SUBMRBASICID='" + id + "' ");
            DataTable dtacc = new DataTable();
            dtacc = datatrans.GetGRNconfig();
            string frieghtledger = "";
            string discledger = "";
            string roundoffledger = "";
            string cgstledger = "";
            string sgstledger = "";
            string igstledger = "";
            string packingledger = "";
            if (dtacc.Rows.Count > 0)
            {
                grn.ADCOMPHID = dtacc.Rows[0]["ADCOMPHID"].ToString();
                for (int i = 0; i < dtacc.Rows.Count; i++)
                {
                    
                    if (dtacc.Rows[i]["ADTYPE"].ToString().Contains("CGST"))
                    {
                        cgstledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                    }
                    if (dtacc.Rows[i]["ADTYPE"].ToString().Contains("SGST"))
                    {
                        sgstledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                    }
                    if (dtacc.Rows[i]["ADTYPE"].ToString().Contains("IGST"))
                    {
                        igstledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                    }
                }
            }
            if (dt.Rows.Count > 0)
            {

               
                   

                     
                    grn.Gross = Convert.ToDouble(dt.Rows[0]["amount"].ToString() == "" ? "0" : dt.Rows[0]["amount"].ToString());
                grn.Net = Convert.ToDouble(dt.Rows[0]["amount"].ToString() == "" ? "0" : dt.Rows[0]["amount"].ToString());


                    //grn.CGST = Convert.ToDouble(dt.Rows[0]["CGST"].ToString() == "" ? "0" : dt.Rows[0]["CGST"].ToString());
                    //grn.SGST = Convert.ToDouble(dt.Rows[0]["SGST"].ToString() == "" ? "0" : dt.Rows[0]["SGST"].ToString());
                    //grn.IGST = Convert.ToDouble(dt.Rows[0]["IGST"].ToString() == "" ? "0" : dt.Rows[0]["IGST"].ToString());
                    //grn.TotalAmt= Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());
                    DataTable dtParty = datatrans.GetData("select P.ACCOUNTNAME from SUBMRBASIC S,PARTYMAST P where S.PARTYID=P.PARTYMASTID AND S.SUBMRBASICID='" + id + "'");
                    string mid = dtParty.Rows[0]["ACCOUNTNAME"].ToString();
                    grn.mid = mid;

                if (grn.Net > 0)
                {
                    tda = new SubMatAccount();
                    // tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.Ledgername = mid;
                    tda.CRAmount = grn.Net;
                    tda.DRAmount = 0;
                    tda.TypeName = "NET";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Cr";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    tda.symbol = "+";
                    TData.Add(tda);
                }
                if (grn.Gross > 0)
                {
                    for (int i = 0; i < dtdet.Rows.Count; i++)
                    {
                        tda = new SubMatAccount();
                        tda.Ledgerlist = BindLedgerLst();
                        tda.Ledgername = dtdet.Rows[i]["ITEMACC"].ToString();
                        tda.CRAmount = 0;
                        tda.DRAmount = Convert.ToDouble(dtdet.Rows[i]["GROSS"].ToString() == "" ? "0" : dtdet.Rows[i]["GROSS"].ToString());
                        tda.TypeName = "GROSS";
                        tda.Isvalid = "Y";
                        tda.CRDR = "Dr";
                        tda.symbol = "-";
                        totalcredit += tda.CRAmount;
                        totaldebit += tda.DRAmount;
                        TData.Add(tda);
                        string hsn = datatrans.GetDataString("select HSN from ITEMMASTER  where  ITEMMASTERID='" + dtdet.Rows[i]["MITEMID"].ToString() + "' ");

                        string hsnid = datatrans.GetDataString("select HSNCODEID from HSNCODE WHERE HSNCODE= '" + hsn + "' ");

                        DataTable trff = new DataTable();
                        trff = datatrans.GetData("select TARIFFMASTER.TARIFFID,HSNROW.TARIFFID as tariff from HSNROW left outer join TARIFFMASTER on TARIFFMASTERID=HSNROW.TARIFFID where  HSNCODEID= '" + hsnid + "' ");
                        double per = '0';
                        if (trff.Rows.Count > 0)
                        {
                            for (int j = 0; j < trff.Rows.Count; j++)
                            {

                                string gst = trff.Rows[j]["tariff"].ToString();

                                DataTable pere = datatrans.GetData("Select PERCENTAGE from TARIFFMASTER where TARIFFMASTERID='" + gst + "'  ");
                                if (pere.Rows.Count > 0)
                                {
                                    per = Convert.ToDouble(pere.Rows[0]["PERCENTAGE"].ToString());
                                }
                            }
                        }



                        if (trff.Rows.Count == 1)
                        {

                            double cgst = per / 2;
                            double sgst = per / 2;


                            double cgstperc = tda.DRAmount / 100 * sgst;
                            double sgstperc = tda.DRAmount / 100 * cgst;
                            grn.CGST = cgstperc + sgstperc;
                            grn.SGST = cgstperc + sgstperc;
                            grn.Net = grn.CGST + grn.SGST + tda.DRAmount;
                            //po.Net = tda.TotalAmount;
                        }


                    }

                }
                
                if (grn.CGST > 0)
                    {
                        tda = new SubMatAccount();
                        //tda.CRDRLst = BindCRDRLst();
                        tda.Ledgerlist = BindLedgerLst();
                        tda.Ledgername = cgstledger;
                        tda.CRAmount = grn.CGST;
                        tda.DRAmount = 0;
                        tda.TypeName = "CGST";
                        tda.Isvalid = "Y";
                        tda.CRDR = "Dr";
                        tda.symbol = "-";
                        totalcredit += tda.CRAmount;
                        totaldebit += tda.DRAmount;
                        TData.Add(tda);
                    }
                    if (grn.SGST > 0)
                    {
                        tda = new SubMatAccount();
                        // tda.CRDRLst = BindCRDRLst();
                        tda.Ledgerlist = BindLedgerLst();
                        tda.Ledgername = sgstledger;
                        tda.CRAmount = grn.SGST;
                        tda.DRAmount = 0;
                        tda.TypeName = "SGST";
                        tda.Isvalid = "Y";
                        tda.CRDR = "Dr";
                        tda.symbol = "-";
                        totalcredit += tda.CRAmount;
                        totaldebit += tda.DRAmount;
                        TData.Add(tda);
                    }
                    if (grn.IGST > 0)
                    {
                        tda = new SubMatAccount();
                        // tda.CRDRLst = BindCRDRLst();
                        tda.Ledgerlist = BindLedgerLst();
                        tda.Ledgername = cgstledger;
                        tda.CRAmount = grn.IGST;
                        tda.DRAmount = 0;
                        tda.TypeName = "IGST";
                        tda.Isvalid = "Y";
                        tda.CRDR = "Dr";
                        tda.symbol = "-";
                        totalcredit += tda.CRAmount;
                        totaldebit += tda.DRAmount;
                        TData.Add(tda);
                    }
                

            }
            grn.TotalCRAmt = totalcredit;
            grn.TotalDRAmt = totaldebit;
            grn.Acclst = TData;
            grn.Accconfiglst = BindAccconfig();
            return View(grn);
        }
        public List<SelectListItem> BindLedgerLst()
        {
            try
            {
                DataTable dtDesg = SubContractingMaterialReceiptService.LedgerList();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LEDNAME"].ToString(), Value = dtDesg.Rows[i]["LEDGERID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindAccconfig()
        {
            try
            {
                DataTable dtDesg = SubContractingMaterialReceiptService.AccconfigLst();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ADSCHEME"].ToString(), Value = dtDesg.Rows[i]["ADCOMPHID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetItemGrpJSON()
        {
            return Json(BindLedgerLst());
        }
    }
}
