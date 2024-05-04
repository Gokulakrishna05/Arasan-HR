using Microsoft.AspNetCore.Mvc;
using Arasan.Models;
using Arasan.Interface.Sales;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Collections.Generic;
using System.Xml.Linq;
using AspNetCore.Reporting;
using System.Reflection;
using Arasan.Interface;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using Arasan.Services.Production;
using Arasan.Services;



namespace Arasan.Controllers
{
    public class ReceiptAgtRetDCController : Controller
    {
        IReceiptAgtRetDC ReceiptAgtRetDCService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public ReceiptAgtRetDCController(IReceiptAgtRetDC _ReceiptAgtRetDCService, IConfiguration _configuratio)
        {

            ReceiptAgtRetDCService = _ReceiptAgtRetDCService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult ReceiptAgtRetDC(string id)
        {
            ReceiptAgtRetDC ca = new ReceiptAgtRetDC();

           
            ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];
            ca.user = Request.Cookies["UserName"];
            ca.Entered = Request.Cookies["UserId"];
            ca.DDate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.Enteredlst = BindEmp();
            ca.Partylst = BindParty();
            ca.Stocklst = BindStock();
            ca.typelst = Bindtype();
            ca.Loclst = BindLoclst();
            ca.Dcnolst = BindDcnolst();
            ca.applst = BindEmp2();
            ca.apprlst = BindEmp2();
            DataTable dtv = datatrans.GetSequence("RecDC");
            if (dtv.Rows.Count > 0)
            {
                ca.Did = dtv.Rows[0]["PREFIX"].ToString() + dtv.Rows[0]["last"].ToString();
            }
            List<ReceiptAgtRetDCItem> TData = new List<ReceiptAgtRetDCItem>();
            ReceiptAgtRetDCItem tda = new ReceiptAgtRetDCItem();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new ReceiptAgtRetDCItem();
                    tda.namelst = Bindnamelst();
                    tda.Itemlst = BindItemlst("");
                    tda.Binlst = BindBinlst();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                double total = 0;
                dt = ReceiptAgtRetDCService.GetReceipt(id);
                if (dt.Rows.Count > 0)
                {

                    ca.Location = dt.Rows[0]["LOCID"].ToString();
                    ca.Did = dt.Rows[0]["DOCID"].ToString();
                    ca.DDate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.DcDate = dt.Rows[0]["DCDATE"].ToString();
                    ca.Party = dt.Rows[0]["PARTYID"].ToString();
                    ca.Stock = dt.Rows[0]["STKTYPE"].ToString();
                    ca.Ref = dt.Rows[0]["REFNO"].ToString();
                    ca.RefDate = dt.Rows[0]["REFDATE"].ToString();
                    ca.Dcno = dt.Rows[0]["DCNO"].ToString();
                    ca.Narration = dt.Rows[0]["NARRATION"].ToString();
                    ca.Approved = dt.Rows[0]["APPPER"].ToString();
                    ca.Approval2 = dt.Rows[0]["APPPER2"].ToString();
                    ca.Entered = dt.Rows[0]["EBY"].ToString();
                    ca.typelst = Bindtype();
                    DataTable dtt = new DataTable();
                    dtt = ReceiptAgtRetDCService.Getdctype(ca.Dcno);
                    if (dtt.Rows.Count > 0)
                    {
                        ca.DcType = dtt.Rows[0]["DELTYPE"].ToString();
                    }
                    ca.ID = id;
                }
                DataTable dt2 = new DataTable();
                dt2 = ReceiptAgtRetDCService.GetReceiptItem(id);
                if (dt2.Rows.Count > 0)
                {

                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        //tda = new ReceiptAgtRetDCItem();
                        //double toaamt = 0;
                        //tda.Itemlst = BindItemlst(tda.itemname);
                        //tda.itemname = dt2.Rows[i]["ITEMID"].ToString();
                        //tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();

                        //tda.namelst = Bindnamelst();
                        //DataTable dt3 = new DataTable();
                        //dt3 = datatrans.GetItemSubGroup(dt2.Rows[i]["ITEMID"].ToString());
                        //if (dt3.Rows.Count > 0)
                        //{
                        //    tda.itemname = dt3.Rows[0]["SUBGROUPCODE"].ToString(); 
                        //}

                        tda = new ReceiptAgtRetDCItem();
                        double toaamt = 0;
                        //tda.namelst = Bindnamelst();
                        //DataTable dt3 = new DataTable();
                        //dt3 = datatrans.GetItemSubGroup(dt2.Rows[i]["CITEMID"].ToString());
                        //if (dt3.Rows.Count > 0)
                        //{
                        //    tda.item = dt3.Rows[0]["SUBGROUPCODE"].ToString();
                        //}
                        tda.Itemlst = BindItemlst(tda.item);
                        tda.itemname = dt2.Rows[i]["ITEMID"].ToString();
                        tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();

                        tda.unit = dt2.Rows[i]["UNITID"].ToString();

                        tda.Binlst = BindBinlst();
                        tda.bin = dt2.Rows[i]["BINID"].ToString();
                        tda.rate = dt2.Rows[i]["RATE"].ToString();
                        tda.amount = dt2.Rows[i]["AMOUNT"].ToString();
                        tda.Recd = dt2.Rows[i]["QTY"].ToString();
                        tda.Pend = dt2.Rows[i]["PENDQTY"].ToString();
                        tda.rej = dt2.Rows[i]["REJQTY"].ToString();
                        //tda.serial = dt2.Rows[i]["SERIALYN"].ToString();
                        //tda.Acc = dt2.Rows[i]["ACCQTY"].ToString();

                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }

            }

