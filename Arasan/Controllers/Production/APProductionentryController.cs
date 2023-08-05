using Arasan.Interface;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Controllers 
{
	public class APProductionentryController : Controller
	{
		IAPProductionEntry IProductionEntry;
		IConfiguration? _configuratio;
		private string? _connectionString;
		DataTransactions datatrans;
		public APProductionentryController(IAPProductionEntry _IProductionEntry, IConfiguration _configuratio)
		{
			IProductionEntry = _IProductionEntry;
			_connectionString = _configuratio.GetConnectionString("OracleDBConnection");
			datatrans = new DataTransactions(_connectionString);
		}


		public ActionResult APProductionentry(string id)
		{
			APProductionentry ca = new APProductionentry();
			DataTable dtv = datatrans.GetSequence("APPro");
			ca.Loclst = BindAPWorkCenter();
			ca.Englst = BindEmp();
			ca.Shiftlst = BindShift();
            ca.Branch = Request.Cookies["BranchId"];
            ca.batchcomplete = "N";
			ca.Shift = "A";
			ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
			if (dtv.Rows.Count > 0)
			{
				ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
			}
			ca.Batchlst = BindBatch();
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
		public List<SelectListItem> BindEmp()
		{
			try
			{
				DataTable dtDesg = datatrans.GetEmp();
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
		public List<SelectListItem> BindBatch()
		{
			try
			{
				DataTable dtDesg = datatrans.GetBatch();
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
		public ActionResult GetEmployeeDetail(string ItemId)
		{
			try
			{
				DataTable dt = new DataTable();

				string code = "";


				dt = IProductionEntry.GetEmployeeDetails(ItemId);

				if (dt.Rows.Count > 0)
				{

					code = dt.Rows[0]["EMPID"].ToString();



				}

				var result = new { code = code };
				return Json(result);
			}
			catch (Exception ex)
			{
				throw ex;
			}
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
        public List<SelectListItem> BindItemlstCon()
		{
			try
			{
				DataTable dtDesg = IProductionEntry.GetItemCon();
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
			IEnumerable<APProductionentry> cmp = IProductionEntry.GetAllAPProductionentry();
			return View(cmp);
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
			if (tag == "2" || tag==null)
			{
				for (int i = 0; i < 3; i++)
				{
					tda3 = new BreakDet();

					tda3.Machinelst = BindMachineID();
					tda3.Emplst = BindEmp();
					tda3.Isvalid = "Y";
					tda3.APID = id;
					TData3.Add(tda3);

				}
				for (int i = 0; i < 3; i++)
				{
					tda = new ProInput();
					tda.APID = id;
					tda.Itemlst = BindItemlst();

					tda.Isvalid = "Y";
					TData.Add(tda);

				}
				for (int i = 0; i < 1; i++)
				{
					tda4 = new ProOutput();
					tda4.APID = id;
					tda4.Itemlst = BindOutItemlst();
					tda4.drumlst = BindDrum();
					tda4.Isvalid = "Y";
					TData4.Add(tda4);

				}
				for (int i = 0; i < 1; i++)
				{
					tda1 = new APProInCons();
					tda1.Itemlst = BindItemlstCon();
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
			}
            DataTable dt = new DataTable();

            dt = IProductionEntry.GetAPProd(id);
            if (dt.Rows.Count > 0)
            {
                ca.Location = dt.Rows[0]["WCID"].ToString();
                ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                ca.DocId = dt.Rows[0]["DOCID"].ToString();
                ca.Eng = dt.Rows[0]["EMPNAME"].ToString();
                ca.Shift = dt.Rows[0]["SHIFT"].ToString();
                ca.SchQty = dt.Rows[0]["SCHQTY"].ToString();
               
                //ca.ParNo = dt.Rows[0]["PARTYREFNO"].ToString();
                ca.ProdQty = dt.Rows[0]["PRODQTY"].ToString();
                //ca.ExRate = dt.Rows[0]["EXCRATERATE"].ToString();
                ca.BatchNo = dt.Rows[0]["BATCH"].ToString();
                ca.batchcomplete = dt.Rows[0]["BATCHYN"].ToString();
            }
            DataTable dt2 = new DataTable();

            dt2 = IProductionEntry.GetInput(id);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new ProInput();
                    tda.Itemlst = BindItemlst();
                    tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                    tda.BinId = dt2.Rows[i]["BINID"].ToString();
                    tda.batchno = dt2.Rows[i]["BATCHNO"].ToString();
                    tda.IssueQty = Convert.ToDouble(dt2.Rows[i]["QTY"].ToString() == "" ? "0" : dt2.Rows[i]["QTY"].ToString());
                    tda.StockAvailable = Convert.ToDouble(dt2.Rows[i]["STOCK"].ToString() == "" ? "0" : dt2.Rows[i]["STOCK"].ToString());
                    tda.APID = id;
                    TData.Add(tda);
                    tda.Isvalid = "Y";
                }

            }
            DataTable dt3 = new DataTable();
            dt3 = IProductionEntry.GetCons(id);
            if (dt3.Rows.Count > 0)
            {
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    tda1 = new APProInCons();
                    tda1.Itemlst = BindItemlstCon();
                    tda1.ItemId = dt3.Rows[i]["ITEMID"].ToString();
                    tda1.consunit = dt3.Rows[i]["UNITID"].ToString();
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
                    TTData2.Add(tda2);
                    tda2.Isvalid = "Y";
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
                    tda4.Itemlst = BindOutItemlst();
                    tda4.ItemId = dt6.Rows[i]["ITEMID"].ToString();
                    tda4.BinId = dt6.Rows[i]["BINID"].ToString();
                    tda4.drumlst = BindDrum();
                    tda4.drumno = dt6.Rows[i]["DRUMNO"].ToString();

                    tda4.OutputQty = Convert.ToDouble(dt6.Rows[i]["OUTQTY"].ToString() == "" ? "0" : dt6.Rows[i]["OUTQTY"].ToString());
                    DataTable dt7 = new DataTable();
                    dt7 = IProductionEntry.GetResult(id);
					if (dt7.Rows.Count > 0)
					{
                        tda4.Result = dt7.Rows[i]["TESTRESULT"].ToString();
                        tda4.Status = dt7.Rows[i]["MOVETOQC"].ToString();
                    }
                        tda4.APID = id;
                    TData4.Add(tda4);
                    tda4.Isvalid = "Y";
                }

            }
			if(tag=="1")
			{
                DataTable ap = datatrans.GetData("select APPRODUCTIONBASICID,DOCID,DOCDATE,SHIFT from APPRODUCTIONBASIC WHERE IS_CURRENT='Yes'");
				if (ap.Rows.Count > 0)
				{
					string apID = datatrans.GetDataString("Select APPRODUCTIONBASICID from APPRODUCTIONBASIC where IS_CURRENT='Yes' ");

                    DataTable adt = new DataTable();

                    adt = IProductionEntry.GetAPProd(apID);
                    if (adt.Rows.Count > 0)
                    {
                        ca.Location = adt.Rows[0]["WCID"].ToString();
                        ca.Docdate = adt.Rows[0]["DOCDATE"].ToString();
                        ca.DocId = adt.Rows[0]["DOCID"].ToString();
                        ca.Eng = adt.Rows[0]["EMPNAME"].ToString();
                        ca.Shift = adt.Rows[0]["SHIFT"].ToString();
                        ca.SchQty = adt.Rows[0]["SCHQTY"].ToString();

                        //ca.ParNo = dt.Rows[0]["PARTYREFNO"].ToString();
                        ca.ProdQty = adt.Rows[0]["PRODQTY"].ToString();
                        //ca.ExRate = dt.Rows[0]["EXCRATERATE"].ToString();
                        ca.BatchNo = adt.Rows[0]["BATCH"].ToString();
                        ca.batchcomplete = adt.Rows[0]["BATCHYN"].ToString();
                    }
                    DataTable adt2 = new DataTable();

                    adt2 = IProductionEntry.GetInput(apID);
                    if (adt2.Rows.Count > 0)
                    {
                        for (int i = 0; i < adt2.Rows.Count; i++)
                        {
                            tda = new ProInput();
                            tda.Itemlst = BindItemlst();
                            tda.ItemId = adt2.Rows[i]["ITEMID"].ToString();
                            tda.BinId = adt2.Rows[i]["BINID"].ToString();
                            tda.batchno = adt2.Rows[i]["BATCHNO"].ToString();
                            tda.IssueQty = Convert.ToDouble(adt2.Rows[i]["QTY"].ToString() == "" ? "0" : adt2.Rows[i]["QTY"].ToString());
                            tda.StockAvailable = Convert.ToDouble(adt2.Rows[i]["STOCK"].ToString() == "" ? "0" : adt2.Rows[i]["STOCK"].ToString());
                            tda.APID = id;
                            TData.Add(tda);
                            tda.Isvalid = "Y";
                        }

                    }
					else
					{
                        for (int i = 0; i < 1; i++)
                        {
                            tda = new ProInput();
                            tda.APID = id;
                            tda.Itemlst = BindItemlst();

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
                            tda1.Itemlst = BindItemlstCon();
                            tda1.ItemId = adt3.Rows[i]["ITEMID"].ToString();
                            tda1.consunit = adt3.Rows[i]["UNITID"].ToString();
                            tda1.BinId = adt3.Rows[i]["BINID"].ToString();
                            tda1.Qty = Convert.ToDouble(adt3.Rows[i]["QTY"].ToString() == "" ? "0" : adt3.Rows[i]["QTY"].ToString());
                            tda1.consQty = Convert.ToDouble(adt3.Rows[i]["CONSQTY"].ToString() == "" ? "0" : adt3.Rows[i]["CONSQTY"].ToString());
                            tda1.ConsStock = Convert.ToDouble(adt3.Rows[i]["STOCK"].ToString() == "" ? "0" : adt3.Rows[i]["STOCK"].ToString());

                            tda1.APID = id;
                            tda1.Isvalid = "Y";
                            TData1.Add(tda1);
                        }

                    }
					else
					{
                        for (int i = 0; i < 1; i++)
                        {
                            tda1 = new APProInCons();
                            tda1.Itemlst = BindItemlstCon();
                            tda1.Isvalid = "Y";
                            tda1.APID = id;
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
                            tda2.ID = id;
                            TTData2.Add(tda2);
                            tda2.Isvalid = "Y";
                        }

                    }
					else
					{
                       
                        for (int i = 0; i < 1; i++)
                        {
                            tda2 = new EmpDetails();
                            tda2.APID = id;
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

                            tda3.APID = id;
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
                            tda3.Isvalid = "Y";
                            tda3.APID = id;
                            TData3.Add(tda3);

                        }
                    }
                    DataTable adt6 = new DataTable();

                    adt6 = IProductionEntry.GetOutput(apID);
                    if (adt6.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt6.Rows.Count; i++)
                        {
                            tda4 = new ProOutput();
                            tda4.Itemlst = BindOutItemlst();
                            tda4.ItemId = adt6.Rows[i]["ITEMID"].ToString();
                            tda4.BinId = adt6.Rows[i]["BINID"].ToString();
                            tda4.drumlst = BindDrum();
                            tda4.drumno = adt6.Rows[i]["DRUMNO"].ToString();

                            tda4.OutputQty = Convert.ToDouble(adt6.Rows[i]["OUTQTY"].ToString() == "" ? "0" : adt6.Rows[i]["OUTQTY"].ToString());
                            DataTable dt7 = new DataTable();
                            dt7 = IProductionEntry.GetResult(id);
                            if (dt7.Rows.Count > 0)
                            {
                                tda4.Result = dt7.Rows[i]["TESTRESULT"].ToString();
                                tda4.Status = dt7.Rows[i]["MOVETOQC"].ToString();
                            }
                            tda4.APID = id;
                            TData4.Add(tda4);
                            tda4.Isvalid = "Y";
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
                            tda4.Isvalid = "Y";
                            TData4.Add(tda4);

                        }
                    }
                }
				else
				{
                    return RedirectToAction("APProductionentry");
                }
            }
            ca.BreakLst = TData3;
			ca.inplst = TData;
            ca.outlst = TData4;
            ca.EmplLst = TTData2;
			ca.Binconslst = TData1;
			
			return View(ca);
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
           
            return View(ca);
        }
        [HttpPost]
        public ActionResult APProdApproves(APProductionentryDet Cy, string id)
        {
			if (Cy.change != "Complete")
			{
				try
				{
					Cy.ID = id;
					string Strout = IProductionEntry.APProEntryCRUD(Cy);
					if (string.IsNullOrEmpty(Strout))
					{
						//if (Cy.ID == null)
						//{
						//    TempData["notice"] = "APProductionentryDetail Inserted Successfully...!";
						//}
						//else
						//{
						//    TempData["notice"] = "APProductionentryDetail Updated Successfully...!";
						//}
						return RedirectToAction("APProductionentryDetail", new { id = Cy.APID });
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
			}
			else
			{
                return RedirectToAction("ListAPProductionentry");
            }
            return View(Cy);
        }
        public ActionResult GetItemDetail(string ItemId)
		{
			try
			{
				DataTable dt = new DataTable();
				DataTable dt1 = new DataTable();

				string bin = "";
				string binid = "";
				dt = IProductionEntry.GetItemDetails(ItemId);

				if (dt.Rows.Count > 0)
				{

					bin = dt.Rows[0]["BINID"].ToString();
					binid = dt.Rows[0]["bin"].ToString();

				}

				var result = new { bin = bin, binid= binid };
				return Json(result);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
        public JsonResult GetItemJSON()
        {
            //EnqItem model = new EnqItem();
            //model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst());
        }
        public JsonResult GetOutItemJSON()
        {
            //EnqItem model = new EnqItem();
            //model.Itemlst = BindItemlst(itemid);
            return Json(BindOutItemlst());
        }
        public JsonResult GetDrumJSON()
        {
            return Json(BindDrum());
        }
        public ActionResult SaveOutDetail(string id,string ItemId,string drum,string time,string qty)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string bin = "";
                string binid = "";
                dt = IProductionEntry.SaveOutDetails(id,ItemId, drum, time, qty);

                 

                var result ="";
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetOutItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string bin = "";
                string binid = "";
                dt = IProductionEntry.GetOutItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    bin = dt.Rows[0]["BINID"].ToString();
                    binid = dt.Rows[0]["bin"].ToString();

                }

                var result = new { bin = bin, binid = binid };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetConItemDetail(string ItemId)
		{
			try
			{
				DataTable dt = new DataTable();
				DataTable dt1 = new DataTable();

				string bin = "";
				string binid = "";
				string unit = "";
				string unitid = "";

				dt = IProductionEntry.GetConItemDetails(ItemId);

				if (dt.Rows.Count > 0)
				{

					bin = dt.Rows[0]["BINID"].ToString();
					binid = dt.Rows[0]["bin"].ToString();
					unit = dt.Rows[0]["UNITID"].ToString();
					unitid = dt.Rows[0]["unit"].ToString();

				}

				var result = new { bin = bin, binid = binid, unit= unit , unitid = unitid };
				return Json(result);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
