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
    public class CountryController : Controller
    {

        ICountryService CountryService;
        
        public CountryController(ICountryService _CountryService)
        {
            CountryService = _CountryService;
        }
        public IActionResult Country(string id)
        {
            Country ic = new Country();
            if (id == null)
            {

            }
            else  
            {
                //DataTable dt = new DataTable();
                ic = CountryService.GetCountryById(id);

            }
            return View(ic);
        }
        [HttpPost]
        public ActionResult Country(Country Ic, string id)
        {

            try
            {
                Ic.ID = id;
                string Strout = CountryService.CountryCRUD(Ic);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Ic.ID == null)
                    {
                        TempData["notice"] = "Country Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Country Updated Successfully...!";
                    }
                    return RedirectToAction("ListCountry");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Country";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Ic);
        }

        public IActionResult ListCountry()
        {
            return View();
        }

        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = CountryService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListCountry");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListCountry");
            }
        }
        public ActionResult Remove(string tag, int id)
        {

            string flag = CountryService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListCountry");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListCountry");
            }
        }
        public ActionResult MyListItemgrid()
        {
            List<Countrygrid> Reg = new List<Countrygrid>();
            DataTable dtUsers = new DataTable();

            dtUsers = CountryService.GetAllCountryGRID();
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                EditRow = "<a href=Country?id=" + dtUsers.Rows[i]["COUNTRYMASTID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["COUNTRYMASTID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";

                Reg.Add(new Countrygrid
                {
                    id = dtUsers.Rows[i]["COUNTRYMASTID"].ToString(),
                    coname = dtUsers.Rows[i]["COUNTRY"].ToString(),
                    concode = dtUsers.Rows[i]["COUNTRYCODE"].ToString(),
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
