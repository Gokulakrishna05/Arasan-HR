﻿using System.Collections.Generic;
using System.Data;
using Arasan.Interface;

using Arasan.Models;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            ca.Brlst = BindBranch();
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
        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dtDesg = LocationService.GetBranch();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["BRANCHID"].ToString(), Value = dtDesg.Rows[i]["BRANCHMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}


