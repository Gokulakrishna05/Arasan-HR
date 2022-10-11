using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Interface.Master;
using Arasan.Interface.Store_Management;
using Arasan.Models.Store_Management;

namespace Arasan.Controllers.Store_Management
{
    public class MaterialRequisitionController : Controller
    {
        IMaterialRequisition materialReq;

        public IActionResult MaterialRequisition()
        {
            MaterialRequisition MR = new MaterialRequisition();
            return View(MR);
        }
    }
}
