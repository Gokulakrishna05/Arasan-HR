using Arasan.Interface;
using Arasan.Models;
using Arasan.Services.Master;
using Arasan.Services.Production;
using AspNetCore.Reporting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Utilities;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;
namespace Arasan.Controllers 
{
    public class IssueToProductionController : Controller
    {
        IIssueToProduction Proc;
        IConfiguration? _configuratio;
       
        private string? _connectionString;
        DataTransactions datatrans;
        public IssueToProductionController(IIssueToProduction _IIssueToProduction, IConfiguration _configuratio )
        {

            Proc = _IIssueToProduction;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
           
        }
        public IActionResult IssueToProduction()
        {
            IssueToProduction ip = new IssueToProduction();
            ip.Branch = Request.Cookies["BranchId"];
           
            ip.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            ip.Loc = BindLocation();
            ip.ToLoc = BindWork();
            ip.Itemlst = BindItemlst();
            List<IssueItem> TData = new List<IssueItem>();
            IssueItem tda = new IssueItem();
            return View(ip);
        }
        [HttpPost]
        public ActionResult IssueToProduction(IssueToProduction Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = Proc.IssueToProductionCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "IssueToProduction Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "IssueToProduction Updated Successfully...!";
                    }
                    return RedirectToAction("IssueToProduction");
                }

                else
                {
                    ViewBag.PageTitle = "Edit IssueToProduction";
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
        public List<SelectListItem> BindLocation()
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
        public List<SelectListItem> BindWork()
        {
            try
            {
                DataTable dtDesg = Proc.GetWorkCenter();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["WCID"].ToString(), Value = dtDesg.Rows[i]["WCBASICID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindItemlst( )
        {
            try
            {
                DataTable dtDesg = Proc.GetItem();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["ITEM_ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetStockDetail(string ItemId, string items)
        {
            try
            {
                DataTable dt = new DataTable();

               
                string stock = "";

                dt = Proc.GetStockDetails(ItemId, items);

                if (dt.Rows.Count > 0)
                {

                     
                    stock = dt.Rows[0]["SUM_QTY"].ToString();



                }

                var result = new {   stock = stock };
                return Json(result);
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
                 
                
                string unit = "";

                dt = datatrans.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                     
                    unit = dt.Rows[0]["UNITID"].ToString();



                }

                var result = new {  unit = unit };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetStockDetails(string id, string item)
        {
            IssueToProduction model = new IssueToProduction();
            DataTable dtt = new DataTable();
            List<IssueItem> Data = new List<IssueItem>();
            IssueItem tda = new IssueItem();
            dtt = Proc.GetStockDetail(id, item);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new IssueItem();

                    tda.item = dtt.Rows[i]["ITEMID"].ToString();
                    tda.itemid = dtt.Rows[i]["item"].ToString();
                    tda.lotno = dtt.Rows[i]["LOT_NO"].ToString();
                    tda.lotnoid = dtt.Rows[i]["lot"].ToString();
                    tda.totalqty = Convert.ToDouble(dtt.Rows[i]["BALANCE_QTY"].ToString() == "" ? "0" : dtt.Rows[i]["BALANCE_QTY"].ToString());
                    tda.qty = Convert.ToDouble(dtt.Rows[i]["BALANCE_QTY"].ToString() == "" ? "0" : dtt.Rows[i]["BALANCE_QTY"].ToString());



                    Data.Add(tda);
                }
            }
            model.Issuelst = Data;
            return Json(model.Issuelst);

        }
    }
}
