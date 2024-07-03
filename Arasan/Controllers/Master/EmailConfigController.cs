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
    public class EmailConfigController : Controller
    {
        IEmailConfig EmailConfigService;

        IConfiguration? _configuration;
        private string? _connectionString;

        DataTransactions datatrans;

        public EmailConfigController(IEmailConfig _EmailConfigService, IConfiguration _configuration)
        {
            EmailConfigService = _EmailConfigService;
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public IActionResult EmailConfig(string id)
        {

            EmailConfig EC = new EmailConfig();
            EC.createby = Request.Cookies["UserId"];
            //for edit & delete

            if (id != null)
                {
                    DataTable dt = new DataTable();
                    double total = 0;
                    dt = EmailConfigService.GetEmailConfig(id);
                    if (dt.Rows.Count > 0)
                    {
                        EC.SMTP = dt.Rows[0]["SMTP_HOST"].ToString();
                        EC.Port = dt.Rows[0]["PORT_NO"].ToString();
                        EC.Email = dt.Rows[0]["EMAIL_ID"].ToString();
                        EC.Password = dt.Rows[0]["PASSWORD"].ToString();
                        EC.ID = id;
                        EC.SSL = dt.Rows[0]["SSL"].ToString();
                        EC.Signature = dt.Rows[0]["SIGNATURE"].ToString();
                    }
                }

            return View(EC);
        }


        [HttpPost] 
        public IActionResult EmailConfig(EmailConfig EC, string id)
        {

            try
            {
                EC.ID = id;
                string Strout = EmailConfigService.EmailConfigCRUD(EC);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (EC.ID == null)
                    {
                        TempData["notice"] = "EmailConfig Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "EmailConfig Updated Successfully...!";
                    }
                    return RedirectToAction("ListEmailConfig");
                }

                else
                {
                    ViewBag.PageTitle = "Edit EmailConfig";
                    TempData["notice"] = Strout;
                    //return View();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(EC);
        }

        public IActionResult ListEmailConfig()
        {
            return View();
     
        }

        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = EmailConfigService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListEmailConfig");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListEmailConfig");
            }
        }
        public ActionResult Remove(string tag, int id)
        {

            string flag = EmailConfigService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListEmailConfig");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListEmailConfig");
            }
        }

        public ActionResult MyListItemgrid(string strStatus)
        {
            List<EmailConfigGrid> Reg = new List<EmailConfigGrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = EmailConfigService.GetAllEmailconfig(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y") 
                {

                    EditRow = "<a href=EmailConfig?id=" + dtUsers.Rows[i]["EMAILCONFIG_ID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["EMAILCONFIG_ID"].ToString() + "";
                }

                else
                {
                    EditRow = "";
                    DeleteRow = "Remove?tag=Del&id=" + dtUsers.Rows[i]["EMAILCONFIG_ID"].ToString() + "";
                }
                Reg.Add(new EmailConfigGrid
                {
                    id = dtUsers.Rows[i]["EMAILCONFIG_ID"].ToString(),
                    smtp = dtUsers.Rows[i]["SMTP_HOST"].ToString(),
                    port = dtUsers.Rows[i]["PORT_NO"].ToString(),
                    email = dtUsers.Rows[i]["EMAIL_ID"].ToString(),
                    password = dtUsers.Rows[i]["PASSWORD"].ToString(),
                    ssl = dtUsers.Rows[i]["SSL"].ToString(),
                    signature = dtUsers.Rows[i]["SIGNATURE"].ToString(),
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
