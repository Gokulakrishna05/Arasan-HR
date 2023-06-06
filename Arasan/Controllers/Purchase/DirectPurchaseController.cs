using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Arasan.Interface;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Models;


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
            ca.Suplst = BindSupplier();
            ca.Curlst = BindCurrency();
            ca.Loclst = GetLoc();
            ca.DocDate = DateTime.Now.ToString("dd-MMM-yyyy");
            List<DirItem> TData = new List<DirItem>();
            DirItem tda = new DirItem();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new DirItem();
                    tda.ItemGrouplst = BindItemGrplst();
                    tda.Itemlst = BindItemlst("");
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
                        tda.CGSTP = Convert.ToDouble(dt2.Rows[i]["CGSTPER"].ToString() == "" ? "0" : dt2.Rows[i]["CGSTPER"].ToString());
                        tda.SGSTP = Convert.ToDouble(dt2.Rows[i]["SGSTPER"].ToString() == "" ? "0" : dt2.Rows[i]["SGSTPER"].ToString());

                        tda.IGSTP = Convert.ToDouble(dt2.Rows[i]["IGSTPER"].ToString() == "" ? "0" : dt2.Rows[i]["IGSTPER"].ToString());
                        tda.CGST = Convert.ToDouble(dt2.Rows[i]["CGSTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["CGSTAMT"].ToString());
                        tda.SGST = Convert.ToDouble(dt2.Rows[i]["SGSTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["SGSTAMT"].ToString());
                        tda.IGST = Convert.ToDouble(dt2.Rows[i]["IGSTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["IGSTAMT"].ToString());
                        // tda.PurType = dt2.Rows[i]["PURTYPE"].ToString();
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }
                //ca.net = Math.Round(total, 2);
               
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
        public IActionResult ListDirectPurchase()
        {
            IEnumerable<DirectPurchase> cmp = directPurchase.GetAllDirectPur();
            return View(cmp);
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

        public ActionResult DeleteMR(string tag, int id)
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
    }
}