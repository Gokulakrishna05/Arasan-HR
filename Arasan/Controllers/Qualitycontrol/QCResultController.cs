using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Interface.Qualitycontrol;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Sales;
using Arasan.Services.Store_Management;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers.Qualitycontrol
{
    public class QCResultController : Controller
    {
        IQCResultService QCResultService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public QCResultController(IQCResultService _QCResultService, IConfiguration _configuratio)
        {
            QCResultService = _QCResultService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult QCResult(string id)
        {
            QCResult ca = new QCResult();
            ca.lst = BindGRNlist();
            ca.assignList = BindEmp();
            ca.Loc = BindLocation();
            ca.Branch = Request.Cookies["BranchId"];
            DataTable dtv = datatrans.GetSequence("Qinin");
            ca.DocDate = DateTime.Now.ToString("dd-MMM-yyyy");
            if (dtv.Rows.Count > 0)
            {
                ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
            List<QCResultItem> TData = new List<QCResultItem>();
            QCResultItem tda = new QCResultItem();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new QCResultItem();
                    tda.Itemlst = BindItemlst("");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                //ca = QCResultService.GetQCResultById(id);

                DataTable dt = new DataTable();

                dt = QCResultService.GetQCResult(id);
                if (dt.Rows.Count > 0)
                {
                    ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.GRNNo = dt.Rows[0]["GRNNO"].ToString();
                    ca.GRNDate = dt.Rows[0]["GRNDATE"].ToString();          
                    ca.ID = id;
                    ca.Supplst = BindSupplst(ca.GRNNo);
                    ca.Party = dt.Rows[0]["PARTYID"].ToString();
                    ca.Location = dt.Rows[0]["LOCATION"].ToString();
                    ca.Remarks = dt.Rows[0]["REMARKS"].ToString();
                    ca.QcLocation = dt.Rows[0]["QCLOCATION"].ToString();
                    ca.TestedBy = dt.Rows[0]["TESTEDBY"].ToString();
                }
                DataTable dt2 = new DataTable();
                dt2 = QCResultService.GetQCResultDetail(id);
                if (dt2.Rows.Count > 0)
                {

                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new QCResultItem();
                        tda.Itemlst = BindItemlst(ca.GRNNo);
                        tda.ItemId = dt2.Rows[0]["ITEMID"].ToString();
                        tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.GrnQty = dt2.Rows[0]["GRNQTY"].ToString();
                        //tda.InsQty = dt2.Rows[0]["INSQTY"].ToString();
                        tda.RejQty = dt2.Rows[0]["REJQTY"].ToString();
                        tda.AccQty = dt2.Rows[0]["ACCQTY"].ToString();
                        tda.CostRate = dt2.Rows[0]["COSTRATE"].ToString();
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }

                }

            }
            ca.QResLst = TData;
            return View(ca);
        }
        [HttpPost]
        public ActionResult QCResult(QCResult Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = QCResultService.QCResultCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "QCResult Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "QCResult Updated Successfully...!";
                    }
                    return RedirectToAction("ListQCResult");

                }
                else
                {
                    ViewBag.PageTitle = "Edit QCResult";
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
        public IActionResult ListQCResult(string st,string ed)
        {

            IEnumerable<QCResult> cmp = QCResultService.GetAllQCResult(st,ed);
            return View(cmp);
        }
        public List<SelectListItem> BindGRNlist()
        {
            try
            {
                DataTable dtDesg = QCResultService.GetGRN();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                   
                        lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["GRNNO"].ToString() });
                 
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetItemJSON()
        {
            QCResult model = new QCResult();
            //model.Itemlst = BindItemlst(itemid);
            return Json(model);

        }
        //public List<SelectListItem> BindType()
        //{
        //    try
        //    {
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        lstdesg.Add(new SelectListItem() { Text = "GRN", Value = "GRN" });
        //        lstdesg.Add(new SelectListItem() { Text = "PO", Value = "PO" });


        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public ActionResult GetGRNDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                string grndate = "";
                string party = "";

                dt = QCResultService.GetGRNDetails(ItemId);
                if (dt.Rows.Count > 0)
                {
                    grndate = dt.Rows[0]["DOCDATE"].ToString();
                    party = dt.Rows[0]["PARTYNAME"].ToString();

                }

                var result = new { grndate = grndate, party = party };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public JsonResult GetPartyJSON(string ItemId)
        //{
        //    QCResult model = new QCResult();
        //    model.Supplst = BindSupplst(ItemId);
        //    return Json(BindSupplst(ItemId));

        //}
        public JsonResult GetGRNItemJSON(string supid)
        {
            QCResultItem model = new QCResultItem();
            model.Itemlst = BindItemlst(supid);
            return Json(BindItemlst(supid));

        }
        public JsonResult GetGRNSuppJSON(string suppid)
        {
            QCResult model = new QCResult();
            model.Supplst = BindSupplst(suppid);
            return Json(BindSupplst(suppid));

        }

        //public List<SelectListItem> BindPartylst(string value)
        //{
        //    try
        //    {
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();

        //        DataTable dtDesg = QCResultService.GetPartybyId(value);
        //        for (int i = 0; i < dtDesg.Rows.Count; i++)
        //        {
        //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTYNAME"].ToString(), Value = dtDesg.Rows[i]["PARTYMASTID"].ToString() });

        //        }
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public List<SelectListItem> BindItemlst(string value)
        {
            try
            {
                DataTable dtDesg = QCResultService.GetItembyId(value);
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
        public List<SelectListItem> BindSupplst(string value)
        {
            try
            {
                DataTable dtDesg = QCResultService.GetParty(value);
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
        public ActionResult GetGRNItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
            

             
                string qty = "";
                string accqty = "";

                string rejqty = "";
                string cost = "";

                dt = QCResultService.GetGRNItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {                   
                    qty = dt.Rows[0]["QTY"].ToString();
                    accqty = dt.Rows[0]["ACCQTY"].ToString();
                    rejqty = dt.Rows[0]["REJQTY"].ToString();
                    cost = dt.Rows[0]["COSTRATE"].ToString();

                }

                var result = new {  qty = qty, accqty = accqty, rejqty = rejqty, cost = cost };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = QCResultService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListQCResult");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListQCResult");
            }
        }

        public IActionResult ViewQCResult(string id)
        {
            QCResult ca = new QCResult();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();

            dt = QCResultService.GetViewQCResult(id);
            if (dt.Rows.Count > 0)
            {
                ca.DocId = dt.Rows[0]["DOCID"].ToString();
                ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                ca.GRNNo = dt.Rows[0]["GRNNO"].ToString();
                ca.GRNDate = dt.Rows[0]["GRNDATE"].ToString();
                ca.Party = dt.Rows[0]["PARTYID"].ToString();
                ca.Location = dt.Rows[0]["LOCATION"].ToString();
                ca.Remarks = dt.Rows[0]["REMARKS"].ToString();
                ca.QcLocation = dt.Rows[0]["QCLOCATION"].ToString();
                ca.TestedBy = dt.Rows[0]["TESTEDBY"].ToString();

                ca.ID = id;


                List<QCResultItem> Data = new List<QCResultItem>();
                QCResultItem tda = new QCResultItem();
                //double tot = 0;

                dtt = QCResultService.GetViewQCResultDetail(id);
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda.ItemId = dtt.Rows[0]["ITEMID"].ToString();
                        tda.GrnQty = dtt.Rows[0]["GRNQTY"].ToString();
                        tda.RejQty = dtt.Rows[0]["REJQTY"].ToString();
                        tda.AccQty = dtt.Rows[0]["ACCQTY"].ToString();
                        tda.CostRate = dtt.Rows[0]["COSTRATE"].ToString();
                        Data.Add(tda);
                    }
                }

                ca.QResLst = Data;
            }
            return View(ca);
        }

    }
}
