using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
using Intuit.Ipp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Controllers
{
    public class LeaveTypeMasterController : Controller
    {
        ILeaveTypeMaster LeaveTypeM;

        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public LeaveTypeMasterController(ILeaveTypeMaster _LeaveTypeM, IConfiguration _configuratio)
        {
            LeaveTypeM = _LeaveTypeM;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }


        public IActionResult LeaveTypeMaster(String id)
        {
            LeaveTypeMaster ic = new LeaveTypeMaster();
            //ic.LTNamelst = BindLTNamelst();
            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();

                dt = LeaveTypeM.GetLeaveTypeMasterEdit(id);
                if (dt.Rows.Count > 0)
                {
                    ic.ID = dt.Rows[0]["ID"].ToString();
                    ic.LTName = dt.Rows[0]["LEAVETYPENAME"].ToString();
                    ic.Des = dt.Rows[0]["DESCRIPTION"].ToString();
                    ic.Mapy = dt.Rows[0]["MAXIMUMALLOWEDPERYEAR"].ToString();
                }
            }
                return View(ic);
        }
        [HttpPost]
        public ActionResult LeaveTypeMaster(LeaveTypeMaster Em, string id)
        {


            try
            {
                Em.ID = id;
                string Strout = LeaveTypeM.GetInsLTM(Em);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Em.ID == null)
                    {
                        TempData["notice"] = "LeaveTypeMaster  Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "LeaveTypeMaster  Updated Successfully...!";
                    }
                    //return RedirectToAction("LeaveTypeMasterlist");
                }
                else
                {
                    TempData["notice"] = Strout;
                    ViewBag.PageTitle = "Edit LeaveTypeMaster";

                    return View(Em);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Em);

        }

        public IActionResult LeaveTypeMasterList()
        {
            return View();
        }

        public ActionResult MyListLeaveTypeMastergrid(string strStatus)
        {
            List<LeaveTypeMasterList> Reg = new List<LeaveTypeMasterList>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = LeaveTypeM.GetAllLeaveTypeMaster(strStatus);

            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string Regenerate = string.Empty;
                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string ViewRow = string.Empty;
                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    EditRow = "<a href=LeaveTypeMaster?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";

                    ViewRow = "<a href=ViewLeaveTypeMaster?id=" + dtUsers.Rows[i]["ID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='Waiting for approval' /></a>";


                    DeleteRow = "DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "";
                }
                else
                {
                    EditRow = "";


                    DeleteRow = " DeleteItem?tag=Active&id=" + dtUsers.Rows[i]["ID"].ToString() + "";
                }


                Reg.Add(new LeaveTypeMasterList
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    leavetypename = dtUsers.Rows[i]["LEAVETYPENAME"].ToString(),
                    description = dtUsers.Rows[i]["DESCRIPTION"].ToString(),
                    allowedperyear = dtUsers.Rows[i]["MAXIMUMALLOWEDPERYEAR"].ToString(),

                    editrow = EditRow,
                    viewrow = ViewRow,
                    delrow = DeleteRow,
                    //rrow = Regenerate
                });
            }

            return Json(new
            {
                Reg
            });



        }

        public IActionResult DeleteItem(string tag, string id)
        {
            string flag = LeaveTypeM.StatusChange(tag, id);

            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("LeaveTypeMasterList");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("LeaveTypeMasterList");
            }

        }

        public IActionResult ViewLeaveTypeMaster(string id)
        {
            LeaveTypeMaster Emp = new LeaveTypeMaster();

            //List<LeaveTypeMasterList> TData = new List<LeaveTypeMasterList>();
           // AttendanceDetails tda = new AttendanceDetails();


            DataTable dt = new DataTable();

            dt = datatrans.GetData("Select ID,LEAVETYPENAME,DESCRIPTION,MAXIMUMALLOWEDPERYEAR from LEAVETYPEMASTER WHERE ID='" + id + "'");
            //"Select IGROUP,ISUBGROUP,ITEMGROUP,SUBGROUPCODE,SUBCATEGORY,BINNO,BINYN,LOTYN,RHYN,RUNPERQTY,RUNHRS,COSTCATEGORY,AUTOCONSYN,QCT,DRUMYN,ITEMFROM,ETARIFFMASTER.TARIFFID,PURCAT,MAJORYN,to_char(LATPURDT,'dd-MON-yyyy')LATPURDT,ITEMID,ITEMDESC,REORDERQTY,REORDERLVL,MINSTK,UNITMAST.UNITID,MASTER.MNAME,HSN,SELLINGPRICE,EXPYN,VALMETHOD,SERIALYN,BSTATEMENTYN,TESTTBASIC.TEMPLATEID,QCCOMPFLAG,LATPURPRICE,REJRAWMATPER,RAWMATPER,ADD1PER,ADD1,RAWMATCAT,ITEMACC,PTEMPLATEID,CURINGDAY,AUTOINDENT from ITEMMASTER LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT LEFT OUTER JOIN MASTER ON MASTER.MASTERID=ITEMMASTER.ITEMACC LEFT OUTER JOIN TESTTBASIC ON TESTTBASIC.TESTTBASICID=ITEMMASTER.TEMPLATEID LEFT OUTER JOIN ETARIFFMASTER ON ETARIFFMASTER.ETARIFFMASTERID=ITEMMASTER.TARIFFID   where ITEMMASTERID=" + id + "");

            if (dt.Rows.Count > 0)
            {
                //Emp.ID = dt.Rows[0]["ID"].ToString();
                Emp.LTName = dt.Rows[0]["LEAVETYPENAME"].ToString();
                Emp.Des = dt.Rows[0]["DESCRIPTION"].ToString();
                Emp.Mapy = dt.Rows[0]["MAXIMUMALLOWEDPERYEAR"].ToString();
                Emp.ID = id;
            }
            return View(Emp);

        }



        

        //public List<SelectListItem> BindLTNamelst()
        //{
        //    try
        //    {
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        lstdesg.Add(new SelectListItem() { Text = "Casual", Value = "Casual" });
        //        lstdesg.Add(new SelectListItem() { Text = "Sick", Value = "Sick" });
        //        lstdesg.Add(new SelectListItem() { Text = "Earned", Value = "Earned" });
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

      



    }
}
