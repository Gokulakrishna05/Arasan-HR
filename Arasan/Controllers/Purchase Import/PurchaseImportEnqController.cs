﻿using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Interface.Master;
using Arasan.Services.Master;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Xml.Linq;
//using DocumentFormat.OpenXml.Wordprocessing;

namespace Arasan.Controllers
{
    public class PurchaseImportEnqController : Controller
    {
        IPurchaseImportEnqService PurenqService;
        private string? _connectionString;
        IConfiguration? _configuratio;
        DataTransactions datatrans;

        public PurchaseImportEnqController(IPurchaseImportEnqService _PurenqService, IConfiguration _configuratio)
        {
            PurenqService = _PurenqService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult PurchaseImportEnq(String id)
        {
            PurchaseImportEnq ca = new PurchaseImportEnq();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Brlst = BindBranch();
            ca.Suplst = BindSupplier();
            ca.Curlst = BindCurrency();
            ca.EnqassignList = BindEmp();
            ca.EnqRecList = BindEmp();

            ca.EnqRecid = Request.Cookies["UserId"];
            List<ImportEnqItem> TData = new List<ImportEnqItem>();
            ImportEnqItem tda = new ImportEnqItem();

            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new ImportEnqItem();
                    //tda.ItemGrouplst = BindItemGrplst();
                    tda.Itemlst = BindItemlst();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
                ca.Cur = "1";
            }
            else
            {
                //ca = PurenqService.GetPurenqServiceById(id);



                //for (int i = 0; i < 3; i++)
                //{
                //  tda = new EnqItem();
                //  tda.ItemGrouplst = BindItemGrplst();
                //  tda.Itemlst = BindItemlst("");
                //  tda.Isvalid = "Y";
                //TData.Add(tda);

                DataTable dt = new DataTable();
                double total = 0;
                dt = PurenqService.GetPurchaseEnqDetails(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    ca.Enqdate = dt.Rows[0]["ENQDATE"].ToString();
                    ca.Supplier = dt.Rows[0]["PARTYMASTID"].ToString();
                    ca.EnqNo = dt.Rows[0]["ENQNO"].ToString();
                    ca.ID = id;
                    ca.ParNo = dt.Rows[0]["PARTYREFNO"].ToString();
                    ca.Cur = dt.Rows[0]["CURRENCYID"].ToString();
                    if (string.IsNullOrEmpty(ca.Cur))
                    {
                        ca.Cur = "1";
                    }
                    ca.ExRate = dt.Rows[0]["EXRATE"].ToString();
                    ca.RefNo = dt.Rows[0]["ENQREF"].ToString();
                    ca.Enqassignid = dt.Rows[0]["ASSIGNTO"].ToString();
                    ca.EnqRecid = Request.Cookies["UserId"];
                }
                DataTable dt2 = new DataTable();
                dt2 = PurenqService.GetPurchaseEnqItemDetails(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new ImportEnqItem();
                        double toaamt = 0;
                        //tda.ItemGrouplst = BindItemGrplst();
                        //DataTable dt3 = new DataTable();
                        //dt3 = PurenqService.GetItemSubGroup(dt2.Rows[i]["ITEMID"].ToString());
                        //if(dt3.Rows.Count > 0)
                        //{
                        //    tda.ItemGroupId = dt3.Rows[0]["SUBGROUPCODE"].ToString();
                        //}
                        tda.Itemlst = BindItemlst();
                        tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();
                        DataTable dt4 = new DataTable();
                        dt4 = PurenqService.GetItemDetails(tda.ItemId);
                        if (dt4.Rows.Count > 0)
                        {
                            tda.Desc = dt4.Rows[0]["ITEMDESC"].ToString();
                            tda.Conversionfactor = dt4.Rows[0]["CF"].ToString();
                            tda.rate = Convert.ToDouble(dt4.Rows[0]["LATPURPRICE"].ToString());
                        }
                        tda.Quantity = Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                        toaamt = tda.rate * tda.Quantity;
                        total += toaamt;
                        //tda.QtyPrim= Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                        tda.Amount = toaamt;
                        tda.Unit = dt2.Rows[i]["UNITID"].ToString();
                        //tda.unitprim= dt2.Rows[i]["UNITID"].ToString();
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }
                ca.Net = Math.Round(total, 2);

                
            }
            ca.EnqLst = TData;
            return View(ca);

        }
        public IActionResult ViewEnq(string id)
        {
            PurchaseImportQuo ca = new PurchaseImportQuo();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();
            dt = PurenqService.GetPurchaseEnqByID(id);
            if (dt.Rows.Count > 0)
            {
                ca.Supplier = dt.Rows[0]["PARTYNAME"].ToString();
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                //ca.QuoId = dt.Rows[0]["DOCID"].ToString();
                //ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                ca.EnqNo = dt.Rows[0]["ENQNO"].ToString();
                ca.EnqDate = dt.Rows[0]["ENQDATE"].ToString();
                ca.Recid = Request.Cookies["UserId"];
                ca.ID = id;
            }
            List<ImportQoItem> Data = new List<ImportQoItem>();
            ImportQoItem tda = new ImportQoItem();
            double tot = 0;
            dtt = PurenqService.GetPurchaseEnqItemDetails(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new ImportQoItem();
                    tda.ItemId = dtt.Rows[i]["ITEMNAME"].ToString();
                    tda.Unit = dtt.Rows[i]["UNITID"].ToString();
                    tda.Quantity = Convert.ToDouble(dtt.Rows[i]["QTY"].ToString());
                    tda.rate = Convert.ToDouble(dtt.Rows[i]["Rate"].ToString() == "" ? "0" : dtt.Rows[i]["Rate"].ToString());
                    tda.TotalAmount = tda.Quantity * tda.rate;
                    tot += tda.TotalAmount;
                    Data.Add(tda);
                }
            }
            ca.Net = tot;
            ca.QoLst = Data;
            return View(ca);
        }

