using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Models;
using Oracle.ManagedDataAccess.Client;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

using Newtonsoft.Json.Linq;
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
                    sq.Start = dt.Rows[0]["STDT"].ToString();
                    sq.End = dt.Rows[0]["EDDT"].ToString();
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
            IEnumerable<Sequence> cmp = sequence.GetAllSequence();
            return View(cmp);
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

    }
}
