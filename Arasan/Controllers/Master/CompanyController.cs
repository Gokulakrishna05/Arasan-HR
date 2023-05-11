using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult ListCompany()
        {
            IEnumerable<Company> cmp = CompanyService.GetAllCompany();
            return View(cmp);
        }

    }
}



