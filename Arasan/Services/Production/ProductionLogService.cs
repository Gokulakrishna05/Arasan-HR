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
                    cmd.CommandText = "Select  BRANCHMAST.BRANCHID,LPRODBASIC. DOCID,to_char(LPRODBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,WCBASIC.WCID,LPRODBASICID from LPRODBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=LPRODBASIC.BRANCH LEFT OUTER JOIN WCBASIC ON WCBASICID=LPRODBASIC.WCID ";
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
                    //objCmd.Parameters.Add("STARTDATE", OracleDbType.Date).Value = DateTime.Parse(cy.startdate);
                    //objCmd.Parameters.Add("ENDDATE", OracleDbType.Date).Value = DateTime.Parse(cy.enddate);
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
    }
}
