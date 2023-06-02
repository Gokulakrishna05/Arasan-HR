using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Master;
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
            if (id == null)
            {

            }
            else
            {

                DataTable dt = new DataTable();

                dt = Customer.GetCustomerType(id);
                if (dt.Rows.Count > 0)
                {
                    cu.Type = dt.Rows[0]["CUSTOMER_TYPE"].ToString();
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
                        TempData["notice"] = "CustomerType Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "CustomerType Updated Successfully...!";
                    }
                    return RedirectToAction("ListCustomerType");
                }

                else
                {
                    ViewBag.PageTitle = "Edit CustomerType";
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
            IEnumerable<CustomerType> ic = Customer.GetAllCustomerType();
            return View(ic);
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
    }
}
