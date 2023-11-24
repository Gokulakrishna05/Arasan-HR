using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Arasan.Interface;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Models;
using Arasan.Services;
using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Office2010.Excel;


namespace Arasan.Controllers
{
    public class DirectPurchaseController : Controller
    {
        IDirectPurchase directPurchase;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public DirectPurchaseController(IDirectPurchase _directPurchase ,IConfiguration _configuratio)
        {
            directPurchase = _directPurchase;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult DirectPurchase(string id)
        {
            DirectPurchase ca = new DirectPurchase();
            ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Currency = "1";
            ca.Suplst = BindSupplier();
            ca.RefDate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.Curlst = BindCurrency();
            ca.Loclst = GetLoc();
            string loc = ca.Location;
            //ViewBag.locdisp = ca.Location;
            ca.Vocherlst = BindVocher();
            ca.DocDate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence1("dp", loc);
            if (dtv.Rows.Count > 0)
            {
                ca.DocNo = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["LASTNO"].ToString();
            }
            List<DirItem> TData = new List<DirItem>();
            DirItem tda = new DirItem();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new DirItem();
                    tda.ItemGrouplst = BindItemGrplst();
                    tda.Itemlst = BindItemlst("");
                    tda.gstlst = Bindgstlst("");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {

                // ca = directPurchase.GetDirectPurById(id);


                DataTable dt = new DataTable();
                double total = 0;
                dt = directPurchase.GetDirectPurchase(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.Supplier = dt.Rows[0]["PARTYID"].ToString();
                    ca.DocNo = dt.Rows[0]["DOCID"].ToString();
                    ca.ID = id;
                    ca.Currency = dt.Rows[0]["MAINCURRENCY"].ToString();
                    ca.RefDate = dt.Rows[0]["REFDT"].ToString();
                    ca.Voucher = dt.Rows[0]["VOUCHER"].ToString();
                    ca.Location = dt.Rows[0]["LOCID"].ToString();
                    ca.Narration = dt.Rows[0]["NARR"].ToString();
                    ca.LRCha = Convert.ToDouble(dt.Rows[0]["LRCH"].ToString() == "" ? "0" : dt.Rows[0]["LRCH"].ToString());
                    ca.DelCh = Convert.ToDouble(dt.Rows[0]["DELCH"].ToString() == "" ? "0" : dt.Rows[0]["DELCH"].ToString());
                    ca.Other = Convert.ToDouble(dt.Rows[0]["OTHERCH"].ToString() == "" ? "0" : dt.Rows[0]["OTHERCH"].ToString());
                    ca.Frig = Convert.ToDouble(dt.Rows[0]["FREIGHT"].ToString() == "" ? "0" : dt.Rows[0]["FREIGHT"].ToString());
                    ca.SpDisc = Convert.ToDouble(dt.Rows[0]["OTHERDISC"].ToString() == "" ? "0" : dt.Rows[0]["OTHERDISC"].ToString());
                    ca.Round = Convert.ToDouble(dt.Rows[0]["ROUNDM"].ToString() == "" ? "0" : dt.Rows[0]["ROUNDM"].ToString());

                    ca.Gross = Convert.ToDouble(dt.Rows[0]["GROSS"].ToString() == "" ? "0" : dt.Rows[0]["GROSS"].ToString());
                    ca.net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());

                }
                DataTable dt2 = new DataTable();
                dt2 = directPurchase.GetDirectPurchaseItemDetails(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new DirItem();
                        double toaamt = 0;
                        tda.ItemGrouplst = BindItemGrplst();
                        DataTable dt3 = new DataTable();
                        dt3 = datatrans.GetItemSubGroup(dt2.Rows[i]["ITEMID"].ToString());
                        if (dt3.Rows.Count > 0)
                        {
                            tda.ItemGroupId = dt3.Rows[0]["SUBGROUPCODE"].ToString();
                        }
                        tda.Itemlst = BindItemlst(tda.ItemGroupId);
                        tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();

                        DataTable dt4 = new DataTable();
                        dt4 = datatrans.GetItemDetails(tda.ItemId);
                        if (dt4.Rows.Count > 0)
                        {

                            tda.ConFac = dt4.Rows[0]["CF"].ToString();
                            tda.rate = Convert.ToDouble(dt4.Rows[0]["LATPURPRICE"].ToString());
                        }
                        tda.Quantity = Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                        toaamt = tda.rate * tda.Quantity;
                        total += toaamt;
                        //tda.QtyPrim= Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                        tda.Amount = toaamt;
                        tda.Unit = dt2.Rows[i]["UNITID"].ToString();
                        //tda.PURLst = BindPurType();
                        //tda.unitprim= dt2.Rows[i]["UNITID"].ToString();

                        tda.Disc = Convert.ToDouble(dt2.Rows[i]["DISC"].ToString() == "" ? "0" : dt2.Rows[i]["DISC"].ToString());
                        tda.DiscAmount = Convert.ToDouble(dt2.Rows[i]["DISCAMOUNT"].ToString() == "" ? "0" : dt2.Rows[i]["DISCAMOUNT"].ToString());

                        tda.FrigCharge = Convert.ToDouble(dt2.Rows[i]["IFREIGHTCH"].ToString() == "" ? "0" : dt2.Rows[i]["IFREIGHTCH"].ToString());
                        tda.TotalAmount = Convert.ToDouble(dt2.Rows[i]["TOTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["TOTAMT"].ToString());
                        tda.CGSTP = Convert.ToDouble(dt2.Rows[i]["CGSTP"].ToString() == "" ? "0" : dt2.Rows[i]["CGSTP"].ToString());
                        tda.SGSTP = Convert.ToDouble(dt2.Rows[i]["SGSTP"].ToString() == "" ? "0" : dt2.Rows[i]["SGSTP"].ToString());
                        tda.IGSTP = Convert.ToDouble(dt2.Rows[i]["IGSTP"].ToString() == "" ? "0" : dt2.Rows[i]["IGSTP"].ToString());
                        tda.CGST = Convert.ToDouble(dt2.Rows[i]["CGST"].ToString() == "" ? "0" : dt2.Rows[i]["CGST"].ToString());
                        tda.SGST = Convert.ToDouble(dt2.Rows[i]["SGST"].ToString() == "" ? "0" : dt2.Rows[i]["SGST"].ToString());
                        tda.IGST = Convert.ToDouble(dt2.Rows[i]["IGST"].ToString() == "" ? "0" : dt2.Rows[i]["IGST"].ToString());
                        // tda.PurType = dt2.Rows[i]["PURTYPE"].ToString();
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }
                ca.net = Math.Round(total, 2);

            }
            ca.DirLst = TData;
            return View(ca);

        }
        public IActionResult DirectPurchaseDetails(string id)
        {
            IEnumerable<DirItem> cmp = directPurchase.GetAllDirectPurItem(id);
            return View(cmp);
        }
        [HttpPost]
        public ActionResult DirectPurchase(DirectPurchase Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = directPurchase.DirectPurCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "DirectPurchase Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "DirectPurchase Updated Successfully...!";
                    }
                    return RedirectToAction("ListDirectPurchase");
                }

                else
                {
                    ViewBag.PageTitle = "Edit DirectPurchase";
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
        public ActionResult MyListDirectPurchaseGrid(string strStatus)
        {
            List<DirectPurchaseItems> Reg = new List<DirectPurchaseItems>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)directPurchase.GetAllDirectPurchases(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string MailRow = string.Empty;
                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string MoveToGRN = string.Empty;
               
                MailRow = "<a href=DirectPurchase?tag=Del&id=" + dtUsers.Rows[i]["DPBASICID"].ToString() + "><img src='../Images/mail_icon.png' alt='Send Email' /></a>";
                EditRow = "<a href=DirectPurchase?id=" + dtUsers.Rows[i]["DPBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                DeleteRow = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["DPBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                if(dtUsers.Rows[i]["STATUS"].ToString()== "GRN Generated")
                {
                    MoveToGRN = "<img src='../Images/tick.png' alt='View Details' width='20' />";
                }
                else
                {
                    MoveToGRN = "<a href=MoveToGRN?id=" + dtUsers.Rows[i]["DPBASICID"].ToString() + "><img src='../Images/move_quote.png' alt='View Details' width='20' /></a>";
                }
                //if (Reg.Status == "GRN Generated")
                //{
                //    @Html.DisplayFor(Reg => Reg.Status);
                //}
                //else
                //{
                    //MoveToGRN = "<a href=MoveToGRN?id=" + dtUsers.Rows[i]["DPBASICID"].ToString() + "><img src='../Images/move_quote.png' alt='View Details' width='20' /></a>";

                //}
                Reg.Add(new DirectPurchaseItems
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["DPBASICID"].ToString()),
                    branch = dtUsers.Rows[i]["BRANCHID"].ToString(),
                    supplier = dtUsers.Rows[i]["PARTYNAME"].ToString(),
                    docNo = dtUsers.Rows[i]["DOCID"].ToString(),
                    docDate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    mailrow = MailRow,
                    editrow = EditRow,
                    delrow = DeleteRow,
                    move = MoveToGRN,
                    

                });
            }

            return Json(new
            {
                Reg
            });

        }
        public IActionResult MoveToGRN(string id)
        {
            Models.DirectPurchase ca = new Models.DirectPurchase();
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            dt = directPurchase.GetDirectPurchaseGrn(id);
            if (dt.Rows.Count > 0)
            {
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                ca.Supplier = dt.Rows[0]["PARTYNAME"].ToString();
                ca.DocNo = dt.Rows[0]["DOCID"].ToString();
                ca.ID = id;
                ca.Currency = dt.Rows[0]["MAINCURR"].ToString();
                ca.RefDate = dt.Rows[0]["REFDT"].ToString();
                ca.Voucher = dt.Rows[0]["VOUCHER"].ToString();
                ca.Location = dt.Rows[0]["LOCID"].ToString();
                ca.Narration = dt.Rows[0]["NARR"].ToString();
                ca.LRCha = Convert.ToDouble(dt.Rows[0]["LRCH"].ToString() == "" ? "0" : dt.Rows[0]["LRCH"].ToString());
                ca.DelCh = Convert.ToDouble(dt.Rows[0]["DELCH"].ToString() == "" ? "0" : dt.Rows[0]["DELCH"].ToString());
                ca.Other = Convert.ToDouble(dt.Rows[0]["OTHERCH"].ToString() == "" ? "0" : dt.Rows[0]["OTHERCH"].ToString());
                ca.Frig = Convert.ToDouble(dt.Rows[0]["FREIGHT"].ToString() == "" ? "0" : dt.Rows[0]["FREIGHT"].ToString());
                ca.SpDisc = Convert.ToDouble(dt.Rows[0]["OTHERDISC"].ToString() == "" ? "0" : dt.Rows[0]["OTHERDISC"].ToString());
                ca.Round = Convert.ToDouble(dt.Rows[0]["ROUNDM"].ToString() == "" ? "0" : dt.Rows[0]["ROUNDM"].ToString());
                ca.Gross = Convert.ToDouble(dt.Rows[0]["GROSS"].ToString() == "" ? "0" : dt.Rows[0]["GROSS"].ToString());
                ca.net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());

            }
            List<DirItem> TData = new List<DirItem>();
            DirItem tda = new DirItem();
            double total = 0;
            dt2 = directPurchase.GetDirectPurchaseItemGRN(id);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new DirItem();
                    tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                    tda.Quantity = Convert.ToDouble(dt2.Rows[i]["QTY"].ToString() == "" ? "0" : dt2.Rows[i]["QTY"].ToString());
                    tda.rate = Convert.ToDouble(dt2.Rows[i]["RATE"].ToString() == "" ? "0" : dt2.Rows[i]["RATE"].ToString());
                    tda.ConFac = dt2.Rows[0]["CF"].ToString();
                    tda.Amount = tda.Quantity * tda.rate;
                    total += tda.Amount;

