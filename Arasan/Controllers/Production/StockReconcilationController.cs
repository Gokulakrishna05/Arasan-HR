using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using Arasan.Interface;
using Arasan.Interface.Sales;
using Arasan.Models;
using Arasan.Services.Sales;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
namespace Arasan.Controllers 
{
    public class StockReconcilationController : Controller
    {

        IStockReconcilation stock;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;

        public StockReconcilationController(IStockReconcilation _stock, IConfiguration _configuratio)
        {
            stock = _stock;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult StockReconcilation(string id)
        {
            StockReconcilation ca = new StockReconcilation();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Enterd = Request.Cookies["UserName"];
            ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.Loclst = BindLoc();
            DataTable dtv = datatrans.GetSequence("DsAdd");
            if (dtv.Rows.Count > 0)
            {
                ca.Docid = dtv.Rows[0]["PREFIX"].ToString() + "" + dtv.Rows[0]["last"].ToString();
            }
            List<stockrecondetail> TData = new List<stockrecondetail>();
            stockrecondetail tda = new stockrecondetail();
            for (int i = 0; i < 1; i++)
            {
                tda = new stockrecondetail();
                tda.Itemlst = BindItem("");
                tda.drumlst = BindDrum("", "");
            
                tda.Isvalid = "Y";
                TData.Add(tda);
            }
            ca.stockLst = TData;
            return View(ca);
        }
        public List<SelectListItem> BindLoc()
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
        public List<SelectListItem> BindItem(string id)
        {
            try
            {
                DataTable dtDesg = stock.GetItem(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["item"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindDrum(string id, string loc)
        {
            try
            {
                DataTable dtDesg = stock.GetDrum(id, loc);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DRUMNO"].ToString(), Value = dtDesg.Rows[i]["DRUMNO"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetItemJSON(string itemid)
        {
            //BatchItem model = new BatchItem();
            //model.Processidlst = BindProcessid(itemid);
            return Json(BindItem(itemid));

        }
        public JsonResult GetDrumJSON(string itemid, string loc)
        {
            //BatchItem model = new BatchItem();
            //model.Processidlst = BindProcessid(itemid);
            return Json(BindDrum(itemid, loc));

        }
        public ActionResult Getitemdetails(string itemid)
        {
            try
            {
                string unit = "";
                DataTable d1 = datatrans.GetItemDetails(itemid);
                if(d1.Rows.Count > 0)
                {
                    unit = d1.Rows[0]["UNITID"].ToString();
                }
                var result = new { unit = unit };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetBatchJSON(string itemid, string item, string loc)
        {
            //BatchItem model = new BatchItem();
            //model.Processidlst = BindProcessid(itemid);
            // return Json(Bindbatch(itemid, item, loc));
            DataTable dt = new DataTable();
            string batch = "";
            string stock = "";
            //string tothrs = "";
            dt = datatrans.GetData("Select L.LOTNO,SUM(L.PLUSQTY-L.MINUSQTY) as qty from LSTOCKVALUE L ,LOTMAST LT WHERE LT.INSFLAG='1' AND L.LOTNO=LT.LOTNO AND L.ITEMID='" + item + "' AND L.LOCID='" + loc + "' AND L.DRUMNO='" + itemid + "' HAVING SUM(L.PLUSQTY-L.MINUSQTY) > 0  GROUP BY  L.LOTNO");
            if (dt.Rows.Count > 0)
            {

                batch = dt.Rows[0]["LOTNO"].ToString();
                stock = dt.Rows[0]["qty"].ToString();

            }

            var result = new { batch = batch, stock = stock };
            return Json(result);

        }
    }
}
