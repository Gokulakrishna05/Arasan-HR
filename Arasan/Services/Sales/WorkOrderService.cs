using Arasan.Interface.Sales;
using Arasan.Models;
using System.Data;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using Dapper;

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
        public IEnumerable<WorkOrder> GetAllWorkOrder(string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                status = "ACTIVE";
            }

            List<WorkOrder> cmpList = new List<WorkOrder>();
			using (OracleConnection con = new OracleConnection(_connectionString))
			{

				using (OracleCommand cmd = con.CreateCommand())
				{
					con.Open();
					cmd.CommandText = "Select JOBASIC.DOCID,to_char(JOBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PARTYMAST.PARTYNAME PARTY,LOCDETAILS.LOCID,BRANCHMAST.BRANCHID,JOBASICID,JOBASIC.STATUS from JOBASIC  left outer join LOCDETAILS on LOCDETAILS.LOCDETAILSID=JOBASIC.LOCID  left outer join BRANCHMAST on BRANCHMAST.BRANCHMASTID=JOBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on JOBASIC.PARTYID=PARTYMAST.PARTYMASTID  Where  JOBASIC.STATUS='" + status + "' order by JOBASIC.JOBASICID DESC ";
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
							Branch = rdr["BRANCHID"].ToString(),
                            status = rdr["STATUS"].ToString()
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
            SvSql = "select QUOTE_NO,SALES_QUOTE.SALESQUOTEID from SALES_QUOTE ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetTax(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select TARIFFMASTER.TARIFFID from HSNROW LEFT OUTER JOIN TARIFFMASTER ON TARIFFMASTERID=HSNROW.TARIFFID WHERE HSNCODEID='" + id + "' ";
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
			string Pid = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);


                 

                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'JOB#' AND ACTIVESEQUENCE = 'T'  ");
                string DocId = string.Format("{0}{1}", "JOB#", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='JOB#' AND ACTIVESEQUENCE ='T'  ";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                cy.JopId = DocId;


                DataTable party = datatrans.GetData("Select PARTYNAME,RATECODE,ADD1,ADD2,ADD3,PINCODE,EMAIL,CITY,STATE,ACCOUNTNAME,CSTNO,SSTNO,MOBILE,CONCODE,PARTYGROUP from PARTYMAST where PARTYMASTID='" + cy.Customer + "' ");
 

                 string symbol = datatrans.GetDataString("Select SYMBOL from CURRENCY where CURRENCY='" + cy.Currency + "' ");
                using (OracleConnection objConn = new OracleConnection(_connectionString))
				{
                    objConn.Open();
     //               OracleCommand objCmd = new OracleCommand("WORKORDERPROC", objConn);
					///*objCmd.Connection = objConn;
     //               objCmd.CommandText = "PURQUOPROC";*/

					//objCmd.CommandType = CommandType.StoredProcedure;

                    if (cy.ID == null)
                    {
                        string SvSqlo = "Insert into JOBASIC (APPROVAL,MAXAPPROVED,CANCEL,T1SOURCEID,LATEMPLATEID,DOCID,DOCDATE,PARTYNAME,CREFNO,CREFDATE,SMSDATE,PARTYID,TRANSID,SENDSMS,REFNO,BRANCHID,MAINCURRENCY,SYMBOL,EXRATE,USERID,RATECODE,TYPE,ORDTYPE,ENTEREDBY,AITEMSPEC,RATETYPE,LOCID,ADD1,ADD2,ADD3,CITY,STATE,PARTYACC,EMAIL,PINCODE,CSTNO,SSTNO,MOBILE,COUNTRYCODE,TRANSAMOUNT,CRLIMIT,PGROUP,NET,BSGST,BCGST,BIGST,BDISC,GROSS,STATUS)" +
                        " VALUES ('0','0','F','0','0' ,'" + cy.JopId + "','"+cy.JopDate+"','" + party.Rows[0]["PARTYNAME"].ToString() + "','" + cy.CusNo + "','"+ cy.Cusdate + "','" + cy.Cusdate + "','"+cy.Customer+ "','jo','NO','NONE','" + cy.Branch + "','" + cy.Currency + "','"+symbol+"','"+cy.ExRate+ "','" + cy.user + "','" + party.Rows[0]["RATECODE"].ToString() + "','STANDARD','"+cy.OrderType+"','"+cy.Emp+ "','STD','OUTRIGHT','"+cy.Location+ "','" + party.Rows[0]["ADD1"].ToString() + "','" + party.Rows[0]["ADD2"].ToString() + "','" + party.Rows[0]["ADD3"].ToString() + "','" + party.Rows[0]["STATE"].ToString() + "','" + party.Rows[0]["CITY"].ToString() + "','" + party.Rows[0]["ACCOUNTNAME"].ToString() + "','" + party.Rows[0]["EMAIL"].ToString() + "','" + party.Rows[0]["PINCODE"].ToString() + "','" + party.Rows[0]["CSTNO"].ToString() + "','" + party.Rows[0]["SSTNO"].ToString() + "','" + party.Rows[0]["MOBILE"].ToString() + "','" + party.Rows[0]["CONCODE"].ToString() + "','"+cy.TransAmount+"','"+cy.CreditLimit+ "','" + party.Rows[0]["PARTYGROUP"].ToString() + "','"+cy.Net+ "','" + cy.sgst + "','" + cy.cgst + "','" + cy.igst + "','" + cy.Discount + "','" + cy.Gross + "','Active') RETURNING JOBASICID INTO :OUTID";
                        OracleCommand objCmdso = new OracleCommand(SvSqlo, objConn);
                        objCmdso.Parameters.Add("OUTID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                        objCmdso.ExecuteNonQuery();
                         Pid = objCmdso.Parameters["OUTID"].Value.ToString();
                    }
                    //else
                    //{
                    //    StatementType = "Update";
                    //    objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;

                    //}



                    //objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
     //               objCmd.Parameters.Add("QUOID", OracleDbType.NVarchar2).Value = cy.Quo;
     //               objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.JopId;
     //               objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
					//objCmd.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.Location;
     //               objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Customer,
     //               objCmd.Parameters.Add("PARTYNAME", OracleDbType.NVarchar2).Value = party.Rows[0]["PARTYNAME"].ToString();

     //               objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.JopDate;
					//objCmd.Parameters.Add("CREFNO", OracleDbType.NVarchar2).Value = cy.CusNo;
     //               objCmd.Parameters.Add("CREFDATE", OracleDbType.NVarchar2).Value = cy.Cusdate;

     //               objCmd.Parameters.Add("MAINCURRENCY", OracleDbType.NVarchar2).Value = cy.Currency;
     //               objCmd.Parameters.Add("MAINCURRENCY", OracleDbType.NVarchar2).Value = symbol;
     //               objCmd.Parameters.Add("EXRATE", OracleDbType.NVarchar2).Value = cy.ExRate;

     //               objCmd.Parameters.Add("TRANSAMOUNT", OracleDbType.NVarchar2).Value = cy.TransAmount;
					//objCmd.Parameters.Add("CRLIMIT", OracleDbType.NVarchar2).Value = cy.CreditLimit;
					////objCmd.Parameters.Add("CONTACT_PERSON_MOBILE", OracleDbType.NVarchar2).Value = cy.SalesValue;
					//objCmd.Parameters.Add("ORDTYPE", OracleDbType.NVarchar2).Value = cy.OrderType;
					//objCmd.Parameters.Add("RATETYPE", OracleDbType.NVarchar2).Value = cy.RateType;
					//objCmd.Parameters.Add("RATECODE", OracleDbType.NVarchar2).Value = party.Rows[0]["RATECODE"].ToString();
					//objCmd.Parameters.Add("ADD1", OracleDbType.NVarchar2).Value = party.Rows[0]["ADD1"].ToString();
					//objCmd.Parameters.Add("ADD2", OracleDbType.NVarchar2).Value = party.Rows[0]["ADD2"].ToString();
					//objCmd.Parameters.Add("ADD3", OracleDbType.NVarchar2).Value = party.Rows[0]["ADD3"].ToString();
					//objCmd.Parameters.Add("PINCODE", OracleDbType.NVarchar2).Value = party.Rows[0]["PINCODE"].ToString();
					//objCmd.Parameters.Add("EMAIL", OracleDbType.NVarchar2).Value = party.Rows[0]["EMAIL"].ToString();
					//objCmd.Parameters.Add("CITY", OracleDbType.NVarchar2).Value = party.Rows[0]["CITY"].ToString();
					//objCmd.Parameters.Add("STATE", OracleDbType.NVarchar2).Value = party.Rows[0]["STATE"].ToString();
					//objCmd.Parameters.Add("PARTYACC", OracleDbType.NVarchar2).Value = party.Rows[0]["ACCOUNTNAME"].ToString();
					//objCmd.Parameters.Add("CSTNO", OracleDbType.NVarchar2).Value = party.Rows[0]["CSTNO"].ToString();
					//objCmd.Parameters.Add("SSTNO", OracleDbType.NVarchar2).Value = party.Rows[0]["SSTNO"].ToString();
					//objCmd.Parameters.Add("MOBILE", OracleDbType.NVarchar2).Value = party.Rows[0]["MOBILE"].ToString();
     //               objCmd.Parameters.Add("COUNTRYCODE", OracleDbType.NVarchar2).Value = party.Rows[0]["CONCODE"].ToString();
     //               objCmd.Parameters.Add("PGROUP", OracleDbType.NVarchar2).Value = party.Rows[0]["PARTYGROUP"].ToString();
     //                    objCmd.Parameters.Add("BSGST", OracleDbType.NVarchar2).Value = cy.sgst;
     //               objCmd.Parameters.Add("BCGST", OracleDbType.NVarchar2).Value = cy.cgst;
     //               objCmd.Parameters.Add("BIGST", OracleDbType.NVarchar2).Value = cy.igst;
     //               objCmd.Parameters.Add("BDISC", OracleDbType.NVarchar2).Value = cy.Discount;
     //               objCmd.Parameters.Add("GROSS", OracleDbType.NVarchar2).Value = cy.Gross;
     //               objCmd.Parameters.Add("NET", OracleDbType.NVarchar2).Value = cy.Net;
     //               objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
					//objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
					try
					{
                        
                       
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


                                    if (cp.Isvalid == "Y" && cp.itemid != "0")
                                    {
                                        svSQL = "Insert into JODETAIL (JOBASICID,QTY,MATSUPP,ITEMID,RATE,AMOUNT,UNIT,ITEMSPEC,PACKSPEC,DISCOUNT,FREIGHTAMT,QDISC,CDISC,IDISC,TDISC,ADISC,SDISC,FREIGHT,TAXTYPE,SGSTP,CGSTP,IGSTP,SGST,CGST,IGST) " +
                                            "VALUES ('" + Pid + "','" + cp.orderqty + "','OWN','" + cp.itemid + "','" + cp.rate + "','" + cp.amount + "','" + UnitId + "','" + cp.itemspec + "','" + cp.packind + "','" + cp.discount + "','" + cp.freightamt + "','" + cp.qtydis + "','" + cp.cashdis + "','" + cp.introdis + "','" + cp.tradedis + "','" + cp.additiondis + "','" + cp.spldis + "','" + cp.freight + "','" + cp.taxtype + "','" + cp.sgstp + "','" + cp.cstp + "','" + cp.igstp + "','" + cp.sgst + "','" + cp.cgst + "','" + cp.igst + "') RETURNING JOBASICID INTO :OUTID";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.Parameters.Add("OUTID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                        objCmds.ExecuteNonQuery();
                                        string did = objCmds.Parameters["OUTID"].Value.ToString();



                                        string[] sqty = cp.schqty.Split('/');
                                        string[] sdate = cp.schdate.Split('/');
                                        int r = 1;


                                        for (int i = 0; i < sqty.Length; i++)
                                        {
                                            string itemname = datatrans.GetDataString("Select ITEMID from ITEMMASTER where ITEMMASTERID='" + cp.itemid + "' ");

                                            string schno = cy.JopId + " - " + itemname + " - " + r;
                                            string ssqty = sqty[i];
                                            string scdate = sdate[i];



                                            svSQL = "Insert into JOSCHEDULE(JOBASICID,PARENTRECORDID,JOSCHEDULEROW,SCHNO,SCHQTY,SCHDATE,SCHSUPPQTY,PARENTROW,SCHITEMID,SCHPRECLQTY,USCHDATE) VALUES ('" + Pid + "','" + did + "','" + r + "','" + schno + "','" + ssqty + "','" + scdate + "','" + ssqty + "','" + r + "','" + cp.itemid + "','0','" + scdate + "')";
                                            objCmds = new OracleCommand(svSQL, objConn);
                                            objCmds.ExecuteNonQuery();

                                            r++;
                                        }
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
                                    if (cp.Isvalid == "Y" && cp.itemid != "0")
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
        public string DrumAllocationCRUD(WDrumAllocation cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("DRUMALLOPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "PURQUOPROC";*/

                    objCmd.CommandType = CommandType.StoredProcedure;

                    StatementType = "Insert";
                    objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                   
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DOCId;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.DocDate;
                    objCmd.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.Locid;
                    objCmd.Parameters.Add("JOPID", OracleDbType.NVarchar2).Value = cy.JOId;
                    objCmd.Parameters.Add("CUSTOMERID", OracleDbType.NVarchar2).Value = cy.CustomerId;
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                    Object Pid = objCmd.Parameters["OUTID"].Value;
                    //if (cy.ID != null)
                    //{
                    //    Pid = cy.ID;
                    //}
                    foreach (WorkItem cp in cy.Worklst)
                    {
                        foreach (Drumdetails ca in cp.drumlst)
                        {
                            if (ca.drumselect == true)
                            {
                                OracleCommand objCmds = new OracleCommand("DRUMALLODETPROC", objConn);
                                objCmds.CommandType = CommandType.StoredProcedure;
                                objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.itemid;
                                objCmds.Parameters.Add("JODRUMALLOCATIONBASICID", OracleDbType.NVarchar2).Value = Pid;
                                objCmds.Parameters.Add("PLSTOCKID", OracleDbType.NVarchar2).Value = ca.invid;
                                objCmds.Parameters.Add("JOPDETAILID", OracleDbType.NVarchar2).Value = cp.Jodetailid;
                                objCmds.Parameters.Add("DRUMNO", OracleDbType.NVarchar2).Value = ca.drumno;
                                objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = ca.qty; 
                                objCmds.Parameters.Add("LOTNO", OracleDbType.NVarchar2).Value =ca.lotno; 
                                objCmds.Parameters.Add("RATE", OracleDbType.NVarchar2).Value =ca.rate; 
                                objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                objCmds.ExecuteNonQuery();
                                string svql = "Update PLSTOCKVALUE SET  IS_LOCK='Y' WHERE PLSTOCKVALUEID='" + ca.invid + "'";
                                OracleCommand objCmdss = new OracleCommand(svql, objConn);
                                objCmdss.ExecuteNonQuery();
                            }
                           
                        }
                    }
                    string allocate = "Update JOBASIC SET  IS_ALLOCATE='Y' WHERE JOBASICID='" + cy.JOId + "'";
                    OracleCommand objCmdssa = new OracleCommand(allocate, objConn);
                    objCmdssa.ExecuteNonQuery();
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
        public DataTable GetWorkOrderByID(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select J.DOCID,to_char(J.DOCDATE,'dd-MON-yyyy')DOCDATE,J.PARTYNAME,LOCDETAILS.LOCID,BRANCHMAST.BRANCHID,JS.JOBASICID,J.STATUS,J.LOCID as LOCMASTERID,J.PARTYID as CUSTOMERID,J.JOBASICID,JS.SCHNO,to_char(JS.SCHDATE,'dd-MON-yyyy')SCHDATE from JOSCHEDULE JS, JOBASIC J left outer join LOCDETAILS on LOCDETAILS.LOCDETAILSID=J.LOCID  left outer join BRANCHMAST on BRANCHMAST.BRANCHMASTID=J.BRANCHID  WHERE J.JOBASICID =JS.JOBASICID AND  JS.JOSCHEDULEID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetWorkOrder(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT BRANCHID,DOCDATE,MAINCURRENCY,CREFNO,PARTYNAME,LOCID,ORDTYPE,TRANSAMOUNT,CRLIMIT,RATECODE,RATETYPE,DOCID,CREFDATE,EXRATE FROM JOBASIC Where JOBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetWorkOrderDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select JD.JOBASICID,JD.QTY,JS.SCHQTY,JD.ITEMID as item,ITEMMASTER.ITEMID,DCQTY,RATE,AMOUNT,UNITMAST.UNITID,ITEMSPEC,PACKSPEC,DISCOUNT,FREIGHTAMT,QDISC,CDISC,IDISC,TDISC,ADISC,SDISC,FREIGHT,TAXTYPE,JD.JODETAILID from JODETAIL JD left outer join ITEMMASTER ON ITEMMASTER.ITEMMASTERID=JD.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT,JOSCHEDULE JS  Where JD.JODETAILID =JS.PARENTRECORDID AND JOSCHEDULEID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        //public DataTable GetDrumDetails(string Itemid)
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "select lstockvalueid,LOTNO,DRUMNO,RATE,PLUSQTY from lstockvalue where STOCKTRANSTYPE='FG PACKED' AND ITEMID='" + Itemid + "' ";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        public DataTable GetDrumDetails(string Itemid, string locid)
        {
            string SvSql = string.Empty;
            SvSql = "select DRUMNO,SUM(PLUSQTY-MINUSQTY) QTY,lotno,rate,plstockvalueid from plstockvalue where ITEMID='" + Itemid + "' AND LOCID='" + locid + "' AND IS_LOCK IS NULL group by DRUMNO,lotno,rate,plstockvalueid having sum(Plusqty-Minusqty)>0 order by DRUMNO DESC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetLocation(string id)
        {
            string SvSql = string.Empty;
           // SvSql = " select locdetails.LOCID ,EMPLOYEELOCATION.LOCID loc from EMPLOYEELOCATION  left outer join locdetails on locdetails.locdetailsid=EMPLOYEELOCATION.LOCID where EMPID='" + id + "' ";
            SvSql = " select LOCID,LOCDETAILSID as loc  from LOCDETAILS";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select I.ITEMID,L.ITEMID as item from  PLOTMAST LT,ITEMMASTER I,PLSTOCKVALUE L    where LT.LOTNO=L.LOTNO AND I.ITEMMASTERID =L.ITEMID AND  L.LOCID= '" + id + "' and LT.INSFLAG='1' HAVING SUM(L.PLUSQTY-L.MINUSQTY) > 0 GROUP BY I.ITEMID,L.ITEMID";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetParty( )
        {
            string SvSql = string.Empty;
            SvSql = "select PARTYNAME,PARTYMASTID from PARTYMAST where TYPE IN('Customer','BOTH') ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public IEnumerable<WDrumAllocation> GetAllWDrumAll( )
        {
            

            List<WDrumAllocation> cmpList = new List<WDrumAllocation>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select jodrumallocationbasic.DOCID,to_char(jodrumallocationbasic.DOCDATE,'dd-MON-yyyy')DOCDATE,jobasic.DOCID as jobid,PARTYMAST.PARTYNAME ,LOCDETAILS.LOCID,JODRUMALLOCATIONBASICID from jodrumallocationbasic  left outer join LOCDETAILS on LOCDETAILS.LOCDETAILSID=jodrumallocationbasic.LOCID  LEFT OUTER JOIN  PARTYMAST on jodrumallocationbasic.CUSTOMERID=PARTYMAST.PARTYMASTID left outer join jobasic on jobasic.jobasicid= jodrumallocationbasic.JOPID order by jodrumallocationbasic.jodrumallocationbasicid DESC ";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        WDrumAllocation cmp = new WDrumAllocation
                        {
                            ID = rdr["JODRUMALLOCATIONBASICID"].ToString(),
                            JobId = rdr["jobid"].ToString(),
                            Customername = rdr["PARTYNAME"].ToString(),
                            DocDate = rdr["DOCDATE"].ToString(),
                            Location = rdr["LOCID"].ToString(),
                            DOCId = rdr["DOCID"].ToString(),
                           
                           
                          
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public DataTable GetDrumAllByID(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select jodrumallocationbasic.DOCID,to_char(jodrumallocationbasic.DOCDATE,'dd-MON-yyyy')DOCDATE,jobasic.DOCID as jobid,PARTYMAST.PARTYNAME ,LOCDETAILS.LOCID,JODRUMALLOCATIONBASICID from jodrumallocationbasic  left outer join LOCDETAILS on LOCDETAILS.LOCDETAILSID=jodrumallocationbasic.LOCID  LEFT OUTER JOIN  PARTYMAST on jodrumallocationbasic.CUSTOMERID=PARTYMAST.PARTYMASTID left outer join jobasic on jobasic.jobasicid= jodrumallocationbasic.JOPID WHERE  jodrumallocationbasic.jodrumallocationbasicid='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDrumAllDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMMASTER.ITEMID,JODRUMALLOCATIONDETAIL.JODRUMALLOCATIONDETAILID from JODRUMALLOCATIONDETAIL left outer join ITEMMASTER ON ITEMMASTER.ITEMMASTERID=JODRUMALLOCATIONDETAIL.ITEMID  Where jodrumallocationbasicid='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAllocationDrumDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select JODRUMALLOCATIONDETAIL.DRUMNO,JODRUMALLOCATIONDETAIL.RATE,LOTNO,JODRUMALLOCATIONDETAIL.QTY,JODRUMALLOCATIONDETAILID from JODRUMALLOCATIONDETAIL     Where jodrumallocationbasicid='" + id + "'  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string StatusDeleteMR(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE JOBASIC SET IS_ACTIVE ='N' WHERE JOBASICID='" + id + "'";
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
        public string StatusStockRelease(string id,string jid,string bid)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    objConnT.Open();
                    svSQL = "UPDATE PLSTOCKVALUE SET IS_LOCK ='' WHERE PLSTOCKVALUEID='" + id + "'";
                    OracleCommand objCmds = new OracleCommand(svSQL, objConnT);
                    
                    objCmds.ExecuteNonQuery();
                    svSQL = "UPDATE JOBASIC SET IS_ALLOCATE ='N' WHERE JOBASICID='" + jid + "'";
                    objCmds = new OracleCommand(svSQL, objConnT);

                    objCmds.ExecuteNonQuery();
                    svSQL = "UPDATE JODRUMALLOCATIONBASIC  SET IS_ALLOCATE ='N' WHERE JODRUMALLOCATIONBASICID='" + jid + "'";
                    objCmds = new OracleCommand(svSQL, objConnT);

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
        public DataTable GetAllListWorkOrderItems(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select JOBASIC.DOCID,to_char(JOBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PARTYMAST.PARTYNAME PARTY,LOCDETAILS.LOCID,BRANCHMAST.BRANCHID,JOBASICID,JOBASIC.STATUS,JOBASIC.IS_ALLOCATE from JOBASIC  left outer join LOCDETAILS on LOCDETAILS.LOCDETAILSID=JOBASIC.LOCID  left outer join BRANCHMAST on BRANCHMAST.BRANCHMASTID=JOBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on JOBASIC.PARTYID=PARTYMAST.PARTYMASTID WHERE JOBASIC.IS_ACTIVE='Y' AND JOBASIC.STATUS='Active' ORDER BY JOBASIC.JOBASICID DESC";
            }
            else
            {
                SvSql = "Select JOBASIC.DOCID,to_char(JOBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PARTYMAST.PARTYNAME PARTY,LOCDETAILS.LOCID,BRANCHMAST.BRANCHID,JOBASICID,JOBASIC.STATUS,JOBASIC.IS_ALLOCATE from JOBASIC  left outer join LOCDETAILS on LOCDETAILS.LOCDETAILSID=JOBASIC.LOCID  left outer join BRANCHMAST on BRANCHMAST.BRANCHMASTID=JOBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on JOBASIC.PARTYID=PARTYMAST.PARTYMASTID WHERE JOBASIC.IS_ACTIVE='N' AND JOBASIC.STATUS='Active' ORDER BY JOBASIC.JOBASICID DESC";
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAllListWDrumAllocationItems()
        {
            string SvSql = string.Empty;
            SvSql = "Select JOBASIC.DOCID,to_char(JOBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PARTYMAST.PARTYNAME PARTY,LOCDETAILS.LOCID,BRANCHMAST.BRANCHID,JOBASICID,JOBASIC.STATUS,JOBASIC.LOCID as LOCMASTERID,JOBASIC.PARTYID as CUSTOMERID,JOBASIC.JOBASICID from JOBASIC  left outer join LOCDETAILS on LOCDETAILS.LOCDETAILSID=JOBASIC.LOCID  left outer join BRANCHMAST on BRANCHMAST.BRANCHMASTID=JOBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on JOBASIC.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAllListWDrumAlloItems()
        {
            string SvSql = string.Empty;
            SvSql = "Select jodrumallocationbasic.DOCID,to_char(jodrumallocationbasic.DOCDATE,'dd-MON-yyyy')DOCDATE,jobasic.DOCID as jobid,PARTYMAST.PARTYNAME ,LOCDETAILS.LOCID,JODRUMALLOCATIONBASICID,JODRUMALLOCATIONBASIC.IS_ALLOCATE from jodrumallocationbasic  left outer join LOCDETAILS on LOCDETAILS.LOCDETAILSID=jodrumallocationbasic.LOCID  LEFT OUTER JOIN  PARTYMAST on jodrumallocationbasic.CUSTOMERID=PARTYMAST.PARTYMASTID left outer join jobasic on jobasic.jobasicid= jodrumallocationbasic.JOPID WHERE jodrumallocationbasic.IS_ALLOCATE ='Y' ORDER BY jodrumallocationbasicid DESC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAllListWorkScheduleItems()
        {
            string SvSql = string.Empty;
            SvSql = "Select JS.JOSCHEDULEID,JS.SCHNO,JS.SCHQTY,J.DOCID,J.PARTYNAME,to_char(JS.SCHDATE,'dd-MON-yyyy')SCHDATE,to_char(J.DOCDATE,'dd-MON-yyyy')DOCDATE,JD.QTY from JOSCHEDULE JS,JOBASIC J,JODETAIL JD    WHERE J.JOBASICID =JS.JOBASICID AND JS.PARENTRECORDID =JD.JODETAILID";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public async Task<IEnumerable<OrderItemDetail>> GetOrderItem(string id )
        {
            using (OracleConnection db = new OracleConnection(_connectionString))
            {
                return await db.QueryAsync<OrderItemDetail>(" SELECT JOBASICID , DOCID, to_char(JOBASIC.DOCDATE,'dd-MM-yyyy')DOCDATE, JOBASIC.PARTYNAME, JOBASIC.PARTYID, TRANSID, SENDSMS, ADOCID, ADOCDATE, ORDERNO, ORDERBASICID, REFNO, BRANCHID, TEMPID, MAINCURRENCY, SYMBOL, EXRATE, SALESREP, USERID,    ASSIGNTO, RECDBY, FOLLOWDT , FOLLOWUPTIME, PARENTACTIVITYID, ORGANISERID, PARENTJOBSID, ACTIVITYDONE, ORDTYPE, NARRATION, DESPTHROUGH, TEST, TEBY, TABY, ENTEREDBY, APPROVEDBY, APPROVEDYN, ODAMOUNT, OSAMOUNT, TRANSAMOUNT, CRLIMIT, BALAMOUNT, AITEMSPEC, RATETYPE, LOCID , JOBASIC.ADD1||''||JOBASIC.ADD2||''||JOBASIC.ADD3||''||JOBASIC.CITY||'-'||JOBASIC.PINCODE as ADDRESS , NET, LIMITQ, SEPDISC, BSGST, BCGST, BIGST, BDISC, GROSS,PARTYMAST.GSTNO,PARTYMAST.STATE FROM TAAIERP.JOBASIC INNER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID = JOBASIC.PARTYID where JOBASIC.JOBASICID='" + id + "'", commandType: CommandType.Text);
            }
        }
        public async Task<IEnumerable<OrderDetail>> GetOrderItemDetail(string id)
        {
            using (OracleConnection db = new OracleConnection(_connectionString))
            {
                return await db.QueryAsync<OrderDetail>(" SELECT JODETAILID, JODETAIL.JOBASICID, QTY, DUEDATE, MATSUPP, ITEMMASTER.ITEMID, DCQTY, RATE, EXCISEQTY, PRECLQTY, MRPQTY, MRPID, BLOCKQTY, PARTYCTRL, POQTY, PEQTY, REWORKQTY, REJQTY, AMOUNT, INVQTY, JODETAILROW, UNIT, ITEMSPEC, PACKSPEC, DISCOUNT, FREIGHTAMT, QDISC, CDISC, IDISC, TDISC, ADISC, SDISC, FREIGHT, BED, TAXTYPE, TOSUBGRID, SUBQTY, TCSCTRL, ORDQTY, ACCESSAMT, SPRATE, ORDCLBY, ORDCLON, ETARIFFID, SGSTP, CGSTP, IGSTP, SGST, CGST, IGST, HSNGP,JOSCHEDULE.SCHNO,to_char(JOSCHEDULE.SCHDATE,'dd-MM-yyyy')SCHDATE FROM  JODETAIL  LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=JODETAIL.ITEMID INNER JOIN JOSCHEDULE ON JOSCHEDULE.PARENTRECORDID = JODETAIL.JODETAILID where JODETAIL.JOBASICID='" + id + "' AND JOSCHEDULE.JOBASICID='" + id + "'", commandType: CommandType.Text);
            }
        }
    }
}
