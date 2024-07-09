using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using static Nest.JoinField;

namespace Arasan.Services
{
    public class AccTreeviewService : IAccTreeView
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public AccTreeviewService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAccClass()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ACCOUNT_CLASS,ACCCLASSID FROM ACCCLASS WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetParent()
        {
            string SvSql = string.Empty;
            SvSql = "select MASTERID,MNAME,MPARENT from master where MPARENT=0";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable Getchild(string parentid)
        {
            string SvSql = string.Empty;
            SvSql = "select MASTERID,MNAME,MPARENT from master where MPARENT='"+ parentid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAccType(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ACCOUNTTYPEID,ACCOUNTTYPE from ACCTYPE where IS_ACTIVE='Y' AND ACCCLASSID='"+ id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAccGroup(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ACCGROUPID,ACCOUNTGROUP from ACCGROUP where IS_ACTIVE='Y' AND ACCOUNTTYPE='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAccLedger(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select LEDGERID,LEDNAME from ACCLEDGER Where IS_ACTIVE='Y' AND ACCGROUP='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string NodeCreation(Accounttree cy)
        {
          
                string msg = "";
                try
                {
                    string StatementType = string.Empty; string svSQL = "";
                   

                        svSQL = " SELECT Count(*) as cnt FROM Master WHERE MNAME =LTRIM(RTRIM('" + cy.accname + "')) ";
                        if (datatrans.GetDataId(svSQL) > 0)
                        {
                            msg = "Account Or Group Name Already Existed";
                            return msg;
                        }
                string mcate=string.Empty;
                string malie=string.Empty;
                if (cy.cate == "Billwise adjustment - Receivable")
                {
                    mcate = "r";
                }
                else if (cy.cate == "Billwise Adjustment - Payable")
                {
                    mcate = "p";
                }
                else if (cy.cate == "Bank book required")
                {
                    mcate = "b";
                }
                else
                {
                    mcate = "x";
                }
                DateTime fromdate = DateTime.Now;
                int year = fromdate.Year;
                int month = fromdate.Month;

                string mname = year + "" + month.ToString().PadLeft(2, '0');
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                        try
                        {
                            objConn.Open();

                           DataTable dtt = new DataTable();
                           dtt=datatrans.GetData("select * from Master where MASTERID='"+ cy.id +"'");
                        if (dtt.Rows[0]["MPARENT1"].ToString() == "1")
                        {
                            malie = "a";
                        }
                        if (dtt.Rows[0]["MPARENT1"].ToString() == "51")
                        {
                            malie = "l";
                        }
                        if (dtt.Rows[0]["MPARENT1"].ToString() == "151")
                        {
                            malie = "e";
                        }
                        if (dtt.Rows[0]["MPARENT1"].ToString() == "101")
                        {
                            malie = "i";
                        }
                        int mlevel = Convert.ToInt32(dtt.Rows[0]["MLEVEL"].ToString() == "" ? 0 : dtt.Rows[0]["MLEVEL"].ToString()) + 1;
                        string mns = "";
                        for(int i=1;i<=mlevel; i++)
                        {
                            mns += "1";
                        }
                        int mparent = mlevel - 1;
                        string svalue = string.Empty;
                        string sres=string.Empty;
                        for(int i=1;i<mparent; i++)
                        {
                            svalue = string.Format("{0},MPARENT{1}", svalue, i);
                            string sres2 = string.Format("MPARENT{0}", i);
                            string value = dtt.Rows[0][sres2].ToString();
                            sres = string.Format("{0},{1}", sres, value);
                        }
                       
                        for(int i=mparent; i<=15; i++)
                        {
                            svalue = string.Format("{0},MPARENT{1}", svalue, i);
                            if(i==mparent)
                            {
                                sres = string.Format("{0},{1}", sres, cy.id);
                            }
                            else
                            {
                                sres = string.Format("{0},{1}", sres, "0");
                            }
                            
                        }

                        svSQL = "Insert into Master (MPARENT,MNAME,DISPNAME,GROUPORACCOUNT,DISPLAYINREPORTS,GROUPELEMENT,CATEGORY,MCATEGORY,MSTATUS,NATIVECURRENCY,SLN,MNOPBAL,MB1DEBIT,MNDEBIT,MNCREDIT,MB1CREDIT,LEDSTATUS,MB2DEBIT,OPRP,MALIE,DOCDT,EMODE,MONTHNO,RPVALID,CANMOVE,CANDELETE,CANADD,MLEVEL,mnsibblestr,MNOOFCHILD " + svalue + ",LEDSTATUSD,MALIED,VBID,VPID,EXRATE,EXCISEACC,SERTAXACC,TDSACC,FBTACC,FBTPER,BTOTAL,BSTATEMENTYN1,RPOPBAL,RPOPBALCHK,ISCONS,MLCHILD) VALUES ";
                        svSQL +="('" + cy.id + "','" + cy.accname + "','" + cy.accname + "','" + cy.group + "','T','f','" + cy.cate + "','" + mcate + "','a','1','1','0','0','0','0','0','i','0','f','" + malie + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','N','" + mname + "','t','T','T','T','" + mlevel + "','" + mns + "','0' " + sres + ",'i','"+ malie + "',0,0,1,'No','No','No','No',0,0,'No',0,0,'No','0')";
                        OracleCommand objCmd = new OracleCommand(svSQL, objConn);
                        objCmd.ExecuteNonQuery();
                    }
                        catch (Exception ex)
                        {
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
        public string NodeDelete(string id)
        {
            string msg = "";
            try
            {
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    string svSQL = "";
                    OracleCommand objCmd = new OracleCommand(svSQL, objConn);
                    objCmd.ExecuteNonQuery();
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
