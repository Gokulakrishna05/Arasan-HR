﻿using Arasan.Interface;
using Arasan.Models;
using Dapper;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
 

namespace Arasan.Services 
{
	public class APProductionentryService : IAPProductionEntry
	{

		private readonly string _connectionString;
		DataTransactions datatrans;
		public APProductionentryService(IConfiguration _configuratio)
		{
			_connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

		public IEnumerable<APProductionentry> GetAllAPProductionentry()
		{
			List<APProductionentry> cmpList = new List<APProductionentry>();
			using (OracleConnection con = new OracleConnection(_connectionString))
			{

				using (OracleCommand cmd = con.CreateCommand())
				{
					con.Open();
					cmd.CommandText = " Select APPRODUCTIONBASIC.DOCID,to_char(APPRODUCTIONBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,WCBASIC.WCID,EMPMAST.EMPNAME,APPRODUCTIONBASIC.SCHQTY,APPRODUCTIONBASIC.PRODQTY,APPRODUCTIONBASICID,BATCH,SHIFT from APPRODUCTIONBASIC LEFT OUTER JOIN WCBASIC ON WCBASICID=APPRODUCTIONBASIC.WCID  LEFT OUTER JOIN EMPMAST ON EMPMASTID=APPRODUCTIONBASIC.ASSIGNENG order by APPRODUCTIONBASICID desc   ";
					OracleDataReader rdr = cmd.ExecuteReader();
					while (rdr.Read())
					{
						APProductionentry cmp = new APProductionentry
						{

							ID = rdr["APPRODUCTIONBASICID"].ToString(),

							DocId = rdr["DOCID"].ToString(),
							Docdate = rdr["DOCDATE"].ToString(),
							Location = rdr["WCID"].ToString(),
							Eng = rdr["EMPNAME"].ToString(),

							Shift = rdr["SHIFT"].ToString(),


						};
						cmpList.Add(cmp);
					}
				}
			}
			return cmpList;
		}

		public DataTable GetAPWorkCenter()
		{
			string SvSql = string.Empty;
			SvSql = "select * from WCBASIC where WCID like 'AP %' AND ACTIVE='Yes'";
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
		public DataTable GetItem( )
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
			SvSql = "select BINBASIC.BINID,ITEMMASTER.BINNO as bin ,ITEMMASTERID from ITEMMASTER left outer join BINBASIC ON BINBASICID= ITEMMASTER.BINNO where ITEMMASTERID='" + id +"' ";

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
		public string APProductionEntryCRUD(APProductionentry cy)
		{
			string msg = "";
            datatrans = new DataTransactions(_connectionString);


            int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'AP-Pro' AND ACTIVESEQUENCE = 'T'");
            string docid = string.Format("{0} {1}", "AP-Pro", (idc + 1).ToString());

            string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='AP-Pro' AND ACTIVESEQUENCE ='T'";
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
                    svSQL = "Update APPRODUCTIONBASIC SET IS_CURRENT='No'";
                    OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                    objCmdd.ExecuteNonQuery();

                    OracleCommand objCmd = new OracleCommand("APPRODUCTIONPROC", objConn);
					
					objCmd.CommandType = CommandType.StoredProcedure;
					 
						StatementType = "Insert";
						objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
				 
					objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
					objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.Docdate;
					objCmd.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = cy.Location;
				 
					objCmd.Parameters.Add("ASSIGNENG", OracleDbType.NVarchar2).Value = cy.Eng;
					objCmd.Parameters.Add("SCHQTY", OracleDbType.NVarchar2).Value = cy.SchQty;
					objCmd.Parameters.Add("PRODQTY", OracleDbType.NVarchar2).Value = cy.ProdQty;
					objCmd.Parameters.Add("BATCH", OracleDbType.NVarchar2).Value = cy.BatchNo;
					objCmd.Parameters.Add("SHIFT", OracleDbType.NVarchar2).Value = cy.Shift;
					objCmd.Parameters.Add("BATCHYN", OracleDbType.NVarchar2).Value = cy.batchcomplete;
					objCmd.Parameters.Add("IS_CURRENT", OracleDbType.NVarchar2).Value = "Yes";
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
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
						cy.APID= Pid;
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

        public string APProductionEntryDetCRUD(APProductionentryDet cy)
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
						foreach (ProInput cp in cy.inplst)
						{
							if (cp.Isvalid == "Y" && cp.ItemId != "0")
							{
								svSQL = "Insert into APPRODINPDET (APPRODUCTIONBASICID,ITEMID,BINID,BATCHNO,STOCK,QTY) VALUES ('" + cp.APID + "','" + cp.ItemId + "','" + cp.Bin + "','" + cp.batchno + "','" + cp.StockAvailable + "','" + cp.IssueQty + "')";
								OracleCommand objCmds = new OracleCommand(svSQL, objConn);
								objCmds.ExecuteNonQuery();


							}

						}
					}
                    if (cy.Binconslst != null)
                    {
                        foreach (APProInCons cp in cy.Binconslst)
                        {
                            if (cp.Isvalid == "Y" && cp.ItemId != "0")
                            {
                                svSQL = "Insert into APPRODCONSDET (APPRODUCTIONBASICID,ITEMID,UNIT,BINID,STOCK,QTY,CONSQTY) VALUES ('" + cp.APID + "','" + cp.ItemId + "','" + cp.unitid + "','" + cp.consBin + "','" + cp.ConsStock + "','" + cp.Qty + "','" + cp.consQty + "')";
                                OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                objCmds.ExecuteNonQuery();


                            }

                        }
                    }
                    if (cy.EmplLst != null)
                    {
                        foreach (EmpDetails cp in cy.EmplLst)
                        {
                            if (cp.Isvalid == "Y" && cp.Employee != "0")
                            {
                                svSQL = "Insert into APPRODEMPDET (APPRODUCTIONBASICID,EMPID,EMPCODE,DEPARTMENT,STARTDATE,ENDDATE,STARTTIME,ENDTIME,OTHOUR,ETOTHER,NHOUR,NATUREOFWORK) VALUES ('" + cp.APID + "','" + cp.Employee + "','" + cp.EmpCode + "','" + cp.Depart + "','" + cp.StartDate + "','" + cp.EndDate + "','" + cp.StartTime + "','" + cp.EndTime + "','" + cp.OTHrs + "','" + cp.ETOther + "','" + cp.Normal + "','" + cp.NOW + "')";
                                OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                objCmds.ExecuteNonQuery();


                            }

                        }
                    }

                    if (cy.BreakLst != null)
                    {
                        foreach (BreakDet cp in cy.BreakLst)
                        {
                            if (cp.Isvalid == "Y" && cp.MachineId != "0")
                            {
                                svSQL = "Insert into APPRODBREAKDET (APPRODUCTIONBASICID,MACHCODE,DESCRIPTION,DTYPE,MTYPE,FROMTIME,TOTIME,PB,ALLOTTEDTO,REASON) VALUES ('" + cp.APID + "','" + cp.MachineId + "','" + cp.MachineDes + "','" + cp.DType + "','" + cp.MType + "','" + cp.StartTime + "','" + cp.EndTime + "','" + cp.PB + "','" + cp.Alloted + "','" + cp.Reason + "')";
                                OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                objCmds.ExecuteNonQuery();


                            }

                        }
                    }
                    if (cy.outlst != null)
                    {
                        foreach (ProOutput cp in cy.outlst)
                        {
                            if (cp.Isvalid == "Y" && cp.ItemId != "0")
                            {
                                svSQL = "Insert into APPRODOUTDET (APPRODUCTIONBASICID,ITEMID,BINID,DRUMNO,OUTQTY,TIME) VALUES ('" + cp.APID + "','" + cp.ItemId + "','" + cp.Bin + "','" + cp.drumno + "','" + cp.OutputQty + "','" + cp.Time + "')";
                                OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                objCmds.ExecuteNonQuery();


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

        public string APProEntryCRUD(APProductionentryDet cy)
        {
            string msg = "";
          
          
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                if(cy.change!="Complete")
                {
                    using (OracleConnection objConn = new OracleConnection(_connectionString))
                    {
                        objConn.Open();
                        DataTable ap = datatrans.GetData("select APPRODUCTIONBASICID,DOCID,WCID,to_char(APPRODUCTIONBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,SHIFT,ASSIGNENG,SCHQTY,PRODQTY,BATCH,BATCHYN,BRANCHID from APPRODUCTIONBASIC WHERE IS_CURRENT='Yes'");
                        svSQL = "Update APPRODUCTIONBASIC SET IS_CURRENT='No'";
                        OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                        objCmdd.ExecuteNonQuery();

                        OracleCommand objCmd = new OracleCommand("APPRODUCTIONPROC", objConn);

                        objCmd.CommandType = CommandType.StoredProcedure;

                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;

                        objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = ap.Rows[0]["DOCID"].ToString();
                        objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = ap.Rows[0]["DOCDATE"].ToString();
                        objCmd.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = ap.Rows[0]["WCID"].ToString();

                        objCmd.Parameters.Add("ASSIGNENG", OracleDbType.NVarchar2).Value = ap.Rows[0]["ASSIGNENG"].ToString(); 
                        objCmd.Parameters.Add("SCHQTY", OracleDbType.NVarchar2).Value = ap.Rows[0]["SCHQTY"].ToString();
                        objCmd.Parameters.Add("PRODQTY", OracleDbType.NVarchar2).Value = ap.Rows[0]["PRODQTY"].ToString();
                        objCmd.Parameters.Add("BATCH", OracleDbType.NVarchar2).Value = ap.Rows[0]["BATCH"].ToString();
                        objCmd.Parameters.Add("SHIFT", OracleDbType.NVarchar2).Value = cy.change;
                        objCmd.Parameters.Add("BATCHYN", OracleDbType.NVarchar2).Value = ap.Rows[0]["BATCHYN"].ToString();
                        objCmd.Parameters.Add("IS_CURRENT", OracleDbType.NVarchar2).Value = "Yes";
                        objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = ap.Rows[0]["BRANCHID"].ToString();
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

        public DataTable GetAPProd(string id)
		{
			string SvSql = string.Empty;
			SvSql = "select APPRODUCTIONBASICID,APPRODUCTIONBASIC.DOCID,to_char(APPRODUCTIONBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,WCBASIC.WCID,EMPMAST.EMPNAME,SCHQTY,PRODQTY,BATCH,SHIFT,BATCHYN from APPRODUCTIONBASIC left outer join WCBASIC ON WCBASICID= APPRODUCTIONBASIC.WCID left outer join EMPMAST ON EMPMASTID= APPRODUCTIONBASIC.ASSIGNENG  where APPRODUCTIONBASICID='" + id + "' ";

			DataTable dtt = new DataTable();
			OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
			OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
			adapter.Fill(dtt);
			return dtt;
		}
        public DataTable GetInput(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select APPRODUCTIONBASICID,ITEMID,BINBASIC.BINID,BATCHNO,STOCK,QTY from APPRODINPDET left outer join BINBASIC ON BINBASICID= APPRODINPDET.BINID  where APPRODUCTIONBASICID='" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetCons(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select APPRODUCTIONBASICID,ITEMID,UNITMAST.UNITID,BINBASIC.BINID,STOCK,QTY,CONSQTY from APPRODCONSDET left outer join BINBASIC ON BINBASICID= APPRODCONSDET.BINID left outer join UNITMAST ON UNITMASTID= APPRODCONSDET.UNIT  where APPRODUCTIONBASICID='" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEmpdet(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select APPRODUCTIONBASICID,EMPID,EMPCODE,DEPARTMENT,to_char(APPRODEMPDET.STARTDATE,'dd-MON-yyyy')STARTDATE,to_char(APPRODEMPDET.ENDDATE,'dd-MON-yyyy')ENDDATE,STARTTIME,ENDTIME,OTHOUR,ETOTHER,NHOUR,NATUREOFWORK from APPRODEMPDET where APPRODUCTIONBASICID='" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetBreak(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select APPRODUCTIONBASICID,MACHCODE,DESCRIPTION,DTYPE,MTYPE,FROMTIME,TOTIME,PB,ALLOTTEDTO,REASON from APPRODBREAKDET  where APPRODUCTIONBASICID='" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetOutput(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select APPRODUCTIONBASICID,ITEMID,BINBASIC.BINID,OUTQTY,DRUMNO from APPRODOUTDET left outer join BINBASIC ON BINBASICID= APPRODOUTDET.BINID  where APPRODUCTIONBASICID='" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetResult(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select APPRODUCTIONBASICID,TESTRESULT,MOVETOQC from APPRODOUTDET  where APPRODUCTIONBASICID='" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable SaveOutDetails(string id, string item, string drum, string time, string qty)
        {
            string SvSql = string.Empty;
            SvSql = "Insert into APPRODOUTDET (APPRODUCTIONBASICID,ITEMID,DRUMNO,TIME,OUTQTY) VALUES ('" + id + "','" + item + "','" + drum + "','" + time + "','" + qty + "')";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }



        public async Task<IEnumerable<APItemDetail>> GetAPItem(string aid, string bid, string cid)
        {
            using OracleConnection db = new OracleConnection(_connectionString);
            {
                 
                    return await db.QueryAsync<APItemDetail>("select * from (SELECT APPRODUCTIONBASIC.APPRODUCTIONBASICID, WCBASIC.WCID, APPRODUCTIONBASIC.DOCID,to_char(APPRODUCTIONBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE, APPRODUCTIONBASIC.SHIFT, ITEMMASTER.ITEMID, APPRODINPDET.APPRODUCTIONBASICID AS EXPR1, APPRODINPDET.APPRODINPDETID, APPRODINPDET.QTY,APPRODINPDET.CHARGINGTIME, APPRODINPDET.UNIT, APPRODOUTDET.APPRODOUTDETID, APPRODOUTDET.APPRODUCTIONBASICID AS EXPR2,DRUMMAST.DRUMNO, APPRODOUTDET.OUTQTY, APPRODOUTDET.FROMTIME, APPRODOUTDET.TESTRESULT, APPRODOUTDET.TOTIME ,APPRODBREAKDET.FROMTIME AS EXPR8, APPRODBREAKDET.TOTIME AS EXPR9 ,EMPMAST.EMPNAME,APPRODCONSDET.APPRODUCTIONBASICID AS EXPR4,APPRODCONSDET.APPRODCONSDETID,  APPRODCONSDET.ITEMID AS EXPR5,  APPRODCONSDET.CONSQTY,  APPRODCONSDET.QTY AS EXPR6 from APPRODUCTIONBASIC left outer join WCBASIC on WCBASIC.WCBASICID=APPRODUCTIONBASIC.WCID  INNER JOIN APPRODINPDET ON APPRODUCTIONBASIC.APPRODUCTIONBASICID = APPRODINPDET.APPRODUCTIONBASICID left outer join ITEMMASTER ON ITEMMASTER.ITEMMASTERID=APPRODINPDET.ITEMID INNER JOIN APPRODOUTDET ON APPRODUCTIONBASIC.APPRODUCTIONBASICID = APPRODOUTDET.APPRODUCTIONBASICID left outer join DRUMMAST on DRUMMAST.DRUMMASTID=APPRODOUTDET.DRUMNO INNER JOIN APPRODCONSDET ON APPRODUCTIONBASIC.APPRODUCTIONBASICID = APPRODCONSDET.APPRODUCTIONBASICID INNER JOIN  APPRODBREAKDET ON APPRODUCTIONBASIC.APPRODUCTIONBASICID = APPRODBREAKDET.APPRODUCTIONBASICID INNER JOIN  APPRODEMPDET ON APPRODUCTIONBASIC.APPRODUCTIONBASICID = APPRODEMPDET.APPRODUCTIONBASICID left outer join EMPMAST on EMPMAST.EMPMASTID=APPRODEMPDET.EMPID where APPRODUCTIONBASIC.APPRODUCTIONBASICID='" + aid + "' and APPRODINPDET.APPRODUCTIONBASICID='"+ aid + "' and APPRODOUTDET.APPRODUCTIONBASICID='"+ aid + "' and APPRODEMPDET.APPRODUCTIONBASICID='"+ aid + "' and APPRODBREAKDET.APPRODUCTIONBASICID='"+ aid + "' and APPRODCONSDET.APPRODUCTIONBASICID='"+ aid + "')  cross join (SELECT APPRODUCTIONBASIC.APPRODUCTIONBASICID, WCBASIC.WCID, APPRODUCTIONBASIC.DOCID,to_char(APPRODUCTIONBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE, APPRODUCTIONBASIC.SHIFT AS BSHIFT, ITEMMASTER.ITEMID AS BITEMID, APPRODINPDET.APPRODUCTIONBASICID AS EXPR1, APPRODINPDET.APPRODINPDETID, APPRODINPDET.QTY AS BQTY,APPRODINPDET.CHARGINGTIME AS BCHARGINGTIME, APPRODINPDET.UNIT AS BUNIT, APPRODOUTDET.APPRODOUTDETID, APPRODOUTDET.APPRODUCTIONBASICID AS EXPR2,DRUMMAST.DRUMNO AS BDRUMNO, APPRODOUTDET.OUTQTY AS BOUTQTY, APPRODOUTDET.FROMTIME AS BFROMTIME, APPRODOUTDET.TESTRESULT AS BTESTRESULT , APPRODOUTDET.TOTIME AS BTOTIME ,APPRODBREAKDET.FROMTIME AS BFROM, APPRODBREAKDET.TOTIME AS BTO ,EMPMAST.EMPNAME AS BEMP, APPRODCONSDET.APPRODCONSDETID,  APPRODCONSDET.ITEMID AS EXPR5,  APPRODCONSDET.CONSQTY as BCONSQTY,  APPRODCONSDET.QTY AS BSQTY   FROM APPRODUCTIONBASIC  left outer join WCBASIC on WCBASIC.WCBASICID=APPRODUCTIONBASIC.WCID  INNER JOIN APPRODINPDET ON APPRODUCTIONBASIC.APPRODUCTIONBASICID = APPRODINPDET.APPRODUCTIONBASICID left outer join ITEMMASTER ON ITEMMASTER.ITEMMASTERID=APPRODINPDET.ITEMID INNER JOIN APPRODOUTDET ON APPRODUCTIONBASIC.APPRODUCTIONBASICID = APPRODOUTDET.APPRODUCTIONBASICID left outer join DRUMMAST on DRUMMAST.DRUMMASTID=APPRODOUTDET.DRUMNO INNER JOIN APPRODCONSDET ON APPRODUCTIONBASIC.APPRODUCTIONBASICID = APPRODCONSDET.APPRODUCTIONBASICID INNER JOIN  APPRODBREAKDET ON APPRODUCTIONBASIC.APPRODUCTIONBASICID = APPRODBREAKDET.APPRODUCTIONBASICID INNER JOIN  APPRODEMPDET ON APPRODUCTIONBASIC.APPRODUCTIONBASICID = APPRODEMPDET.APPRODUCTIONBASICID left outer join EMPMAST on EMPMAST.EMPMASTID=APPRODEMPDET.EMPID where APPRODUCTIONBASIC.APPRODUCTIONBASICID='" + bid +"' and APPRODINPDET.APPRODUCTIONBASICID='" + bid +"' and APPRODOUTDET.APPRODUCTIONBASICID='" + bid +"' and APPRODEMPDET.APPRODUCTIONBASICID='" + bid +"' and APPRODBREAKDET.APPRODUCTIONBASICID='" + bid + "' and APPRODCONSDET.APPRODUCTIONBASICID='"+ bid + "') cross join (SELECT APPRODUCTIONBASIC.APPRODUCTIONBASICID, WCBASIC.WCID, APPRODUCTIONBASIC.DOCID,to_char(APPRODUCTIONBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE, APPRODUCTIONBASIC.SHIFT AS CSHIFT, ITEMMASTER.ITEMID AS CITEMID, APPRODINPDET.APPRODUCTIONBASICID AS EXPR1, APPRODINPDET.APPRODINPDETID, APPRODINPDET.QTY AS CQTY,APPRODINPDET.CHARGINGTIME AS CCHARGINGTIME, APPRODINPDET.UNIT AS CUNIT, APPRODOUTDET.APPRODOUTDETID, APPRODOUTDET.APPRODUCTIONBASICID AS EXPR2,DRUMMAST.DRUMNO AS CDRUMNO, APPRODOUTDET.OUTQTY AS COUTQTY, APPRODOUTDET.FROMTIME AS CFROMTIME, APPRODOUTDET.TESTRESULT AS CTESTRESULT, APPRODOUTDET.TOTIME AS CTOTIME,APPRODBREAKDET.FROMTIME AS CFROM, APPRODBREAKDET.TOTIME AS CTO ,EMPMAST.EMPNAME AS CEMP,APPRODCONSDET.APPRODCONSDETID,  APPRODCONSDET.ITEMID AS EXPR5,  APPRODCONSDET.CONSQTY as CCONSQTY,  APPRODCONSDET.QTY AS CSQTY FROM APPRODUCTIONBASIC left outer join WCBASIC on WCBASIC.WCBASICID=APPRODUCTIONBASIC.WCID  INNER JOIN APPRODINPDET ON APPRODUCTIONBASIC.APPRODUCTIONBASICID = APPRODINPDET.APPRODUCTIONBASICID left outer join ITEMMASTER ON ITEMMASTER.ITEMMASTERID=APPRODINPDET.ITEMID INNER JOIN APPRODOUTDET ON APPRODUCTIONBASIC.APPRODUCTIONBASICID = APPRODOUTDET.APPRODUCTIONBASICID left outer join DRUMMAST on DRUMMAST.DRUMMASTID=APPRODOUTDET.DRUMNO INNER JOIN APPRODCONSDET ON APPRODUCTIONBASIC.APPRODUCTIONBASICID = APPRODCONSDET.APPRODUCTIONBASICID INNER JOIN  APPRODBREAKDET ON APPRODUCTIONBASIC.APPRODUCTIONBASICID = APPRODBREAKDET.APPRODUCTIONBASICID INNER JOIN  APPRODEMPDET ON APPRODUCTIONBASIC.APPRODUCTIONBASICID = APPRODEMPDET.APPRODUCTIONBASICID left outer join EMPMAST on EMPMAST.EMPMASTID=APPRODEMPDET.EMPID where APPRODUCTIONBASIC.APPRODUCTIONBASICID='" + cid + "' and APPRODINPDET.APPRODUCTIONBASICID='" + cid + "' and APPRODOUTDET.APPRODUCTIONBASICID='" + cid + "' and APPRODEMPDET.APPRODUCTIONBASICID='" + cid + "' and APPRODBREAKDET.APPRODUCTIONBASICID='" + cid + "' and APPRODCONSDET.APPRODUCTIONBASICID='"+ cid +"')", commandType: CommandType.Text);


                
            }
             
        }
       
      
    }
}