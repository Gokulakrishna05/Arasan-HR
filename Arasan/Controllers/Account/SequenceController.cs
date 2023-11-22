using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Models;
using Oracle.ManagedDataAccess.Client;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

using Newtonsoft.Json.Linq;
using DocumentFormat.OpenXml.Bibliography;
//using PdfSharp.Pdf.Content.Objects;


namespace Arasan.Controllers 
{
    public class SequenceController : Controller
    {
        ISequence sequence;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public SequenceController(ISequence _sequence, IConfiguration _configuratio)
        {
            sequence = _sequence;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult Sequence(string id)
        {
            Sequence sq = new Sequence();
            sq.Start = DateTime.Now.ToString("dd-MMM-yyyy");
            if (id == null)
            {


            }
            else
            {

                DataTable dt = new DataTable();

                dt = sequence.GetSequence(id);
                if (dt.Rows.Count > 0)
                {
                    sq.Prefix = dt.Rows[0]["PREFIX"].ToString();
                    sq.Des = dt.Rows[0]["DESCRIPTION"].ToString();
                    sq.Last = dt.Rows[0]["LASTNO"].ToString();
                    sq.Trans = dt.Rows[0]["TRANSTYPE"].ToString();
                    sq.Start = dt.Rows[0]["STDATE"].ToString();
                    sq.End = dt.Rows[0]["EDDATE"].ToString();
                    sq.ID = id;

                } 

            }
            return View(sq);
        }
        [HttpPost]
        public ActionResult Sequence(Sequence Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = sequence.SequenceCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Sequence Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Sequence Updated Successfully...!";
                    }
                    return RedirectToAction("ListSequence");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Sequence";
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
        public IActionResult ListSequence()
        {
            //IEnumerable<Sequence> cmp = sequence.GetAllSequence();
            return View(/*cmp*/);
        }
        public ActionResult DeleteSeq(string tag, int id)
        {

            string flag = sequence.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListSequence");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListSequence");
            }
        }

        public ActionResult MyListItemgrid(string strStatus)
        {
            List<Sequencegrid> Reg = new List<Sequencegrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = sequence.GetAllSeq(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                EditRow = "<a href=Sequence?id=" + dtUsers.Rows[i]["SEQUENCEID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                DeleteRow = "<a href=DeleteSeq?tag=Del&id=" + dtUsers.Rows[i]["SEQUENCEID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";

                Reg.Add(new Sequencegrid
                {
                    id = dtUsers.Rows[i]["SEQUENCEID"].ToString(),
                    prefix = dtUsers.Rows[i]["PREFIX"].ToString(),
                    trans = dtUsers.Rows[i]["TRANSTYPE"].ToString(),
                    last = dtUsers.Rows[i]["LASTNO"].ToString(),
                    des = dtUsers.Rows[i]["DESCRIPTION"].ToString(),
                    start = dtUsers.Rows[i]["STDATE"].ToString(),
                    end = dtUsers.Rows[i]["EDDATE"].ToString(),
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
