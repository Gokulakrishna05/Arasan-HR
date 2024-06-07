using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Models;
using Oracle.ManagedDataAccess.Client;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Interface.Master;
using Newtonsoft.Json.Linq;
using Arasan.Services.Store_Management;
//using DocumentFormat.OpenXml.Office2010.Excel;

namespace Arasan.Controllers.Store_Management
{
    public class MaterialRequisitionController : Controller
    {
        IMaterialRequisition materialReq;
        // IPurchaseIndent PurIndent;
        private string? _connectionString;
        IConfiguration? _configuratio;
        DataTransactions datatrans;
        private string storeid;
        public MaterialRequisitionController(IMaterialRequisition _MatreqService, IConfiguration _configuratio)
        {
            materialReq = _MatreqService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
            storeid = datatrans.GetDataString("select LOCDETAILSID from locdetails where locid = 'STORES'");
        }
        public IActionResult MaterialRequisition(string id)
        {
            MaterialRequisition MR = new MaterialRequisition();
            MR.Brlst = BindBranch();
            var userId = Request.Cookies["UserId"];
            MR.Loclst = GetLoc(userId);
            //MR.Loclst = GetLocation();
            MR.Worklst = BindWorkCenter("");
            MR.Processlst = BindProcess("");
            MR.assignList = BindEmp();
            MR.Statuslst = BindStatus();
            MR.Branch = Request.Cookies["BranchId"];
            MR.Entered = Request.Cookies["UserName"];
            MR.EnteredId = Request.Cookies["UserId"];
            MR.Location = Request.Cookies["LocationName"];
            MR.Storeid = storeid;
            MR.DocDa = DateTime.Now.ToString("dd-MMM-yyyy");

            List<MaterialRequistionItem> TData = new List<MaterialRequistionItem>();
            MaterialRequistionItem tda = new MaterialRequistionItem();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new MaterialRequistionItem();
                    //tda.ItemGrouplst = BindItemGrplst();
                    tda.Itemlst = BindItemlst();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                //st = StoreAccService.GetStoreAccById(id);

                DataTable dt = new DataTable();

                dt = materialReq.GetmaterialReqDetails(id);
                if (dt.Rows.Count > 0)
                {
                    MR.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    MR.Location = dt.Rows[0]["FROMLOCID"].ToString();
                    MR.Worklst = BindWorkCenter(MR.Location);
                    MR.WorkCenter = dt.Rows[0]["WCID"].ToString();
                    MR.Processlst = BindProcess(MR.WorkCenter);
                    MR.Process = dt.Rows[0]["PROCESSID"].ToString();
                    MR.RequestType = dt.Rows[0]["REQTYPE"].ToString();
                    MR.DocId = dt.Rows[0]["DOCID"].ToString();
                    MR.DocDa = dt.Rows[0]["DOCDATE"].ToString();
                    MR.matno = dt.Rows[0]["DOCID"].ToString();
                }

                DataTable dtt = new DataTable();
                dtt = materialReq.GetmaterialReqItemDetails(id);
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda = new MaterialRequistionItem();

                        tda.ItemGrouplst = BindItemGrplst();
                        DataTable dt3 = new DataTable();
                        dt3 = materialReq.GetItemGroup(dtt.Rows[i]["ITEMID"].ToString());
                        if (dt3.Rows.Count > 0)
                        {
                            tda.ItemGroupId = dt3.Rows[0]["ITEMGROUP"].ToString();
                        }
                        tda.Itemlst = BindItemlst();
                        tda.ItemId = dtt.Rows[i]["ITEMID"].ToString();
                        tda.UnitID = dtt.Rows[i]["UNITID"].ToString();
                        MR.Narration = dtt.Rows[i]["NARR"].ToString();
                        tda.ReqQty = dtt.Rows[i]["QTY"].ToString();
                        DataTable dt1 = materialReq.Getstkqty(dtt.Rows[i]["ITEMMASTERID"].ToString(), storeid );
                        if (dt1.Rows.Count > 0)
                        {
                            tda.ClosingStock = dt1.Rows[0]["QTY"].ToString();
                        }


                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }

                }
            }
            MR.MRlst = TData;
            return View(MR);
        }

        public List<SelectListItem> BindItemlst()
        {
            try
            {
                DataTable dtDesg = materialReq.GetItem();
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
                DataTable dtDesg = materialReq.GetItemGrp();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["GROUPTYPE"].ToString(), Value = dtDesg.Rows[i]["ITEMGROUPID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetItemDetail(string ItemId, string branch)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string unit = "";
                string stk = "";
                string unitid = "";

                dt = materialReq.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    unit = dt.Rows[0]["UNITID"].ToString();
                    unitid = dt.Rows[0]["UNITMASTID"].ToString();
                }
                dt1 = materialReq.Getstkqty(ItemId, storeid);
                if (dt1.Rows.Count > 0)
                {
                    stk = dt1.Rows[0]["QTY"].ToString();
                }
                if (stk == "")
                {
                    stk = "0";
                }
                var result = new { unit = unit, stk = stk, unitid = unitid };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult MaterialRequisition(MaterialRequisition Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = materialReq.MaterialCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "MaterialRequisition Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "MaterialRequisition Updated Successfully...!";
                    }
                    return RedirectToAction("ListMaterialRequisition");
                }

                else
                {
                    ViewBag.PageTitle = "Edit ListMaterialRequisition";
                    TempData["notice"] = Strout;
                    //return View();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Cy);
        }
        public JsonResult GetWorkJSON(string supid)
        {
            MaterialRequisition model = new MaterialRequisition();
            model.Worklst = BindWorkCenter(supid);
            return Json(BindWorkCenter(supid));

        }
        public List<SelectListItem> BindWorkCenter(string value)
        {
            try
            {
                DataTable dtDesg = materialReq.GetWorkCenter(value);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["WCID"].ToString(), Value = dtDesg.Rows[i]["WCBASICID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetProcessJSON(string supid)
        {
            MaterialRequisition model = new MaterialRequisition();
            model.Processlst = BindProcess(supid);
            return Json(BindProcess(supid));

        }
        public List<SelectListItem> BindProcess(string id)
        {
            try
            {
                DataTable dtDesg = materialReq.BindProcess(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PROCESSNAME"].ToString(), Value = dtDesg.Rows[i]["PROCESSID"].ToString() });
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
                DataTable dt = new DataTable();
                datatrans = new DataTransactions(_connectionString);
                dt = datatrans.GetBranch();
                // DataTable dtDesg = datatrans.GetBranch();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dt.Rows[i]["BRANCHID"].ToString(), Value = dt.Rows[i]["BRANCHMASTID"].ToString() });
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
        //public JsonResult GetLocJSON(string itemid)
        //{
        //    MaterialRequisition model = new MaterialRequisition();
        //    model.Loclst = GetLoc(itemid);
        //    return Json(GetLoc(itemid));

        //}
        public List<SelectListItem> GetLoc(string id)
        {
            try
            {
                DataTable dtDesg = materialReq.GetLocation(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOCID"].ToString(), Value = dtDesg.Rows[i]["loc"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> GetLocation()
        {
            try
            {
                DataTable dtDesg = materialReq.GetLoc();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOCID"].ToString(), Value = dtDesg.Rows[i]["LOCATIONDETAILSID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //private List<SelectListItem> PopulateDropDown(String query, string textcolumn, string valuecolumn)
        //{
        //   // DataTable dt = new DataTable();
        //   // dt = datatrans.GetBranch();
        //    List<SelectListItem> items = new List<SelectListItem>();
        //   // string constr = ConfigurationManager._connectionString.["IGFSCON"].ConnectionString;
        //    using (OracleConnection con = new OracleConnection(_connectionString))
        //    {
        //        using (OracleCommand cmd = new OracleCommand(query))
        //        {
        //            cmd.Connection = con;
        //            con.Open();
        //            using (OracleDataReader sdr = cmd.ExecuteReader())
        //            {

        //                while (sdr.Read())
        //                {
        //                    items.Add(new SelectListItem
        //                    {
        //                        Text = sdr[textcolumn].ToString(),
        //                        Value = sdr[valuecolumn].ToString(),
        //                    });
        //                }

        //            }
        //            con.Close();

        //        }
        //    }
        //    return items;

        //}
        public IActionResult ListMaterialRequisition()
        {
            //IEnumerable<MaterialRequisition> cmp = materialReq.GetAllMaterial(status, st, ed);
            return View();
        }
        public ActionResult MyListMaterialRequisitionGrid(string strStatus, string strfrom, string strTo)
        {
            List<MaterialItem> Reg = new List<MaterialItem>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)materialReq.GetAllMaterialRequItems(strStatus, strfrom, strTo);
            
           
             
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                //string invid = dtUsers.Rows[0]["STORESREQBASICID"].ToString();
                //DataTable dt = (DataTable)materialReq.GetAllMaterialDetailRequItems(invid);
                //double qty = Convert.ToDouble(dt.Rows[i]["QTY"].ToString());
                //string item = dt.Rows[0]["ITEMID"].ToString();
                //string stock = datatrans.GetDataString("Select SUM(BALANCE_QTY) from INVENTORY_ITEM where ITEM_ID='" + item + "' AND LOCATION_ID='10001000000827' ");

                //double stk = Convert.ToDouble(stock);
                string Issuse = string.Empty;
                //string FollowUp = string.Empty;
                string MoveToIndent = string.Empty;
                //string Pdf = string.Empty;
                string View = string.Empty;
                string EditRow = string.Empty;
                string DeleteRow = string.Empty;
                string priority = string.Empty;
                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y" || dtUsers.Rows[i]["IS_ACTIVE"].ToString() == null)
                {
                    if (dtUsers.Rows[i]["STATUS"].ToString() == "Issued")
                    {
                        Issuse = "";
                        EditRow = "";
                        View = "";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(dtUsers.Rows[i]["REASON"].ToString()))
                        {
                         
                            priority = "";
                        }
                        else { priority = "1"; }
                            Issuse = "<a href=ApproveMaterial?&id=" + dtUsers.Rows[i]["STORESREQBASICID"].ToString() + "><img src='../Images/issue_icon.png' alt='View Details' width='20' /></a>";
                        EditRow = "<a href=MaterialRequisition?id=" + dtUsers.Rows[i]["STORESREQBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";

                        View = "<a href=MaterialStatus?id=" + dtUsers.Rows[i]["STORESREQBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/close_icon.png' alt='View Details' width='20' /></a>";
                        DeleteRow = "DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["STORESREQBASICID"].ToString() + "";
                    }
                    //if (dtUsers.Rows[i]["STATUS"].ToString() == "Indent")
                    //{
                    //  //  MoveToIndent = "<img src='../Images/tick.png' alt='View Details' width='20' />";
                    //    EditRow = "";
                    //}
                    //else
                    //{
                    //   // MoveToIndent = "<a href=IssueToindent?id=" + dtUsers.Rows[i]["STORESREQBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/move_quote.png' alt='View Details' width='20' /></a>";
                    //    EditRow = "<a href=MaterialRequisition?id=" + dtUsers.Rows[i]["STORESREQBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    //}
                  

                }
                else
                {
                    Issuse = "";
                  
                    EditRow = "";
                    View = "";
                    DeleteRow = "Active?tag=Del&id=" + dtUsers.Rows[i]["STORESREQBASICID"].ToString() + "";
                }
                Reg.Add(new MaterialItem
                {
                    piid = dtUsers.Rows[i]["STORESREQBASICID"].ToString(),
                    branch = dtUsers.Rows[i]["BRANCHID"].ToString(),
                    docid = dtUsers.Rows[i]["DOCID"].ToString(),
                    docDate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    location = dtUsers.Rows[i]["LOCID"].ToString(),
                    enter = dtUsers.Rows[i]["ENTBY"].ToString(),
                    enterdate = dtUsers.Rows[i]["ENTAT"].ToString(),
                    //   work = dtUsers.Rows[i]["WCID"].ToString(),
                       //pri = dtUsers.Rows[i]["PRIORITY"].ToString(),
                    iss = Issuse,
                    //follow = FollowUp,
                    move = MoveToIndent,
                    //pdf = Pdf,
                    view = View,
                    priority = priority,
                    editrow = EditRow,
                    delrow = DeleteRow,



                });
            }

            return Json(new
            {
                Reg
            });

        }
        public ActionResult ListMRItemgrid(string strfrom, string strTo)
        {
            List<MRItemBindList> EnqChkItem = new List<MRItemBindList>();
            DataTable dtEnq = new DataTable();
            dtEnq = materialReq.GetMRItem(strfrom, strTo);
            for (int i = 0; i < dtEnq.Rows.Count; i++)
            {
                EnqChkItem.Add(new MRItemBindList
                {
                    indentid = dtEnq.Rows[i]["STORESREQDETAILID"].ToString(),
                    piid = dtEnq.Rows[i]["STORESREQBASICID"].ToString(),
                    itemname = dtEnq.Rows[i]["ITEMID"].ToString(),
                    unit = dtEnq.Rows[i]["UNITID"].ToString(),
                    quantity = dtEnq.Rows[i]["QTY"].ToString(),
                   
                });
            }

            return Json(new
            {
                EnqChkItem
            });
        }
        public ActionResult DeleteItem(string tag, string id)
        {

            string flag = materialReq.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListMaterialRequisition");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListMaterialRequisition");
            }
        }
        public ActionResult Active(string tag, string id)
        {

            string flag = materialReq.StatusActChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListMaterialRequisition");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListMaterialRequisition");
            }
        }
        public IActionResult ApproveMaterial(string id)
        {
            MaterialRequisition MR = new MaterialRequisition();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();
            dt = materialReq.GetMatbyID(id);
            if (dt.Rows.Count > 0)
            {
                MR.Location = dt.Rows[0]["LOCID"].ToString();
                MR.Branch = dt.Rows[0]["BRANCHID"].ToString();
                MR.BranchId = dt.Rows[0]["BRANCH"].ToString();
                MR.DocId = dt.Rows[0]["DOCID"].ToString();
                MR.DocDa = dt.Rows[0]["DOCDATE"].ToString();
                MR.WorkCenter = dt.Rows[0]["WCID"].ToString();
                MR.WorkCenterid = dt.Rows[0]["work"].ToString();
                MR.Process = dt.Rows[0]["PROCESSNAME"].ToString();
                MR.Storeid = storeid;
                MR.RequestType = dt.Rows[0]["REQTYPE"].ToString();
                MR.BranchId = dt.Rows[0]["BRANCHIDS"].ToString();
                MR.LocationId = dt.Rows[0]["FROMLOCID"].ToString();
                MR.ID = id;
            }
            List<MaterialRequistionItem> TData = new List<MaterialRequistionItem>();
            MaterialRequistionItem tda = new MaterialRequistionItem();
            dtt = materialReq.GetMatItemByID(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new MaterialRequistionItem();
                    tda.Item = dtt.Rows[i]["ITEMID"].ToString();
                    tda.ItemId = dtt.Rows[i]["ITEMMASTERID"].ToString();
                    tda.UnitID = dtt.Rows[i]["UNIT"].ToString();
                    tda.Unit = dtt.Rows[i]["UNITID"].ToString();
                    tda.ReqQty = dtt.Rows[i]["QTY"].ToString();
                    tda.detid = dtt.Rows[i]["STORESREQDETAILID"].ToString();

                    tda.InvQty = Convert.ToDouble(dtt.Rows[i]["QTY"].ToString() == "" ? "0" : dtt.Rows[i]["QTY"].ToString());
                    tda.issqty = Convert.ToDouble(dtt.Rows[i]["ISSQTY"].ToString() == "" ? "0" : dtt.Rows[i]["ISSQTY"].ToString());
                    double pendqty = tda.InvQty - tda.issqty;
                    tda.IndQty = pendqty;
                    tda.InvQty = 0;//pendqty;
                    tda.indentid = dtt.Rows[i]["STORESREQDETAILID"].ToString();

                    tda.Storeid = dtt.Rows[i]["STORESREQBASICID"].ToString();
                    string binid = datatrans.GetDataString("SELECT BINBASIC.BINID FROM ITEMMASTER LEFT OUTER JOIN BINBASIC ON BINBASICID=ITEMMASTER.BINNO WHERE ITEMMASTERID='"+ tda.ItemId + "'");
                    tda.bin = binid;
                    tda.Isvalid = "Y";
                    double reqqty = Convert.ToDouble(dtt.Rows[i]["QTY"].ToString() == "" ? "0" : dtt.Rows[i]["QTY"].ToString());
                    DataTable dt1 = materialReq.Getstkqty(dtt.Rows[i]["ITEMMASTERID"].ToString(), storeid);
                    if (dt1.Rows.Count > 0)
                    {
                        if (string.IsNullOrEmpty(dt1.Rows[0]["QTY"].ToString()))
                        {
                            tda.ClosingStock = "0";
                        }
                        else
                        {
                            tda.ClosingStock = dt1.Rows[0]["QTY"].ToString();
                        }
                    }
                    DataTable dt2 = materialReq.GetItemLot(dtt.Rows[i]["ITEMMASTERID"].ToString(), storeid);
                    if (dt2.Rows.Count > 0)
                    {

                        tda.lot= dt2.Rows[0]["LOTNO"].ToString();
                    }
                    else
                    {
                        tda.lot = "0";
                    }
                    double stkqty = 0;
                    if (!string.IsNullOrEmpty(tda.ClosingStock))
                    {
                        stkqty = Convert.ToDouble(tda.ClosingStock);
                    }
                   // if (tda.IndQty > 0)
                   // {
                   //     tda.InvQty = tda.IndQty;
                   // }
                   //else if (stkqty > reqqty)
                   // {
                   //     tda.InvQty = reqqty;
                   //     tda.IndQty = 0;
                   // }
                   // else
                   // {
                   //     tda.InvQty = stkqty;
                   //     tda.IndQty = (reqqty - stkqty);
                   // }


                    //tda.Itemlst = BindItemlst();
                    string lot = datatrans.GetDataString("SELECT LOTYN FROM ITEMMASTER WHERE ITEMMASTERID='" + tda.ItemId + "'");
                    if (lot == "YES")
                    {
                        tda.TotalStock = datatrans.GetDataString("select SUM(S.PLUSQTY-S.MINUSQTY) as QTY  from LSTOCKVALUE S  where S.LOCATION_ID NOT IN '" + storeid + "' AND S.ITEMID='" + tda.ItemId + "' HAVING SUM(S.PLUSQTY-S.MINUSQTY) > 0");

                    }
                    else
                    {
                        tda.TotalStock = datatrans.GetDataString("select SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) as QTY  from STOCKVALUE S  where S.LOCID NOT IN '" + storeid + "' AND S.ITEMID='" + tda.ItemId + "' HAVING SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) > 0");

                    }
                      
                    TData.Add(tda);
                }
            }
            MR.MRlst = TData;
            return View(MR);
        }
        public IActionResult IssueToindent(string id)
        {
            MaterialRequisition MR = new MaterialRequisition();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();
            dt = materialReq.GetMatbyID(id);
            if (dt.Rows.Count > 0)
            {
                MR.Location = dt.Rows[0]["LOCID"].ToString();
                MR.Branch = dt.Rows[0]["BRANCHID"].ToString();
                MR.BranchId = dt.Rows[0]["BRANCH"].ToString();
                MR.DocId = dt.Rows[0]["DOCID"].ToString();
                MR.DocDa = dt.Rows[0]["DOCDATE"].ToString();
                MR.WorkCenter = dt.Rows[0]["WCID"].ToString();
                MR.WorkCenterid = dt.Rows[0]["work"].ToString();
                MR.RequestType = dt.Rows[0]["REQTYPE"].ToString();
                MR.BranchId = dt.Rows[0]["BRANCHIDS"].ToString();
                MR.LocationId = dt.Rows[0]["FROMLOCID"].ToString();
                MR.Entered = Request.Cookies["UserId"];
                MR.MaterialReqId = id;
                MR.Storeid = storeid;
            }
            List<MaterialRequistionItem> TData = new List<MaterialRequistionItem>();
            MaterialRequistionItem tda = new MaterialRequistionItem();
            dtt = materialReq.GetIndMatItemByID(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new MaterialRequistionItem();
                    tda.Item = dtt.Rows[i]["ITEMID"].ToString();
                    tda.ItemId = dtt.Rows[i]["ITEMMASTERID"].ToString();
                    tda.UnitID = dtt.Rows[i]["UNIT"].ToString();
                    tda.Unit = dtt.Rows[i]["UNITID"].ToString();
                    tda.ReqQty = dtt.Rows[i]["QTY"].ToString();
                    double reqqty = Convert.ToDouble(dtt.Rows[i]["QTY"].ToString() == "" ? "0" : dtt.Rows[i]["QTY"].ToString());
                    DataTable dt1 = materialReq.Getstkqty(dtt.Rows[i]["ITEMMASTERID"].ToString(), storeid );
                    if (dt1.Rows.Count > 0)
                    {
                        if (string.IsNullOrEmpty(dt1.Rows[0]["QTY"].ToString()))
                        {
                            tda.ClosingStock = "0";
                        }
                        else
                        {
                            tda.ClosingStock = dt1.Rows[0]["QTY"].ToString();
                        }

                    }
                    double stkqty = 0;
                    if (!string.IsNullOrEmpty(tda.ClosingStock))
                    {
                        stkqty = Convert.ToDouble(tda.ClosingStock);
                    }
                    if (stkqty > reqqty)
                    {
                        tda.InvQty = reqqty;
                        tda.IndQty = 0;
                    }
                    else
                    {
                        tda.InvQty = stkqty;
                        tda.IndQty = (reqqty - stkqty);
                    }
                    //tda.Itemlst = BindItemlst();
                    DataTable stock = datatrans.GetData("Select SUM(BALANCE_QTY) as qty from INVENTORY_ITEM where ITEM_ID='" + tda.ItemId + "' AND BALANCE_QTY > 0 AND LOCATION_ID NOT IN '" + storeid + "' AND BRANCH_ID='" + MR.BranchId + "'  ");
                    if (stock.Rows.Count > 0)
                    {
                        tda.TotalStock = stock.Rows[0]["qty"].ToString();
                    }
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            MR.MRlst = TData;
            return View(MR);
        }
        [HttpPost]
        public ActionResult IssueToindent(MaterialRequisition Cy, string id)
        {
            try
            {
                Cy.ID = id;
                string Strout = materialReq.IssuetoIndent(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = " Issued Indent Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Issued Indent Successfully...!";
                    }
                    return RedirectToAction("ListMaterialRequisition");
                }

                else
                {
                    ViewBag.PageTitle = "Edit PO";
                    TempData["notice"] = Strout;
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("ListPO");
        }
        [HttpPost]
        public ActionResult ApproveMaterial(MaterialRequisition Cy, string id)
        {
            try
            {
                Cy.ID = id;
                string Strout = materialReq.ApproveMaterial(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Material Issued Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Material Issued Successfully...!";
                    }
                    return RedirectToAction("ListMaterialRequisition");
                }

                else
                {
                    ViewBag.PageTitle = "Edit PO";
                    TempData["notice"] = Strout;
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("ListPO");
        }
        public JsonResult GetItemJSON()
        {
            MaterialRequistionItem model = new MaterialRequistionItem();
            model.Itemlst = BindItemlst();
            return Json(BindItemlst());

        }

        public JsonResult GetItemGrpJSON()
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItemGrplst());
        }
        public List<SelectListItem> BindStatus()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "OPEN", Value = "OPEN" });
                lstdesg.Add(new SelectListItem() { Text = "CLOSE", Value = "CLOSE" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult MaterialStatus(string id)
        {
            MaterialRequisition MR = new MaterialRequisition();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();
            dt = materialReq.GetMatStabyID(id);
            if (dt.Rows.Count > 0)
            {
                MR.Location = dt.Rows[0]["LOCID"].ToString();
                MR.Branch = dt.Rows[0]["BRANCHID"].ToString();
                MR.DocId = dt.Rows[0]["DOCID"].ToString();
                MR.DocDa = dt.Rows[0]["DOCDATE"].ToString();
                MR.WorkCenter = dt.Rows[0]["WCID"].ToString();
                MR.Process = dt.Rows[0]["PROCESSNAME"].ToString();

                MR.RequestType = dt.Rows[0]["REQTYPE"].ToString();
                MR.BranchId = dt.Rows[0]["BRANCHIDS"].ToString();
                MR.LocationId = dt.Rows[0]["FROMLOCID"].ToString();
            }
            List<MaterialRequistionItem> TData = new List<MaterialRequistionItem>();
            MaterialRequistionItem tda = new MaterialRequistionItem();
            dtt = materialReq.GetMatStaItemByID(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new MaterialRequistionItem();
                    tda.Item = dtt.Rows[i]["ITEMID"].ToString();
                    tda.UnitID = dtt.Rows[i]["UNIT"].ToString();
                    tda.Unit = dtt.Rows[i]["UNITID"].ToString();
                    tda.ReqQty = dtt.Rows[i]["QTY"].ToString();
                    tda.ClosingStock = dtt.Rows[i]["STOCK"].ToString();
                    tda.detid = dtt.Rows[i]["STORESREQDETAILID"].ToString();
                    tda.Isvalid = "Y";
                    //double reqqty = Convert.ToDouble(dtt.Rows[i]["QTY"].ToString());
                    //DataTable dt1 = materialReq.Getstkqty(dtt.Rows[i]["ITEMMASTERID"].ToString(), dt.Rows[0]["FROMLOCID"].ToString(), dt.Rows[0]["BRANCHIDS"].ToString());
                    //if (dt1.Rows.Count > 0)
                    //{
                    //    tda.ClosingStock = dt1.Rows[0]["QTY"].ToString();
                    //}
                    //double stkqty = 0;
                    //if (!string.IsNullOrEmpty(tda.ClosingStock))
                    //{
                    //    stkqty = Convert.ToDouble(tda.ClosingStock);
                    //}
                    //if (stkqty > reqqty)
                    //{
                    //    tda.InvQty = reqqty;
                    //    tda.IndQty = 0;
                    //}
                    //else
                    //{
                    //    tda.InvQty = stkqty;
                    //    tda.IndQty = (reqqty - stkqty);
                    //}
                    //tda.Itemlst = BindItemlst();

                    TData.Add(tda);
                }
            }
            MR.MRlst = TData;
            return View(MR);
        }
        [HttpPost]
        public ActionResult MaterialStatus(MaterialRequisition Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = materialReq.MaterialStatus(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "MaterialRequisition Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "MaterialRequisition Updated Successfully...!";
                    }
                    return RedirectToAction("ListMaterialRequisition");
                }

                else
                {
                    ViewBag.PageTitle = "Edit MaterialStatus";
                    TempData["notice"] = Strout;
                    //return View();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Cy);
        }


        public IActionResult WholeStock(string id)
        {
            MaterialRequisition MR = new MaterialRequisition();
            MR.Entered = Request.Cookies["UserId"];
            List<StockItem> TData = new List<StockItem>();
            StockItem tda = new StockItem();
            DataTable dtt = new DataTable();
            string lot = datatrans.GetDataString("SELECT LOTYN FROM ITEMMASTER WHERE ITEMMASTERID='" + id + "'");

            if(lot=="YES")
            {
                dtt = datatrans.GetData("Select ITEMMASTER.ITEMID,S.ITEMID as item,LOCDETAILS.LOCID,to_char(S.DOCDATE,'dd-MON-yyyy')DOCDATE,S.LOTNO,SUM(S.PLUSQTY-S.MINUSQTY) as QTY from  LSTOCKVALUE S left outer join ITEMMASTER ON ITEMMASTERID=S.ITEMID left outer join LOCDETAILS ON LOCDETAILSID=S.LOCID where S.ITEMID=" + id + " AND S.LOCID NOT IN '10001000000827' HAVING SUM(S.PLUSQTY-S.MINUSQTY) > 0  GROUP BY ITEMMASTER.ITEMID,S.LOTNO,S.ITEMID ,LOCDETAILS.LOCID,S.DOCDATE ");

            }
            else
            {
                dtt = datatrans.GetData("Select I.ITEMID,S.ITEMID as item,L.LOCID,SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) as QTY from STOCKVALUE S,ITEMMASTER I,LOCDETAILS L WHERE I.ITEMMASTERID=S.ITEMID AND LOCDETAILSID=S.LOCID AND S.ITEMID='" + id + "'  AND S.LOCID NOT IN '10001000000827'  HAVING SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) > 0 GROUP BY I.ITEMID,S.ITEMID,L.LOCID");

            }
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new StockItem();
                    tda.item = dtt.Rows[i]["ITEMID"].ToString();
                    tda.itemid = dtt.Rows[i]["item"].ToString();
                    //tda.invid = dtt.Rows[i]["INVENTORY_ITEM_ID"].ToString();
                    
                    tda.location = dtt.Rows[i]["LOCID"].ToString();
                    // tda.locationid = dtt.Rows[i]["LOCATION_ID"].ToString();
                    // tda.docDate = dtt.Rows[i]["DOCDATE"].ToString();
                    tda.qty = dtt.Rows[i]["QTY"].ToString();


                    TData.Add(tda);
                }
            }
            MR.stklst = TData;
            return View(MR);
        }

        [HttpPost]
        public ActionResult WholeStock(MaterialRequisition Cy, string id)
        {
            try
            {
                Cy.ID = id;
                string Strout = materialReq.WholeStockGURD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Material Issued Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Material Issued Successfully...!";
                    }
                    return RedirectToAction("ListMaterialReq");
                }

                else
                {
                    ViewBag.PageTitle = "Edit WholeStock";
                    TempData["notice"] = Strout;
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("ListMaterialReq");
        }
        public IActionResult LotNo(string id, string rowid)
        {
            MaterialRequisition MR = new MaterialRequisition();
            MR.Entered = Request.Cookies["UserId"];
            List<ItemLotNo> TData = new List<ItemLotNo>();
            ItemLotNo tda = new ItemLotNo();
            //DataTable dtt = datatrans.GetData("Select ITEMID from STORESREQDETAIL WHERE STORESREQBASICID='"+id+"' ");
            //if (dtt.Rows.Count > 0)
            //{
            //    for (int j = 0; j < dtt.Rows.Count; j++)
            //    {
            //        tda.itemid = dtt.Rows[j]["ITEMID"].ToString();
                    DataTable dt2 = materialReq.GetItemLot(id, storeid );

                    if (dt2.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            tda = new ItemLotNo();
                           // tda.invid = dt2.Rows[i]["LSTOCKVALUEID"].ToString();
                            tda.Lot = dt2.Rows[i]["LOTNO"].ToString();
                            tda.qty = dt2.Rows[i]["QTY"].ToString();
                            tda.item = dt2.Rows[i]["ITEMID"].ToString();
                            tda.itemid = dt2.Rows[i]["item"].ToString();
                             


                            TData.Add(tda);
                        }
                    }
                //}
            //}
            MR.lotlst = TData;
            return View(MR);
        }
        public IActionResult ListMaterialReq()
        {
            //IEnumerable<MaterialRequisition> cmp = materialReq.GetAllMaterial(status, st, ed);
            return View();
        }
        public ActionResult MyListMaterialRequGrid(string strStatus)
        {
            List<MaterialReqItem> Reg = new List<MaterialReqItem>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)materialReq.GetAllInventoryReq(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string Issuse = string.Empty;
                //string FollowUp = string.Empty;
                string MoveToIndent = string.Empty;
                ////string Pdf = string.Empty;
                //string View = string.Empty;
                string EditRow = string.Empty;
                string DeleteRow = string.Empty;
                if (dtUsers.Rows[i]["STATUS"].ToString() == "Approved")
                {
                    MoveToIndent = "<img src='../Images/tick.png' alt='View Details' width='20' />";
                    EditRow = "";
                }
                else
                {
                    Issuse = "<a href=ApproveReq?&id=" + dtUsers.Rows[i]["INVENTORYITEMREQID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/issue_icon.png' alt='View Details' width='20' /></a>";
                    EditRow = "<a href=MaterialReq?id=" + dtUsers.Rows[i]["INVENTORYITEMREQID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";

                }


                //if (dtUsers.Rows[i]["STATUS"].ToString() == "CLOSE")
                //{
                //    MoveToIndent = "<img src='../Images/tick.png' alt='View Details' width='20' />";
                //    EditRow = "";
                //}
                //else
                //{
                //    MoveToIndent = "<a href=IssueToindent?id=" + dtUsers.Rows[i]["INVENTORYITEMREQID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/move_quote.png' alt='View Details' width='20' /></a>";
                //}
                //View = "<a href=MaterialStatus?id=" + dtUsers.Rows[i]["INVENTORYITEMREQID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View Details' width='20' /></a>";
                DeleteRow = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["INVENTORYITEMREQID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";

                Reg.Add(new MaterialReqItem
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["INVENTORYITEMREQID"].ToString()),
                     item = dtUsers.Rows[i]["ITEMID"].ToString(),
                    location = dtUsers.Rows[i]["LOCID"].ToString(),
                    reqloc = dtUsers.Rows[i]["location"].ToString(),
                    docDate = dtUsers.Rows[i]["REQ_DATE"].ToString(),
                    qty = dtUsers.Rows[i]["REQ_QTY"].ToString(),
                    iss = Issuse,
                    //follow = FollowUp,
                    //move = MoveToIndent,
                    ////pdf = Pdf,
                    //view = View,
                    editrow = EditRow,
                    delrow = DeleteRow,



                });
            }

            return Json(new
            {
                Reg
            });

        }
        public IActionResult MaterialReq(string id)
        {
            MaterialReq MR = new MaterialReq();
            MR.Entered = Request.Cookies["UserId"];
            
            DataTable dtt = new DataTable();
            dtt = materialReq.GetReqMatItemByID(id);
            if (dtt.Rows.Count > 0)
            {


                MR.branch = dtt.Rows[0]["BRANCHID"].ToString();
                MR.branchid = dtt.Rows[0]["BRANCH_ID"].ToString();
                MR.item = dtt.Rows[0]["ITEMID"].ToString();
                MR.itemid = dtt.Rows[0]["ITEM_ID"].ToString();


                MR.location = dtt.Rows[0]["LOCID"].ToString();
                MR.locationid = dtt.Rows[0]["LOCATION_ID"].ToString();
                MR.reqlocation = dtt.Rows[0]["location"].ToString();
                MR.reqlocationid = dtt.Rows[0]["REQ_LOCID"].ToString();
                MR.reqqty = Convert.ToDouble( dtt.Rows[0]["REQ_QTY"].ToString());
                MR.docDate = dtt.Rows[0]["REQ_DATE"].ToString();
                MR.user = MR.Entered;
                MR.ID =id;
                   

                
                
            }
         
            return View(MR);
        }
        [HttpPost]
        public ActionResult MaterialReq(MaterialReq Cy, string id)
        {
            try
            {
                Cy.ID = id;
                string Strout = materialReq.MaterialReqGURD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "MaterialReq Issued Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "MaterialReq Issued Successfully...!";
                    }
                    return RedirectToAction("ListMaterialReq");
                }

                else
                {
                    ViewBag.PageTitle = "Edit MaterialReq";
                    TempData["notice"] = Strout;
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("MaterialReq");
        }

        public IActionResult ApproveReq(string id)
        {
            MaterialReq MR = new MaterialReq();
            MR.Entered = Request.Cookies["UserId"];

            DataTable dtt = new DataTable();
            dtt = materialReq.GetReqMatItemByID(id);
            if (dtt.Rows.Count > 0)
            {


                MR.branch = dtt.Rows[0]["BRANCHID"].ToString();
                MR.branchid = dtt.Rows[0]["BRANCH_ID"].ToString();
                MR.item = dtt.Rows[0]["ITEMID"].ToString();
                MR.itemid = dtt.Rows[0]["ITEM_ID"].ToString();
                MR.invid = dtt.Rows[0]["INVNTORY_ID"].ToString();


                MR.location = dtt.Rows[0]["LOCID"].ToString();
                MR.locationid = dtt.Rows[0]["LOCATION_ID"].ToString();
                MR.reqlocation = dtt.Rows[0]["location"].ToString();
                MR.reqlocationid = dtt.Rows[0]["REQ_LOCID"].ToString();
                MR.reqqty = Convert.ToDouble(dtt.Rows[0]["REQ_QTY"].ToString());
                MR.docDate = dtt.Rows[0]["REQ_DATE"].ToString();
                MR.user = MR.Entered;
                MR.ID = id;




            }

            return View(MR);
        }

        [HttpPost]
        public ActionResult ApproveReq(MaterialReq Cy, string id)
        {
            try
            {
                Cy.ID = id;
                string Strout = materialReq.ApproveReqGURD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "MaterialReq Approved Issued Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "MaterialReq Approved Issued Successfully...!";
                    }
                    return RedirectToAction("ListMaterialReq");
                }

                else
                {
                    ViewBag.PageTitle = "Edit MaterialReq";
                    TempData["notice"] = Strout;
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("MaterialReq");
        }
    }
}
