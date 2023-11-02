using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Mail;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Reporting.Map.WebForms.BingMaps;

namespace Arasan.Controllers
{
    public class PromotionMailController : Controller
    {
        IPromotionMailService PromotionMailService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public PromotionMailController(IPromotionMailService _PromotionMailService, IConfiguration _configuratio)
        {
            PromotionMailService = _PromotionMailService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult PromotionMail(string id)
        {
            PromotionMail br = new PromotionMail();
           


            //for edit & delete
            //if (id != null)
            //{
            //    DataTable dt = new DataTable();
            //    //double total = 0;
            //    dt = BranchService.GetEditBranch(id);
            //    if (dt.Rows.Count > 0)
            //    {
            //        br.CompanyName = dt.Rows[0]["COMPANYID"].ToString();
            //        br.BranchName = dt.Rows[0]["BRANCHID"].ToString();
            //        br.Address = dt.Rows[0]["ADDRESS1"].ToString();
            //        br.StateName = dt.Rows[0]["STATE"].ToString();
            //        br.Citylst = BindCity(br.StateName);
            //        br.City = dt.Rows[0]["CITY"].ToString();
            //        br.PinCode = dt.Rows[0]["PINCODE"].ToString();
            //        br.GSTNo = dt.Rows[0]["CSTNO"].ToString();
            //        br.GSTDate = dt.Rows[0]["CSTDATE"].ToString();
            //        br.ID = id;

            //    }
            //}
            return View(br);
        }
        //[HttpPost]
        //public ActionResult PromotionMail(PromotionMail Cy, string id)
        //{

        //    try
        //    {
        //        Cy.ID = id;
        //        string Strout = PromotionMailService.PromotionMailCRUD(Cy);
        //        if (string.IsNullOrEmpty(Strout))
        //        {
        //            if (Cy.ID == null)
        //            {
        //                TempData["notice"] = "PromotionMail Inserted Successfully...!";
        //            }
        //            else
        //            {
        //                TempData["notice"] = "PromotionMail Updated Successfully...!";
        //            }
        //            return RedirectToAction("PromotionMail");
        //        }

        //        else
        //        {
        //            ViewBag.PageTitle = "Edit PromotionMail";
        //            TempData["notice"] = Strout;
        //            //return View();
        //        }

        //        // }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return View(Cy);
        //}
        [HttpPost]
        public ActionResult PromotionMail(PromotionMail Cy, PromotionMail _email)
        {
           
                datatrans = new DataTransactions(_connectionString);
                DataTable ddt1 = new DataTable();
                ddt1 = datatrans.GetEmailConfig();
                string HostAdd = ddt1.Rows[0]["SMTP_HOST"].ToString();
                string FromEmailid = ddt1.Rows[0]["EMAIL_ID"].ToString();
                string password = ddt1.Rows[0]["PASSWORD"].ToString();
                int port = Convert.ToInt32(ddt1.Rows[0]["PORT_NO"].ToString());
                Boolean ssl = ddt1.Rows[0]["SSL"].ToString() == "No" ? false : true;

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(FromEmailid); //From Email Id  
                mailMessage.Subject = Cy.Sub; //Subject of Email  
                mailMessage.Body = Cy.editors; //body or message of Email  
                mailMessage.IsBodyHtml = true;
                mailMessage.To.Add(new MailAddress(Cy.To)); //adding multiple TO Email Id  
                if (!string.IsNullOrEmpty(Cy.Cc))
                {
                    string[] CCId = Cy.Cc.Split(',');
                    foreach (string CCEmail in CCId)
                    {
                        mailMessage.CC.Add(new MailAddress(CCEmail)); //Adding Multiple CC email Id  
                    }
                }
                if (!string.IsNullOrEmpty(Cy.Bcc))
                {
                    string[] bccid = Cy.Bcc.Split(',');

                    foreach (string bccEmailId in bccid)
                    {
                        mailMessage.Bcc.Add(new MailAddress(bccEmailId)); //Adding Multiple BCC email Id  
                    }
                }
                SmtpClient smtp = new SmtpClient();  // creating object of smptpclient  
                smtp.Host = HostAdd;              //host of emailaddress for example smtp.gmail.com etc  
                smtp.EnableSsl = ssl;
                NetworkCredential NetworkCred = new NetworkCredential();
                NetworkCred.UserName = mailMessage.From.Address;
                NetworkCred.Password = password;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = NetworkCred;
                smtp.Port = port;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            if (_email.Attachment.Length > 0)
            {
                string fileName = Path.GetFileName(_email.Attachment.FileName);
                mailMessage.Attachments.Add(new Attachment(_email.Attachment.OpenReadStream(), fileName));
            }
            //foreach (Customeremailattach cp in Cy.Upload)
            //{
            //    if (cp.Isvalid == "Y")
            //    {
            //        mailMessage.Attachments.Add(new System.Net.Mail.Attachment(cp.FilePath));
            //    }
            //}
            smtp.Send(mailMessage);
            return Ok();

            

        }

    }
}
