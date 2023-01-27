using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class AccGroupController : Controller
    {
        IAccGroup Accgroup;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public AccGroupController(IAccGroup _Accgroup, IConfiguration _configuratio)
        {
            Accgroup = _Accgroup;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult AccountGroup(string id)
        {
            AccGroup ca = new AccGroup();
            ca.Brlst = BindBranch();
            if (id == null)
            {


            }
            else
            {


                DataTable dt = new DataTable();

                dt = Accgroup.GetAccGroup(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    ca.CPMName = dt.Rows[0]["CPMNAME"].ToString();
                    ca.PmName = dt.Rows[0]["PMNAME"].ToString();
                    ca.Unique = dt.Rows[0]["UNIQUEID"].ToString();
                    ca.ID = id;

                }
               
            }
            return View(ca);
        }
        [HttpPost]
        public ActionResult AccountGroup(AccGroup Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = Accgroup.AccGroupCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "AccGroup Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "AccGroup Updated Successfully...!";
                    }
                    return RedirectToAction("ListAccountGroup");
                }

                else
                {
                    ViewBag.PageTitle = "Edit AccountGroup";
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

        public IActionResult ListAccountGroup()
        {
            IEnumerable<AccGroup> cmp = Accgroup.GetAllAccGroup();
            return View(cmp);
        }
    }
}
