using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
namespace Arasan.Services 
{
    public class ProductionLogService : IProductionLog
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public ProductionLogService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<ProductionLog> GetAllProductionLog()
        {
            List<ProductionLog> cmpList = new List<ProductionLog>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select  BRANCHMAST.BRANCHID,LPRODBASIC. DOCID,to_char(LPRODBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,WCBASIC.WCID,LPRODBASICID,LPRODBASIC.STATUS from LPRODBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=LPRODBASIC.BRANCH LEFT OUTER JOIN WCBASIC ON WCBASICID=LPRODBASIC.WCID WHERE LPRODBASIC.STATUS='ACTIVE' ";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ProductionLog cmp = new ProductionLog
                        {

                            ID = rdr["LPRODBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            WorkId = rdr["WCID"].ToString(),
                            DocId = rdr["DOCID"].ToString(),
                           Docdate = rdr["DOCDATE"].ToString()





                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public string  ProductionLogCRUD(ProductionLog cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                string[] sdateList = cy.startdate.Split(" - ");
                string sdate = "";
                string stime = "";
                if (sdateList.Length > 0)
                {
                    sdate = sdateList[0];
                    stime = sdateList[1];
                }
                string[] edateList = cy.enddate.Split(" - ");
                string endate = "";
                string endtime = "";
                if (sdateList.Length > 0)
                {
                    endate = edateList[0];
                    endtime = edateList[1];
                }
              

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("PRODUCTIONLOGPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "DIRECTPURCHASEPROC";*/

                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (cy.ID == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    }
                    else
                    {
                        StatementType = "Update";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;

                    }
                    objCmd.Parameters.Add("BRANCH", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("PROCESSID", OracleDbType.NVarchar2).Value = cy.ProcessId;
                    objCmd.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = cy.WorkId;
                    objCmd.Parameters.Add("SHIFT", OracleDbType.NVarchar2).Value = cy.Shift;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.Date).Value = DateTime.Parse(cy.Docdate);
                    objCmd.Parameters.Add("TBMELT", OracleDbType.NVarchar2).Value = cy.Melting;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                    objCmd.Parameters.Add("SUPERBY", OracleDbType.NVarchar2).Value = cy.Supervised;
                    objCmd.Parameters.Add("STARTDATE", OracleDbType.Date).Value = DateTime.Parse(sdate);
                    objCmd.Parameters.Add("ENDDATE", OracleDbType.Date).Value = DateTime.Parse(endate);
                    objCmd.Parameters.Add("STARTTIME", OracleDbType.NVarchar2).Value = stime;
                    objCmd.Parameters.Add("ENDTIME", OracleDbType.NVarchar2).Value = endtime;
                    objCmd.Parameters.Add("ENTEREDBY", OracleDbType.NVarchar2).Value = cy.Entered;
                    objCmd.Parameters.Add("EBUNITSCONS", OracleDbType.NVarchar2).Value = cy.EUnit;
                    objCmd.Parameters.Add("FUELQTY", OracleDbType.NVarchar2).Value = cy.FuelQty;
                    objCmd.Parameters.Add("PROCLOTNO", OracleDbType.NVarchar2).Value = cy.ProcessLot;
                    objCmd.Parameters.Add("PSCHNO", OracleDbType.NVarchar2).Value = cy.ProdSieve;
                    objCmd.Parameters.Add("PROCLOTYN", OracleDbType.NVarchar2).Value = cy.ProdLog;
                    objCmd.Parameters.Add("TOTALINPUT", OracleDbType.NVarchar2).Value = cy.InputQty;
                    objCmd.Parameters.Add("TOTALOUTPUT", OracleDbType.NVarchar2).Value = cy.OutputQty;
                    objCmd.Parameters.Add("TOTCONSQTY", OracleDbType.NVarchar2).Value = cy.ConsQty;
                    objCmd.Parameters.Add("TOTRMVALUE", OracleDbType.NVarchar2).Value = cy.Rmvalue;
                    objCmd.Parameters.Add("TOTALWASTAGE", OracleDbType.NVarchar2).Value = cy.TotalWaste;
                    objCmd.Parameters.Add("TOTALDUST", OracleDbType.NVarchar2).Value = cy.TotalDust;
                    objCmd.Parameters.Add("TOTALPOWDER", OracleDbType.NVarchar2).Value = cy.TotalPowder;
                    objCmd.Parameters.Add("COMPLETEDYN", OracleDbType.NVarchar2).Value = cy.ComplYN;
                    objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Remark;
                    objCmd.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = "ACTIVE";
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        Object Pid = objCmd.Parameters["OUTID"].Value;
                        //string Pid = "0";
                        if (cy.ID != null)
                        {
                            Pid = cy.ID;
                        }
                        //foreach (WorkCenter cp in cy.WorkLst)
                        //{
                        //    if (cp.Isvalid == "Y" && cp.WorkId != "0")
                        //    {
                        //        using (OracleConnection objConns = new OracleConnection(_connectionString))
                        //        {
                        //            OracleCommand objCmds = new OracleCommand("PRODWORKCENTERDETAILPROC", objConns);
                        //            if (cy.ID == null)
                        //            {
                        //                StatementType = "Insert";
                        //                objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                        //            }
                        //            else
                        //            {
                        //                StatementType = "Update";
                        //                objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                        //            }
                        //            objCmds.CommandType = CommandType.StoredProcedure;
                        //            objCmds.Parameters.Add("LPRODBASICID", OracleDbType.NVarchar2).Value = Pid;
                        //            objCmds.Parameters.Add("WORKCENTER", OracleDbType.NVarchar2).Value = cp.WorkId;
                        //            objCmds.Parameters.Add("WSTATUS", OracleDbType.NVarchar2).Value = cp.Status;
                        //            objCmds.Parameters.Add("PTYPE", OracleDbType.NVarchar2).Value = cp.PType;
                        //            objCmds.Parameters.Add("WSTARTDATE", OracleDbType.Date).Value = DateTime.Parse(cp.StartDate);
                        //            objCmds.Parameters.Add("WENDDATE", OracleDbType.Date).Value = DateTime.Parse(cp.EndDate);
                        //            objCmds.Parameters.Add("WSTARTTIME", OracleDbType.NVarchar2).Value = cp.StartTime;
                        //            objCmds.Parameters.Add("WENDTIME", OracleDbType.NVarchar2).Value = cp.EndTime;

                        //            objCmds.Parameters.Add("WTOTHRS", OracleDbType.NVarchar2).Value = cp.TotalHrs;
                        //            objCmds.Parameters.Add("WREASON", OracleDbType.NVarchar2).Value = cp.Reason;

                        //            objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;

                        //            objConns.Open();
                        //            objCmds.ExecuteNonQuery();
                        //            objConns.Close();
                        //        }



                        //    }
                        //}
                        foreach (MachineItem cp in cy.MachineLst)
                        {
                            if (cp.Isvalid == "Y" && cp.Machine != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("PRODMACHINEDETPROC", objConns);
                                    if (cy.ID == null)
                                    {
                                        StatementType = "Insert";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        StatementType = "Update";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                                    }
                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    objCmds.Parameters.Add("LPRODBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("MACHINEID", OracleDbType.NVarchar2).Value = cp.MachineId;
                                    objCmds.Parameters.Add("MACHINENAME", OracleDbType.NVarchar2).Value = cp.Machine;
                                    objCmds.Parameters.Add("RSTATUS", OracleDbType.NVarchar2).Value = cp.Status;
                                    objCmds.Parameters.Add("FROMDATE", OracleDbType.Date).Value = DateTime.Parse(cp.StartDate);
                                    objCmds.Parameters.Add("TODATE", OracleDbType.Date).Value = DateTime.Parse(cp.EndDate);
                                    objCmds.Parameters.Add("FROMTIME", OracleDbType.NVarchar2).Value = cp.StartTime;
                                    objCmds.Parameters.Add("TOTIME", OracleDbType.NVarchar2).Value = cp.EndTime;
                                    objCmds.Parameters.Add("MTOTMINS", OracleDbType.NVarchar2).Value = cp.TotalMins;
                                    objCmds.Parameters.Add("MTOTHRS", OracleDbType.NVarchar2).Value = cp.TotalHrs;
                                    objCmds.Parameters.Add("MACHINECOST", OracleDbType.NVarchar2).Value = cp.TotalMachineCost;
                                    objCmds.Parameters.Add("MREASON", OracleDbType.NVarchar2).Value = cp.Reason;

                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;

                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }



                            }
                        }
                        foreach (EmpDetail cp in cy.EmpLst)
                        {
                            if (cp.Isvalid == "Y" && cp.Employee != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("PRODEMPDETPROC", objConns);
                                    if (cy.ID == null)
                                    {
                                        StatementType = "Insert";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        StatementType = "Update";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                                    }
                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    objCmds.Parameters.Add("LPRODBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("EMPNAME", OracleDbType.NVarchar2).Value = cp.Employee;
                                    objCmds.Parameters.Add("EMPCODE1", OracleDbType.NVarchar2).Value = cp.EmpCode;
                                    objCmds.Parameters.Add("DEPARTMENT", OracleDbType.NVarchar2).Value = cp.Depart;
                                    objCmds.Parameters.Add("ESTARTDATE", OracleDbType.Date).Value = DateTime.Parse(cp.StartDate);
                                    objCmds.Parameters.Add("EENDDATE", OracleDbType.Date).Value = DateTime.Parse(cp.EndDate);
                                    objCmds.Parameters.Add("ESTARTTIME", OracleDbType.NVarchar2).Value = cp.StartTime;
                                    objCmds.Parameters.Add("EENDTIME", OracleDbType.NVarchar2).Value = cp.EndTime;
                                    objCmds.Parameters.Add("OTHRS", OracleDbType.NVarchar2).Value = cp.OTHrs;
                                    objCmds.Parameters.Add("ETOTHRS", OracleDbType.NVarchar2).Value = cp.ETOther;
                                    objCmds.Parameters.Add("NORMHRS", OracleDbType.NVarchar2).Value = cp.Normal;
                                    objCmds.Parameters.Add("NATOFW", OracleDbType.NVarchar2).Value = cp.NOW;

                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;

                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }



                            }
                        }
                        foreach (BreakDetail cp in cy.BreakLst)
                        {
                            if (cp.Isvalid == "Y" && cp.MachineId != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("PRODBREAKDETPROC", objConns);
                                    if (cy.ID == null)
                                    {
                                        StatementType = "Insert";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        StatementType = "Update";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                                    }
                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    objCmds.Parameters.Add("LPRODBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("BMACNO", OracleDbType.NVarchar2).Value = cp.MachineId;
                                    objCmds.Parameters.Add("BMACHINEDESC", OracleDbType.NVarchar2).Value = cp.MachineDes;
                                    objCmds.Parameters.Add("DTYPE", OracleDbType.NVarchar2).Value = cp.DType;
                                    objCmds.Parameters.Add("BFROMTIME", OracleDbType.NVarchar2).Value = cp.StartTime;
                                    objCmds.Parameters.Add("BTOTIME", OracleDbType.NVarchar2).Value = cp.EndTime;
                                    objCmds.Parameters.Add("PREORBRE", OracleDbType.NVarchar2).Value = cp.PB;
                                    objCmds.Parameters.Add("ALLOTEDTO", OracleDbType.NVarchar2).Value = cp.Alloted;
                                    objCmds.Parameters.Add("MTYPE", OracleDbType.NVarchar2).Value = cp.MType;
                                    objCmds.Parameters.Add("ACTDESC", OracleDbType.NVarchar2).Value = cp.Reason;


                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;

                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }



                            }

                        }
                        foreach (InputDetail cp in cy.InputLst)
                        {
                            if (cp.Isvalid == "Y" && cp.Item != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("PRODINPDETPROC", objConns);
                                    if (cy.ID == null)
                                    {
                                        StatementType = "Insert";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        StatementType = "Update";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                                    }
                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    objCmds.Parameters.Add("LPRODBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("IITEMID", OracleDbType.NVarchar2).Value = cp.Item;
                                    objCmds.Parameters.Add("LOTYN", OracleDbType.NVarchar2).Value = cp.LotYN;
                                    objCmds.Parameters.Add("DRUMYN", OracleDbType.NVarchar2).Value = cp.Drumyn;
                                    objCmds.Parameters.Add("IDRUMNO", OracleDbType.NVarchar2).Value = cp.DrumNo;
                                    objCmds.Parameters.Add("IBATCHNO", OracleDbType.NVarchar2).Value = cp.Batch;
                                    objCmds.Parameters.Add("IBATCHQTY", OracleDbType.NVarchar2).Value = cp.BatchQty;
                                    objCmds.Parameters.Add("ICSTOCK", OracleDbType.NVarchar2).Value = cp.Stock;
                                    objCmds.Parameters.Add("IQTY", OracleDbType.NVarchar2).Value = cp.IQty;
                                    objCmds.Parameters.Add("IBRATE", OracleDbType.NVarchar2).Value = cp.IBRate;


                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;

                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }



                            }
                        }
                        foreach (ConsumDetail cp in cy.ConsLst)
                        {
                            if (cp.Isvalid == "Y" && cp.Item != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("PRODCONSDETPROC", objConns);
                                    if (cy.ID == null)
                                    {
                                        StatementType = "Insert";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        StatementType = "Update";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                                    }
                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    objCmds.Parameters.Add("LPRODBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("CITEMID", OracleDbType.NVarchar2).Value = cp.Item;
                                    objCmds.Parameters.Add("CUNIT", OracleDbType.NVarchar2).Value = cp.Unit;
                                    objCmds.Parameters.Add("CLOTYN", OracleDbType.NVarchar2).Value = cp.LotYN;
                                    objCmds.Parameters.Add("CONSQTY", OracleDbType.NVarchar2).Value = cp.ConsQty;
                                    objCmds.Parameters.Add("CSUBQTY", OracleDbType.NVarchar2).Value = cp.Stock;
                                    objCmds.Parameters.Add("CVALUE", OracleDbType.NVarchar2).Value = cp.Value;
                                    objCmds.Parameters.Add("CRATE", OracleDbType.NVarchar2).Value = cp.Rate;



                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;

                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }



                            }
                        }
                        foreach (OutputDetail cp in cy.OutLst)
                        {
                            if (cp.Isvalid == "Y" && cp.Item != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("PRODOUTDETPROC", objConns);
                                    if (cy.ID == null)
                                    {
                                        StatementType = "Insert";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        StatementType = "Update";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                                    }
                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    objCmds.Parameters.Add("LPRODBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("OITEMID", OracleDbType.NVarchar2).Value = cp.Item;
                                    objCmds.Parameters.Add("OLOTYN", OracleDbType.NVarchar2).Value = cp.LotYN;
                                    objCmds.Parameters.Add("ODRUMYN", OracleDbType.NVarchar2).Value = cp.Drumyn;
                                    objCmds.Parameters.Add("ODRUMNO", OracleDbType.NVarchar2).Value = cp.DrumNo;
                                    objCmds.Parameters.Add("NBATCHNO", OracleDbType.NVarchar2).Value = cp.Batch;
                                    objCmds.Parameters.Add("OQTY", OracleDbType.NVarchar2).Value = cp.OQty;
                                    objCmds.Parameters.Add("DSDT", OracleDbType.Date).Value = DateTime.Parse(cp.StartDate);
                                    objCmds.Parameters.Add("DEDT", OracleDbType.Date).Value = DateTime.Parse(cp.EndDate);
                                    objCmds.Parameters.Add("STDTTIME", OracleDbType.NVarchar2).Value = cp.StartTime;
                                    objCmds.Parameters.Add("EDTTIME", OracleDbType.NVarchar2).Value = cp.EndTime;
                                    objCmds.Parameters.Add("RHRS", OracleDbType.NVarchar2).Value = cp.Hrs;



                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;

                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }



                            }
                        }
                        foreach (WasteDetail cp in cy.WasteLst)
                        {
                            if (cp.Isvalid == "Y" && cp.Item != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("PRODWASTEDETPROC", objConns);
                                    if (cy.ID == null)
                                    {
                                        StatementType = "Insert";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        StatementType = "Update";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                                    }
                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    objCmds.Parameters.Add("LPRODBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("WITEMID", OracleDbType.NVarchar2).Value = cp.Item;
                                    objCmds.Parameters.Add("WBATCHNO", OracleDbType.NVarchar2).Value = cp.WBatch;
                                    objCmds.Parameters.Add("WQTY", OracleDbType.NVarchar2).Value = cp.WQty;
                                    objCmds.Parameters.Add("WRATE", OracleDbType.NVarchar2).Value = cp.WRate;




                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;

                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }



                            }
                        }
                        foreach (SourcingDetail cp in cy.SourcingLst)
                        {
                            if (cp.Isvalid == "Y" && cp.NoOfEmp != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("PRODOUTSDETPROC", objConns);
                                    if (cy.ID == null)
                                    {
                                        StatementType = "Insert";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        StatementType = "Update";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                                    }
                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    objCmds.Parameters.Add("LPRODBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("NOOFEMP", OracleDbType.NVarchar2).Value = cp.NoOfEmp;
                                    objCmds.Parameters.Add("EMPWHRS", OracleDbType.NVarchar2).Value = cp.WorkHrs;
                                    objCmds.Parameters.Add("EMPPAY", OracleDbType.NVarchar2).Value = cp.EmpCost;
                                    objCmds.Parameters.Add("MANPOWEXP", OracleDbType.NVarchar2).Value = cp.Expence;
                                    objCmds.Parameters.Add("ONATOFW", OracleDbType.NVarchar2).Value = cp.NOW;

                                    objCmds.Parameters.Add("OWSTDT", OracleDbType.Date).Value = DateTime.Parse(cp.StartDate);
                                    objCmds.Parameters.Add("OWEDDT", OracleDbType.Date).Value = DateTime.Parse(cp.EndDate);
                                    objCmds.Parameters.Add("OWSTT", OracleDbType.NVarchar2).Value = cp.StartTime;
                                    objCmds.Parameters.Add("OWEDT", OracleDbType.NVarchar2).Value = cp.EndTime;




                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;

                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }



                            }
                        }
                        foreach (BunkerDetail cp in cy.BunkLst)
                        {

                            using (OracleConnection objConns = new OracleConnection(_connectionString))
                            {
                                OracleCommand objCmds = new OracleCommand("PRODBUNKERPROC", objConns);
                                if (cy.ID == null)
                                {
                                    StatementType = "Insert";
                                    objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                }
                                else
                                {
                                    StatementType = "Update";
                                    objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                                }
                                objCmds.CommandType = CommandType.StoredProcedure;
                                objCmds.Parameters.Add("LPRODBASICID", OracleDbType.NVarchar2).Value = Pid;
                                objCmds.Parameters.Add("OPBBAL", OracleDbType.NVarchar2).Value = cp.OPBin;
                                objCmds.Parameters.Add("TOTPINP", OracleDbType.NVarchar2).Value = cp.PIP;
                                objCmds.Parameters.Add("TOTGINP", OracleDbType.NVarchar2).Value = cp.GIP;
                                objCmds.Parameters.Add("CLBBAL", OracleDbType.NVarchar2).Value = cp.CLBin;
                                objCmds.Parameters.Add("MLOPBAL", OracleDbType.NVarchar2).Value = cp.MLOP;


                                objCmds.Parameters.Add("MLADD", OracleDbType.NVarchar2).Value = cp.MLAdd;
                                objCmds.Parameters.Add("MLDED", OracleDbType.NVarchar2).Value = cp.MLDed;

                                objCmds.Parameters.Add("MLCLBAL", OracleDbType.NVarchar2).Value = cp.MLCL;



                                objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;

                                objConns.Open();
                                objCmds.ExecuteNonQuery();
                                objConns.Close();
                            }




                        }
                        foreach (ParameterDetail cp in cy.ParamLst)
                        {
                            if (cp.Isvalid == "Y" && cp.Param != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("PRODPARAMETERPROC", objConns);
                                    if (cy.ID == null)
                                    {
                                        StatementType = "Insert";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        StatementType = "Update";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                                    }
                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    objCmds.Parameters.Add("LPRODBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("PARAMETERS", OracleDbType.NVarchar2).Value = cp.Param;
                                    objCmds.Parameters.Add("PARAMUNIT", OracleDbType.NVarchar2).Value = cp.Unit;
                                    objCmds.Parameters.Add("PARAMVALUE", OracleDbType.NVarchar2).Value = cp.Value;
                                    objCmds.Parameters.Add("PSDT", OracleDbType.Date).Value = DateTime.Parse(cp.StartDate);
                                    objCmds.Parameters.Add("PEDT", OracleDbType.Date).Value = DateTime.Parse(cp.EndDate);
                                    objCmds.Parameters.Add("PSTIME", OracleDbType.NVarchar2).Value = cp.StartTime;
                                    objCmds.Parameters.Add("PETIME", OracleDbType.NVarchar2).Value = cp.EndTime;




                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;

                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }



                            }
                        }
                        foreach (ProcessDetail cp in cy.ProcessLst)
                        {
                            if (cp.Isvalid == "Y" && cp.Process != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("PRODPROCESSDETPROC", objConns);
                                    if (cy.ID == null)
                                    {
                                        StatementType = "Insert";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        StatementType = "Update";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                                    }
                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    objCmds.Parameters.Add("LPRODBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("BATCH", OracleDbType.NVarchar2).Value = cp.Batch;
                                    objCmds.Parameters.Add("DISTNO", OracleDbType.NVarchar2).Value = cp.DistNo;
                                    objCmds.Parameters.Add("DISTNO2", OracleDbType.NVarchar2).Value = cp.DistNo1;
                                    objCmds.Parameters.Add("EPROCESSID", OracleDbType.NVarchar2).Value = cp.Process;
                                    objCmds.Parameters.Add("EPSEQ", OracleDbType.NVarchar2).Value = cp.Seq;
                                    objCmds.Parameters.Add("PETOTHRS", OracleDbType.NVarchar2).Value = cp.TotHrs;
                                    objCmds.Parameters.Add("ESDT", OracleDbType.Date).Value = DateTime.Parse(cp.StartDate);
                                    objCmds.Parameters.Add("EEDT", OracleDbType.Date).Value = DateTime.Parse(cp.EndDate);
                                    objCmds.Parameters.Add("EST", OracleDbType.NVarchar2).Value = cp.StartTime;
                                    objCmds.Parameters.Add("EET", OracleDbType.NVarchar2).Value = cp.EndTime;

                                    objCmds.Parameters.Add("EBRHRS", OracleDbType.NVarchar2).Value = cp.BreakHrs;
                                    objCmds.Parameters.Add("ERUNHRS", OracleDbType.NVarchar2).Value = cp.RunHrs;
                                    objCmds.Parameters.Add("ENARR", OracleDbType.NVarchar2).Value = cp.Narr;


                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;

                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }



                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //System.Console.WriteLine("Exception: {0}", ex.ToString());
                    }
                    objConn.Close();
                }
            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;

        }
        public DataTable GetWorkCenter()
        {
            string SvSql = string.Empty;
            SvSql = "Select WCID,WCBASICID from WCBASIC ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetReason()
        {
            string SvSql = string.Empty;
            SvSql = "Select REASON,REASONDETAILID from REASONDETAIL ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetMachine()
        {
            string SvSql = string.Empty;
            SvSql = "Select MNAME,MCODE,MACHINEINFOBASICID from MACHINEINFOBASIC ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetMachineDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select MNAME,MCODE,MTYPE,MACHINEINFOBASICID from MACHINEINFOBASIC where MACHINEINFOBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEmployeeDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPID,EMPNAME,EMPMASTID from EMPMAST where EMPMASTID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable ShiftDeatils()
        {
            string SvSql = string.Empty;
            SvSql = "Select SHIFTMASTID,SHIFTNO from SHIFTMAST";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable BindProcess()
        {
            string SvSql = string.Empty;
            SvSql = "select PROCESSID,PROCESSMASTID from PROCESSMAST";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItem()
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable DrumDetails()
        {
            string SvSql = string.Empty;
            SvSql = "select DRUMMASTID,DRUMNO from DRUMMAST where DRUMTYPE='PRODUCTION'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProductionLog(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCH,PROCESSID,WCID,SHIFT,to_char(LPRODBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,TBMELT,DOCID,SUPERBY,ENTEREDBY,EBUNITSCONS,FUELQTY,PROCLOTNO,PSCHNO,PROCLOTYN,TOTALINPUT,TOTALOUTPUT,TOTCONSQTY,TOTRMVALUE,TOTALWASTAGE,TOTALDUST,TOTALPOWDER,COMPLETEDYN,LPRODBASICID from LPRODBASIC where LPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProWorkCenterDet(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select LPRODBASICID,WORKCENTER,WSTATUS,PTYPE,to_char(LPRODWCDET.WSTARTDATE,'dd-MON-yyyy')WSTARTDATE,to_char(LPRODWCDET.WENDDATE,'dd-MON-yyyy')WENDDATE,WSTARTTIME,WENDTIME,WTOTHRS,WREASON from LPRODWCDET where LPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProMachineDet(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select LPRODBASICID,MACHINEID,MACHINENAME,RSTATUS,to_char(LPRODMACDET.FROMDATE,'dd-MON-yyyy')FROMDATE,to_char(LPRODMACDET.TODATE,'dd-MON-yyyy')TODATE,FROMTIME,TOTIME,MTOTMINS,MTOTHRS,MACHINECOST,MREASON from LPRODMACDET where LPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProEmpDet(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select LPRODBASICID,EMPNAME,EMPCODE1,DEPARTMENT,to_char(LPRODEMPDET.ESTARTDATE,'dd-MON-yyyy')ESTARTDATE,to_char(LPRODEMPDET.EENDDATE,'dd-MON-yyyy')EENDDATE,ESTARTTIME,EENDTIME,OTHRS,ETOTHRS,NORMHRS,NATOFW from LPRODEMPDET where LPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProBreakDet(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select LPRODBASICID,BMACNO,BMACHINEDESC,DTYPE,BFROMTIME,BTOTIME,PREORBRE,ALLOTEDTO,MTYPE,ACTDESC from LPRODBRKDWN where LPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProInpDet(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select LPRODBASICID,IITEMID,LOTYN,DRUMYN,IDRUMNO,IBATCHNO,IBATCHQTY,ICSTOCK,IQTY,IBRATE from LPRODINPDET where LPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProConsDet(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select LPRODBASICID,CITEMID,CUNIT,CLOTYN,CONSQTY,CSUBQTY,CVALUE,CRATE from LPRODCONSDET where LPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProOutDet(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select LPRODBASICID,OITEMID,OLOTYN,ODRUMYN,ODRUMNO,NBATCHNO,OQTY,to_char(LPRODOUTDET.DSDT,'dd-MON-yyyy')DSDT,to_char(LPRODOUTDET.DEDT,'dd-MON-yyyy')DEDT,STDTTIME,EDTTIME,RHRS from LPRODOUTDET where LPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProWasteDet(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select LPRODBASICID,WITEMID,WBATCHNO,WQTY,WRATE from LPRODWASTEDET where LPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProOutsDet(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select LPRODBASICID,NOOFEMP,EMPWHRS,EMPPAY,MANPOWEXP,ONATOFW,to_char(LPRODOUTS.OWSTDT,'dd-MON-yyyy')OWSTDT, OWEDDT,OWSTT,OWEDT from LPRODOUTS where LPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProBunkDet(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select LPRODBASICID,OPBBAL,TOTPINP,TOTGINP,CLBBAL,MLOPBAL,MLADD,MLDED,MLCLBAL from LPRODBUNK where LPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProParamDet(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select LPRODBASICID,PARAMETERS,PARAMUNIT,PARAMVALUE,to_char(LPRODPARAMDETAIL.PSDT,'dd-MON-yyyy')PSDT,to_char(LPRODPARAMDETAIL.PEDT,'dd-MON-yyyy')PEDT,PSTIME,PETIME from LPRODPARAMDETAIL where LPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProProcessDet(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select LPRODBASICID,BATCH,DISTNO,DISTNO2,EPROCESSID,EPSEQ,PETOTHRS,to_char(LPRODPROCDETAIL.ESDT,'dd-MON-yyyy')ESDT,to_char(LPRODPROCDETAIL.EEDT,'dd-MON-yyyy')EEDT,EST,EET,EBRHRS,ERUNHRS,ENARR from LPRODPROCDETAIL where LPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProductionLogByName(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCH,PROCESSID,WCID,SHIFT,to_char(LPRODBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,TBMELT,DOCID,SUPERBY,ENTEREDBY,EBUNITSCONS,FUELQTY,PROCLOTNO,PSCHNO,PROCLOTYN,TOTALINPUT,TOTALOUTPUT,TOTCONSQTY,TOTRMVALUE,TOTALWASTAGE,TOTALDUST,TOTALPOWDER,COMPLETEDYN,LPRODBASICID from LPRODBASIC where LPRODBASICID='" + name + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable ProWorkCenterDet(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select LPRODBASICID,WORKCENTER,WSTATUS,PTYPE,to_char(LPRODWCDET.WSTARTDATE,'dd-MON-yyyy')WSTARTDATE,to_char(LPRODWCDET.WENDDATE,'dd-MON-yyyy')WENDDATE,WSTARTTIME,WENDTIME,WTOTHRS,WREASON from LPRODWCDET where LPRODBASICID='" + name + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable ProMachineDet(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select LPRODBASICID,MACHINEID,MACHINENAME,RSTATUS,to_char(LPRODMACDET.FROMDATE,'dd-MON-yyyy')FROMDATE,to_char(LPRODMACDET.TODATE,'dd-MON-yyyy')TODATE,FROMTIME,TOTIME,MTOTMINS,MTOTHRS,MACHINECOST,MREASON from LPRODMACDET where LPRODBASICID='" + name + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable ProEmpDet(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select LPRODBASICID,EMPNAME,EMPCODE1,DEPARTMENT,to_char(LPRODEMPDET.ESTARTDATE,'dd-MON-yyyy')ESTARTDATE,to_char(LPRODEMPDET.EENDDATE,'dd-MON-yyyy')EENDDATE,ESTARTTIME,EENDTIME,OTHRS,ETOTHRS,NORMHRS,NATOFW from LPRODEMPDET where LPRODBASICID='" + name + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable ProBreakDet(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select LPRODBASICID,BMACNO,BMACHINEDESC,DTYPE,BFROMTIME,BTOTIME,PREORBRE,ALLOTEDTO,MTYPE,ACTDESC from LPRODBRKDWN where LPRODBASICID='" + name + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable ProInpDet(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select LPRODBASICID,IITEMID,LOTYN,DRUMYN,IDRUMNO,IBATCHNO,IBATCHQTY,ICSTOCK,IQTY,IBRATE from LPRODINPDET where LPRODBASICID='" + name + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable ProConsDet(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select LPRODBASICID,CITEMID,CUNIT,CLOTYN,CONSQTY,CSUBQTY,CVALUE,CRATE from LPRODCONSDET where LPRODBASICID='" + name + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable ProOutDet(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select LPRODBASICID,OITEMID,OLOTYN,ODRUMYN,ODRUMNO,NBATCHNO,OQTY,to_char(LPRODOUTDET.DSDT,'dd-MON-yyyy')DSDT,to_char(LPRODOUTDET.DEDT,'dd-MON-yyyy')DEDT,STDTTIME,EDTTIME,RHRS from LPRODOUTDET where LPRODBASICID='" + name + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable ProWasteDet(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select LPRODBASICID,WITEMID,WBATCHNO,WQTY,WRATE from LPRODWASTEDET where LPRODBASICID='" + name + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable ProOutsDet(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select LPRODBASICID,NOOFEMP,EMPWHRS,EMPPAY,MANPOWEXP,ONATOFW,to_char(LPRODOUTS.OWSTDT,'dd-MON-yyyy')OWSTDT, OWEDDT,OWSTT,OWEDT from LPRODOUTS where LPRODBASICID='" + name + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable ProBunkDet(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select LPRODBASICID,OPBBAL,TOTPINP,TOTGINP,CLBBAL,MLOPBAL,MLADD,MLDED,MLCLBAL from LPRODBUNK where LPRODBASICID='" + name + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable ProParamDet(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select LPRODBASICID,PARAMETERS,PARAMUNIT,PARAMVALUE,to_char(LPRODPARAMDETAIL.PSDT,'dd-MON-yyyy')PSDT,to_char(LPRODPARAMDETAIL.PEDT,'dd-MON-yyyy')PEDT,PSTIME,PETIME from LPRODPARAMDETAIL where LPRODBASICID='" + name + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable ProProcessDet(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select LPRODBASICID,BATCH,DISTNO,DISTNO2,EPROCESSID,EPSEQ,PETOTHRS,to_char(LPRODPROCDETAIL.ESDT,'dd-MON-yyyy')ESDT,to_char(LPRODPROCDETAIL.EEDT,'dd-MON-yyyy')EEDT,EST,EET,EBRHRS,ERUNHRS,ENARR from LPRODPROCDETAIL where LPRODBASICID='" + name + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string StatusChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE LPRODBASIC SET STATUS ='INACTIVE' WHERE LPRODBASICID='" + id + "'";
                    OracleCommand objCmds = new OracleCommand(svSQL, objConnT);
                    objConnT.Open();
                    objCmds.ExecuteNonQuery();
                    objConnT.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "";

        }
    }
}
