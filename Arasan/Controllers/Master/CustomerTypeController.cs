using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Master;
//using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class CustomerTypeController : Controller
    {
        ICustomerType Customer;
        public CustomerTypeController(ICustomerType _Customer)
        {
            Customer = _Customer;
        }
        public IActionResult CustomerType(string id)
        {
            CustomerType cu = new CustomerType();
            cu.createby = Request.Cookies["UserId"];
            if (id == null)
            {

            }
            else
            {

                DataTable dt = new DataTable();

                dt = Customer.GetCustomerType(id);
                if (dt.Rows.Count > 0)
                {
                    cu.Type = dt.Rows[0]["CUSTOMER_GROUP"].ToString();
                    cu.Des = dt.Rows[0]["DESCRIPTION"].ToString();
                    cu.ID = id;
                }
            }
            return View(cu);
        }
        [HttpPost]
        public ActionResult CustomerType(CustomerType Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = Customer.CustomerCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "CustomerGroup Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "CustomerGroup Updated Successfully...!";
                    }
                    return RedirectToAction("ListCustomerType");
                }

                else
                {
                    ViewBag.PageTitle = "Edit CustomerGroup";
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
        public IActionResult ListCustomerType()
        {
            return View();
        }

        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = Customer.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListCustomerType");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListCustomerType");
            }
        }
        public ActionResult Remove(string tag, int id)
        {

            string flag = Customer.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListCustomerType");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListCustomerType");
            }
        }

        public ActionResult MyListItemgrid(string strStatus)
        {
            List<CustomerGrid> Reg = new List<CustomerGrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = Customer.GetAllCUSTOMERTYPE(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    EditRow = "<a href=CustomerType?id=" + dtUsers.Rows[i]["CUSTOMERGROUPID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["CUSTOMERGROUPID"].ToString() + "";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "Remove?tag=Del&id=" + dtUsers.Rows[i]["CUSTOMERGROUPID"].ToString() + "";
                }

                Reg.Add(new CustomerGrid
                {
                    id = dtUsers.Rows[i]["CUSTOMERGROUPID"].ToString(),
                    type = dtUsers.Rows[i]["CUSTOMER_GROUP"].ToString(),
                    des = dtUsers.Rows[i]["DESCRIPTION"].ToString(),
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
