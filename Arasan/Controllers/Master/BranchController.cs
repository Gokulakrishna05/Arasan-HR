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
        public BranchController(IBranchService _BranchService)
        {
            BranchService = _BranchService;
        }

        public IActionResult Branch(string id)
        {
            Branch br = new Branch();
            br.Compalst = BindCompany();
            br.cuntylst = BindCountry();
            br.statlst = BindState("");


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
        public List<SelectListItem> BindCountry()
        {
            try
            {
                DataTable dtDesg = BranchService.Getcountry();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["COUNTRYNAME"].ToString(), Value = dtDesg.Rows[i]["COUNTRYMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindState(string id)
        {
            try
            {
                DataTable dtDesg = BranchService.GetState(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["STATE"].ToString(), Value = dtDesg.Rows[i]["STATE"].ToString() });
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
        public JsonResult GetStateJSON(string supid)
        {
            Branch model = new Branch();
            model.statlst = BindState(supid);
            return Json(BindState(supid));

        }
        public IActionResult ListBranch()
        {
            IEnumerable<Branch> br = BranchService.GetAllBranch();
            return View(br);
        }

    }
}




