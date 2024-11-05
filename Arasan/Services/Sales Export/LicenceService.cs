using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services
{
    public class LicenceService : ILicence
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public LicenceService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public DataTable GetItem()
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER WHERE IGROUP = 'FINISHED'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetExportItem()
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER WHERE IGROUP = 'FINISHED'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSupplier()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PARTYMASTID,PARTYNAME FROM PARTYMAST WHERE PARTYMAST.TYPE IN ('Customer','BOTH')";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "select UNITMAST.UNITID,ITEMID,ITEMDESC,LATPURPRICE,UNITMAST.UNITMASTID from ITEMMASTER LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID LEFT OUTER JOIN ITEMMASTERPUNIT ON  ITEMMASTER.ITEMMASTERID=ITEMMASTERPUNIT.ITEMMASTERID Where ITEMMASTER.ITEMMASTERID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetExportDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "select UNITMAST.UNITID,ITEMID,ITEMDESC,LATPURPRICE,UNITMAST.UNITMASTID from ITEMMASTER LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID LEFT OUTER JOIN ITEMMASTERPUNIT ON  ITEMMASTER.ITEMMASTERID=ITEMMASTERPUNIT.ITEMMASTERID Where ITEMMASTER.ITEMMASTERID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAllListLicenceItems(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,LICENCENO,to_char(LICENCEDATE,'dd-MON-yyyy')LICENCEDATE,LICCLBASIC.STATUS,LICCLBASICID FROM LICCLBASIC WHERE LICCLBASIC.STATUS='Y' ORDER BY  LICCLBASIC.LICCLBASICID DESC";
            }
            else
            {
                SvSql = "SELECT DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,LICENCENO,to_char(LICENCEDATE,'dd-MON-yyyy')LICENCEDATE,LICCLBASIC.STATUS,LICCLBASICID FROM LICCLBASIC WHERE LICCLBASIC.STATUS='N' ORDER BY  LICCLBASIC.LICCLBASICID DESC";
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
                    svSQL = "UPDATE LICCLBASIC SET LICCLBASIC.STATUS ='N' WHERE LICCLBASICID='" + id + "'";
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
        public string ActStatusChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE LICCLBASIC SET LICCLBASIC.STATUS ='Y' WHERE LICCLBASICID='" + id + "'";
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
        public DataTable GetData(string sql)
        {
            DataTable _Dt = new DataTable();
            try
            {
                OracleDataAdapter adapter = new OracleDataAdapter(sql, _connectionString);
                OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
                adapter.Fill(_Dt);
            }
            catch (Exception ex)
            {

            }
            return _Dt;
        }
        public int GetDataId(String sql)
        {
            DataTable _dt = new DataTable();
            int Id = 0;
            try
            {
                _dt = GetData(sql);
                if (_dt.Rows.Count > 0)
                {
                    Id = Convert.ToInt32(_dt.Rows[0][0].ToString() == string.Empty ? "0" : _dt.Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Id;
        }

        public bool UpdateStatus(string query)
        {
            bool Saved = true;
            try
            {
                OracleConnection objConn = new OracleConnection(_connectionString);
                OracleCommand objCmd = new OracleCommand(query, objConn);
                objCmd.Connection.Open();
                objCmd.ExecuteNonQuery();
                objCmd.Connection.Close();
            }
            catch (Exception ex)
            {

                Saved = false;
            }
            return Saved;
        }
        public DataTable GetEditLicence(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,LICENCENO,to_char(LICENCEDATE,'dd-MON-yyyy')LICENCEDATE,NARRATION,LICCLBASICID FROM LICCLBASIC  WHERE LICCLBASIC.LICCLBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetLicenceImport(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT IBILLNO,to_char(IBILLDT,'dd-MON-yyyy')IBILLDT,IITEMDESC,UNITMAST.UNITID,IQTY,IAMOUNT,LICCLIMPDETAILID,LICCLBASICID FROM LICCLIMPDETAIL LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=LICCLIMPDETAIL.IUNIT  where LICCLIMPDETAIL.LICCLBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetLicenceImportView(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT IBILLNO,to_char(IBILLDT,'dd-MON-yyyy')IBILLDT,ITEMMASTER.ITEMID,UNITMAST.UNITID,IQTY,IAMOUNT,LICCLIMPDETAILID,LICCLBASICID FROM LICCLIMPDETAIL LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=LICCLIMPDETAIL.IUNIT LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=LICCLIMPDETAIL.IITEMDESC   where LICCLIMPDETAIL.LICCLBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetLicenceExport(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT LICCLBASICID,EINVNO,to_char(EINVDT,'dd-MON-yyyy')EINVDT,EPARTYID,EITEMDESC,UNITMAST.UNITID,EQTY,EAMOUNT,LICCLBASICID FROM LICCLEXPDETAIL LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=LICCLEXPDETAIL.EUNIT  where LICCLEXPDETAIL.LICCLBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetLicenceExportView(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT LICCLBASICID,EINVNO,to_char(EINVDT,'dd-MON-yyyy')EINVDT,ITEMMASTER.ITEMID,PARTYMAST.PARTYID,UNITMAST.UNITID,EQTY,EAMOUNT,LICCLBASICID FROM LICCLEXPDETAIL LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=LICCLEXPDETAIL.EUNIT LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=LICCLEXPDETAIL.EITEMDESC LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID=LICCLEXPDETAIL.EPARTYID where LICCLEXPDETAIL.LICCLBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string LicenceCRUD(Licence cy)
        {
            string msg = "";
            try
            {

                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);

                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE TRANSTYPE = 'Ex-F' AND ACTIVESEQUENCE = 'T'");
                string DocId = string.Format("{0}{1}", "Ex-F", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE TRANSTYPE = 'Ex-F' AND ACTIVESEQUENCE ='T'";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                cy.DocId = DocId;

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("LICCLBASICPROC", objConn);
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
                    
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.DocDate;
                    objCmd.Parameters.Add("LICENCENO", OracleDbType.NVarchar2).Value = cy.LicenceNo;
                    objCmd.Parameters.Add("LICENCEDATE", OracleDbType.NVarchar2).Value = cy.LicenceDate;
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cy.Narration;
                    objCmd.Parameters.Add("CREATED_BY", OracleDbType.Date).Value = DateTime.Now;
                    objCmd.Parameters.Add("CREATED_ON", OracleDbType.NVarchar2).Value = cy.CreatedOn;
                    objCmd.Parameters.Add("UPDATED_BY", OracleDbType.Date).Value = DateTime.Now;
                    objCmd.Parameters.Add("UPDATED_ON", OracleDbType.NVarchar2).Value = cy.UpdatedOn;
                    objCmd.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = "Y";
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
                        if (cy.ImportLst != null)
                        {
                            if (cy.ID == null)
                            {
                                int r = 1;
                                foreach (ImportDeatils cp in cy.ImportLst)
                                {
                                    string unit = datatrans.GetDataString("Select UNITMASTID from UNITMAST where UNITID='" + cp.Unit + "' ");

                                    if (cp.ItemId != "0")
                                    {
                                        svSQL = "Insert into LICCLIMPDETAIL (LICCLBASICID,IBILLNO,IBILLDT,IITEMDESC,IUNIT,IQTY,IAMOUNT) VALUES ('" + Pid + "','" + cp.InvNo + "','" + cp.InvDate + "','" + cp.ItemId + "','" + unit + "','" + cp.Qty + "','" + cp.Amount + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }

                                    r++;
                                }
                            }
                            else
                            {
                                svSQL = "Delete LICCLIMPDETAIL WHERE LICCLBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (ImportDeatils cp in cy.ImportLst)
                                {
                                    string unit = datatrans.GetDataString("Select UNITMASTID from UNITMAST where UNITID='" + cp.Unit + "' ");

                                    if (cp.Isvalid == "Y" && cp.ItemId != "0")
                                    {
                                        svSQL = "Insert into LICCLIMPDETAIL (LICCLBASICID,IBILLNO,IBILLDT,IITEMDESC,IUNIT,IQTY,IAMOUNT) VALUES ('" + Pid + "','" + cp.InvNo + "','" + cp.InvDate + "','" + cp.ItemId + "','" + unit + "','" + cp.Qty + "','" + cp.Amount + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }


                                }
                            }
                        }
                        if (cy.ExportLst != null)
                        {
                            if (cy.ID == null)
                            {
                                int r = 1;
                                foreach (ExportDeatils cp in cy.ExportLst)
                                {
                                    string unit1 = datatrans.GetDataString("Select UNITMASTID from UNITMAST where UNITID='" + cp.Unit + "' ");

                                    if (cp.ItemId != "0")
                                    {
                                        svSQL = "Insert into LICCLEXPDETAIL (LICCLBASICID,EINVNO,EINVDT,EPARTYID,EITEMDESC,EUNIT,EQTY,EAMOUNT) VALUES ('" + Pid + "','" + cp.ExpNo + "','" + cp.ExpDate + "','" + cp.Customer + "','" + cp.ItemId + "','" + unit1 + "','" + cp.Qty + "','" + cp.Amount + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }

                                    r++;
                                }
                            }
                            else
                            {
                                svSQL = "Delete LICCLEXPDETAIL WHERE LICCLBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (ExportDeatils cp in cy.ExportLst)
                                {
                                    string unit1 = datatrans.GetDataString("Select UNITMASTID from UNITMAST where UNITID='" + cp.Unit + "' ");

                                    if (cp.Isvalid1 == "Y" && cp.ItemId != "0")
                                    {
                                        svSQL = "Insert into LICCLEXPDETAIL (LICCLBASICID,EINVNO,EINVDT,EPARTYID,EITEMDESC,EUNIT,EQTY,EAMOUNT) VALUES ('" + Pid + "','" + cp.ExpNo + "','" + cp.ExpDate + "','" + cp.Customer + "','" + cp.ItemId + "','" + unit1 + "','" + cp.Qty + "','" + cp.Amount + "')";
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
                    //objConn.Close();
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
