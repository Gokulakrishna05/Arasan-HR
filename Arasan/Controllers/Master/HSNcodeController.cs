using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers.Master
{
    public class HSNcodeController : Controller
    {
        IHSNcodeService HSNcodeService;
        public HSNcodeController(IHSNcodeService _HSNcodeService)
        {
            HSNcodeService = _HSNcodeService;
        }
        public IActionResult HSNcode(string id)
        {
            HSNcode st = new HSNcode();
           
            if (id == null)
            {

            }
            else
            {
                st = HSNcodeService.GetHSNcodeById(id);

            }
            return View(st);
        }
        [HttpPost]
        public ActionResult HSNcode(HSNcode ss, string id)
        {

            try
            {
                ss.ID = id;
                string Strout = HSNcodeService.HSNcodeCRUD(ss);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (ss.ID == null)
                    {
                        TempData["notice"] = " HSNcode Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " HSNcode Updated Successfully...!";
                    }
                    return RedirectToAction("ListHSNcode");
                }

                else
                {
                    ViewBag.PageTitle = "Edit HSNcode";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(ss);
        }
        public IActionResult ListHSNcode()
        {
            IEnumerable<HSNcode> sta = HSNcodeService.GetAllHSNcode();
            return View(sta);
        }

       

    }
}