        public IActionResult ViewImportPurEnq(string id)
        {
            PurchaseImportQuo ca = new PurchaseImportQuo();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();
            dt = PurenqService.GetPurchaseEnq(id);
            if (dt.Rows.Count > 0)
            {
                ca.Supplier = dt.Rows[0]["PARTYNAME"].ToString();
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                //ca.QuoId = dt.Rows[0]["DOCID"].ToString();
                //ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                ca.EnqNo = dt.Rows[0]["ENQNO"].ToString();
                ca.EnqDate = dt.Rows[0]["ENQDATE"].ToString();
                ca.ID = id;
            }
            List<ImportQoItem> Data = new List<ImportQoItem>();
            ImportQoItem tda = new ImportQoItem();
            double tot = 0;
            dtt = PurenqService.GetPurchaseEnqItem(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new ImportQoItem();
                    tda.ItemId = dtt.Rows[i]["ITEMNAME"].ToString();
                    tda.Unit = dtt.Rows[i]["UNITID"].ToString();
                    tda.Quantity = Convert.ToDouble(dtt.Rows[i]["QTY"].ToString());
                    tda.rate = Convert.ToDouble(dtt.Rows[i]["Rate"].ToString() == "" ? "0" : dtt.Rows[i]["Rate"].ToString());
                    tda.TotalAmount = tda.Quantity * tda.rate;
                    tot += tda.TotalAmount;
                    Data.Add(tda);
                }
            }
            ca.Net = tot;
            ca.QoLst = Data;
            return View(ca);
        }
        public IActionResult PurchaseEnquiryDetails(string id)
        {
            IEnumerable<ImportEnqItem> cmp = PurenqService.GetAllPurenquriyItem(id);
            return View(cmp);
        }
        public IActionResult RegenerateImport(string id)
        {
            PurchaseImportEnq ca = new PurchaseImportEnq();
            ca.Brlst = BindBranch();
            ca.Suplst = BindSupplier();
            ca.Curlst = BindCurrency();
            ca.EnqassignList = BindEmp();
            ca.EnqRecList = BindEmp();
            ca.Branch = Request.Cookies["BranchId"];
            ca.EnqRecid = Request.Cookies["UserId"];

            List<ImportEnqItem> TData = new List<ImportEnqItem>();
            ImportEnqItem tda = new ImportEnqItem();
            DataTable dtv = datatrans.GetSequence("Puenq");
            if (dtv.Rows.Count > 0)
            {
                ca.EnqNo = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
            DataTable dt = new DataTable();
            double total = 0;
            dt = PurenqService.GetRegenerateDetails(id);
            if (dt.Rows.Count > 0)
            {
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.Enqdate = dt.Rows[0]["ENQDATE"].ToString();
                ca.Supplier = dt.Rows[0]["PARTYMASTID"].ToString();
                //ca.EnqNo = dt.Rows[0]["ENQNO"].ToString();
                ca.ID = id;
                ca.ParNo = dt.Rows[0]["PARTYREFNO"].ToString();
                ca.Cur = dt.Rows[0]["CURRENCYID"].ToString();
                if (string.IsNullOrEmpty(ca.Cur))
                {
                    ca.Cur = "1";
                }
                ca.ExRate = dt.Rows[0]["EXRATE"].ToString();
                ca.RefNo = dt.Rows[0]["ENQREF"].ToString();
                ca.Enqassignid = dt.Rows[0]["ASSIGNTO"].ToString();
                ca.EnqRecid = Request.Cookies["UserId"];
            }
            DataTable dt2 = new DataTable();
            dt2 = PurenqService.GetPurchaseEnqItemDetails(id);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new ImportEnqItem();
                    double toaamt = 0;
                    //tda.ItemGrouplst = BindItemGrplst();
                    //DataTable dt3 = new DataTable();
                    //dt3 = PurenqService.GetItemSubGroup(dt2.Rows[i]["ITEMID"].ToString());
                    //if (dt3.Rows.Count > 0)
                    //{
                    //    tda.ItemGroupId = dt3.Rows[0]["SUBGROUPCODE"].ToString();
                    //}
                    tda.Itemlst = BindItemlst();
                    tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                    tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();
                    DataTable dt4 = new DataTable();
                    dt4 = PurenqService.GetItemDetails(tda.ItemId);
                    if (dt4.Rows.Count > 0)
                    {
                        tda.Desc = dt4.Rows[0]["ITEMDESC"].ToString();
                        tda.Conversionfactor = dt4.Rows[0]["CF"].ToString();
                        tda.rate = Convert.ToDouble(dt4.Rows[0]["LATPURPRICE"].ToString());
                    }
                    tda.Quantity = Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                    toaamt = tda.rate * tda.Quantity;
                    total += toaamt;
                    //tda.QtyPrim= Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                    tda.Amount = toaamt;
                    tda.Unit = dt2.Rows[i]["UNITID"].ToString();
                    //tda.unitprim= dt2.Rows[i]["UNITID"].ToString();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            ca.Net = Math.Round(total, 2);

            //}
            ca.EnqLst = TData;

            return View(ca);
        }
        [HttpPost]
        public ActionResult RegenerateEnq(PurchaseImportEnq Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = PurenqService.RegenerateCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    //if (Cy.ID == null)
                    //{
                    TempData["notice"] = "Regenerate Successfully...!";
                    //}
                    //else
                    //{
                    //    TempData["notice"] = "PurchaseImportEnq Updated Successfully...!";
                    //}
                    return RedirectToAction("ListEnquiry");
                }
                //else
                //{
                //    ViewBag.PageTitle = "Edit PurchaseEnquiry";
                //    TempData["notice"] = Strout;
                //    //return View();
                //}

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Cy);
        }
        public ActionResult SendMail(string id)
        {


            try
            {
                datatrans = new DataTransactions(_connectionString);
                MailRequest requestwer = new MailRequest();

                requestwer.ToEmail = "deepa@icand.in";
                requestwer.Subject = "Enquiry";
                //string Content = "";
                IEnumerable<ImportEnqItem> cmp = PurenqService.GetAllPurenquriyItem(id);
                string Content = @"<html> 
                <head>
    <style>
                table, th, td {
                border: 1px solid black;
                    border - collapse: collapse;
                }
    </style>
</head>
<body>
<p>Dear Sir,</p>
</br>
  <p> Kindly arrange to send your lowest price offer for the following items through our email immediately.</p>
</br>";




                foreach (ImportEnqItem item in cmp)
                {


                    Content += "<table><tr><td>" + item.ItemId + "-" + "</td>";
                    Content += "  <td>" + item.Quantity + "-" + "</td>";
                    Content += "  <td>" + item.Unit + "</td></tr></table>";
                }


                Content += @" </br> 
<p style='padding-left:30px;font-style:italic;'>With Regards,
</br><img src='../assets/images/Arasan_Logo.png' alt='Arasan Logo'/>
</br>N Balaji Purchase Manager
</br>The Arasan Aluminium Industries (P) Ltd.
<br/102-A

</br>
</p> ";
                Content += @"</body> 
</html> ";

                requestwer.Body = Content;
                //request.Attachments = "No";
                datatrans.sendemail("Test mail", Content, "kesavanmoorthi81@gmail.com", "kesavanmoorthi70@gmail.com", "spabnjcirlfipjco", "587", "true", "smtp.gmail.com", "IcanD");
                return Ok();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        [HttpPost]
        public ActionResult PurchaseImportEnq(PurchaseImportEnq Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = PurenqService.PurenquriyCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "PurchaseImportEnq Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "PurchaseImportEnq Updated Successfully...!";
                    }
                    return RedirectToAction("ListPurchaseImportEnq");
                }

                else
                {
                    ViewBag.PageTitle = "Edit PurchaseImportEnq";
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

        [HttpPost]
        public ActionResult ViewEnq(PurchaseImportQuo Cy, string id)
        {
            try
            {
                Cy.ID = id;
                string Strout = PurenqService.EnquirytoQuote(Cy.ID);
                //datatrans.UpdateStatus("UPDATE PURENQBASIC SET STATUS = 2 where PURENQBASICID = '" + id + "'");
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Quote Generated Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Quote Generated Successfully...!";
                    }
                    return RedirectToAction("ListEnquiry");
                }

                else
                {
                    ViewBag.PageTitle = "Edit PurchaseImportEnq";
                    TempData["notice"] = Strout;
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("ListPurchaseImportQuo");
        }
        public IActionResult ListPurchaseImportEnq()
        {
            //IEnumerable<PurchaseEnquiry> cmp = PurenqService.GetAllPurenquriy(st,ed);
            return View();
        }
        public ActionResult MyListPurchaseEnquiryGrid(string strStatus)
        {
            List<PurchaseImportEnquiryItems> Reg = new List<PurchaseImportEnquiryItems>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)PurenqService.GetAllPurchaseEnquiryItems(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string MailRow = string.Empty;
                string FollowUp = string.Empty;
                string MoveToQuo = string.Empty;
                string Regenerate = string.Empty;
                string View = string.Empty;
                string EditRow = string.Empty;
                string DeleteRow = string.Empty;

                MailRow = "<a href=SendMail?tag=Del&id=" + dtUsers.Rows[i]["IPURENQBASICID"].ToString() + "><img src='../Images/mail_icon.png' alt='Send Email' /></a>";
                FollowUp = "<a href=FollowupImport?id=" + dtUsers.Rows[i]["IPURENQBASICID"].ToString() + "><img src='../Images/followup.png' /></a>";
                //if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "N")
                //{


                //}
                //else
                //{
                if (dtUsers.Rows[i]["STATUS"].ToString() == "Quote")
                {
                    MoveToQuo = "<img src='../Images/tick.png' alt='View Details' width='20' />";
                    EditRow = "";

                }
                else
                {
                    MoveToQuo = "<a href=ViewEnq?id=" + dtUsers.Rows[i]["IPURENQBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/move_quote.png' alt='View Details' width='20' /></a>";
                    EditRow = "<a href=PurchaseImportEnq?id=" + dtUsers.Rows[i]["IPURENQBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";

                }
                //}
                Regenerate = "<a href=RegenerateImport?id=" + dtUsers.Rows[i]["IPURENQBASICID"].ToString() + "><img src='../Images/Change.png' alt='Edit' /></a>";
                View = "<a href=ViewImportPurEnq?id=" + dtUsers.Rows[i]["IPURENQBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View Details' width='20' /></a>";
                //EditRow = "<a href=PurchaseEnquiry?id=" + dtUsers.Rows[i]["PURENQBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                DeleteRow = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["IPURENQBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";

                Reg.Add(new PurchaseImportEnquiryItems
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["IPURENQBASICID"].ToString()),
                    branch = dtUsers.Rows[i]["BRANCHID"].ToString(),
                    docNo = dtUsers.Rows[i]["ENQNO"].ToString(),
                    docDate = dtUsers.Rows[i]["ENQDATE"].ToString(),
                    supplier = dtUsers.Rows[i]["PARTYNAME"].ToString(),
                    mailrow = MailRow,
                    follow = FollowUp,
                    move = MoveToQuo,
                    reg = Regenerate,
                    view = View,
                    editrow = EditRow,
                    delrow = DeleteRow,



                });
            }

            return Json(new
            {
                Reg
            });

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
        public ActionResult MovetoQuote(string id)
        {
            EnquiryList el = new EnquiryList();
            string Strout = PurenqService.EnquirytoQuote(id);
            return RedirectToAction("ListEnquiry");
        }
        public List<SelectListItem> BindSupplier()
        {
            try
            {
                DataTable dtDesg = PurenqService.GetSupplier();
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

        public List<SelectListItem> BindItemGrplst()
        {
            try
            {
                DataTable dtDesg = PurenqService.GetItemSubGrp();
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

        public List<SelectListItem> BindEmp()
        {
            try
            {
                DataTable dtDesg = PurenqService.GetEmp();
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
        public List<SelectListItem> BindEmpFolw()
        {
            try
            {
                DataTable dtDesg = PurenqService.GetEmp();
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

        public JsonResult GetItemJSON()
        {
            ImportEnqItem model = new ImportEnqItem();
            model.Itemlst = BindItemlst();
            return Json(BindItemlst());

        }


        public ActionResult GetItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                string Desc = "";
                string unit = "";
                string CF = "";
                string price = "";
                dt = PurenqService.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    Desc = dt.Rows[0]["ITEMDESC"].ToString();
                    unit = dt.Rows[0]["UNITID"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();
                    dt1 = PurenqService.GetItemCF(ItemId, dt.Rows[0]["UNITMASTID"].ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        CF = dt1.Rows[0]["CF"].ToString();
                    }
                }

                var result = new { Desc = Desc, unit = unit, CF = CF, price = price };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public JsonResult GetItemGrpJSON()
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItemGrplst());
        }
        public IActionResult FollowupImport(string id)
        {
            PurchaseImportFollowup cmp = new PurchaseImportFollowup();
            cmp.EnqassignList = BindEmpFolw();
            cmp.Followdate = DateTime.Now.ToString("dd-MMM-yyyy");
            List<PurchaseImportFollowupDetails> TData = new List<PurchaseImportFollowupDetails>();
            if (id == null)
            {

            }
            else
            {
                if (!string.IsNullOrEmpty(id))
                {
                    cmp.Enqid = id;
                    DataTable dt = new DataTable();
                    dt = PurenqService.GetPurchaseEnqFolwDetails(id);
                    if (dt.Rows.Count > 0)
                    {
                        cmp.EnqNo = dt.Rows[0]["ENQNO"].ToString();
                        cmp.Supname = dt.Rows[0]["PARTYID"].ToString();
                    }

                    DataTable dtt = new DataTable();
                    string e = cmp.EnqNo;
                    dtt = PurenqService.GetFolowup(e);
                    PurchaseImportFollowupDetails tda = new PurchaseImportFollowupDetails();

                    if (dtt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtt.Rows.Count; i++)
                        {
                            tda = new PurchaseImportFollowupDetails();
                            tda.Followby = dtt.Rows[i]["FOLLOWED_BY"].ToString();
                            tda.Followdate = dtt.Rows[i]["FOLLOW_DATE"].ToString();
                            tda.Nfdate = dtt.Rows[i]["NEXT_FOLLOW_DATE"].ToString();

                            tda.Rmarks = dtt.Rows[i]["REMARKS"].ToString();
                            tda.Enquiryst = dtt.Rows[i]["FOLLOW_STATUS"].ToString();
                            TData.Add(tda);
                        }
                    }
                }
            }
            cmp.pflst = TData;
            return View(cmp);
        }

        //public IActionResult Followup()
        //{

        //    IEnumerable<PurchaseFollowup> cmp = PurenqService.GetAllPurchaseFollowup();
        //    return View(cmp);
        //}


        [HttpPost]
        public ActionResult Followup(PurchaseImportFollowup Pf, string id)
        {

            try
            {
                Pf.FoID = id;
                string Strout = PurenqService.PurchaseFollowupCRUD(Pf);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Pf.FoID == null)
                    {
                        TempData["notice"] = "Followup Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Followup Updated Successfully...!";
                    }

                }

                else
                {
                    ViewBag.PageTitle = "Edit Followup";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("Followup", new { id = Pf.Enqid });
        }

        public IActionResult POFollowup()
        {
            return View();
        }
        //public IActionResult ListPurchaseEnquiry()
        //{
        //    return View();
        //}

        //public ActionResult MyListEnquirygrid(EnquiryList CL)
        //{
        //    EnquiryList objProductsData = new EnquiryList();
        //    List<EnquiryBindList> Reg = new List<EnquiryBindList>();
        //    DataTable dtUsers = new DataTable();

        //    dtUsers = objProductsData.GetEnquiry();
        //    for (int i = 0; i < dtUsers.Rows.Count; i++)
        //    {

        //        string DeleteRow = string.Empty;
        //        string EditRow = string.Empty;
        //        string FollowUp = string.Empty;
        //        string MoveQuote = string.Empty;
        //        string SendMail = string.Empty;

        //        SendMail = "<a href=SendMail?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/mail_icon.png' alt='Send Email' /></a>";
        //        EditRow = "<a href=Enquiry?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
        //        FollowUp = "<a href=EnquiryFollowup?Fid=" + dtUsers.Rows[i]["ID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/followup.png' alt='FollowUp' /> - (" + 1 + ")</a>";
        //        DeleteRow = "<a href=DeleteEnquiry?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + " onclick='return confirm(" + "\"Are you sure you want to Disable this record...?\"" + ")'><img src='../Images/Inactive.png' alt='Deactivate' /></a>";




        //        Reg.Add(new EnquiryBindList
        //        {
        //            PRID = Convert.ToInt32(dtUsers.Rows[i]["ID"].ToString()),
        //            SuppName = dtUsers.Rows[i]["SuppName"].ToString(),
        //            Branch = dtUsers.Rows[i]["Branch"].ToString(),
        //            EnqNo = dtUsers.Rows[i]["EnqNo"].ToString(),
        //            EnqDate = dtUsers.Rows[i]["EnqDate"].ToString(),
        //            Currency = dtUsers.Rows[i]["Currency"].ToString(),
        //            SendMail = SendMail,
        //            EditRow = EditRow,
        //            DelRow = DeleteRow,
        //            FollowUp = FollowUp

        //        });
        //    }

        //    return Json(new
        //    {
        //        Reg
        //    });

        //}


        //public ActionResult ListEnquiryItemgrid(EnquiryList SL, string ENQID)
        //{
        //    EnquiryList objSpareData = new EnquiryList();
        //    List<EnquiryItemBindList> EnqChkItem = new List<EnquiryItemBindList>();
        //    DataTable dtEnq = new DataTable();
        //    dtEnq = objSpareData.GetEnquiryItem(ENQID);
        //    for (int i = 0; i < dtEnq.Rows.Count; i++)
        //    {
        //        EnqChkItem.Add(new EnquiryItemBindList
        //        {
        //            OrderID = Convert.ToInt32(dtEnq.Rows[i]["CALL_ID"].ToString()),
        //            PRID = Convert.ToInt32(dtEnq.Rows[i]["ID"].ToString()),
        //            ProName = dtEnq.Rows[i]["CATEGORY_NAME"].ToString(),
        //            // SUB_CATEGORY = dtEnq.Rows[i]["PART_NO"].ToString(),
        //            Unit = dtEnq.Rows[i]["PRODUCT_NAME"].ToString(),
        //            Quantity = dtEnq.Rows[i]["QUANTITY"].ToString(),
        //            Rate = dtEnq.Rows[i]["DESCRIPTION"].ToString(),
        //            Amount = dtEnq.Rows[i]["UNIT_PRICE"].ToString()
        //        });
        //    }

        //    return Json(new
        //    {
        //        EnqChkItem
        //    });
        //}

        public ActionResult DeleteItem(string tag, string id)
        {

            string flag = PurenqService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListEnquiry");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListEnquiry");
            }
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


        public IActionResult ListPO()
        {
            return View();
        }
        public IActionResult ListGRN()
        {
            return View();
        }

    }
}
