using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;


namespace Arasan.Controllers.Master
{
    public class CurrencyController : Controller
    {
        ICurrencyService CurrencyService;
        public CurrencyController(ICurrencyService _CurrencyService)
        {
            CurrencyService = _CurrencyService;
        }
        public IActionResult Currency(string id)
        {
            Currency cu = new Currency();
            if (id == null)
            {

            }
            else
            {
                cu = CurrencyService.GetCurrencyById(id);

            }
            return View(cu);
        }
        [HttpPost]
        public ActionResult Currency(Currency Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = CurrencyService.CurrencyCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Currency Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Currency Updated Successfully...!";
                    }
                    return RedirectToAction("ListCurrency");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Currency";
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
        public IActionResult ListCurrency()
        {
            IEnumerable<Currency> cmp = CurrencyService.GetAllCurrency();
            return View(cmp);
        }

       
    }
}
