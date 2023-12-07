using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Services;
//using DocumentFormat.OpenXml.Bibliography;

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
            else
            {
                DataTable dt = new DataTable();
                //double total = 0;
                dt = DrumCategoryService.GetDrumCategory(id);
                if (dt.Rows.Count > 0)
                {
                    DM.CateType = dt.Rows[0]["CATEGORYTYPE"].ToString();
                    DM.Description = dt.Rows[0]["DESCRIPTION"].ToString();
                    
                    DM.ID = id;
                }
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
            return View();
        }

        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = DrumCategoryService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListNewCategory");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListNewCategory");
            }
        }
        public ActionResult Remove(string tag, int id)
        {

            string flag = DrumCategoryService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListNewCategory");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListNewCategory");
            }
        }

        public ActionResult MyListItemgrid(string strStatus)
        {
            List<DrumCategorygrid> Reg = new List<DrumCategorygrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = DrumCategoryService.GetAllCategory(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    EditRow = "<a href=DrumCategory?id=" + dtUsers.Rows[i]["CATEGORYID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["CATEGORYID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                }
                else
                {

                    EditRow = "";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["CATEGORYID"].ToString() + "><img src='../Images/close_icon.png' alt='Deactivate' /></a>";

                }

               
                Reg.Add(new DrumCategorygrid
                {
                    id = dtUsers.Rows[i]["CATEGORYID"].ToString(),
                    catetype = dtUsers.Rows[i]["CATEGORYTYPE"].ToString(),
                    description = dtUsers.Rows[i]["DESCRIPTION"].ToString(),
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
