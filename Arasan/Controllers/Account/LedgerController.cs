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
            ca.Date = DateTime.Now.ToString("dd-MMM-yyyy");
            if (id == null)
            {


            }
            else
            {


                DataTable dt = new DataTable();

                dt = ledger.GetLedger(id);
                if (dt.Rows.Count > 0)
                {
                    ca.MName = dt.Rows[0]["MNAME"].ToString();
                    ca.Date = dt.Rows[0]["DOCDT"].ToString();
                    ca.DispName = dt.Rows[0]["DISPNAME"].ToString();
                    ca.Category = dt.Rows[0]["CATEGORY"].ToString();
                    ca.GrpAccount = dt.Rows[0]["GROUPORACCOUNT"].ToString();
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
    }
    }

