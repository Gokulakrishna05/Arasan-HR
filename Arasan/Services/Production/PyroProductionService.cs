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
            SvSql = "select ITEMMASTER.ITEMID,DRUM_STOCK.ITEMID as item from DRUM_STOCK left outer join ITEMMASTER on ITEMMASTER.ITEMMASTERID=DRUM_STOCK.ITEMID  where LOCID='10035000000038' GROUP BY ITEMMASTER.ITEMID,DRUM_STOCK.ITEMID";

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
            SvSql = "select DRUMNO,DRUMMASTID from DRUMMAST WHERE IS_EMPTY='Y'";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDrum(string item)
        {
            string SvSql = string.Empty;
            SvSql = "select DRUM_NO,DRUM_ID from DRUM_STOCK where ITEMID='"+item+ "' AND LOCID='10035000000038' ";

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
        public DataTable GetItemCon()
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMMASTER.ITEMID,INVENTORY_ITEM.ITEM_ID as item from INVENTORY_ITEM left outer join ITEMMASTER on ITEMMASTER.ITEMMASTERID=INVENTORY_ITEM.ITEM_ID where ITEMMASTER.igroup='Consumables'  AND LOCATION_ID='10035000000038' GROUP BY ITEMMASTER.ITEMID,INVENTORY_ITEM.ITEM_ID ";

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
            SvSql = "select PYROPRODBASICID,ITEMID,BINID,BATCH BATCHNO,STOCK,QTY,TIME from PYROPRODINPDET   where PYROPRODBASICID='" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetCons(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PYROPRODBASICID,ITEMID,UNITMAST.UNITID,BINID,STOCK,QTY,CONSQTY from PYROPRODCONSDET  left outer join UNITMAST ON UNITMASTID= PYROPRODCONSDET.UNITID  where PYROPRODBASICID='" + id + "' ";

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
            SvSql = "select PYROPRODOUTDETID,PYROPRODBASICID,PYROPRODOUTDET.ITEMID,BINID,OUTQTY,DRUM DRUMNO,STARTTIME FROMTIME,ENDTIME TOTIME,ITEMMASTER.ITEMID as ITEMNAME from PYROPRODOUTDET  LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PYROPRODOUTDET.ITEMID  where PYROPRODBASICID='" + id + "' ";

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
        public DataTable GetProduction(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select W.WCID WORKID,N.BRANCH,N.PROCESSID,N.WCID,SHIFT,to_char(N.DOCDATE,'dd-MON-yyyy')DOCDATE,N.DOCID,N.ENTEREDBY,N.SCHQTY,N.PRODQTY,N.PROCLOTNO from NPRODBASIC N,WCBASIC W where W.WCBASICID=N.WCID AND N.NPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;

        }

        public DataTable GetInputDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PYROPRODBASICID,ITEMMASTER.ITEMID,PYROPRODINPDET.ITEMID as item,DRUMMAST.DRUMNO,PYROPRODINPDET.DRUMNO as drum,BINID,BATCH,STOCK,QTY,TIME,PYROPRODINPDETID from PYROPRODINPDET   left outer join ITEMMASTER ON ITEMMASTER.ITEMMASTERID= PYROPRODINPDET.ITEMID left outer join DRUMMAST ON DRUMMAST.DRUMMASTID= PYROPRODINPDET.DRUMNO  where PYROPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetConsDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PYROPRODBASICID,ITEMMASTER.ITEMID,PYROPRODCONSDET.UNITID,PYROPRODCONSDET.ITEMID as item,PYROPRODCONSDET.BINID,STOCK,QTY,UNITMAST.UNITID,CONSQTY,PYROPRODCONSDETID from PYROPRODCONSDET left outer join ITEMMASTER ON ITEMMASTER.ITEMMASTERID= PYROPRODCONSDET.ITEMID LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID where PYROPRODBASICID='" + id + "' ";
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
            SvSql = "select PYROPRODBASICID,ITEMMASTER.ITEMID,BINID,OUTQTY,PYROPRODOUTDET.ITEMID as item,DRUMMAST.DRUMNO,PYROPRODOUTDET.DRUM,STARTTIME,ENDTIME,ITEMMASTER.ITEMID as ITEMNAME,PYROPRODOUTDET.PYROPRODOUTDETID from PYROPRODOUTDET  LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PYROPRODOUTDET.ITEMID LEFT OUTER JOIN DRUMMAST ON DRUMMAST.DRUMMASTID=PYROPRODOUTDET.DRUM  where PYROPRODBASICID='" + id + "' ";
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

        public DataTable SaveInputDetails(string id, string item, string bin, string time, string qty, string stock, string batch,string drum,int r)
        {
            string SvSql = string.Empty;
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
              
                DataTable drlot = datatrans.GetData("SELECT DRUMYN,LOTYN,ITEMACC,QCT FROM ITEMMASTER WHERE ITEMMASTERID='" + item + "'");
                DataTable wopro = datatrans.GetData("SELECT WCBASIC.WCID,PROCESSMAST.PROCESSID,NPRODBASIC.DOCID,to_char(NPRODBASIC.DOCDATE,'dd-MM-yy')DOCDATE,ILOCDETAILSID FROM NPRODBASIC LEFT OUTER JOIN WCBASIC ON WCBASICID=NPRODBASIC.WCID LEFT OUTER JOIN PROCESSMAST ON PROCESSMAST.PROCESSMASTID=NPRODBASIC.PROCESSID WHERE NPRODBASICID ='" + id + "'");
                string qc = drlot.Rows[0]["QCT"].ToString();
                string docdate = DateTime.Now.ToString("dd-MMM-yyyy");
                string work = wopro.Rows[0]["WCID"].ToString();
                string process = wopro.Rows[0]["PROCESSID"].ToString();
                 
                    narr = "Consumption in " + work + " - " + process +" - "+ drum;
                
                SvSql = "Insert into NPRODINPDET (NPRODBASICID,IITEMID,IBINID,CHARGINGTIME,IQTY,IBATCHQTY,IBATCHNO,IDRUMNO,ICDRUMNO,IDRUMSLOCID,IDRUMSLOCATION,DRUMYN,LOTYN,IITEMACC,NPRODINPDETROW,INARR,IS_INSERT,IITEMMASTERID,ICLSTKBUP,ICSOCTKBUP) VALUES ('" + id + "','" + item + "','0','" + time + "','" + qty + "','" + stock + "','" + batch + "','" + drumid + "','" + drum + "','" + drumloc + "','" + drumloc + "','" + drlot.Rows[0]["DRUMYN"].ToString() + "','" + drlot.Rows[0]["LOTYN"].ToString() + "','" + drlot.Rows[0]["ITEMACC"].ToString() + "','" + r + "','" + narr + "','Y','"+ item + "','0','0') RETURNING NPRODINPDETID INTO :LASTCID";
                OracleCommand objCmds = new OracleCommand(SvSql, objConnT);

                objCmds.Parameters.Add("LASTCID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                objCmds.ExecuteNonQuery();
                string detid = objCmds.Parameters["LASTCID"].Value.ToString();
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
                DataTable drlot = datatrans.GetData("SELECT DRUMYN,LOTYN,ITEMACC,QCT FROM ITEMMASTER WHERE ITEMMASTERID='" + item + "'");
                DataTable wopro = datatrans.GetData("SELECT WCBASIC.WCID,PROCESSMAST.PROCESSID,NPRODBASIC.DOCID,to_char(NPRODBASIC.DOCDATE,'dd-MM-yy')DOCDATE,ILOCDETAILSID FROM NPRODBASIC LEFT OUTER JOIN WCBASIC ON WCBASICID=NPRODBASIC.WCID LEFT OUTER JOIN PROCESSMAST ON PROCESSMAST.PROCESSMASTID=NPRODBASIC.PROCESSID WHERE NPRODBASICID='" + id + "'");
                string qc = drlot.Rows[0]["QCT"].ToString();
                string work = wopro.Rows[0]["WCID"].ToString();
                string process = wopro.Rows[0]["PROCESSID"].ToString();
                string docdate = DateTime.Now.ToString("dd-MMM-yyyy");
                string narr = "Consumption in " + work + " - " + process;


                SvSql = "Insert into NPRODCONSDET (NPRODBASICID,CITEMID,CBINID,CUNIT,CLOTYN,CNARR,CONSQTY,CSUBQTY,VALIDROW3,NPRODCONSDETROW,CITEMACC,IS_INSERT) VALUES ('" + id + "','" + item + "','0','" + unit + "','" + drlot.Rows[0]["LOTYN"].ToString() + "','" + narr + "','" + qty + "','" + usedqty + "','T','" + l + "','" + drlot.Rows[0]["ITEMACC"].ToString() + "','Y') RETURNING NPRODCONSDETID INTO :LASTCID";
                OracleCommand objCmds = new OracleCommand(SvSql, objConnT);

                objCmds.Parameters.Add("LASTCID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                objCmds.ExecuteNonQuery();
                // SvSql = "Insert into PYROPRODCONSDET (PYROPRODBASICID,ITEMID,BINID,UNITID,QTY,CONSQTY,STOCK) VALUES ('" + id + "','" + item + "','" + bin + "','" + unit + "','" + usedqty + "','" + qty + "','" + stock + "')";
            }
            return dtt;
        }
        public DataTable SaveOutputDetails(string id, string item, string bin, string stime, string ttime, string qty, string drum,string status, string stock,string excess)
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
                DataTable wopro = datatrans.GetData("SELECT WCBASIC.WCID,WCBASIC.ILOCATION,PROCESSMAST.PROCESSID,NPRODBASIC.DOCID,DRUMILOCDETAILSID,PSBASIC.DOCID as psno,SHIFT,to_char(NPRODBASIC.DOCDATE,'dd-MM-yy')DOCDATE,ILOCDETAILSID FROM NPRODBASIC LEFT OUTER JOIN WCBASIC ON WCBASICID=NPRODBASIC.WCID LEFT OUTER JOIN PROCESSMAST ON PROCESSMAST.PROCESSMASTID=NPRODBASIC.PROCESSID LEFT OUTER JOIN PSBASIC ON PSBASIC.PSBASICID=NPRODBASIC.PSCHNO WHERE NPRODBASICID='" + id + "'");
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
                SvSql = "Insert into NPRODOUTDET (NPRODBASICID,OITEMID,ODRUMNO,STIME,OQTY,ETIME,OXQTY,STATUS,OSTOCK,TOLOCATION,TOLOCDETAILSID,OCDRUMNO,LWCID,LPROCESS,LSHIFT,LSCH,OBINID,OBATCHNO,NBATCHNO,ONARR,INSFLAGCTRL,ODRUMYN,OLOTYN,DSDT,DSTIME,DEDT,DETIME,OBQTY,DRUMLOADID,OQCYN,ODRUMSLOCID) VALUES ('" + id + "','" + item + "','" + drum + "','" + stime + "','" + qty + "','" + ttime + "','" + excess + "','" + status + "','" + stock + "','" + loc + "','" + loc + "','" + drumno + "','" + work + "','" + process + "','" + shift + "','" + psno + "','0','" + obatch + "','" + nbatch + "','" + narr + "','" + ins + "','"+ drumyn +"','"+lot+"','"+ docdate+"','"+ stime + "','"+ docdate +"','"+ ttime +"','"+excess +"','0','"+ qcyn +"','"+ drumloc +"')";

            }
           // SvSql = "Insert into PYROPRODOUTDET (PYROPRODBASICID,ITEMID,BINID,STARTTIME,ENDTIME,OUTQTY,DRUM) VALUES ('" + id + "','" + item + "','" + bin + "','" + stime + "','" + ttime + "','" + qty + "','" + drum + "')";
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
                SvSql = "Delete PYROPRODBREAKDET WHERE PYROPRODBASICID='" + id + "'";
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
            SvSql = "Select SHEDNUMBER,CURINGMASTERID,CAPACITY from CURINGMASTER WHERE STATUS='Active' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable CuringsetDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select SHEDNUMBER,CAPACITY from CURINGMASTER WHERE SHEDNUMBER= '"+ id +"' ";
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


    }
}
