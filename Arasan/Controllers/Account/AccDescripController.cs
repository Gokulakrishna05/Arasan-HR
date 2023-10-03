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
    public class AccDescripController : Controller
    {

        IAccDescrip AccDescripService;

        IConfiguration? _configuration;
        private string? _connectionString;

        public AccDescripController(IAccDescrip _AccDescripService, IConfiguration _configuration)
        {
             AccDescripService = _AccDescripService;
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
           
        }

        public IActionResult AccDescrip(string id)
        {
            AccDescrip AG = new AccDescrip();
            //AG.Brlst = BindBranch();
            AG.Branch = Request.Cookies["BRANCHID"];
            //AG.Createdby = Request.Cookies["UserId"];
            //AG.CreatedOn = Request.Cookies["UserId"];


            //for edit & delete
            if (id != null)
            {
                DataTable dt = new DataTable();
                double total = 0;
                dt = AccDescripService.GetAccDescrip(id);
                if (dt.Rows.Count > 0)
                {
                    
                    AG.TransactionName = dt.Rows[0]["ADTRANSDESC"].ToString();
                    AG.TransactionID = dt.Rows[0]["ADTRANSID"].ToString();
                    AG.SchemeName = dt.Rows[0]["ADSCHEME"].ToString();
                    AG.Description = dt.Rows[0]["ADSCHEMEDESC"].ToString();
                   
                    AG.ID = id;
                }
            }
            return View(AG);
        }

        [HttpPost]
        public ActionResult AccDescrip(AccDescrip Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = AccDescripService.AccDescripCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "AccDescrip Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "AccDescrip Updated Successfully...!";
                    }
                    return RedirectToAction("ListAccDescrip");
                }

                else
                {
                    ViewBag.PageTitle = "Edit AccDescrip";
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

        //public List<SelectListItem> BindBranch()
        //{
        //    try
        //    {
        //        DataTable dtDesg = AccDescripService.GetBranch();
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        for (int i = 0; i < dtDesg.Rows.Count; i++)
        //        {
        //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["BRANCHID"].ToString(), Value = dtDesg.Rows[i]["BRANCHMASTID"].ToString() });
        //        }
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = AccDescripService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListAccDescrip");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListAccDescrip");
            }
        }

        public ActionResult Remove(string tag, int id)
        {

            string flag = AccDescripService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListAccDescrip");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListAccDescrip");
            }
        }
        public IActionResult ListAccDescrip(string Active)
        {
            IEnumerable<AccDescrip> cmp = AccDescripService.GetAllAccDescrip(Active);
            return View(cmp);
        }
    }
}
