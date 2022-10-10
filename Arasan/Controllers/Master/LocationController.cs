using System.Collections.Generic;
using Arasan.Interface;

using Arasan.Models;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Controllers
{
    public class LocationController : Controller

    {
        ILocationService LocationService;
        public LocationController(ILocationService _locationService)
        {
            LocationService = _locationService;
        }
        public IActionResult Location(string id)
        {
            Location ca = new Location();
            if (id == null)
            {

            }
            else
            {
                ca = LocationService.GetLocationsById(id);

            }
            return View(ca);
        }
        [HttpPost]
        public ActionResult Location(Location Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = LocationService.LocationsCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Location Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Location Updated Successfully...!";
                    }
                    return RedirectToAction("ListLocation");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Location";
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
        public IActionResult ListLocation()
        {
            IEnumerable<Location> cmp = LocationService.GetAllLocations();
            return View(cmp);
        }

    }
}



