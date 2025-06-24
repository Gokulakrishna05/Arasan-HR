using Arasan.Interface;
 using Arasan.Models;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Controllers
{
    public class ShiftController : Controller
    {
        IShift shift;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;

        public ShiftController(IShift _shift, IConfiguration _configuratio)
        {
            shift = _shift;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult Shift(string id)
        {
            Shift py = new Shift();
            py.createby = Request.Cookies["UserId"];
              

            
            if (id == null)
            {
             
            }
            else
            {
                DataTable dt = new DataTable();

                //double total = 0;
                dt = datatrans.GetData("SELECT SHIFTNO,FROMTIME,TOTIME,SHIFTHRS,SHIFTOTHRS FROM SHIFTMAST WHERE SHIFTMASTID='" + id+"'");
                if (dt.Rows.Count > 0)
                {

                    py.shiftn = dt.Rows[0]["SHIFTNO"].ToString();
                    py.ftime = dt.Rows[0]["FROMTIME"].ToString();
                    py.ttime = dt.Rows[0]["TOTIME"].ToString();
                    py.shifthrs = dt.Rows[0]["SHIFTHRS"].ToString();
                    py.othrs = dt.Rows[0]["SHIFTOTHRS"].ToString();
 
                    py.ID = id;
                }
            }
             
            return View(py);
        }
        [HttpPost]

        public ActionResult Shift(Shift by, string id)
        {

            try
            {
                by.ID = id;
                string Strout = shift.ShiftCRUD(by);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (by.ID == null)
                    {
                        TempData["notice"] = "Shift Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Shift Updated Successfully...!";
                    }
                    return RedirectToAction("ListShift");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Shift";
                    TempData["notice"] = Strout;
                    return RedirectToAction("Shift");
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(by);
        }
        public IActionResult ListShift()
        {
            return View();
        }
        public ActionResult MyListItemgrid(string strStatus)
        {
            List<shiftgrid> Reg = new List<shiftgrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = shift.GetAlLshift(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string ViewRow = string.Empty;


                //if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                //{

                    EditRow = "<a href=Shift?id=" + dtUsers.Rows[i]["SHIFTMASTID"].ToString() + "><img src='../Images/edit.png' alt='Edit'/></a>";
                    ViewRow = "<a href=ViewPayPeriod?id=" + dtUsers.Rows[i]["SHIFTMASTID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='Waiting for approval' /></a>";

                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["SHIFTMASTID"].ToString() + "";
                //}
                //else
                //{

                //    EditRow = "";
                //    DeleteRow = "Remove?tag=Del&id=" + dtUsers.Rows[i]["SHIFTMASTID"].ToString() + "";

                //}

                Reg.Add(new shiftgrid
                {
                    shiftn = dtUsers.Rows[i]["SHIFTNO"].ToString(),
                    fromtime = dtUsers.Rows[i]["FROMTIME"].ToString(),
                    totime = dtUsers.Rows[i]["TOTIME"].ToString(),
                    shifthrs = dtUsers.Rows[i]["SHIFTHRS"].ToString(),
                    othrs = dtUsers.Rows[i]["SHIFTOTHRS"].ToString(),
 
                    editrow = EditRow,
                    viewrow = ViewRow,

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
