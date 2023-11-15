using Arasan.Interface;
using Arasan.Models;
using Dapper;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;

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
					cmd.CommandText = " Select APPRODUCTIONBASIC.DOCID,to_char(APPRODUCTIONBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,WCBASIC.WCID,EMPMAST.EMPNAME,APPRODUCTIONBASIC.SCHQTY,APPRODUCTIONBASIC.PRODQTY,APPRODUCTIONBASICID,BATCH,SHIFT,IS_APPROVE,IS_CURRENT from APPRODUCTIONBASIC LEFT OUTER JOIN WCBASIC ON WCBASICID=APPRODUCTIONBASIC.WCID  LEFT OUTER JOIN EMPMAST ON EMPMASTID=APPRODUCTIONBASIC.ASSIGNENG order by APPRODUCTIONBASICID desc   ";
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
                            isapprove= rdr["IS_APPROVE"].ToString(),
                            iscurrent= rdr["IS_CURRENT"].ToString(),
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
            SvSql = "select DRUMNO,DRUMMASTID from DRUMMAST WHERE IS_EMPTY='Y' ";

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
            if (cy.ID == null)
            {
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
            }
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

        public string ApproveAPProEntryCRUD(APProductionentryDet cy)
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
                            if (cp.Isvalid == "Y" && cp.Item != "0")
                            {
                                string locid = datatrans.GetDataString("Select ILOCATION from WCBASIC where WCBASICID='" + cy.LOCID + "' ");

                                ///////////////////////////// Input Inventory
                                double qty = cp.IssueQty;
                                DataTable dt = datatrans.GetData("Select INVENTORY_ITEM.BALANCE_QTY,INVENTORY_ITEM.ITEM_ID,INVENTORY_ITEM.LOCATION_ID,INVENTORY_ITEM.BRANCH_ID,GRNID,INVENTORY_ITEM_ID,TSOURCEID,GRN_DATE from INVENTORY_ITEM where INVENTORY_ITEM.ITEM_ID='" + cp.Item + "' AND INVENTORY_ITEM.LOCATION_ID='" + locid + "' and INVENTORY_ITEM.BRANCH_ID='" + cy.BranchId + "' and LOT_NO='" + cp.Lotno + "' and BALANCE_QTY!=0 order by GRN_DATE ASC");
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
                                            objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = cp.Item;
                                            objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = cp.inpid;
                                            objCmdIn.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = cy.ID;
                                            objCmdIn.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = dt.Rows[i]["GRNID"].ToString();
                                            objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["INVENTORY_ITEM_ID"].ToString();
                                            objCmdIn.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "APPROD";
                                            objCmdIn.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "O";
                                            objCmdIn.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = qty;
                                            objCmdIn.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "APPROD";
                                            objCmdIn.Parameters.Add("TRANS_DATE", OracleDbType.Date).Value = DateTime.Now;
                                            objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                            objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                            objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                            objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = locid;
                                            objCmdIn.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.BranchId;
                                            objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                            objCmdIn.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                            objCmdIn.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                            objCmdIn.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                            objCmdIn.ExecuteNonQuery();



                                            //using (OracleConnection objConnIn = new OracleConnection(_connectionString))
                                            //{
                                            //    OracleCommand objCmdIn = new OracleCommand("DRUMSTKPROC", objConnIn);
                                            //    objCmdIn.CommandType = CommandType.StoredProcedure;
                                            //    objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                            //    objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["INVENTORY_ITEM_ID"].ToString();
                                            //    objCmdIn.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.saveitemId;
                                            //    objCmdIn.Parameters.Add("IN_DATE", OracleDbType.Date).Value = DateTime.Now;
                                            //    objCmdIn.Parameters.Add("DRUM_ID", OracleDbType.NVarchar2).Value = cp.drumid;
                                            //    objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = cp.drumno;
                                            //    objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = cp.Proinid;
                                            //    objCmdIn.Parameters.Add("STOCKTRANSTYPE", OracleDbType.NVarchar2).Value = "BPRODIN";
                                            //    objCmdIn.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.LOCID;
                                            //    objCmdIn.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = qty;
                                            //    objCmdIn.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = 0;
                                            //    objCmdIn.Parameters.Add("OUT_ID", OracleDbType.NVarchar2).Value = 0;
                                            //    objCmdIn.Parameters.Add("BATCHNO", OracleDbType.NVarchar2).Value = cp.batchno;
                                            //    objCmdIn.Parameters.Add("BATCH_QTY", OracleDbType.NVarchar2).Value = cp.batchqty;
                                            //    objCmdIn.Parameters.Add("ISPRODINV", OracleDbType.NVarchar2).Value = "Y";
                                            //    objConnIn.Open();
                                            //    objCmdIn.ExecuteNonQuery();
                                            //    objConnIn.Close();

                                            //}


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
                                            objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = cp.Item;
                                            objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = cp.inpid;
                                            objCmdIn.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = cy.ID;
                                            objCmdIn.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = dt.Rows[i]["GRNID"].ToString();
                                            objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["INVENTORY_ITEM_ID"].ToString();
                                            objCmdIn.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "APPROD";
                                            objCmdIn.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "O";
                                            objCmdIn.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = rqty;
                                            objCmdIn.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "APPROD";
                                            objCmdIn.Parameters.Add("TRANS_DATE", OracleDbType.Date).Value = DateTime.Now;
                                            objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                            objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                            objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                            objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = locid;
                                            objCmdIn.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.BranchId;
                                            objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                            objCmdIn.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                            objCmdIn.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                            objCmdIn.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                            objCmdIn.ExecuteNonQuery();



                                            //using (OracleConnection objConnIn = new OracleConnection(_connectionString))
                                            //{
                                            //    OracleCommand objCmdIn = new OracleCommand("DRUMSTKPROC", objConnIn);
                                            //    objCmdIn.CommandType = CommandType.StoredProcedure;
                                            //    objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                            //    objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["INVENTORY_ITEM_ID"].ToString();
                                            //    objCmdIn.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.saveitemId;
                                            //    objCmdIn.Parameters.Add("IN_DATE", OracleDbType.Date).Value = DateTime.Now;
                                            //    objCmdIn.Parameters.Add("DRUM_ID", OracleDbType.NVarchar2).Value = cp.drumid;
                                            //    objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = cp.drumno;
                                            //    objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = cp.Proinid;
                                            //    objCmdIn.Parameters.Add("STOCKTRANSTYPE", OracleDbType.NVarchar2).Value = "BPRODIN";
                                            //    objCmdIn.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.LOCID;
                                            //    objCmdIn.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = qty;
                                            //    objCmdIn.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = 0;
                                            //    objCmdIn.Parameters.Add("OUT_ID", OracleDbType.NVarchar2).Value = 0;
                                            //    objCmdIn.Parameters.Add("BATCHNO", OracleDbType.NVarchar2).Value = cp.batchno;
                                            //    objCmdIn.Parameters.Add("BATCH_QTY", OracleDbType.NVarchar2).Value = cp.batchqty;
                                            //    objCmdIn.Parameters.Add("ISPRODINV", OracleDbType.NVarchar2).Value = "Y";
                                            //    objConnIn.Open();
                                            //    objCmdIn.ExecuteNonQuery();
                                            //    objConnIn.Close();

                                            //}



                                        }



                                    }
                                }
                                ///////////////////////////// Input Inventory

                            }

                        }
                    }
                    if (cy.Binconslst != null)
                    {
                        foreach (APProInCons cp in cy.Binconslst)
                        {
                            if (cp.Isvalid == "Y" && cp.saveitemId != "0")
                            {
                                string locid = datatrans.GetDataString("Select ILOCATION from WCBASIC where WCBASICID='" + cy.LOCID + "' ");
                                ///////////////////////////// Input Inventory
                                double qty = cp.consQty;
                                DataTable dt = datatrans.GetData("Select INVENTORY_ITEM.BALANCE_QTY,INVENTORY_ITEM.ITEM_ID,INVENTORY_ITEM.LOCATION_ID,GRNID,INVENTORY_ITEM.BRANCH_ID,INVENTORY_ITEM_ID,TSOURCEID,GRN_DATE from INVENTORY_ITEM where INVENTORY_ITEM.ITEM_ID='" + cp.saveitemId + "' AND INVENTORY_ITEM.LOCATION_ID='" + locid + "' and INVENTORY_ITEM.BRANCH_ID='" + cy.BranchId + "' and BALANCE_QTY!=0 order by GRN_DATE ASC");
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
                                            objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value =cp.consid;
                                            objCmdIn.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value =cy.ID;
                                            objCmdIn.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = dt.Rows[i]["GRNID"].ToString();
                                            objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["INVENTORY_ITEM_ID"].ToString();
                                            objCmdIn.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "APPROD";
                                            objCmdIn.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "O";
                                            objCmdIn.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = qty;
                                            objCmdIn.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "APPROD";
                                            objCmdIn.Parameters.Add("TRANS_DATE", OracleDbType.Date).Value = DateTime.Now;
                                            objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                            objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                            objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                            objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = locid;
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
                                            objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = cp.consid;
                                            objCmdIn.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = cy.ID;
                                            objCmdIn.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = dt.Rows[i]["GRNID"].ToString();
                                            objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["INVENTORY_ITEM_ID"].ToString();
                                            objCmdIn.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "APPROD";
                                            objCmdIn.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "O";
                                            objCmdIn.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = rqty;
                                            objCmdIn.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "APPROD";
                                            objCmdIn.Parameters.Add("TRANS_DATE", OracleDbType.Date).Value = DateTime.Now;
                                            objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                            objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                            objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                            objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = locid;
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
                        foreach (ProOutput cp in cy.outlst)
                        {
                            if (cp.Isvalid == "Y" && cp.saveitemId != "0")
                            {
                                string wcid = datatrans.GetDataString("Select WCBASICID from WCBASIC where ILOCATION='10036000012392' ");
                                /////////////////////////output inventory
                                double qty = cp.OutputQty;
                                double totqty =qty+cp.StockQty;

                                OracleCommand objCmdIn = new OracleCommand("DRUMSTKPROC", objConn);
                                objCmdIn.CommandType = CommandType.StoredProcedure;
                                objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                objCmdIn.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.saveitemId;
                                objCmdIn.Parameters.Add("DOC_DATE", OracleDbType.Date).Value = DateTime.Now;
                                objCmdIn.Parameters.Add("DRUM_ID", OracleDbType.NVarchar2).Value = cp.drumid;
                                objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = cp.drumno;
                                objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = cp.outid;
                                objCmdIn.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = cy.ID;
                                objCmdIn.Parameters.Add("STOCKTRANSTYPE", OracleDbType.NVarchar2).Value = "APOUTDET";
                                objCmdIn.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = "10036000012392";
                                objCmdIn.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = wcid;
                                objCmdIn.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = totqty;
                                objCmdIn.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = totqty;

                                objCmdIn.Parameters.Add("OUT_ID", OracleDbType.NVarchar2).Value = "0";
                                objCmdIn.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;

                                //try
                                //{

                                objCmdIn.ExecuteNonQuery();
                                Object Pid1 = objCmdIn.Parameters["OUTID"].Value;


                               


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
                                objCmdInp.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = cy.ID;
                                objCmdInp.Parameters.Add("SOURCETYPE", OracleDbType.NVarchar2).Value = "APOUTDET";
                                objCmdInp.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = "10036000012392";
                                objCmdInp.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = wcid;

                                objCmdInp.Parameters.Add("PLUSQTY", OracleDbType.NVarchar2).Value = totqty;
                                objCmdInp.Parameters.Add("MINSQTY", OracleDbType.NVarchar2).Value = "0";
                                objCmdInp.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                objCmdInp.Parameters.Add("LOTNO", OracleDbType.NVarchar2).Value = docid;
                                objCmdInp.Parameters.Add("SHEDNO", OracleDbType.NVarchar2).Value = "";

                                objCmdInp.ExecuteNonQuery();
                                string Sql = string.Empty;
                                Sql = "Update DRUMMAST SET  DRUMLOC='10036000012392',IS_EMPTY='N' WHERE DRUMNO='" + cp.drumno + "'";
                                OracleCommand objCmds = new OracleCommand(Sql, objConn);
                                objCmds.ExecuteNonQuery();
                                /////////////////////////output inventory

                                Sql = "Update APPRODUCTIONBASIC SET  IS_APPROVE='Y' WHERE APPRODUCTIONBASICID='" + cy.ID + "'";
                                objCmds = new OracleCommand(Sql, objConn);
                                objCmds.ExecuteNonQuery();

                               


                            }






                                objConn.Close();
                            }
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
        public DataTable Getstkqty(string ItemId, string locid, string brid)
        {
            string SvSql = string.Empty;
            SvSql = "select SUM(BALANCE_QTY) as QTY from INVENTORY_ITEM where BALANCE_QTY > 0 AND LOCATION_ID='" + locid + "' AND BRANCH_ID='" + brid + "' AND ITEM_ID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetConstkqty(string ItemId, string locid, string brid)
        {
            string SvSql = string.Empty;
            SvSql = "select SUM(BALANCE_QTY) as QTY from INVENTORY_ITEM where BALANCE_QTY > 0 AND LOCATION_ID='" + locid + "' AND BRANCH_ID='" + brid + "' AND ITEM_ID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetLotNo(string ItemId, string locid, string brid)
        {
            string SvSql = string.Empty;
            SvSql = "select LOT_NO from INVENTORY_ITEM where BALANCE_QTY > 0 AND ITEM_ID='" + ItemId + "' AND LOCATION_ID='" + locid + "' AND BRANCH_ID='" + brid + "'  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetStkDetails( string Lot, string brid, string loc, string item)
        {
            string SvSql = string.Empty;
            SvSql = "select BALANCE_QTY from INVENTORY_ITEM where BALANCE_QTY > 0 AND LOT_NO='"+ Lot + "' AND LOCATION_ID='" + loc + "' AND BRANCH_ID='" + brid + "' AND ITEM_ID='" + item + "'  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
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
                                svSQL = "Insert into APPRODCONSDET (APPRODUCTIONBASICID,ITEMID,UNIT,BINID,STOCK,QTY,CONSQTY) VALUES ('" + cp.APID + "','" + cp.ItemId + "','" + cp.consunit + "','" + cp.BinId + "','" + cp.ConsStock + "','" + cp.Qty + "','" + cp.consQty + "')";
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
                                svSQL = "Insert into APPRODOUTDET (APPRODUCTIONBASICID,ITEMID,BINID,DRUMNO,OUTQTY,TIME) VALUES ('" + cp.APID + "','" + cp.ItemId + "','" + cp.Bin + "','" + cp.drumno + "','" + cp.OutputQty + "','" + cp.FromTime + "')";
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
                    using (OracleConnection objConn = new OracleConnection(_connectionString))
                    {
                        objConn.Open();

                        if (cy.change == "Complete")
                        {
                            svSQL = "Update APPRODUCTIONBASIC SET IS_CURRENT='No' WHERE APPRODUCTIONBASICID='"+ cy.ID +"'";
                            OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                            objCmdd.ExecuteNonQuery();
                        }
                        else 
                    { 
                        DataTable ap = datatrans.GetData("select APPRODUCTIONBASICID,DOCID,WCID,to_char(APPRODUCTIONBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,SHIFT,ASSIGNENG,SCHQTY,PRODQTY,BATCH,BATCHYN,BRANCHID from APPRODUCTIONBASIC WHERE IS_CURRENT='Yes'");
                        svSQL = "Update APPRODUCTIONBASIC SET IS_CURRENT='No' WHERE APPRODUCTIONBASICID='"+ cy.ID + "'";
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
                         objCmd.ExecuteNonQuery();
                            Object Pid = objCmd.Parameters["OUTID"].Value;

                            
                            cy.APID = Pid;
                            
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

        public DataTable GetAPProd(string id)
		{
			string SvSql = string.Empty;
			SvSql = "select APPRODUCTIONBASICID,APPRODUCTIONBASIC.DOCID,to_char(APPRODUCTIONBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,WCBASIC.WCID,EMPMAST.EMPNAME,SCHQTY,PRODQTY,BATCH,SHIFT,BATCHYN,WCBASIC.ILOCATION,APPRODUCTIONBASIC.BRANCHID from APPRODUCTIONBASIC left outer join WCBASIC ON WCBASICID= APPRODUCTIONBASIC.WCID left outer join EMPMAST ON EMPMASTID= APPRODUCTIONBASIC.ASSIGNENG  where APPRODUCTIONBASICID='" + id + "' ";

			DataTable dtt = new DataTable();
			OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
			OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
			adapter.Fill(dtt);
			return dtt;
		}
        public DataTable GetInput(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select APPRODUCTIONBASICID,ITEMID,BINID,BATCHNO,STOCK,QTY,CHARGINGTIME from APPRODINPDET  where APPRODUCTIONBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetCons(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select APPRODUCTIONBASICID,ITEMID,UNIT,BINID,STOCK,QTY,CONSQTY from APPRODCONSDET  where APPRODUCTIONBASICID='" + id + "' ";
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
            SvSql = "select APPRODOUTDETID,APPRODUCTIONBASICID,APPRODOUTDET.ITEMID,BINID,OUTQTY,DRUMNO,FROMTIME,TOTIME,ITEMMASTER.ITEMID as ITEMNAME,TESTRESULT,MOVETOQC,STATUS,EXQTY from APPRODOUTDET  LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=APPRODOUTDET.ITEMID  where APPRODUCTIONBASICID='" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetLogdetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select APPRODUCTIONBASICID,to_char(APPRODLOGDET.STARTDATE,'dd-MON-yyyy')STARTDATE,to_char(APPRODLOGDET.ENDDATE,'dd-MON-yyyy')ENDDATE,STARTTIME,ENDTIME,TOTALHRS,REASON from APPRODLOGDET where APPRODUCTIONBASICID='" + id + "' ";

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
        public DataTable SaveOutDetails(string id, string item, string drum, string time, string qty,string totime, string exqty, string stat, string stock)
        {
            string SvSql = string.Empty;
            string is_account=string.Empty;
            if(stat== "PENDING")
            {
                is_account = "N";
            }
            SvSql = "Insert into APPRODOUTDET (APPRODUCTIONBASICID,ITEMID,DRUMNO,FROMTIME,OUTQTY,TOTIME,EXQTY,STATUS,IS_ACCOUNTED,STKQTY) VALUES ('" + id + "','" + item + "','" + drum + "','" + time + "','" + qty + "','" + totime + "','" + exqty + "','" + stat + "','"+ is_account + "','"+stock+"')";

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
                 SvSql = "Delete APPRODINPDET WHERE APPRODUCTIONBASICID='" + id + "'";
                 OracleCommand objCmdd = new OracleCommand(SvSql, objConnT);
                 objCmdd.ExecuteNonQuery();
            }
            SvSql = "Insert into APPRODINPDET (APPRODUCTIONBASICID,ITEMID,BINID,CHARGINGTIME,QTY,STOCK,BATCHNO) VALUES ('" + id + "','" + item + "','" + bin + "','" + time + "','" + qty + "','" + stock + "','" + batch + "')";
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
                SvSql = "Delete APPRODCONSDET WHERE APPRODUCTIONBASICID='" + id + "'";
                OracleCommand objCmdd = new OracleCommand(SvSql, objConnT);
                objCmdd.ExecuteNonQuery();
            }

            SvSql = "Insert into APPRODCONSDET (APPRODUCTIONBASICID,ITEMID,BINID,UNIT,QTY,CONSQTY,STOCK) VALUES ('" + id + "','" + item + "','" + bin + "','" + unit + "','" + usedqty + "','" + qty + "','" + stock + "')";
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
                SvSql = "Delete APPRODEMPDET WHERE APPRODUCTIONBASICID='" + id + "'";
                OracleCommand objCmdd = new OracleCommand(SvSql, objConnT);
                objCmdd.ExecuteNonQuery();
            }

            SvSql = "Insert into APPRODEMPDET (APPRODUCTIONBASICID,EMPID,EMPCODE,DEPARTMENT,STARTDATE,STARTTIME,ENDDATE,ENDTIME,OTHOUR,ETOTHER,NHOUR,NATUREOFWORK) VALUES ('" + id + "','" + empname + "','" + code + "','" + depat + "','" + sdate + "','" + stime + "','" + edate + "','" + etime + "','" + ot + "','" + et + "','" + normal + "','" + now + "')";

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
                SvSql = "Delete APPRODBREAKDET WHERE APPRODUCTIONBASICID='" + id + "'";
                OracleCommand objCmdd = new OracleCommand(SvSql, objConnT);
                objCmdd.ExecuteNonQuery();
            }
            SvSql = "Insert into APPRODBREAKDET (APPRODUCTIONBASICID,MACHCODE,DESCRIPTION,DTYPE,MTYPE,FROMTIME,TOTIME,PB,ALLOTTEDTO,REASON) VALUES ('" + id + "','" + machine + "','" + des + "','" + dtype + "','" + mtype + "','" + stime + "','" + etime + "','" + pb + "','" + all + "','" + reason + "')";

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
                SvSql = "Delete APPRODLOGDET WHERE APPRODUCTIONBASICID='" + id + "'";
                OracleCommand objCmdd = new OracleCommand(SvSql, objConnT);
                objCmdd.ExecuteNonQuery();
            }
            SvSql = "Insert into APPRODLOGDET (APPRODUCTIONBASICID,STARTDATE,STARTTIME,ENDDATE,ENDTIME,TOTALHRS,REASON) VALUES ('" + id + "','" + sdate + "','" + stime + "','" + edate + "','" + etime + "','" + tot + "','" + reason + "')";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public async Task<IEnumerable<APItemDetail>> GetAPItem(string aid )
        {
            using OracleConnection db = new OracleConnection(_connectionString);
            {
                 
                    return await db.QueryAsync<APItemDetail>("SELECT APPRODUCTIONBASIC.APPRODUCTIONBASICID, WCBASIC.WCID, APPRODUCTIONBASIC.DOCID,to_char(APPRODUCTIONBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE, APPRODUCTIONBASIC.SHIFT, ITEMMASTER.ITEMID, APPRODINPDET.APPRODUCTIONBASICID AS EXPR1, APPRODINPDET.APPRODINPDETID, APPRODINPDET.QTY,APPRODINPDET.CHARGINGTIME, APPRODINPDET.UNIT, APPRODOUTDET.APPRODOUTDETID, APPRODOUTDET.APPRODUCTIONBASICID AS EXPR2,DRUMMAST.DRUMNO, APPRODOUTDET.OUTQTY, APPRODOUTDET.FROMTIME, APPRODOUTDET.TESTRESULT, APPRODOUTDET.TOTIME ,APPRODBREAKDET.FROMTIME AS EXPR7, APPRODBREAKDET.TOTIME AS EXPR8 ,EMPMAST.EMPNAME,APPRODCONSDET.APPRODUCTIONBASICID AS EXPR4,APPRODCONSDET.APPRODCONSDETID,  APPRODCONSDET.ITEMID AS EXPR6,  APPRODCONSDET.CONSQTY,  APPRODCONSDET.QTY AS EXPR5 from APPRODUCTIONBASIC left outer join WCBASIC on WCBASIC.WCBASICID=APPRODUCTIONBASIC.WCID  INNER JOIN APPRODINPDET ON APPRODUCTIONBASIC.APPRODUCTIONBASICID = APPRODINPDET.APPRODUCTIONBASICID left outer join ITEMMASTER ON ITEMMASTER.ITEMMASTERID=APPRODINPDET.ITEMID INNER JOIN APPRODOUTDET ON APPRODUCTIONBASIC.APPRODUCTIONBASICID = APPRODOUTDET.APPRODUCTIONBASICID left outer join DRUMMAST on DRUMMAST.DRUMMASTID=APPRODOUTDET.DRUMNO INNER JOIN APPRODCONSDET ON APPRODUCTIONBASIC.APPRODUCTIONBASICID = APPRODCONSDET.APPRODUCTIONBASICID INNER JOIN  APPRODBREAKDET ON APPRODUCTIONBASIC.APPRODUCTIONBASICID = APPRODBREAKDET.APPRODUCTIONBASICID INNER JOIN  APPRODEMPDET ON APPRODUCTIONBASIC.APPRODUCTIONBASICID = APPRODEMPDET.APPRODUCTIONBASICID left outer join EMPMAST on EMPMAST.EMPMASTID=APPRODEMPDET.EMPID where APPRODUCTIONBASIC.APPRODUCTIONBASICID='" + aid + "' and APPRODINPDET.APPRODUCTIONBASICID='"+ aid + "' and APPRODOUTDET.APPRODUCTIONBASICID='"+ aid + "' and APPRODEMPDET.APPRODUCTIONBASICID='"+ aid + "' and APPRODBREAKDET.APPRODUCTIONBASICID='"+ aid + "' and APPRODCONSDET.APPRODUCTIONBASICID='"+ aid + "'", commandType: CommandType.Text);


                
            }
             
        }
        public async Task<IEnumerable<APItemDetails>> GetAPItems(string bid )
        {
            using OracleConnection db = new OracleConnection(_connectionString);
            {
                return await db.QueryAsync<APItemDetails>("SELECT TAAIERP.APPRODUCTIONBASIC.APPRODUCTIONBASICID, TAAIERP.APPRODUCTIONBASIC.DOCID, to_char(APPRODUCTIONBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE, TAAIERP.APPRODUCTIONBASIC.SHIFT,TAAIERP.APPRODINPDET.APPRODINPDETID, TAAIERP.APPRODINPDET.APPRODUCTIONBASICID AS EXPR1, ITEMMASTER.ITEMID, TAAIERP.APPRODINPDET.QTY, TAAIERP.APPRODINPDET.CHARGINGTIME, TAAIERP.APPRODINPDET.UNIT, TAAIERP.APPRODCONSDET.APPRODCONSDETID, TAAIERP.APPRODCONSDET.APPRODUCTIONBASICID AS EXPR2, TAAIERP.APPRODCONSDET.CONSQTY,  TAAIERP.APPRODCONSDET.QTY AS EXPR3, DRUMMAST.DRUMNO, TAAIERP.APPRODOUTDET.ITEMID AS EXPR4, TAAIERP.APPRODOUTDET.APPRODUCTIONBASICID AS EXPR5,TAAIERP.APPRODOUTDET.APPRODOUTDETID, TAAIERP.APPRODOUTDET.OUTQTY, TAAIERP.APPRODOUTDET.FROMTIME, TAAIERP.APPRODOUTDET.TESTRESULT, TAAIERP.APPRODOUTDET.TOTIME, TAAIERP.APPRODBREAKDET.APPRODBREAKDETID, TAAIERP.APPRODBREAKDET.FROMTIME AS EXPR6, TAAIERP.APPRODBREAKDET.TOTIME AS EXPR7, TAAIERP.APPRODBREAKDET.PB,  TAAIERP.APPRODBREAKDET.REASON, TAAIERP.APPRODEMPDET.APPRODEMPDETID, EMPMAST.EMPNAME FROM  TAAIERP.APPRODUCTIONBASIC INNER JOIN TAAIERP.APPRODINPDET ON TAAIERP.APPRODUCTIONBASIC.APPRODUCTIONBASICID = TAAIERP.APPRODINPDET.APPRODUCTIONBASICID left outer join ITEMMASTER ON ITEMMASTER.ITEMMASTERID=APPRODINPDET.ITEMID INNER JOIN TAAIERP.APPRODOUTDET ON TAAIERP.APPRODUCTIONBASIC.APPRODUCTIONBASICID = TAAIERP.APPRODOUTDET.APPRODUCTIONBASICID left outer join DRUMMAST on DRUMMAST.DRUMMASTID=APPRODOUTDET.DRUMNO INNER JOIN TAAIERP.APPRODCONSDET ON TAAIERP.APPRODUCTIONBASIC.APPRODUCTIONBASICID = TAAIERP.APPRODCONSDET.APPRODUCTIONBASICID INNER JOIN  TAAIERP.APPRODBREAKDET ON TAAIERP.APPRODUCTIONBASIC.APPRODUCTIONBASICID = TAAIERP.APPRODBREAKDET.APPRODUCTIONBASICID INNER JOIN TAAIERP.APPRODEMPDET ON TAAIERP.APPRODUCTIONBASIC.APPRODUCTIONBASICID = TAAIERP.APPRODEMPDET.APPRODUCTIONBASICID left outer join EMPMAST on EMPMAST.EMPMASTID=APPRODEMPDET.EMPID WHERE APPRODUCTIONBASIC.APPRODUCTIONBASICID='" + bid + "' and APPRODINPDET.APPRODUCTIONBASICID='" + bid + "' and APPRODOUTDET.APPRODUCTIONBASICID='" + bid + "' and APPRODEMPDET.APPRODUCTIONBASICID='" + bid + "' and APPRODBREAKDET.APPRODUCTIONBASICID='" + bid + "' and APPRODCONSDET.APPRODUCTIONBASICID='" + bid + "'", commandType: CommandType.Text);
            }
        }
        public async Task<IEnumerable<APItemDetailsc>> GetAPItemsc(string cid)
        {
            using OracleConnection db = new OracleConnection(_connectionString);
            {
                return await db.QueryAsync<APItemDetailsc>("SELECT TAAIERP.APPRODUCTIONBASIC.APPRODUCTIONBASICID, TAAIERP.APPRODUCTIONBASIC.DOCID, to_char(APPRODUCTIONBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE, TAAIERP.APPRODUCTIONBASIC.SHIFT,TAAIERP.APPRODINPDET.APPRODINPDETID, TAAIERP.APPRODINPDET.APPRODUCTIONBASICID AS EXPR1, ITEMMASTER.ITEMID, TAAIERP.APPRODINPDET.QTY, TAAIERP.APPRODINPDET.CHARGINGTIME, TAAIERP.APPRODINPDET.UNIT, TAAIERP.APPRODCONSDET.APPRODCONSDETID, TAAIERP.APPRODCONSDET.APPRODUCTIONBASICID AS EXPR2, TAAIERP.APPRODCONSDET.CONSQTY,  TAAIERP.APPRODCONSDET.QTY AS EXPR3, DRUMMAST.DRUMNO, TAAIERP.APPRODOUTDET.ITEMID AS EXPR4, TAAIERP.APPRODOUTDET.APPRODUCTIONBASICID AS EXPR5,TAAIERP.APPRODOUTDET.APPRODOUTDETID, TAAIERP.APPRODOUTDET.OUTQTY, TAAIERP.APPRODOUTDET.FROMTIME, TAAIERP.APPRODOUTDET.TESTRESULT, TAAIERP.APPRODOUTDET.TOTIME, TAAIERP.APPRODBREAKDET.APPRODBREAKDETID, TAAIERP.APPRODBREAKDET.FROMTIME AS EXPR6, TAAIERP.APPRODBREAKDET.TOTIME AS EXPR7, TAAIERP.APPRODBREAKDET.PB,  TAAIERP.APPRODBREAKDET.REASON, TAAIERP.APPRODEMPDET.APPRODEMPDETID, EMPMAST.EMPNAME FROM  TAAIERP.APPRODUCTIONBASIC INNER JOIN TAAIERP.APPRODINPDET ON TAAIERP.APPRODUCTIONBASIC.APPRODUCTIONBASICID = TAAIERP.APPRODINPDET.APPRODUCTIONBASICID left outer join ITEMMASTER ON ITEMMASTER.ITEMMASTERID=APPRODINPDET.ITEMID INNER JOIN TAAIERP.APPRODOUTDET ON TAAIERP.APPRODUCTIONBASIC.APPRODUCTIONBASICID = TAAIERP.APPRODOUTDET.APPRODUCTIONBASICID left outer join DRUMMAST on DRUMMAST.DRUMMASTID=APPRODOUTDET.DRUMNO INNER JOIN TAAIERP.APPRODCONSDET ON TAAIERP.APPRODUCTIONBASIC.APPRODUCTIONBASICID = TAAIERP.APPRODCONSDET.APPRODUCTIONBASICID INNER JOIN  TAAIERP.APPRODBREAKDET ON TAAIERP.APPRODUCTIONBASIC.APPRODUCTIONBASICID = TAAIERP.APPRODBREAKDET.APPRODUCTIONBASICID INNER JOIN TAAIERP.APPRODEMPDET ON TAAIERP.APPRODUCTIONBASIC.APPRODUCTIONBASICID = TAAIERP.APPRODEMPDET.APPRODUCTIONBASICID left outer join EMPMAST on EMPMAST.EMPMASTID=APPRODEMPDET.EMPID WHERE APPRODUCTIONBASIC.APPRODUCTIONBASICID='" + cid + "' and APPRODINPDET.APPRODUCTIONBASICID='" + cid + "' and APPRODOUTDET.APPRODUCTIONBASICID='" + cid + "' and APPRODEMPDET.APPRODUCTIONBASICID='" + cid + "' and APPRODBREAKDET.APPRODUCTIONBASICID='" + cid + "' and APPRODCONSDET.APPRODUCTIONBASICID='" + cid + "'", commandType: CommandType.Text);
            }
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

        public DataTable GetAPProductionentryName(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select APPRODUCTIONBASICID,APPRODUCTIONBASIC.DOCID,to_char(APPRODUCTIONBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,WCBASIC.WCID,EMPMAST.EMPNAME,SCHQTY,PRODQTY,BATCH,SHIFT,BATCHYN,WCBASICID from APPRODUCTIONBASIC left outer join WCBASIC ON WCBASICID= APPRODUCTIONBASIC.WCID left outer join EMPMAST ON EMPMASTID= APPRODUCTIONBASIC.ASSIGNENG  where APPRODUCTIONBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetInputDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select APPRODUCTIONBASICID,ITEMMASTER.ITEMID,APPRODINPDET.ITEMID as item,APPRODINPDETID ,BATCHNO,BINID,STOCK,QTY,CHARGINGTIME from APPRODINPDET  left outer join ITEMMASTER ON ITEMMASTER.ITEMMASTERID= APPRODINPDET.ITEMID  where APPRODUCTIONBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetConsDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select APPRODUCTIONBASICID,ITEMMASTER.ITEMID, APPRODCONSDET.ITEMID as item,APPRODCONSDET.UNIT,APPRODCONSDET.BINID,STOCK,QTY,UNITMAST.UNITID,CONSQTY,APPRODCONSDETID from APPRODCONSDET left outer join ITEMMASTER ON ITEMMASTER.ITEMMASTERID= APPRODCONSDET.ITEMID  LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID where APPRODUCTIONBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEmpdetDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select APPRODUCTIONBASICID,EMPMAST.EMPID,EMPCODE,DEPARTMENT,to_char(APPRODEMPDET.STARTDATE,'dd-MON-yyyy')STARTDATE,to_char(APPRODEMPDET.ENDDATE,'dd-MON-yyyy')ENDDATE,STARTTIME,ENDTIME,OTHOUR,ETOTHER,NHOUR,NATUREOFWORK from APPRODEMPDET left outer join EMPMAST ON EMPMAST.EMPMASTID= APPRODEMPDET.EMPID where APPRODUCTIONBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetBreakDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select APPRODUCTIONBASICID,machineinfobasic.MCODE,DESCRIPTION,DTYPE,APPRODBREAKDET.MTYPE,FROMTIME,TOTIME,PB,EMPMAST.EMPNAME,REASON from APPRODBREAKDET left outer join machineinfobasic on machineinfobasic.machineinfobasicid=APPRODBREAKDET.MACHCODE LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID=APPRODBREAKDET.ALLOTTEDTO\r\n  where APPRODUCTIONBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetOutputDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select APPRODOUTDETID,APPRODUCTIONBASICID,ITEMMASTER.ITEMID,APPRODOUTDET.ITEMID as item,BINID,OUTQTY,DRUMMAST.DRUMNO,FROMTIME,TOTIME,ITEMMASTER.ITEMID as ITEMNAME,TESTRESULT,MOVETOQC,EXQTY,APPRODOUTDET.STATUS,APPRODOUTDET.DRUMNO as drum from APPRODOUTDET  LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=APPRODOUTDET.ITEMID LEFT OUTER JOIN DRUMMAST ON DRUMMAST.DRUMMASTID=APPRODOUTDET.DRUMNO  where APPRODUCTIONBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetLogdetailDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select APPRODUCTIONBASICID,to_char(APPRODLOGDET.STARTDATE,'dd-MON-yyyy')STARTDATE,to_char(APPRODLOGDET.ENDDATE,'dd-MON-yyyy')ENDDATE,STARTTIME,ENDTIME,TOTALHRS,REASON from APPRODLOGDET where APPRODUCTIONBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
       
    }
}
