using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Arasan.Controllers
{
    public class POController : Controller
    {
        IPO PoService;
        private string? _connectionString;
        IConfiguration? _configuratio;
        DataTransactions datatrans;
        public POController(IPO _PoService, IConfiguration _configuratio)
        {
            PoService = _PoService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        
        public IActionResult PurchaseOrder(string id)
        {
            PO po = new PO();
            po.Brlst = BindBranch();
            po.Suplst = BindSupplier();
            po.Curlst = BindCurrency();
            po.RecList = BindEmp();
            List<POItem> TData = new List<POItem>();
            POItem tda = new POItem();

            if (id == null)
            {

            }
            else
            {
                double total = 0;
                DataTable dt = new DataTable();
                dt = PoService.EditPObyID(id);
                if (dt.Rows.Count > 0)
                {
                    po.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    po.POdate = dt.Rows[0]["DOCDATE"].ToString();
                    po.Supplier = dt.Rows[0]["PARTYID"].ToString();
                    po.PONo = dt.Rows[0]["DOCID"].ToString();
                    po.ID = id;
                    po.QuoteNo = dt.Rows[0]["Quotno"].ToString();
                    po.Cur = dt.Rows[0]["MAINCURRENCY"].ToString();
                    po.ExRate = dt.Rows[0]["EXRATE"].ToString();
                    //po.RefNo = dt.Rows[0]["ENQREF"].ToString();
                }
                DataTable dt2 = new DataTable();
                dt2 = PoService.GetPOItemDetails(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new POItem();
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
                        tda.PURLst = BindPurType();
                        //tda.unitprim= dt2.Rows[i]["UNITID"].ToString();
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }
                po.PoItem = TData;

            }
            return View(po);
        }
        public IActionResult ListPO()
        {
            IEnumerable<PO> cmp = PoService.GetAllPO();
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
        public List<SelectListItem> BindPurType()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "CONSUMABLES PURCHASE", Value = "CONSUMABLES PURCHASE" });
                lstdesg.Add(new SelectListItem() { Text = "FIXED PURCHASE", Value = "FIXED PURCHASE" });
                lstdesg.Add(new SelectListItem() { Text = "MACHINERIES PURCHASE", Value = "MACHINERIES PURCHASE" });
                lstdesg.Add(new SelectListItem() { Text = "RAW MATERIAL", Value = "RAW MATERIAL" });
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
        public IActionResult PODetails(string id)
        {
            IEnumerable<POItem> cmp = PoService.GetAllPOItem(id);
            return View(cmp);
        }
        public IActionResult ViewPO(string id)
        {
            PO ca = new PO();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();
            dt = PoService.GetPObyID(id);
            if (dt.Rows.Count > 0)
            {
                ca.Supplier = dt.Rows[0]["PARTY"].ToString();
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.PONo = dt.Rows[0]["DOCID"].ToString();
                ca.POdate = dt.Rows[0]["DOCDATE"].ToString();
                ca.QuoteNo = dt.Rows[0]["Quotno"].ToString();
                ca.QuoteDate = dt.Rows[0]["Quotedate"].ToString();
                ca.ID = id;
            }
            List<POItemlst> Data = new List<POItemlst>();
            POItemlst tda = new POItemlst();
            double tot = 0;
            dtt = PoService.GetPOItembyID(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new POItemlst();
                    tda.ItemId = dtt.Rows[i]["ITEMID"].ToString();
                    tda.Unit = dtt.Rows[i]["UNITID"].ToString();
                    tda.Quantity = Convert.ToDouble(dtt.Rows[i]["QTY"].ToString());
                    tda.Rate = Convert.ToDouble(dtt.Rows[i]["RATE"].ToString());
                    tda.Amount = tda.Quantity * tda.Rate;
                    tot += tda.Amount;
                    Data.Add(tda);
                }
            }
            ca.Net = tot;
            ca.PoItemlst = Data;
            return View(ca);
        }
    }
}
