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
                int legcode = Convert.ToInt32(Cy.Ledgercode);
                string code = GetNumberwithPrefix(legcode, 6);
                Cy.LegCode = code;

                //int grocode = Convert.ToInt32(Cy.Groupcode);
                //string code1 = GetNumberwithPrefix1(grocode, 6);
                //Cy.Grocode = code1;

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
        public static string GetNumberwithPrefix(int Ledgercode, int totalchar)
        {
            string tempnumber = Ledgercode.ToString();
            while (tempnumber.Length < 6)
            tempnumber = "0" + tempnumber;
            return tempnumber;
        }
        //public static string GetNumberwithPrefix1(int Groupcode, int totalchar)
        //{
        //    string tempnumber = Groupcode.ToString();
        //    while (tempnumber.Length < 6)
        //        tempnumber = "0" + tempnumber;
        //    return tempnumber;
        //}
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
        public ActionResult GetGroupCodeDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
               
                string code = "";

                dt = ledger.GetGroupCodeDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    code = dt.Rows[0]["GROUPCODE"].ToString();
                   
                }

                var result = new { code = code };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
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

