using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Interface.Master;
using Org.BouncyCastle.Bcpg;
namespace Arasan.Controllers
{
    public class PIApprovalController : Controller
    {
        IPurchaseIndent PurIndent;
        private string? _connectionString;
        IConfiguration? _configuratio;
        DataTransactions datatrans;
        public PIApprovalController(IPurchaseIndent _PurService, IConfiguration _configuratio)
        {
            PurIndent = _PurService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);

        }
        public IActionResult List_PI_Approval()
        {
            return View();
        }
        public ActionResult IndentApproved(string id)
        {
            datatrans = new DataTransactions(_connectionString);
            string app1 = datatrans.GetDataString("SELECT APPROVED1 FROM PINDDETAIL WHERE  PINDDETAILID='" + id + "'");
            if (app1 == "")
            {
                bool result = datatrans.UpdateStatus("UPDATE PINDDETAIL SET APPROVED1='YES',APPROVAL1U='NAGES',APP1DT='" + DateTime.Now.ToString("dd-MMM-yyyy") + "' Where PINDDETAILID='" + id + "'");

            }
            else
            {
                bool result = datatrans.UpdateStatus("UPDATE PINDDETAIL SET APPROVED2='YES',APPROVAL2U='SRRAJAN',APP2DT='" + DateTime.Now.ToString("dd-MMM-yyyy") + "' Where PINDDETAILID='" + id + "'");

            }
            return RedirectToAction("List_PI_Approval");
        }
        public ActionResult IndentDisApproved(string id)
        {
            PurchaseIndent p = new PurchaseIndent();
            p.closereasonid = id;
            //string user= Request.Cookies["UserId"];
            //datatrans = new DataTransactions(_connectionString);
            //bool result = datatrans.UpdateStatus("UPDATE PINDDETAIL SET APPROVED1=' ',MODIFY_BY='"+ user+"',MODIFY_ON='" + DateTime.Now.ToString("dd-MMM-yyyy") + "' Where PINDDETAILID='" + id + "'");
            return View(p);
        }
        public ActionResult IndentDisApprove(string detid, string reason)
        {
            string user = Request.Cookies["UserId"];
            datatrans = new DataTransactions(_connectionString);
            bool result = datatrans.UpdateStatus("UPDATE PINDDETAIL SET APPROVED1=' ',MODIFY_BY='" + user + "',MODIFY_ON='" + DateTime.Now.ToString("dd-MMM-yyyy") + "',CLOSEREASON='"+reason+"' Where PINDDETAILID='" + detid + "'");
            return RedirectToAction("List_PI_Approval");
        }
    }
}