                    //toaamt = tda.rate * tda.Quantity;
                    //total += toaamt;
                   
                    //tda.Amount = toaamt;
                    tda.Unit = dt2.Rows[i]["UNITID"].ToString();
                   
                    tda.Disc = Convert.ToDouble(dt2.Rows[i]["DISC"].ToString() == "" ? "0" : dt2.Rows[i]["DISC"].ToString());
                    tda.DiscAmount = Convert.ToDouble(dt2.Rows[i]["DISCAMOUNT"].ToString() == "" ? "0" : dt2.Rows[i]["DISCAMOUNT"].ToString());

                    tda.FrigCharge = Convert.ToDouble(dt2.Rows[i]["IFREIGHTCH"].ToString() == "" ? "0" : dt2.Rows[i]["IFREIGHTCH"].ToString());
                    tda.TotalAmount = Convert.ToDouble(dt2.Rows[i]["TOTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["TOTAMT"].ToString());
                    tda.CGSTP = Convert.ToDouble(dt2.Rows[i]["CGSTP"].ToString() == "" ? "0" : dt2.Rows[i]["CGSTP"].ToString());
                    tda.SGSTP = Convert.ToDouble(dt2.Rows[i]["SGSTP"].ToString() == "" ? "0" : dt2.Rows[i]["SGSTP"].ToString());
                    tda.IGSTP = Convert.ToDouble(dt2.Rows[i]["IGSTP"].ToString() == "" ? "0" : dt2.Rows[i]["IGSTP"].ToString());
                    tda.CGST = Convert.ToDouble(dt2.Rows[i]["CGST"].ToString() == "" ? "0" : dt2.Rows[i]["CGST"].ToString());
                    tda.SGST = Convert.ToDouble(dt2.Rows[i]["SGST"].ToString() == "" ? "0" : dt2.Rows[i]["SGST"].ToString());
                    tda.IGST = Convert.ToDouble(dt2.Rows[i]["IGST"].ToString() == "" ? "0" : dt2.Rows[i]["IGST"].ToString());
                    // tda.PurType = dt2.Rows[i]["PURTYPE"].ToString();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
                ca.net = Math.Round(total, 2);
            }
            //ca.Net = tot;
            ca.DirLst = TData;
            return View(ca);
        }
        [HttpPost]
        public ActionResult MoveToGRN(Models.DirectPurchase Cy, string id)
        {
            try
            {
                Cy.ID = id;
                string Strout = directPurchase.DirectPurchasetoGRN(Cy.ID);
                return RedirectToAction("ListDirectPurchase");
              

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("ListDirectPurchase");
        }
        public ActionResult DeleteItem(string tag, string id)
        {

            string flag = directPurchase.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListDirectPurchase");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListDirectPurchase");
            }
        }
        //public ActionResult DeleteItem(string tag, string id)
        //{
        //    //EnquiryList eqgl = new EnquiryList();
        //    //eqgl.StatusChange(tag, id);
        //    return RedirectToAction("ListDirectPurchase");

