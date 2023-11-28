using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
 
 using Arasan.Models;
 
using Arasan.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers 
{
    public class PaymentRequestController : Controller
    {
        IPaymentRequest request;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public PaymentRequestController(IPaymentRequest _request, IConfiguration _configuratio)
        {
            request = _request;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult PaymentRequest(string id)
        {
            PaymentRequest pr = new PaymentRequest();
            pr.Typelst = BindType();
            pr.Grnlst = BindGRNPO("","");
            pr.Reqlst = BindEmp();
            pr.Branch = Request.Cookies["BranchId"];
            pr.Suplst = BindSupplier();
            pr.Date = DateTime.Now.ToString("dd-MMM-yyyy");
			DataTable dtv = datatrans.GetSequence("PAREQ");
			 
			if (dtv.Rows.Count > 0)
			{
				pr.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
			}
			if (id == null)
            {

            }
            else
            {
                DataTable dt = request.EditPaymentRequest(id);
                if (dt.Rows.Count > 0)
                {
                    pr.DocId = dt.Rows[0]["DOCID"].ToString();
                    pr.Type = dt.Rows[0]["TYPE"].ToString();
                    pr.Supplier = dt.Rows[0]["SUPPLIERID"].ToString();
                    pr.Grnlst = BindGRNPO(pr.Type,pr.Supplier);
                    pr.GRN = dt.Rows[0]["PO_OR_GRN"].ToString();
                    pr.Amount = dt.Rows[0]["AMOUNT"].ToString();
                    pr.Final = dt.Rows[0]["REQUESTAMOUNT"].ToString();
                    pr.Date = dt.Rows[0]["DOCDATE"].ToString();
                    pr.ReqBy = dt.Rows[0]["REQUESTEDBY"].ToString();

                }
            }
                return View(pr);
        }
        [HttpPost]
        public ActionResult PaymentRequest(PaymentRequest Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = request.PaymentCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "PaymentRequest Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "PaymentRequest Updated Successfully...!";
                    }
                    return RedirectToAction("ListPaymentRequest");
                }

                else
                {
                    ViewBag.PageTitle = "Edit PaymentRequest";
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
        public IActionResult ListPaymentRequest()
        {

            IEnumerable<PaymentRequest> cmp = request.GetAllPaymentRequest();
            return View(cmp);
        }
        public List<SelectListItem> BindType()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "Against Invoice", Value = "Against Invoice" });
                lstdesg.Add(new SelectListItem() { Text = "Advance Payment", Value = "Advance Payment" });
               
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
                DataTable dtDesg = request.GetSupplier();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTYNAME"].ToString(), Value = dtDesg.Rows[i]["PARTYMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetGRNPOJSON(string Type, string Item)
        {
            PaymentRequest model = new PaymentRequest();
            model.Typelst = BindGRNPO(Type, Item);
            return Json(BindGRNPO(Type, Item));

        }
        public List<SelectListItem> BindGRNPO( string type,string item)
        {
            try

            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                if (type == "Against Invoice")
                {
                    DataTable dtDesg = request.GetGRN(item);

                    for (int i = 0; i < dtDesg.Rows.Count; i++)
                    {
                        lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["DOCID"].ToString() });
                    }
                }

                else
                {
                    DataTable dtDesg = request.GetPO(item);

                    for (int i = 0; i < dtDesg.Rows.Count; i++)
                    {
                        lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["DOCID"].ToString() });
                    }
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
        public ActionResult GetGRNPODetail(string ItemId, string Type)
        {
            try
            {
                DataTable dt = new DataTable();
                string amount = "";
                string final = "";
                if (Type == "Against Invoice")
                {
                    dt = request.GetGRNDetails(ItemId);
                    if (dt.Rows.Count > 0)
                    {
                        amount = dt.Rows[0]["NET"].ToString();
                      
                    }
                }
                else
                {
                    dt = request.GetPODetails(ItemId);
                    if (dt.Rows.Count > 0)
                    {
                        amount = dt.Rows[0]["GROSS"].ToString();

                    }

                }
                var result = new { amount = amount  };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult ApprovePaymentRequest(string id)
        {
            PaymentRequest ca = new PaymentRequest();
            DataTable dt = request.EditPayment(id);
            if (dt.Rows.Count > 0)
            {
                ca.DocId = dt.Rows[0]["DOCID"].ToString();
                ca.Supplier = dt.Rows[0]["PARTYNAME"].ToString();
                ca.Final = dt.Rows[0]["REQUESTAMOUNT"].ToString();
                ca.Date = dt.Rows[0]["DOCDATE"].ToString();
                ca.GRN = dt.Rows[0]["PO_OR_GRN"].ToString();
                ca.Type = dt.Rows[0]["TYPE"].ToString();
                ca.Amount = dt.Rows[0]["AMOUNT"].ToString();
              
                
            }
            return View(ca);
        }
        [HttpPost]
        public ActionResult ApprovePaymentRequest(PaymentRequest Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = request.PaymentApprove(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    TempData["notice"] = "Payment Request Approved Successfully...!";
                    return RedirectToAction("ListPaymentRequest");
                }

                else
                {
                    ViewBag.PageTitle = "Edit PaymentRequest";
                    TempData["notice"] = Strout;
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

            string flag = request.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListPaymentRequest");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListPaymentRequest");
            }
        }
        public IActionResult ListApprovedPaymentRequest()
        {

            IEnumerable<PaymentRequest> cmp = request.GetAllApprovePaymentRequest();
            return View(cmp);
        }
    }
}
