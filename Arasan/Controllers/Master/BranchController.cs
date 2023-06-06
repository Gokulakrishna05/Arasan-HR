using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class BranchController : Controller
    {
        IBranchService BranchService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public BranchController(IBranchService _BranchService, IConfiguration _configuratio)
        {
            BranchService = _BranchService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public IActionResult Branch(string id)
        {
            Branch br = new Branch();
            br.Compalst = BindCompany();
            //br.cuntylst = BindCountry();
            br.statlst = BindState();
            br.Citylst = BindCity("");

            if (id == null)
            {
                

            }
            else
            {

                DataTable dt = new DataTable();

                dt = BranchService.GetEditBranch(id);
                if (dt.Rows.Count > 0)
                {
                    br.CompanyName = dt.Rows[0]["COMPANYID"].ToString();
                    br.BranchName = dt.Rows[0]["BRANCHID"].ToString();
                    br.Address = dt.Rows[0]["ADDRESS1"].ToString();
                    br.StateName = dt.Rows[0]["STATE"].ToString();
                    br.Citylst = BindCity(br.StateName);
                    br.City = dt.Rows[0]["CITY"].ToString();
                    br.PinCode = dt.Rows[0]["PINCODE"].ToString();
                    br.GSTNo = dt.Rows[0]["CSTNO"].ToString();
                    br.GSTDate = dt.Rows[0]["CSTDATE"].ToString();
                    br.ID = id;


                }
            }

            return View(br);
        }

        public List<SelectListItem> BindCompany()
        {
            try
            {
                DataTable dtDesg = BranchService.GetCompany();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["COMPANYID"].ToString(), Value = dtDesg.Rows[i]["COMPANYMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public List<SelectListItem> BindCountry()
        //{
        //    try
        //    {
        //        DataTable dtDesg = BranchService.Getcountry();
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        for (int i = 0; i < dtDesg.Rows.Count; i++)
        //        {
        //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["COUNTRYNAME"].ToString(), Value = dtDesg.Rows[i]["COUNTRYMASTID"].ToString() });
        //        }
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public List<SelectListItem> BindState()
        {
            try
            {
                DataTable dtDesg = BranchService.GetState();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["STATE"].ToString(), Value = dtDesg.Rows[i]["STATEMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindCity(String id)
        {
            try
            {
                DataTable dtDesg = BranchService.GetCity(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["CITYNAME"].ToString(), Value = dtDesg.Rows[i]["CITYNAME"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Branch(Branch Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = BranchService.BranchCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Branch Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Branch Updated Successfully...!";
                    }
                    return RedirectToAction("ListBranch");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Branch";
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
        public JsonResult GetCityJSON(string supid)
        {
            //string CityID = datatrans.GetDataString("Select STATEMASTID from STATEMAST where STATE='" + supid + "' ");
            Branch model = new Branch();
            model.Citylst = BindCity(supid);
            return Json(BindCity(supid));

        }
        public IActionResult ListBranch()
        {
            IEnumerable<Branch> br = BranchService.GetAllBranch();
            return View(br);
        }

        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = BranchService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListBranch");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListBranch");
            }
        }

    }
}




