using System.Collections.Generic;
using Arasan.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Models;
using AspNetCore.Reporting;
using NuGet.Packaging.Signing;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
namespace Arasan.Controllers
{
    public class PurchaseQuoController : Controller
    {
        IPurchaseQuo PurquoService;
        private string? _connectionString;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        IConfiguration? _configuratio;
        DataTransactions datatrans;
        public PurchaseQuoController(IPurchaseQuo _PurquoService, IConfiguration _configuratio, IWebHostEnvironment WebHostEnvironment)
        {
            this._WebHostEnvironment = WebHostEnvironment;
            PurquoService = _PurquoService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        }
        public IActionResult PurchaseQuotation(string id)
        {
            PurchaseQuo ca = new PurchaseQuo();
            ca.Brlst =  BindBranch();
            ca.Suplst = BindSupplier();
            ca.Curlst = BindCurrency();
            ca.RecList = BindEmp();
            ca.user = Request.Cookies["UserId"];
            ca.Recid = Request.Cookies["UserId"];
            ca.assignList = BindEmp();
            List<QoItem> Data = new List<QoItem>();
            QoItem tda = new QoItem();
            if (id == null)
            {
               
            }
            else
            {
                DataTable dt = new DataTable();
                double total = 0;
                dt = PurquoService.GetPurchaseQuo(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.Supplier = dt.Rows[0]["PARTYID"].ToString();
                    ca.EnqNo = dt.Rows[0]["ENQNO"].ToString();
                    ca.Enq = dt.Rows[0]["enq"].ToString();
                    ca.EnqDate= dt.Rows[0]["ENQDATE"].ToString();
                    ca.ExRate = dt.Rows[0]["EXRATE"].ToString();
                    ca.ID = id;
                    //ca.ParNo = dt.Rows[0]["PARTYREFNO"].ToString();
                    ca.Currency = dt.Rows[0]["MAINCURRENCY"].ToString();
                    //ca.ExRate = dt.Rows[0]["EXCRATERATE"].ToString();
                    ca.QuoId = dt.Rows[0]["DOCID"].ToString();

                }
                //ca = PurquoService.GetPurQuotationById(id);
                DataTable dt2 = new DataTable();
                dt2 = PurquoService.GetPurchaseQuoItemDetails(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new QoItem();
                        double toaamt = 0;
                        tda.ItemGrouplst = BindItemGrplst();
                        DataTable dt3 = new DataTable();
                        dt3 = datatrans.GetItemSubGroup(dt2.Rows[i]["ITEMID"].ToString());
                        if (dt3.Rows.Count > 0)
                        {
                            tda.ItemGroupId = dt3.Rows[0]["SUBGROUPCODE"].ToString();
                        }
                        tda.Ilst = BindItemlst(tda.ItemGroupId);
                        tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();
                        DataTable dt4 = new DataTable();
                        dt4 = datatrans.GetItemDetails(tda.ItemId);
                        if (dt4.Rows.Count > 0)
                        {
                            tda.Desc = dt4.Rows[0]["ITEMDESC"].ToString();
                            tda.ConsFa = dt4.Rows[0]["CF"].ToString();
                            tda.rate = Convert.ToDouble(dt4.Rows[0]["LATPURPRICE"].ToString());
                        }
                        tda.Quantity = Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                        toaamt = tda.rate * tda.Quantity;
                        total += toaamt;
                        //tda.QtyPrim= Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                        tda.TotalAmount = toaamt;
                        tda.Unit = dt2.Rows[i]["UNITID"].ToString();
                        //tda.unitprim= dt2.Rows[i]["UNITID"].ToString();
                        tda.Isvalid = "Y";
                        Data.Add(tda);
                    }
                }
                ca.Net = Math.Round(total, 2);
                ca.QoLst = Data;
            }
            return View(ca);
        }
            [HttpPost]
        public ActionResult PurchaseQuotation(PurchaseQuo Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = PurquoService.PurQuotationCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "PurchaseQuo Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "PurchaseQuo Updated Successfully...!";
                    }
                    return RedirectToAction("ListPurchaseQuo");
                }

                else
                {
                    ViewBag.PageTitle = "Edit PurchaseQuotation";
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
        public ActionResult MyListPurchaseQuoGrid(string strStatus)
        {
            List<PurchaseQuoItems> Reg = new List<PurchaseQuoItems>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)PurquoService.GetAllPurchaseQuoItems(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string MailRow = string.Empty;
                string FollowUp = string.Empty;
                string MoveToPO = string.Empty;
                string Pdf = string.Empty;
                string View = string.Empty;
                string EditRow = string.Empty;
                string DeleteRow = string.Empty;

                MailRow = "<a href=SendMail?tag=Del&id=" + dtUsers.Rows[i]["PURQUOTBASICID"].ToString() + "><img src='../Images/mail_icon.png' alt='Send Email' /></a>";
                FollowUp = "<a href=Followup?id=" + dtUsers.Rows[i]["PURQUOTBASICID"].ToString() + "><img src='../Images/followup.png' /></a>";
                //if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "N")
                //{


                //}
                //else
                //{
                if (dtUsers.Rows[i]["STATUS"].ToString() == "Generated")
                {
                    MoveToPO = "<img src='../Images/tick.png' alt='View Details' width='20' />";
                    EditRow = "";
                }
                else
                {
                    MoveToPO = "<a href=ViewQuote?id=" + dtUsers.Rows[i]["PURQUOTBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/move_quote.png' alt='View Details' width='20' /></a>";
                    EditRow = "<a href=PurchaseQuotation?id=" + dtUsers.Rows[i]["PURQUOTBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";


                   
                }
                //}
                Pdf = "<a href=Print?id=" + dtUsers.Rows[i]["PURQUOTBASICID"].ToString() + "><img src='../Images/pdficon.png' width='20' alt='Deactivate' /></a>";

                //Pdf = "<a href=Print?id=" + dtUsers.Rows[i]["PURQUOTBASICID"].ToString() + "<img src='../Images/pdficon.png' width='30' /></a>";
                View = "<a href=ViewPurQuote?id=" + dtUsers.Rows[i]["PURQUOTBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View Details' width='20' /></a>";
                //EditRow = "<a href=PurchaseEnquiry?id=" + dtUsers.Rows[i]["PURENQBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                DeleteRow = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["PURQUOTBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";

                Reg.Add(new PurchaseQuoItems
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["PURQUOTBASICID"].ToString()),
                    branch = dtUsers.Rows[i]["BRANCHID"].ToString(),
                    enqno = dtUsers.Rows[i]["ENQNO"].ToString(),
                    docDate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    supplier = dtUsers.Rows[i]["PARTYNAME"].ToString(),
                    mailrow = MailRow,
                    follow = FollowUp,
                    move = MoveToPO,
                    pdf = Pdf,
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
        public IActionResult ListPurchaseQuo()
        {
            //IEnumerable<PurchaseQuo> cmp = PurquoService.GetAllPurQuotation(st, ed);
            return View();
        }
        public ActionResult DeleteItem(string tag, string id)
        {

            string flag = PurquoService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListPurchaseQuo");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListPurchaseQuo");
            }
        }

        public ActionResult SendMail(string id)
        {
            try
            {
                datatrans = new DataTransactions(_connectionString);
                MailRequest requestwer = new MailRequest();

                requestwer.ToEmail = "deepa@icand.in";
                requestwer.Subject = "Quotations";
                string Content = "";
                IEnumerable<QoItem> cmp = PurquoService.GetAllPurQuotationItem(id);
                Content = @"<html> 
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
</br>
<table style = 'border: 1px solid black;border-collapse: collapse;'> ";



                foreach (QoItem item in cmp)
                {
                    Content += " <tr><td>" + item.ItemId + "</td>";
                    Content += " <td>" + item.Quantity + "</td>";
                    Content += " <td>" + item.Unit + "</td></tr>";
                }
                Content += "</table>";

                Content += @" </br> 
<p style='padding-left:30px;font-style:italic;'>With Regards,
</br><img src='../assets/images/Arasan_Logo.png' alt='Arasan Logo'/>
</br>N Balaji Purchase Manager
</br>The Arasan Aluminium Industries (P) Ltd.
<br/102-A

</br>
</p> ";
                Content += @" </body> 
</html> ";

                requestwer.Body = Content;
                //request.Attachments = "Yes";
                datatrans.sendemail("Test mail", Content, "gokulakrishna76@gmail.com", "gokulakrishna76@gmail.com", "wxojuguvqfnjcejj", "587", "true", "smtp.gmail.com", "Arasan");
                return Ok();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public IActionResult PurchaseQuotationFollowup()
        {
            return View();
        }
        public IActionResult ViewQuote(string id)
        {
            PurchaseQuo ca = new PurchaseQuo();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();
            dt = PurquoService.GetPurQuotationByName(id);
            if(dt.Rows.Count > 0)
            {
                ca.Supplier = dt.Rows[0]["PARTYNAME"].ToString();
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.QuoId = dt.Rows[0]["DOCID"].ToString();
                ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                ca.EnqNo = dt.Rows[0]["ENQNO"].ToString();
                ca.EnqDate = dt.Rows[0]["ENQDATE"].ToString();
                ca.ID= id;
            }
            List<QoItem> Data = new List<QoItem>();
            QoItem tda = new QoItem();
            double tot = 0;
            dtt = PurquoService.GetPurQuoteItem(id);
            if(dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new QoItem();
                    tda.ItemId= dtt.Rows[i]["ITEMID"].ToString();
                    tda.Unit = dtt.Rows[i]["UNITID"].ToString();
                    tda.Quantity = Convert.ToDouble(dtt.Rows[i]["QTY"].ToString());
                    tda.rate = Convert.ToDouble(dtt.Rows[i]["RATE"].ToString() == "" ? "0" : dtt.Rows[i]["RATE"].ToString());
                    tda.TotalAmount = tda.Quantity * tda.rate;
                    tot += tda.TotalAmount;
                    Data.Add(tda);
                }
            }
            ca.Net=tot;
            ca.QoLst = Data;
            return View(ca);
        }

        public IActionResult ViewPurQuote(string id)
        {
            PurchaseQuo ca = new PurchaseQuo();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();
            dt = PurquoService.GetPurQuotationName(id);
            if (dt.Rows.Count > 0)
            {
                ca.Supplier = dt.Rows[0]["PARTYNAME"].ToString();
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.QuoId = dt.Rows[0]["DOCID"].ToString();
                ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                ca.EnqNo = dt.Rows[0]["ENQNO"].ToString();
                ca.EnqDate = dt.Rows[0]["ENQDATE"].ToString();
                ca.ID = id;
            }
            List<QoItem> Data = new List<QoItem>();
            QoItem tda = new QoItem();
            double tot = 0;
            dtt = PurquoService.GetPurQuoteDetails(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new QoItem();
                    tda.ItemId = dtt.Rows[i]["ITEMID"].ToString();
                    tda.Unit = dtt.Rows[i]["UNITID"].ToString();
                    tda.Quantity = Convert.ToDouble(dtt.Rows[i]["QTY"].ToString());
                    tda.rate = Convert.ToDouble(dtt.Rows[i]["RATE"].ToString() == "" ? "0" : dtt.Rows[i]["RATE"].ToString());
                    tda.TotalAmount = tda.Quantity * tda.rate;
                    tot += tda.TotalAmount;
                    Data.Add(tda);
                }
            }
            ca.Net = tot;
            ca.QoLst = Data;
            return View(ca);
        }

        [HttpPost]
        public ActionResult ViewQuote(PurchaseQuo Cy, string id)
        {
            try
            {
                Cy.ID = id;
                string Strout = PurquoService.QuotetoPO(Cy.ID);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "PO Generated Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "PO Generated Successfully...!";
                    }
                    return RedirectToAction("ListPurchaseQuo");
                }

                else
                {
                    ViewBag.PageTitle = "Edit PurchaseQuotation";
                    TempData["notice"] = Strout;
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("ListPurchaseQuo");
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
        public List<SelectListItem> BindEmployee()
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
        public List<SelectListItem> BindSupplier()
        {
            try
            {
                DataTable dtDesg = datatrans.GetSupplier();
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
                DataTable dtDesg = datatrans.GetCurency();
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
        public List<SelectListItem> BindItemGrplst()
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
        //public JsonResult GetItemJSON(string itemid)
        //{
        //    QoItem model = new QoItem();
        //    model.Ilst = BindItemlst(itemid);
        //    return Json(BindItemlst(itemid));

        //}
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
                dt = datatrans.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    Desc = dt.Rows[0]["ITEMDESC"].ToString();
                    unit = dt.Rows[0]["UNITID"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();
                    dt1 = PurquoService.GetItemCF(ItemId, dt.Rows[0]["UNITMASTID"].ToString());
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


        //public JsonResult GetItemGrpJSON()
        //{
        //    //EnqItem model = new EnqItem();
        //    //model.ItemGrouplst = BindItemGrplst(value);
        //    return Json(BindItemGrplst());
        //}
        public IActionResult Followup(string id)
        {
            QuoFollowup cmp = new QuoFollowup();
            cmp.EnqassignList = BindEmployee();
            List<QuotationFollowupDetails> TData = new List<QuotationFollowupDetails>();
            if (id == null)
            {

            }
            else
            {
                if (!string.IsNullOrEmpty(id))
                {
                    cmp.Quoteid = id;
                    DataTable dt = new DataTable();
                     dt = PurquoService.GetPurchaseQuoDetails(id);
                    if (dt.Rows.Count > 0)
                    {
                        cmp.QuoNo = dt.Rows[0]["DOCID"].ToString();
                        cmp.Supname = dt.Rows[0]["PARTYNAME"].ToString();
                    }
                    DataTable dtt = new DataTable();
                    string e = cmp.QuoNo;
                    dtt = PurquoService.GetFolowup(e);
                    QuotationFollowupDetails tda = new QuotationFollowupDetails();

                    if (dtt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtt.Rows.Count; i++)
                        {
                            tda = new QuotationFollowupDetails();
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
            cmp.qflst = TData;

            return View(cmp);

       
        }

      [HttpPost]
        public ActionResult Followup(QuoFollowup Pf, string id)
        {

            try
            {
                Pf.FolID = id;
                string Strout = PurquoService.PurchaseFollowupCRUD(Pf);
                //IEnumerable<QuoFollowup> cmp = PurquoService.GetAllPurchaseFollowup();
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Pf.FolID == null)
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
                return RedirectToAction("Followup", new { id = Pf.Quoteid });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Pf);
}
        //public IActionResult Followup()
        //{
        //    IEnumerable<QuoFollowup> cmp = PurquoService.GetAllPurchaseFollowup();
        //    return View(cmp);
        //}

        
        public async Task<IActionResult> Print(string id)
        {
            string mimtype = "";
            int extension = 1;
            DataSet ds = new DataSet();
            var path = $"{this._WebHostEnvironment.WebRootPath}\\Reports\\QuotationReport.rdlc";
            Dictionary<string, string> Parameters = new Dictionary<string, string>();
            //  Parameters.Add("rp1", " Hi Everyone");
            var PQuoitem = await PurquoService.GetPQuoItem(id);
            ////var po = await PoService.GetPO(id);
            //DataTable dt = new DataTable("POBASIC");
            //DataTable dt2 = new DataTable("PODETAIL");
            // dt= PoService.GetPO(id);
            // dt2= PoService.GetPOItem(id);
            //ds.Tables.Add(dt);
            //ds.Tables.Add(dt2);
            //ds.Tables.AddRange(new DataTable[] { dt, dt2 });
            //ReportDataSource rds = new AspNetCore.Reporting.ReportDataSource("DataSet_Reservaties", ds.Tables[0]);
            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("PurchaseQuotation", PQuoitem);
            //localReport.AddDataSource("DataSet1_DataTable1", po);
            var result = localReport.Execute(RenderType.Pdf, extension, Parameters, mimtype);
            return File(result.MainStream, "application/Pdf");
        }
    }
}
