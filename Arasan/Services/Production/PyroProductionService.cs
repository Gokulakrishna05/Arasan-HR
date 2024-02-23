using Arasan.Interface ;
using Arasan.Models;
using System.Data;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using System.DirectoryServices.Protocols;

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

        //public IEnumerable<PyroProduction> GetAllPyro( )
        //{
            
        //    List<PyroProduction> cmpList = new List<PyroProduction>();
        //    using (OracleConnection con = new OracleConnection(_connectionString))
        //    {

        //        using (OracleCommand cmd = con.CreateCommand())
        //        {
        //            con.Open();
        //            cmd.CommandText = "Select  DOCID,to_char(PYROPRODBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,SHIFT,EMPMAST.EMPNAME,LOCDETAILS.LOCID,PYROPRODBASICID,PYROPRODBASIC.IS_APPROVED from PYROPRODBASIC LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID=PYROPRODBASIC.SUPERVISOR  LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=PYROPRODBASIC.LOCID ORDER BY PYROPRODBASICID desc";
        //            OracleDataReader rdr = cmd.ExecuteReader();
        //            while (rdr.Read())
        //            {
        //                PyroProduction cmp = new PyroProduction
        //                {
        //                    ID = rdr["PYROPRODBASICID"].ToString(),
        //                    super = rdr["EMPNAME"].ToString(),
        //                    DocId = rdr["DOCID"].ToString(),
        //                    Docdate = rdr["DOCDATE"].ToString(),
        //                    Location = rdr["LOCID"].ToString(),
        //                    Approve = rdr["IS_APPROVED"].ToString(),
        //                    Shift = rdr["SHIFT"].ToString()

        //                };
        //                cmpList.Add(cmp);
        //            }
        //        }
        //    }
        //    return cmpList;
        //}
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
            SvSql = "Select EMPID,EMPNAME,EMPMASTID,EMPDEPT, DDBASIC.DEPTNAME,EMPCOST,OTPERHR from EMPMAST LEFT OUTER JOIN DDBASIC ON DDBASICID=EMPMAST.EMPDEPT where EMPMASTID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select P.RITEMID,I.ITEMID from PSINPDETAIL P,LSTOCKVALUE L,ITEMMASTER I   where P.RITEMID=I.ITEMMASTERID AND PSBASICID='" + id + "'  HAVING SUM(L.PLUSQTY-L.MINUSQTY) > 0 GROUP BY P.RITEMID,I.ITEMID";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDrumBatch(string ItemId, string loc, string item)
        {
            string SvSql = string.Empty;
            SvSql = "select  l.LOTNO ,SUM(l.PLUSQTY-l.MINUSQTY) as QTY  from LSTOCKVALUE l,LOTMAST lt WHERE lt.LOTNO=l.LOTNO AND lt.INSFLAG='1' AND l.DRUMNO='" + ItemId + "' AND l.LOCID='" + loc + "' AND l.ITEMID ='" + item + "' HAVING SUM(l.PLUSQTY-l.MINUSQTY) > 0 GROUP BY l.LOTNO";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetOutItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select P.OITEMID,I.ITEMID from PSOUTDETAIL P,LSTOCKVALUE L,ITEMMASTER I   where P.OITEMID=I.ITEMMASTERID AND PSBASICID='" + id + "' GROUP BY P.OITEMID,I.ITEMID";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDrum()
        {
            string SvSql = string.Empty;
            SvSql = "select DRUMNO,DRUMMASTID from DRUMMAST WHERE IS_EMPTY='Y'";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDrum(string item,string loc)
        {
            string SvSql = string.Empty;
            SvSql = "select DRUMNO,SUM(PLUSQTY-MINUSQTY) as QTY  from LSTOCKVALUE where ITEMID='" + item+ "' AND LOCID='"+loc+ "' HAVING SUM(PLUSQTY-MINUSQTY) > 0 GROUP BY DRUMNO ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetStockDetails(string ItemId,string item)
        {
            string SvSql = string.Empty;
            SvSql = "select BALANCE_QTY as qty from DRUM_STOCK where DRUM_ID='" + ItemId + "' AND ITEMID='"+item+ "' AND LOCID='10035000000038' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemCon(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMMASTER.ITEMID,STOCKVALUE.ITEMID as item from STOCKVALUE left outer join ITEMMASTER on ITEMMASTER.ITEMMASTERID=STOCKVALUE.ITEMID where ITEMMASTER.igroup='Consumables'  AND LOCID='"+ id + "' GROUP BY ITEMMASTER.ITEMID,STOCKVALUE.ITEMID ";

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
        public DataTable GetWork()
        {
            string SvSql = string.Empty;
            SvSql = "Select WCID,WCBASICID from WCBASIC";
           // SvSql = "Select EMPID EMPNAME,EP.LOCATIONID LOCATIONNAME,LOCDETAILS.LOCID,EP.EMPALLOCATIONID from EMPALLOCATION left outer join EMPALLOCATIONDETAILS EP ON EMPALLOCATION.EMPALLOCATIONID=EP.EMPALLOCATIONID left outer join LOCDETAILS on LOCDETAILS.LOCDETAILSID=EP.LOCATIONID  where EMPID='" + id + "' order by EMPALLOCATIONDETAILSID ASC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProdSch(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select PSBASICID,DOCID from PSBASIC WHERE WCID='"+id+"' ";
            // SvSql = "Select EMPID EMPNAME,EP.LOCATIONID LOCATIONNAME,LOCDETAILS.LOCID,EP.EMPALLOCATIONID from EMPALLOCATION left outer join EMPALLOCATIONDETAILS EP ON EMPALLOCATION.EMPALLOCATIONID=EP.EMPALLOCATIONID left outer join LOCDETAILS on LOCDETAILS.LOCDETAILSID=EP.LOCATIONID  where EMPID='" + id + "' order by EMPALLOCATIONDETAILSID ASC";
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
            SvSql = "select NPRODBASICID,IITEMID,BINBASIC.BINID,IBINID,IBATCHNO,IBATCHQTY,IQTY,IS_INSERT,CHARGINGTIME,ICDRUMNO,UNITMAST.UNITID from NPRODINPDET INNER JOIN ITEMMASTER on ITEMMASTERID=NPRODINPDET.IITEMID LEFT OUTER  JOIN UNITMAST on UNITMASTID=ITEMMASTER.PRIUNIT   LEFT OUTER JOIN BINBASIC ON BINBASICID=NPRODINPDET.IBINID where NPRODBASICID='" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetCons(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select NPRODBASICID,CITEMID,CBINID,CUNIT,CLOTYN,CNARR,CONSQTY,IS_INSERT,CSUBQTY,VALIDROW3,NPRODCONSDETROW from NPRODCONSDET  where NPRODBASICID='" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEmpdet(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select NPRODBASICID,EMPNAME,EMPCODE1,DEPARTMENT,to_char(NPRODEMPDET.ESTARTDATE,'dd-MON-yyyy')ESTARTDATE,ESTARTTIME,to_char(NPRODEMPDET.EENDDATE,'dd-MON-yyyy')EENDDATE,EENDTIME,OTHRS,ETOTHRS,NORMHRS,NATOFW from NPRODEMPDET where NPRODBASICID='" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetBreak(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select NPRODBASICID,BMACNO,BMACHINEDESC,DTYPE,MTYPE,BFROMTIME,BTOTIME,PREORBRE,ALLOTEDTO,ACTDESC from NPRODBRKDWN  where NPRODBASICID='" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetOutput(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select NPRODBASICID,OITEMID,ODRUMNO,STIME,OQTY,ETIME,OXQTY,SHEDNUMBER,NPRODOUTDET.STATUS,OSTOCK,TOLOCATION,OCDRUMNO,UNITMAST.UNITID from NPRODOUTDET INNER JOIN ITEMMASTER on ITEMMASTERID=NPRODOUTDET.OITEMID LEFT OUTER  JOIN UNITMAST on UNITMASTID=ITEMMASTER.PRIUNIT    where NPRODBASICID='" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetLogdetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select NPRODBASICID,to_char(NPRODLOGDET.STARTDATE,'dd-MON-yyyy')STARTDATE,to_char(NPRODLOGDET.ENDDATE,'dd-MON-yyyy')ENDDATE,STARTTIME,ENDTIME,TOTALHRS,REASON from NPRODLOGDET where NPRODBASICID='" + id + "' ";

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
            SvSql = "Select W.WCID WORKID,N.BRANCH,N.PROCESSID,N.PSCHNO ,P.PROCESSID PROCESS,PS.DOCID psno,N.WCID,SHIFT,to_char(N.DOCDATE,'dd-MON-yyyy')DOCDATE,N.ILOCDETAILSID,N.DOCID,N.ENTEREDBY,N.SCHQTY,N.PRODQTY,N.PROCLOTNO from NPRODBASIC N,WCBASIC W ,PROCESSMAST P,PSBASIC PS where W.WCBASICID=N.WCID AND P.PROCESSMASTID=N.PROCESSID AND PS.PSBASICID=N.PSCHNO AND N.NPRODBASICID='" + id + "' ";
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
        public DataTable GetProduction(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select W.WCID WORKID,N.BRANCH,N.PROCESSID,N.PSCHNO ,P.PROCESSID PROCESS,PS.DOCID psno,N.WCID,SHIFT,to_char(N.DOCDATE,'dd-MON-yyyy')DOCDATE,N.ILOCDETAILSID,N.DOCID,N.ENTEREDBY,N.SCHQTY,N.PRODQTY,N.PROCLOTNO from NPRODBASIC N,WCBASIC W ,PROCESSMAST P,PSBASIC PS where W.WCBASICID=N.WCID AND P.PROCESSMASTID=N.PROCESSID AND PS.PSBASICID=N.PSCHNO AND N.NPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;

        }

        public DataTable GetInputDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select NPRODBASICID,ITEMMASTER.ITEMID,BINBASIC.BINID,IBINID,IBATCHNO,IBATCHQTY,IQTY,CHARGINGTIME,DRUMMAST.DRUMNO,IDRUMNO,UNITMAST.UNITID from NPRODINPDET INNER JOIN ITEMMASTER on ITEMMASTERID=NPRODINPDET.IITEMID LEFT OUTER  JOIN UNITMAST on UNITMASTID=ITEMMASTER.PRIUNIT   LEFT OUTER JOIN BINBASIC ON BINBASICID=NPRODINPDET.IBINID LEFT OUTER JOIN DRUMMAST ON DRUMMASTID=NPRODINPDET.IDRUMNO where NPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetConsDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select NPRODBASICID,ITEMMASTER.ITEMID,CITEMID,CBINID,CUNIT,CLOTYN,CNARR,CONSQTY,CSUBQTY,VALIDROW3,NPRODCONSDETROW from NPRODCONSDET LEFT OUTER JOIN ITEMMASTER ON  ITEMMASTERID=NPRODCONSDET.CITEMID WHERE NPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEmpdetDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select NPRODBASICID,EMPMAST.EMPNAME,EMPCODE1,DEPARTMENT,to_char(NPRODEMPDET.ESTARTDATE,'dd-MON-yyyy')ESTARTDATE,ESTARTTIME,to_char(NPRODEMPDET.EENDDATE,'dd-MON-yyyy')EENDDATE,EENDTIME,OTHRS,ETOTHRS,NORMHRS,NATOFW from NPRODEMPDET left outer join EMPMAST ON EMPMAST.EMPMASTID= NPRODEMPDET.EMPNAME  where NPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetBreakDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select NPRODBASICID,machineinfobasic.MCODE,BMACHINEDESC,NPRODBRKDWN.DTYPE,NPRODBRKDWN.MTYPE,BFROMTIME,BTOTIME,PREORBRE,ALLOTEDTO,ACTDESC from NPRODBRKDWN left outer join machineinfobasic on machineinfobasic.machineinfobasicid=NPRODBRKDWN.BMACNO where NPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetOutputDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select NPRODBASICID,ITEMMASTER.ITEMID,OITEMID,SHEDNUMBER,DRUMMAST.DRUMNO,ODRUMNO,STIME,OQTY,ETIME,OXQTY,NPRODOUTDET.STATUS,OSTOCK,TOLOCATION,OCDRUMNO,UNITMAST.UNITID from NPRODOUTDET INNER JOIN ITEMMASTER on ITEMMASTERID=NPRODOUTDET.OITEMID LEFT OUTER  JOIN UNITMAST on UNITMASTID=ITEMMASTER.PRIUNIT LEFT OUTER JOIN DRUMMAST ON DRUMMAST.DRUMMASTID=NPRODOUTDET.ODRUMNO   where NPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetLogdetailDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select NPRODBASICID,to_char(NPRODLOGDET.STARTDATE,'dd-MON-yyyy')STARTDATE,to_char(NPRODLOGDET.ENDDATE,'dd-MON-yyyy')ENDDATE,STARTTIME,ENDTIME,TOTALHRS,REASON from NPRODLOGDET where NPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetOutsdetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select NPRODBASICID,NOOFEMP,to_char(NPRODOUTS.OWSTDTT,'dd-MON-yyyy')OWSTDTT,OWSTT,to_char(NPRODOUTS.OWEDDTT,'dd-MON-yyyy')OWEDDTT,OWEDT,EMPWHRS,EMPPAY,MANPOWEXP,ONATOFW from NPRODOUTS where NPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable SaveInputDetails(string id, string item, string bin, string time, string qty, string stock, string batch,string drum,int r)
        {
            string SvSql = string.Empty;
            string SvSql1 = string.Empty;
            string insflag = string.Empty;
            DataTable dtt = new DataTable();
            using (OracleConnection objConnT = new OracleConnection(_connectionString))
            {
                objConnT.Open();
                DataTable drumno = datatrans.GetData("SELECT DRUMMASTID,DRUMSLOCID FROM DRUMMAST WHERE DRUMNO ='" + drum + "'");
                string drumloc = "";
                string drumid = "";
                string narr = "";
                if (drumno.Rows.Count > 0)
                {
                    drumloc = drumno.Rows[0]["DRUMSLOCID"].ToString();
                    drumid = drumno.Rows[0]["DRUMMASTID"].ToString();
                }
              
                DataTable drlot = datatrans.GetData("SELECT DRUMYN,LOTYN,ITEMACC,QCCOMPFLAG FROM ITEMMASTER WHERE ITEMMASTERID='" + item + "'");
                DataTable wopro = datatrans.GetData("SELECT WCBASIC.WCID,PROCESSMAST.PROCESSID,NPRODBASIC.DOCID,to_char(NPRODBASIC.DOCDATE,'dd-MM-yy')DOCDATE,ILOCDETAILSID FROM NPRODBASIC LEFT OUTER JOIN WCBASIC ON WCBASICID=NPRODBASIC.WCID LEFT OUTER JOIN PROCESSMAST ON PROCESSMAST.PROCESSMASTID=NPRODBASIC.PROCESSID WHERE NPRODBASICID ='" + id + "'");
                string qc = drlot.Rows[0]["QCCOMPFLAG"].ToString();
                string docdate = DateTime.Now.ToString("dd-MMM-yyyy");
                string work = wopro.Rows[0]["WCID"].ToString();
                string process = wopro.Rows[0]["PROCESSID"].ToString();
                 
                    narr = "Consumption in " + work + " - " + process +" - "+ drum;
                
                SvSql = "Insert into NPRODINPDET (NPRODBASICID,IITEMID,IBINID,CHARGINGTIME,IQTY,IBATCHQTY,IBATCHNO,IDRUMNO,ICDRUMNO,IDRUMSLOCID,IDRUMSLOCATION,DRUMYN,LOTYN,IITEMACC,NPRODINPDETROW,INARR,IS_INSERT,IITEMMASTERID,ICLSTKBUP,ICSOCTKBUP) VALUES ('" + id + "','" + item + "','0','" + time + "','" + qty + "','" + stock + "','" + batch + "','" + drumid + "','" + drum + "','" + drumloc + "','" + drumloc + "','" + drlot.Rows[0]["DRUMYN"].ToString() + "','" + drlot.Rows[0]["LOTYN"].ToString() + "','" + drlot.Rows[0]["ITEMACC"].ToString() + "','" + r + "','" + narr + "','Y','"+ item + "','0','0') RETURNING NPRODINPDETID INTO :LASTCID";
                OracleCommand objCmds = new OracleCommand(SvSql, objConnT);

                objCmds.Parameters.Add("LASTCID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                objCmds.ExecuteNonQuery();
                string detid = objCmds.Parameters["LASTCID"].Value.ToString();
                if (qc == "YES") { insflag = "0"; } else { insflag = "1"; }
                DataTable lstock = datatrans.GetData("SELECT ITEMID,T1SOURCEID,QTY FROM STOCKVALUE WHERE ITEMID='" + item + "' and T1SOURCEID='" +id + "'");
                if (lstock.Rows.Count > 0)
                {
                    double sqty = Convert.ToDouble(lstock.Rows[0]["QTY"].ToString());
                    double iqty = Convert.ToDouble(qty);
                    double totqty = sqty+ iqty;
                    SvSql = "UPDATE STOCKVALUE SET QTY='" + totqty + "' WHERE ITEMID='" + item + "' and T1SOURCEID='" + id + "'";
                    OracleCommand objCmdss1 = new OracleCommand(SvSql, objConnT);
                    objCmdss1.ExecuteNonQuery();
                }
                else
                {
                    SvSql1 = "Insert into STOCKVALUE (T1SOURCEID,PLUSORMINUS,ITEMID,DOCDATE,QTY,LOCID,BINID,RATEC,PROCESSID,SNO,SCSID,SVID,FROMLOCID,STOCKTRANSTYPE,SINSFLAG) VALUES ('" + detid + "','m','" + item + "','" + docdate + "','" + qty + "' ,'" + wopro.Rows[0]["ILOCDETAILSID"].ToString() + "','0','0','0','0','0','0','0','PROD INPUT','" + insflag + "')";
                    OracleCommand objCmdss2 = new OracleCommand(SvSql1, objConnT);
                    objCmdss2.ExecuteNonQuery();
                }
                DataTable dt = datatrans.GetData("Select INVENTORY_ITEM.BALANCE_QTY,INVENTORY_ITEM.ITEM_ID,INVENTORY_ITEM.LOCATION_ID,INVENTORY_ITEM.BRANCH_ID,GRNID,INVENTORY_ITEM_ID,TSOURCEID,GRN_DATE from INVENTORY_ITEM where INVENTORY_ITEM.ITEM_ID='" + item + "' AND INVENTORY_ITEM.LOCATION_ID='" + wopro.Rows[0]["ILOCDETAILSID"].ToString() + "' and LOT_NO='" + batch + "' and BALANCE_QTY!=0 order by GRN_DATE ASC");
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        double rqty = Convert.ToDouble(dt.Rows[i]["BALANCE_QTY"].ToString());
                        double inqty = Convert.ToDouble(qty);
                        if (rqty >= inqty)
                        {
                            double bqty = rqty - inqty;

                            string Sql = string.Empty;
                            Sql = "Update INVENTORY_ITEM SET  BALANCE_QTY='" + bqty + "' WHERE INVENTORY_ITEM_ID='" + dt.Rows[i]["INVENTORY_ITEM_ID"].ToString() + "'";
                            OracleCommand objCmds1 = new OracleCommand(Sql, objConnT);
                            objCmds1.ExecuteNonQuery();




                            OracleCommand objCmdIn = new OracleCommand("INVITEMTRANSPROC", objConnT);
                            objCmdIn.CommandType = CommandType.StoredProcedure;
                            objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                            objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = item;
                            objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = detid;
                            objCmdIn.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = id;
                            objCmdIn.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = dt.Rows[i]["GRNID"].ToString();
                            objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["INVENTORY_ITEM_ID"].ToString();
                            objCmdIn.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "PROD INPUT";
                            objCmdIn.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "I";
                            objCmdIn.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = qty;
                            objCmdIn.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "PROD INPUT";
                            objCmdIn.Parameters.Add("TRANS_DATE", OracleDbType.Date).Value = DateTime.Now;
                            objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                            objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                            objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                            objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = wopro.Rows[0]["ILOCDETAILSID"].ToString();
                            objCmdIn.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["BRANCH_ID"].ToString();
                            objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                            objCmdIn.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                            objCmdIn.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                            objCmdIn.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                            objCmdIn.ExecuteNonQuery();






                            break;
                        }



                    }
                }
                SvSql1 = "Insert into LSTOCKVALUE (APPROVAL,MAXAPPROVED,CANCEL,T1SOURCEID,LATEMPLATEID,DOCID,DOCDATE,LOTNO,PLUSQTY,MINUSQTY,DRUMNO,RATE,STOCKVALUE,ITEMID,LOCID,BINNO,FROMLOCID,STOCKTRANSTYPE) VALUES ('0','0','F','" + detid + "','754368046','" + wopro.Rows[0]["DOCID"].ToString() + "','" + docdate + "','" + batch + "' ,'0','" + qty + "','" + drum + "','0','0','" + item + "','" + wopro.Rows[0]["ILOCDETAILSID"].ToString() + "','0','0','PROD INPUT' )";
                OracleCommand objCmdss = new OracleCommand(SvSql1, objConnT);
                objCmdss.ExecuteNonQuery();

                double dqty = Convert.ToDouble(qty);
                dt = datatrans.GetData("Select DRUM_STOCK.BALANCE_QTY,DRUM_STOCK.ITEMID,DRUM_STOCK.LOCID,DRUM_STOCK.DRUM_NO,DRUM_ID,DRUM_STOCK_ID from DRUM_STOCK where DRUM_STOCK.ITEMID='" + item + "' AND DRUM_STOCK.LOCID='" + wopro.Rows[0]["ILOCDETAILSID"].ToString() + "' and LOTNO='" + batch + "' and BALANCE_QTY!=0 order by DOC_DATE ASC");
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        double rqty = Convert.ToDouble(dt.Rows[i]["BALANCE_QTY"].ToString());

                        double bqty = rqty - dqty;

                        string Sql = string.Empty;
                        Sql = "Update DRUM_STOCK SET  BALANCE_QTY='" + bqty + "' WHERE DRUM_STOCK_ID='" + dt.Rows[i]["DRUM_STOCK_ID"].ToString() + "'";
                        OracleCommand objCmds2 = new OracleCommand(Sql, objConnT);
                        objCmds.ExecuteNonQuery();




                        OracleCommand objCmdIn = new OracleCommand("DRUMSTKDETPROC", objConnT);
                        objCmdIn.CommandType = CommandType.StoredProcedure;
                        objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                        objCmdIn.Parameters.Add("DRUMSTKID", OracleDbType.NVarchar2).Value = dt.Rows[i]["DRUM_STOCK_ID"].ToString();
                        objCmdIn.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = item;
                        objCmdIn.Parameters.Add("DOCDATE", OracleDbType.Date).Value = DateTime.Now;
                        objCmdIn.Parameters.Add("DRUMNO", OracleDbType.NVarchar2).Value = drumid;
                        objCmdIn.Parameters.Add("T1SOURCEID", OracleDbType.NVarchar2).Value = detid;
                        objCmdIn.Parameters.Add("T1SOURCEBASICID", OracleDbType.NVarchar2).Value = id;
                        objCmdIn.Parameters.Add("SOURCETYPE", OracleDbType.NVarchar2).Value = "BPROD INPUT";
                        objCmdIn.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = wopro.Rows[0]["ILOCDETAILSID"].ToString();
                        objCmdIn.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = work;

                        objCmdIn.Parameters.Add("PLUSQTY", OracleDbType.NVarchar2).Value = "0";
                        objCmdIn.Parameters.Add("MINSQTY", OracleDbType.NVarchar2).Value = qty;
                        objCmdIn.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                        objCmdIn.Parameters.Add("LOTNO", OracleDbType.NVarchar2).Value = batch;
                        objCmdIn.Parameters.Add("SHEDNO", OracleDbType.NVarchar2).Value = "";

                        objCmdIn.ExecuteNonQuery();

                        //string Sqla = string.Empty;
                        //Sqla = "Update DRUMMAST SET  DRUMLOC='10044000011739',IS_EMPTY='Y' WHERE DRUMMASTID='" + cp.drumid + "'";
                        //OracleCommand objCmdsss = new OracleCommand(Sqla, objConnT);
                        //objCmdsss.ExecuteNonQuery();



                    }
                }
            }
        
            //SvSql = "Insert into PYROPRODINPDET (PYROPRODBASICID,ITEMID,BINID,TIME,QTY,STOCK,BATCH,DRUMNO) VALUES ('" + id + "','" + item + "','" + bin + "','" + time + "','" + qty + "','" + stock + "','" + batch + "','"+ drum +"')";
            
          
            return dtt;
        }

        public DataTable SaveConsDetails(string id, string item, string bin, string unit, string usedqty, string qty, string stock,int l)
        {
            string SvSql = string.Empty;
            string SvSql1 = string.Empty;
            string insflag = string.Empty;
            DataTable dtt = new DataTable();
            using (OracleConnection objConnT = new OracleConnection(_connectionString))
            {
                objConnT.Open();
                DataTable drlot = datatrans.GetData("SELECT DRUMYN,LOTYN,ITEMACC,QCCOMPFLAG FROM ITEMMASTER WHERE ITEMMASTERID='" + item + "'");
                DataTable wopro = datatrans.GetData("SELECT WCBASIC.WCID,PROCESSMAST.PROCESSID,NPRODBASIC.DOCID,to_char(NPRODBASIC.DOCDATE,'dd-MM-yy')DOCDATE,ILOCDETAILSID FROM NPRODBASIC LEFT OUTER JOIN WCBASIC ON WCBASICID=NPRODBASIC.WCID LEFT OUTER JOIN PROCESSMAST ON PROCESSMAST.PROCESSMASTID=NPRODBASIC.PROCESSID WHERE NPRODBASICID='" + id + "'");
                string qc = drlot.Rows[0]["QCCOMPFLAG"].ToString();
                string work = wopro.Rows[0]["WCID"].ToString();
                string process = wopro.Rows[0]["PROCESSID"].ToString();
                string docdate = DateTime.Now.ToString("dd-MMM-yyyy");
                string narr = "Consumption in " + work + " - " + process;


                SvSql = "Insert into NPRODCONSDET (NPRODBASICID,CITEMID,CBINID,CUNIT,CLOTYN,CNARR,CONSQTY,CSUBQTY,VALIDROW3,NPRODCONSDETROW,CITEMACC,IS_INSERT) VALUES ('" + id + "','" + item + "','0','" + unit + "','" + drlot.Rows[0]["LOTYN"].ToString() + "','" + narr + "','" + qty + "','" + usedqty + "','T','" + l + "','" + drlot.Rows[0]["ITEMACC"].ToString() + "','Y') RETURNING NPRODCONSDETID INTO :LASTCID";
                OracleCommand objCmds = new OracleCommand(SvSql, objConnT);

                objCmds.Parameters.Add("LASTCID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                objCmds.ExecuteNonQuery();
                string detid = objCmds.Parameters["LASTCID"].Value.ToString();
                if (qc == "YES") { insflag = "0"; } else { insflag = "1"; }
                DataTable lstock = datatrans.GetData("SELECT ITEMID,T1SOURCEID,QTY FROM STOCKVALUE WHERE ITEMID='" + item + "' and T1SOURCEID='" + id + "'");
                if (lstock.Rows.Count > 0)
                {
                    double sqty = Convert.ToDouble(lstock.Rows[0]["QTY"].ToString());
                    double iqty = Convert.ToDouble(qty);
                    double totqty = sqty + iqty;
                    SvSql = "UPDATE STOCKVALUE SET QTY='" + totqty + "' WHERE ITEMID='" + item + "' and T1SOURCEID='" + id + "'";
                    OracleCommand objCmdss1 = new OracleCommand(SvSql, objConnT);
                    objCmdss1.ExecuteNonQuery();
                }
                else
                {
                    SvSql1 = "Insert into STOCKVALUE (T1SOURCEID,PLUSORMINUS,ITEMID,DOCDATE,QTY,LOCID,BINID,RATEC,PROCESSID,SNO,SCSID,SVID,FROMLOCID,STOCKTRANSTYPE,SINSFLAG) VALUES ('" + detid + "','m','" + item + "','" + docdate + "','" + qty + "' ,'" + wopro.Rows[0]["ILOCDETAILSID"].ToString() + "','0','0','0','0','0','0','0','PROD CONS','" + insflag + "')";
                    OracleCommand objCmdss = new OracleCommand(SvSql1, objConnT);
                    objCmdss.ExecuteNonQuery();
                }
                // SvSql = "Insert into PYROPRODCONSDET (PYROPRODBASICID,ITEMID,BINID,UNITID,QTY,CONSQTY,STOCK) VALUES ('" + id + "','" + item + "','" + bin + "','" + unit + "','" + usedqty + "','" + qty + "','" + stock + "')";
            }
            return dtt;
        }
        public DataTable SaveOutputDetails(string id, string item , string stime, string ttime, string qty, string drum,string status, string stock,string excess,string shed)
        {
            string SvSql = string.Empty;
            string loc = string.Empty;
            using (OracleConnection objConnT = new OracleConnection(_connectionString))
            {
                objConnT.Open();
                string drumno = datatrans.GetDataString("SELECT DRUMNO FROM DRUMMAST WHERE DRUMMASTID='" + drum + "'");
                string itemname = datatrans.GetDataString("SELECT ITEMID FROM ITEMMASTER WHERE ITEMMASTERID='" + item + "'");
                string qcyn = datatrans.GetDataString("SELECT QCCOMPFLAG FROM ITEMMASTER WHERE ITEMMASTERID='" + item + "'");
                DataTable ld = datatrans.GetData("SELECT DRUMYN,LOTYN FROM ITEMMASTER WHERE ITEMMASTERID='" + item + "'");
                DataTable wopro = datatrans.GetData("SELECT BRANCHID,WCBASIC.WCID,WCBASIC.ILOCATION,PROCESSMAST.PROCESSID,NPRODBASIC.DOCID,NPRODBASIC.ENTEREDBY,DRUMILOCDETAILSID,PSBASIC.DOCID as psno,SHIFT,to_char(NPRODBASIC.DOCDATE,'dd-MM-yy')DOCDATE,ILOCDETAILSID FROM NPRODBASIC LEFT OUTER JOIN WCBASIC ON WCBASICID=NPRODBASIC.WCID LEFT OUTER JOIN PROCESSMAST ON PROCESSMAST.PROCESSMASTID=NPRODBASIC.PROCESSID LEFT OUTER JOIN PSBASIC ON PSBASIC.PSBASICID=NPRODBASIC.PSCHNO WHERE NPRODBASICID='" + id + "'");
                string work = wopro.Rows[0]["WCID"].ToString();
                string process = wopro.Rows[0]["PROCESSID"].ToString();
                
                string doc = wopro.Rows[0]["DOCID"].ToString();
                string psno = wopro.Rows[0]["psno"].ToString();
                string shift = wopro.Rows[0]["SHIFT"].ToString();
                string drumloc = wopro.Rows[0]["DRUMILOCDETAILSID"].ToString();
                string lot = ld.Rows[0]["LOTYN"].ToString();
                string drumyn = ld.Rows[0]["DRUMYN"].ToString();
                string obatch = "";
                string ins = "";
                string sflag = "1";
                string docdate = DateTime.Now.ToString("dd-MMM-yyyy");
                if (qcyn == "YES")
                {
                    ins = "0";
                }
                else { ins = "1"; }
                if (stock == "0")
                {
                    obatch = "None";
                }
                else
                {
                    obatch = itemname + " - " + work + " - " + drumno + " - " + doc;
                }
                if (status == "COMPLETED") { loc = "10044000011739"; } else { loc = wopro.Rows[0]["ILOCATION"].ToString(); }
                string nbatch = itemname + " - " + work + " - " + drumno + " - " + doc;
                string narr = "Production in " + work + " - " + process + " - " + drumno;
                SvSql = "Insert into NPRODOUTDET (NPRODBASICID,OITEMID,ODRUMNO,STIME,OQTY,ETIME,OXQTY,STATUS,OSTOCK,TOLOCATION,TOLOCDETAILSID,OCDRUMNO,LWCID,LPROCESS,LSHIFT,LSCH,OBINID,OBATCHNO,NBATCHNO,ONARR,INSFLAGCTRL,ODRUMYN,OLOTYN,DSDT,DSTIME,DEDT,DETIME,OBQTY,DRUMLOADID,OQCYN,ODRUMSLOCID,SFLAG,SHEDNUMBER) VALUES ('" + id + "','" + item + "','" + drum + "','" + stime + "','" + qty + "','" + ttime + "','" + excess + "','" + status + "','" + stock + "','" + loc + "','" + loc + "','" + drumno + "','" + work + "','" + process + "','" + shift + "','" + psno + "','0','" + obatch + "','" + nbatch + "','" + narr + "','" + ins + "','"+ drumyn +"','"+lot+"','"+ docdate+"','"+ stime + "','"+ docdate +"','"+ ttime +"','"+excess +"','0','"+ qcyn +"','"+ drumloc + "','"+ sflag +"','"+ shed +"') RETURNING NPRODOUTDETID INTO :LASTCID";
                OracleCommand objCmds = new OracleCommand(SvSql, objConnT);

                objCmds.Parameters.Add("LASTCID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                objCmds.ExecuteNonQuery();

                string detid = objCmds.Parameters["LASTCID"].Value.ToString();
                if (status == "COMPLETED")
                {
                    SvSql = " INSERT INTO LOTMAST (APPROVAL, MAXAPPROVED, CANCEL, T1SOURCEID, LATEMPLATEID, ITEMID, PARTYID, RATE, DOCID, DOCDATE, QTY, LOTNO, DRUMNO, LOCATION, INSFLAG, RCFLAG, PRODTYPE, AMOUNT, QCRELASEFLAG, ESTATUS, COMPFLAG, PACKFLAG, SHIFT, STARTTIME, ENDTIME, CURINWFLAG, CUROUTFLAG, PACKINSFLAG, PSCHNO, MATCOST, MCCOST, EMPCOST, OTHERCOST, ADMINCOST, GENSETCOST, EBCOST, EBUNITRATE, DIESELRATE, TESTINSFLAG, FIDRMS, DRMPRF, PACKDRMNO, WCID, ProcessID,SHEDNO) Values(0, 0, 'F', '" + detid + "', '748213892', '" + item + "', '0', '0', '" + wopro.Rows[0]["DOCID"].ToString() + "', '" + docdate + "','" + qty + "','" + nbatch + "', '" + drumno + "', '" + loc + "', '" + ins + "', '0', 'PROD','0', '0', '0','" + sflag + "',' 0', '" + shift + "', '" + stime + "', '" + ttime + "', '1', '0', 0, '"+ psno + "', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, '', '', '"+work+ "','" + process + "','" + shed + "')";
                    objCmds = new OracleCommand(SvSql, objConnT);
                    objCmds.ExecuteNonQuery();
                    SvSql = "  INSERT INTO LSTOCKVALUE (APPROVAL, MAXAPPROVED, CANCEL, T1SOURCEID, LATEMPLATEID, DOCID, DOCDATE, Drumno, LOTNO, PLUSQTY, MINUSQTY, RATE, STOCKVALUE, ITEMID, LOCID, BINNO, FROMLOCID, StockTranstype, batch) VALUES ('0', '0', 'F', '" + detid + "', 748213892, '" + wopro.Rows[0]["DOCID"].ToString() + "',  '" + docdate + "', '" + drumno + "','" + nbatch + "','" + qty + "', '0', '0','0', '" + item + "','" + loc + "', '0', '0', 'PROD OUTPUT', '')";
                    objCmds = new OracleCommand(SvSql, objConnT);
                    objCmds.ExecuteNonQuery();

                    int occupied = datatrans.GetDataId(" SELECT OCCUPIED FROM CURINGMASTER WHERE SHEDNUMBER = '" + shed + "' ");


                    string updateoccupied = " UPDATE CURINGMASTER SET OCCUPIED ='" + (occupied + 1).ToString() + "' WHERE SHEDNUMBER ='" + shed + "'";

                    datatrans.UpdateStatus(updateoccupied);
                    string Sql = "Update CURINGMASTER SET  STATUS='Occupied' WHERE SHEDNUMBER='" + shed + "'";
                    objCmds = new OracleCommand(Sql, objConnT);
                    objCmds.ExecuteNonQuery();
                    datatrans = new DataTransactions(_connectionString);


                    int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'CURI' AND ACTIVESEQUENCE = 'T'");
                    string curid = string.Format("{0}{1}", "CURI", (idc + 1).ToString());

                    string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='CURI' AND ACTIVESEQUENCE ='T'";
                    try
                    {
                        datatrans.UpdateStatus(updateCMd);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    DataTable shiftid = datatrans.GetData("Select SHIFTMASTID,FROMTIME,TOTIME from SHIFTMAST where SHIFTNO='" + shift + "' ");

                    
                    OracleCommand objCmdsss = new OracleCommand("CURINGINWARDPPROC", objConnT);
                    objCmdsss.CommandType = CommandType.StoredProcedure;
                    objCmdsss.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    objCmdsss.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = wopro.Rows[0]["BRANCHID"].ToString();
                    objCmdsss.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = curid;
                    objCmdsss.Parameters.Add("DOCDATE", OracleDbType.Date).Value = DateTime.Now;
                    objCmdsss.Parameters.Add("LOCDETAILSID", OracleDbType.NVarchar2).Value = "10044000011739";
                    objCmdsss.Parameters.Add("SHIFT", OracleDbType.NVarchar2).Value = shiftid.Rows[0]["SHIFTMASTID"].ToString();
                    objCmdsss.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = "10932000000839";
                    objCmdsss.Parameters.Add("STARTTIME", OracleDbType.NVarchar2).Value = shiftid.Rows[0]["FROMTIME"].ToString();
                    objCmdsss.Parameters.Add("ENDTIME", OracleDbType.NVarchar2).Value = shiftid.Rows[0]["TOTIME"].ToString();
                    objCmdsss.Parameters.Add("ENTEREDBY", OracleDbType.NVarchar2).Value = wopro.Rows[0]["ENTEREDBY"].ToString(); 
                    objCmdsss.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = "";
                    objCmdsss.Parameters.Add("NPRODBASICID", OracleDbType.NVarchar2).Value = id;
                    objCmdsss.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                    objCmdsss.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;

                    try
                    {

                        objCmdsss.ExecuteNonQuery();
                        Object crdid = objCmdsss.Parameters["OUTID"].Value;

                        
                        
                        string batch = string.Format("{0}-{1}", item, drum.ToString());
                        DataTable dttt = datatrans.GetData("Select SHEDNUMBER,CAPACITY,OCCUPIED from CURINGMASTER where SHEDNUMBER='" + shed + "'");
                        int curday = datatrans.GetDataId("Select CURINGDAY from ITEMMASTER where ITEMMASTERID='" + item + "' ");
                        DateTime due = DateTime.Now.AddDays(curday);
                        string dueda = due.ToString("dd-MMM-yyyy");
                        for (int i = 0; i < dttt.Rows.Count; i++)
                        {

                            OracleCommand objCmdInp1 = new OracleCommand("CURINPDETAILPROC", objConnT);

                            objCmdInp1.CommandType = CommandType.StoredProcedure;
                            objCmdInp1.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                            objCmdInp1.Parameters.Add("CURINGESBASICID", OracleDbType.NVarchar2).Value = crdid;
                            objCmdInp1.Parameters.Add("FDATE", OracleDbType.Date).Value = DateTime.Now;
                            objCmdInp1.Parameters.Add("FTIME", OracleDbType.NVarchar2).Value = shiftid.Rows[0]["FROMTIME"].ToString();
                            objCmdInp1.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = item;
                            objCmdInp1.Parameters.Add("DRUMNO", OracleDbType.NVarchar2).Value = drumno;
                            objCmdInp1.Parameters.Add("BATCHNO", OracleDbType.NVarchar2).Value = nbatch;
                            objCmdInp1.Parameters.Add("BATCHQTY", OracleDbType.NVarchar2).Value = qty;
                            objCmdInp1.Parameters.Add("BINID", OracleDbType.NVarchar2).Value = "";
                            objCmdInp1.Parameters.Add("CURDAY", OracleDbType.NVarchar2).Value = curday;
                            objCmdInp1.Parameters.Add("BINMASTERID", OracleDbType.NVarchar2).Value = "";
                            objCmdInp1.Parameters.Add("CAPACITY", OracleDbType.NVarchar2).Value = dttt.Rows[i]["CAPACITY"].ToString();
                            objCmdInp1.Parameters.Add("OCCUPIED", OracleDbType.NVarchar2).Value = dttt.Rows[i]["OCCUPIED"].ToString();
                            objCmdInp1.Parameters.Add("DUEDATE", OracleDbType.Date).Value = due;
                            objCmdInp1.Parameters.Add("CBINID", OracleDbType.NVarchar2).Value = dttt.Rows[i]["SHEDNUMBER"].ToString();
                            objCmdInp1.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";

                            objCmdInp1.ExecuteNonQuery();
                            // Sql = string.Empty;
                            //Sql = "Update DRUM_STOCK SET  CURINGDUEDATE='" + dueda + "' where DRUM_STOCK_ID='" + Pid1 + "' ";
                            //OracleCommand objCmdsd = new OracleCommand(Sql, objConnT);
                            //objCmdsd.ExecuteNonQuery();

                        }
                    }
                    catch (Exception ex)
                    {
                        //System.Console.WriteLine("Exception: {0}", ex.ToString());
                    }
                }
            }
            // SvSql = "Insert into PYROPRODOUTDET (PYROPRODBASICID,ITEMID,BINID,STARTTIME,ENDTIME,OUTQTY,DRUM) VALUES ('" + id + "','" + item + "','" + bin + "','" + stime + "','" + ttime + "','" + qty + "','" + drum + "')";
            DataTable dtt = new DataTable();
           
            return dtt;
        }
        public DataTable SaveEmpDetails(string id, string empname, string code, string depat, string sdate, string stime, string edate, string etime, string ot, string et, string normal, string now)
        {
            string SvSql = string.Empty;
            using (OracleConnection objConnT = new OracleConnection(_connectionString))
            {
                objConnT.Open();
                SvSql = "Delete NPRODEMPDET WHERE NPRODBASICID='" + id + "'";
                OracleCommand objCmdd = new OracleCommand(SvSql, objConnT);
                objCmdd.ExecuteNonQuery();
            }
            SvSql = "Insert into NPRODEMPDET (NPRODBASICID,EMPNAME,EMPCODE1,DEPARTMENT,ESTARTDATE,ESTARTTIME,EENDDATE,EENDTIME,OTHRS,ETOTHRS,NORMHRS,NATOFW) VALUES ('" + id + "','" + empname + "','" + code + "','" + depat + "','" + sdate + "','" + stime + "','" + edate + "','" + etime + "','" + ot + "','" + et + "','" + normal + "','" + now + "')";

            // SvSql = "Insert into PYROPRODEMPDET (PYROPRODBASICID,EMPID,EMPCODE,DEPARTMENT,STARTDATE,STARTTIME,ENDDATE,ENDTIME,OTHRS,ETOTHER,NORMELHRS,NATUREOFWORK) VALUES ('" + id + "','" + empname + "','" + code + "','" + depat + "','" + sdate + "','" + stime + "','" + edate + "','" + etime + "','" + ot + "','" + et + "','" + normal + "','" + now + "')";

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
                SvSql = "Delete NPRODBRKDWN WHERE NPRODBASICID='" + id + "'";
                OracleCommand objCmdd = new OracleCommand(SvSql, objConnT);
                objCmdd.ExecuteNonQuery();
            }
            SvSql = "Insert into NPRODBRKDWN (NPRODBASICID,BMACNO,BMACHINEDESC,DTYPE,MTYPE,BFROMTIME,BTOTIME,PREORBRE,ALLOTEDTO,ACTDESC) VALUES ('" + id + "','" + machine + "','" + des + "','" + dtype + "','" + mtype + "','" + stime + "','" + etime + "','" + pb + "','" + all + "','" + reason + "')";

            // SvSql = "Insert into PYROPRODBREAKDET (PYROPRODBASICID,MEACHINECODE,MEACHDES,DTYPE,MTYPE,STARTTIME,ENDTIME,PB,ALLOTTEDTO,REASON) VALUES ('" + id + "','" + machine + "','" + des + "','" + dtype + "','" + mtype + "','" + stime + "','" + etime + "','" + pb + "','" + all + "','" + reason + "')";

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
                SvSql = "Delete NPRODLOGDET WHERE NPRODBASICID='" + id + "'";
                OracleCommand objCmdd = new OracleCommand(SvSql, objConnT);
                objCmdd.ExecuteNonQuery();
            }
            SvSql = "Insert into NPRODLOGDET (NPRODBASICID,STARTDATE,STARTTIME,ENDDATE,ENDTIME,TOTALHRS,REASON) VALUES ('" + id + "','" + sdate + "','" + stime + "','" + edate + "','" + etime + "','" + tot + "','" + reason + "')";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable SaveOutsDetails(string id, string noofemp, string sdate, string stime, string edate, string etime, string workhrs, string cost, string expence, string now)

        {
            string SvSql = string.Empty;
            using (OracleConnection objConnT = new OracleConnection(_connectionString))
            {
                objConnT.Open();
                SvSql = "Delete NPRODOUTS WHERE NPRODBASICID='" + id + "'";
                OracleCommand objCmdd = new OracleCommand(SvSql, objConnT);
                objCmdd.ExecuteNonQuery();
            }

            SvSql = "Insert into NPRODOUTS (NPRODBASICID,NOOFEMP,OWSTDTT,OWSTT,OWEDDTT,OWEDT,EMPWHRS,EMPPAY,MANPOWEXP,ONATOFW) VALUES ('" + id + "','" + noofemp + "','" + sdate + "','" + stime + "','" + edate + "','" + etime + "','" + workhrs + "','" + cost + "','" + expence + "','" + now + "')";

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

        public string SaveBasicDetail(string schno, string docid, string docdate, string loc, string proc, string shift, string schqty, string prodqty, string wcid, string proclot,string branchid, string enterd)
        {
            string msg = "";
            string Pid = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                int idc = datatrans.GetDataId("select LASTNO  from sequence where TRANSTYPE='nProd' AND ACTIVESEQUENCE='T'");
                DataTable dt=new DataTable();
                dt = datatrans.GetData("select ILOCATION,RLOCATION,WIPLOCID,WIPITEMID,REJLOCATION,EBCONSPERHR,DRUMILOCATION from wcbasic where WCBASICID='" + wcid + "'");
                string iloc = "";
                string rloc = "";string rejlocid = "";string drumlocid = "";
                string wiplocid = "";string wipitemd = "";string ebhr = "";
                if (dt.Rows.Count > 0)
                {
                    iloc = dt.Rows[0]["ILOCATION"].ToString();
                    rloc= dt.Rows[0]["RLOCATION"].ToString();
                    wiplocid= dt.Rows[0]["WIPLOCID"].ToString();
                    wipitemd= dt.Rows[0]["WIPITEMID"].ToString();
                    rejlocid = dt.Rows[0]["REJLOCATION"].ToString();
                    ebhr= dt.Rows[0]["EBCONSPERHR"].ToString();
                    drumlocid = dt.Rows[0]["DRUMILOCATION"].ToString();
                }
                DataTable dtt = new DataTable();
                string fromtime = "";
                string totime = "";
                string tothrs = "";
                dtt = datatrans.GetData("Select FROMTIME,TOTIME,SHIFTHRS from SHIFTMAST where SHIFTNO='" + shift + "'");
                if (dtt.Rows.Count > 0)
                {

                    fromtime = dtt.Rows[0]["FROMTIME"].ToString();
                    totime = dtt.Rows[0]["TOTIME"].ToString();
                    tothrs = dtt.Rows[0]["SHIFTHRS"].ToString();
                }
                DataTable dt2 = new DataTable();
                dt2 = datatrans.GetData("select DRUMRETURNYN,PRODHRTYPE from PROCESSMAST where PROCESSMASTID='"+ proc + "'");
                string drmreturnyn = "";
                string prohrtype = "";
                if(dt2.Rows.Count > 0)
                {
                    drmreturnyn= dt2.Rows[0]["DRUMRETURNYN"].ToString();
                    prohrtype= dt2.Rows[0]["PRODHRTYPE"].ToString();
                }

                DateTime fdate = DateTime.Now;
                string entdate = DateTime.Now.ToString("dd/MMM/yyyy  h:mm:ss" );
               DateTime tdate = fdate.AddHours(Convert.ToDouble(tothrs == "" ?0 : tothrs));
                using (OracleConnection objConn = new OracleConnection(_connectionString))

                {
                    objConn.Open();

                    using (OracleCommand command = objConn.CreateCommand())
                    {
                        using (OracleTransaction transaction = objConn.BeginTransaction(IsolationLevel.ReadCommitted))
                        {
                            command.Transaction = transaction;

                            try
                            {
                                command.CommandText = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE TRANSTYPE='nProd' AND ACTIVESEQUENCE='T'";
                                command.ExecuteNonQuery();

                                command.CommandText = "insert into NPRODBASIC (APPROVAL,MAXAPPROVED,CANCEL,BRANCH,DOCID,DOCDATE,PROCESSID,WCID,PROCLOTNO,PSCHNO,ETYPE,ILOCDETAILSID,RLOCDETAILSID,WIPLOCDETAILSID,SHIFT,STARTDATE,STARTTIME,ENDDATE,ENDTIME,TOTMINS,TOTHRS,REJLOCDETAILSID,WIPITEMMASTERID,WIPITEMID,PTYPE,IBINYN,SCHQTY,PRODQTY,EBHRS,PROCESSMASTID,LSTARTDT,LSTARTTIME,LENDDT,LENDTIME,LHOUR,ILOCID,RLOCID,DRUMILOCDETAILSID,DRUMRETURNYN,PRODHRTYPE,ENTEREDBY,ENTDATE) Values ('0','0','F','" + branchid + "','" + docid + "','" + docdate + "','" + proc + "','" + wcid + "','" + proclot + "','" + schno + "','BOTH','" + iloc + "','" + rloc + "','" + wiplocid + "','" + shift + "','" + fdate.ToString("dd-MMM-yyyy") + "','" + fromtime + "','" + tdate.ToString("dd-MMM-yyyy") + "','" + totime + "',0,'" + tothrs + "','" + rejlocid + "','" + wipitemd + "','" + wipitemd + "','EB','NO','" + schqty + "','" + prodqty + "','" + ebhr + "','" + proc + "','" + fdate.ToString("dd-MMM-yyyy") + "','" + fromtime + "','" + tdate.ToString("dd-MMM-yyyy") + "','" + totime + "','" + tothrs + "','" + iloc + "','" + rloc + "','" + drumlocid + "','" + drmreturnyn + "','" + prohrtype + "','"+ enterd +"','"+ entdate+"') RETURNING NPRODBASICID INTO :LASTCID";
                                command.Parameters.Add("LASTCID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                command.ExecuteNonQuery();
                                Pid = command.Parameters["LASTCID"].Value.ToString();
                                


                                transaction.Commit();
                            }
                            catch (DataException e)
                            {
                                transaction.Rollback();
                                Console.WriteLine(e.ToString());
                                Console.WriteLine("Neither record was written to database.");
                            }
                        }
                    }
                }

                            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return Pid;
        }
        public string ApprovePyroProductionEntryGURD(PyroProductionentryDet cy)
        {
            string msg = "";

            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    objConn.Open();
                    if (cy.inplst != null)
                    {
                        foreach (PProInput cp in cy.inplst)
                        {
                            if (cp.Isvalid == "Y" && cp.saveitemId != "0")
                            {
                                //string locid = datatrans.GetDataString("Select ILOCATION from WCBASIC where WCBASICID='10035000000038' ");

                                ///////////////////////////// Input Inventory
                                double qty = cp.IssueQty;
                                DataTable dt = datatrans.GetData("Select DRUM_STOCK.BALANCE_QTY,DRUM_STOCK.ITEMID,DRUM_STOCK.LOCID,DRUM_STOCK.DRUM_NO,DRUM_ID,DRUM_STOCK_ID from DRUM_STOCK where DRUM_STOCK.ITEMID='" + cp.saveitemId + "' AND DRUM_STOCK.LOCID='10035000000038' and DRUM_NO='"+ cp.drumno+"' and BALANCE_QTY!=0 order by DOC_DATE ASC");
                                if (dt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        double rqty = Convert.ToDouble(dt.Rows[i]["BALANCE_QTY"].ToString());

                                        double bqty = rqty - qty;

                                        string Sql = string.Empty;
                                        Sql = "Update DRUM_STOCK SET  BALANCE_QTY='" + bqty + "' WHERE DRUM_STOCK_ID='" + dt.Rows[i]["DRUM_STOCK_ID"].ToString() +"'";
                                        OracleCommand objCmds = new OracleCommand(Sql, objConn);
                                        objCmds.ExecuteNonQuery();

                                        string wcid = datatrans.GetDataString("Select WCBASICID from WCBASIC where ILOCATION='" + dt.Rows[i]["LOCID"].ToString() + "' ");
                                        string wc = datatrans.GetDataString("Select WCID from WCBASIC where WCBASICID='" + wcid + "' ");
                                        string item = cp.ItemId;
                                        string drum = cp.drumno;
                                        string wcenter = wc;
                                        string docid = string.Format("{0}-{1}-{2}", item, wcenter, drum.ToString());


                                        OracleCommand objCmdIn = new OracleCommand("DRUMSTKDETPROC", objConn);
                                        objCmdIn.CommandType = CommandType.StoredProcedure;
                                        objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                        objCmdIn.Parameters.Add("DRUMSTKID", OracleDbType.NVarchar2).Value = dt.Rows[i]["DRUM_STOCK_ID"].ToString();
                                        objCmdIn.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.saveitemId;
                                        objCmdIn.Parameters.Add("DOCDATE", OracleDbType.Date).Value = DateTime.Now;
                                        objCmdIn.Parameters.Add("DRUMNO", OracleDbType.NVarchar2).Value = cp.drumid;
                                        objCmdIn.Parameters.Add("T1SOURCEID", OracleDbType.NVarchar2).Value = cp.inpid;
                                        objCmdIn.Parameters.Add("T1SOURCEBASICID", OracleDbType.NVarchar2).Value = cy.ID;
                                        objCmdIn.Parameters.Add("SOURCETYPE", OracleDbType.NVarchar2).Value = "PYROINPDET";
                                        objCmdIn.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = dt.Rows[i]["LOCID"].ToString();
                                        objCmdIn.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = wcid;
                                      
                                        objCmdIn.Parameters.Add("PLUSQTY", OracleDbType.NVarchar2).Value = "0"; 
                                        objCmdIn.Parameters.Add("MINSQTY", OracleDbType.NVarchar2).Value = qty;
                                        objCmdIn.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                        objCmdIn.Parameters.Add("LOTNO", OracleDbType.NVarchar2).Value = docid;
                                        objCmdIn.Parameters.Add("SHEDNO", OracleDbType.NVarchar2).Value = "";
                                        
                                        objCmdIn.ExecuteNonQuery();

                                        string Sqla = string.Empty;
                                        Sqla = "Update DRUMMAST SET  DRUMLOC='10044000011739',IS_EMPTY='Y' WHERE DRUMMASTID='" + cp.drumid + "'";
                                        OracleCommand objCmdsss = new OracleCommand(Sqla, objConn);
                                        objCmdsss.ExecuteNonQuery();



                                    }
                                        }



                                    }
                                }
                                ///////////////////////////// Input Inventory

                            }

                    if (cy.Binconslst != null)
                    {
                        foreach (PAPProInCons cp in cy.Binconslst)
                        {
                            if (cp.Isvalid == "Y" && cp.saveitemId != "0")
                            {
                                ///////////////////////////// Input Inventory
                                double qty = cp.consQty;
                                DataTable dt = datatrans.GetData("Select INVENTORY_ITEM.BALANCE_QTY,INVENTORY_ITEM.ITEM_ID,INVENTORY_ITEM.LOCATION_ID,INVENTORY_ITEM.BRANCH_ID,INVENTORY_ITEM_ID,GRNID,GRN_DATE from INVENTORY_ITEM where INVENTORY_ITEM.ITEM_ID='" + cp.saveitemId + "' AND INVENTORY_ITEM.LOCATION_ID='10035000000038' and INVENTORY_ITEM.BRANCH_ID='" + cy.BranchId + "' and BALANCE_QTY!=0 order by GRN_DATE ASC");
                                if (dt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        double rqty = Convert.ToDouble(dt.Rows[i]["BALANCE_QTY"].ToString());
                                        if (rqty >= qty)
                                        {
                                            double bqty = rqty - qty;

                                            string Sql = string.Empty;
                                            Sql = "Update INVENTORY_ITEM SET  BALANCE_QTY='" + bqty + "' WHERE INVENTORY_ITEM_ID='" + dt.Rows[i]["INVENTORY_ITEM_ID"].ToString() + "'";
                                            OracleCommand objCmds = new OracleCommand(Sql, objConn);
                                            objCmds.ExecuteNonQuery();




                                            OracleCommand objCmdIn = new OracleCommand("INVITEMTRANSPROC", objConn);
                                            objCmdIn.CommandType = CommandType.StoredProcedure;
                                            objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                            objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = cp.saveitemId;
                                            objCmdIn.Parameters.Add("T1SOURCEID", OracleDbType.NVarchar2).Value = cp.consid;
                                            objCmdIn.Parameters.Add("T1SOURCEBASICID", OracleDbType.NVarchar2).Value = cy.ID;
                                            objCmdIn.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = dt.Rows[i]["GRNID"].ToString();
                                            objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["INVENTORY_ITEM_ID"].ToString();
                                            objCmdIn.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "PYROPROD";
                                            objCmdIn.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "O";
                                            objCmdIn.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = qty;
                                            objCmdIn.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "PYROPROD";
                                            objCmdIn.Parameters.Add("TRANS_DATE", OracleDbType.Date).Value = DateTime.Now;
                                            objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                            objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                            objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                            objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cy.LOCID;
                                            objCmdIn.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.BranchId;
                                            objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                            objCmdIn.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                            objCmdIn.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                            objCmdIn.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                            objCmdIn.ExecuteNonQuery();



                                            break;
                                        }
                                        else
                                        {
                                            qty = qty - rqty;

                                            /////////////////////////////////Outward Entry
                                            string Sql = string.Empty;
                                            Sql = "Update INVENTORY_ITEM SET  BALANCE_QTY='" + rqty + "' WHERE INVENTORY_ITEM_ID='" + dt.Rows[i]["INVENTORY_ITEM_ID"].ToString() + "'";
                                            OracleCommand objCmds = new OracleCommand(Sql, objConn);
                                            objCmds.ExecuteNonQuery();


                                            OracleCommand objCmdIn = new OracleCommand("INVITEMTRANSPROC", objConn);
                                            objCmdIn.CommandType = CommandType.StoredProcedure;
                                            objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                            objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = cp.saveitemId;
                                            objCmdIn.Parameters.Add("T1SOURCEID", OracleDbType.NVarchar2).Value = cp.consid;
                                            objCmdIn.Parameters.Add("T1SOURCEBASICID", OracleDbType.NVarchar2).Value = cy.ID;
                                            objCmdIn.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = dt.Rows[i]["GRNID"].ToString();
                                            objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["INVENTORY_ITEM_ID"].ToString();
                                            objCmdIn.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "PYROPROD";
                                            objCmdIn.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "O";
                                            objCmdIn.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = rqty;
                                            objCmdIn.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "PYROPROD";
                                            objCmdIn.Parameters.Add("TRANS_DATE", OracleDbType.Date).Value = DateTime.Now;
                                            objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                            objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                            objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                            objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cy.LOCID;
                                            objCmdIn.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.BranchId;
                                            objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                            objCmdIn.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                            objCmdIn.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                            objCmdIn.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                            objCmdIn.ExecuteNonQuery();



                                        }



                                    }
                                }
                                ///////////////////////////// Input Inventory

                            }

                        }
                    }

                   
                    

                    if (cy.outlst != null)
                    {
                        foreach (PProOutput cp in cy.outlst)
                        {
                            if (cp.Isvalid == "Y" && cp.saveitemId != "0")
                            {
                                string wcid = datatrans.GetDataString("Select WCBASICID from WCBASIC where ILOCATION='10044000011739' ");
                                /////////////////////////output inventory
                                double qty = cp.OutputQty;
                                OracleCommand objCmdIn = new OracleCommand("DRUMSTKPROC", objConn);
                                objCmdIn.CommandType = CommandType.StoredProcedure;
                                objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                objCmdIn.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.saveitemId;
                                objCmdIn.Parameters.Add("DOC_DATE", OracleDbType.Date).Value = DateTime.Now;
                                objCmdIn.Parameters.Add("DRUM_ID", OracleDbType.NVarchar2).Value = cp.drumid;
                                objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = cp.drumno;
                                objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = cp.outid;
                                objCmdIn.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = cy.ID;
                                objCmdIn.Parameters.Add("STOCKTRANSTYPE", OracleDbType.NVarchar2).Value = "PYROPRODOUT";
                                objCmdIn.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = "10044000011739";
                                objCmdIn.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = wcid;
                                objCmdIn.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = qty;
                                objCmdIn.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = qty;
                                objCmdIn.Parameters.Add("OUT_ID", OracleDbType.NVarchar2).Value = "0";
                                objCmdIn.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                                objCmdIn.ExecuteNonQuery();
                                Object Pid1 = objCmdIn.Parameters["OUTID"].Value;
                                try
                                {

                                   

                                    //if (cy.ID != null)
                                    //{
                                    //    Pid = cy.ID;
                                    //}
                                    
                               
                               
                              
                                string wc = datatrans.GetDataString("Select WCID from WCBASIC where WCBASICID='" + wcid + "' ");
                                string item = cp.ItemId;
                                string drum = cp.drumno;
                                string wcenter = wc;
                                string docid = string.Format("{0}-{1}-{2}", item, wcenter, drum.ToString());


                                OracleCommand objCmdInp = new OracleCommand("DRUMSTKDETPROC", objConn);
                                objCmdInp.CommandType = CommandType.StoredProcedure;
                                objCmdInp.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                objCmdInp.Parameters.Add("DRUMSTKID", OracleDbType.NVarchar2).Value = Pid1;
                                objCmdInp.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.saveitemId;
                                objCmdInp.Parameters.Add("DOCDATE", OracleDbType.Date).Value = DateTime.Now;
                                objCmdInp.Parameters.Add("DRUMNO", OracleDbType.NVarchar2).Value = cp.drumid;
                                objCmdInp.Parameters.Add("DRUM", OracleDbType.NVarchar2).Value = cp.drumno;
                                objCmdInp.Parameters.Add("T1SOURCEID", OracleDbType.NVarchar2).Value = cp.outid;
                                objCmdInp.Parameters.Add("T1SOURCEBASICID", OracleDbType.NVarchar2).Value = cy.ID;
                                objCmdInp.Parameters.Add("SOURCETYPE", OracleDbType.NVarchar2).Value = "PYROOUTDET";
                                objCmdInp.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = "10044000011739";
                                objCmdInp.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = wcid;

                                objCmdInp.Parameters.Add("PLUSQTY", OracleDbType.NVarchar2).Value = qty;
                                objCmdInp.Parameters.Add("MINSQTY", OracleDbType.NVarchar2).Value = "0";
                                objCmdInp.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                objCmdInp.Parameters.Add("LOTNO", OracleDbType.NVarchar2).Value = docid;
                                objCmdInp.Parameters.Add("SHEDNO", OracleDbType.NVarchar2).Value = cp.ShedNo;

                                objCmdInp.ExecuteNonQuery();
                                    string Sql = string.Empty;
                                    Sql = "Update DRUMMAST SET  DRUMLOC='10044000011739',IS_EMPTY='N' WHERE DRUMNO='" +cp.drumno+ "'";
                                    OracleCommand objCmds = new OracleCommand(Sql, objConn);
                                    objCmds.ExecuteNonQuery();
                                    Sql = "Update PYROPRODBASIC SET  IS_APPROVED='Y' WHERE PYROPRODBASICID='" + cy.ID + "'";
                                     objCmds = new OracleCommand(Sql, objConn);
                                    objCmds.ExecuteNonQuery();
                                    Sql = "Update CURINGMASTER SET  STATUS='Occupied' WHERE SHEDNUMBER='" + cp.ShedNo + "'";
                                    objCmds = new OracleCommand(Sql, objConn);
                                    objCmds.ExecuteNonQuery();
                                    datatrans = new DataTransactions(_connectionString);


                                    int occupied = datatrans.GetDataId(" SELECT OCCUPIED FROM CURINGMASTER WHERE SHEDNUMBER = '"+ cp.ShedNo+"' ");
                                     

                                    string updateoccupied = " UPDATE CURINGMASTER SET OCCUPIED ='" + (occupied + 1).ToString() + "' WHERE SHEDNUMBER ='"+ cp.ShedNo+"'";
                                    try
                                    {
                                        datatrans.UpdateStatus(updateoccupied);
                                    }
                                    catch (Exception ex)
                                    {
                                        throw ex;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //System.Console.WriteLine("Exception: {0}", ex.ToString());
                                }
                                /////////////////////////output inventory
                                ///
                                  /////////////////////////Curing Inward
                                datatrans = new DataTransactions(_connectionString);


                                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'CURI' AND ACTIVESEQUENCE = 'T'");
                                string curid = string.Format("{0}{1}", "CURI", (idc + 1).ToString());

                                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='CURI' AND ACTIVESEQUENCE ='T'";
                                try
                                {
                                    datatrans.UpdateStatus(updateCMd);
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                                string shiftid = datatrans.GetDataString("Select SHIFTMASTID from SHIFTMAST where SHIFTNO='"+ cy.Shift+"' ");
                                OracleCommand objCmdsss = new OracleCommand("CURINGINWARDPPROC", objConn);
                                objCmdsss.CommandType = CommandType.StoredProcedure;
                                objCmdsss.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                objCmdsss.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                                objCmdsss.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = curid;
                                objCmdsss.Parameters.Add("DOCDATE", OracleDbType.Date).Value = DateTime.Now;
                                objCmdsss.Parameters.Add("LOCDETAILSID", OracleDbType.NVarchar2).Value = "10044000011739";
                                objCmdsss.Parameters.Add("SHIFT", OracleDbType.NVarchar2).Value = shiftid;
                                objCmdsss.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = "10932000000839";
                                objCmdsss.Parameters.Add("STARTTIME", OracleDbType.NVarchar2).Value =cp.FromTime;
                                objCmdsss.Parameters.Add("ENDTIME", OracleDbType.NVarchar2).Value = cp.ToTime;
                                objCmdsss.Parameters.Add("ENTEREDBY", OracleDbType.NVarchar2).Value = "";
                                objCmdsss.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = "";
                                objCmdsss.Parameters.Add("NPRODBASICID", OracleDbType.NVarchar2).Value = "";
                                objCmdsss.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                objCmdsss.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;

                                try
                                {

                                    objCmdsss.ExecuteNonQuery();
                                    Object crdid = objCmdsss.Parameters["OUTID"].Value;

                                    //if (cy.ID != null)
                                    //{
                                    //    Pid = cy.ID;
                                    //}

                                    //string wcid = datatrans.GetDataString("Select WCBASICID from WCBASIC where ILOCATION='10044000011739' ");
                                    string item = cp.ItemId;
                                    string drum = cp.drumno;
                                    
                                    string batch = string.Format("{0}-{1}", item, drum.ToString());
                                    DataTable dttt = datatrans.GetData("Select SHEDNUMBER,CAPACITY,OCCUPIED from CURINGMASTER where SHEDNUMBER='" + cp.ShedNo + "'");
                                    int curday = datatrans.GetDataId("Select CURINGDAY from ITEMMASTER where ITEMMASTERID='"+ cp.saveitemId+"' ");
                                    DateTime due = DateTime.Now.AddDays(curday);
                                    string dueda = due.ToString("dd-MMM-yyyy");
                                    for (int i = 0; i < dttt.Rows.Count; i++)
                                    {

                                        OracleCommand objCmdInp1 = new OracleCommand("CURINPDETAILPROC", objConn);
                                      
                                        objCmdInp1.CommandType = CommandType.StoredProcedure;
                                        objCmdInp1.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                        objCmdInp1.Parameters.Add("CURINGESBASICID", OracleDbType.NVarchar2).Value = crdid;
                                        objCmdInp1.Parameters.Add("FDATE", OracleDbType.Date).Value = DateTime.Now;
                                        objCmdInp1.Parameters.Add("FTIME", OracleDbType.NVarchar2).Value = cp.FromTime;
                                        objCmdInp1.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.saveitemId;
                                        objCmdInp1.Parameters.Add("DRUMNO", OracleDbType.NVarchar2).Value = cp.drumno;
                                        objCmdInp1.Parameters.Add("BATCHNO", OracleDbType.NVarchar2).Value = batch;
                                        objCmdInp1.Parameters.Add("BATCHQTY", OracleDbType.NVarchar2).Value = qty;
                                        objCmdInp1.Parameters.Add("BINID", OracleDbType.NVarchar2).Value = "";
                                        objCmdInp1.Parameters.Add("CURDAY", OracleDbType.NVarchar2).Value = curday;
                                        objCmdInp1.Parameters.Add("BINMASTERID", OracleDbType.NVarchar2).Value ="";
                                        objCmdInp1.Parameters.Add("CAPACITY", OracleDbType.NVarchar2).Value = dttt.Rows[i]["CAPACITY"].ToString();
                                        objCmdInp1.Parameters.Add("OCCUPIED", OracleDbType.NVarchar2).Value = dttt.Rows[i]["OCCUPIED"].ToString();
                                        objCmdInp1.Parameters.Add("DUEDATE", OracleDbType.Date).Value = due;
                                        objCmdInp1.Parameters.Add("CBINID", OracleDbType.NVarchar2).Value = dttt.Rows[i]["SHEDNUMBER"].ToString();
                                        objCmdInp1.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";

                                        objCmdInp1.ExecuteNonQuery();
                                        string Sql = string.Empty;
                                        Sql = "Update DRUM_STOCK SET  CURINGDUEDATE='"+ dueda + "' where DRUM_STOCK_ID='"+ Pid1 + "' ";
                                        OracleCommand objCmds = new OracleCommand(Sql, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }
                                }
                                catch (Exception ex)
                                {
                                    //System.Console.WriteLine("Exception: {0}", ex.ToString());
                                }
                            }

                        }
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

        public DataTable GetShedNo()
        {
            string SvSql = string.Empty;
            SvSql = "Select SHEDNUMBER,CURINGMASTERID,CAPACITY from CURINGMASTER WHERE STATUS IN ('Active','Occupied') ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable CuringsetDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select SHEDNUMBER,CAPACITY,OCCUPIED +1 as occ from CURINGMASTER WHERE SHEDNUMBER= '" + id +"' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public async Task<IEnumerable<PyroDetail>> Getpyropdf(string id) 
        {
            using (OracleConnection db = new OracleConnection(_connectionString)) 
            {

                return await db.QueryAsync<PyroDetail>(" SELECT to_char(PYROPRODBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PYROPRODBASIC.SHIFT FROM PYROPRODBASIC LEFT OUTER JOIN PYROPRODINPDET ON PYROPRODBASIC.PYROPRODBASICID = PYROPRODINPDET.PYROPRODINPDETID WHERE PYROPRODBASIC.PYROPRODBASICID ='" + id + "' and PYROPRODINPDET.PYROPRODBASICID  ='" + id + "' ", commandType: CommandType.Text);


            }

        }
        //public string StatusChange(string tag, string id)
        //{

        //    try
        //    {
        //        string svSQL = string.Empty;
        //        using (OracleConnection objConnT = new OracleConnection(_connectionString))
        //        {
        //            svSQL = "UPDATE PYROPRODBASIC SET IS_APPROVED ='N' WHERE PYROPRODBASICID='" + id + "'";
        //            OracleCommand objCmds = new OracleCommand(svSQL, objConnT);
        //            objConnT.Open();
        //            objCmds.ExecuteNonQuery();
        //            objConnT.Close();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return "";

        //}

        //public string RemoveChange(string tag, string id)
        //{

        //    try
        //    {
        //        string svSQL = string.Empty;
        //        using (OracleConnection objConnT = new OracleConnection(_connectionString))
        //        {
        //            svSQL = "UPDATE PYROPRODBASIC SET IS_APPROVED ='Y' WHERE PYROPRODBASICID='" + id + "'";
        //            OracleCommand objCmds = new OracleCommand(svSQL, objConnT);
        //            objConnT.Open();
        //            objCmds.ExecuteNonQuery();
        //            objConnT.Close();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return "";

        //}
        public DataTable GetAllPyro(string strStatus)
        {
            string SvSql = string.Empty;
            //if (strStatus == "Y" || strStatus == null)
            //{
            //    SvSql = "Select  DOCID,to_char(PYROPRODBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,SHIFT,EMPMAST.EMPNAME,LOCDETAILS.LOCID,PYROPRODBASICID,PYROPRODBASIC.IS_APPROVED from PYROPRODBASIC LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID=PYROPRODBASIC.SUPERVISOR  LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=PYROPRODBASIC.LOCID  ORDER BY PYROPRODBASICID desc ";

            //}
            //else
            //{
            //    SvSql = "Select  DOCID,to_char(PYROPRODBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,SHIFT,EMPMAST.EMPNAME,LOCDETAILS.LOCID,PYROPRODBASICID,PYROPRODBASIC.IS_APPROVED from PYROPRODBASIC LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID=PYROPRODBASIC.SUPERVISOR  LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=PYROPRODBASIC.LOCID  ORDER BY PYROPRODBASICID desc ";

            //}
            SvSql = "Select N.ENTEREDBY,N.NPRODBASICID,N.SHIFT,W.WCID,N.DOCID,to_char(N.DOCDATE,'dd-MON-yyyy')DOCDATE from NPRODBASIC N,WCBASIC W where W.WCBASICID=N.WCID order by N.NPRODBASICid ASC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string GetBinOPBal(string ProcLotNo,string DocID,string ProcessMastID,string WcMastID)
        {
            string opbal = string.Empty;
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = @"Select BB.ClBbal
From LProdBasic B , LProdBunk BB
Where B.LProdBasicID = BB.LProdBasicID
And DocID = (Select Max(LB.DocID) 
             From LProdBasic LB , ProcLot P
             Where LB.WcID = :WcMastID 
             And LB.ProcessID = :ProcessMastID 
             And LB.ProcLotNo = P.ProcLotID
             And P.ProcLotNo = :ProcLotNo
             And LB.DocID < :DocID)
Union
Select P.OpBbal
From WcBasic W, ProcLot P
Where W.WcBasicid = P.WcID 
And W.WcBasicID = :WcMastID
And P.ProcessID = :ProcessMastID
And P.ProcLotNo = :ProcLotNo
And Upper(W.BunkerYN) = 'YES'
And (Select  Max(LB.DocID) 
             From LProdBasic LB , LProdBunk B , ProcLot P
             Where LB.LProdBasicID = B.LProdBasicID
             And LB.WcID = :WcMastID 
             And LB.ProcessID = :ProcessMastID
             And LB.ProcLotNo = P.ProcLotID
             And P.ProcLotNo = :ProcLotNo 
             And LB.DocID < :DocID) Is Null";
                    cmd.Parameters.Add("WcMastID", WcMastID);
                    cmd.Parameters.Add("ProcessMastID", ProcessMastID);
                    cmd.Parameters.Add("ProcLotNo", ProcLotNo);
                    cmd.Parameters.Add("DocID", DocID);
                    cmd.BindByName = true;
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        opbal = rdr["ClBbal"].ToString();
                    }

                }
            }
                    return opbal;
        }
        public string GetMLOPBal(string ProcLotNo, string DocID, string ProcessMastID, string WcMastID)
        {
            string opbal = string.Empty;
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = @"Select BB.MLClBal
From LProdBasic B , LProdBunk BB
Where B.LProdBasicID = BB.LProdBasicID
And DocID = (Select Max(LB.DocID) 
             From LProdBasic LB , ProcLot P
             Where LB.WcID = :WcMastID 
             And LB.ProcessID = :ProcessMastID 
             And LB.ProcLotNo = P.ProcLotID
             And P.ProcLotNo = :ProcLotNo
             And LB.DocID < :DocID)
Union
Select P.OpMLBal
From WcBasic W, ProcLot P
Where W.WcBasicid = P.WcID 
And W.WcBasicID = :WcMastID
And P.ProcLotNo = :ProcLotNo
And Upper(W.MLYN) = 'YES'
And (Select  Max(LB.DocID) 
             From LProdBasic LB , LProdBunk B , ProcLot P
             Where LB.LProdBasicID = B.LProdBasicID
             And LB.WcID = :WcMastID 
             And LB.ProcessID = :ProcessMastID
             And LB.ProcLotNo = P.ProcLotID
             And P.ProcLotNo = :ProcLotNo
             And LB.DocID < :DocID) Is Null";
                    cmd.Parameters.Add("WcMastID", WcMastID);
                    cmd.Parameters.Add("ProcessMastID", ProcessMastID);
                    cmd.Parameters.Add("ProcLotNo", ProcLotNo);
                    cmd.Parameters.Add("DocID", DocID);
                    cmd.BindByName = true;
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        opbal = rdr["MLClBal"].ToString();
                    }

                }
            }
            return opbal;
        }

        public DataTable GetReason()
        {
            string SvSql = string.Empty;
            SvSql = "select REASON,REASONBASICID  from REASONDETAIL ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
