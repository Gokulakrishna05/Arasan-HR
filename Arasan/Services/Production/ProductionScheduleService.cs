using Arasan.Interface.Production;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services.Production
{
    public class ProductionScheduleService : IProductionScheduleService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public ProductionScheduleService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
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
        public DataTable GetItemSubGrp()
        {
            string SvSql = string.Empty;
            SvSql = "Select SGCODE,ITEMSUBGROUPID FROM ITEMSUBGROUP";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemSubGroup(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,SUBGROUPCODE from ITEMMASTER WHERE ITEMMASTERID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItem(string value)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER WHERE SUBGROUPCODE='" + value + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItem()
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProcess()
        {
            string SvSql = string.Empty;
            SvSql = "Select PROCESSID ,PROCESSMASTID from PROCESSMAST";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetProductionSchedule(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHID,SCHPLANTYPE,DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,WCID,PROCESSID,to_char(SCHDATE,'dd-MON-yyyy') SCHDATE,FORMULA,to_char(PDOCDT,'dd-MON-yyyy') PDOCDT,OPITEMID,OPUNIT,EXPRUNHRS,REFSCHNO,AMDSCHNO,ENTEREDBY from PSBASIC Where PSBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        //public DataTable GetProductionScheduleServiceDetail(string id)
        //{
        //    throw new NotImplementedException();
        //}

        public string ProductionScheduleCRUD(ProductionSchedule cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("PSBASICPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "PSBASICPROC";*/

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
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("SCHPLANTYPE", OracleDbType.NVarchar2).Value = cy.Type;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.Date).Value = DateTime.Parse(cy.Docdate);
                    objCmd.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = cy.WorkCenter;
                    objCmd.Parameters.Add("PROCESSID", OracleDbType.NVarchar2).Value = cy.Process;
                    objCmd.Parameters.Add("SCHDATE", OracleDbType.Date).Value = DateTime.Parse(cy.Schdate);
                    objCmd.Parameters.Add("FORMULA", OracleDbType.NVarchar2).Value = cy.Formula;
                    objCmd.Parameters.Add("PDOCDT", OracleDbType.Date).Value = DateTime.Parse(cy.Proddt);
                    objCmd.Parameters.Add("OPITEMID", OracleDbType.NVarchar2).Value = cy.Itemid;
                    objCmd.Parameters.Add("OPUNIT", OracleDbType.NVarchar2).Value = cy.Unit;
                    objCmd.Parameters.Add("OPQTY", OracleDbType.NVarchar2).Value = cy.Qty;
                    objCmd.Parameters.Add("EXPRUNHRS", OracleDbType.NVarchar2).Value = cy.Exprunhrs;
                    objCmd.Parameters.Add("REFSCHNO", OracleDbType.NVarchar2).Value = cy.Refno;
                    objCmd.Parameters.Add("AMDSCHNO", OracleDbType.NVarchar2).Value = cy.Amdno;
                    objCmd.Parameters.Add("ENTEREDBY", OracleDbType.NVarchar2).Value = cy.Entered;
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
                        foreach (ProductionScheduleItem cp in cy.PrsLst)
                        {
                            if (cp.Isvalid == "Y" && cp.ItemId != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("PSINPDETAILPROC", objConns);
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
                                    objCmds.Parameters.Add("PSBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("RITEMID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                    objCmds.Parameters.Add("RITEMDESC", OracleDbType.NVarchar2).Value = cp.Desc;
                                    objCmds.Parameters.Add("RUNIT", OracleDbType.NVarchar2).Value = cp.Unit;
                                    objCmds.Parameters.Add("IPER", OracleDbType.NVarchar2).Value = cp.Input;
                                    objCmds.Parameters.Add("RQTY", OracleDbType.NVarchar2).Value = cp.Qty;
                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;

                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }



                            }

                        }
                        foreach (ProductionItem cp in cy.ProLst)
                        {
                            if (cp.Isvalid == "Y" && cp.Item != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("PSOUTDETAILPROC", objConns);
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
                                    objCmds.Parameters.Add("PSBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("OITEMID", OracleDbType.NVarchar2).Value = cp.Item;
                                    objCmds.Parameters.Add("OITEMDESC", OracleDbType.NVarchar2).Value = cp.Des;
                                    objCmds.Parameters.Add("OUNIT", OracleDbType.NVarchar2).Value = cp.Unit;
                                    objCmds.Parameters.Add("OPER", OracleDbType.NVarchar2).Value = cp.Output;
                                    objCmds.Parameters.Add("ALPER", OracleDbType.NVarchar2).Value = cp.Alam;
                                    objCmds.Parameters.Add("OTYPE", OracleDbType.NVarchar2).Value = cp.OutputType;
                                    objCmds.Parameters.Add("SCHQTY", OracleDbType.NVarchar2).Value = cp.Sch;
                                    objCmds.Parameters.Add("PQTY", OracleDbType.NVarchar2).Value = cp.Produced;
                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }



                            }
                        }
                        foreach (ProItem cp in cy.Prlst)
                        {
                            if (cp.Isvalid == "Y" && cp.Parameters != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("PSPARAMDETAILPROC", objConns);
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
                                    objCmds.Parameters.Add("PSBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("PARAMETERS", OracleDbType.NVarchar2).Value = cp.Parameters;
                                    objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = cp.Unit;
                                    objCmds.Parameters.Add("IPARAMVALUE", OracleDbType.NVarchar2).Value = cp.Initial;
                                    objCmds.Parameters.Add("FPARAMVALUE", OracleDbType.NVarchar2).Value = cp.Final;
                                    objCmds.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cp.Remarks;
                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }



                            }
                        }
                        foreach (ProScItem cp in cy.ProscLst)
                        {
                            if (cp.Isvalid == "Y" && cp.Itemd != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("PSOUTDAYDETAILPROC", objConns);
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
                                    objCmds.Parameters.Add("PSBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("ODDATE", OracleDbType.Date).Value = cp.SchDate;
                                    objCmds.Parameters.Add("ODRUNHRS", OracleDbType.NVarchar2).Value = cp.Hrs;
                                    objCmds.Parameters.Add("ODITEMID", OracleDbType.NVarchar2).Value = cp.Itemd;
                                    objCmds.Parameters.Add("ODQTY", OracleDbType.NVarchar2).Value = cp.Qty;
                                    objCmds.Parameters.Add("NOOFCHARGE", OracleDbType.NVarchar2).Value = cp.Change;
                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }



                            }
                        }
                        foreach (ProSchItem cp in cy.ProschedLst)
                        {
                            if (cp.Isvalid == "Y" && cp.Pack != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("PSPACKDETAILPROC", objConns);
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
                                    objCmds.Parameters.Add("PSBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("PKITEMID", OracleDbType.NVarchar2).Value = cp.Pack;
                                    objCmds.Parameters.Add("PKQTY", OracleDbType.NVarchar2).Value = cp.Qty;
                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
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
        public DataTable GetProductionScheduleDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PSBASICID,RITEMID,RUNIT,RITEMDESC,IPER,RQTY from PSINPDETAIL  where PSBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProductionScheduleOutputDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PSBASICID,OITEMID,OITEMDESC,OUNIT,OPER,ALPER,OTYPE,SCHQTY,PQTY from PSOUTDETAIL where PSBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProductionScheduleParametersDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PSBASICID,PARAMETERS,UNIT,IPARAMVALUE,FPARAMVALUE,REMARKS from PSPARAMDETAIL  where PSBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetOutputDetailsDayWiseDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PSBASICID,ODDATE,ODRUNHRS,ODITEMID,ODQTY,NOOFCHARGE from PSOUTDAYDETAIL  where PSBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPackDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PSBASICID,PKITEMID,PKQTY from PSPACKDETAIL where PSBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "select UNITMAST.UNITID,ITEMID,ITEMDESC,UNITMAST.UNITMASTID,LATPURPRICE,ITEMMASTERPUNIT.CF,ITEMMASTER.LATPURPRICE from ITEMMASTER LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID LEFT OUTER JOIN ITEMMASTERPUNIT ON  ITEMMASTER.ITEMMASTERID=ITEMMASTERPUNIT.ITEMMASTERID Where ITEMMASTER.ITEMMASTERID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public IEnumerable<ProductionSchedule> GetProductionSchedule()
        {
            List<ProductionSchedule> cmpList = new List<ProductionSchedule>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select BRANCHMAST.BRANCHID,DOCID,to_char(PSBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PROCESSMAST.PROCESSID,WCBASIC.WCID,PSBASICID from PSBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=PSBASIC.BRANCHID  LEFT OUTER JOIN PROCESSMAST ON PROCESSMASTID=PSBASIC.PROCESSID  LEFT OUTER JOIN WCBASIC ON WCBASICID=PSBASIC.WCID";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ProductionSchedule cmp = new ProductionSchedule
                        {

                            ID = rdr["PSBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            WorkCenter = rdr["WCID"].ToString(),
                            Process = rdr["PROCESSID"].ToString(),
                            Docdate = rdr["DOCDATE"].ToString(),


                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
    }
}
