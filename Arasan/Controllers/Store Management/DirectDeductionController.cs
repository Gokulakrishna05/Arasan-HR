using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Interface.Store_Management;
using Arasan.Models;
using Arasan.Services.Store_Management;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers.Store_Management
{
    public class DirectDeductionController : Controller
    {
        IDirectDeductionService DirectDeductionService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public DirectDeductionController(IDirectDeductionService _DirectDeductionService, IConfiguration _configuratio)
        {
            DirectDeductionService = _DirectDeductionService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult DirectDeduction(string id)
        {
            DirectDeduction st = new DirectDeduction();
            st.Loc = BindLocation();
            st.Brlst = BindBranch();
            st.Branch = Request.Cookies["BranchId"];
            st.assignList = BindEmp();
            DataTable dtv = datatrans.GetSequence("Ddecu");
            if (dtv.Rows.Count > 0)
            {
                st.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
            st.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            List<DeductionItem> TData = new List<DeductionItem>();
            DeductionItem tda = new DeductionItem();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new DeductionItem();
                    tda.ItemGrouplst = BindItemGrplst();
                    tda.Itemlst = BindItemlst("");
                    tda.Processlst = BindProcess();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                //st = DirectDeductionService.GetDirectDeductionById(id);

                DataTable dt = new DataTable();
                double total = 0;
                dt = DirectDeductionService.GetDirectDeductionDetails(id);
                if (dt.Rows.Count > 0)
                {
                    st.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    st.Location = dt.Rows[0]["LOCID"].ToString();
                    st.DocId = dt.Rows[0]["DOCID"].ToString();
                    st.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    st.Dcno = dt.Rows[0]["DCNO"].ToString();
                    st.Reason = dt.Rows[0]["REASON"].ToString();
                    st.Gro = dt.Rows[0]["GROSS"].ToString();
                    st.Entered = dt.Rows[0]["ENTBY"].ToString();
                    st.Narr = dt.Rows[0]["NARRATION"].ToString();
                    st.NoDurms = dt.Rows[0]["NOOFD"].ToString();
                    st.ID = id;



                }
                DataTable dt2 = new DataTable();
                dt2 = DirectDeductionService.GetDDItemDetails(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new DeductionItem();
                        double toaamt = 0;
                        tda.ItemGrouplst = BindItemGrplst();
                        DataTable dt3 = new DataTable();
                        dt3 = datatrans.GetItemSubGroup(dt2.Rows[i]["ITEMID"].ToString());
                        if (dt3.Rows.Count > 0)
                        {
                            tda.ItemGroupId = dt3.Rows[0]["SUBGROUPCODE"].ToString();
                        }
                        tda.Itemlst = BindItemlst(tda.ItemGroupId);
                        tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();

                        DataTable dt4 = new DataTable();
                        dt4 = datatrans.GetItemDetails(tda.ItemId);
                        if (dt4.Rows.Count > 0)
                        {

                            tda.ConFac = dt4.Rows[0]["CF"].ToString();
                            tda.rate = Convert.ToDouble(dt4.Rows[0]["LATPURPRICE"].ToString());
                        }
                        tda.Quantity = Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                        toaamt = tda.rate * tda.Quantity;
                        total += toaamt;
                        //tda.QtyPrim= Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                        tda.Amount = toaamt;
                        tda.Unit = dt2.Rows[i]["UNITID"].ToString();

                        //tda.DRLst = BindDrum();
                        //tda.SRLst = BindSerial();
                        //tda.Drum = dt2.Rows[i]["DRUMYN"].ToString();
                        //tda.Serial = dt2.Rows[i]["SERIALYN"].ToString();
                        //tda.unitprim= dt2.Rows[i]["UNITID"].ToString();
                        //tda.FromBin = Convert.ToDouble(dt2.Rows[i]["CGSTPER"].ToString() == "" ? "0" : dt2.Rows[i]["CGSTPER"].ToString());
                        tda.BinID = Convert.ToDouble(dt2.Rows[i]["BINID"].ToString() == "" ? "0" : dt2.Rows[i]["BINID"].ToString());
                        //tda.Process = Convert.ToDouble(dt2.Rows[i]["PROCESSID"].ToString() == "" ? "0" : dt2.Rows[i]["PROCESSID"].ToString());
                        tda.Processlst = BindProcess();
                        tda.Process = dt2.Rows[i]["PROCESSID"].ToString();
                        //tda.Indp = Convert.ToDouble(dt2.Rows[i]["INDP"].ToString() == "" ? "0" : dt2.Rows[i]["INDP"].ToString());
                        //tda.SGSTAmt = Convert.ToDouble(dt2.Rows[i]["SGSTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["SGSTAMT"].ToString());
                        //tda.IGSTAmt = Convert.ToDouble(dt2.Rows[i]["IGSTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["IGSTAMT"].ToString());
                        //tda.DiscPer = Convert.ToDouble(dt2.Rows[i]["DISCPER"].ToString() == "" ? "0" : dt2.Rows[i]["DISCPER"].ToString());
                        //tda.DiscAmt = Convert.ToDouble(dt2.Rows[i]["DISCAMT"].ToString() == "" ? "0" : dt2.Rows[i]["DISCAMT"].ToString());
                        //tda.FrieghtAmt = Convert.ToDouble(dt2.Rows[i]["FREIGHTCHGS"].ToString() == "" ? "0" : dt2.Rows[i]["FREIGHTCHGS"].ToString());
                        //tda.TotalAmount = Convert.ToDouble(dt2.Rows[i]["TOTALAMT"].ToString() == "" ? "0" : dt2.Rows[i]["TOTALAMT"].ToString());

                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }

            }
            st.Itlst = TData;
            return View(st);
        }
        [HttpPost]
        public ActionResult DirectDeduction(DirectDeduction ss, string id)
        {

            try
            {
                ss.ID = id;
                string Strout = DirectDeductionService.DirectDeductionCRUD(ss);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (ss.ID == null)
                    {
                        TempData["notice"] = " DirectDeduction Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " DirectDeduction Updated Successfully...!";
                    }
                    return RedirectToAction("ListDirectDeduction");
                }

                else
                {
                    ViewBag.PageTitle = "Edit DirectDeduction";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(ss);
        }
        public ActionResult MyListDirectDeductionGrid(string strStatus)
        {
            List<ListDirectDeductionItem> Reg = new List<ListDirectDeductionItem>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)DirectDeductionService.GetAllListDirectDeductionItems(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                //string MailRow = string.Empty;
                //string FollowUp = string.Empty;
                //string MoveToQuo = string.Empty;
                string View = string.Empty;
                string EditRow = string.Empty;
                string DeleteRow = string.Empty;

                View = "<a href=viewDirectDeduction?id=" + dtUsers.Rows[i]["DEDBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View Details' width='20' /></a>";
                EditRow = "<a href=DirectDeduction?id=" + dtUsers.Rows[i]["DEDBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                DeleteRow = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["DEDBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";

                Reg.Add(new ListDirectDeductionItem
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["DEDBASICID"].ToString()),
                    branch = dtUsers.Rows[i]["BRANCHID"].ToString(),
                    docNo = dtUsers.Rows[i]["DOCID"].ToString(),
                    docDate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    loc = dtUsers.Rows[i]["LOCID"].ToString(),
                    entby = dtUsers.Rows[i]["ENTBY"].ToString(),
                    view = View,
                    editrow = EditRow,
                    delrow = DeleteRow,



                });
            }

            return Json(new
            {
                Reg
            });

        }
        public IActionResult ListDirectDeduction()
        {
            //IEnumerable<DirectDeduction> sta = DirectDeductionService.GetAllDirectDeduction(st, ed);
            return View();
        }
        public List<SelectListItem> BindProcess()
        {
            try
            {
                DataTable dtDesg = DirectDeductionService.BindProcess();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PROCESSID"].ToString(), Value = dtDesg.Rows[i]["PROCESSMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult DeleteItem(string tag, string id)
        {

            string flag = DirectDeductionService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListDirectDeduction");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListDirectDeduction");
            }
        }
        public List<SelectListItem> BindLocation()
        {
            try
            {
                DataTable dtDesg = DirectDeductionService.GetLocation();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOCID"].ToString(), Value = dtDesg.Rows[i]["LOCDETAILSID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindEmp()
        {
            try
            {
                DataTable dtDesg = datatrans.GetEmp();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPNAME"].ToString(), Value = dtDesg.Rows[i]["EMPNAME"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindItemlst(string value) 
        {
            try
            {
                DataTable dtDesg = DirectDeductionService.GetItem(value);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["ITEMMASTERID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindItemGrplst()
        {
            try
            {
                DataTable dtDesg = datatrans.GetItemSubGrp();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SGCODE"].ToString(), Value = dtDesg.Rows[i]["ITEMSUBGROUPID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dtDesg = DirectDeductionService.GetBranch();
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
        public ActionResult GetItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string unit = "";
                string CF = "";
                string price = "";
                dt = datatrans.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    unit = dt.Rows[0]["UNITID"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();
                    dt1 = DirectDeductionService.GetItemCF(ItemId,dt.Rows[0]["UNITMASTID"].ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        CF = dt1.Rows[0]["CF"].ToString();
                    }
                }

                var result = new { unit = unit, CF = CF, price = price };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetItemJSON(string itemid)
        {
            DeductionItem model = new DeductionItem();
            model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst(itemid));

        }
        public JsonResult GetItemGrpJSON()
        {
            //DeductionItem model = new DeductionItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItemGrplst());
        }
        public JsonResult GetItemProcessJSON()
        {
            //DirectItem model = new DirectItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindProcess());
        }


        public IActionResult viewDirectDeduction(string id)
        {
            DirectDeduction st = new DirectDeduction();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();

            dt = DirectDeductionService.GetDDByName(id);
            if (dt.Rows.Count > 0)
            {
                st.Branch = dt.Rows[0]["BRANCHID"].ToString();
                st.Location = dt.Rows[0]["LOCID"].ToString();
                st.DocId = dt.Rows[0]["DOCID"].ToString();
                st.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                st.Dcno = dt.Rows[0]["DCNO"].ToString();
                st.Reason = dt.Rows[0]["REASON"].ToString();
                st.Gro = dt.Rows[0]["GROSS"].ToString();
                st.Entered = dt.Rows[0]["ENTBY"].ToString();
                st.Narr = dt.Rows[0]["NARRATION"].ToString();
                st.NoDurms = dt.Rows[0]["NOOFD"].ToString();
                st.Material = dt.Rows[0]["MATSUPP"].ToString();
                st.net = dt.Rows[0]["NET"].ToString();

                st.ID = id;

                List<DeductionItem> Data = new List<DeductionItem>();
                DeductionItem tda = new DeductionItem();
                //double tot = 0;

                dtt = DirectDeductionService.GetDDItem(id);
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {

                        //tda.ItemGroupId = dtt.Rows[i]["SGCODE"].ToString();
                        tda.ItemId = dtt.Rows[i]["ITEMID"].ToString();
                        tda.ConFac = dtt.Rows[i]["CONFAC"].ToString();
                        tda.Unit = dtt.Rows[i]["UNITID"].ToString();
                        tda.BinID = Convert.ToDouble(dtt.Rows[i]["BINID"].ToString() == "" ? "0" : dtt.Rows[i]["BINID"].ToString());
                        tda.Quantity = Convert.ToDouble(dtt.Rows[i]["QTY"].ToString() == "" ? "0" : dtt.Rows[i]["QTY"].ToString());
                        tda.rate = Convert.ToDouble(dtt.Rows[i]["RATE"].ToString() == "" ? "0" : dtt.Rows[i]["RATE"].ToString());
                        //tda.disc = Convert.ToDouble(dtt.Rows[i]["DISCPER"].ToString());
                        tda.Amount = Convert.ToDouble(dtt.Rows[i]["AMOUNT"].ToString() == "" ? "0" : dtt.Rows[i]["AMOUNT"].ToString());


                        tda.Process = dtt.Rows[i]["PROCESSID"].ToString() ;


                        //tda.Process = Convert.ToDouble(dtt.Rows[i]["PROCESSID"].ToString() == "" ? "0" : dtt.Rows[i]["PROCESSID"].ToString());

                        

                        tda.Unit = dtt.Rows[i]["UNIT"].ToString();
                        tda.BinID = Convert.ToDouble(dtt.Rows[i]["BINID"].ToString());
                        tda.Quantity = Convert.ToDouble(dtt.Rows[i]["QTY"].ToString());
                        tda.rate = Convert.ToDouble(dtt.Rows[i]["RATE"].ToString());
                        //tda.disc = Convert.ToDouble(dtt.Rows[i]["DISCPER"].ToString());
                        tda.Amount = Convert.ToDouble(dtt.Rows[i]["AMOUNT"].ToString());
                        tda.Processlst = BindProcess();
                        tda.Process = dtt.Rows[i]["PROCESSID"].ToString();
                        //tda.Process = Convert.ToDouble(dtt.Rows[i]["PROCESSID"].ToString());
                 
                        Data.Add(tda);
                    }
                }

                st.Itlst = Data;
            }
            return View(st);
        }


    }
}

