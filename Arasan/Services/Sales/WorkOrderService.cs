using Arasan.Interface.Sales;
using Arasan.Models;
using System.Data;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
namespace Arasan.Services.Sales
{
    public class WorkOrderService : IWorkOrderService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;

        public WorkOrderService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);

        }
        public IEnumerable<WorkOrder> GetAllWorkOrder()
		{
			List<WorkOrder> cmpList = new List<WorkOrder>();
			using (OracleConnection con = new OracleConnection(_connectionString))
			{

				using (OracleCommand cmd = con.CreateCommand())
				{
					con.Open();
					cmd.CommandText = "Select JOBASIC.DOCID,to_char(JOBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PARTYRCODE.PARTY,LOCDETAILS.LOCID,BRANCHMAST.BRANCHID,JOBASICID from JOBASIC  left outer join LOCDETAILS on LOCDETAILS.LOCDETAILSID=JOBASIC.LOCID  left outer join BRANCHMAST on BRANCHMAST.BRANCHMASTID=JOBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on JOBASIC.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Customer','BOTH')";
					OracleDataReader rdr = cmd.ExecuteReader();
					while (rdr.Read())
					{
						WorkOrder cmp = new WorkOrder
						{
							ID = rdr["JOBASICID"].ToString(),
							JopId = rdr["DOCID"].ToString(),
							Customer = rdr["PARTY"].ToString(),
							JopDate = rdr["DOCDATE"].ToString(),
							Location = rdr["LOCID"].ToString(),
							 
							Branch = rdr["BRANCHID"].ToString()

						};
						cmpList.Add(cmp);
					}
				}
			}
			return cmpList;
		}
		public DataTable GetQuo()
        {
            string SvSql = string.Empty;
            SvSql = "select QUOTE_NO,SALES_QUOTE.ID from SALES_QUOTE ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetQuoDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PARTYRCODE.PARTY,CURRENCY.MAINCURR from SALES_QUOTE LEFT OUTER JOIN CURRENCY ON CURRENCY.CURRENCYID=SALES_QUOTE.CURRENCY_TYPE LEFT OUTER JOIN  PARTYMAST on SALES_QUOTE.CUSTOMER=PARTYMAST.PARTYMASTID LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Customer','BOTH') AND SALES_QUOTE.ID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string WorkOrderCRUD(WorkOrder cy)
        {
			string msg = "";
			try
			{
				string StatementType = string.Empty; string svSQL = "";
                string party = datatrans.GetDataString("Select ID from PARTYRCODE where PARTY='" + cy.Customer + "' ");
                string partyid = datatrans.GetDataString("Select PARTYMASTID from PARTYMAST where PARTYNAME='" + party + "' ");


                string currency = datatrans.GetDataString("Select CURRENCYID from CURRENCY where MAINCURR='" + cy.Currency + "' ");
                string symbol = datatrans.GetDataString("Select SYMBOL from CURRENCY where MAINCURR='" + cy.Currency + "' ");
                using (OracleConnection objConn = new OracleConnection(_connectionString))
				{
					OracleCommand objCmd = new OracleCommand("WORKORDERPROC", objConn);
					/*objCmd.Connection = objConn;
                    objCmd.CommandText = "PURQUOPROC";*/

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


                    //objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                    objCmd.Parameters.Add("QUOID", OracleDbType.NVarchar2).Value = cy.Quo;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.JopId;
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
					objCmd.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.Location;
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = partyid;
                    objCmd.Parameters.Add("PARTYNAME", OracleDbType.NVarchar2).Value = party;

                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.JopDate;
					objCmd.Parameters.Add("CREFNO", OracleDbType.NVarchar2).Value = cy.CusNo;
                    objCmd.Parameters.Add("CREFDATE", OracleDbType.NVarchar2).Value = cy.Cusdate;

                    objCmd.Parameters.Add("MAINCURRENCY", OracleDbType.NVarchar2).Value = currency;
                    objCmd.Parameters.Add("MAINCURRENCY", OracleDbType.NVarchar2).Value = symbol;
                    objCmd.Parameters.Add("EXRATE", OracleDbType.NVarchar2).Value = cy.ExRate;

                    objCmd.Parameters.Add("TRANSAMOUNT", OracleDbType.NVarchar2).Value = cy.TransAmount;
					objCmd.Parameters.Add("CRLIMIT", OracleDbType.NVarchar2).Value = cy.CreditLimit;
					//objCmd.Parameters.Add("CONTACT_PERSON_MOBILE", OracleDbType.NVarchar2).Value = cy.SalesValue;
					objCmd.Parameters.Add("ORDTYPE", OracleDbType.NVarchar2).Value = cy.OrderType;
					objCmd.Parameters.Add("RATETYPE", OracleDbType.NVarchar2).Value = cy.RateType;
					objCmd.Parameters.Add("RATECODE", OracleDbType.NVarchar2).Value = cy.RateCode;
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
                        if (cy.Worklst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (WorkItem cp in cy.Worklst)
                                {
                                     
                                    string UnitId = datatrans.GetDataString("Select UNITMASTID from UNITMAST where UNITID='" + cp.unit + "' ");

                                    if (cp.Isvalid == "Y"  )
                                    {
                                        svSQL = "Insert into JODETAIL (JOBASICID,QTY,MATSUPP,ITEMID,DCQTY,RATE,AMOUNT,UNIT,ITEMSPEC,PACKSPEC,DISCOUNT,FREIGHTAMT,QDISC,CDISC,IDISC,TDISC,ADISC,SDISC,FREIGHT,TAXTYPE) VALUES ('" + Pid + "','" + cp.orderqty + "','" + cp.matsupply + "','" + cp.itemid + "','" + cp.disqty + "','" + cp.rate + "','" + cp.amount + "','" + UnitId + "','" + cp.itemspec + "','" + cp.packind + "','" + cp.discount + "','" + cp.freightamt + "','" + cp.qtydis + "','" + cp.cashdis + "','" + cp.introdis + "','" + cp.tradedis + "','" + cp.additiondis + "','" + cp.spldis + "','" + cp.freight + "','" + cp.taxtype + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                svSQL = "Delete JODETAIL WHERE JOBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (WorkItem cp in cy.Worklst)
                                {
                                   
                                    string UnitId = datatrans.GetDataString("Select UNITMASTID from UNITMAST where UNITID='" + cp.unit + "' ");
                                    if (cp.Isvalid == "Y"  )
                                    {
                                        svSQL = "Insert into JODETAIL (JOBASICID,QTY,MATSUPP,ITEMID,DCQTY,RATE,AMOUNT,UNIT,ITEMSPEC,PACKSPEC,DISCOUNT,FREIGHTAMT,QDISC,CDISC,IDISC,TDISC,ADISC,SDISC,FREIGHT,TAXTYPE) VALUES ('" + Pid + "','" + cp.orderqty + "','" + cp.matsupply + "','" + cp.itemid + "','" + cp.disqty + "','" + cp.rate + "','" + cp.amount + "','" + UnitId + "','" + cp.itemspec + "','" + cp.packind + "','" + cp.discount + "','" + cp.freightamt + "','" + cp.qtydis + "','" + cp.cashdis + "','" + cp.introdis + "','" + cp.tradedis + "','" + cp.additiondis + "','" + cp.spldis + "','" + cp.freight + "','" + cp.taxtype + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
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
        public DataTable GetSatesQuoDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select SALESQUOTEDETAIL.ITEMID as itemi,ITEMMASTER.ITEMID,UNIT,DISCAMOUNT,TOTAMT,QTY,RATE,SALESQUOTEDETAILID from SALESQUOTEDETAIL left outer join ITEMMASTER ON ITEMMASTER.ITEMMASTERID=SALESQUOTEDETAIL.ITEMID where SALESQUOID='" + id +"' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetWorkOrder(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select QUOID,JOBASIC.DOCID,JOBASIC.BRANCHID,JOBASIC.LOCID,PARTYRCODE.PARTY,to_char(JOBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,CREFNO,to_char(JOBASIC.CREFDATE,'dd-MON-yyyy')CREFDATE,CURRENCY.MAINCURR,JOBASIC.EXRATE,JOBASIC.TRANSAMOUNT,CRLIMIT,ORDTYPE,JOBASIC.RATETYPE,JOBASIC.RATECODE from JOBASIC LEFT OUTER JOIN CURRENCY ON CURRENCY.CURRENCYID=JOBASIC.MAINCURRENCY LEFT OUTER JOIN  PARTYMAST on JOBASIC.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Customer','BOTH')  And JOBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetWorkOrderDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select JOBASICID,QTY,MATSUPP,ITEMMASTER.ITEMID,DCQTY,RATE,AMOUNT,UNITMAST.UNITID,ITEMSPEC,PACKSPEC,DISCOUNT,FREIGHTAMT,QDISC,CDISC,IDISC,TDISC,ADISC,SDISC,FREIGHT,TAXTYPE from JODETAIL left outer join ITEMMASTER ON ITEMMASTER.ITEMMASTERID=JODETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT Where JOBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