        //}
        public ActionResult GetDocidDetail(string locid)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string doc = "";

                dt = datatrans.GetSequences("dp", locid);
                if (dt.Rows.Count > 0)
                {
                    doc = dt.Rows[0]["doc"].ToString();
                }


                var result = new { doc = doc };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult ListDirectPurchase()
        {
            //IEnumerable<DirectPurchase> cmp = directPurchase.GetAllDirectPur(status);
            return View();
        }
        public IActionResult ListDirectPurchaseGRN()
        {
            //IEnumerable<DirectPurchase> cmp = directPurchase.GetAllDirectPur(status);
            return View();
        }
        public ActionResult MyListDirectPurchaseGRN(string strStatus)
        {
            List<DirectPurchaseItems> Reg = new List<DirectPurchaseItems>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)directPurchase.GetAllDirectPurchases(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string MailRow = string.Empty;
                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string Account = string.Empty;
                //string Status = string.Empty;

                MailRow = "<a href=DirectPurchase?tag=Del&id=" + dtUsers.Rows[i]["DPBASICID"].ToString() + "><img src='../Images/mail_icon.png' alt='Send Email' /></a>";
                EditRow = "<a href=DirectPurchase?id=" + dtUsers.Rows[i]["DPBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                DeleteRow = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["DPBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                Account = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["DPBASICID"].ToString() + "><img src='../Images/checklist.png' alt='Edit' /></a>";
                Reg.Add(new DirectPurchaseItems
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["DPBASICID"].ToString()),
                    branch = dtUsers.Rows[i]["BRANCHID"].ToString(),
                    supplier = dtUsers.Rows[i]["PARTYNAME"].ToString(),
                    docNo = dtUsers.Rows[i]["DOCID"].ToString(),
                    docDate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    mailrow = MailRow,
                    editrow = EditRow,
                    delrow = DeleteRow,
                    Accrow = Account,
                    //Status = Status,


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
        public List<SelectListItem> BindVocher()
        {
            try
            {
                DataTable dtDesg = directPurchase.GetVocher();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DESCRIPTION"].ToString(), Value = dtDesg.Rows[i]["DESCRIPTION"].ToString() });
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
        public List<SelectListItem> GetLoc()
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
        public List<SelectListItem> Bindgstlst(string id)
        {
            try
            {

                DataTable dtDesg = directPurchase.GetTariff(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["TARIFFID"].ToString(), Value = dtDesg.Rows[i]["tariff"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public List<SelectListItem> BindPurType()
        //{
        //    try
        //    {

        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        lstdesg.Add(new SelectListItem() { Text = "CONSUMABLES PURCHASE", Value = "CONSUMABLES PURCHASE" });
        //        lstdesg.Add(new SelectListItem() { Text = "FIXED PURCHASE", Value = "FIXED PURCHASE" });
        //        lstdesg.Add(new SelectListItem() { Text = "MACHINERIES PURCHASE", Value = "MACHINERIES PURCHASE" });
        //        lstdesg.Add(new SelectListItem() { Text = "RAW MATERIAL", Value = "RAW MATERIAL" });
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

        public ActionResult GetItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                
                string unit = "";
                string CF = "";
                string price = "";
                dt = datatrans.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                 
                    unit = dt.Rows[0]["UNITID"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();
                    dt1 = directPurchase.GetItemCF(ItemId, dt.Rows[0]["UNITMASTID"].ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        CF = dt1.Rows[0]["CF"].ToString();
                    }
                }

                var result = new { unit = unit, CF = CF, price = price };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetItemJSON(string itemid)
        {
            DirItem model = new DirItem();
            model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst(itemid));

        }
        public JsonResult GetItemGrpJSON()
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItemGrplst());
        }

        public IActionResult Index()
        {
            return View();
        }
        public ActionResult GetGSTDetail(string ItemId, string custid)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string per = "0";

                string hsn = "";
                string hsnid = "";
                string gst = "";
                
               
                //if (ItemId == "1")
                //{
                //    hsn = "996519";
                //}
                //else
                //{
                    dt = directPurchase.GetHsn(ItemId);
                    if (dt.Rows.Count > 0)
                    {
                        hsn = dt.Rows[0]["HSN"].ToString();
                    }
                dt1 = directPurchase.GethsnDetails(hsn);
                if (dt1.Rows.Count > 0)
                {

                    hsnid = dt1.Rows[0]["HSNCODEID"].ToString();


                }
                //}
                //if (ItemId == "1")
                //{
                //    sgst = "18";
                //}
                //else
                //{
                DataTable trff = new DataTable();
                trff = directPurchase.GetgstDetails(hsnid);
               
                if (trff.Rows.Count ==1)
                {
                    
                        gst = trff.Rows[0]["TARIFFID"].ToString();

                        DataTable percen = datatrans.GetData("Select PERCENTAGE from TARIFFMASTER where TARIFFMASTERID='" + gst + "'  ");
                        per = percen.Rows[0]["PERCENTAGE"].ToString();
                    

                }
                //}

                string cmpstate = datatrans.GetDataString("select STATE from CONTROL");

                string type = "";

                string partystate = datatrans.GetDataString("select STATE from PARTYMAST where PARTYMASTID='" + custid + "'");
                if (partystate == cmpstate)
                {
                    type = "GST";
                }
                else
                {
                    type = "IGST";
                }

                var result = new { per = per, type = type};
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetGSTItemJSON(string itemid)
        {
            DirItem model = new DirItem();
            string hsn = datatrans.GetDataString("select HSN,ITEMMASTERID from ITEMMASTER WHERE ITEMMASTERID='" + itemid + "'");
            string hsnid = datatrans.GetDataString("select HSNCODEID from HSNCODE WHERE HSNCODE='" + hsn + "'");
            
            model.gstlst = Bindgstlst(hsnid);
            return Json(Bindgstlst(hsnid));

        }
        public ActionResult GetGSTPerDetail(string ItemId, string custid)
        {
            try
            {
                DataTable hs = new DataTable();
                DataTable dt1 = new DataTable();
                string per = "";


                DataTable percentage = datatrans.GetData("Select PERCENTAGE from TARIFFMASTER where TARIFFMASTERID='" + ItemId + "'  ");
                if (percentage.Rows.Count > 0)
                {
                    per = percentage.Rows[0]["PERCENTAGE"].ToString();


                }
                //}

                string cmpstate = datatrans.GetDataString("select STATE from CONTROL");

                string type = "";

                string partystate = datatrans.GetDataString("select STATE from PARTYMAST where PARTYMASTID='" + custid + "'");

                if (partystate == cmpstate)
                {
                    type = "GST";
                }
                else
                {
                    type = "IGST";
                }



                var result = new { per = per, type = type };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetNarrDetail(string party)
        {
            try
            {
                string type = "";

                string partystate = datatrans.GetDataString("select PARTYNAME from PARTYMAST where PARTYMASTID='" + party + "'");

                
                    type = "Purchased From " +partystate;
                



                var result = new { type = type };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}