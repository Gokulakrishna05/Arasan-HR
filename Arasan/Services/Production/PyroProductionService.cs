using Arasan.Interface ;
using Arasan.Models;
using System.Data;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Arasan.Services
{
    public class PyroProductionService : IPyroProduction
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public PyroProductionService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public IEnumerable<PyroProduction> GetAllPyro( )
        {
            
            List<PyroProduction> cmpList = new List<PyroProduction>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select  DOCID,to_char(PYROPRODBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,SHIFT,EMPMAST.EMPNAME,LOCDETAILS.LOCID,PYROPRODBASICID from PYROPRODBASIC LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID=PYROPRODBASIC.SUPERVISOR  LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=PYROPRODBASIC.LOCID ORDER BY PYROPRODBASICID desc";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        PyroProduction cmp = new PyroProduction
                        {
                            ID = rdr["PYROPRODBASICID"].ToString(),
                            super = rdr["EMPNAME"].ToString(),
                            DocId = rdr["DOCID"].ToString(),
                            Docdate = rdr["DOCDATE"].ToString(),
                            Location = rdr["LOCID"].ToString(),
                            Shift = rdr["SHIFT"].ToString()

                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
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
        public DataTable ShiftDeatils()
        {
            string SvSql = string.Empty;
            SvSql = "Select SHIFTMASTID,SHIFTNO from SHIFTMAST where SHIFTNO in ('A','B','C') ";
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
        public DataTable GetItem()
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER where IGROUP='RAW MATERIAL' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetOutItem()
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER where IGROUP='SEMI FINISHED GOODS' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDrum()
        {
            string SvSql = string.Empty;
            SvSql = "select DRUMNO,DRUMMASTID from DRUMMAST ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemCon()
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER where igroup='Consumables'";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select BINBASIC.BINID,ITEMMASTER.BINNO as bin ,ITEMMASTERID from ITEMMASTER left outer join BINBASIC ON BINBASICID= ITEMMASTER.BINNO where ITEMMASTERID='" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetOutItemDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select BINBASIC.BINID,ITEMMASTER.BINNO as bin ,ITEMMASTERID from ITEMMASTER left outer join BINBASIC ON BINBASICID= ITEMMASTER.BINNO where ITEMMASTERID='" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetConItemDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select BINBASIC.BINID,ITEMMASTER.BINNO as bin , UNITMAST.UNITID,ITEMMASTER.PRIUNIT as unit,ITEMMASTERID from ITEMMASTER left outer join BINBASIC ON BINBASICID= ITEMMASTER.BINNO LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID where ITEMMASTERID='" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEmp()
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPID,EMPNAME,EMPMASTID from EMPMAST UNION  Select NULL, 'Contract Employee', NULL FROM dual";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetWork(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPID EMPNAME,EP.LOCATIONID LOCATIONNAME,LOCDETAILS.LOCID,EP.EMPALLOCATIONID from EMPALLOCATION left outer join EMPALLOCATIONDETAILS EP ON EMPALLOCATION.EMPALLOCATIONID=EP.EMPALLOCATIONID left outer join LOCDETAILS on LOCDETAILS.LOCDETAILSID=EP.LOCATIONID  where EMPID='" + id + "' order by EMPALLOCATIONDETAILSID ASC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetWorkedit(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PB.LOCID LOCATIONNAME,LOCDETAILS.LOCID from PYROPRODBASIC PB left outer join LOCDETAILS on LOCDETAILS.LOCDETAILSID=PB.LOCID where IS_COMPLETE='No' AND SUPERVISOR='"+ id +"' GROUP BY PB.LOCID,LOCDETAILS.LOCID";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        
        public DataTable GetAPProd(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PYROPRODBASICID,PYROPRODBASIC.DOCID,to_char(PYROPRODBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,SHIFT,LOCID from PYROPRODBASIC  where PYROPRODBASICID='" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetInput(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PYROPRODBASICID,ITEMID,BINBASIC.BINID,BATCH BATCHNO,STOCK,QTY,TIME from PYROPRODINPDET left outer join BINBASIC ON BINBASICID= PYROPRODINPDET.BINID  where PYROPRODBASICID='" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetCons(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PYROPRODBASICID,ITEMID,UNITMAST.UNITID,BINBASIC.BINID,STOCK,QTY,CONSQTY from PYROPRODCONSDET left outer join BINBASIC ON BINBASICID= PYROPRODCONSDET.BINID left outer join UNITMAST ON UNITMASTID= PYROPRODCONSDET.UNITID  where PYROPRODBASICID='" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEmpdet(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PYROPRODBASICID,EMPID,EMPCODE,DEPARTMENT,to_char(PYROPRODEMPDET.STARTDATE,'dd-MON-yyyy')STARTDATE,to_char(PYROPRODEMPDET.ENDDATE,'dd-MON-yyyy')ENDDATE,STARTTIME,ENDTIME,OTHRS OTHOUR,ETOTHER,NORMELHRS NHOUR,NATUREOFWORK from PYROPRODEMPDET where PYROPRODBASICID='" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetBreak(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PYROPRODBASICID,MEACHINECODE MACHCODE,DTYPE,MTYPE,STARTTIME FROMTIME,ENDTIME TOTIME,PB,ALLOTTEDTO,REASON,MEACHDES from PYROPRODBREAKDET  where PYROPRODBASICID='" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetOutput(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PYROPRODOUTDETID,PYROPRODBASICID,PYROPRODOUTDET.ITEMID,BINBASIC.BINID,OUTQTY,DRUM DRUMNO,STARTTIME FROMTIME,ENDTIME TOTIME,ITEMMASTER.ITEMID as ITEMNAME from PYROPRODOUTDET left outer join BINBASIC ON BINBASICID= PYROPRODOUTDET.BINID LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PYROPRODOUTDET.ITEMID  where PYROPRODBASICID='" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetLogdetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PYROPRODBASICID,to_char(PYROPRODLOGDET.STARTDATE,'dd-MON-yyyy')STARTDATE,to_char(PYROPRODLOGDET.ENDDATE,'dd-MON-yyyy')ENDDATE,STARTTIME,ENDTIME,TOTALHRS,REASON from PYROPRODLOGDET where PYROPRODBASICID='" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string PyroProductionEntry(PyroProduction cy)
        {
            string msg = "";
            datatrans = new DataTransactions(_connectionString);


            int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'PYRO-Pro' AND ACTIVESEQUENCE = 'T'");
            string docid = string.Format("{0} {1}", "PYRO-Pro", (idc + 1).ToString());

            string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='PYRO-Pro' AND ACTIVESEQUENCE ='T'";
            try
            {
                datatrans.UpdateStatus(updateCMd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            cy.DocId = docid;
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    objConn.Open();
                    //svSQL = "Update PYROPRODBASIC SET IS_CURRENT='No'";
                    //OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                    //objCmdd.ExecuteNonQuery();

                    OracleCommand objCmd = new OracleCommand("PYROPRODUCTIONPROC", objConn);

                    objCmd.CommandType = CommandType.StoredProcedure;

                    StatementType = "Insert";
                    objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;

                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.Docdate;
                    objCmd.Parameters.Add("LOC", OracleDbType.NVarchar2).Value = cy.Location;

                    objCmd.Parameters.Add("SUPERVISOR", OracleDbType.NVarchar2).Value = cy.super;

                    objCmd.Parameters.Add("SHIFT", OracleDbType.NVarchar2).Value = cy.Shift;

                    // objCmd.Parameters.Add("IS_CURRENT", OracleDbType.NVarchar2).Value = "Yes";
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("IS_COMPLETE", OracleDbType.NVarchar2).Value = "No";
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                    try
                    {

                        objCmd.ExecuteNonQuery();
                        Object Pid = objCmd.Parameters["OUTID"].Value;

                        if (cy.ID != null)
                        {
                            Pid = cy.ID;
                        }
                        cy.APID = Pid;
                        //            if (cy.inplst != null)
                        //            {
                        //                foreach (PProInput cp in cy.inplst)
                        //                {
                        //                    if (cp.Isvalid == "Y" && cp.ItemId != "0")
                        //                    {
                        //                        svSQL = "Insert into PYROPRODINPDET (PYROPRODBASICID,ITEMID,BINID,BATCH,STOCK,QTY,TIME) VALUES ('" + Pid + "','" + cp.ItemId + "','" + cp.Bin + "','" + cp.batchno + "','" + cp.StockAvailable + "','" + cp.IssueQty + "','" + cp.Time +"')";
                        //                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        //                        objCmds.ExecuteNonQuery();


                        //                    }

                        //                }
                        //            }
                        //            if (cy.Binconslst != null)
                        //            {
                        //                foreach (PAPProInCons cp in cy.Binconslst)
                        //                {
                        //                    if (cp.Isvalid == "Y" && cp.ItemId != "0")
                        //                    {
                        //                        svSQL = "Insert into PYROPRODCONSDET (PYROPRODBASICID,ITEMID,UNIT,STOCK,QTY,CONSQTY) VALUES ('" + Pid + "','" + cp.ItemId + "','" + cp.consunit + "','" + cp.ConsStock + "','" + cp.Qty + "','" + cp.consQty + "')";
                        //                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        //                        objCmds.ExecuteNonQuery();


                        //                    }

                        //                }
                        //            }
                        //            if (cy.EmplLst != null)
                        //            {
                        //                foreach (PEmpDetails cp in cy.EmplLst)
                        //                {
                        //                    if (cp.Isvalid == "Y" && cp.Employee != "0")
                        //                    {
                        //                        svSQL = "Insert into PYROPRODEMPDET (PYROPRODBASICID,EMPID,EMPCODE,DEPARTMENT,STARTDATE,ENDDATE,STARTTIME,ENDTIME,OTHRS,ETOTHER,NORMELHRS,NATUREOFWORK) VALUES ('" + Pid + "','" + cp.Employee + "','" + cp.EmpCode + "','" + cp.Depart + "','" + cp.StartDate + "','" + cp.EndDate + "','" + cp.StartTime + "','" + cp.EndTime + "','" + cp.OTHrs + "','" + cp.ETOther + "','" + cp.Normal + "','" + cp.NOW + "')";
                        //                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        //                        objCmds.ExecuteNonQuery();


                        //                    }

                        //                }
                        //            }

                        //            if (cy.BreakLst != null)
                        //            {
                        //                foreach (PBreakDet cp in cy.BreakLst)
                        //                {
                        //                    if (cp.Isvalid == "Y" && cp.MachineId != "0")
                        //                    {
                        //                        svSQL = "Insert into PYROPRODBREAKDET (PYROPRODBASICID,MEACHINECODE,MEACHDES,DTYPE,MTYPE,STARTTIME,ENDTIME,PB,ALLOTTEDTO,REASON) VALUES ('" + Pid + "','" + cp.MachineId + "','" + cp.MachineDes + "','" + cp.DType + "','" + cp.MType + "','" + cp.StartTime + "','" + cp.EndTime + "','" + cp.PB + "','" + cp.Alloted + "','" + cp.Reason + "')";
                        //                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        //                        objCmds.ExecuteNonQuery();


                        //                    }

                        //                }
                        //            }
                        //            if (cy.outlst != null)
                        //            {
                        //                foreach (PProOutput cp in cy.outlst)
                        //                {
                        //                    if (cp.Isvalid == "Y" && cp.ItemId != "0")
                        //                    {
                        //                        svSQL = "Insert into PYROPRODOUTDET (PYROPRODBASICID,ITEMID,BINID,DRUM,OUTQTY,STARTTIME,ENDTIME) VALUES ('" + Pid + "','" + cp.ItemId + "','" + cp.Bin + "','" + cp.drumno + "','" + cp.OutputQty + "','" + cp.FromTime + "','" + cp.ToTime +"')";
                        //                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        //                        objCmds.ExecuteNonQuery();


                        //                    }

                        //                }
                        //            }
                        //            if (cy.LogLst != null)
                        //            {
                        //                foreach (PLogDetails cp in cy.LogLst)
                        //                {
                        //                    if (cp.Isvalid == "Y" && cp.StartDate != "0")
                        //                    {
                        //                        svSQL = "Insert into PYROPRODLOGDET (PYROPRODBASICID,STARTDATE,STARTTIME,ENDDATE,ENDTIME,TOTALHRS,REASON) VALUES ('" + Pid + "','" + cp.StartDate + "','" + cp.StartTime + "','" + cp.EndDate + "','" + cp.EndTime + "','" + cp.tothrs + "','" + cp.Reason + "')";
                        //                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        //                        objCmds.ExecuteNonQuery();


                        //                    }

                        //                }
                        //            }
                        //            objConn.Close();

                    }


                    catch (Exception ex)
                    {
                        //System.Console.WriteLine("Exception: {0}", ex.ToString());
                    }
                    
                
            }
            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }
        public DataTable GetPyroProductionName(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PYROPRODBASICID,PYROPRODBASIC.DOCID,to_char(PYROPRODBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,LOCDETAILS.LOCID,EMPMAST.EMPNAME,SHIFT from PYROPRODBASIC left outer join LOCDETAILS ON LOCDETAILSID= PYROPRODBASIC.LOCID left outer join EMPMAST ON EMPMASTID= PYROPRODBASIC.SUPERVISOR  where PYROPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetPyroProd(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PYROPRODBASICID,PYROPRODBASIC.DOCID,to_char(PYROPRODBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,LOCDETAILS.LOCID,EMPMAST.EMPNAME,SHIFT from PYROPRODBASIC left outer join LOCDETAILS ON LOCDETAILSID= PYROPRODBASIC.LOCID left outer join EMPMAST ON EMPMASTID= PYROPRODBASIC.SUPERVISOR  where PYROPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetInputDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PYROPRODBASICID,ITEMMASTER.ITEMID,BINBASIC.BINID,BATCH,STOCK,QTY,TIME from PYROPRODINPDET left outer join BINBASIC ON BINBASICID= PYROPRODINPDET.BINID left outer join ITEMMASTER ON ITEMMASTER.ITEMMASTERID= PYROPRODINPDET.ITEMID  where PYROPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetConsDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PYROPRODBASICID,ITEMMASTER.ITEMID,PYROPRODCONSDET.UNITID,PYROPRODCONSDET.BINID,STOCK,QTY,UNITMAST.UNITID,CONSQTY,BINBASIC.BINID AS BIN from PYROPRODCONSDET left outer join ITEMMASTER ON ITEMMASTER.ITEMMASTERID= PYROPRODCONSDET.ITEMID left outer join BINBASIC ON BINBASICID= PYROPRODCONSDET.BINID LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID where PYROPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEmpdetDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PYROPRODBASICID,EMPMAST.EMPNAME,EMPCODE,DEPARTMENT,to_char(PYROPRODEMPDET.STARTDATE,'dd-MON-yyyy')STARTDATE,to_char(PYROPRODEMPDET.ENDDATE,'dd-MON-yyyy')ENDDATE,STARTTIME,ENDTIME,OTHRS,ETOTHER,NORMELHRS,NATUREOFWORK from PYROPRODEMPDET left outer join EMPMAST ON EMPMAST.EMPMASTID= PYROPRODEMPDET.EMPID where PYROPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetBreakDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PYROPRODBASICID,machineinfobasic.MCODE,MEACHDES,DTYPE,PYROPRODBREAKDET.MTYPE,STARTTIME,ENDTIME,PB,EMPMAST.EMPNAME,REASON from PYROPRODBREAKDET left outer join machineinfobasic on machineinfobasic.machineinfobasicid=PYROPRODBREAKDET.MEACHINECODE LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID=PYROPRODBREAKDET.ALLOTTEDTO  where PYROPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetOutputDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PYROPRODBASICID,ITEMMASTER.ITEMID,BINBASIC.BINID,OUTQTY,DRUMMAST.DRUMNO,STARTTIME,ENDTIME,ITEMMASTER.ITEMID as ITEMNAME from PYROPRODOUTDET left outer join BINBASIC ON BINBASICID= PYROPRODOUTDET.BINID LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PYROPRODOUTDET.ITEMID LEFT OUTER JOIN DRUMMAST ON DRUMMAST.DRUMMASTID=PYROPRODOUTDET.DRUM  where PYROPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetLogdetailDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PYROPRODBASICID,to_char(PYROPRODLOGDET.STARTDATE,'dd-MON-yyyy')STARTDATE,to_char(PYROPRODLOGDET.ENDDATE,'dd-MON-yyyy')ENDDATE,STARTTIME,ENDTIME,TOTALHRS,REASON from PYROPRODLOGDET where PYROPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable SaveInputDetails(string id, string item, string bin, string time, string qty, string stock, string batch)
        {
            string SvSql = string.Empty;
            using (OracleConnection objConnT = new OracleConnection(_connectionString))
            {
                objConnT.Open();
                SvSql = "Delete PYROPRODINPDET WHERE PYROPRODBASICID='" + id + "'";
                OracleCommand objCmdd = new OracleCommand(SvSql, objConnT);
                objCmdd.ExecuteNonQuery();
            }
            SvSql = "Insert into PYROPRODINPDET (PYROPRODBASICID,ITEMID,BINID,TIME,QTY,STOCK,BATCH) VALUES ('" + id + "','" + item + "','" + bin + "','" + time + "','" + qty + "','" + stock + "','" + batch + "')";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable SaveConsDetails(string id, string item, string bin, string unit, string usedqty, string qty, string stock)
        {
            string SvSql = string.Empty;
            using (OracleConnection objConnT = new OracleConnection(_connectionString))
            {
                objConnT.Open();
                SvSql = "Delete PYROPRODCONSDET WHERE PYROPRODBASICID='" + id + "'";
                OracleCommand objCmdd = new OracleCommand(SvSql, objConnT);
                objCmdd.ExecuteNonQuery();
            }

            SvSql = "Insert into PYROPRODCONSDET (PYROPRODBASICID,ITEMID,BINID,UNITID,QTY,CONSQTY,STOCK) VALUES ('" + id + "','" + item + "','" + bin + "','" + unit + "','" + usedqty + "','" + qty + "','" + stock + "')";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable SaveOutputDetails(string id, string item, string bin, string stime, string ttime, string qty, string drum)
        {
            string SvSql = string.Empty;
            using (OracleConnection objConnT = new OracleConnection(_connectionString))
            {
                objConnT.Open();
                SvSql = "Delete PYROPRODOUTDET WHERE PYROPRODBASICID='" + id + "'";
                OracleCommand objCmdd = new OracleCommand(SvSql, objConnT);
                objCmdd.ExecuteNonQuery();
            }
            SvSql = "Insert into PYROPRODOUTDET (PYROPRODBASICID,ITEMID,BINID,STARTTIME,ENDTIME,OUTQTY,DRUM) VALUES ('" + id + "','" + item + "','" + bin + "','" + stime + "','" + ttime + "','" + qty + "','" + drum + "')";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable SaveEmpDetails(string id, string empname, string code, string depat, string sdate, string stime, string edate, string etime, string ot, string et, string normal, string now)
        {
            string SvSql = string.Empty;
            using (OracleConnection objConnT = new OracleConnection(_connectionString))
            {
                objConnT.Open();
                SvSql = "Delete PYROPRODEMPDET WHERE PYROPRODBASICID='" + id + "'";
                OracleCommand objCmdd = new OracleCommand(SvSql, objConnT);
                objCmdd.ExecuteNonQuery();
            }

            SvSql = "Insert into PYROPRODEMPDET (PYROPRODBASICID,EMPID,EMPCODE,DEPARTMENT,STARTDATE,STARTTIME,ENDDATE,ENDTIME,OTHRS,ETOTHER,NORMELHRS,NATUREOFWORK) VALUES ('" + id + "','" + empname + "','" + code + "','" + depat + "','" + sdate + "','" + stime + "','" + edate + "','" + etime + "','" + ot + "','" + et + "','" + normal + "','" + now + "')";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable SaveBreakDetails(string id, string machine, string des, string dtype, string mtype, string stime, string etime, string pb, string all, string reason)
        {
            string SvSql = string.Empty;
            using (OracleConnection objConnT = new OracleConnection(_connectionString))
            {
                objConnT.Open();
                SvSql = "Delete PYROPRODBREAKDET WHERE PYROPRODBASICID='" + id + "'";
                OracleCommand objCmdd = new OracleCommand(SvSql, objConnT);
                objCmdd.ExecuteNonQuery();
            }
            SvSql = "Insert into PYROPRODBREAKDET (PYROPRODBASICID,MEACHINECODE,MEACHDES,DTYPE,MTYPE,STARTTIME,ENDTIME,PB,ALLOTTEDTO,REASON) VALUES ('" + id + "','" + machine + "','" + des + "','" + dtype + "','" + mtype + "','" + stime + "','" + etime + "','" + pb + "','" + all + "','" + reason + "')";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable SaveLogDetails(string id, string sdate, string stime, string edate, string etime, string tot, string reason)

        {
            string SvSql = string.Empty;
            using (OracleConnection objConnT = new OracleConnection(_connectionString))
            {
                objConnT.Open();
                SvSql = "Delete PYROPRODLOGDET WHERE PYROPRODBASICID='" + id + "'";
                OracleCommand objCmdd = new OracleCommand(SvSql, objConnT);
                objCmdd.ExecuteNonQuery();
            }
            SvSql = "Insert into PYROPRODLOGDET (PYROPRODBASICID,STARTDATE,STARTTIME,ENDDATE,ENDTIME,TOTALHRS,REASON) VALUES ('" + id + "','" + sdate + "','" + stime + "','" + edate + "','" + etime + "','" + tot + "','" + reason + "')";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string PyroProEntryCRUD(PyroProductionentryDet cy)
        {
            string msg = "";


            try
            {
                string StatementType = string.Empty; string svSQL = "";

                if (cy.change != "Complete")
                {
                    using (OracleConnection objConn = new OracleConnection(_connectionString))
                    {
                        objConn.Open();

                        if (cy.change == "Complete")
                        {
                            svSQL = "Update PYROPRODBASIC SET IS_CURRENT='No' WHERE PYROPRODBASICID='" + cy.APID + "'";
                            OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                            objCmdd.ExecuteNonQuery();
                        }

                        DataTable ap = datatrans.GetData("select PYROPRODBASICID,DOCID,LOCID,to_char(PYROPRODBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,SHIFT,SUPERVISOR,BRANCHID from PYROPRODBASIC WHERE IS_CURRENT='Yes'");
                        //svSQL = "Update APPRODUCTIONBASIC SET IS_CURRENT='No'";
                        //OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                        //objCmdd.ExecuteNonQuery();

                        OracleCommand objCmd = new OracleCommand("PYROPRODUCTIONPROC", objConn);

                        objCmd.CommandType = CommandType.StoredProcedure;

                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;

                        objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = ap.Rows[0]["DOCID"].ToString();
                        objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = ap.Rows[0]["DOCDATE"].ToString();
                        objCmd.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = ap.Rows[0]["LOCID"].ToString();

                        objCmd.Parameters.Add("SUPERVISOR", OracleDbType.NVarchar2).Value = ap.Rows[0]["SUPERVISOR"].ToString();
                        objCmd.Parameters.Add("SHIFT", OracleDbType.NVarchar2).Value = cy.change;

                        objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = ap.Rows[0]["BRANCHID"].ToString();
                        objCmd.Parameters.Add("IS_COMPLETE", OracleDbType.NVarchar2).Value = "Yes";
                   
                        objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                        objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                        try
                        {

                            objCmd.ExecuteNonQuery();
                            Object Pid = objCmd.Parameters["OUTID"].Value;

                            if (cy.ID != null)
                            {
                                Pid = cy.ID;
                            }
                            cy.APID = Pid;
                        }
                        catch (Exception ex)
                        {
                            //System.Console.WriteLine("Exception: {0}", ex.ToString());
                        }
                        objConn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }
    }
}
