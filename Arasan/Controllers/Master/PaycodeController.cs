using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Controllers 
{
    public class PaycodeController : Controller
    {
        IPaycodeService paycodeService;
        DataTransactions datatrans;
        IConfiguration? _configuratio;
        private string? _connectionString;
        public PaycodeController(IPaycodeService _paycodeService, IConfiguration _configuratio)
        {
            paycodeService = _paycodeService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult Paycode(string id)
        {
            Paycode py = new Paycode();
            py.Set = DateTime.Now.ToString("dd-MMM-yyyy");

            List<subgroup> TData = new List<subgroup>();
            subgroup tda = new subgroup();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new subgroup();
                    tda.Less = BindAdd();
                    tda.callst = BindCal();

                    tda.BasedOn = "ONLY WORKINGDAYS";
                    tda.Print = "N";
                    tda.Display = "N";
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                DataTable dt = new DataTable();

                //double total = 0;
                //dt = paycodeService.GetPaycode(id);
                if (dt.Rows.Count > 0)
                {
                    
                    py.Set = dt.Rows[0]["DOCDATE"].ToString();
                    
                    py.ID = id;
                }
            }
            DataTable dt2 = new DataTable();
            //dt2 = paycodeService.GetEditPayCodes(id);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new subgroup();

                    tda.Paycode = dt2.Rows[i]["PAYCODE"].ToString();
                    tda.Print = dt2.Rows[i]["PRINT"].ToString();
                    tda.PrintAs = dt2.Rows[i]["PRINTAS"].ToString();
                    tda.Less = BindAdd();
                    tda.Addorless = dt2.Rows[i]["ADDORLESS"].ToString();
                    tda.callst = BindCal();
                    tda.CalculateFrom = dt2.Rows[i]["PAYCODEVALUE"].ToString();
                    tda.BasedOn = dt2.Rows[i]["PAYCALCULATEFOR"].ToString();
                    tda.Display = dt2.Rows[i]["DISPLAY"].ToString();
                    tda.Formula = dt2.Rows[i]["FORMULA"].ToString();
                    tda.Sno = dt2.Rows[i]["SNO"].ToString();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            py.PayLists=TData;
            return View(py);
        }
        public IActionResult ViewPaycode(string id)
        {
            Paycode M = new Paycode();
            List<subgroup> TData = new List<subgroup>();
            subgroup tda = new subgroup();

            DataTable dt = new DataTable();


            dt = datatrans.GetData("Select  DOCID,to_char (DOCDATE,'dd-MON-yyyy')DOCDATE from PARAMETERBASIC  WHERE PARAMETERBASICID='" + id + "'");

            //"Select IGROUP,ISUBGROUP,ITEMGROUP,SUBGROUPCODE,SUBCATEGORY,BINNO,BINYN,LOTYN,RHYN,RUNPERQTY,RUNHRS,COSTCATEGORY,AUTOCONSYN,QCT,DRUMYN,ITEMFROM,ETARIFFMASTER.TARIFFID,PURCAT,MAJORYN,to_char(LATPURDT,'dd-MON-yyyy')LATPURDT,ITEMID,ITEMDESC,REORDERQTY,REORDERLVL,MINSTK,UNITMAST.UNITID,MASTER.MNAME,HSN,SELLINGPRICE,EXPYN,VALMETHOD,SERIALYN,BSTATEMENTYN,TESTTBASIC.TEMPLATEID,QCCOMPFLAG,LATPURPRICE,REJRAWMATPER,RAWMATPER,ADD1PER,ADD1,RAWMATCAT,ITEMACC,PTEMPLATEID,CURINGDAY,AUTOINDENT from ITEMMASTER LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT LEFT OUTER JOIN MASTER ON MASTER.MASTERID=ITEMMASTER.ITEMACC LEFT OUTER JOIN TESTTBASIC ON TESTTBASIC.TESTTBASICID=ITEMMASTER.TEMPLATEID LEFT OUTER JOIN ETARIFFMASTER ON ETARIFFMASTER.ETARIFFMASTERID=ITEMMASTER.TARIFFID   where ITEMMASTERID=" + id + "");

            if (dt.Rows.Count > 0)
            {
                M.Set = dt.Rows[0]["DOCDATE"].ToString();
               

               
            }
            DataTable dt2 = new DataTable();

            dt2 = datatrans.GetData("Select  PARAMETERBASICID,PAYCODE,PRINT, PRINTAS, ADDORLESS, PAYCODEVALUE, PAYCALCULATEFOR,SNO,FORMULA,DISPLAY from PARAMETERDETAIL  WHERE PARAMETERBASICID='" + id + "'");

            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {



                    tda = new subgroup();
                    tda.Paycode = dt2.Rows[0]["PAYCODE"].ToString();
                    tda.Print = dt2.Rows[0]["PRINT"].ToString();
                    tda.PrintAs = dt2.Rows[0]["PRINTAS"].ToString();
                    tda.Addorless = dt2.Rows[0]["ADDORLESS"].ToString();
                    tda.CalculateFrom = dt2.Rows[0]["PAYCODEVALUE"].ToString();
                    tda.BasedOn = dt2.Rows[0]["PAYCALCULATEFOR"].ToString();
                    tda.Formula = dt2.Rows[0]["FORMULA"].ToString();
                   tda.Order = dt2.Rows[0]["SNO"].ToString();

                    tda.Display = dt2.Rows[0]["DISPLAY"].ToString();
                    tda.Less = BindAdd();
                    tda.callst = BindCal();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }


            M.PayLists = TData;
            
            return View(M);
        }
        public JsonResult GetPaycodeJSON()
        {
            return Json(BindAdd());
        }
        public JsonResult GetPaycodesJSON()
        {
            return Json(BindCal());
        }
        [HttpPost]
        public ActionResult Paycode(Paycode by, string id)
        {

            try
            {
                by.ID = id;
                string Strout = paycodeService.PaycodeCRUD(by);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (by.ID == null)
                    {
                        TempData["notice"] = "Paycode Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Paycode Updated Successfully...!";
                    }
                    return RedirectToAction("ListPaycode");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Paycode";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception it)
            {
                throw it;
            }

            return View(by);
        }
        public IActionResult ListPaycode()
        {
            return View();
        }
        public List<SelectListItem> BindAdd()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "ADD", Value = "ADD" });
                lstdesg.Add(new SelectListItem() { Text = "LESS", Value = "LESS" });
                lstdesg.Add(new SelectListItem() { Text = "CALC", Value = "CALC" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindCal()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "FROM EMPLOYEEMASTER", Value = "FROM EMPLOYEEMASTER" });
                lstdesg.Add(new SelectListItem() { Text = "FROM TRANSACTION", Value = "FROM TRANSACTION" });
                lstdesg.Add(new SelectListItem() { Text = "FROM FORMULA", Value = "FROM FORMULA" });
                lstdesg.Add(new SelectListItem() { Text = "FROM SLAB", Value = "FROM SLAB" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult DeleteMR(string tag, string id)
        {

            string flag = paycodeService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListPaycode");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListPaycode");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = paycodeService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListPaycode");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListPaycode");
            }
        }

        public ActionResult MyListItemgrid(string strStatus)
        {
            List<IPaycodeGrid> Reg = new List<IPaycodeGrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = paycodeService.GetAlLPaycode(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string ViewRow = string.Empty;


                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    EditRow = "<a href=Paycode?id=" + dtUsers.Rows[i]["PARAMETERBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit'/></a>";
                    ViewRow = "<a href=ViewPaycode?id=" + dtUsers.Rows[i]["PARAMETERBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='Waiting for approval' /></a>";

                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["PARAMETERBASICID"].ToString() + "";
                }
                else
                {

                    EditRow = "";
                    DeleteRow = "Remove?tag=Del&id=" + dtUsers.Rows[i]["PARAMETERBASICID"].ToString() + "";

                }

                Reg.Add(new IPaycodeGrid
                {
                    id = dtUsers.Rows[i]["PARAMETERBASICID"].ToString(),
                    docid = dtUsers.Rows[i]["DOCID"].ToString(),
                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                   
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
