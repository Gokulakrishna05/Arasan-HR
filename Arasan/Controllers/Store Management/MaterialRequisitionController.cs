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
            MR.Worklst = BindWorkCenter("");
            MR.Processlst = BindProcess("");
            MR.assignList = BindEmp();
            MR.Statuslst = BindStatus();
            MR.Branch = Request.Cookies["BranchId"];
            MR.Entered= Request.Cookies["UserId"];
            MR.Location= Request.Cookies["LocationName"];
            MR.DocDa = DateTime.Now.ToString("dd-MMM-yyyy");
            List<MaterialRequistionItem> TData = new List<MaterialRequistionItem>();
            MaterialRequistionItem tda = new MaterialRequistionItem();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new MaterialRequistionItem();
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
                    //MR.DocId = dt.Rows[0]["DOCID"].ToString();
                    MR.DocDa = dt.Rows[0]["DOCDATE"].ToString();

                }

                DataTable dtt = new DataTable();
                dtt = materialReq.GetmaterialReqItemDetails(id);
                if (dt.Rows.Count > 0)
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
                        tda.Itemlst = BindItemlst(tda.ItemGroupId);
                        tda.ItemId = dtt.Rows[i]["ITEMID"].ToString();
                        tda.Unit = dtt.Rows[i]["UNITID"].ToString();
                        tda.ReqQty = dtt.Rows[i]["QTY"].ToString();
                        DataTable dt1 = materialReq.Getstkqty(dtt.Rows[i]["ITEMMASTERID"].ToString(), storeid, dt.Rows[0]["BRANCHID"].ToString());
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

        public List<SelectListItem> BindItemlst(string value)
        {
            try
            {
                DataTable dtDesg = materialReq.GetItem(value);
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
        public ActionResult GetItemDetail(string ItemId,string branch)
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
                    unitid= dt.Rows[0]["UNITMASTID"].ToString();
                }
                dt1 = materialReq.Getstkqty(ItemId, storeid,branch);
                if(dt1.Rows.Count > 0)
                {
                    stk= dt1.Rows[0]["QTY"].ToString();
                }
                if(stk == "")
                {
                    stk = "0";
                }
                var result = new { unit = unit , stk = stk , unitid = unitid };
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
            IEnumerable<MaterialRequisition> cmp = materialReq.GetAllMaterial();
            return View(cmp);
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
                MR.DocId= dt.Rows[0]["DOCID"].ToString();
                MR.DocDa= dt.Rows[0]["DOCDATE"].ToString();
                MR.WorkCenter = dt.Rows[0]["WCID"].ToString();
                MR.Process = dt.Rows[0]["PROCESSNAME"].ToString();

                MR.RequestType = dt.Rows[0]["REQTYPE"].ToString();
                MR.BranchId = dt.Rows[0]["BRANCHIDS"].ToString();
                MR.LocationId= dt.Rows[0]["FROMLOCID"].ToString();
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
                    tda.ItemId =  dtt.Rows[i]["ITEMMASTERID"].ToString();
                    tda.UnitID = dtt.Rows[i]["UNIT"].ToString();
                    tda.Unit = dtt.Rows[i]["UNITID"].ToString();
                    tda.ReqQty = dtt.Rows[i]["QTY"].ToString();
                    tda.indentid= dtt.Rows[i]["STORESREQDETAILID"].ToString();
                    tda.Isvalid = "Y";
                    double reqqty = Convert.ToDouble(dtt.Rows[i]["QTY"].ToString()); 
                    DataTable dt1 = materialReq.Getstkqty(dtt.Rows[i]["ITEMMASTERID"].ToString(), storeid, dt.Rows[0]["BRANCHIDS"].ToString());
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
                    if(!string.IsNullOrEmpty(tda.ClosingStock))
                    {
                        stkqty = Convert.ToDouble(tda.ClosingStock);
                    }
                    if(stkqty > reqqty)
                    {
                        tda.InvQty = reqqty;
                        tda.IndQty = 0;
                    }
                    else
                    {
                        tda.InvQty = stkqty;
                        tda.IndQty = (reqqty- stkqty);
                    }
                    //tda.Itemlst = BindItemlst();
                  
                    TData.Add(tda);
                }
            }
            MR.MRlst=TData;
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
                MR.DocId = dt.Rows[0]["DOCID"].ToString();
                MR.DocDa = dt.Rows[0]["DOCDATE"].ToString();
                MR.RequestType = dt.Rows[0]["REQTYPE"].ToString();
                MR.BranchId = dt.Rows[0]["BRANCHIDS"].ToString();
                MR.LocationId = dt.Rows[0]["FROMLOCID"].ToString();
                MR.MaterialReqId = id;
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
                    double reqqty = Convert.ToDouble(dtt.Rows[i]["QTY"].ToString());
                    DataTable dt1 = materialReq.Getstkqty(dtt.Rows[i]["ITEMMASTERID"].ToString(), storeid, dt.Rows[0]["BRANCHIDS"].ToString());
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
        public JsonResult GetItemJSON(string itemid)
        {
            MaterialRequistionItem model = new MaterialRequistionItem();
            model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst(itemid));

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
                    tda.Isvalid = "Y";
                    double reqqty = Convert.ToDouble(dtt.Rows[i]["QTY"].ToString());
                    DataTable dt1 = materialReq.Getstkqty(dtt.Rows[i]["ITEMMASTERID"].ToString(), dt.Rows[0]["FROMLOCID"].ToString(), dt.Rows[0]["BRANCHIDS"].ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        tda.ClosingStock = dt1.Rows[0]["QTY"].ToString();
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

        public ActionResult DeleteMR(string tag, int id)
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
        
    }
}
