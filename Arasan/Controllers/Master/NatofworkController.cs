using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers.Master
{
    public class NatofworkController : Controller
    {
        INatofworkService NatofworkService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public NatofworkController(INatofworkService _NatofworkService, IConfiguration _configuratio)
        {
            NatofworkService = _NatofworkService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }


        public IActionResult Natofwork(string id)
        {
            Natofwork nat = new Natofwork();


            //for edit & delete
            if (id != null)
            {
                DataTable dt = new DataTable();
                //double total = 0;
                dt = NatofworkService.GetEditNatofwork(id);
                if (dt.Rows.Count > 0)
                {
                    nat.Natofworkname = dt.Rows[0]["NATOFWORK"].ToString();
                    nat.ID = id;

                }
            }
            return View(nat);
        }

        [HttpPost]

        public ActionResult Natofwork(Natofwork Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = NatofworkService.NatofworkCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Nature of Work Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Nature of Work Updated Successfully...!";
                    }
                    return RedirectToAction("ListNatofwork");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Natofwork";
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
        public IActionResult ListNatofwork()
        {
            return View();
        }

        public ActionResult MyListNatofworkgrid(string strStatus)
        {
            List<NatofworkItem> Reg = new List<NatofworkItem>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;

            dtUsers = NatofworkService.GetAllNatofwork(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    EditRow = "<a href=Natofwork?id=" + dtUsers.Rows[i]["NATOFWORKID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["NATOFWORKID"].ToString() + "";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "Remove?tag=Del&id=" + dtUsers.Rows[i]["NATOFWORKID"].ToString() + "";
                }

                Reg.Add(new NatofworkItem
                {
                    id = dtUsers.Rows[i]["NATOFWORKID"].ToString(),
                    natofwork = dtUsers.Rows[i]["NATOFWORK"].ToString(),
                    editrow = EditRow,
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }
        public ActionResult DeleteItem(string tag, int id)
        {

            string flag = NatofworkService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListNatofwork");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListNatofwork");
            }
        }
        public ActionResult Remove(string tag, int id)
        {

            string flag = NatofworkService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListNatofwork");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListNatofwork");
            }
        }
    }
}
