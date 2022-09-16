using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;


namespace Arasan.Controllers.Stores_Management
{
    public class StoresReturnController : Controller
    {
        public IActionResult StoresReturn()
        {
            return View();
        }
        public IActionResult Returnable_NonReturnable_Dc()
        {
            return View();
        }
        public IActionResult Stores_Acceptance()
        {
            return View();
        }
        public IActionResult Receipt_Against_Returnable_DC()
        {
            return View();
        }
        public IActionResult Stores_Issuse_Consumbables()
        {
            return View();
        }
        public IActionResult Purchase_Indent()
        {
            return View();
        }
        public IActionResult List_Purchase_Indent()
        {
            return View();
        }
        public IActionResult Stores_Issuse_Production()
        {
            return View();
        }
        public IActionResult Material_Requisition_Short_Close()
        {
            return View();
        }
        public IActionResult Purchase_Return()
        {
            return View();
        }
        public IActionResult Receipt_for_SubContract()
        {
            return View();
        }
        public IActionResult Direct_Deducation()
        {
            return View();
        }
        public IActionResult Direct_Addition()
        {
            return View();
        }
        public IActionResult Sub_Contracting_DC()
        {
            return View();
        }
        public IActionResult Item_Transfer()
        {
            return View();
        }
        public IActionResult Sub_Contracting_Material_Receipt()
        {
            return View();
        }
        public IActionResult Material_Requisition()
        {
            return View();
        }
        public IActionResult List_Material_Requisition()
        {
            return View();
        }
    }
}
