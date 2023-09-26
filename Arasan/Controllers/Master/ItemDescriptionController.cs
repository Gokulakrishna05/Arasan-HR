using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Arasan.Controllers.Master
{
    public class ItemDescriptionController : Controller
    {
        IItemDescriptionService ItemDescriptionService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public ItemDescriptionController(IItemDescriptionService _ItemDescriptionService, IConfiguration _configuratio)
        {
            ItemDescriptionService = _ItemDescriptionService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult ItemDescription(string id)
        {
            ItemDescription br = new ItemDescription();
            //br.Deslst = BindDes();
            if (id != null)
            {
                DataTable dt = new DataTable();
                dt = ItemDescriptionService.GetEditItemDescription(id);
                if (dt.Rows.Count > 0)
                {
                    br.Des = dt.Rows[0]["TESTDESC"].ToString();
                    br.Unit = dt.Rows[0]["UNITID"].ToString();
                    br.Value = dt.Rows[0]["VALUEORMANUAL"].ToString();
                    br.ID = id;
                }
            }
            return View(br);

        }
        [HttpPost]
        public ActionResult ItemDescription(ItemDescription Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = ItemDescriptionService.ItemDescriptionCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Branch ItemDescription Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Branch ItemDescription Successfully...!";
                    }
                    return RedirectToAction("ListItemDescription");
                }

                else
                {
                    ViewBag.PageTitle = "Edit ItemDescription";
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
        public IActionResult ListItemDescription()
        {
            IEnumerable<ItemDescription> br = ItemDescriptionService.GetAllItemDescription();
            return View(br);
        }
        //public List<SelectListItem> BindDes()
        //{
        //    try
        //    {
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        lstdesg.Add(new SelectListItem() { Text = "MANUAL", Value = "MANUAL" });
        //        lstdesg.Add(new SelectListItem() { Text = "VALUE", Value = "VALUE" });
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

    }
}
