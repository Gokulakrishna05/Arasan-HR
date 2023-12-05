using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
//using Arasan.Models.Master;
using Arasan.Services;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class ETariffController : Controller
    {
        IETariff ETariffService;

        IConfiguration? _configuration;
        private string? _connectionString;

        public ETariffController(IETariff _ETariffService, IConfiguration _configuration)
        {
            ETariffService = _ETariffService;
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");

        }

        public IActionResult ETariff(string id)   
        {
            ETariff AG = new ETariff();

            if (id != null)
            {
                DataTable dt = new DataTable();
                double total = 0;
                dt = ETariffService.GetETariff(id);
                if (dt.Rows.Count > 0)
                {

                    AG.Tariff = dt.Rows[0]["TARIFFID"].ToString();
                    AG.Tariffdes = dt.Rows[0]["TARIFFDESC"].ToString();
                    AG.Per = dt.Rows[0]["PERCENTAGE"].ToString();
                    //AG.Cgst = dt.Rows[0]["CGST"].ToString();
                    //AG.Igst = dt.Rows[0]["IGST"].ToString();

                    AG.ID = id;
                }
            }
            return View(AG);
        }

        [HttpPost]
        public ActionResult ETariff(ETariff Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = ETariffService.ETariffCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "ETariff Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "ETariff Updated Successfully...!";
                    }
                    return RedirectToAction("ListETariff");
                }

                else
                {
                    ViewBag.PageTitle = "Edit ETariff";
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


        public IActionResult ListETariff()
        {
            return View();
        }
        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = ETariffService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListETariff");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListETariff");
            }
        }

        public ActionResult Remove(string tag, int id)
        {

            string flag = ETariffService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListETariff");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListETariff");
            }
        }

        public ActionResult MyListItemgrid(string strStatus)
        {
            List<ETariffgrid> Reg = new List<ETariffgrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;

            dtUsers = ETariffService.GetAllETariff(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    EditRow = "<a href=ETariff?id=" + dtUsers.Rows[i]["TARIFFMASTERID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["TARIFFMASTERID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                }
                else
                {

                    EditRow = "";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["TARIFFMASTERID"].ToString() + "><img src='../Images/close_icon.png' alt='Deactivate' /></a>";

                }

               
                Reg.Add(new ETariffgrid
                {
                    id = dtUsers.Rows[i]["TARIFFMASTERID"].ToString(),
                    tariff = dtUsers.Rows[i]["TARIFFID"].ToString(),
                    tariffdes = dtUsers.Rows[i]["TARIFFDESC"].ToString(),
                    sgst = dtUsers.Rows[i]["PERCENTAGE"].ToString(),
                    //cgst = dtUsers.Rows[i]["CGST"].ToString(),
                    //igst = dtUsers.Rows[i]["CGST"].ToString(),
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