            ca.ReceiptLst = TData;

            return View(ca);
        }

        [HttpPost]
        public ActionResult ReceiptAgtRetDC(ReceiptAgtRetDC Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = ""; //ReceiptAgtRetDCService.ReceiptAgtRetDCCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "ReceiptAgtRetDC Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "ReceiptAgtRetDC Updated Successfully...!";
                    }
                    return RedirectToAction("ListReceiptAgtRetDC");
                }

                else
                {
                    ViewBag.PageTitle = "Edit ReceiptAgtRetDC";
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


        public JsonResult GetBinJSON()
        {
            ReceiptAgtRetDCItem model = new ReceiptAgtRetDCItem();

            model.Binlst = BindBinlst();
            return Json(BindBinlst());

        }
        public List<SelectListItem> BindLoclst()
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
        } public List<SelectListItem> BindDcnolst()
        {
            try
            {
                DataTable dtDesg = ReceiptAgtRetDCService.Getdocno();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["RDELBASICID"].ToString() });
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
        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dtDesg = ReceiptAgtRetDCService.GetBranch();
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
       
        public List<SelectListItem> Bindbranchlst()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "(TAAI)SVK-FACTORY", Value = "(TAAI)SVK-FACTORY" });
                lstdesg.Add(new SelectListItem() { Text = "(TAAI)CHENNAI-DEPOT", Value = "(TAAI)CHENNAI-DEPOT" });
                lstdesg.Add(new SelectListItem() { Text = "(TAAI)KOLKATA-DEPOT", Value = "(TAAI)KOLKATA-DEPOT" });
                lstdesg.Add(new SelectListItem() { Text = "All Branches", Value = "All Branches" });

                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindParty()
        {
            try
            {
                DataTable dtDesg = ReceiptAgtRetDCService.GetParty();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTYID"].ToString(), Value = dtDesg.Rows[i]["PARTYMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        public List<SelectListItem> Bindnamelst()
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
         
        public List<SelectListItem> BindBinlst()
        {
            try
            {
                DataTable dtDesg = ReceiptAgtRetDCService.Getbin();
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
        public JsonResult GetItemJSON(string itemid)
        {
            ReceiptAgtRetDCItem model = new ReceiptAgtRetDCItem();
            model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst(itemid));

        }

        public JsonResult GetDCJSON(string ItemId)
        {
            ReceiptAgtRetDC model = new ReceiptAgtRetDC();
            model.Dcnolst = BindDClst(ItemId);
            return Json(BindDClst(ItemId));

        }

        public List<SelectListItem> BindDClst(string value)
        {
            try
            {
                DataTable dtDesg = ReceiptAgtRetDCService.GetdocnoS(value);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["RDELBASICID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindEmp2()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("Select EMPNAME||' / '||EMPID as empcode from EMPMAST");
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["empcode"].ToString(), Value = dtDesg.Rows[i]["empcode"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetDCDetails(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string dc = "";
                string part = "";
                string stock = "";
                string narr = "";
                string eby = "";
                string dceby = "";


                dt = ReceiptAgtRetDCService.GetDCDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    dc = dt.Rows[0]["DOCDATE"].ToString();
                    part = dt.Rows[0]["PARTYID"].ToString();
                    stock = dt.Rows[0]["STKTYPE"].ToString();
                    eby = dt.Rows[0]["EBY"].ToString();
                    dceby = datatrans.GetDataString("Select EMPNAME||' / '||EMPID as empcode from EMPMAST WHERE EMPMASTEID='"+ eby + "'");
                    narr = "Received from " + part;

                }

                var result = new { dc = dc, part= part,stock = stock , narr = narr, dceby= dceby };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public JsonResult GetPartJSON(string ItemId)
        //{
        //    ReceiptAgtRetDC model = new ReceiptAgtRetDC();
        //    model.Partylst = BindPartylst(ItemId);
        //    return Json(BindPartylst(ItemId));


        //}

        

        //public JsonResult GetPartyJSON(string ItemId)
        //{
        //    ReceiptAgtRetDC model = new ReceiptAgtRetDC();
        //    model.Partylst = BindPartylst(ItemId);
        //    return Json(BindPartylst(ItemId));

        //}

        //public JsonResult GetStockJSON(string ItemId)
        //{
        //    ReceiptAgtRetDC model = new ReceiptAgtRetDC();
        //    model.Stocklst = BindStocklst(ItemId);
        //    return Json(BindStocklst(ItemId));


        //}

        public List<SelectListItem> BindPartylst(string value)
        {
            try
            {
                DataTable dtDesg = ReceiptAgtRetDCService.GetPartys(value);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTYID"].ToString(), Value = dtDesg.Rows[i]["PARTYMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindStocklst(string value)
        {
            try
            {
                DataTable dtDesg = ReceiptAgtRetDCService.GetPartys(value);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTYID"].ToString(), Value = dtDesg.Rows[i]["PARTYMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindStock()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "Stock", Value = "Stock" });
                lstdesg.Add(new SelectListItem() { Text = "Asset", Value = "Asset" });

                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> Bindtype()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "Returnable DC", Value = "Returnable DC" });
                lstdesg.Add(new SelectListItem() { Text = "Non-Returnable DC", Value = "Non-Returnable DC" });
                lstdesg.Add(new SelectListItem() { Text = "Condemn", Value = "Condemn" });

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

                dt = ReceiptAgtRetDCService.GetItemDetail(ItemId);

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

        public JsonResult GetGrpitemJSON()
        {
            //CreditorDebitNote model = new CreditorDebitNote();
            //model.Grouplst = BindGrouplst(ItemId);
            return Json(Bindnamelst());

        }
        public IActionResult ListReceiptAgtRetDC()
        {
            return View();
        }
        
        //public IActionResult ReceiptReport()
        //{
        //    ReceiptAgtRetDC ca = new ReceiptAgtRetDC();

        //    ca.branchlst = Bindbranchlst();

        //    return View();
        //}
        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = ReceiptAgtRetDCService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListReceiptAgtRetDC");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListReceiptAgtRetDC");
            }
        }

        public ActionResult Remove(string tag, int id)
        {

            string flag = ReceiptAgtRetDCService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListReceiptAgtRetDC");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListReceiptAgtRetDC");
            }
        }

        public IActionResult ViewReceiptAgtRetDC(string id)
        {

            ReceiptAgtRetDC ca = new ReceiptAgtRetDC();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            dt = ReceiptAgtRetDCService.ViewGetReceipt(id);
            if (dt.Rows.Count > 0)
            {
                ca.Location = dt.Rows[0]["LOCID"].ToString();
                ca.Did = dt.Rows[0]["DOCID"].ToString();
                ca.DDate = dt.Rows[0]["DOCDATE"].ToString();
                ca.DcDate = dt.Rows[0]["DCDATE"].ToString();
                ca.Party = dt.Rows[0]["PARTYID"].ToString();
                ca.Stock = dt.Rows[0]["STKTYPE"].ToString();
                ca.Ref = dt.Rows[0]["REFNO"].ToString();
                ca.RefDate = dt.Rows[0]["REFDATE"].ToString();
                ca.Dcno = dt.Rows[0]["DOCID"].ToString();
                ca.Narration = dt.Rows[0]["NARRATION"].ToString();
                ca.Approved = dt.Rows[0]["APPPER"].ToString();
                ca.Approval2 = dt.Rows[0]["APPPER"].ToString();
                ca.typelst = Bindtype();
                DataTable dtt = new DataTable();
                dtt = ReceiptAgtRetDCService.Getviewdctype(ca.Dcno);
                if (dtt.Rows.Count > 0)
                {
                    ca.DcType = dtt.Rows[0]["DELTYPE"].ToString();
                }
                ca.Entered = dt.Rows[0]["EBY"].ToString();
                ca.ID = id;

                List<ReceiptAgtRetDCItem> Data = new List<ReceiptAgtRetDCItem>();
                ReceiptAgtRetDCItem tda = new ReceiptAgtRetDCItem();
                //double tot = 0;

                dt2 = ReceiptAgtRetDCService.ViewGetReceiptitem(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new ReceiptAgtRetDCItem();
                        
                        tda.itemname = dt2.Rows[i]["ITEMID"].ToString();
                        tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();

                       
                        //DataTable dt3 = new DataTable();
                        //dt3 = datatrans.GetItemSubGroup(dt2.Rows[i]["ITEMID"].ToString());
                        //if (dt3.Rows.Count > 0)
                        //{
                        //    tda.itemname = dt3.Rows[0]["SUBGROUPCODE"].ToString();
                        //}

                        tda.unit = dt2.Rows[i]["UNITID"].ToString();
                        tda.bin = dt2.Rows[i]["BINID"].ToString();
                        tda.rate = dt2.Rows[i]["RATE"].ToString();
                        tda.amount = dt2.Rows[i]["AMOUNT"].ToString();
                        tda.Recd = dt2.Rows[i]["QTY"].ToString();
                        tda.Pend = dt2.Rows[i]["PENDQTY"].ToString();
                        tda.rej = dt2.Rows[i]["REJQTY"].ToString();
                        //tda.serial = dt2.Rows[i]["SERIALYN"].ToString();
                        //tda.Acc = dt2.Rows[i]["ACCQTY"].ToString();

                        Data.Add(tda);
                    }
                }

                ca.ReceiptLst = Data;

            }
            return View(ca);
        }
        public ActionResult MyListItemgrid(string strStatus)
        {
            List<ReceiptAgtRetDCGrid> Reg = new List<ReceiptAgtRetDCGrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = ReceiptAgtRetDCService.GetAllReceipt(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

              
                string approve = string.Empty;
                string ViewRow = string.Empty;
                string EditRow = string.Empty;
                string DeleteRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    if (dtUsers.Rows[i]["STATUS"].ToString() == "Approve")
                    {
                        approve = "";
                        ViewRow = "<a href=ViewReceiptAgtRetDC?id=" + dtUsers.Rows[i]["RECDCBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View Details' width='20' /></a>";

                        EditRow = "";
                        DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["RECDCBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";

                    }
                    else
                    {
                        approve = "<a href=ApproveReceiptAgtRetDC?id=" + dtUsers.Rows[i]["RECDCBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/move_quote.png' alt='View Details' width='20' /></a>";
                        ViewRow = "<a href=ViewReceiptAgtRetDC?id=" + dtUsers.Rows[i]["RECDCBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View Details' width='20' /></a>";
                        EditRow = "<a href=ReceiptAgtRetDC?id=" + dtUsers.Rows[i]["RECDCBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                        DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["RECDCBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                    }
                       
                }
                else
                {


                    approve = "";
                    ViewRow = "";
                    EditRow = "";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["RECDCBASICID"].ToString() + "><img src='../Images/close_icon.png' alt='Deactivate' /></a>";

                }

                Reg.Add(new ReceiptAgtRetDCGrid
                {
                    id = dtUsers.Rows[i]["RECDCBASICID"].ToString(),
                    did = dtUsers.Rows[i]["DOCID"].ToString(),
                    ddate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    dctype = dtUsers.Rows[i]["DOCID"].ToString(),
                    party = dtUsers.Rows[i]["PARTYID"].ToString(),



                    approve = approve,
                    viewrow = ViewRow,
                    editrow = EditRow,
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }


        public ActionResult GetItemgrpDetails(string id)
        {
            ReceiptAgtRetDC model = new ReceiptAgtRetDC();
            DataTable dt2 = new DataTable();
            List<ReceiptAgtRetDCItem> Data = new List<ReceiptAgtRetDCItem>();
            ReceiptAgtRetDCItem tda = new ReceiptAgtRetDCItem();
            dt2 = ReceiptAgtRetDCService.GetItemgrpDetail(id);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new ReceiptAgtRetDCItem();

                    tda.itemname = dt2.Rows[i]["ITEMID"].ToString();
                    tda.itemid = dt2.Rows[i]["IID"].ToString();
                    tda.unit = dt2.Rows[i]["UNIT"].ToString();
                   
                    tda.rej = dt2.Rows[i]["QTY"].ToString();
                    tda.rate = dt2.Rows[i]["RATE"].ToString();
                    tda.detid = dt2.Rows[i]["RDELDETAILID"].ToString();


                    Data.Add(tda);
                }
            }
            model.ReceiptLst = Data;
            return Json(model.ReceiptLst);

        }



        public IActionResult ApproveReceiptAgtRetDC(string id)
        {

            ReceiptAgtRetDC ca = new ReceiptAgtRetDC();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            dt = ReceiptAgtRetDCService.ViewGetReceipt(id);
            if (dt.Rows.Count > 0)
            {
                ca.Location = dt.Rows[0]["LOCID"].ToString();
                ca.Locationid = dt.Rows[0]["loc"].ToString();
                ca.Did = dt.Rows[0]["DOCID"].ToString();
                ca.DDate = dt.Rows[0]["DOCDATE"].ToString();
                ca.DcDate = dt.Rows[0]["DCDATE"].ToString();
                ca.Party = dt.Rows[0]["PARTYID"].ToString();
                ca.Stock = dt.Rows[0]["STKTYPE"].ToString();
                ca.Ref = dt.Rows[0]["REFNO"].ToString();
                ca.RefDate = dt.Rows[0]["REFDATE"].ToString();
                ca.Dcno = dt.Rows[0]["DOCID"].ToString();
                ca.Narration = dt.Rows[0]["NARRATION"].ToString();
                ca.Approved = dt.Rows[0]["APPPER"].ToString();
                ca.Approval2 = dt.Rows[0]["APPPER"].ToString();
              
                ca.typelst = Bindtype();
                DataTable dtt = new DataTable();
                dtt = ReceiptAgtRetDCService.Getviewdctype(ca.Dcno);
                if (dtt.Rows.Count > 0)
                {
                    ca.DcType = dtt.Rows[0]["DELTYPE"].ToString();
                }

                ca.ID = id;

                List<ReceiptAgtRetDCItem> Data = new List<ReceiptAgtRetDCItem>();
                ReceiptAgtRetDCItem tda = new ReceiptAgtRetDCItem();
                //double tot = 0;

                dt2 = ReceiptAgtRetDCService.ViewGetReceiptitem(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new ReceiptAgtRetDCItem();

                        tda.itemname = dt2.Rows[i]["ITEMID"].ToString();
                        tda.saveItemId = dt2.Rows[i]["CITEMID"].ToString();


                        //DataTable dt3 = new DataTable();
                        //dt3 = datatrans.GetItemSubGroup(dt2.Rows[i]["ITEMID"].ToString());
                        //if (dt3.Rows.Count > 0)
                        //{
                        //    tda.itemname = dt3.Rows[0]["SUBGROUPCODE"].ToString();
                        //}

                        tda.detid = dt2.Rows[i]["RECDCDETAILID"].ToString();
                        tda.unit = dt2.Rows[i]["UNITID"].ToString();
                        tda.bin = dt2.Rows[i]["BINID"].ToString();
                        tda.rate = dt2.Rows[i]["RATE"].ToString();
                        tda.amount = dt2.Rows[i]["AMOUNT"].ToString();
                        tda.Recd = dt2.Rows[i]["QTY"].ToString();
                        tda.Pend = dt2.Rows[i]["PENDQTY"].ToString();
                        tda.rej = dt2.Rows[i]["REJQTY"].ToString();
                        tda.serial = dt2.Rows[i]["SERIALYN"].ToString();
                        tda.Acc = dt2.Rows[i]["ACCQTY"].ToString();

                        Data.Add(tda);
                    }
                }

                ca.ReceiptLst = Data;

            }
            return View(ca);
        }
        [HttpPost]
        public ActionResult ApproveReceiptAgtRetDC(ReceiptAgtRetDC Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = ReceiptAgtRetDCService.ApproveReceiptAgtRetDCCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                { 
                        TempData["notice"] = "Approve ReceiptAgtRetDC Inserted Successfully...!";
                    
                    return RedirectToAction("ListReceiptAgtRetDC");
                }

                else
                {
                    ViewBag.PageTitle = "Edit ReceiptAgtRetDC";
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
 
    }
}
