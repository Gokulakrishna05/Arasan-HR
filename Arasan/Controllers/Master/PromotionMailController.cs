using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class PromotionMailController : Controller
    {
        IPromotionMailService PromotionMailService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public PromotionMailController(IPromotionMailService _PromotionMailService, IConfiguration _configuratio)
        {
            PromotionMailService = _PromotionMailService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult PromotionMail(string id)
        {
            PromotionMail br = new PromotionMail();
           


            //for edit & delete
            //if (id != null)
            //{
            //    DataTable dt = new DataTable();
            //    //double total = 0;
            //    dt = BranchService.GetEditBranch(id);
            //    if (dt.Rows.Count > 0)
            //    {
            //        br.CompanyName = dt.Rows[0]["COMPANYID"].ToString();
            //        br.BranchName = dt.Rows[0]["BRANCHID"].ToString();
            //        br.Address = dt.Rows[0]["ADDRESS1"].ToString();
            //        br.StateName = dt.Rows[0]["STATE"].ToString();
            //        br.Citylst = BindCity(br.StateName);
            //        br.City = dt.Rows[0]["CITY"].ToString();
            //        br.PinCode = dt.Rows[0]["PINCODE"].ToString();
            //        br.GSTNo = dt.Rows[0]["CSTNO"].ToString();
            //        br.GSTDate = dt.Rows[0]["CSTDATE"].ToString();
            //        br.ID = id;

            //    }
            //}
            return View(br);
        }
        [HttpPost]
        public ActionResult PromotionMail(PromotionMail Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = PromotionMailService.PromotionMailCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "PromotionMail Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "PromotionMail Updated Successfully...!";
                    }
                    return RedirectToAction("PromotionMail");
                }

                else
                {
                    ViewBag.PageTitle = "Edit PromotionMail";
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
       
    }
}
