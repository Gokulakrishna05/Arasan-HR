using Arasan.Interface;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Reflection.PortableExecutable;
namespace Arasan.Services
{
    public class ServicePOService : IServicePO
    {
        private readonly string _connectionString;
        DataTransactions datatrans;

        public ServicePOService(IConfiguration _configuratio)


        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetCurrency()
        {
            string SvSql = string.Empty;
            SvSql = "Select CURRENCYID,MAINCURR from CURRENCY  WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPartyID()
        {
            string SvSql = string.Empty;
            SvSql = "Select PARTYMASTID,PARTYID from PARTYMAST  WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPrepared()
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPMASTID,EMPNAME from EMPMAST  WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetTemp()
        {
            string SvSql = string.Empty;
            SvSql = "Select TESTTBASICID,TEMPLATEDESC from TESTTBASIC  WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetTAN()
        {
            string SvSql = string.Empty;
            SvSql = "Select TANDCDETAILID,TANDC from TANDCDETAIL  WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetUnit()
        {
            string SvSql = string.Empty;
            SvSql = "Select UNITMASTID,UNITID from UNITMAST  WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }


        public string GetInsService(ServicePOModel s)
        {
            string msg = "";
            string svSQL = "";
            string updateCMd = "";
            string Pid = "";
            try
            {
                if (s.ID == null)
                {
                    int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'SPO#' AND ACTIVESEQUENCE = 'T'");
                    string pono = string.Format("{0}{1}", "SPO#", (idc + 1).ToString());

                    updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='SPO#' AND ACTIVESEQUENCE ='T'";
                    datatrans.UpdateStatus(updateCMd);
                    s.PONo = pono;
                }
                using (OracleConnection objconn = new OracleConnection(_connectionString))
                {
                    objconn.Open();
                    //servicepo
                    if (s.ID == null)
                    {
                        svSQL = "Insert into SPOBASIC (DOCID,DOCDATE,MAINCURRENCY,EXRATE,REFNO,REFDT,PARTYID,TERMSTEMPLATE,DUEDATE,NET) values ('" + s.PONo + "','" + s.PODate + "','" + s.Curr + "','" + s.ExRate + "','" + s.RefNo + "','" + s.RefDate + "','" + s.PartyId + "','" + s.Term + "','" + s.DueDate + "','" + s.Net + "') RETURNING SPOBASICID  INTO: OUTID";
                    }

                    else
                    {
                        svSQL = " UPDATE SPOBASIC SET DOCID = '" + s.PONo + "', DOCDATE = '" + s.PODate + "', MAINCURRENCY = '" + s.Curr + "', EXRATE = '" + s.ExRate + "', REFNO = '" + s.RefNo + "', REFDT = '" + s.RefDate + "', PARTYID = '" + s.PartyId + "', TERMSTEMPLATE = '" + s.Term + "', DUEDATE = '" + s.DueDate + "',NET  = '" + s.Net + "' Where SPOBASICID = '" + s.ID + "'";

                    }

                    OracleCommand oracleCommand = new OracleCommand(svSQL, objconn);
                    oracleCommand.Parameters.Add("OUTID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                    oracleCommand.ExecuteNonQuery();
                    if (s.ID == null)
                    {
                        Pid = oracleCommand.Parameters["OUTID"].Value.ToString();
                    }

                    //dairy
                    if (s.ID == null)
                    {
                        svSQL = "Insert into SPOORGANISER (SPOBASICID,POSENTBY,ASSIGNTO,FOLLOWUPDT,REMARKS) values ('" + Pid + "','" + s.Send + "','" + s.Receive + "','" + s.FollowupDate + "','" + s.FollowupDetails + "') ";
                          oracleCommand = new OracleCommand(svSQL, objconn);
                        oracleCommand.ExecuteNonQuery();

                    }

                    else
                    {
                        svSQL = " UPDATE SPOORGANISER SET POSENTBY = '" + s.Send + "', ASSIGNTO = '" + s.Receive + "' ,FOLLOWUPDT = '" + s.FollowupDate + "',REMARKS = '" + s.FollowupDetails + "'  Where SPOBASICID = '" + s.ID + "'";
                        oracleCommand = new OracleCommand(svSQL, objconn);
                        oracleCommand.ExecuteNonQuery();
                    }

                    //servicepo table
                    if (s.ServicePOlist != null)
                    {
                        if (s.ID == null)
                        {
                            int r = 1;
                            foreach (ServicePO cp in s.ServicePOlist)
                            {
                                if (cp.Isvalid == "Y" && cp.ServiceID != "")
                                {


                                    svSQL = "Insert into SPODETAIL (SPOBASICID,SERVICEID,SERVICEDESC,UNIT,QTY,RATE,AMOUNT) VALUES ('" + Pid + "','10047000049320','" + cp.ServiceDesc + "','" + cp.Unit + "','" + cp.Qty + "','" + cp.Rate + "','" + cp.Amount + "' )";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objconn);
                                    objCmds.ExecuteNonQuery();

                                }

                                r++;
                            }


                        }
                        else
                        {
                            svSQL = "Delete SPODETAIL WHERE SPOBASICID='" + s.ID + "'";
                            OracleCommand objCmdd = new OracleCommand(svSQL, objconn);
                            objCmdd.ExecuteNonQuery();
                            foreach (ServicePO cp in s.ServicePOlist)
                            {
                                int r = 1;
                                if (cp.Isvalid == "Y" && cp.ServiceID != "")
                                {
                                    svSQL = "Insert into SPODETAIL (SPOBASICID,SERVICEID,SERVICEDESC,UNIT,QTY,RATE,AMOUNT) VALUES ('" + s.ID + "','10047000049320','" + cp.ServiceDesc + "','" + cp.Unit + "','" + cp.Qty + "','" + cp.Rate + "','" + cp.Amount + "' )";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objconn);
                                    objCmds.ExecuteNonQuery();

                                }
                                r++;
                            }
                        }

                    }

                    ////additional table
                    //if (s.ServicePOAdditionallist != null)
                    //{
                    //    if (s.ID == null)
                    //    {
                    //        int r = 1;
                    //        foreach (ServicePOAdditional cp in s.ServicePOAdditionallist)
                    //        {
                    //            if (cp.Isvalid == "Y" && cp.AddDedec != "")
                    //            {


                    //                svSQL = "Insert into SPOADDDED (SPOBASICID,ADNAME,ADVALUE) VALUES ('" + Pid + "','','" + cp.Amounts + "'  )";
                    //                OracleCommand objCmds = new OracleCommand(svSQL, objconn);
                    //                objCmds.ExecuteNonQuery();

                    //            }

                    //            r++;
                    //        }

                    //    }
                    //    else
                    //    {
                    //        svSQL = "Delete SPOADDDED WHERE SPOBASICID='" + s.ID + "'";
                    //        OracleCommand objCmdd = new OracleCommand(svSQL, objconn);
                    //        objCmdd.ExecuteNonQuery();
                    //        foreach (ServicePOAdditional cp in s.ServicePOAdditionallist)
                    //        {
                    //            int r = 1;
                    //            if (cp.Isvalid == "Y" && cp.AddDedec != "")
                    //            {
                    //                svSQL = "Insert into SPOADDDED (SPOBASICID,ADNAME,ADVALUE) VALUES ('" + s.ID + "','','" + cp.Amounts + "'  )";
                    //                OracleCommand objCmds = new OracleCommand(svSQL, objconn);
                    //                objCmds.ExecuteNonQuery();

                    //            }
                    //            r++;
                    //        }
                    //    }

                    //}
                    //terms and condition table
                    if (s.TermsAndConditionlist != null)
                    {
                        if (s.ID == null)
                        {
                            int r = 1;
                            foreach (TermsAndCondition cp in s.TermsAndConditionlist)
                            {
                                if (cp.Isvalid == "Y" && cp.TAC != "")
                                {


                                    svSQL = "Insert into SPOTANDC (SPOBASICID,TERMSANDCOND ) VALUES ('" + Pid + "', '" + cp.TAC + "'  )";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objconn);
                                    objCmds.ExecuteNonQuery();

                                }

                                r++;
                            }

                        }
                        else
                        {
                            svSQL = "Delete SPOTANDC WHERE SPOBASICID='" + s.ID + "'";
                            OracleCommand objCmdd = new OracleCommand(svSQL, objconn);
                            objCmdd.ExecuteNonQuery();
                            foreach (TermsAndCondition cp in s.TermsAndConditionlist)
                            {
                                int r = 1;
                                if (cp.Isvalid == "Y" && cp.TAC != "")
                                {
                                    svSQL = "Insert into SPOTANDC (SPOBASICID,TERMSANDCOND ) VALUES ('" + s.ID + "', '" + cp.TAC + "'  )";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objconn);
                                    objCmds.ExecuteNonQuery();

                                }
                                r++;
                            }
                        }

                    }




                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw ex;
            }
            return msg;
        }



        public DataTable GetAllServicePo(string status)
        {
            string SvSql = string.Empty;
            if (status == "Y" || status == null)
            {
                SvSql = "Select SPOBASICID,SPOBASIC.DOCID,SPOBASIC.DOCDATE,MAINCURRENCY,EXRATE,REFNO,REFDT,PARTYID,TESTTBASIC.TEMPLATEDESC,to_char(DUEDATE,'dd-MON-yyyy')DUEDATE,NET,SPOBASIC.IS_ACTIVE from SPOBASIC  left outer join TESTTBASIC ON TESTTBASICID=SPOBASIC.TERMSTEMPLATE  WHERE SPOBASIC.IS_ACTIVE='Y' ORDER BY SPOBASIC.SPOBASICID DESC ";

            }
            else
            {
                SvSql = "Select SPOBASICID,SPOBASIC.DOCID,SPOBASIC.DOCDATE,MAINCURRENCY,EXRATE,REFNO,REFDT,PARTYID,TESTTBASIC.TEMPLATEDESC,to_char(DUEDATE,'dd-MON-yyyy')DUEDATE,NET,SPOBASIC.IS_ACTIVE from SPOBASIC  left outer join TESTTBASIC ON TESTTBASICID=SPOBASIC.TERMSTEMPLATE  WHERE SPOBASIC.IS_ACTIVE='Y' ORDER BY SPOBASIC.SPOBASICID DESC ";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string StatusChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {

                    if (tag == "Del")
                    {
                        svSQL = "UPDATE SPOBASIC SET IS_ACTIVE ='N' WHERE SPOBASICID='" + id + "'";
                    }
                    else
                    {
                        svSQL = "UPDATE SPOBASIC SET IS_ACTIVE ='Y' WHERE SPOBASICID='" + id + "'";
                    }
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
        public DataTable GetSPOBasicEdit(string id)
        {
            string SvSql = string.Empty;

            SvSql = "Select S.SPOBASICID,S.DOCID,to_char(S.DOCDATE,'dd-MON-yyyy')DOCDATE,S.MAINCURRENCY,S.EXRATE,S.REFNO,to_char(S.REFDT,'dd-MON-yyyy')REFDT,P.PARTYID,S.TERMSTEMPLATE,to_char(S.DUEDATE,'dd-MON-yyyy')DUEDATE,S.NET,P.PARTYNAME,P.ADD1,P.ADD2,P.ADD3,P.EMAIL,P.MOBILE,C.SYMBOL from SPOBASIC S,PARTYMAST P,CURRENCY C WHERE S.PARTYID=P.PARTYMASTID AND S.MAINCURRENCY=C.CURRENCYID AND SPOBASICID='" + id + "'";


            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSPOOrganizerEdit(string id)
        {
            string SvSql = string.Empty;

            SvSql = "Select SPOBASICID,POSENTBY,ASSIGNTO,to_char(FOLLOWUPDT,'dd-MON-yyyy')FOLLOWUPDT,REMARKS from SPOORGANISER WHERE SPOBASICID='" + id + "'";


            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSPODetailEdit(string id)
        {
            string SvSql = string.Empty;

            SvSql = "Select SPOBASICID,SERVICEID,SERVICEDESC,UNIT,QTY,RATE,AMOUNT from  SPODETAIL WHERE SPOBASICID='" + id + "'";


            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSPOTandcEdit(string id)
        {
            string SvSql = string.Empty;

            SvSql = "Select SPOBASICID,TERMSANDCOND  from  SPOTANDC WHERE SPOBASICID='" + id + "'";


            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
