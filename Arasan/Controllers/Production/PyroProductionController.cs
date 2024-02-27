using System.Collections.Generic;
using Arasan.Interface.Production;
using Arasan.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Services.Production;
using System.Collections.Specialized;
using Arasan.Interface;
using System.Xml.Linq;
using Arasan.Services.Sales;
using AspNetCore.Reporting;
using DocumentFormat.OpenXml.EMMA;

namespace Arasan.Controllers
{
    public class PyroProductionController : Controller
    {
        IPyroProduction Pyro;
        IConfiguration? _configuratio;
        private string? _connectionString;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        DataTransactions datatrans;
        public PyroProductionController(IPyroProduction _Pyro, IConfiguration _configuratio, IWebHostEnvironment WebHostEnvironment)
        {
            this._WebHostEnvironment = WebHostEnvironment;
            Pyro = _Pyro;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult PyroProductionentry(string id, string tag, string shift)
        {
            PyroProductionentryDet ca = new PyroProductionentryDet();
            //ca.Complete = "No";
            //ca.Eng = Request.Cookies["UserName"];
            //ca.super = Request.Cookies["UserId"];
            //ca.Branch = Request.Cookies["BranchId"];
            //ca.Shiftlst = BindShift();
            //ca.worklst = BindWork(ca.super);
            //ca.Shiftlst = BindShift();
            //ca.Wclst = BindWorkedit(ca.super);
            //ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            //ca.ProdSchlst = BindProdSch("");
            //ca.Plotlst = BindPLot("", "");
            //ca.ProdLog = "N";
            //ca.Processlst = BindProcess();
            //DataTable dtv = datatrans.GetSequence("nProd");
            //if (dtv.Rows.Count > 0)
            //{
            //    ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + "" + dtv.Rows[0]["last"].ToString();
            //}
            List<PBreakDet> TData3 = new List<PBreakDet>();
            PBreakDet tda3 = new PBreakDet();
            List<PProInput> TData = new List<PProInput>();
            PProInput tda = new PProInput();
            List<PProOutput> TData4 = new List<PProOutput>();
            PProOutput tda4 = new PProOutput();
            List<PAPProInCons> TData1 = new List<PAPProInCons>();
            PAPProInCons tda1 = new PAPProInCons();
            List<PEmpDetails> TTData2 = new List<PEmpDetails>();
            PEmpDetails tda2 = new PEmpDetails();
            List<PLogDetails> TTData5 = new List<PLogDetails>();
            PLogDetails tda5 = new PLogDetails();
            List<PSourcingDetail> TData6 = new List<PSourcingDetail>();
            PSourcingDetail tda6 = new PSourcingDetail();
            List<PBunkerDetail> TData9 = new List<PBunkerDetail>();
            PBunkerDetail tda9 = new PBunkerDetail();

            if (string.IsNullOrEmpty(id))
            {
                for (int i = 0; i < 3; i++)
                {
                    tda3 = new PBreakDet();

                    tda3.Machinelst = BindMachineID();
                    tda3.Emplst = BindEmp();
                    tda3.Reasonlst = BindReason();
                    tda3.Isvalid = "Y";
                    tda3.APID = id;
                    TData3.Add(tda3);

                }
                for (int i = 0; i < 1; i++)
                {
                    tda = new PProInput();
                    tda.APID = id;
                    tda.Itemlst = BindInputItemlst("");
                    tda.drumlst = BindDrum("","");
                    tda.batchlst = BindDrumBatch("", "", "");
                    tda.Isvalid = "Y";
                    TData.Add(tda);

                }
                for (int i = 0; i < 1; i++)
                {
                    tda4 = new PProOutput();
                    tda4.APID = id;
                    tda4.Itemlst = BindOutItemlst("");
                    tda4.drumlst = BindDrum();
                    tda4.statuslst = BindStatus();
                    tda4.shedlst = BindShed();
                    tda4.Isvalid = "Y";
                    TData4.Add(tda4);

                }
                for (int i = 0; i < 1; i++)
                {
                    tda1 = new PAPProInCons();
                    tda1.Itemlst = BindItemlstCon("");
                    tda1.Isvalid = "Y";
                    tda1.APID = id;
                    TData1.Add(tda1);
                }
                for (int i = 0; i < 3; i++)
                {
                    tda2 = new PEmpDetails();
                    tda2.APID = id;
                    tda2.Employeelst = BindEmp();
                    tda2.Isvalid = "Y";
                    TTData2.Add(tda2);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda5 = new PLogDetails();
                    tda5.APID = id;
                    tda5.Isvalid = "Y";
                    tda5.Reasonlst = BindReason();
                    TTData5.Add(tda5);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda6 = new PSourcingDetail();
                    tda6.APID = id;
                    int ShiftTime = datatrans.GetDataId("Select SHIFTHRS from SHIFTMAST where shiftno='" + ca.Shift + "' ");
                    tda6.StartDate = DateTime.Now.ToString("dd-MMM-yyyy  HH:mm:ss");
                    tda6.StartTime = DateTime.Now.ToString("HH:mm");
                    DateTime dateTime = DateTime.Parse(tda6.StartDate);
                    //TimeSpan t1 = new TimeSpan(24,0,0);
                    tda6.WorkHrs = ShiftTime.ToString();

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
                    TData6.Add(tda6);
                }
                tda9 = new PBunkerDetail();
                TData9.Add(tda9);
            }
            if (!string.IsNullOrEmpty(id))
            {


                DataTable dt = new DataTable();

                dt = Pyro.GetProduction(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Location = dt.Rows[0]["WORKID"].ToString();
                    ca.Locationid = dt.Rows[0]["ILOCDETAILSID"].ToString();
                    ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    ca.Eng = dt.Rows[0]["ENTEREDBY"].ToString();
                    ca.Shift = dt.Rows[0]["SHIFT"].ToString();
                    ViewBag.shift = dt.Rows[0]["SHIFT"].ToString();
                    ca.ProcessLot= dt.Rows[0]["PROCLOTNO"].ToString();
                    ca.process= dt.Rows[0]["PROCESS"].ToString();
                    ca.ProcessId= dt.Rows[0]["PROCESSID"].ToString();
                    ca.ProdQty = dt.Rows[0]["PRODQTY"].ToString();
                    ca.SchQty = dt.Rows[0]["SCHQTY"].ToString();
                    ca.ProdSchNo = dt.Rows[0]["psno"].ToString();
                    ca.ProdSchid = dt.Rows[0]["PSCHNO"].ToString();
                    ca.APID = id;
                    ca.workid = dt.Rows[0]["WCID"].ToString(); 
                    double MLDed = 0;
                    string binopbal = Pyro.GetBinOPBal(ca.ProcessLot, ca.DocId,ca.ProcessId, ca.workid);
                    string mlopbal = Pyro.GetMLOPBal(ca.ProcessLot, ca.DocId, ca.ProcessId, ca.workid);
                    string powinp = datatrans.GetDataString("Select Sum(I.IQty) TotPinp From nProdBasic B , nProdInpDet I , LProdBasic LB , ItemMaster IM Where B.nProdBasicID = I.nProdBasicID And B.ProdLogID = LB.LProdBasicID And I.IItemID = IM.ItemMasterID And ( Upper(IM.SubCategory) <> 'GREASE' Or IM.SubCategory Is Null ) And LB.DocID = '"+ ca.DocId + "'");
                    string totginp= datatrans.GetDataString("Select SUm(Qty) from (Select Sum(I.IQty) Qty From nProdBasic B , nProdInpDet I , LProdBasic LB , ItemMaster IM Where B.nProdBasicID = I.nProdBasicID And B.ProdLogID = LB.LProdBasicID And I.IItemID = IM.ItemMasterID And Upper(IM.SubCategory) = 'GREASE' And LB.DocID = '" + ca.DocId + "' Union All Select Sum(I.ConsQty) TotGinp From nProdBasic B , nProdConsDet I , LProdBasic LB , ItemMaster IM Where B.nProdBasicID = I.nProdBasicID And B.ProdLogID = LB.LProdBasicID And I.CItemID = IM.ItemMasterID And Upper(IM.SubCategory) = 'GREASE' And LB.DocID = '" + ca.DocId + "') ");
                    string TotOut = datatrans.GetDataString("Select Sum(I.OQty) TotOut From nProdBasic B , nProdOutDet I , LProdBasic LB , ItemMaster IM Where B.nProdBasicID = I.nProdBasicID And B.ProdLogID = LB.LProdBasicID And I.OItemID = IM.ItemMasterID And LB.DocID = '"+ ca.DocId + "'");
                    string TotOxd = datatrans.GetDataString("Select Sum(I.OxQty) TotOxd From nProdBasic B , nProdOutDet I , LProdBasic LB , ItemMaster IM Where B.nProdBasicID = I.nProdBasicID And B.ProdLogID = LB.LProdBasicID And I.OItemID = IM.ItemMasterID And Upper(IM.SnCategory) = 'PYRO POWDER' And LB.DocID = '"+ ca.DocId + "'");
                    string MLAdd = datatrans.GetDataString("Select Sum(I.MLOADADD) TotOxd From nProdBasic B , nProdInpDet I , LProdBasic LB  Where B.nProdBasicID = I.nProdBasicID And B.ProdLogID = LB.LProdBasicID And LB.DocID = '"+ ca.DocId + "'");
                    double Totinp = Convert.ToDouble(binopbal == "" ? 0 : binopbal) + Convert.ToDouble(powinp == "" ? 0 : powinp);
                    double MlClBal = (Convert.ToDouble(mlopbal == "" ? 0 : mlopbal) + Convert.ToDouble(MLAdd == "" ? 0 : MLAdd)) - MLDed;
                    double TotRmCh = Convert.ToDouble(TotOut == "" ? 0 : TotOut) + Convert.ToDouble(MLAdd == "" ? 0 : MLAdd) - MLDed - Convert.ToDouble(TotOxd == "" ? 0 : TotOxd) - Convert.ToDouble(totginp == "" ? 0 : totginp);
                    double ClBal = Math.Round((((Totinp + Convert.ToDouble(totginp == "" ? 0 : totginp) + Convert.ToDouble(TotOxd == "" ? 0 : TotOxd)) - Convert.ToDouble(TotOut == "" ? 0 : TotOut)) - Convert.ToDouble(MLAdd == "" ? 0 : MLAdd) + MLDed), 0);

                    tda9 = new PBunkerDetail();
                    tda9.OPBin = Convert.ToDouble(binopbal == "" ? 0 : binopbal);
                    tda9.PIP= Convert.ToDouble(powinp == "" ? 0 : powinp);
                    tda9.GIP = Convert.ToDouble(totginp == "" ? 0 : totginp);
                    tda9.TIP = Totinp;
                    tda9.TOP = Convert.ToDouble(TotOut == "" ? 0 : TotOut);
                    tda9.OXD = Convert.ToDouble(TotOxd == "" ? 0 : TotOxd);
                    tda9.TRM = TotRmCh;
                    tda9.CLBin = ClBal;
                    tda9.MLOP = Convert.ToDouble(mlopbal == "" ? 0 : mlopbal);
                    tda9.MLAdd = Convert.ToDouble(MLAdd == "" ? 0 : MLAdd);
                    tda9.MLCL = MlClBal;
                    TData9.Add(tda9);




                    DataTable dt2 = new DataTable();

                    dt2 = Pyro.GetInput(id);
                    if (dt2.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            tda = new PProInput();
                            tda.Itemlst = BindInputItemlst(ca.ProdSchid);
                            tda.ItemId = dt2.Rows[i]["IITEMID"].ToString();
                            tda.saveitemId = dt2.Rows[i]["IITEMID"].ToString();
                            tda.Time = dt2.Rows[i]["CHARGINGTIME"].ToString();
                            tda.drumlst = BindDrum(tda.ItemId, ca.Locationid);
                            tda.drumno = dt2.Rows[i]["ICDRUMNO"].ToString();
                            tda.insert = dt2.Rows[i]["IS_INSERT"].ToString();
                         
                            //tda.unit = dt2.Rows[i]["UNITID"].ToString();
                            tda.Bin = dt2.Rows[i]["IBINID"].ToString();
                            tda.BinId = dt2.Rows[i]["BINID"].ToString();
                            
                          tda.batchlst = BindDrumBatch(tda.drumno, ca.Locationid, tda.ItemId);
                             
                            tda.batchno = dt2.Rows[i]["IBATCHNO"].ToString();
                            tda.IssueQty = Convert.ToDouble(dt2.Rows[i]["IQTY"].ToString() == "" ? "0" : dt2.Rows[i]["IQTY"].ToString());
                            DataTable dtstk = new DataTable();

                            dtstk = datatrans.GetData("SELECT SUM(S.PLUSQTY-S.MINUSQTY) as QTY FROM LSTOCKVALUE S WHERE S.ITEMID='" + tda.ItemId + "' and S.LOCID='" + ca.Locationid + "' and s.LOTNO='" + tda.batchno + "' HAVING SUM(S.PLUSQTY-S.MINUSQTY) > 0");

                                 

                            if (dtstk.Rows.Count > 0)
                            {
                                tda.StockAvailable = Convert.ToDouble(dtstk.Rows[0]["QTY"].ToString() == "" ? "0" : dtstk.Rows[0]["QTY"].ToString());
                            }
                            tda.APID = id;
                            tda.Isvalid = "Y";
                            TData.Add(tda);

                        }

                    }
                    else
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            tda = new PProInput();
                            tda.APID = id;
                            tda.Itemlst = BindInputItemlst(ca.ProdSchid);
                            tda.drumlst = BindDrum("", "");
                            tda.batchlst = BindDrumBatch("", "", "");
                            tda.Isvalid = "Y";
                            TData.Add(tda);

                        }
                    }
                    DataTable dt3 = new DataTable();
                    dt3 = Pyro.GetCons(id);
                    if (dt3.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt3.Rows.Count; i++)
                        {
                            tda1 = new PAPProInCons();
                            tda1.Itemlst = BindItemlstCon(ca.Locationid);
                            tda1.ItemId = dt3.Rows[i]["CITEMID"].ToString();
                            tda1.consunit = dt3.Rows[i]["CUNIT"].ToString();
                            tda1.BinId = dt3.Rows[i]["CBINID"].ToString();
                            tda1.insert = dt3.Rows[i]["IS_INSERT"].ToString();
                            tda1.Qty = Convert.ToDouble(dt3.Rows[i]["CSUBQTY"].ToString() == "" ? "0" : dt3.Rows[i]["CSUBQTY"].ToString());
                            tda1.consQty = Convert.ToDouble(dt3.Rows[i]["CONSQTY"].ToString() == "" ? "0" : dt3.Rows[i]["CONSQTY"].ToString());

                            DataTable  dtstk = datatrans.GetData("SELECT SUM(DECODE(s.PLUSORMINUS,'p',S.QTY,-S.QTY)) as QTY FROM STOCKVALUE S WHERE ITEMID='" + tda1.ItemId + "' and LOCID='" + ca.LOCID + "'");
                            if (dtstk.Rows.Count > 0)
                            {
                                tda1.ConsStock = Convert.ToDouble(dtstk.Rows[0]["QTY"].ToString() == "" ? "0" : dtstk.Rows[0]["QTY"].ToString());
                            }
                            tda1.APID = id;
                            tda1.Isvalid = "Y";
                            TData1.Add(tda1);
                        }

                    }
                    else
                    {
                        for (int i = 0; i < 1; i++)
                        {
                            tda1 = new PAPProInCons();
                            tda1.Itemlst = BindItemlstCon(ca.Locationid);
                            tda1.Isvalid = "Y";
                            tda1.APID = id;
                            TData1.Add(tda1);
                        }
                    }
                    DataTable dt4 = new DataTable();
                    dt4 = Pyro.GetEmpdet(id);
                    if (dt4.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt4.Rows.Count; i++)
                        {
                            tda2 = new PEmpDetails();
                            tda2.Employeelst = BindEmp();
                            tda2.Employee = dt4.Rows[i]["EMPNAME"].ToString();

                            tda2.EmpCode = dt4.Rows[i]["EMPCODE1"].ToString();
                            tda2.Depart = dt4.Rows[i]["DEPARTMENT"].ToString();
                            tda2.StartDate = dt4.Rows[i]["ESTARTDATE"].ToString();
                            tda2.StartTime = dt4.Rows[i]["ESTARTTIME"].ToString();
                            tda2.EndDate = dt4.Rows[i]["EENDDATE"].ToString();
                            tda2.EndTime = dt4.Rows[i]["EENDTIME"].ToString();
                            tda2.OTHrs = dt4.Rows[i]["OTHRS"].ToString();

                            tda2.ETOther = dt4.Rows[i]["ETOTHRS"].ToString();
                            tda2.Normal = dt4.Rows[i]["NORMHRS"].ToString();
                            tda2.NOW = dt4.Rows[i]["NATOFW"].ToString();
                            tda2.ID = id;
                            tda2.Isvalid = "Y";
                            TTData2.Add(tda2);

                        }

                    }
                    else
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            tda2 = new PEmpDetails();
                            tda2.APID = id;
                            tda2.Employeelst = BindEmp();
                            tda2.Isvalid = "Y";
                            TTData2.Add(tda2);
                        }
                    }
                    DataTable dt5 = new DataTable();
                    dt5 = Pyro.GetBreak(id);
                    if (dt5.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt5.Rows.Count; i++)
                        {
                            tda3 = new PBreakDet();
                            tda3.Machinelst = BindMachineID();
                            tda3.MachineId = dt5.Rows[i]["BMACNO"].ToString();
                            tda3.Emplst = BindBreakEmp();
                            tda3.MachineDes = dt5.Rows[i]["BMACHINEDESC"].ToString();
                            tda3.StartTime = dt5.Rows[i]["BFROMTIME"].ToString();
                            tda3.EndTime = dt5.Rows[i]["BTOTIME"].ToString();
                            tda3.PB = dt5.Rows[i]["PREORBRE"].ToString();
                            tda3.Isvalid = "Y";
                            tda3.Alloted = dt5.Rows[i]["ALLOTEDTO"].ToString();
                            tda3.DType = dt5.Rows[i]["DTYPE"].ToString();
                            tda3.MType = dt5.Rows[i]["MTYPE"].ToString();
                            tda3.Reasonlst = BindReason();
                            tda3.Reason = dt5.Rows[i]["ACTDESC"].ToString();

                            tda3.APID = id;
                            TData3.Add(tda3);
                        }

                    }
                    else
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            tda3 = new PBreakDet();

                            tda3.Machinelst = BindMachineID();
                            tda3.Emplst = BindBreakEmp();
                            tda3.Reasonlst = BindReason();
                            tda3.Isvalid = "Y";
                            tda3.APID = id;
                            TData3.Add(tda3);

                        }
                    }
                    DataTable dt6 = new DataTable();

                    dt6 = Pyro.GetOutput(id);
                    if (dt6.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt6.Rows.Count; i++)
                        {
                            tda4 = new PProOutput();
                            tda4.Itemlst = BindOutItemlst(ca.ProdSchid);
                            tda4.ItemId = dt6.Rows[i]["OITEMID"].ToString();

                            tda4.drumlst = BindDrum();
                            tda4.statuslst = BindStatus();
                           
                            tda4.Status = dt6.Rows[i]["STATUS"].ToString();
                            // tda4.unit = dt6.Rows[i]["UNITID"].ToString();
                            tda4.ExcessQty = Convert.ToDouble(dt6.Rows[i]["OXQTY"].ToString() == "" ? "0" : dt6.Rows[i]["OXQTY"].ToString());
                            tda4.drumno = dt6.Rows[i]["ODRUMNO"].ToString();
                            tda4.FromTime = dt6.Rows[i]["STIME"].ToString();
                            tda4.ToTime = dt6.Rows[i]["ETIME"].ToString();
                            tda4.shedlst = BindShed();
                            tda4.ShedNo = dt6.Rows[i]["SHEDNUMBER"].ToString();
                            tda4.OutputQty = Convert.ToDouble(dt6.Rows[i]["OQTY"].ToString() == "" ? "0" : dt6.Rows[i]["OQTY"].ToString());
                            //DataTable dt7 = new DataTable();
                            //dt7 = IProductionEntry.GetResult(id);
                            //if (dt7.Rows.Count > 0)
                            //{
                            //    tda4.Result = dt7.Rows[i]["TESTRESULT"].ToString();
                            //    tda4.Status = dt7.Rows[i]["MOVETOQC"].ToString();
                            //}
                            DataTable cur = datatrans.GetData("SELECT OCCUPIED,CAPACITY FROM CURINGMASTER WHERE SHEDNUMBER='" + tda4.ShedNo + "' ");
                          


                            if (cur.Rows.Count > 0)
                            {

                                tda4.ShedOccu = cur.Rows[0]["OCCUPIED"].ToString();
                                tda4.ShedCap = cur.Rows[0]["CAPACITY"].ToString();


                            }
                            tda4.APID = id;
                            tda4.Isvalid = "Y";
                            TData4.Add(tda4);

                        }

                    }
                    else
                    {
                        for (int i = 0; i < 1; i++)
                        {
                            tda4 = new PProOutput();
                            tda4.APID = id;
                            tda4.Itemlst = BindOutItemlst(ca.ProdSchid);
                            tda4.drumlst = BindDrum();
                            tda4.shedlst = BindShed();
                            tda4.statuslst = BindStatus();
                            tda4.Isvalid = "Y";
                            TData4.Add(tda4);

                        }
                    }
                    DataTable adt7 = new DataTable();

                    adt7 = Pyro.GetLogdetail(id);
                    if (adt7.Rows.Count > 0)
                    {
                        for (int i = 0; i < adt7.Rows.Count; i++)
                        {
                            tda5 = new PLogDetails();

                            tda5.StartDate = adt7.Rows[i]["STARTDATE"].ToString();
                            tda5.StartTime = adt7.Rows[i]["STARTTIME"].ToString();

                            tda5.EndDate = adt7.Rows[i]["ENDDATE"].ToString();
                            tda5.Reasonlst = BindReason();
                            tda5.Reason = adt7.Rows[i]["REASON"].ToString();



                            tda5.EndTime = adt7.Rows[i]["ENDTIME"].ToString();
                            tda5.tothrs = adt7.Rows[i]["TOTALHRS"].ToString();

                            tda5.APID = id;
                            tda5.Isvalid = "Y";
                            TTData5.Add(tda5);
                           
                        }

                    }
                    else
                    {

                        for (int i = 0; i < 1; i++)
                        {
                            tda5 = new PLogDetails();
                            tda5.APID = id;
                            tda5.Isvalid = "Y";
                            tda5.Reasonlst = BindReason();
                            TTData5.Add(tda5);
                        }
                    }
                    DataTable adt8 = new DataTable();

                    adt8 = Pyro.GetOutsdetail(id);
                    if (adt8.Rows.Count > 0)
                    {
                        for (int i = 0; i < adt8.Rows.Count; i++)
                        {
                            tda6 = new PSourcingDetail();

                            tda6.NoOfEmp = adt8.Rows[i]["NOOFEMP"].ToString();
                            tda6.StartDate = adt8.Rows[i]["OWSTDTT"].ToString();

                            tda6.StartTime = adt8.Rows[i]["OWSTT"].ToString();
                            tda6.EndDate = adt8.Rows[i]["OWEDDTT"].ToString();
                            tda6.EndTime = adt8.Rows[i]["OWEDT"].ToString();
                            tda6.Expence = Convert.ToDouble(adt8.Rows[i]["MANPOWEXP"].ToString() == "" ? "0" : adt8.Rows[i]["MANPOWEXP"].ToString());
                            tda6.WorkHrs = adt8.Rows[i]["EMPWHRS"].ToString();
                            tda6.EmpCost = Convert.ToDouble(adt8.Rows[i]["EMPPAY"].ToString() == "" ? "0" : adt8.Rows[i]["EMPPAY"].ToString());
                            tda6.NOW = adt8.Rows[i]["ONATOFW"].ToString();

                            tda6.APID = id;
                            tda6.Isvalid = "Y";
                            TData6.Add(tda6);
                           
                        }

                    }
                    else
                    {
                        for (int i = 0; i < 1; i++)
                        {
                            tda6 = new PSourcingDetail();
                            tda6.APID = id;
                            int ShiftTime = datatrans.GetDataId("Select SHIFTHRS from SHIFTMAST where shiftno='" + ca.Shift + "' ");
                            tda6.StartDate = DateTime.Now.ToString("dd-MMM-yyyy  HH:mm:ss");
                            tda6.StartTime = DateTime.Now.ToString("HH:mm");
                            DateTime dateTime = DateTime.Parse(tda6.StartDate);
                            //TimeSpan t1 = new TimeSpan(24,0,0);
                            tda6.WorkHrs = ShiftTime.ToString();

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
                            TData6.Add(tda6);
                        }
                    }

                }

                //tda9 = new PBunkerDetail();
                //TData9.Add(tda9);
            }



            ca.BreakLst = TData3;
            ca.inplst = TData;
            ca.outlst = TData4;
            ca.EmplLst = TTData2;
            ca.Binconslst = TData1;
            ca.LogLst = TTData5;
            ca.BunkLst = TData9;
            ca.SourcingLst = TData6;
            return View(ca);
        }


        public IActionResult PyroProduction(string id, string tag, string shift)
        {

            PyroProductionentryDet ca = new PyroProductionentryDet();
            //ca.Complete = "No";
            ca.Eng = Request.Cookies["UserName"];
            ca.super = Request.Cookies["UserId"];
            ca.Branch = Request.Cookies["BranchId"];
            ca.Shiftlst = BindShift();
            ca.worklst = BindWork(ca.super);
            ca.Shiftlst = BindShift();
            ca.Wclst = BindWorkedit(ca.super);
            ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.ProdSchlst = BindProdSch("");
            ca.Plotlst = BindPLot("", "");
            ca.ProdLog = "N";
            ca.Processlst = BindProcess();
            DataTable dtv = datatrans.GetSequence("nProd");
            if (dtv.Rows.Count > 0)
            {
                ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + "" + dtv.Rows[0]["last"].ToString();
            }
            return View(ca);
        }

        [HttpPost]
        public ActionResult PyroProduction(PyroProduction Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = Pyro.PyroProductionEntry(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "PyroProduction Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "PyroProduction Updated Successfully...!";
                    }
                    //return RedirectToAction("APProductionentryDetail", new { id = id });
                }

                else
                {
                    ViewBag.PageTitle = "Edit PyroProduction";
                    TempData["notice"] = Strout;
                    //return View();
                }
                return RedirectToAction("PyroProductionentryDetail", new { id = Cy.APID, tag = 2 });
                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Cy);
        }
        public ActionResult GetMachineDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string name = "";
                string type = "";

                dt = Pyro.GetMachineDetails(ItemId);

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
        public List<SelectListItem> BindInputItemlst(string value)
        {
            try
            {
                DataTable dtDesg = Pyro.GetItem(value);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["RITEMID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindOutItemlst(string id)
        {
            try
            {
                DataTable dtDesg = Pyro.GetOutItem(id);
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

        public List<SelectListItem> BindItemlstCon(string id)
        {
            try
            {
                DataTable dtDesg = Pyro.GetItemCon(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["item"].ToString() });
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
                DataTable dtDesg = Pyro.GetDrum();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DRUMNO"].ToString(), Value = dtDesg.Rows[i]["DRUMMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetDrumnoJSON(string itemid,string loc)
        {
            PProInput model = new PProInput();
            model.Itemlst = BindDrum(itemid, loc);
            return Json(BindDrum(itemid, loc));

        }
        public JsonResult GetPLotJSON(string procid, string schid)
        {
            PyroProductionentryDet model = new PyroProductionentryDet();
            model.Plotlst = BindPLot(procid, schid);
            return Json(BindPLot(procid, schid));

        }
        public JsonResult GetPSchedJSON(string schid)
        {
            PyroProductionentryDet model = new PyroProductionentryDet();
            model.Plotlst = BindProdSch(schid);
            return Json(BindProdSch(schid));

        }
        public List<SelectListItem> BindDrum(string item,string loc)
        {
            try
            {
                DataTable dtDesg = Pyro.GetDrum(item, loc);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DRUMNO"].ToString(), Value = dtDesg.Rows[i]["DRUMNO"].ToString() });
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
                DataTable dtDesg = Pyro.GetEmp();
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
        public List<SelectListItem> BindBreakEmp()
        {
            try
            {
                DataTable dtDesg = Pyro.GetEmp();
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
        public ActionResult GetEmployeeDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string code = "";
                string dept = "";
                string empcost = "";
                string othrs = "";

                dt = Pyro.GetEmployeeDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    code = dt.Rows[0]["EMPID"].ToString();
                    dept = dt.Rows[0]["DEPTNAME"].ToString();
                    empcost = dt.Rows[0]["EMPCOST"].ToString();
                    othrs = dt.Rows[0]["OTPERHR"].ToString();

                }

                var result = new { code = code, dept = dept, empcost = empcost, othrs = othrs };
                return Json(result);
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
        public List<SelectListItem> BindMachineID()
        {
            try
            {
                DataTable dtDesg = datatrans.GetMachine();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["MCODE"].ToString(), Value = dtDesg.Rows[i]["MACHINEINFOBASICID"].ToString() });
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
                DataTable dtDesg = Pyro.ShiftDeatils();
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
        public JsonResult GetStockDrumBatchJSON(string ItemId, string loc, string item)
        {
            //EnqItem model = new EnqItem();
            //model.Itemlst = BindItemlst(itemid);
            return Json(BindDrumBatch(ItemId, loc, item));
        }
        public List<SelectListItem> BindDrumBatch(string ItemId, string loc, string item)
        {
            try
            {
                DataTable dtDesg = Pyro.GetDrumBatch(ItemId, loc, item);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOTNO"].ToString(), Value = dtDesg.Rows[i]["LOTNO"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetBatchqty(string ItemId)
        {
            try
            {

                DataTable dt1 = new DataTable();

                string stk = "";

                dt1 = datatrans.GetData("SELECT SUM(S.PLUSQTY-S.MINUSQTY) as QTY  FROM LSTOCKVALUE S WHERE LOTNO='" + ItemId + "' HAVING SUM(S.PLUSQTY-S.MINUSQTY) > 0 ");
                if (dt1.Rows.Count > 0)
                {
                    stk = dt1.Rows[0]["QTY"].ToString();
                }
                if (stk == "")
                {
                    stk = "0";
                }
                var result = new { stk = stk };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindProdSch(string value)
        {
            try
            {
                DataTable dtDesg = Pyro.GetProdSch(value);
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
        public List<SelectListItem> BindPLot(string procid, string wcid)
        {
            try
            {
                DataTable dtDesg = datatrans.GetPLot(procid, wcid);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PROCLOTNO"].ToString(), Value = dtDesg.Rows[i]["PROCLOTID"].ToString() });
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
                DataTable dtDesg = datatrans.BindProcess();
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
        public List<SelectListItem> BindWork(string id)
        {
            try
            {
                DataTable dtDesg = Pyro.GetWork();
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

        public List<SelectListItem> BindWorkedit(string id)
        {
            try
            {
                DataTable dtDesg = Pyro.GetWorkedit(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOCID"].ToString(), Value = dtDesg.Rows[i]["LOCATIONNAME"].ToString() });
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

                string bin = "";
                string binid = "";
                dt = Pyro.GetItemDetails(ItemId);

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
        public ActionResult GetStockDetail(string ItemId, string item)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string stock = "";

                dt = Pyro.GetStockDetails(ItemId, item);

                if (dt.Rows.Count > 0)
                {

                    stock = dt.Rows[0]["qty"].ToString();

                }

                var result = new { stock = stock };
                return Json(result);
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
                DataTable dtDesg = Pyro.GetReason();
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
        public JsonResult GetItemJSON(string id)
        {
            //EnqItem model = new EnqItem();
            //model.Itemlst = BindItemlst(itemid);
            return Json(BindInputItemlst(id));
        }
        public JsonResult GetconsItemJSON(string id)
        {
            return Json(BindItemlstCon(id));
        }
        public JsonResult GetOutItemJSON(string id)
        {
            //EnqItem model = new EnqItem();
            //model.Itemlst = BindItemlst(itemid);
            return Json(BindOutItemlst(id));
        }
        public JsonResult GetEmpJSON()
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindEmp());
        }
        public JsonResult GetDrumJSON()
        {
            return Json(BindDrum());
        }
        //public JsonResult GetDrumIdJSON(string item)
        //{
        //    //PProInput model = new PProInput();
        //    //model.drumlst = BindDrum(item);
        //    return Json(BindDrum(item));
        //}
        public JsonResult GetReasonJSON()
        {
            return Json(BindReason());
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
        public JsonResult GetShedJSON()
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindShed());
        }
        public ActionResult SaveOutDetail(string schno, string docid, string docdate, string loc, string proc, string shift, string schqty, string prodqty, string wcid, string proclot, string brid,string eng)
        {
            try
            {
                string pid = "0";
                pid = Pyro.SaveBasicDetail(schno, docid, docdate, loc, proc, shift, schqty, prodqty, wcid, proclot, brid,eng);
                var result = new { pid = pid };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetWcRec(string wcid, string shift)
        {
            string res = "Yes";
            string id = datatrans.GetDataString("select PYROPRODBASICID from PYROPRODBASIC WHERE LOCID='" + wcid + "' AND IS_COMPLETE='No' AND SHIFT='" + shift + "'");
            if (id == null || id == "0" || id == "")
            {
                res = "No";
            }
            var result = new { url = id, res = res };
            return Json(result);
        }
        public ActionResult GetOutItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string bin = "";
                string binid = "";
                dt = Pyro.GetOutItemDetails(ItemId);

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
        public ActionResult GetConItemDetail(string ItemId,string loc)
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

                dt = Pyro.GetConItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    bin = dt.Rows[0]["BINID"].ToString();
                    binid = dt.Rows[0]["bin"].ToString();
                    unit = dt.Rows[0]["UNITID"].ToString();
                    unitid = dt.Rows[0]["unit"].ToString();

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
                var result = new { bin = bin, binid = binid, unit = unit, unitid = unitid, stk= stk };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult ListPyroProduction()
        {

            //HttpContext.Session.SetString("SalesStatus", "Y");
            //IEnumerable<PyroProduction> cmp = Pyro.GetAllPyro();
            return View();
        }
        public IActionResult ViewPyroProduction(string id)
        {
            PyroProductionentryDet ca = new PyroProductionentryDet();
            DataTable dt = new DataTable();
            dt = Pyro.GetPyroProductionName(id);
            if (dt.Rows.Count > 0)
            {
                ca.Location = dt.Rows[0]["WORKID"].ToString();
                ca.Locationid = dt.Rows[0]["ILOCDETAILSID"].ToString();
                ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                ca.DocId = dt.Rows[0]["DOCID"].ToString();
                ca.Eng = dt.Rows[0]["ENTEREDBY"].ToString();
                ca.Shift = dt.Rows[0]["SHIFT"].ToString();
                ViewBag.shift = dt.Rows[0]["SHIFT"].ToString();
                ca.ProcessLot = dt.Rows[0]["PROCLOTNO"].ToString();
                ca.process = dt.Rows[0]["PROCESS"].ToString();
                ca.ProcessId = dt.Rows[0]["PROCESSID"].ToString();
                ca.ProdQty = dt.Rows[0]["PRODQTY"].ToString();
                ca.SchQty = dt.Rows[0]["SCHQTY"].ToString();
                ca.ProdSchNo = dt.Rows[0]["psno"].ToString();
                ca.ProdSchid = dt.Rows[0]["PSCHNO"].ToString();
                ca.APID = id;
                ca.workid = dt.Rows[0]["WCID"].ToString();

                ca.ID = id;
            }
            DataTable dt2 = new DataTable();
            List<PProInput> TData = new List<PProInput>();
            PProInput tda = new PProInput();
            dt2 = Pyro.GetInputDeatils(id);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new PProInput();
                
                    tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                    //tda.unit = dt2.Rows[i]["UNITID"].ToString();
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
            List<PAPProInCons> TData1 = new List<PAPProInCons>();
            PAPProInCons tda1 = new PAPProInCons();
            dt3 = Pyro.GetConsDeatils(id);
            if (dt3.Rows.Count > 0)
            {
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    tda1 = new PAPProInCons();
                    tda1.ItemId = dt3.Rows[i]["ITEMID"].ToString();
                    tda1.consunit = dt3.Rows[i]["CUNIT"].ToString();
                    tda1.Qty = Convert.ToDouble(dt3.Rows[i]["CSUBQTY"].ToString() == "" ? "0" : dt3.Rows[i]["CSUBQTY"].ToString());
                    tda1.consQty = Convert.ToDouble(dt3.Rows[i]["CONSQTY"].ToString() == "" ? "0" : dt3.Rows[i]["CONSQTY"].ToString());
                    DataTable dtstk = datatrans.GetData("SELECT SUM(DECODE(s.PLUSORMINUS,'p',S.QTY,-S.QTY)) as QTY FROM STOCKVALUE S WHERE ITEMID='" + tda1.ItemId + "' and LOCID='" + ca.LOCID + "'");
                    if (dtstk.Rows.Count > 0)
                    {
                        tda1.ConsStock = Convert.ToDouble(dtstk.Rows[0]["QTY"].ToString() == "" ? "0" : dtstk.Rows[0]["QTY"].ToString());
                    }
                    tda1.APID = id;
                    tda1.Isvalid = "Y";
                    TData1.Add(tda1);
                }

            }

            DataTable dt4 = new DataTable();
            List<PEmpDetails> TTData2 = new List<PEmpDetails>();
            PEmpDetails tda2 = new PEmpDetails();
            dt4 = Pyro.GetEmpdetDeatils(id);
            if (dt4.Rows.Count > 0)
            {
                for (int i = 0; i < dt4.Rows.Count; i++)
                {
                    tda2 = new PEmpDetails();
                    tda2.Employeelst = BindEmp();
                    tda2.Employee = dt4.Rows[i]["EMPNAME"].ToString();

                    tda2.EmpCode = dt4.Rows[i]["EMPCODE1"].ToString();
                    tda2.Depart = dt4.Rows[i]["DEPARTMENT"].ToString();
                    tda2.StartDate = dt4.Rows[i]["ESTARTDATE"].ToString();
                    tda2.StartTime = dt4.Rows[i]["ESTARTTIME"].ToString();
                    tda2.EndDate = dt4.Rows[i]["EENDDATE"].ToString();
                    tda2.EndTime = dt4.Rows[i]["EENDTIME"].ToString();
                    tda2.OTHrs = dt4.Rows[i]["OTHRS"].ToString();

                    tda2.ETOther = dt4.Rows[i]["ETOTHRS"].ToString();
                    tda2.Normal = dt4.Rows[i]["NORMHRS"].ToString();
                    tda2.NOW = dt4.Rows[i]["NATOFW"].ToString();
                    tda2.ID = id;
                    tda2.Isvalid = "Y";
                    TTData2.Add(tda2);

                }

            }
            DataTable dt5 = new DataTable();
            List<PBreakDet> TData3 = new List<PBreakDet>();
            PBreakDet tda3 = new PBreakDet();
            dt5 = Pyro.GetBreakDeatils(id);
            if (dt5.Rows.Count > 0)
            {
                for (int i = 0; i < dt5.Rows.Count; i++)
                {
                    tda3 = new PBreakDet();
                    tda3.Machinelst = BindMachineID();
                    tda3.MachineId = dt5.Rows[i]["MCODE"].ToString();
                    tda3.Emplst = BindEmp();
                    tda3.MachineDes = dt5.Rows[i]["BMACHINEDESC"].ToString();
                    tda3.StartTime = dt5.Rows[i]["BFROMTIME"].ToString();
                    tda3.EndTime = dt5.Rows[i]["BTOTIME"].ToString();
                    tda3.PB = dt5.Rows[i]["PREORBRE"].ToString();
                    tda3.Isvalid = "Y";
                    tda3.Alloted = dt5.Rows[i]["ALLOTEDTO"].ToString();
                    tda3.DType = dt5.Rows[i]["DTYPE"].ToString();
                    tda3.MType = dt5.Rows[i]["MTYPE"].ToString();
                    tda3.Reason = dt5.Rows[i]["ACTDESC"].ToString();

                    tda3.APID = id;
                    TData3.Add(tda3);
                }

            }
            DataTable dt6 = new DataTable();
            List<PProOutput> TData4 = new List<PProOutput>();
            PProOutput tda4 = new PProOutput();
            dt6 = Pyro.GetOutputDeatils(id);
            if (dt6.Rows.Count > 0)
            {
                for (int i = 0; i < dt6.Rows.Count; i++)
                {
                    tda4 = new PProOutput();
                    
                    tda4.ItemId = dt6.Rows[i]["ITEMID"].ToString();

                    tda4.drumlst = BindDrum();
                    tda4.statuslst = BindStatus();
                    tda4.drumno = dt6.Rows[i]["DRUMNO"].ToString();
                   // tda4.unit = dt6.Rows[i]["UNITID"].ToString();
                    tda4.FromTime = dt6.Rows[i]["STIME"].ToString();
                    tda4.ToTime = dt6.Rows[i]["ETIME"].ToString();
                    tda4.ShedNo = dt6.Rows[i]["SHEDNUMBER"].ToString();
                    tda4.Status = dt6.Rows[i]["STATUS"].ToString();
                    tda4.OutputQty = Convert.ToDouble(dt6.Rows[i]["OQTY"].ToString() == "" ? "0" : dt6.Rows[i]["OQTY"].ToString());
                    tda4.ExcessQty = Convert.ToDouble(dt6.Rows[i]["OXQTY"].ToString() == "" ? "0" : dt6.Rows[i]["OXQTY"].ToString());
                    //DataTable dt7 = new DataTable();
                    //dt7 = IProductionEntry.GetResult(id);
                    //if (dt7.Rows.Count > 0)
                    //{
                    //    tda4.Result = dt7.Rows[i]["TESTRESULT"].ToString();
                    //    tda4.Status = dt7.Rows[i]["MOVETOQC"].ToString();
                    //}
                    tda4.APID = id;
                    tda4.Isvalid = "Y";
                    TData4.Add(tda4);

                }

            }
            DataTable adt7 = new DataTable();
            List<PLogDetails> TTData5 = new List<PLogDetails>();
            PLogDetails tda5 = new PLogDetails();
            adt7 = Pyro.GetLogdetailDeatils(id);
            if (adt7.Rows.Count > 0)
            {
                for (int i = 0; i < adt7.Rows.Count; i++)
                {
                    tda5 = new PLogDetails();

                    tda5.StartDate = adt7.Rows[i]["STARTDATE"].ToString();
                    tda5.StartTime = adt7.Rows[i]["STARTTIME"].ToString();

                    tda5.EndDate = adt7.Rows[i]["ENDDATE"].ToString();

                    tda5.Reason = adt7.Rows[i]["REASON"].ToString();



                    tda5.EndTime = adt7.Rows[i]["ENDTIME"].ToString();
                    tda5.tothrs = adt7.Rows[i]["TOTALHRS"].ToString();

                    tda5.APID = id;
                    tda5.Isvalid = "Y";
                    TTData5.Add(tda5);
                   
                }

            }
            List<PSourcingDetail> TData6 = new List<PSourcingDetail>();
            PSourcingDetail tda6 = new PSourcingDetail();
            DataTable adt8 = new DataTable();

            adt8 = Pyro.GetOutsdetail(id);
            if (adt8.Rows.Count > 0)
            {
                for (int i = 0; i < adt8.Rows.Count; i++)
                {
                    tda6 = new PSourcingDetail();

                    tda6.NoOfEmp = adt8.Rows[i]["NOOFEMP"].ToString();
                    tda6.StartDate = adt8.Rows[i]["OWSTDTT"].ToString();

                    tda6.StartTime = adt8.Rows[i]["OWSTT"].ToString();
                    tda6.EndDate = adt8.Rows[i]["OWEDDTT"].ToString();
                    tda6.EndTime = adt8.Rows[i]["OWEDT"].ToString();
                    tda6.Expence = Convert.ToDouble(adt8.Rows[i]["MANPOWEXP"].ToString() == "" ? "0" : adt8.Rows[i]["MANPOWEXP"].ToString());
                    tda6.WorkHrs = adt8.Rows[i]["EMPWHRS"].ToString();
                    tda6.EmpCost = Convert.ToDouble(adt8.Rows[i]["EMPPAY"].ToString() == "" ? "0" : adt8.Rows[i]["EMPPAY"].ToString());
                    tda6.NOW = adt8.Rows[i]["ONATOFW"].ToString();

                    tda6.APID = id;
                    tda6.Isvalid = "Y";
                    TData6.Add(tda6);
                   
                }

            }
            ca.BreakLst = TData3;
            ca.inplst = TData;
            ca.outlst = TData4;
            ca.EmplLst = TTData2;
            ca.Binconslst = TData1;
            ca.LogLst = TTData5;
            ca.SourcingLst = TData6;
            return View(ca);

        }
        public IActionResult ApprovePyroProduction(string id)
        {
            PyroProductionentryDet ca = new PyroProductionentryDet();
            DataTable dt = new DataTable();
            ca.Branch = Request.Cookies["BranchId"];
            dt = Pyro.GetPyroProductionName(id);
            if (dt.Rows.Count > 0)
            {
                ca.Location = dt.Rows[0]["WORKID"].ToString();
                ca.Locationid = dt.Rows[0]["ILOCDETAILSID"].ToString();
                ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                ca.DocId = dt.Rows[0]["DOCID"].ToString();
                ca.Eng = dt.Rows[0]["ENTEREDBY"].ToString();
                ca.Shift = dt.Rows[0]["SHIFT"].ToString();
                ViewBag.shift = dt.Rows[0]["SHIFT"].ToString();
                ca.ProcessLot = dt.Rows[0]["PROCLOTNO"].ToString();
                ca.process = dt.Rows[0]["PROCESS"].ToString();
                ca.ProcessId = dt.Rows[0]["PROCESSID"].ToString();
                ca.ProdQty = dt.Rows[0]["PRODQTY"].ToString();
                ca.SchQty = dt.Rows[0]["SCHQTY"].ToString();
                ca.ProdSchNo = dt.Rows[0]["psno"].ToString();
                ca.ProdSchid = dt.Rows[0]["PSCHNO"].ToString();
                ca.APID = id;
                ca.workid = dt.Rows[0]["WCID"].ToString();

                ca.ID = id;
            }
            DataTable dt2 = new DataTable();
            List<PProInput> TData = new List<PProInput>();
            PProInput tda = new PProInput();
            dt2 = Pyro.GetInputDeatils(id);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new PProInput();
                    tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                    //tda.unit = dt2.Rows[i]["UNITID"].ToString();
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
            List<PAPProInCons> TData1 = new List<PAPProInCons>();
            PAPProInCons tda1 = new PAPProInCons();
            dt3 = Pyro.GetConsDeatils(id);
            if (dt3.Rows.Count > 0)
            {
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    tda1 = new PAPProInCons();
                    tda1.ItemId = dt3.Rows[i]["ITEMID"].ToString();
                    tda1.consunit = dt3.Rows[i]["CUNIT"].ToString();
                    tda1.Qty = Convert.ToDouble(dt3.Rows[i]["CSUBQTY"].ToString() == "" ? "0" : dt3.Rows[i]["CSUBQTY"].ToString());
                    tda1.consQty = Convert.ToDouble(dt3.Rows[i]["CONSQTY"].ToString() == "" ? "0" : dt3.Rows[i]["CONSQTY"].ToString());
                    DataTable dtstk = datatrans.GetData("SELECT SUM(DECODE(s.PLUSORMINUS,'p',S.QTY,-S.QTY)) as QTY FROM STOCKVALUE S WHERE ITEMID='" + tda1.ItemId + "' and LOCID='" + ca.LOCID + "'");
                    if (dtstk.Rows.Count > 0)
                    {
                        tda1.ConsStock = Convert.ToDouble(dtstk.Rows[0]["QTY"].ToString() == "" ? "0" : dtstk.Rows[0]["QTY"].ToString());
                    }
                    tda1.APID = id;
                    tda1.Isvalid = "Y";
                    TData1.Add(tda1);
                }

            }


            DataTable dt6 = new DataTable();
            List<PProOutput> TData4 = new List<PProOutput>();
            PProOutput tda4 = new PProOutput();
            dt6 = Pyro.GetOutputDeatils(id);
            if (dt6.Rows.Count > 0)
            {
                for (int i = 0; i < dt6.Rows.Count; i++)
                {
                    tda4 = new PProOutput();
                    tda4.ItemId = dt6.Rows[i]["ITEMID"].ToString();

                    tda4.drumlst = BindDrum();
                    tda4.statuslst = BindStatus();
                    tda4.drumno = dt6.Rows[i]["DRUMNO"].ToString();
                    // tda4.unit = dt6.Rows[i]["UNITID"].ToString();
                    tda4.FromTime = dt6.Rows[i]["STIME"].ToString();
                    tda4.ToTime = dt6.Rows[i]["ETIME"].ToString();
                    tda4.ShedNo = dt6.Rows[i]["SHEDNUMBER"].ToString();
                    tda4.Status = dt6.Rows[i]["STATUS"].ToString();
                    tda4.OutputQty = Convert.ToDouble(dt6.Rows[i]["OQTY"].ToString() == "" ? "0" : dt6.Rows[i]["OQTY"].ToString());
                    tda4.ExcessQty = Convert.ToDouble(dt6.Rows[i]["OXQTY"].ToString() == "" ? "0" : dt6.Rows[i]["OXQTY"].ToString());
                    //DataTable dt7 = new DataTable();
                    //dt7 = IProductionEntry.GetResult(id);
                    //if (dt7.Rows.Count > 0)
                    //{
                    //    tda4.Result = dt7.Rows[i]["TESTRESULT"].ToString();
                    //    tda4.Status = dt7.Rows[i]["MOVETOQC"].ToString();
                    //}
                    tda4.APID = id;
                    tda4.shedlst = BindShed();
                    tda4.Isvalid = "Y";
                    TData4.Add(tda4);
                    
                     

                }

            }


            ca.inplst = TData;
            ca.outlst = TData4;

            ca.Binconslst = TData1;

            return View(ca);

        }
        public List<SelectListItem> BindShed()
        {
            try
            {
                DataTable dtDesg = Pyro.GetShedNo();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SHEDNUMBER"].ToString(), Value = dtDesg.Rows[i]["SHEDNUMBER"].ToString() });

                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult ApprovePyroProduction(PyroProductionentryDet Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = Pyro.ApprovePyroProductionEntryGURD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "ApprovePyroProduction Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "ApprovePyroProduction Updated Successfully...!";
                    }
                    return RedirectToAction("ListPyroProduction");
                }

                else
                {
                    ViewBag.PageTitle = "Edit ApprovePyroProduction";
                    TempData["notice"] = Strout;
                    //return View();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Cy);
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

                    dt = Pyro.SaveOutsDetails(id, noofemp, sdate, stime, edate, etime, workhrs, cost, expence, now);
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
        public ActionResult PyroProductionentryDetail(string id, string tag)
        {
            PyroProductionentryDet ca = new PyroProductionentryDet();
            //ca.Complete = "No";
            ca.Eng = Request.Cookies["UserName"];
            ca.super = Request.Cookies["UserId"];
            ca.Branch = Request.Cookies["BranchId"];
            ca.Shiftlst = BindShift();
            ca.Wclst = BindWorkedit(ca.super);
            List<PBreakDet> TData3 = new List<PBreakDet>();
            PBreakDet tda3 = new PBreakDet();
            List<PProInput> TData = new List<PProInput>();
            PProInput tda = new PProInput();
            List<PProOutput> TData4 = new List<PProOutput>();
            PProOutput tda4 = new PProOutput();
            List<PAPProInCons> TData1 = new List<PAPProInCons>();
            PAPProInCons tda1 = new PAPProInCons();
            List<PEmpDetails> TTData2 = new List<PEmpDetails>();
            PEmpDetails tda2 = new PEmpDetails();
            List<PLogDetails> TTData5 = new List<PLogDetails>();
            PLogDetails tda5 = new PLogDetails();
            if (tag == "2" || tag == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda3 = new PBreakDet();

                    tda3.Machinelst = BindMachineID();
                    tda3.Emplst = BindEmp();
                    tda3.Isvalid = "Y";
                    tda3.APID = id;
                    TData3.Add(tda3);

                }
                for (int i = 0; i < 3; i++)
                {
                    tda = new PProInput();
                    tda.APID = id;
                    tda.Itemlst = BindInputItemlst("");
                    tda.drumlst = BindDrum("","");
                    tda.Isvalid = "Y";
                    TData.Add(tda);

                }
                for (int i = 0; i < 1; i++)
                {
                    tda4 = new PProOutput();
                    tda4.APID = id;
                    tda4.Itemlst = BindOutItemlst("");
                    tda4.drumlst = BindDrum();
                    tda4.Isvalid = "Y";
                    TData4.Add(tda4);

                }
                for (int i = 0; i < 1; i++)
                {
                    tda1 = new PAPProInCons();
                    tda1.Itemlst = BindItemlstCon("");
                    tda1.Isvalid = "Y";
                    tda1.APID = id;
                    TData1.Add(tda1);
                }
                for (int i = 0; i < 3; i++)
                {
                    tda2 = new PEmpDetails();
                    tda2.APID = id;
                    tda2.Employeelst = BindEmp();
                    tda2.Isvalid = "Y";
                    TTData2.Add(tda2);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda5 = new PLogDetails();
                    tda5.APID = id;
                    tda5.Isvalid = "Y";
                    TTData5.Add(tda5);


                }
            }
            if (!string.IsNullOrEmpty(id))
            {


                DataTable dt = new DataTable();

                dt = Pyro.GetPyroProd(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Location = dt.Rows[0]["LOCID"].ToString();
                    ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    ca.Eng = dt.Rows[0]["EMPNAME"].ToString();
                    ca.Shift = dt.Rows[0]["SHIFT"].ToString();
                    ViewBag.shift = dt.Rows[0]["SHIFT"].ToString();

                    ca.APID = id;
                }
                //    DataTable dt2 = new DataTable();

                //    dt2 = Pyro.GetInput(id);
                //    if (dt2.Rows.Count > 0)
                //    {
                //        for (int i = 0; i < dt2.Rows.Count; i++)
                //        {
                //            tda = new PProInput();
                //            tda.Itemlst = BindItemlst();
                //            tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                //            tda.Time = dt2.Rows[i]["CHARGINGTIME"].ToString();
                //            tda.BinId = dt2.Rows[i]["BINID"].ToString();
                //            tda.batchno = dt2.Rows[i]["BATCHNO"].ToString();
                //            tda.IssueQty = Convert.ToDouble(dt2.Rows[i]["QTY"].ToString() == "" ? "0" : dt2.Rows[i]["QTY"].ToString());
                //            tda.StockAvailable = Convert.ToDouble(dt2.Rows[i]["STOCK"].ToString() == "" ? "0" : dt2.Rows[i]["STOCK"].ToString());
                //            tda.APID = id;
                //            tda.Isvalid = "Y";
                //            TData.Add(tda);

                //        }

                //    }
                //    DataTable dt3 = new DataTable();
                //    dt3 = Pyro.GetCons(id);
                //    if (dt3.Rows.Count > 0)
                //    {
                //        for (int i = 0; i < dt3.Rows.Count; i++)
                //        {
                //            tda1 = new PAPProInCons();
                //            tda1.Itemlst = BindItemlstCon();
                //            tda1.ItemId = dt3.Rows[i]["ITEMID"].ToString();
                //            tda1.consunit = dt3.Rows[i]["UNITID"].ToString();
                //            tda1.BinId = dt3.Rows[i]["BINID"].ToString();
                //            tda1.Qty = Convert.ToDouble(dt3.Rows[i]["QTY"].ToString() == "" ? "0" : dt3.Rows[i]["QTY"].ToString());
                //            tda1.consQty = Convert.ToDouble(dt3.Rows[i]["CONSQTY"].ToString() == "" ? "0" : dt3.Rows[i]["CONSQTY"].ToString());
                //            tda1.ConsStock = Convert.ToDouble(dt3.Rows[i]["STOCK"].ToString() == "" ? "0" : dt3.Rows[i]["STOCK"].ToString());

                //            tda1.APID = id;
                //            tda1.Isvalid = "Y";
                //            TData1.Add(tda1);
                //        }

                //    }

                //    DataTable dt4 = new DataTable();
                //    dt4 = Pyro.GetEmpdet(id);
                //    if (dt4.Rows.Count > 0)
                //    {
                //        for (int i = 0; i < dt4.Rows.Count; i++)
                //        {
                //            tda2 = new PEmpDetails();
                //            tda2.Employeelst = BindEmp();
                //            tda2.Employee = dt4.Rows[i]["EMPID"].ToString();

                //            tda2.EmpCode = dt4.Rows[i]["EMPCODE"].ToString();
                //            tda2.Depart = dt4.Rows[i]["DEPARTMENT"].ToString();
                //            tda2.StartDate = dt4.Rows[i]["STARTDATE"].ToString();
                //            tda2.StartTime = dt4.Rows[i]["STARTTIME"].ToString();
                //            tda2.EndDate = dt4.Rows[i]["ENDDATE"].ToString();
                //            tda2.EndTime = dt4.Rows[i]["ENDTIME"].ToString();
                //            tda2.OTHrs = dt4.Rows[i]["OTHOUR"].ToString();

                //            tda2.ETOther = dt4.Rows[i]["ETOTHER"].ToString();
                //            tda2.Normal = dt4.Rows[i]["NHOUR"].ToString();
                //            tda2.NOW = dt4.Rows[i]["NATUREOFWORK"].ToString();
                //            tda2.ID = id;
                //            tda2.Isvalid = "Y";
                //            TTData2.Add(tda2);

                //        }

                //    }
                //    DataTable dt5 = new DataTable();
                //    dt5 = Pyro.GetBreak(id);
                //    if (dt5.Rows.Count > 0)
                //    {
                //        for (int i = 0; i < dt5.Rows.Count; i++)
                //        {
                //            tda3 = new PBreakDet();
                //            tda3.Machinelst = BindMachineID();
                //            tda3.MachineId = dt5.Rows[i]["MACHCODE"].ToString();
                //            tda3.Emplst = BindEmp();
                //            tda3.MachineDes = dt5.Rows[i]["DESCRIPTION"].ToString();
                //            tda3.StartTime = dt5.Rows[i]["FROMTIME"].ToString();
                //            tda3.EndTime = dt5.Rows[i]["TOTIME"].ToString();
                //            tda3.PB = dt5.Rows[i]["PB"].ToString();
                //            tda3.Isvalid = "Y";
                //            tda3.Alloted = dt5.Rows[i]["ALLOTTEDTO"].ToString();
                //            tda3.DType = dt5.Rows[i]["DTYPE"].ToString();
                //            tda3.MType = dt5.Rows[i]["MTYPE"].ToString();
                //            tda3.Reason = dt5.Rows[i]["REASON"].ToString();

                //            tda3.APID = id;
                //            TData3.Add(tda3);
                //        }

                //    }
                //    DataTable dt6 = new DataTable();

                //    dt6 = Pyro.GetOutput(id);
                //    if (dt6.Rows.Count > 0)
                //    {
                //        for (int i = 0; i < dt6.Rows.Count; i++)
                //        {
                //            tda4 = new PProOutput();
                //            tda4.Itemlst = BindOutItemlst();
                //            tda4.ItemId = dt6.Rows[i]["ITEMID"].ToString();
                //            tda4.BinId = dt6.Rows[i]["BINID"].ToString();
                //            tda4.drumlst = BindDrum();
                //            tda4.drumno = dt6.Rows[i]["DRUMNO"].ToString();
                //            tda4.FromTime = dt6.Rows[i]["FROMTIME"].ToString();
                //            tda4.ToTime = dt6.Rows[i]["TOTIME"].ToString();
                //            tda4.OutputQty = Convert.ToDouble(dt6.Rows[i]["OUTQTY"].ToString() == "" ? "0" : dt6.Rows[i]["OUTQTY"].ToString());
                //            DataTable dt7 = new DataTable();
                //            //dt7 = Pyro.GetResult(id);
                //            //if (dt7.Rows.Count > 0)
                //            //{
                //            //    tda4.Result = dt7.Rows[i]["TESTRESULT"].ToString();
                //            //    tda4.Status = dt7.Rows[i]["MOVETOQC"].ToString();
                //            //}
                //            tda4.APID = id;
                //            tda4.Isvalid = "Y";
                //            TData4.Add(tda4);

                //        }

                //    }
                //    DataTable adt7 = new DataTable();

                //    adt7 = Pyro.GetLogdetail(id);
                //    if (adt7.Rows.Count > 0)
                //    {
                //        for (int i = 0; i < dt6.Rows.Count; i++)
                //        {
                //            tda5 = new PLogDetails();

                //            tda5.StartDate = adt7.Rows[i]["STARTDATE"].ToString();
                //            tda5.StartTime = adt7.Rows[i]["STARTTIME"].ToString();

                //            tda5.EndDate = adt7.Rows[i]["ENDDATE"].ToString();

                //            tda5.Reason = adt7.Rows[i]["REASON"].ToString();



                //            tda5.EndTime = adt7.Rows[i]["ENDTIME"].ToString();
                //            tda5.tothrs = adt7.Rows[i]["TOTALHRS"].ToString();

                //            tda5.APID = id;
                //            TTData5.Add(tda5);
                //            tda5.Isvalid = "Y";
                //        }

                //    }
            }



            ca.BreakLst = TData3;
            ca.inplst = TData;
            ca.outlst = TData4;
            ca.EmplLst = TTData2;
            ca.Binconslst = TData1;
            ca.LogLst = TTData5;

            return View(ca);
        }
        public ActionResult PyroProdApprove(string id)
        {
            PyroProductionentryDet ca = new PyroProductionentryDet();
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
        public ActionResult Getsch(string schid)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                DataTable dt3 = new DataTable();

                dt = datatrans.GetData("select  PROCESSID from  WCBASIC  where WCBASICID ='" + schid + "'");
                //dt1 = datatrans.GetData("select SUM(RQTY) as qty from PSINPDETAIL WHERE PSBASICID='" + schid + "'");

                string work = "";
                string workid = "";
                string schqty = "";
                string prodqty = "";
                string proc = "";
                if (dt.Rows.Count > 0)
                {
                    //work = dt.Rows[0]["WCID"].ToString();
                    //workid = dt.Rows[0]["WCBASICID"].ToString();
                    //schqty = dt1.Rows[0]["qty"].ToString();
                    //prodqty = dt1.Rows[0]["qty"].ToString();
                    proc = dt.Rows[0]["PROCESSID"].ToString();
                }

                var result = new { /*work = work, workid = workid, schqty = schqty, prodqty = prodqty,*/ proc = proc };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Getschqty(string schid)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                DataTable dt3 = new DataTable();

                dt = datatrans.GetData("select  OPQTY,PRODQTY from  PSBASIC  where PSBASICID ='" + schid + "'");
                //dt1 = datatrans.GetData("select SUM(RQTY) as qty from PSINPDETAIL WHERE PSBASICID='" + schid + "'");

                
                string schqty = "";
                string prodqty = "";
              
                if (dt.Rows.Count > 0)
                {
                     
                    schqty = dt.Rows[0]["OPQTY"].ToString();
                    prodqty = dt.Rows[0]["PRODQTY"].ToString();
                     
                }

                var result = new {  schqty = schqty, prodqty = prodqty  };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult PyroProdApprove(PyroProductionentryDet Cy, string id)
        {
            if (Cy.change != "Complete")
            {
                try
                {
                    Cy.ID = id;
                    string Strout = Pyro.PyroProEntryCRUD(Cy);
                    if (string.IsNullOrEmpty(Strout))
                    {
                        return RedirectToAction("PyroProductionentryDetail", new { id = Cy.APID });
                    }

                    else
                    {
                        ViewBag.PageTitle = "Edit PyroProductionentryDetail";
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
                return RedirectToAction("ListPyroProduction");
            }
            return View(Cy);
        }
        public ActionResult InsertProInput([FromBody] PProInput[] model)
        {
            try
            {
                int r = 1;
                foreach (PProInput input in model)
                {

                    string item = input.ItemId;
                    string bin = input.BinId;
                    string batch = input.batchno;
                    string time = input.Time;
                    string id = input.APID;
                    string drum = input.drumno;
                    string stock = input.StockAvailable.ToString();
                    string qty = input.IssueQty.ToString();
                    DataTable dt = new DataTable();
                    DataTable insert = datatrans.GetData("SELECT NPRODINPDETID,IS_INSERT,NPRODINPDETROW FROM NPRODINPDET WHERE NPRODBASICID='" + id + "' and ICDRUMNO='"+drum+"' and IS_INSERT='Y'");

                    if (insert.Rows.Count > 0)
                    {
                        r = (int)Convert.ToDouble(insert.Rows[0]["NPRODINPDETROW"].ToString());
                        r++;
                    }
                    else
                    {
                        dt = Pyro.SaveInputDetails(id, item, bin, time, qty, stock, batch, drum, r);
                        r++;
                    }
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
        public ActionResult InsertConsInput([FromBody] PAPProInCons[] model)
        {
            try
            {
                int l = 1;
                foreach (PAPProInCons Cons in model)
                {

                    string item = Cons.ItemId;
                    string bin = Cons.BinId;
                    string unit = Cons.consunit;
                    string qty = Cons.consQty.ToString();
                    string id = Cons.APID;
                    string stock = Cons.ConsStock.ToString();
                    string usedqty = Cons.Qty.ToString();
                    DataTable dt = new DataTable();
                    DataTable insert = datatrans.GetData("SELECT NPRODCONSDETID,IS_INSERT,NPRODCONSDETROW FROM NPRODCONSDET WHERE NPRODBASICID='" + id + "'   and IS_INSERT='Y'");
                    if (insert.Rows.Count > 0)
                    {

                        l = (int)Convert.ToDouble(insert.Rows[0]["NPRODCONSDETROW"].ToString());
                        l++;

                    }
                    else
                    {
                        dt = Pyro.SaveConsDetails(id, item, bin, unit, usedqty, qty, stock, l);
                        l++;
                    }
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
        public ActionResult InsertProOut(string id, string ItemId, string drum, string time, string qty, string totime, string exqty, string stat, string stock, string ShedNo)
        {
            try
            {
                 
                    DataTable dt = new DataTable();
                    DataTable dt2 = new DataTable();

                    dt = Pyro.SaveOutputDetails(id, ItemId, time, totime, qty, drum, stat, stock, exqty, ShedNo);
                dt2 = datatrans.GetData("SELECT OCCUPIED,CAPACITY FROM CURINGMASTER WHERE SHEDNUMBER='"+ ShedNo +"' ");


                string occ = dt2.Rows[0]["OCCUPIED"].ToString();
                string cap = dt2.Rows[0]["CAPACITY"].ToString();
                var result = new { occ = occ , cap = cap };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult InsertProEmp([FromBody] PEmpDetails[] model)
        {
            try
            {
                foreach (PEmpDetails emp in model)
                {

                    string empname = emp.Employee;
                    string code = emp.EmpCode;
                    string depat = emp.Depart;
                    string sdate = emp.StartDate;
                    string id = emp.APID;
                    string stime = emp.StartTime;
                    string edate = emp.EndDate;
                    string etime = emp.EndTime;
                    string ot = emp.OTHrs;
                    string et = emp.ETOther;
                    string normal = emp.Normal;
                    string now = emp.NOW;
                    DataTable dt = new DataTable();

                    dt = Pyro.SaveEmpDetails(id, empname, code, depat, sdate, stime, edate, etime, ot, et, normal, now);
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
        public ActionResult InsertProLog([FromBody] PLogDetails[] model)
        {
            try
            {
                foreach (PLogDetails log in model)
                {


                    string sdate = log.StartDate;
                    string id = log.APID;
                    string stime = log.StartTime;
                    string edate = log.EndDate;
                    string etime = log.EndTime;
                    string tot = log.tothrs;
                    string reason = log.Reason;

                    DataTable dt = new DataTable();

                    dt = Pyro.SaveLogDetails(id, sdate, stime, edate, etime, tot, reason);
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
        public ActionResult InsertProBreak([FromBody] PBreakDet[] model)
        {
            try
            {
                foreach (PBreakDet det in model)
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

                    dt = Pyro.SaveBreakDetails(id, machine, des, dtype, mtype, stime, etime, pb, all, reason);
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
        public ActionResult Getshedocc(string item)
        {
            try
            {



                string shed = "";
                string cap = "";


                DataTable dt = new DataTable();

                dt = Pyro.CuringsetDetails(item);


                if (dt.Rows.Count > 0)
                {

                    shed = dt.Rows[0]["occ"].ToString();
                    cap = dt.Rows[0]["CAPACITY"].ToString();


                }

                var result = new { shed = shed , cap = cap };
                return Json(result);
            }



            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IActionResult> Print(string id)
        {
            string mimtype = "";
            int extension = 1;
            DataSet ds = new DataSet();
            var path = $"{this._WebHostEnvironment.WebRootPath}\\Reports\\PyroReport.rdlc";
            Dictionary<string, string> Parameters = new Dictionary<string, string>();

            var Pyroitem = await Pyro.Getpyropdf(id);

            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("DataSet1", Pyroitem);


            var result = localReport.Execute(RenderType.Pdf, extension, Parameters, mimtype);
            return File(result.MainStream, "application/Pdf");
        }

        public ActionResult MyListItemgrid(string strStatus)
        {
            List<Pyrogrid> Reg = new List<Pyrogrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = Pyro.GetAllPyro(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string Generate = string.Empty;
                string ViewRow = string.Empty;
                string Approve = string.Empty;
                string Edit = string.Empty;
                //if (dtUsers.Rows[i]["IS_APPROVED"].ToString() == "Y")
                //{

                //    //Generate = "<a href=Print?id=" + dtUsers.Rows[i]["PYROPRODBASICID"].ToString() + "><img src='../Images/view_icon.png' alt='View Details' /></a>";
                //    Generate = "<a href=Print?id=" + dtUsers.Rows[i]["NPRODBASICID"].ToString() + " target='_blank'><img src='../Images/pdf.png' alt='Generate Pyro' width='20' /></a>";
                //    ViewRow = "<a href=ViewPyroProduction?id=" + dtUsers.Rows[i]["NPRODBASICID"].ToString() + "><img src='../Images/view_icon.png' alt='View Details' /></a>";
                //    Approve = "";
                //    Edit = "";
                //}

                //else
                //{
                    Generate = "<a href=Print?id=" + dtUsers.Rows[i]["NPRODBASICID"].ToString() + " target='_blank'><img src='../Images/pdf.png' alt='Generate Pyro' width='20' /></a>";
                    ViewRow = "<a href=ViewPyroProduction?id=" + dtUsers.Rows[i]["NPRODBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View Details' /></a>";
                    Approve = "<a href=ApprovePyroProduction?id=" + dtUsers.Rows[i]["NPRODBASICID"].ToString() + "><img src='../Images/checklist.png' alt='Approve' /></a>";
                    Edit = "<a href=PyroProductionentry?id=" + dtUsers.Rows[i]["NPRODBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                //}

                Reg.Add(new Pyrogrid
                {
                    id = dtUsers.Rows[i]["NPRODBASICID"].ToString(),
                    docid = dtUsers.Rows[i]["DOCID"].ToString(),
                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    super = dtUsers.Rows[i]["ENTEREDBY"].ToString(),
                    shi = dtUsers.Rows[i]["SHIFT"].ToString(),
                    location = dtUsers.Rows[i]["WCID"].ToString(),
                    reptrow = Generate,
                    editrow = Edit,
                    viewrow = ViewRow,
                    delrow = Approve,


                });
            }

            return Json(new
            {
                Reg
            });

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
    }
}
