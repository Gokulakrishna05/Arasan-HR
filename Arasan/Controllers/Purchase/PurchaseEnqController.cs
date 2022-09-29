using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Interface.Master;

namespace Arasan.Controllers
{
    public class PurchaseEnqController : Controller
    {
        IPurchaseEnqService PurenqService;
        public PurchaseEnqController(IPurchaseEnqService _PurenqService)
        {
            PurenqService = _PurenqService;
        }
        public IActionResult PurchaseEnquiry()
        {
            PurchaseEnquiry ca = new PurchaseEnquiry();
            ca.Brlst = BindBranch();
            ca.Suplst = BindSupplier();
            ca.Curlst = BindCurrency();
            List<EnqItem> TData = new List<EnqItem>();
            EnqItem tda = new EnqItem();
            for (int i = 0; i < 3; i++)
            {
                tda = new EnqItem();
                tda.Itemlst = BindItemlst();
              TData.Add(tda);
            }
            ca.EnqLst = TData;
          return View(ca);
        }

        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dtDesg = PurenqService.GetBranch();
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

        public List<SelectListItem> BindSupplier()
        {
            try
            {
                DataTable dtDesg = PurenqService.GetSupplier();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTY"].ToString(), Value = dtDesg.Rows[i]["PARTYMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindCurrency()
        {
            try
            {
                DataTable dtDesg = PurenqService.GetCurency();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["Cur"].ToString(), Value = dtDesg.Rows[i]["CURRENCYID"].ToString() });
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
                DataTable dtDesg = PurenqService.GetItem();
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

        [HttpPost]
        public JsonResult GetItemJSON()
        {
            return Json(BindItemlst());

        }
        public IActionResult PurchaseFollowup()
        {
            return View();
        }
        public IActionResult POFollowup()
        {
            return View();
        }
        public IActionResult PurchaseQuotationFollowup()
        {
            return View();
        }
        public IActionResult ListPurchaseEnquiry()
        {
            return View();
        }

        public IActionResult Direct_Purchase()
        {
            return View();
        }

        public IActionResult Purchase_Quotations()
        {
            return View();
        }

        public IActionResult GRN_CUM_BILL()
        {
            return View();
        }
        public IActionResult Purchase_Order()
        {
            return View();
        }
        public IActionResult Purchse_Order_close()
        {
            return View();
        }
        public IActionResult Purchse_Indent()
        {
            return View();
        }

        public IActionResult Purchase_Order_ament()
        {
            return View();
        }
        public IActionResult ListEnquiry()
        {
            return View();
        }
        public IActionResult ListPurchaseQuotation()
        {
            return View();
        }
        public IActionResult ListPO()
        {
            return View();
        }
        public IActionResult ListGRN()
        {
            return View();
        }
        public ActionResult MyListEnquirygrid(EnquiryList CL)
        {
            EnquiryList objProductsData = new EnquiryList();
            List<EnquiryBindList> Reg = new List<EnquiryBindList>();
            DataTable dtUsers = new DataTable();

            dtUsers = objProductsData.GetEnquiry();
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string FollowUp = string.Empty;
                string MoveQuote = string.Empty;
                string SendMail = string.Empty;

                SendMail = "<a href=SendMailPdf?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/mail_icon.png' alt='Send Email' /></a>";
                EditRow = "<a href=Enquiry?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                FollowUp = "<a href=EnquiryFollowup?Fid=" + dtUsers.Rows[i]["ID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/followup.png' alt='FollowUp' /> - (" + 1 + ")</a>";
                DeleteRow = "<a href=DeleteEnquiry?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + " onclick='return confirm(" + "\"Are you sure you want to Disable this record...?\"" + ")'><img src='../Images/Inactive.png' alt='Deactivate' /></a>";




                Reg.Add(new EnquiryBindList
                {
                    PRID = Convert.ToInt32(dtUsers.Rows[i]["ID"].ToString()),
                    SuppName = dtUsers.Rows[i]["SuppName"].ToString(),
                    Branch = dtUsers.Rows[i]["Branch"].ToString(),
                    EnqNo = dtUsers.Rows[i]["EnqNo"].ToString(),
                    EnqDate = dtUsers.Rows[i]["EnqDate"].ToString(),
                    Currency = dtUsers.Rows[i]["Currency"].ToString(),
                    SendMail = SendMail,
                    EditRow = EditRow,
                    DelRow = DeleteRow,
                    FollowUp = FollowUp

                });
            }

            return Json(new
            {
                Reg
            });

        }
        

        public ActionResult ListEnquiryItemgrid(EnquiryList SL, string ENQID)
        {
            EnquiryList objSpareData = new EnquiryList();
            List<EnquiryItemBindList> EnqChkItem = new List<EnquiryItemBindList>();
            DataTable dtEnq = new DataTable();
            dtEnq = objSpareData.GetEnquiryItem(ENQID);
            for (int i = 0; i < dtEnq.Rows.Count; i++)
            {
                EnqChkItem.Add(new EnquiryItemBindList
                {
                    OrderID = Convert.ToInt32(dtEnq.Rows[i]["CALL_ID"].ToString()),
                    PRID = Convert.ToInt32(dtEnq.Rows[i]["ID"].ToString()),
                    ProName = dtEnq.Rows[i]["CATEGORY_NAME"].ToString(),
                    // SUB_CATEGORY = dtEnq.Rows[i]["PART_NO"].ToString(),
                    Unit = dtEnq.Rows[i]["PRODUCT_NAME"].ToString(),
                    Quantity = dtEnq.Rows[i]["QUANTITY"].ToString(),
                    Rate = dtEnq.Rows[i]["DESCRIPTION"].ToString(),
                    Amount = dtEnq.Rows[i]["UNIT_PRICE"].ToString()
                });
            }

            return Json(new
            {
                EnqChkItem
            });
        }
    }
}
