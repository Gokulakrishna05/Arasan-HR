using System.Collections.Generic;
using Arasan.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Models;
using System.Xml.Linq;

namespace Arasan.Controllers
{
    public class PurchaseQuoController : Controller
    {
        IPurchaseQuo PurquoService;
        private string? _connectionString;
        IConfiguration? _configuratio;
        DataTransactions datatrans;
        public PurchaseQuoController(IPurchaseQuo _PurquoService, IConfiguration _configuratio)
        {
            PurquoService = _PurquoService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);

        }
        public IActionResult PurchaseQuotation(string id)
        {
            PurchaseQuo ca = new PurchaseQuo();
            ca.Brlst = BindBranch();
            ca.Suplst = BindSupplier();
            ca.Curlst = BindCurrency();
            ca.RecList = BindEmp();
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
        public IActionResult ListPurchaseQuo()
        {
            IEnumerable<PurchaseQuo> cmp = PurquoService.GetAllPurQuotation();
            return View(cmp);
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
                ca.Supplier = dt.Rows[0]["PARTY"].ToString();
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
           // ca.Net=tot;
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
        public List<SelectListItem> BindSupplier()
        {
            try
            {
                DataTable dtDesg = datatrans.GetSupplier();
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
            List<PurchaseFollowupDetails> TData = new List<PurchaseFollowupDetails>();
            if (id == null)
            {

            }
            else
            {
                if (!string.IsNullOrEmpty(id))
                {
                    DataTable dt = new DataTable();
                     dt = PurquoService.GetPurchaseQuoDetails(id);
                    if (dt.Rows.Count > 0)
                    {
                        cmp.QuoNo = dt.Rows[0]["DOCID"].ToString();
                        cmp.Supname = dt.Rows[0]["PARTY"].ToString();
                    }
                   
                   
                }
            }
            
            return View(cmp);
        }



        //IEnumerable<PurchaseFollowup> cmp = PurenqService.GetAllPurchaseFollowup();



        //[HttpPost]
        //public ActionResult Followup(QuoFollowup Pf, string id)
        //{

        //    try
        //    {
        //        Pf.ID = id;
        //        // string Strout = PurquoService.PurchaseFollowupCRUD(Pf);
        //        //if (string.IsNullOrEmpty(Strout))
        //        //{
        //        //    if (Pf.ID == null)
        //        //    {
        //        //        TempData["notice"] = "PurchaseQuotationFollowup Inserted Successfully...!";
        //        //    }
        //        //    else
        //        //    {
        //        //        TempData["notice"] = "PurchaseQuotationFollowup Updated Successfully...!";
        //        //    }
        //        //    return RedirectToAction("PurchaseQuotationFollowup");
        //        //}

        //        //else
        //        //{
        //        //    ViewBag.PageTitle = "Edit PurchaseQuotationFollowup";
        //        //    TempData["notice"] = Strout;
        //        //    //return View();
        //        //}

        //        // }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return View(Pf);
        //}
    }
}
