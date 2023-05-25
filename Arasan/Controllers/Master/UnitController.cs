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
    public class UnitController : Controller
    {
        IUnitService UnitService;
        IConfiguration? _configuration;
        private string? _connectionString;
        public UnitController(IUnitService _UnitService, IConfiguration _configuration)
        {
            UnitService = _UnitService;
          
        }
        public IActionResult Unit(string id)
        {
            Unit ca = new Unit();
            if (id != null)
            {
                DataTable dt = new DataTable();
                double total = 0;
                dt = UnitService.GetUnit(id);
                if (dt.Rows.Count > 0)
                {
                    ca.UnitName = dt.Rows[0]["UNITID"].ToString();
                   
                }
            }
            return View(ca);
        }
        [HttpPost]
        public ActionResult Unit(Unit Cy, string id)
        {
           try
            {
                Cy.ID = id;
                string Strout = UnitService.UnitCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Unit Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Unit Updated Successfully...!";
                    }
                    return RedirectToAction("ListUnit");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Unit";
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
        public IActionResult ListUnit()
        {
            IEnumerable<Unit> cmp = UnitService.GetAllUnit();
            return View(cmp);
        }
        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = UnitService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListUnit");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListUnit");
            }
        }
    }
}
