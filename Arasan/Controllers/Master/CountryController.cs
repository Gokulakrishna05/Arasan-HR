using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;

using Microsoft.AspNetCore.Mvc;

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

        public IActionResult ListCountry(string status)
        {
            IEnumerable<Country> ic = CountryService.GetAllCountry(status);
            return View(ic);
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
    }
}
