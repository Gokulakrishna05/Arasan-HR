using Arasan.Interface;
using Arasan.Models;
using System.Data;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
namespace Arasan.Services 
{
    public class CuringOutwardService :ICuringOutward
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public CuringOutwardService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IEnumerable<CuringOutward> GetAllCuringOutward()
        {
            List<CuringOutward> cmpList = new List<CuringOutward>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select BRANCHMAST.BRANCHID,ITEMMASTER.ITEMID,DOCID,to_char(CUROPBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,SHIFTMAST.SHIFTNO,CUROPBASICID,CUROPBASIC.STATUS from CUROPBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=CUROPBASIC.BRANCHID left outer join SHIFTMAST on SHIFTMASTID=CUROPBASIC.SHIFT left outer join ITEMMASTER on ITEMMASTERID =CUROPBASIC.ITEM WHERE CUROPBASIC.STATUS='ACTIVE'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        CuringOutward cmp = new CuringOutward
                        {
                            ID = rdr["CUROPBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            ItemId = rdr["ITEMID"].ToString(),
                            Docdate = rdr["DOCDATE"].ToString(),
                            DocId = rdr["DOCID"].ToString(),
                            Shift = rdr["SHIFTNO"].ToString(),
                          

                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public string CuringOutwardCRUD(CuringOutward cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("CURINGOUTWARDPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "PURRETURNPROC";*/

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
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                    objCmd.Parameters.Add("ITEM", OracleDbType.NVarchar2).Value = cy.ItemId;
                    objCmd.Parameters.Add("PACKNOTE", OracleDbType.NVarchar2).Value = cy.PackingNote;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value =cy.Docdate;
                    objCmd.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = cy.FromWork;
                    objCmd.Parameters.Add("TOWCID", OracleDbType.NVarchar2).Value = cy.ToWork;
                    objCmd.Parameters.Add("ENTDATE", OracleDbType.NVarchar2).Value = cy.enddate;
                    objCmd.Parameters.Add("SHIFT", OracleDbType.NVarchar2).Value = cy.Shift;
                    objCmd.Parameters.Add("ENTEREDBY", OracleDbType.NVarchar2).Value = cy.Enterd;
                    objCmd.Parameters.Add("TOTQTY", OracleDbType.NVarchar2).Value = cy.TotalQty;
                    objCmd.Parameters.Add("TOTVALUE", OracleDbType.NVarchar2).Value = cy.TotalValue;
                    objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Remark;

                    objCmd.Parameters.Add("FRATE", OracleDbType.NVarchar2).Value = cy.FRate;
                    objCmd.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = "ACTIVE";

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
                        foreach (CuringDetail cp in cy.Curinglst)
                        {


                            using (OracleConnection objConns = new OracleConnection(_connectionString))
                            {
                                OracleCommand objCmds = new OracleCommand("CURINGOUTWARDDETPROC", objConns);
                                if (cy.ID == null)
                                {
                                    StatementType = "Insert";
                                    objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;

                                }
                                else
                                {
                                    StatementType = "Update";
                                    objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;

                                }
                                objCmds.CommandType = CommandType.StoredProcedure;
                                objCmds.Parameters.Add("CUROPBASICID", OracleDbType.NVarchar2).Value = Pid;
                                objCmds.Parameters.Add("DRUMNO", OracleDbType.NVarchar2).Value = cp.drum;
                                objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cy.ItemId;
                                objCmds.Parameters.Add("BATCHNO", OracleDbType.NVarchar2).Value = cp.batch;
                                objCmds.Parameters.Add("BATCHQTY", OracleDbType.NVarchar2).Value = cp.qty;
                                objCmds.Parameters.Add("COMBNO", OracleDbType.NVarchar2).Value = cp.comp;
                                //objCmds.Parameters.Add("TOTAMT", OracleDbType.NVarchar2).Value = cp.totalamount;
                                //objCmds.Parameters.Add("CF", OracleDbType.NVarchar2).Value = cp.confac;
                                //objCmds.Parameters.Add("CGSTPER", OracleDbType.NVarchar2).Value = cp.cgstper;
                                //objCmds.Parameters.Add("CGSTAMT", OracleDbType.NVarchar2).Value = cp.cgstamt;
                                //objCmds.Parameters.Add("SGSTPER", OracleDbType.NVarchar2).Value = cp.sgstper;
                                //objCmds.Parameters.Add("SGSTAMT", OracleDbType.NVarchar2).Value = cp.sgstamt;
                                //objCmds.Parameters.Add("IGSTPER", OracleDbType.NVarchar2).Value = cp.igstper;
                                //objCmds.Parameters.Add("IGSTAMT", OracleDbType.NVarchar2).Value = cp.igstamt;
                                objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;

                                objConns.Open();
                                objCmds.ExecuteNonQuery();

                                objConns.Close();

                            }
                           
                            string drumid = datatrans.GetDataString("select DRUMMASTID from DRUMMAST where DRUMNO='" + cp.drum + "'  ");
                            using (OracleConnection objConns = new OracleConnection(_connectionString))
                            {
                                OracleCommand objCmds = new OracleCommand("QCNOTIFICATIONPROC", objConns);
                                /*objCmds.Connection = objConns;
                                objCmds.CommandText = "QCNOTIFICATIONPROC";*/

                                objCmds.CommandType = CommandType.StoredProcedure;
                                if (cy.ID == null)
                                {
                                    StatementType = "Insert";
                                    objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                }
                                else
                                {
                                    StatementType = "Update";
                                    objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;

                                }
                                objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                objCmds.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                                objCmds.Parameters.Add("TYPE", OracleDbType.NVarchar2).Value = "Curing Outward";
                                objCmds.Parameters.Add("DRUMNO", OracleDbType.NVarchar2).Value = drumid;
                                objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cy.ItemId;
                                objCmds.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                objCmds.Parameters.Add("IS_COMPLETED", OracleDbType.NVarchar2).Value = "NO";
                                objCmds.Parameters.Add("QC_STATUS", OracleDbType.NVarchar2).Value = "Raised";

                                objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;

                                objConns.Open();

                                objCmds.ExecuteNonQuery();

                                objConns.Close();
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
         public DataTable GetWorkCenter()
         {
            string SvSql = string.Empty;
            SvSql = "Select WCID,WCBASICID from WCBASIC ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
         }
        public DataTable GetPackingNote()
        {
            string SvSql = string.Empty;
            SvSql = "Select DOCID,PACKNOTEBASICID from PACKNOTEBASIC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetWorkCenterID()
        {
            string SvSql = string.Empty;
            SvSql = "Select WCID,WCBASICID from WCBASIC where WCID IN ('RVD SHED','CURING') ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable ShiftDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select PACKNOTEBASICID,SHIFTMAST.SHIFTNO ,SHIFT from PACKNOTEBASIC left outer join SHIFTMAST on SHIFTMASTID=PACKNOTEBASIC.SHIFT where PACKNOTEBASICID='" + id +"' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDrumLocation()
        {
            string SvSql = string.Empty;
            SvSql = "select LOCDETAILS.LOCID,NPRODBASICID,TOLOCATION,NPRODOUTDETID from NPRODOUTDET left outer join LOCDETAILS on LOCDETAILSID = NPRODOUTDET.TOLOCATION ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDrumNo(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select DRUMMAST.DRUMNO,ODRUMNO,NPRODBASICID from NPRODOUTDET left outer join DRUMMAST on DRUMMASTID=NPRODOUTDET.ODRUMNO  where TOLOCATION= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetBatch(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select  ODRUMNO,NBATCHNO, NPRODBASICID from NPRODOUTDET    where ODRUMNO= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItembyId(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMMASTER.ITEMID,OITEMID,PACKNOTEBASICID from PACKNOTEBASIC left outer join ITEMMASTER on ITEMMASTERID =PACKNOTEBASIC.OITEMID  where PACKNOTEBASICID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPackingDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = " select DRUMMAST.DRUMNO,IBATCHNO,IBATCHQTY,COMBNO,PACKNOTEBASICID from PACKNOTEINPDETAIL left outer join DRUMMAST on DRUMMASTID =PACKNOTEINPDETAIL.IDRUMNO    where PACKNOTEBASICID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable Getcuringoutward(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select BRANCHID,DOCID,ITEM,PACKNOTE,to_char(CUROPBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,WCID,TOWCID,to_char(CUROPBASIC.DOCDATE,'dd-MON-yyyy')ENTDATE,SHIFT,ENTEREDBY,TOTQTY,TOTVALUE,REMARKS,FRATE from CUROPBASIC   where CUROPBASICID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetCuringDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select CUROPDETAILID,DRUMNO,BATCHNO,BATCHQTY,COMBNO from CUROPDETAIL   where CUROPBASICID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetCuringOutwardByName(string name)
        {
            string SvSql = string.Empty;
            SvSql = "select BRANCHMAST.BRANCHID,CUROPBASIC.DOCID,ITEMMASTER.ITEMID,Pack.DOCID PSN,to_char(CUROPBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,WCBASIC.WCID,twc.WCID wc,to_char(CUROPBASIC.DOCDATE,'dd-MON-yyyy')ENTDATE,SHIFTMAST.SHIFTNO,CUROPBASIC.ENTEREDBY,TOTQTY,TOTVALUE,CUROPBASIC.REMARKS,FRATE from CUROPBASIC left outer join ITEMMASTER on ITEMMASTERID =CUROPBASIC.ITEM left outer join SHIFTMAST on SHIFTMASTID=CUROPBASIC.SHIFT LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=CUROPBASIC.BRANCHID LEFT OUTER JOIN PACKNOTEBASIC Pack ON Pack.PACKNOTEBASICID=CUROPBASIC.PACKNOTE LEFT OUTER JOIN WCBASIC ON WCBASIC.WCBASICID=CUROPBASIC.WCID LEFT OUTER JOIN WCBASIC twc ON twc.WCBASICID=CUROPBASIC.TOWCID  where CUROPBASICID= '" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable CuringOutwardDetail(string name)
        {
            string SvSql = string.Empty;
            SvSql = "select DRUMNO,BATCHNO,BATCHQTY,COMBNO from CUROPDETAIL   where CUROPBASICID= '" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetQty(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select SUM(IBATCHQTY) as SUM_QTY  from PACKNOTEINPDETAIL   where PACKNOTEBASICID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string StatusChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE CUROPBASIC SET STATUS ='ISACTIVE' WHERE CUROPBASICID='" + id + "'";
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
    }
}
