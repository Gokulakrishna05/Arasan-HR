using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Models;
using Oracle.ManagedDataAccess.Client;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

using Newtonsoft.Json.Linq;
using Arasan.Services.Master;
using Arasan.Interface.Account;
using Arasan.Services.Sales;

namespace Arasan.Controllers
{
    public class CreditorDebitNotesController : Controller
    {

        ICreditorDebitNote CreditorDebitNote;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public CreditorDebitNotesController(ICreditorDebitNote _CreditorDebitNote, IConfiguration _configuratio)
        {
            CreditorDebitNote = _CreditorDebitNote;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult CreditorDebitNotes(string id)
        {
            CreditorDebitNote ca = new CreditorDebitNote();
            ca.VTypelst = BindVTypelst();
            ca.Grouplst = BindGrouplst();
            ca.Ledgerlst = BindLedgerlst();

            ca.TDate = DateTime.Now.ToString("dd-MMM-yyyy");

            List<CreDebNoteItems> TData = new List<CreDebNoteItems>();
            CreDebNoteItems tda = new CreDebNoteItems();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new CreDebNoteItems();

                    //tda.ItemGrouplst = BindItemGrplst();
                    tda.Grplst = BindGrplst();
                    tda.Ledlst = BindLedlst();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {

            }
            ca.NoteLst = TData;
            return View(ca);
        }

        //[HttpPost]
        //public ActionResult CreditorDebitNotes(CreditorDebitNote Cy, string id)
        //{

        //    try
        //    {
        //        Cy.ID = id;
        //        Cy.branchid= Request.Cookies["BranchId"];
        //        Cy.createdby = Request.Cookies["UserId"];
        //        string Strout = CreditorDebitNote.CreditorDebitNotesCRUD(Cy);
        //        if (string.IsNullOrEmpty(Strout))
        //        {
        //            if (Cy.ID == null)
        //            {
        //                TempData["notice"] = "CreditorDebitNote Inserted Successfully...!";
        //            }
        //            else
        //            {
        //                TempData["notice"] = "CreditorDebitNote Updated Successfully...!";
        //            }
        //            return RedirectToAction("CreditorDebitNotes");
        //        }

        //        else
        //        {
        //            ViewBag.PageTitle = "Edit DebitNoteBill";
        //            TempData["notice"] = Strout;
        //            //return View();
        //        }

        //        // }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return View(Cy);
        //}

        public List<SelectListItem> BindVTypelst()
        {

            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "Credit Note", Value = "CreditNote" });
                lstdesg.Add(new SelectListItem() { Text = "Debit Note", Value = "DebitNote" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindGrouplst()
        {
            try
            {
                DataTable dtDesg = CreditorDebitNote.GetGroup();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ACCOUNTGROUP"].ToString(), Value = dtDesg.Rows[i]["ACCGROUPID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }  
          public List<SelectListItem> BindGrplst()
        {
            try
            {
                DataTable dtDesg = CreditorDebitNote.GetGrp();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ACCOUNTGROUP"].ToString(), Value = dtDesg.Rows[i]["ACCGROUPID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }  
        
        public List<SelectListItem> BindLedgerlst()
        {
            try
            {
                DataTable dtDesg = CreditorDebitNote.GetLedger();
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
         public List<SelectListItem> BindLedlst()
        {
            try
            {
                DataTable dtDesg = CreditorDebitNote.GetLed();
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
        public ActionResult GetGrpDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string led = "";

                dt = CreditorDebitNote.GetGrpDetail(ItemId);

                if (dt.Rows.Count > 0)
                {
                    led = dt.Rows[0]["LEDNAME"].ToString();
                }

                var result = new { led = led };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetGrpJSON(string ItemId)
        {
            CreditorDebitNote model = new CreditorDebitNote();
            model.Grouplst = BindGroulst(ItemId);
            return Json(BindGroulst(ItemId));

        } 
        public JsonResult GetGrpitemJSON()
        {
            //CreditorDebitNote model = new CreditorDebitNote();
            //model.Grouplst = BindGrouplst(ItemId);
            return Json(BindGrplst());

        }
        public JsonResult GetLedgerJSON(string ItemId)
        {
            CreditorDebitNote model = new CreditorDebitNote();
            model.Ledgerlst = BindLedglst(ItemId);
            return Json(BindLedglst(ItemId));

        }

        public List<SelectListItem> BindLedglst(string value)
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();

                DataTable dtDesg = CreditorDebitNote.GetLedbyId(value);
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
         public List<SelectListItem> BindGroulst(string value)
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();

                DataTable dtDesg = CreditorDebitNote.GetGRPbyId(value);
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ACCOUNTGROUP"].ToString(), Value = dtDesg.Rows[i]["ACCGROUPID"].ToString() });

                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetcrdeoDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string seq = "";
                string sequnce = "";
                string lasto = "";


               
                
                    dt = CreditorDebitNote.GetSeq(ItemId);
                    if (dt.Rows.Count > 0)
                    {
                    seq = dt.Rows[0]["PREFIX"].ToString();
                    lasto = dt.Rows[0]["LASTNO"].ToString();
                    }
                sequnce = seq + "" + lasto;

                var result = new { sequnce = sequnce };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public JsonResult GetgrpItemJSON(string itemid)
        //{
        //    DeductionItem model = new DeductionItem();
        //    model.Grplst = BindgrpItemlst(itemid);
        //    return Json(BindgrpItemlst(itemid));

        //}

        //public List<SelectListItem> BindgrpItemlst(string value)
        //{
        //    try
        //    {
        //        DataTable dtDesg = datatrans.GetGRPbyId(value);
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        for (int i = 0; i < dtDesg.Rows.Count; i++)
        //        {
        //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ACCOUNTGROUP"].ToString(), Value = dtDesg.Rows[i]["ACCGROUPID"].ToString() });
        //        }
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
