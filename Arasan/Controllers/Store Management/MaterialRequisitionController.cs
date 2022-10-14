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
        IPurchaseIndent PurIndent;
        public MaterialRequisitionController(IMaterialRequisition _MatreqService , IPurchaseIndent purIndent)
        {
            materialReq = _MatreqService;
            PurIndent = purIndent;
        }

        public IActionResult MaterialRequisition()
        {
            MaterialRequisition MR = new MaterialRequisition();
             PurchaseIndent pi = new PurchaseIndent();
            MR.Brlst = BindBranch();
            return View(MR);
        }

        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dtDesg = PurIndent.GetBranch();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["BRANCHID"].ToString(), Value = dtDesg.Rows[i]["BRANCHMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
