using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Arasan.Controllers
{
    public class CompanyController : Controller
       
    {
        ICompanyService CompanyService;
        public CompanyController (ICompanyService _CompanyService)
        {
            CompanyService = _CompanyService;
        }
        public IActionResult Company(string id)
        {
            Company ca = new Company();
            if (id == null) 
            {
                
            }
            else
            {
                ca = CompanyService.GetCompanyById(id);
                 
            }
            return View(ca);
        }
        [HttpPost]
        public ActionResult Company(Company Cy, string id)
        {
          try
            {
                Cy.ID = id;
                string Strout = CompanyService.CompanyCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Company Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Company Updated Successfully...!";
                    }
                    return RedirectToAction("ListCompany");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Company";
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
        public IActionResult ListCompany(string status)
        {
            //IEnumerable<Company> cmp = CompanyService.GetAllCompany(status);
            return View();
        }

        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = CompanyService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListCompany");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListCompany");
            }
        } public ActionResult Remove(string tag, int id)
        {

            string flag = CompanyService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListCompany");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListCompany");
            }
        }

        public ActionResult MyListCompanygrid(string strStatus)
        {
            List<CompanyList> Reg = new List<CompanyList>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "ACTIVE" : strStatus;
            dtUsers = CompanyService.GetAllCompanies(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                if (dtUsers.Rows[i]["STATUS"].ToString() == "ACTIVE")
                {

                    EditRow = "<a href=Company?id=" + dtUsers.Rows[i]["COMPANYMASTID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["COMPANYMASTID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["COMPANYMASTID"].ToString() + "><img src='../Images/close_icon.png' alt='Deactivate' /></a>";

                }

                Reg.Add(new CompanyList
                {
                    compname = dtUsers.Rows[i]["COMPANYID"].ToString(),
                    compdesc = dtUsers.Rows[i]["COMPANYDESC"].ToString(),
                    editrow = EditRow,
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }
    }
}



