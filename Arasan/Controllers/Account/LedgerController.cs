using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Models;
using Oracle.ManagedDataAccess.Client;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

using Newtonsoft.Json.Linq;

namespace Arasan.Controllers
{
    public class LedgerController : Controller
    {
        ILedger ledger;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public LedgerController(ILedger _ledger, IConfiguration _configuratio)
        {
            ledger = _ledger;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult Ledger(string id)
        {
            Ledger ca = new Ledger();
            ca.Typelst = BindAccType();
            ca.AccGrouplst = BindAccGroup("");
            //ca.Date = DateTime.Now.ToString("dd-MMM-yyyy");
            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();

                dt = ledger.GetLedger(id);
                if (dt.Rows.Count > 0)
                {
                    ca.AType = dt.Rows[0]["ACCOUNTTYPE"].ToString();
                    ca.AccGroup = dt.Rows[0]["ACCGROUP"].ToString();
                    ca.LedName = dt.Rows[0]["LEDNAME"].ToString();
                    ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.OpStock = dt.Rows[0]["OPSTOCK"].ToString();
                    ca.ClStock = dt.Rows[0]["CLSTOCK"].ToString();
                    ca.DisplayName = dt.Rows[0]["DISPLAY_NAME"].ToString();
                    ca.Category = dt.Rows[0]["CATEGORY"].ToString();
                    ca.ID = id;

                }
            }
            return View(ca);
        }
        [HttpPost]
        public ActionResult Ledger(Ledger Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = ledger.LedgerCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Ledger Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Ledger Updated Successfully...!";
                    }
                    return RedirectToAction("ListLedger");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Ledger";
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
       
        public IActionResult ListLedger()
        {
            IEnumerable<Ledger> cmp = ledger.GetAllLedger();
            return View(cmp);
        }
        public JsonResult GetItemJSON(string itemid)
        {
            Ledger model = new Ledger();
            model.AccGrouplst = BindAccGroup(itemid);
            return Json(BindAccGroup(itemid));
        }


        public List<SelectListItem> BindAccGroup(string value)
        {
            try
            {
                DataTable dtDesg = ledger.GetAccGroup(value);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ACCOUNTGROUP"].ToString(), Value = dtDesg.Rows[i]["ACCGROUPID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = ledger.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListLedger");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListLedger");
            }
        }
       
        public List<SelectListItem> BindAccType()
        {
            try
            {
                DataTable dtDesg = ledger.GetAccType();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ACCOUNTTYPE"].ToString(), Value = dtDesg.Rows[i]["ACCOUNTTYPEID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

