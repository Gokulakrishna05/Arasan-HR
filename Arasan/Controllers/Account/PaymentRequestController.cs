﻿using System.Collections.Generic;
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
            List<PaymentRequestDetail> TData = new List<PaymentRequestDetail>();
            PaymentRequestDetail tda = new PaymentRequestDetail();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new PaymentRequestDetail();

                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }

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
                Cy.createdby= Request.Cookies["UserId"];
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
            return View();
        }
        public ActionResult GetPaymentRequestDetails(string id,string type)
        {
            PaymentRequest model = new PaymentRequest();
            DataTable dtt = new DataTable();
            List<PaymentRequestDetail> Data = new List<PaymentRequestDetail>();
            PaymentRequestDetail tda = new PaymentRequestDetail();
            //dtt = request.GetPaymentRequestDetail1(id);
            //if (dtt.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dtt.Rows.Count; i++)
            //    {
            //        tda = new PaymentRequestDetail();
            //        tda.docid = dtt.Rows[i]["PARTYID"].ToString();
            //        tda.type = dtt.Rows[i]["POBASICID"].ToString();
            //        //tda.amount = dtt.Rows[i]["REQUESTAMOUNT"].ToString();
            //        Data.Add(tda);
            //    }
            //}
            //dtt = request.GetPaymentRequestDetail2(tda.docid);
            //if (dtt.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dtt.Rows.Count; i++)
            //    {
            //        tda = new PaymentRequestDetail();
            //        tda.docid = dtt.Rows[i]["PARTYID"].ToString();
            //        tda.type = dtt.Rows[i]["POBASICID"].ToString();
            //        //tda.amount = dtt.Rows[i]["REQUESTAMOUNT"].ToString();
            //        Data.Add(tda);
            //    }
            //}

            
            dtt = request.GetPaymentRequestDetail(id, type);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new PaymentRequestDetail();
                    tda.ID = dtt.Rows[i]["PAYMENTREQUESTID"].ToString();
                    tda.docid = dtt.Rows[i]["PO_OR_GRN"].ToString();
                    tda.type = dtt.Rows[i]["TYPE"].ToString();
                    tda.amount = dtt.Rows[i]["REQUESTAMOUNT"].ToString();
                    tda.reqby= dtt.Rows[i]["EMPNAME"].ToString();
                    tda.date= dtt.Rows[i]["DOCDATE"].ToString();
                    Data.Add(tda);
                }
            }
            
            model.PREQlst = Data;
            return Json(model.PREQlst);

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
        public JsonResult GetGRNPOJSON(string Type, string supid)
        {
            PaymentRequest model = new PaymentRequest();
            model.Typelst = BindGRNPO(Type, supid);
            return Json(BindGRNPO(Type, supid));

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
                        lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["GRNBLBASICID"].ToString() });
                    }
                }

                else
                {
                    DataTable dtDesg = request.GetPO(item);

                    for (int i = 0; i < dtDesg.Rows.Count; i++)
                    {
                        lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["POBASICID"].ToString() });
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
                        amount = dt.Rows[0]["NET"].ToString();

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

            return View();
        }


        public ActionResult MyListItemgrid(string strStatus) 
        {
            List<PaymentRequestGrid> Reg = new List<PaymentRequestGrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = request.GetAllrequest(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                EditRow = "<a href=PaymentRequest?id=" + dtUsers.Rows[i]["PAYMENTREQUESTID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["PAYMENTREQUESTID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";

                Reg.Add(new PaymentRequestGrid
                {
                    id = dtUsers.Rows[i]["PAYMENTREQUESTID"].ToString(),
                    docId = dtUsers.Rows[i]["DOCID"].ToString(),
                    date = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    type = dtUsers.Rows[i]["TYPE"].ToString(),
                    supplier = dtUsers.Rows[i]["PARTYNAME"].ToString(),
                    grn = dtUsers.Rows[i]["PO_OR_GRN"].ToString(),
                    
                    editrow = EditRow,
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }

        public ActionResult MyListItemgrids(string strStatus)
        {
            List<PaymentReqVoucherGrid> Reg = new List<PaymentReqVoucherGrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = request.GetAllpayrequests(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string Voucher = string.Empty;

                Voucher = "<a href=../PaymentVoucher/PaymentVoc?tag=payment&id=" + dtUsers.Rows[i]["PAYMENTREQUESTID"].ToString() + "><img src='../Images/checklist.png' alt='View Details' /></a>";
                //< td >< div class="fa-hover col-md-2 col-sm-4"> <a href = "@Url.Action("PaymentVoucher", "PaymentVoucher",new { id=item.ID })"><img src = '../Images/checklist.png' alt='View Details' width='20' /></a></div></td>

                Reg.Add(new PaymentReqVoucherGrid
                {
                    id = dtUsers.Rows[i]["PAYMENTREQUESTID"].ToString(),
                    docId = dtUsers.Rows[i]["DOCID"].ToString(),
                    date = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    type = dtUsers.Rows[i]["TYPE"].ToString(),
                    supplier = dtUsers.Rows[i]["PARTYNAME"].ToString(),
                    grn = dtUsers.Rows[i]["PO_OR_GRN"].ToString(),
                    amount = dtUsers.Rows[i]["AMOUNT"].ToString(),
                    voucher = Voucher,

                });
            }

            return Json(new
            {
                Reg
            });

        }
    }
}
