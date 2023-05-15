using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Services;

namespace Arasan.Controllers
{
    public class DrumCategoryController : Controller
    {

        IDrumCategory DrumCategoryService;
        public DrumCategoryController(IDrumCategory _DrumCategoryService)
        {
            DrumCategoryService = _DrumCategoryService;
        }

        public IActionResult NewCategory(string id)
        {
            DrumCategory DM = new DrumCategory();

            if (id == null)
            {

            }

            return View(DM);
        }


        [HttpPost]
        public ActionResult NewCategory(DrumCategory Dm, string id)
        {

            try
            {
                Dm.ID = id;
                string Strout = DrumCategoryService.DrumCategoryCRUD(Dm);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Dm.ID == null)
                    {
                        TempData["notice"] = "DrumCategory Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "DrumCategory Updated Successfully...!";
                    }
                    return RedirectToAction("ListNewCategory");
                }

                else
                {
                    ViewBag.PageTitle = "Edit NewCategory";
                    TempData["notice"] = Strout;
                    //return View();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Dm);
        }

        public ActionResult ListNewCategory()
        {
            IEnumerable<DrumCategory> cmp = DrumCategoryService.GetAllDrumCategory();
            return View(cmp);
        }

    }
}
