using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Master;
using AspNetCore.Reporting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Utilities;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Arasan.Controllers 
{
	public class APProductionentryController : Controller
	{
		IAPProductionEntry IProductionEntry;
		IConfiguration? _configuratio;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private string? _connectionString;
		DataTransactions datatrans;
		public APProductionentryController(IAPProductionEntry _IProductionEntry, IConfiguration _configuratio, IWebHostEnvironment WebHostEnvironment)
		{
            this._WebHostEnvironment = WebHostEnvironment;
            IProductionEntry = _IProductionEntry;
			_connectionString = _configuratio.GetConnectionString("OracleDBConnection");
			datatrans = new DataTransactions(_connectionString);
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public ActionResult APProductinentryselection()
        {
            var userId = Request.Cookies["UserName"];
            int cunt = datatrans.GetDataId("select count(*) as cunt from BPRODBASIC where IS_COMPLETE='No' AND ENTEREDBY='"+ userId + "'");
            if (cunt == 0)
            {
                return RedirectToAction("APProductionentry");
            }
            else
            {
                return RedirectToAction("APProductionentryDetail", new { tag = 1 });
            }
        }


		public ActionResult APProductionentry(string id)
		{
			APProductionentry ca = new APProductionentry();
			DataTable dtv = datatrans.GetSequence("APPro");
			ca.Loclst = BindAPWorkCenter();
			ca.processlst = BindProcess();
			
			ca.Shiftlst = BindShift();
            ca.Branch = Request.Cookies["BranchId"];
            var userId = Request.Cookies["UserId"];
            ca.Englst = BindEmp();
            ca.suplst = BindSupEmp(userId);
            ca.batchcomplete = "N";
			ca.Shift = "A";
			ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
			if (dtv.Rows.Count > 0)
			{
				ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
			}
			
			ca.shedulst = BindShedule();
			ca.Batchlst = BindBatch("");
			if (id == null)
			{

			}
			else
			{
				if (!string.IsNullOrEmpty(id))
				{
					ca.APID = id;
					
				}
			}

			//List<BreakDetail> TData3 = new List<BreakDetail>();
			//BreakDetail tda3 = new BreakDetail();
			//List<ProInput> TData = new List<ProInput>();
			//ProInput tda = new ProInput();
			//List<APProInCons> TData1 = new List<APProInCons>();
			//APProInCons tda1 = new APProInCons();
			//List<EmpDetails> TTData2 = new List<EmpDetails>();
			//EmpDetails tda2 = new EmpDetails();
			//for (int i = 0; i < 3; i++)
			//{
			//	tda3 = new BreakDetail();

			//	tda3.Machinelst = BindMachineID();
			//	tda3.Emplst = BindEmp();
			//	tda3.Isvalid = "Y";
			//	TData3.Add(tda3);

			//}
			//for (int i = 0; i < 3; i++)
			//{
			//	tda = new ProInput();

			//	tda.Itemlst = BindItemlst();


			//	tda.Isvalid = "Y";
			//	TData.Add(tda);

			//}
			//for (int i = 0; i < 1; i++)
			//{
			//	tda1 = new APProInCons();
			//	tda1.Itemlst = BindItemlst();
			//	tda1.Isvalid = "Y";
			//	TData1.Add(tda1);
			//}
			//for (int i = 0; i < 3; i++)
			//{
			//	tda2 = new EmpDetails();

			//	tda2.Employeelst = BindEmp();
			//	tda2.Isvalid = "Y";
			//	TTData2.Add(tda2);
			//}

			//ca.BreakLst = TData3;
			//ca.inplst = TData;
			//ca.EmplLst = TTData2;
			//ca.Binconslst = TData1;
			return View(ca);
		}
		public JsonResult GetEmpJSON()
		{
			//EnqItem model = new EnqItem();
			//  model.ItemGrouplst = BindItemGrplst(value);
			return Json(BindEmp());
		}
        public JsonResult GetLogJSON()
        {
            LogDetails model = new LogDetails();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(model);
        }
        public JsonResult GetBreakJSON()
		{
			//EnqItem model = new EnqItem();
			//  model.ItemGrouplst = BindItemGrplst(value);
			return Json(BindMachineID());
		}
		public JsonResult GetBreakEmpJSON()
		{
			//EnqItem model = new EnqItem();
			//  model.ItemGrouplst = BindItemGrplst(value);
			return Json(BindEmp());
		}



		[HttpPost]
		public ActionResult APProductionentry(APProductionentry Cy, string id)
		{

			try
			{
				Cy.ID = id;
				string Strout = IProductionEntry.APProductionEntryCRUD(Cy);
				if (string.IsNullOrEmpty(Strout))
				{
					if (Cy.ID == null)
					{
						TempData["notice"] = "APProductionentry Inserted Successfully...!";
					}
					else
					{
						TempData["notice"] = "APProductionentry Updated Successfully...!";
					}
					//return RedirectToAction("APProductionentryDetail", new { id = id });
				}

				else
				{
					ViewBag.PageTitle = "Edit APProductionentry";
					TempData["notice"] = Strout;
					//return View();
				}
				return RedirectToAction("APProductionentryDetail", new { id = Cy.APID ,tag=2});
				// }
			}
			catch (Exception ex)
			{
				throw ex;
			}
			
			return View(Cy);
		}

        [HttpPost]
        public ActionResult ApproveAPProductionentry(APProductionentryDet Cy, string id)
        {
            try
            {
                Cy.ID = id;
                string Strout = IProductionEntry.ApproveAPProEntryCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                     TempData["notice"] = "APProductionentry Approved Successfully...!";
                }

                else
                {
                    ViewBag.PageTitle = "Edit APProductionentry";
                    TempData["notice"] = Strout;
                }
                return RedirectToAction("ListAPProductionentry");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(Cy);
        }

        public List<SelectListItem> BindEmp( )
		{
			try
			{
				DataTable dtDesg = IProductionEntry.GetEmp();
				List<SelectListItem> lstdesg = new List<SelectListItem>();
				for (int i = 0; i < dtDesg.Rows.Count; i++)
				{
					lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPNAME"].ToString(), Value = dtDesg.Rows[i]["EMPMASTID"].ToString() });
				}
				return lstdesg;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
        public List<SelectListItem> BindReason()
        {
            try
            {
                DataTable dtDesg = IProductionEntry.GetReason();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["REASON"].ToString(), Value = dtDesg.Rows[i]["REASON"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindSupEmp(string id)
        {
            try
            {
                DataTable dtDesg = IProductionEntry.GetSupEmp(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPNAME"].ToString(), Value = dtDesg.Rows[i]["USERNAME"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindBatch(string value)
		{
			try
			{
				DataTable dtDesg = IProductionEntry.GetBatch(value);
				List<SelectListItem> lstdesg = new List<SelectListItem>();
				for (int i = 0; i < dtDesg.Rows.Count; i++)
				{
					lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["DOCID"].ToString() });
				}
				return lstdesg;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
        public JsonResult GetBatchJSON(string ItemId)
        {
            return Json(BindBatch(ItemId));
        }
        public List<SelectListItem> BindShedule()
        {
            try
            {
                DataTable dtDesg = datatrans.GetSchedule();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["PSBASICID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindAPWorkCenter()
		{
			try
			{
				DataTable dtDesg = IProductionEntry.GetAPWorkCenter();
				List<SelectListItem> lstdesg = new List<SelectListItem>();
				for (int i = 0; i < dtDesg.Rows.Count; i++)
				{
					lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["WCID"].ToString(), Value = dtDesg.Rows[i]["WCBASICID"].ToString() });
				}
				return lstdesg;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
        public List<SelectListItem> BindProcess()
        {
            try
            {
                DataTable dtDesg = IProductionEntry.GetProcess();
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
        public ActionResult GetEmployeeDetail(string ItemId)
		{
			try
			{
				DataTable dt = new DataTable();

				string code = "";
				string dept = "";


				dt = IProductionEntry.GetEmployeeDetails(ItemId);

				if (dt.Rows.Count > 0)
				{

					code = dt.Rows[0]["EMPID"].ToString();
                    dept = dt.Rows[0]["DEPTNAME"].ToString();



				}

				var result = new { code = code , dept = dept };
				return Json(result);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
        public IActionResult ApproveAPProductionentry(string id)
        {
            APProductionentryDet ca = new APProductionentryDet();
            DataTable dt = new DataTable();
            dt = IProductionEntry.GetAPProductionentryName(id);
            if (dt.Rows.Count > 0)
            {
                ca.Location = dt.Rows[0]["WCID"].ToString();
                ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                ca.DocId = dt.Rows[0]["DOCID"].ToString();
                ca.Eng = dt.Rows[0]["EMPNAME"].ToString();
                ca.Shift = dt.Rows[0]["SHIFT"].ToString();
                ViewBag.shift = dt.Rows[0]["SHIFT"].ToString();
                ca.SchQty = dt.Rows[0]["SCHQTY"].ToString();
                ca.ProdQty = dt.Rows[0]["PRODQTY"].ToString();
                ca.BatchNo = dt.Rows[0]["BATCH"].ToString();
                ca.batchcomplete = dt.Rows[0]["BATCHYN"].ToString();
                ca.ID = id;
                ca.LOCID= dt.Rows[0]["WCBASICID"].ToString();
                ca.BranchId = Request.Cookies["BranchId"];
            }
            DataTable dt2 = new DataTable();
            List<ProInput> TData = new List<ProInput>();
            ProInput tda = new ProInput();
            dt2 = IProductionEntry.GetInputDeatils(id);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new ProInput();
                    tda.Itemlst = BindItemlst();
                   
                    tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                    
                    tda.Item = dt2.Rows[i]["item"].ToString();
                    tda.lotlist = BindLotNo(tda.Item, ca.LOCID, ca.BranchId);
                    tda.Time = dt2.Rows[i]["CHARGINGTIME"].ToString();
                    tda.BinId = dt2.Rows[i]["BINID"].ToString();
                    tda.batchno = dt2.Rows[i]["BATCHNO"].ToString();
                    tda.IssueQty = Convert.ToDouble(dt2.Rows[i]["QTY"].ToString() == "" ? "0" : dt2.Rows[i]["QTY"].ToString());
                    tda.StockAvailable = Convert.ToDouble(dt2.Rows[i]["STOCK"].ToString() == "" ? "0" : dt2.Rows[i]["STOCK"].ToString());
                    tda.inpid = dt2.Rows[i]["APPRODINPDETID"].ToString();
                    tda.APID = id;
                    tda.Isvalid = "Y";
                    TData.Add(tda);

                }

            }
            DataTable dt3 = new DataTable();
            List<APProInCons> TData1 = new List<APProInCons>();
            APProInCons tda1 = new APProInCons();
            dt3 = IProductionEntry.GetConsDeatils(id);
            if (dt3.Rows.Count > 0)
            {
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    tda1 = new APProInCons();
                   // tda1.Itemlst = BindItemlstCon();
                    tda1.ItemId = dt3.Rows[i]["ITEMID"].ToString();
                    tda1.saveitemId = dt3.Rows[i]["item"].ToString();
                    tda1.consunit = dt3.Rows[i]["UNITID"].ToString();
                    tda1.consid = dt3.Rows[i]["APPRODCONSDETID"].ToString();
                    tda1.BinId = dt3.Rows[i]["BINID"].ToString();
                    tda1.Qty = Convert.ToDouble(dt3.Rows[i]["QTY"].ToString() == "" ? "0" : dt3.Rows[i]["QTY"].ToString());
                    tda1.consQty = Convert.ToDouble(dt3.Rows[i]["CONSQTY"].ToString() == "" ? "0" : dt3.Rows[i]["CONSQTY"].ToString());
                    tda1.ConsStock = Convert.ToDouble(dt3.Rows[i]["STOCK"].ToString() == "" ? "0" : dt3.Rows[i]["STOCK"].ToString());

                    tda1.APID = id;
                    tda1.Isvalid = "Y";
                    TData1.Add(tda1);
                }

            }

           
           
            DataTable dt6 = new DataTable();
            List<ProOutput> TData4 = new List<ProOutput>();
            ProOutput tda4 = new ProOutput();
            dt6 = IProductionEntry.GetOutputDeatils(id);
            if (dt6.Rows.Count > 0)
            {
                for (int i = 0; i < dt6.Rows.Count; i++)
                {
                    tda4 = new ProOutput();
                    tda4.Itemlst = BindOutItemlst();
                    tda4.ItemId = dt6.Rows[i]["ITEMID"].ToString();
                    tda4.saveitemId = dt6.Rows[i]["item"].ToString();
                    tda4.BinId = dt6.Rows[i]["BINID"].ToString();
                    //tda4.drumlst = BindDrum();
                    
                    tda4.drumno = dt6.Rows[i]["DRUMNO"].ToString();
                    tda4.drumid = dt6.Rows[i]["drum"].ToString();
                    tda4.FromTime = dt6.Rows[i]["FROMTIME"].ToString();
                    tda4.ToTime = dt6.Rows[i]["TOTIME"].ToString();
                    tda4.statuslst = BindStatus();
                    tda4.Status = dt6.Rows[i]["STATUS"].ToString();
                    tda4.OutputQty = Convert.ToDouble(dt6.Rows[i]["OUTQTY"].ToString() == "" ? "0" : dt6.Rows[i]["OUTQTY"].ToString());
                    tda4.ExcessQty= Convert.ToDouble(dt6.Rows[i]["EXQTY"].ToString() == "" ? "0" : dt6.Rows[i]["EXQTY"].ToString());
                    DataTable dt7 = new DataTable();
                    dt7 = IProductionEntry.GetResult(id);
                    if (dt7.Rows.Count > 0)
                    {
                        tda4.Result = dt7.Rows[i]["TESTRESULT"].ToString();
                        //tda4.Status = dt7.Rows[i]["MOVETOQC"].ToString();
                    }
                    tda4.APID = id;
                    tda4.outid= dt6.Rows[i]["APPRODOUTDETID"].ToString();
                    tda4.Isvalid = "Y";
                    TData4.Add(tda4);

                }

            }
           
            ca.inplst = TData;
            ca.outlst = TData4;
            ca.Binconslst = TData1;
            return View(ca);

        }
        public List<SelectListItem> BindLotNo(string item,string loc,string branch)
        {
            string locid = datatrans.GetDataString("Select ILOCATION from WCBASIC where WCBASICID='" + loc + "' ");
            try
            {
                DataTable dtDesg = IProductionEntry.GetLotNo(item, locid, branch);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOT_NO"].ToString(), Value = dtDesg.Rows[i]["LOT_NO"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult ViewAPProductionentry(string id)
        {
            APProductionentryDet ca = new APProductionentryDet();
            DataTable dt = new DataTable();
            dt = IProductionEntry.GetAPProductionentryName(id);
            if (dt.Rows.Count > 0)
            {
                ca.Location = dt.Rows[0]["WCID"].ToString();
                ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                ca.DocId = dt.Rows[0]["DOCID"].ToString();
                ca.Eng = dt.Rows[0]["ENTEREDBY"].ToString();
                ca.Shift = dt.Rows[0]["SHIFT"].ToString();
                ViewBag.shift = dt.Rows[0]["SHIFT"].ToString();
                ca.SchQty = dt.Rows[0]["SCHQTY"].ToString();
                ca.LOCID = dt.Rows[0]["ILOCDETAILSID"].ToString();
                ca.BranchId = dt.Rows[0]["BRANCH"].ToString();
                ca.Process = dt.Rows[0]["PROCESSID"].ToString();
                ca.Sheduleno = dt.Rows[0]["psno"].ToString();
                ca.ProdQty = dt.Rows[0]["PRODQTY"].ToString();
                //ca.ExRate = dt.Rows[0]["EXCRATERATE"].ToString();
                ca.BatchNo = dt.Rows[0]["BATCH"].ToString();
                ca.batchid = datatrans.GetDataString("SELECT BCPRODBASICID FROM BCPRODBASIC WHERE DOCID='" + ca.BatchNo + "'");
                //ca.batchid = dt.Rows[0]["batchid"].ToString();
                ca.batchcomplete = dt.Rows[0]["BATCHCOMP"].ToString();
                ca.APID = id;
            }
            DataTable dt2 = new DataTable();
            List<ProInput> TData = new List<ProInput>();
            ProInput tda = new ProInput();
            dt2 = IProductionEntry.GetInputDeatils(id);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new ProInput();
                    tda.Itemlst = BindItemlst();
                    tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                    tda.unit = dt2.Rows[i]["UNITID"].ToString();
                    tda.Time = dt2.Rows[i]["CHARGINGTIME"].ToString();
                    tda.BinId = dt2.Rows[i]["BINID"].ToString();
                    tda.batchno = dt2.Rows[i]["IBATCHNO"].ToString();
                    tda.drumno = dt2.Rows[i]["DRUMNO"].ToString();
                    tda.IssueQty = Convert.ToDouble(dt2.Rows[i]["IQTY"].ToString() == "" ? "0" : dt2.Rows[i]["IQTY"].ToString());
                  
                    tda.APID = id;
                    tda.Isvalid = "Y";
                    TData.Add(tda);

                }

            }
            DataTable dt3 = new DataTable();
            List<APProInCons> TData1 = new List<APProInCons>();
            APProInCons tda1 = new APProInCons();
            dt3 = IProductionEntry.GetConsDeatils(id);
            if (dt3.Rows.Count > 0)
            {
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    tda1 = new APProInCons();
                    //tda1.Itemlst = BindItemlstCon();
                    tda1.ItemId = dt3.Rows[i]["ITEMID"].ToString();
                    tda1.consunit = dt3.Rows[i]["CUNIT"].ToString();
                     tda1.Qty = Convert.ToDouble(dt3.Rows[i]["QTY"].ToString() == "" ? "0" : dt3.Rows[i]["QTY"].ToString());
                    tda1.consQty = Convert.ToDouble(dt3.Rows[i]["CONSQTY"].ToString() == "" ? "0" : dt3.Rows[i]["CONSQTY"].ToString());
                    tda1.ConsStock = Convert.ToDouble(dt3.Rows[i]["STOCK"].ToString() == "" ? "0" : dt3.Rows[i]["STOCK"].ToString());

                    tda1.APID = id;
                    tda1.Isvalid = "Y";
                    TData1.Add(tda1);
                }

            }

            DataTable dt4 = new DataTable();
            List<EmpDetails> TTData2 = new List<EmpDetails>();
            EmpDetails tda2 = new EmpDetails();
            dt4 = IProductionEntry.GetEmpdetDeatils(id);
            if (dt4.Rows.Count > 0)
            {
                for (int i = 0; i < dt4.Rows.Count; i++)
                {
                    tda2 = new EmpDetails();
                    tda2.Employeelst = BindEmp();
                    tda2.Employee = dt4.Rows[i]["EMPID"].ToString();

                    tda2.EmpCode = dt4.Rows[i]["EMPCODE"].ToString();
                    tda2.Depart = dt4.Rows[i]["DEPARTMENT"].ToString();
                    tda2.StartDate = dt4.Rows[i]["STARTDATE"].ToString();
                    tda2.StartTime = dt4.Rows[i]["STARTTIME"].ToString();
                    tda2.EndDate = dt4.Rows[i]["ENDDATE"].ToString();
                    tda2.EndTime = dt4.Rows[i]["ENDTIME"].ToString();
                    tda2.OTHrs = dt4.Rows[i]["OTHOUR"].ToString();

                    tda2.ETOther = dt4.Rows[i]["ETOTHER"].ToString();
                    tda2.Normal = dt4.Rows[i]["NHOUR"].ToString();
                    tda2.NOW = dt4.Rows[i]["NATUREOFWORK"].ToString();
                    tda2.ID = id;
                    tda2.Isvalid = "Y";
                    TTData2.Add(tda2);

                }

            }
            DataTable dt5 = new DataTable();
            List<BreakDet> TData3 = new List<BreakDet>();
            BreakDet tda3 = new BreakDet();
            dt5 = IProductionEntry.GetBreakDeatils(id);
            if (dt5.Rows.Count > 0)
            {
                for (int i = 0; i < dt5.Rows.Count; i++)
                {
                    tda3 = new BreakDet();
                    tda3.Machinelst = BindMachineID();
                    tda3.MachineId = dt5.Rows[i]["MCODE"].ToString();
                    tda3.Emplst = BindEmp();
                    tda3.MachineDes = dt5.Rows[i]["DESCRIPTION"].ToString();
                    tda3.StartTime = dt5.Rows[i]["FROMTIME"].ToString();
                    tda3.EndTime = dt5.Rows[i]["TOTIME"].ToString();
                    tda3.PB = dt5.Rows[i]["PB"].ToString();
                    tda3.Isvalid = "Y";
                    tda3.Alloted = dt5.Rows[i]["EMPNAME"].ToString();
                    tda3.DType = dt5.Rows[i]["DTYPE"].ToString();
                    tda3.MType = dt5.Rows[i]["MTYPE"].ToString();
                    tda3.Reason = dt5.Rows[i]["REASON"].ToString();

                    tda3.APID = id;
                    TData3.Add(tda3);
                }

            }
            DataTable dt6 = new DataTable();
            List<ProOutput> TData4 = new List<ProOutput>();
            ProOutput tda4 = new ProOutput();
            dt6 = IProductionEntry.GetOutputDeatils(id);
            if (dt6.Rows.Count > 0)
            {
                for (int i = 0; i < dt6.Rows.Count; i++)
                {
                    tda4 = new ProOutput();
                    tda4.Itemlst = BindOutItemlst();
                    tda4.ItemId = dt6.Rows[i]["ITEMID"].ToString();
                    tda4.BinId = dt6.Rows[i]["BINID"].ToString();
                    tda4.drumlst = BindDrum();
                    tda4.statuslst = BindStatus();
                    tda4.drumno = dt6.Rows[i]["DRUMNO"].ToString();
                    tda4.FromTime = dt6.Rows[i]["FROMTIME"].ToString();
                    tda4.ToTime = dt6.Rows[i]["TOTIME"].ToString();
                    tda4.OutputQty = Convert.ToDouble(dt6.Rows[i]["OUTQTY"].ToString() == "" ? "0" : dt6.Rows[i]["OUTQTY"].ToString());
                    DataTable dt7 = new DataTable();
                    dt7 = IProductionEntry.GetResult(id);
                    if (dt7.Rows.Count > 0)
                    {
                        tda4.Result = dt7.Rows[i]["TESTRESULT"].ToString();
                        tda4.Status = dt7.Rows[i]["MOVETOQC"].ToString();
                    }
                    tda4.APID = id;
                    tda4.Isvalid = "Y";
                    TData4.Add(tda4);

                }

            }
            DataTable adt7 = new DataTable();
            List<LogDetails> TTData5 = new List<LogDetails>();
            LogDetails tda5 = new LogDetails();
            adt7 = IProductionEntry.GetLogdetailDeatils(id);
            if (adt7.Rows.Count > 0)
            {
                for (int i = 0; i < adt7.Rows.Count; i++)
                {
                    tda5 = new LogDetails();

                    tda5.StartDate = adt7.Rows[i]["STARTDATE"].ToString();
                    tda5.StartTime = adt7.Rows[i]["STARTTIME"].ToString();

                    tda5.EndDate = adt7.Rows[i]["ENDDATE"].ToString();

                    tda5.Reason = adt7.Rows[i]["REASON"].ToString();



                    tda5.EndTime = adt7.Rows[i]["ENDTIME"].ToString();
                    tda5.tothrs = adt7.Rows[i]["TOTALHRS"].ToString();

                    tda5.APID = id;
                    TTData5.Add(tda5);
                    tda5.Isvalid = "Y";
                }

            }
            ca.BreakLst = TData3;
            ca.inplst = TData;
            ca.outlst = TData4;
            ca.EmplLst = TTData2;
            ca.Binconslst = TData1;
            ca.LogLst = TTData5;
            return View(ca);

        }
        public List<SelectListItem> BindMachineID()
		{
			try
			{
				DataTable dtDesg = datatrans.GetMachine();
				List<SelectListItem> lstdesg = new List<SelectListItem>();
				for (int i = 0; i < dtDesg.Rows.Count; i++)
				{
					lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["MCODE"].ToString(), Value = dtDesg.Rows[i]["MACHINEINFOBASICID"].ToString()});
				}
				return lstdesg;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public List<SelectListItem> BindShift()
		{
			try
			{
				DataTable dtDesg = IProductionEntry.ShiftDeatils();
				List<SelectListItem> lstdesg = new List<SelectListItem>();
				for (int i = 0; i < dtDesg.Rows.Count; i++)
				{
					lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SHIFTNO"].ToString(), Value = dtDesg.Rows[i]["SHIFTNO"].ToString() });
				}
				return lstdesg;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public List<SelectListItem> BindItemlst()
		{
			try
			{
				DataTable dtDesg = IProductionEntry.GetItem();
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
        public List<SelectListItem> BindBatchItemlst(string value)
        {
            try
            {
                DataTable dtDesg = IProductionEntry.GetBatchItem(value);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["IITEMID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindBatchOutItemlst(string value)
        {
            try
            {
                DataTable dtDesg = IProductionEntry.GetBatchOutItem(value);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["OITEMID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindOutItemlst()
        {
            try
            {
                DataTable dtDesg = IProductionEntry.GetOutItem();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["ITEMMASTERID"].ToString()});
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindItemlstCon(string value)
		{
			try
			{
				DataTable dtDesg = IProductionEntry.GetItemCon(value);
				List<SelectListItem> lstdesg = new List<SelectListItem>();
				for (int i = 0; i < dtDesg.Rows.Count; i++)
				{
					lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["item"].ToString()});
				}
				return lstdesg;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
        public List<SelectListItem> BindDrum()
        {
            try
            {
                DataTable dtDesg = IProductionEntry.GetDrum();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DRUMNO"].ToString(), Value = dtDesg.Rows[i]["DRUMMASTID"].ToString()});
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetshiftDetail(string Shiftid)
		{
			try
			{
				DataTable dt = new DataTable();
				string fromtime = "";
				string totime = "";
				string tothrs = "";
				dt = datatrans.GetData("Select FROMTIME,TOTIME,SHIFTHRS from SHIFTMAST where SHIFTNO='" + Shiftid + "'");
				if (dt.Rows.Count > 0)
				{

					fromtime = dt.Rows[0]["FROMTIME"].ToString();
					totime = dt.Rows[0]["TOTIME"].ToString();
					tothrs = dt.Rows[0]["SHIFTHRS"].ToString();
				}

				var result = new { fromtime = fromtime, totime = totime, tothrs = tothrs };
				return Json(result);
			}
			catch (Exception ex)
			{
				throw ex;
            }
        }
        public ActionResult InsertProInput([FromBody] ProInput[] model)
        {
            try
			{
                foreach (ProInput input in model)
                {
                   
                    string item = input.ItemId;
                    string bin = input.BinId;
                    string batch = input.batchno;
                    string time = input.Time;
                    string id = input.APID;
                    string stock = input.StockAvailable.ToString();
                    string qty = input.IssueQty.ToString();
                    string unit = input.unit.ToString();
                    string drum = input.drumno.ToString();
                    DataTable dt = new DataTable();
                    
                    dt = IProductionEntry.SaveInputDetails(id, item, bin, time, qty, stock, batch, drum);
                }
                    if (model != null)
                    {

                        return Json("Success");
                    }
                    else
                    {
                        return Json("An Error Has occoured");
                    }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult InsertConsInput([FromBody] APProInCons[] model)
        {
            try
            {
                int l = 1;
                foreach (APProInCons Cons in model)
                {

                    string item = Cons.ItemId;
                    string bin = Cons.BinId;
                    string unit = Cons.consunit;
                    string qty = Cons.consQty.ToString(); 
                    string id = Cons.APID;
                    string stock = Cons.ConsStock.ToString();
                    string usedqty = Cons.Qty.ToString();

                    DataTable wopro = datatrans.GetData("SELECT WCBASIC.WCID,PROCESSMAST.PROCESSID FROM BPRODBASIC LEFT OUTER JOIN WCBASIC ON WCBASICID=BPRODBASIC.WCID LEFT OUTER JOIN PROCESSMAST ON PROCESSMASTID=BPRODBASIC.PROCESSID");
                    string work = wopro.Rows[0]["WCID"].ToString();
                    string process = wopro.Rows[0]["PROCESSID"].ToString();
                    DataTable dt = new DataTable();

                    dt = IProductionEntry.SaveConsDetails(id, item, unit,qty, usedqty, work, process,l);
                    l++;
                }

                if (model != null)
                {
                    return Json("Success");
                }
                else
                {
                    return Json("An Error Has occoured");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult InsertProEmp([FromBody] EmpDetails[] model)
        {
            try
            {
                foreach (EmpDetails emp in model)
                {

                    string empname = emp.Employee;
                    string code = emp.EmpCode;
                    string depat = emp.Depart;
                    string sdate = emp.StartDate ;
                    string id = emp.APID;
                    string stime = emp.StartTime ;
                    string edate = emp.EndDate ;
                    string etime = emp.EndTime;
                    string ot = emp.OTHrs;
                    string et = emp.ETOther;
                    string normal = emp.Normal;
                    string now = emp.NOW;
                    DataTable dt = new DataTable();

                    dt = IProductionEntry.SaveEmpDetails(id,empname, code, depat, sdate, stime, edate, etime, ot, et, normal, now);
                }

                if (model != null)
                {
                    return Json("Success");
                }
                else
                {
                    return Json("An Error Has occoured");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult InsertProOutsource([FromBody] SourceDetail[] model)
        {
            try
            {
                foreach (SourceDetail outs in model)
                {

                    string noofemp = outs.NoOfEmp;
                    string sdate = outs.StartDate;
                    string stime = outs.StartTime;
                    string edate = outs.StartDate;
                    string id = outs.APID;
                
                    
                    string etime = outs.EndTime;
                    string workhrs = outs.WorkHrs.ToString();
                    string cost = outs.EmpCost.ToString();
                    string expence = outs.Expence.ToString();
                    string now = outs.NOW;
                    DataTable dt = new DataTable();

                    dt = IProductionEntry.SaveOutsDetails(id, noofemp, sdate, stime, edate, etime, workhrs, cost, expence, now);
                }

                if (model != null)
                {
                    return Json("Success");
                }
                else
                {
                    return Json("An Error Has occoured");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult InsertProLog([FromBody] LogDetails[] model)
        {
            try
            {
                foreach (LogDetails log in model)
                {

                  
                    string sdate = log.StartDate;
                    string id = log.APID;
                    string stime = log.StartTime;
                    string edate = log.EndDate;
                    string etime = log.EndTime;
                    string tot = log.tothrs;
                    string reason = log.Reason;
                     
                    DataTable dt = new DataTable();

                    dt = IProductionEntry.SaveLogDetails(id, sdate, stime, edate, etime, tot, reason);
                }

                if (model != null)
                {
                    return Json("Success");
                }
                else
                {
                    return Json("An Error Has occoured");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult InsertProBreak([FromBody] BreakDet[] model)
        {
            try
            {
                foreach (BreakDet det in model)
                {

                    string machine = det.MachineId;
                    string des = det.MachineDes;
                    string dtype = det.DType;
                    string mtype = det.MType;
                    string id = det.APID;
                    string stime = det.StartTime;
                    string pb = det.PB;
                    string etime = det.EndTime;
                    string reason = det.Reason;
                    string all = det.Alloted;
                    
                    DataTable dt = new DataTable();

                    dt = IProductionEntry.SaveBreakDetails(id, machine, des, dtype, mtype, stime, etime, pb, all, reason);
                }

                if (model != null)
                {
                    return Json("Success");
                }
                else
                {
                    return Json("An Error Has occoured");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetMachineDetail(string ItemId)
		{
			try
			{
				DataTable dt = new DataTable();

				string name = "";
				string type = "";

				dt = IProductionEntry.GetMachineDetails(ItemId);

				if (dt.Rows.Count > 0)
				{

					name = dt.Rows[0]["MNAME"].ToString();
					type = dt.Rows[0]["MTYPE"].ToString();
				}
				var result = new { name = name, type = type };
				return Json(result);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public IActionResult ListAPProductionentry()
		{
			//IEnumerable<APProductionentry> cmp = IProductionEntry.GetAllAPProductionentry();
			return View();
		}
        public ActionResult MyListAPProductionentryGrid()
        {
            List<APProductionentryItems> Reg = new List<APProductionentryItems>();
            DataTable dtUsers = new DataTable();
            //strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)IProductionEntry.GetAllAPProductionentryItems();
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string View = string.Empty;
                string Print = string.Empty;
                string APPrint1 = string.Empty;
                string Approve = string.Empty;
                string EditRow = string.Empty;
                string DeleteRow = string.Empty;

                View = "<a href=ViewAPProductionentry?id=" + dtUsers.Rows[i]["BPRODBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View Details' width='20' /></a>";
                Print = "<a href=Print?id=" + dtUsers.Rows[i]["DOCID"].ToString() + " target='_blank'><img src='../Images/pdf.png' alt='Generate PO' width='20' /></a>";
                APPrint1 = "<a href=APPrint1?id=" + dtUsers.Rows[i]["BPRODBASICID"].ToString() + " target='_blank'><img src='../Images/pdf.png' alt='Generate PO' width='20' /></a>";
                if (dtUsers.Rows[i]["IS_APPROVE"].ToString() == "N")
                {
                    if (dtUsers.Rows[i]["IS_COMPLETE"].ToString() == "No")
                    {
                        Approve = "";
                        EditRow = "";

                    }
                    else
                    {
                      
                        Approve = "<a href=ApproveAPProductionentry?id=" + dtUsers.Rows[i]["BPRODBASICID"].ToString() + "><img src='../Images/checklist.png' alt='Approve' /></a>";
                        EditRow = "<a href=APProductionentryDetail?id=" + dtUsers.Rows[i]["BPRODBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";

                    }
                }
                else
                {

                }
                DeleteRow = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["BPRODBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                Reg.Add(new APProductionentryItems
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["BPRODBASICID"].ToString()),
                    docNo = dtUsers.Rows[i]["DOCID"].ToString(),
                    docDate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    workcenter = dtUsers.Rows[i]["WCID"].ToString(),
                    batch = dtUsers.Rows[i]["BATCH"].ToString(),
                    psno = dtUsers.Rows[i]["psno"].ToString(),
                    process = dtUsers.Rows[i]["PROCESSID"].ToString(),
                    shi = dtUsers.Rows[i]["SHIFT"].ToString(),
                    view = View,
                    print = Print,
                    apprint = APPrint1,
                    approve = Approve,
                    editrow = EditRow,
                    delrow = DeleteRow,
                });
            }

            return Json(new
            {
                Reg
            });

        }

        public ActionResult APProductionentryDetail(string id,string tag)
		{
			APProductionentryDet ca = new APProductionentryDet();
			//ca.Complete = "No";
            APProductionentry cy = new APProductionentry();
            List<BreakDet> TData3 = new List<BreakDet>();
            BreakDet tda3 = new BreakDet();
			List<ProInput> TData = new List<ProInput>();
			ProInput tda = new ProInput();
            List<ProOutput> TData4 = new List<ProOutput>();
            ProOutput tda4 = new ProOutput();
            List<APProInCons> TData1 = new List<APProInCons>();
			APProInCons tda1 = new APProInCons();
			List<EmpDetails> TTData2 = new List<EmpDetails>();
			EmpDetails tda2 = new EmpDetails();
            List<SourceDetail> TTData6 = new List<SourceDetail>();
            SourceDetail tda6 = new SourceDetail();
            List<LogDetails> TTData5 = new List<LogDetails>();
            LogDetails tda5 = new LogDetails();
            if (tag == "2")
			{
		
                if (!string.IsNullOrEmpty(id))
                {


                    DataTable dt = new DataTable();

                    dt = IProductionEntry.GetAPProd(id);
                    if (dt.Rows.Count > 0)
                    {
                        ca.Location = dt.Rows[0]["WCID"].ToString();
                        ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                        ca.DocId = dt.Rows[0]["DOCID"].ToString();
                        ca.Eng = dt.Rows[0]["ENTEREDBY"].ToString();
                        ca.Shift = dt.Rows[0]["SHIFT"].ToString();
                        ViewBag.shift = dt.Rows[0]["SHIFT"].ToString();
                        ca.SchQty = dt.Rows[0]["SCHQTY"].ToString();
                        ca.LOCID = dt.Rows[0]["ILOCDETAILSID"].ToString();
                        ca.BranchId = dt.Rows[0]["BRANCH"].ToString();
                        ca.Process = dt.Rows[0]["PROCESSID"].ToString();
                        ca.Sheduleno = dt.Rows[0]["psno"].ToString();
                        ca.ProdQty = dt.Rows[0]["PRODQTY"].ToString();
                        //ca.ExRate = dt.Rows[0]["EXCRATERATE"].ToString();
                        ca.BatchNo = dt.Rows[0]["BATCH"].ToString();
                        ca.batchid = datatrans.GetDataString("SELECT BCPRODBASICID FROM BCPRODBASIC WHERE DOCID='" + ca.BatchNo + "'");
                       //ca.batchid = dt.Rows[0]["batchid"].ToString();
                        ca.batchcomplete = dt.Rows[0]["BATCHCOMP"].ToString();
                        ca.APID = id;
                    }
                 
                        for (int i = 0; i < 3; i++)
                        {
                            tda = new ProInput();
                            tda.APID = id;
                            tda.Itemlst = BindBatchItemlst(ca.batchid);
                        tda.drumlst = BindDrum();
                        tda.Isvalid = "Y";
                            TData.Add(tda);

                        }
 
                        for (int i = 0; i < 1; i++)
                        {
                            tda4 = new ProOutput();
                            tda4.APID = id;
                            tda4.Itemlst = BindBatchOutItemlst(ca.batchid);
                            tda4.drumlst = BindDrum();
                            tda4.statuslst = BindStatus();
                            tda4.Isvalid = "Y";
                            TData4.Add(tda4);

                        }

                    for (int i = 0; i < 3; i++)
                    {
                        tda3 = new BreakDet();

                        tda3.Machinelst = BindMachineID();
                        tda3.Emplst = BindEmp();
                        tda3.Reasonlst = BindReason();
                        tda3.Isvalid = "Y";
                        tda3.APID = id;
                        TData3.Add(tda3);

                    }


                    for (int i = 0; i < 1; i++)
                    {
                        tda1 = new APProInCons();
                        tda1.Itemlst = BindItemlstCon(ca.LOCID);
                        tda1.Isvalid = "Y";
                        tda1.APID = id;
                        TData1.Add(tda1);
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        tda2 = new EmpDetails();
                        tda2.APID = id;
                        tda2.Employeelst = BindEmp();
                        tda2.Isvalid = "Y";
                        TTData2.Add(tda2);
                    }
                    for (int i = 0; i < 1; i++)
                    {
                        tda6 = new SourceDetail();
                        tda6.APID = id;
                        int ShiftTime = datatrans.GetDataId("Select SHIFTHRS from SHIFTMAST where shiftno='" + ca.Shift + "' ");
                        tda6.StartDate = DateTime.Now.ToString("dd-MMM-yyyy  HH:mm:ss");
                        tda6.StartTime = DateTime.Now.ToString("HH:mm");
                        DateTime dateTime = DateTime.Parse(tda6.StartDate);
                        //TimeSpan t1 = new TimeSpan(24,0,0);


                        //int hours = int.Parse(ShiftTime);
                        TimeSpan t2 = new TimeSpan(ShiftTime, 0, 0);
                        DateTime resultDateTime = dateTime + t2;
                        tda6.EndDate = resultDateTime.ToString("dd-MMM-yyyy - HH:mm");

                        string[] sdateList = tda6.StartDate.Split(" ");
                        string sdate = "";
                        string stime = "";
                        if (sdateList.Length > 0)
                        {
                            sdate = sdateList[0];
                            stime = sdateList[1];
                        }
                        string[] edateList = tda6.EndDate.Split(" - ");
                        string endate = "";
                        string endtime = "";
                        if (sdateList.Length > 0)
                        {
                            endate = edateList[0];
                            endtime = edateList[1];
                        }
                        tda6.StartDate = sdate;
                        tda6.EndDate = endate;


                        tda6.EndTime = endtime;

                        tda6.Isvalid = "Y";
                        TTData6.Add(tda6);
                    }
                    for (int i = 0; i < 1; i++)
                    {
                        tda5 = new LogDetails();
                        tda5.APID = id;
                        string ShiftTime = datatrans.GetDataString("Select SHIFTHRS from SHIFTMAST where shiftno='" + ca.Shift + "' ");
                        tda5.StartDate = DateTime.Now.ToString("dd-MMM-yyyy  HH:mm:ss");
                        tda5.StartTime = DateTime.Now.ToString("HH:mm");
                        DateTime dateTime = DateTime.Parse(tda5.StartDate);
                        //TimeSpan t1 = new TimeSpan(24,0,0);


                        //int hours = int.Parse(ShiftTime);
                        TimeSpan t2 = new TimeSpan(8, 0, 0);
                        DateTime resultDateTime = dateTime + t2;
                        tda5.EndDate = resultDateTime.ToString("dd-MMM-yyyy - HH:mm");

                        string[] sdateList = tda5.StartDate.Split(" ");
                        string sdate = "";
                        string stime = "";
                        if (sdateList.Length > 0)
                        {
                            sdate = sdateList[0];
                            stime = sdateList[1];
                        }
                        string[] edateList = tda5.EndDate.Split(" - ");
                        string endate = "";
                        string endtime = "";
                        if (sdateList.Length > 0)
                        {
                            endate = edateList[0];
                            endtime = edateList[1];
                        }
                        tda5.StartDate = sdate;
                        tda5.EndDate = endate;
                        tda5.reasonlst = BindReason();

                        tda5.EndTime = endtime;

                        tda5.Isvalid = "Y";
                        TTData5.Add(tda5);

                    }
                }
            }
            if (tag == null)
            {
                if (!string.IsNullOrEmpty(id))
                {


                    DataTable dt = new DataTable();

                    dt = IProductionEntry.GetAPProd(id);
                    if (dt.Rows.Count > 0)
                    {
                        ca.Location = dt.Rows[0]["WCID"].ToString();
                        ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                        ca.DocId = dt.Rows[0]["DOCID"].ToString();
                        ca.Eng = dt.Rows[0]["ENTEREDBY"].ToString();
                        ca.Shift = dt.Rows[0]["SHIFT"].ToString();
                        ViewBag.shift = dt.Rows[0]["SHIFT"].ToString();
                        ca.SchQty = dt.Rows[0]["SCHQTY"].ToString();
                        ca.LOCID = dt.Rows[0]["ILOCDETAILSID"].ToString();
                        ca.BranchId = dt.Rows[0]["BRANCH"].ToString();
                        ca.Process = dt.Rows[0]["PROCESSID"].ToString();
                        ca.Sheduleno = dt.Rows[0]["psno"].ToString();
                        ca.ProdQty = dt.Rows[0]["PRODQTY"].ToString();
                        //ca.ExRate = dt.Rows[0]["EXCRATERATE"].ToString();
                        ca.BatchNo = dt.Rows[0]["BATCH"].ToString();
                        ca.batchid = datatrans.GetDataString("SELECT BCPRODBASICID FROM BCPRODBASIC WHERE DOCID='" + ca.BatchNo + "'");
                        ca.batchcomplete = dt.Rows[0]["BATCHCOMP"].ToString();
                        ca.APID = id;
                    }
                    DataTable dt2 = new DataTable();
                    DataTable dtstk = new DataTable();
                    dt2 = IProductionEntry.GetInput(id);
                    if (dt2.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            tda = new ProInput();
                            tda.Itemlst = BindBatchItemlst(ca.batchid);
                            tda.ItemId = dt2.Rows[i]["IITEMID"].ToString();
                            tda.Time = dt2.Rows[i]["CHARGINGTIME"].ToString();
                            tda.drumlst = BindDrum();

                            tda.drumno = dt2.Rows[i]["IDRUMNO"].ToString();
                            tda.unit = dt2.Rows[i]["UNITID"].ToString();
                            tda.Bin = dt2.Rows[i]["IBINID"].ToString();
                            tda.BinId = dt2.Rows[i]["BINID"].ToString();
                            tda.batchno = dt2.Rows[i]["IBATCHNO"].ToString();
                            tda.IssueQty = Convert.ToDouble(dt2.Rows[i]["IQTY"].ToString() == "" ? "0" : dt2.Rows[i]["IQTY"].ToString());
                            dtstk = IProductionEntry.Getstkqty(tda.ItemId, ca.LOCID);
                            if (dtstk.Rows.Count > 0)
                            {
                                tda.StockAvailable = Convert.ToDouble(dtstk.Rows[0]["QTY"].ToString() == "" ? "0" : dtstk.Rows[0]["QTY"].ToString());
                            }
                            tda.APID = id;
                            tda.Isvalid = "Y";
                            TData.Add(tda);

                        }

                    }
                    DataTable dt3 = new DataTable();
                    dt3 = IProductionEntry.GetCons(id);
                    if (dt3.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt3.Rows.Count; i++)
                        {
                            tda1 = new APProInCons();
                            tda1.Itemlst = BindItemlstCon(ca.LOCID);
                            tda1.ItemId = dt3.Rows[i]["ITEMID"].ToString();
                            tda1.consunit = dt3.Rows[i]["UNIT"].ToString();
                            tda1.BinId = dt3.Rows[i]["BINID"].ToString();
                            tda1.Qty = Convert.ToDouble(dt3.Rows[i]["QTY"].ToString() == "" ? "0" : dt3.Rows[i]["QTY"].ToString());
                            tda1.consQty = Convert.ToDouble(dt3.Rows[i]["CONSQTY"].ToString() == "" ? "0" : dt3.Rows[i]["CONSQTY"].ToString());
                            tda1.ConsStock = Convert.ToDouble(dt3.Rows[i]["STOCK"].ToString() == "" ? "0" : dt3.Rows[i]["STOCK"].ToString());

                            tda1.APID = id;
                            tda1.Isvalid = "Y";
                            TData1.Add(tda1);
                        }

                    }

                    DataTable dt4 = new DataTable();
                    dt4 = IProductionEntry.GetEmpdet(id);
                    if (dt4.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt4.Rows.Count; i++)
                        {
                            tda2 = new EmpDetails();
                            tda2.Employeelst = BindEmp();
                            tda2.Employee = dt4.Rows[i]["EMPID"].ToString();

                            tda2.EmpCode = dt4.Rows[i]["EMPCODE"].ToString();
                            tda2.Depart = dt4.Rows[i]["DEPARTMENT"].ToString();
                            tda2.StartDate = dt4.Rows[i]["STARTDATE"].ToString();
                            tda2.StartTime = dt4.Rows[i]["STARTTIME"].ToString();
                            tda2.EndDate = dt4.Rows[i]["ENDDATE"].ToString();
                            tda2.EndTime = dt4.Rows[i]["ENDTIME"].ToString();
                            tda2.OTHrs = dt4.Rows[i]["OTHOUR"].ToString();

                            tda2.ETOther = dt4.Rows[i]["ETOTHER"].ToString();
                            tda2.Normal = dt4.Rows[i]["NHOUR"].ToString();
                            tda2.NOW = dt4.Rows[i]["NATUREOFWORK"].ToString();
                            tda2.ID = id;
                            tda2.Isvalid = "Y";
                            TTData2.Add(tda2);

                        }

                    }
                    DataTable dt5 = new DataTable();
                    dt5 = IProductionEntry.GetBreak(id);
                    if (dt5.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt5.Rows.Count; i++)
                        {
                            tda3 = new BreakDet();
                            tda3.Machinelst = BindMachineID();
                            tda3.MachineId = dt5.Rows[i]["MACHCODE"].ToString();
                            tda3.Emplst = BindEmp();
                            tda3.MachineDes = dt5.Rows[i]["DESCRIPTION"].ToString();
                            tda3.StartTime = dt5.Rows[i]["FROMTIME"].ToString();
                            tda3.EndTime = dt5.Rows[i]["TOTIME"].ToString();
                            tda3.PB = dt5.Rows[i]["PB"].ToString();
                            tda3.Isvalid = "Y";
                            tda3.Alloted = dt5.Rows[i]["ALLOTTEDTO"].ToString();
                            tda3.DType = dt5.Rows[i]["DTYPE"].ToString();
                            tda3.MType = dt5.Rows[i]["MTYPE"].ToString();
                            tda3.Reason = dt5.Rows[i]["REASON"].ToString();

                            tda3.APID = id;
                            TData3.Add(tda3);
                        }

                    }
                    DataTable dt6 = new DataTable();

                    dt6 = IProductionEntry.GetOutput(id);
                    if (dt6.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt6.Rows.Count; i++)
                        {
                            tda4 = new ProOutput();
                            tda4.Itemlst = BindBatchOutItemlst(ca.batchid);
                            tda4.ItemId = dt6.Rows[i]["OITEMID"].ToString();

                            tda4.drumlst = BindDrum();
                            tda4.statuslst = BindStatus();
                            tda4.StID = dt6.Rows[i]["STATUS"].ToString();
                            tda4.unit = dt6.Rows[i]["UNITID"].ToString();
                            tda4.ExcessQty = Convert.ToDouble(dt6.Rows[i]["OXQTY"].ToString() == "" ? "0" : dt6.Rows[i]["OXQTY"].ToString());
                            tda4.drumno = dt6.Rows[i]["ODRUMNO"].ToString();
                            tda4.FromTime = dt6.Rows[i]["STIME"].ToString();
                            tda4.ToTime = dt6.Rows[i]["ETIME"].ToString();
                            tda4.OutputQty = Convert.ToDouble(dt6.Rows[i]["OQTY"].ToString() == "" ? "0" : dt6.Rows[i]["OQTY"].ToString());
                            DataTable dt7 = new DataTable();
                            dt7 = IProductionEntry.GetResult(id);
                            if (dt7.Rows.Count > 0)
                            {
                                tda4.Result = dt7.Rows[i]["TESTRESULT"].ToString();
                                tda4.Status = dt7.Rows[i]["MOVETOQC"].ToString();
                            }
                            tda4.APID = id;
                            tda4.Isvalid = "Y";
                            TData4.Add(tda4);

                        }

                    }
                    DataTable adt7 = new DataTable();

                    adt7 = IProductionEntry.GetLogdetail(id);
                    if (adt7.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt6.Rows.Count; i++)
                        {
                            tda5 = new LogDetails();

                            tda5.StartDate = adt7.Rows[i]["STARTDATE"].ToString();
                            tda5.StartTime = adt7.Rows[i]["STARTTIME"].ToString();

                            tda5.EndDate = adt7.Rows[i]["ENDDATE"].ToString();

                            tda5.Reason = adt7.Rows[i]["REASON"].ToString();



                            tda5.EndTime = adt7.Rows[i]["ENDTIME"].ToString();
                            tda5.tothrs = adt7.Rows[i]["TOTALHRS"].ToString();

                            tda5.APID = id;
                            TTData5.Add(tda5);
                            tda5.Isvalid = "Y";
                        }

                    }
                }
            }
            if (tag=="1")
			{
              
                DataTable ap = datatrans.GetData("select BPRODBASICID,DOCID,DOCDATE,SHIFT from BPRODBASIC WHERE IS_COMPLETE='No'");
				if (ap.Rows.Count > 0)
				{
					string apID = datatrans.GetDataString("Select BPRODBASICID from BPRODBASIC where IS_COMPLETE='No' ");

                    DataTable adt = new DataTable();
                    DataTable dt6 = new DataTable();
                    adt = IProductionEntry.GetAPProd(apID);
                    if (adt.Rows.Count > 0)
                    {
                        ca.Location = adt.Rows[0]["WCID"].ToString();
                        ca.Docdate = adt.Rows[0]["DOCDATE"].ToString();
                        ca.DocId = adt.Rows[0]["DOCID"].ToString();
                        ca.Eng = adt.Rows[0]["ENTEREDBY"].ToString();
                        ca.Shift = adt.Rows[0]["SHIFT"].ToString();
                        ViewBag.shift = adt.Rows[0]["SHIFT"].ToString();
                        ca.SchQty = adt.Rows[0]["SCHQTY"].ToString();
                        ca.LOCID = adt.Rows[0]["ILOCDETAILSID"].ToString();
                        ca.BranchId = adt.Rows[0]["BRANCH"].ToString();
                        ca.Process = adt.Rows[0]["PROCESSID"].ToString();
                        ca.Sheduleno = adt.Rows[0]["psno"].ToString();
                        ca.ProdQty = adt.Rows[0]["PRODQTY"].ToString();
                        //ca.ExRate = dt.Rows[0]["EXCRATERATE"].ToString();
                        ca.BatchNo = adt.Rows[0]["BATCH"].ToString();
                        ca.batchid = datatrans.GetDataString("SELECT BCPRODBASICID FROM BCPRODBASIC WHERE DOCID='" + ca.BatchNo + "'");
                        ca.batchcomplete = adt.Rows[0]["BATCHCOMP"].ToString();
                         
                        ca.APID= apID;
                    }
                  
                    DataTable adt2 = new DataTable();
                    DataTable dtstk = new DataTable();
                    adt2 = IProductionEntry.GetInput(apID);
                    if (adt2.Rows.Count > 0)
                    {
                        for (int i = 0; i < adt2.Rows.Count; i++)
                        {
                            tda = new ProInput();
                            tda.Itemlst = BindBatchItemlst(ca.batchid);
                            tda.ItemId = adt2.Rows[i]["IITEMID"].ToString();
                            tda.BinId = adt2.Rows[i]["IBINID"].ToString();
                            tda.Time = adt2.Rows[i]["CHARGINGTIME"].ToString();
                            tda.batchno = adt2.Rows[i]["IBATCHNO"].ToString();
                            tda.drumlst = BindDrum();
                            tda.drumno = adt2.Rows[i]["IDRUMNO"].ToString();
                            tda.IssueQty = Convert.ToDouble(adt2.Rows[i]["IQTY"].ToString() == "" ? "0" : adt2.Rows[i]["IQTY"].ToString());
                            dtstk = IProductionEntry.Getstkqty(tda.ItemId, ca.LOCID );
                            if(dtstk.Rows.Count > 0)
                            {
                                tda.StockAvailable = Convert.ToDouble(dtstk.Rows[0]["QTY"].ToString() == "" ? "0" : dtstk.Rows[0]["QTY"].ToString());
                            }
                            //tda.StockAvailable = Convert.ToDouble(adt2.Rows[i]["STOCK"].ToString() == "" ? "0" : adt2.Rows[i]["STOCK"].ToString());
                            tda.APID = apID;
                            TData.Add(tda);
                            tda.Isvalid = "Y";
                        }

                    }
					else
					{
                        for (int i = 0; i < 1; i++)
                        {
                            tda = new ProInput();
                            tda.APID = apID;
                            tda.Itemlst = BindBatchItemlst(ca.batchid);
                            tda.drumlst = BindDrum();
                            tda.Isvalid = "Y";
                            TData.Add(tda);

                        }
                    }
                    DataTable adt3 = new DataTable();
                    adt3 = IProductionEntry.GetCons(apID);
                    if (adt3.Rows.Count > 0)
                    {
                        for (int i = 0; i < adt3.Rows.Count; i++)
                        {
                            tda1 = new APProInCons();
                            tda1.Itemlst = BindItemlstCon(ca.LOCID);
                            tda1.ItemId = adt3.Rows[i]["ITEMID"].ToString();
                            tda1.consunit = adt3.Rows[i]["UNIT"].ToString();
                            tda1.BinId = adt3.Rows[i]["BINID"].ToString();
                            tda1.Qty = Convert.ToDouble(adt3.Rows[i]["QTY"].ToString() == "" ? "0" : adt3.Rows[i]["QTY"].ToString());
                            tda1.consQty = Convert.ToDouble(adt3.Rows[i]["CONSQTY"].ToString() == "" ? "0" : adt3.Rows[i]["CONSQTY"].ToString());
                            tda1.ConsStock = Convert.ToDouble(adt3.Rows[i]["STOCK"].ToString() == "" ? "0" : adt3.Rows[i]["STOCK"].ToString());

                            tda1.APID = apID;
                            tda1.Isvalid = "Y";
                            TData1.Add(tda1);
                        }

                    }
					else
					{
                        for (int i = 0; i < 1; i++)
                        {
                            tda1 = new APProInCons();
                            tda1.Itemlst = BindItemlstCon(ca.LOCID);
                            tda1.Isvalid = "Y";
                            tda1.APID = apID;
                            TData1.Add(tda1);
                        }
                    }

                    DataTable adt4 = new DataTable();
                    adt4 = IProductionEntry.GetEmpdet(apID);
                    if (adt4.Rows.Count > 0)
                    {
                        for (int i = 0; i < adt4.Rows.Count; i++)
                        {
                            tda2 = new EmpDetails();
                            tda2.Employeelst = BindEmp();
                            tda2.Employee = adt4.Rows[i]["EMPID"].ToString();

                            tda2.EmpCode = adt4.Rows[i]["EMPCODE"].ToString();
                            tda2.Depart = adt4.Rows[i]["DEPARTMENT"].ToString();
                            tda2.StartDate = adt4.Rows[i]["STARTDATE"].ToString();
                            tda2.StartTime = adt4.Rows[i]["STARTTIME"].ToString();
                            tda2.EndDate = adt4.Rows[i]["ENDDATE"].ToString();
                            tda2.EndTime = adt4.Rows[i]["ENDTIME"].ToString();
                            tda2.OTHrs = adt4.Rows[i]["OTHOUR"].ToString();

                            tda2.ETOther = adt4.Rows[i]["ETOTHER"].ToString();
                            tda2.Normal = adt4.Rows[i]["NHOUR"].ToString();
                            tda2.NOW = adt4.Rows[i]["NATUREOFWORK"].ToString();
                            tda2.APID = apID;
                            TTData2.Add(tda2);
                            tda2.Isvalid = "Y";
                        }

                    }
					else
					{
                       
                        for (int i = 0; i < 1; i++)
                        {
                            tda2 = new EmpDetails();
                            tda2.APID = apID;
                            tda2.Employeelst = BindEmp();
                            tda2.Isvalid = "Y";
                            TTData2.Add(tda2);
                        }
                    }
                    DataTable adt5 = new DataTable();
                    adt5 = IProductionEntry.GetBreak(apID);
                    if (adt5.Rows.Count > 0)
                    {
                        for (int i = 0; i < adt5.Rows.Count; i++)
                        {
                            tda3 = new BreakDet();
                            tda3.Machinelst = BindMachineID();
                            tda3.MachineId = adt5.Rows[i]["MACHCODE"].ToString();
                            tda3.Emplst = BindEmp();
                            tda3.MachineDes = adt5.Rows[i]["DESCRIPTION"].ToString();
                            tda3.StartTime = adt5.Rows[i]["FROMTIME"].ToString();
                            tda3.EndTime = adt5.Rows[i]["TOTIME"].ToString();
                            tda3.PB = adt5.Rows[i]["PB"].ToString();
                            tda3.Isvalid = "Y";
                            tda3.Alloted = adt5.Rows[i]["ALLOTTEDTO"].ToString();
                            tda3.DType = adt5.Rows[i]["DTYPE"].ToString();
                            tda3.MType = adt5.Rows[i]["MTYPE"].ToString();
                            tda3.Reason = adt5.Rows[i]["REASON"].ToString();

                            tda3.APID = apID;
                            TData3.Add(tda3);
                        }

                    }
					else
					{
                        for (int i = 0; i < 1; i++)
                        {
                            tda3 = new BreakDet();

                            tda3.Machinelst = BindMachineID();
                            tda3.Emplst = BindEmp();
                            tda3.Reasonlst = BindReason();
                            tda3.Isvalid = "Y";
                            tda3.APID = apID;
                            TData3.Add(tda3);

                        }
                    }
                    DataTable adt6 = new DataTable();

                    adt6 = IProductionEntry.GetOutput(apID);
                    if (adt6.Rows.Count > 0)
                    {
                        for (int i = 0; i < adt6.Rows.Count; i++)
                        {
                            tda4 = new ProOutput();
                            tda4.Itemlst = BindOutItemlst();
                            tda4.ItemId = adt6.Rows[i]["ITEMID"].ToString();
                            tda4.saveitemId= adt6.Rows[i]["ITEMNAME"].ToString();
                            tda4.BinId = adt6.Rows[i]["BINID"].ToString();
                            tda4.drumlst = BindDrum();
                            tda4.statuslst=BindStatus();
                            tda4.StID = adt6.Rows[i]["STATUS"].ToString();
                            tda4.ExcessQty= Convert.ToDouble(adt6.Rows[i]["EXQTY"].ToString() == "" ? "0" : adt6.Rows[i]["EXQTY"].ToString());
                            tda4.drumno = adt6.Rows[i]["DRUMNO"].ToString();
                            tda4.FromTime = adt6.Rows[i]["FROMTIME"].ToString();
                            tda4.ToTime = adt6.Rows[i]["TOTIME"].ToString();
                            tda4.OutputQty = Convert.ToDouble(adt6.Rows[i]["OUTQTY"].ToString() == "" ? "0" : adt6.Rows[i]["OUTQTY"].ToString());
                            tda4.Result = adt6.Rows[i]["TESTRESULT"].ToString();
                            tda4.Status = adt6.Rows[i]["MOVETOQC"].ToString();
                           DataTable dt7 = new DataTable();
                            dt7 = IProductionEntry.GetResult(id);
                            if (dt7.Rows.Count > 0)
                            {
                                tda4.Result = dt7.Rows[i]["TESTRESULT"].ToString();
                                tda4.Status = dt7.Rows[i]["MOVETOQC"].ToString();
                            }
                            tda4.APID = adt6.Rows[i]["APPRODOUTDETID"].ToString();
                            tda4.Isvalid = "Y";
                            TData4.Add(tda4);
                           
                        }

                    }
					else
					{
                        for (int i = 0; i < 1; i++)
                        {
                            tda4 = new ProOutput();
                            tda4.APID = apID;
                            tda4.Itemlst = BindBatchOutItemlst(ca.batchid);
                            tda4.drumlst = BindDrum();
                            tda4.statuslst = BindStatus();
                            tda4.StID = "COMPLETED";
                            tda4.Result = "";
                            tda4.Isvalid = "Y";
                            TData4.Add(tda4);

                        }
                    }
                    DataTable adt7 = new DataTable();

                    adt7 = IProductionEntry.GetLogdetail(apID);
                    if (adt7.Rows.Count > 0)
                    {
                        for (int i = 0; i < adt7.Rows.Count; i++)
                        {
                            tda5 = new LogDetails();
                          
                            tda5.StartDate = adt7.Rows[i]["STARTDATE"].ToString();
                            tda5.StartTime = adt7.Rows[i]["STARTTIME"].ToString();
                           
                            tda5.EndDate = adt7.Rows[i]["ENDDATE"].ToString();

                            tda5.Reason = adt7.Rows[i]["REASON"].ToString();



                            tda5.EndTime = adt7.Rows[i]["ENDTIME"].ToString();
                                tda5.tothrs = adt7.Rows[i]["TOTALHRS"].ToString();
                         
                            tda5.APID = apID;
                            TTData5.Add(tda5);
                            tda5.Isvalid = "Y";
                        }

                    }
                    else
                    {
                        for (int i = 0; i < 1; i++)
                        {
                            tda5 = new LogDetails();
                            tda5.APID = apID;
                            string ShiftTime = datatrans.GetDataString("Select SHIFTHRS from SHIFTMAST where shiftno='" + ca.Shift + "' ");
                            tda5.StartDate = DateTime.Now.ToString("dd-MMM-yyyy  HH:mm:ss");
                            tda5.StartTime = DateTime.Now.ToString("HH:mm");
                            DateTime dateTime = DateTime.Parse(tda5.StartDate);
                            //TimeSpan t1 = new TimeSpan(24,0,0);
                             
                           
                            int hours = int.Parse(ShiftTime);
                            TimeSpan t2 = new TimeSpan(hours,0,0);
                            DateTime resultDateTime = dateTime + t2;
                            tda5.EndDate = resultDateTime.ToString("dd-MMM-yyyy - HH:mm");

                            string[] sdateList = tda5.StartDate.Split(" ");
                            string sdate = "";
                            string stime = "";
                            if (sdateList.Length > 0)
                            {
                                sdate = sdateList[0];
                                stime = sdateList[1];
                            }
                            string[] edateList = tda5.EndDate.Split(" - ");
                            string endate = "";
                            string endtime = "";
                            if (sdateList.Length > 0)
                            {
                                endate = edateList[0];
                                endtime = edateList[1];
                            }
                            tda5.StartDate = sdate;
                            tda5.EndDate = endate;
                            tda5.reasonlst = BindReason();
                            tda5.EndTime = endtime;
                             
                           tda5.Isvalid = "Y";
                            TTData5.Add(tda5);
                          
                        }
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        tda6 = new SourceDetail();
                        tda6.APID = id;

                        tda6.Isvalid = "Y";
                        TTData6.Add(tda6);
                    }
                }
				else
				{
                    return RedirectToAction("APProductionentry");
                }
            }
            if(tag=="4")
            {
                string apdoc = datatrans.GetDataString("Select DOCID from APPRODUCTIONBASIC where APPRODUCTIONBASIC.APPRODUCTIONBASICID='" + id + "'   ");
                string shift = datatrans.GetDataString("Select SHIFT from APPRODUCTIONBASIC where APPRODUCTIONBASIC.APPRODUCTIONBASICID='" + id + "'   ");
                string baid = datatrans.GetDataString("Select APPRODUCTIONBASICID from APPRODUCTIONBASIC where APPRODUCTIONBASIC.DOCID='" + apdoc + "' and SHIFT NOT IN '"+ shift+"'   ");

                

                DataTable ap = datatrans.GetData("select APPRODUCTIONBASICID,DRUMNO,ITEMID,APPRODOUTDETID,STATUS,OUTQTY,TESTRESULT,FROMTIME,TOTIME,STKQTY from APPRODOUTDET WHERE APPRODOUTDET.APPRODUCTIONBASICID='"+ baid + "' and APPRODOUTDET.STATUS='PENDING'");
                
                  

                    DataTable adt = new DataTable();
                    DataTable dt6 = new DataTable();
                    adt = IProductionEntry.GetAPProd(id);
                    if (adt.Rows.Count > 0)
                    {
                        ca.Location = adt.Rows[0]["WCID"].ToString();
                        ca.Docdate = adt.Rows[0]["DOCDATE"].ToString();
                        ca.DocId = adt.Rows[0]["DOCID"].ToString();
                        ca.Eng = adt.Rows[0]["EMPNAME"].ToString();
                        ca.Shift = adt.Rows[0]["SHIFT"].ToString();
                        ca.SchQty = adt.Rows[0]["SCHQTY"].ToString();
                        ViewBag.shift = adt.Rows[0]["SHIFT"].ToString();
                        ca.LOCID = adt.Rows[0]["ILOCATION"].ToString();
                        ca.BranchId = adt.Rows[0]["BRANCHID"].ToString();
                        //ca.ParNo = dt.Rows[0]["PARTYREFNO"].ToString();
                        ca.ProdQty = adt.Rows[0]["PRODQTY"].ToString();
                        //ca.ExRate = dt.Rows[0]["EXCRATERATE"].ToString();
                        ca.BatchNo = adt.Rows[0]["BATCH"].ToString();
                        ca.batchcomplete = adt.Rows[0]["BATCHYN"].ToString();
                        ca.APID = id;
                    }

                 
                        for (int i = 0; i < 1; i++)
                        {
                            tda = new ProInput();
                            tda.APID = id;
                            tda.Itemlst = BindItemlst();
                    tda.drumlst = BindDrum();
                    tda.Isvalid = "Y";
                            TData.Add(tda);

                        }
                   
                        for (int i = 0; i < 1; i++)
                        {
                            tda1 = new APProInCons();
                    tda1.Itemlst = BindItemlstCon(ca.LOCID);
                    tda1.Isvalid = "Y";
                            tda1.APID = id;
                            TData1.Add(tda1);
                        }
                    

                        for (int i = 0; i < 1; i++)
                        {
                            tda2 = new EmpDetails();
                            tda2.APID = id;
                            tda2.Employeelst = BindEmp();
                            tda2.Isvalid = "Y";
                            TTData2.Add(tda2);
                        }
                    for (int i = 0; i < 1; i++)
                        {
                            tda3 = new BreakDet();

                            tda3.Machinelst = BindMachineID();
                    tda3.Reasonlst = BindReason();
                    tda3.Emplst = BindEmp();
                            tda3.Isvalid = "Y";
                            tda3.APID = id;
                            TData3.Add(tda3);

                        }
                for (int i = 0; i < 3; i++)
                {
                    tda6 = new SourceDetail();
                    tda6.APID = id;

                    tda6.Isvalid = "Y";
                    TTData6.Add(tda6);
                }

                DataTable adt6 = new DataTable();

                    adt6 = IProductionEntry.GetOutput(baid);
                    if (adt6.Rows.Count > 0)
                    {
                        for (int i = 0; i < adt6.Rows.Count; i++)
                        {
                            tda4 = new ProOutput();
                            tda4.Itemlst = BindOutItemlst();
                            tda4.ItemId = adt6.Rows[i]["ITEMID"].ToString();
                            tda4.saveitemId = adt6.Rows[i]["ITEMNAME"].ToString();
                            
                           
                           
                            
                            tda4.drumno = adt6.Rows[i]["DRUMNO"].ToString();
                            
                            tda4.StockQty= Convert.ToDouble(ap.Rows[i]["OUTQTY"].ToString()); 


                        tda4.APID = adt6.Rows[i]["APPRODOUTDETID"].ToString();
                            tda4.Isvalid = "Y";
                            TData4.Add(tda4);

                        }
                  

                }
                    else
                    {
                        for (int i = 0; i < 1; i++)
                        {
                            tda4 = new ProOutput();
                            tda4.APID = id;
                            tda4.Itemlst = BindOutItemlst();
                            tda4.drumlst = BindDrum();
                            tda4.statuslst = BindStatus();
                            tda4.StID = "COMPLETED";
                            tda4.Result = "";
                            tda4.Isvalid = "Y";
                            TData4.Add(tda4);

                        }
                    }
                
                        for (int i = 0; i < 1; i++)
                        {
                            tda5 = new LogDetails();
                            tda5.APID = id;
                            string ShiftTime = datatrans.GetDataString("Select SHIFTHRS from SHIFTMAST where shiftno='" + ca.Shift + "' ");
                            tda5.StartDate = DateTime.Now.ToString("dd-MMM-yyyy  HH:mm:ss");
                            tda5.StartTime = DateTime.Now.ToString("HH:mm");
                            DateTime dateTime = DateTime.Parse(tda5.StartDate);
                            //TimeSpan t1 = new TimeSpan(24,0,0);


                            int hours = int.Parse(ShiftTime);
                            TimeSpan t2 = new TimeSpan(hours, 0, 0);
                            DateTime resultDateTime = dateTime + t2;
                            tda5.EndDate = resultDateTime.ToString("dd-MMM-yyyy - HH:mm");

                            string[] sdateList = tda5.StartDate.Split(" ");
                            string sdate = "";
                            string stime = "";
                            if (sdateList.Length > 0)
                            {
                                sdate = sdateList[0];
                                stime = sdateList[1];
                            }
                            string[] edateList = tda5.EndDate.Split(" - ");
                            string endate = "";
                            string endtime = "";
                            if (sdateList.Length > 0)
                            {
                                endate = edateList[0];
                                endtime = edateList[1];
                            }
                            tda5.StartDate = sdate;
                            tda5.EndDate = endate;
                    tda5.reasonlst = BindReason();
                    tda5.EndTime = endtime;

                            tda5.Isvalid = "Y";
                            TTData5.Add(tda5);

                        }
                     
               
            }
            ca.BreakLst = TData3;
			ca.inplst = TData;
            ca.outlst = TData4;
            ca.EmplLst = TTData2;
			ca.Binconslst = TData1;
            ca.LogLst = TTData5;
            ca.SourcingLst = TTData6;

            return View(ca);
		}
        public List<SelectListItem> BindStatus()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "COMPLETED", Value = "COMPLETED" });
                lstdesg.Add(new SelectListItem() { Text = "PENDING", Value = "PENDING" });
                
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult APProductionentryDetail(APProductionentryDet Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = IProductionEntry.APProductionEntryDetCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "APProductionentryDetail Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "APProductionentryDetail Updated Successfully...!";
                    }
                    return RedirectToAction("APProductionentryDetail", new { id = Cy.inplst[0].APID });
                }

                else
                {
                    ViewBag.PageTitle = "Edit APProductionentryDetail";
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

        public ActionResult APProdApprove(string id)
        {
            APProductionentryDet ca = new APProductionentryDet();
            ca.ID = id;
            DataTable dt = datatrans.GetData("Select SHIFTNO from SHIFTMAST WHERE SHIFTNO IN ('A','B','C') and SHIFTNO not IN  (Select Shift from  APPRODUCTIONBASIC where DOCID=(select DOCID from APPRODUCTIONBASIC where APPRODUCTIONBASICID='" + id + "'))");
            List<string> list = new List<string>();
           
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                list.Add(dt.Rows[i]["SHIFTNO"].ToString());
            }
            list.Add("Complete");
            ca.ShiftNames = list;
            return View(ca);
        }
        [HttpPost]
        public ActionResult APProdApprove(APProductionentryDet Cy, string id)
        {
			
				try
				{
                //Cy.ID = id;
                string Strout = IProductionEntry.APProEntryCRUD(Cy);
					if (string.IsNullOrEmpty(Strout))
					{
                    if(Cy.change=="Complete")
                    {
                        return RedirectToAction("ListAPProductionentry");
                    }
                    else
                    {

                   
						return RedirectToAction("APProductionentryDetail", new { id = Cy.APID, tag= 4 });
                    }
                }

					else
					{
						ViewBag.PageTitle = "Edit APProductionentryDetail";
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
        public ActionResult GetItemDetail(string ItemId, string branch,string loc)
		{
			try
			{
				DataTable dt = new DataTable();
				DataTable dt1 = new DataTable();

				string bin = "";
				string binid = "";
                string stk = "";
                string unit = "";
                string drum = "";
                string lot = "";
                dt = IProductionEntry.GetItemDetails(ItemId);

				if (dt.Rows.Count > 0)
				{

					bin = dt.Rows[0]["BINID"].ToString();
					binid = dt.Rows[0]["bin"].ToString();
					unit = dt.Rows[0]["UNITID"].ToString();
					drum = dt.Rows[0]["DRUMYN"].ToString();
					lot = dt.Rows[0]["LOTYN"].ToString();

				}
                dt1 = datatrans.GetData("SELECT SUM(DECODE(s.PLUSORMINUS,'p',S.QTY,-S.QTY)) as QTY FROM STOCKVALUE S WHERE ITEMID='"+ItemId+"' and LOCID='"+ loc +"'");
                if (dt1.Rows.Count > 0)
                {
                    stk = dt1.Rows[0]["QTY"].ToString();
                }
                if (stk == "")
                {
                    stk = "0";
                }
                var result = new { bin = bin, binid= binid, stk = stk, unit= unit , drum = drum , lot= lot };
				return Json(result);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
        public ActionResult GetStockQty(string Lot, string branch, string loc,string item)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                string locid = datatrans.GetDataString("Select ILOCATION from WCBASIC where WCBASICID='" + loc + "' ");

                string stk = "";
                dt = IProductionEntry.GetStkDetails(Lot, branch, locid, item);

                if (dt.Rows.Count > 0)
                {

                    stk = dt.Rows[0]["BALANCE_QTY"].ToString();
                   

                }
               
                var result = new { stk = stk };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetItemJSON(string batch)
        {
            //EnqItem model = new EnqItem();
            //model.Itemlst = BindItemlst(itemid);
            return Json(BindBatchItemlst(batch));
        }
        public JsonResult GetconsItemJSON(string id)
        {
            return Json(BindItemlstCon(id));
        }
        public JsonResult GetOutItemJSON(string batch)
        {
            //EnqItem model = new EnqItem();
            //model.Itemlst = BindItemlst(itemid);
            return Json(BindBatchOutItemlst(batch));
        }
        public JsonResult GetDrumJSON()
        {
            return Json(BindDrum());
        }
        public ActionResult SaveOutDetail(string id,string ItemId,string drum,string time,string qty,string totime,string exqty,string stat, string stock,string loc,string work,string process,string shift,string schedule,string doc)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                 
                dt = IProductionEntry.SaveOutDetails(id,ItemId, drum, time, qty, totime,exqty,stat, stock, loc, work, process, shift, schedule, doc);

                 

                var result ="";
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetQCResult(string id, string ItemId, string drum)
        {
            try
            {
                var res = "No";
                var resdetail = "";
                int cunt = datatrans.GetDataId("select count(APPRODOUTDETID) as cunt  from APPRODOUTDET where TESTRESULT is not null and APPRODOUTDETID='" + id + "'");
                if (cunt > 0)
                {
                    res = "Yes";
                   resdetail= datatrans.GetDataString("select TESTRESULT  from APPRODOUTDET where TESTRESULT is not null and APPRODOUTDETID='" + id + "'");
                }
                var result = new { res = res, resdetail = resdetail };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetOutItemDetail(string ItemId,string loc)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string bin = "";
                string binid = "";
                string unit = "";
                string stk = "";
                dt = IProductionEntry.GetOutItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    bin = dt.Rows[0]["BINID"].ToString();
                    binid = dt.Rows[0]["bin"].ToString();
                    unit = dt.Rows[0]["UNITID"].ToString();

                }
                dt1 = datatrans.GetData("SELECT SUM(DECODE(s.PLUSORMINUS,'p',S.QTY,-S.QTY)) as QTY FROM STOCKVALUE S WHERE ITEMID='" + ItemId + "' and LOCID='" + loc + "'");
                if (dt1.Rows.Count > 0)
                {
                    stk = dt1.Rows[0]["QTY"].ToString();
                }
                if (stk == "")
                {
                    stk = "0";
                }
                var result = new { bin = bin, binid = binid , unit = unit , stk = stk };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetBatchNo(string ItemId,string work,string doc,string item)
        {
            try
            {
                string itemname = datatrans.GetDataString("SELECT ITEMID FROM ITEMMASTER WHERE ITEMMASTERID='" + item + "'");
                string drum = datatrans.GetDataString("SELECT DRUMNO FROM  DRUMMAST WHERE DRUMMASTID='" + ItemId + "'");


                string batch = itemname + " - " + work + " - " + drum + " - " + doc;
                var result = new { batch = batch };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetConItemDetail(string ItemId,string loc,string branch)
		{
			try
			{
				DataTable dt = new DataTable();
				DataTable dt1 = new DataTable();

				string bin = "";
				string binid = "";
				string unit = "";
				string unitid = "";
                string stk = "";
                dt = IProductionEntry.GetConItemDetails(ItemId);

				if (dt.Rows.Count > 0)
				{

					bin = dt.Rows[0]["BINID"].ToString();
					binid = dt.Rows[0]["bin"].ToString();
					unit = dt.Rows[0]["UNITID"].ToString();
					unitid = dt.Rows[0]["unit"].ToString();

				}
                // dt1 = IProductionEntry.GetConstkqty(ItemId, loc, branch);
                dt1 = datatrans.GetData("SELECT SUM(DECODE(s.PLUSORMINUS,'p',S.QTY,-S.QTY)) as QTY FROM STOCKVALUE S WHERE ITEMID='" + ItemId + "' and LOCID='"+ loc+"'");
                if (dt1.Rows.Count > 0)
                {
                    stk = dt1.Rows[0]["QTY"].ToString();
                }
                if (stk == "")
                {
                    stk = "0";
                }

                var result = new { bin = bin, binid = binid, unit= unit , unitid = unitid, stk= stk };
				return Json(result);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
        public async Task<IActionResult> APPrint1(string id)
        {

            string mimtype = "";
            int extension = 1;
            //string DrumID = datatrans.GetDataString("Select PARTYID from POBASIC where POBASICID='" + id + "' ");

            System.Data.DataSet ds = new System.Data.DataSet();
            var path = $"{this._WebHostEnvironment.WebRootPath}\\Reports\\APProductionDailyReport.rdlc";
            Dictionary<string, string> Parameters = new Dictionary<string, string>();
            //  Parameters.Add("rp1", " Hi Everyone");
            //var Poitem = await PoService.GetPOItem(id, DrumID);

            AspNetCore.Reporting.LocalReport localReport = new AspNetCore.Reporting.LocalReport(path);
            //localReport.AddDataSource("DataSet1", Poitem);
            //localReport.AddDataSource("DataSet1_DataTable1", po);
            var result = localReport.Execute(RenderType.Pdf, extension, Parameters, mimtype);

            return File(result.MainStream, "application/Pdf");
            //responce.redirect();
            //            FunctionExecutor.Run((string[] args) =>
            //            {

            //                using (var fs = new FileStream(args[1], FileMode.Create, FileAccess.Write))
            //            {
            //                fs.Write(result.MainStream);
            //            }
            //        }, new string[] {jsonDataFilePath, generatedFilePath, rdlcFilePath, DataSetName
            //    });

            //            var memory = new MemoryStream();
            //            using (var stream = new FileStream(Path.Combine("", generatedFilePath), FileMode.Open))
            //            {
            //                stream.CopyTo(memory);
            //            }

            //File.Delete(generatedFilePath);
            //File.Delete(jsonDataFilePath);
            //memory.Position = 0;
            //return memory.ToArray();
        }

        public async Task<IActionResult> Print(string id)

        {

            string mimtype = "";
            int extension = 1;
            DataTable ap = datatrans.GetData("Select SHIFT from APPRODUCTIONBASIC where DOCID='" + id + "' ");
            string a = ap.Rows[0]["SHIFT"].ToString();
            string b = ap.Rows[1]["SHIFT"].ToString();
            string c = "";
            if (ap.Rows.Count > 2)
            {
                c = ap.Rows[2]["SHIFT"].ToString();
            }

            string aid = datatrans.GetDataString("Select APPRODUCTIONBASICID from APPRODUCTIONBASIC where SHIFT='" + a + "' and DOCID='" + id + "' ");
            string bid = datatrans.GetDataString("Select APPRODUCTIONBASICID from APPRODUCTIONBASIC where SHIFT='" + b + "'  and DOCID='" + id + "' ");
            string cid = datatrans.GetDataString("Select APPRODUCTIONBASICID from APPRODUCTIONBASIC where SHIFT='" + c + "'  and DOCID='" + id + "' ");
            DataSet ds = new DataSet();
            var path = $"{this._WebHostEnvironment.WebRootPath}\\Reports\\Production.rdlc";
            Dictionary<string, string> Parameters = new Dictionary<string, string>();
            //  Parameters.Add("rp1", " Hi Everyone");

            var APitem = await IProductionEntry.GetAPItem(aid);

            var APitems = await IProductionEntry.GetAPItems(bid);
            var APitemsc = await IProductionEntry.GetAPItemsc(cid);
            LocalReport localReport = new LocalReport(path);

            localReport.AddDataSource("APProduction", APitem);
            localReport.AddDataSource("APOut", APitems);
            localReport.AddDataSource("APCShift", APitemsc);
            //localReport.AddDataSource("DataSet1_DataTable1", po);
            var result = localReport.Execute(RenderType.Pdf, extension, Parameters, mimtype);

            return File(result.MainStream, "application/Pdf");

        }

        public ActionResult GetBatch(string batchid)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                DataTable dt3 = new DataTable();
             
                dt = datatrans.GetData("select W.WCID,S.WCID as work,S.BCPRODBASICID from BCPRODBASIC S ,WCBASIC W where   W.WCBASICID=S.WCID AND S.DOCID='" + batchid + "'");
                dt1 = datatrans.GetData("select SUM(IQTY) as qty from BCINPUTDETAIL where   BCPRODBASICID='" + dt.Rows[0]["BCPRODBASICID"].ToString() + "'");
                //dt2 = datatrans.GetData("select SUM(PRODQTY) as qty from APPRODUCTIONBASIC where   BATCH='" + batchid + "'");

                string work = "";
                string workid = "";
                string schqty = "";
                string prodqty = "";
                string doc = "";
                if (dt.Rows.Count > 0)
                {
                    
                    work = dt.Rows[0]["WCID"].ToString();
                    workid = dt.Rows[0]["work"].ToString();
                    schqty = dt1.Rows[0]["qty"].ToString();
                    //prodqty = dt2.Rows[0]["qty"].ToString();
                   

                   
                }
                
                var result = new { work = work , workid = workid , schqty = schqty/*, prodqty= prodqty*/ };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
