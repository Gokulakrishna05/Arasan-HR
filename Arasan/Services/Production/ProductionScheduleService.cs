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
            SvSql = "Select BRANCHID,SCHPLANTYPE,DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,WCID,PROCESSID,to_char(SCHDATE,'dd-MON-yyyy') SCHDATE,OPQTY,PRODQTY,FORMULA,to_char(PDUEDATE,'dd-MON-yyyy') PDUEDATE,OPITEMID,OPUNIT,EXPRUNHRS,REFSCHNO,AMDSCHNO,ENTEREDBY from PSBASIC Where PSBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        //public DataTable GetData(string sql)
        //{
        //    DataTable _Dt = new DataTable();
        //    try
        //    {
        //        OracleDataAdapter adapter = new OracleDataAdapter(sql, _connectionString);
        //        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //        adapter.Fill(_Dt);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return _Dt;
        //}
        //public int GetDataId(String sql)
        //{
        //    DataTable _dt = new DataTable();
        //    int Id = 0;
        //    try
        //    {
        //        _dt = GetData(sql);
        //        if (_dt.Rows.Count > 0)
        //        {
        //            Id = Convert.ToInt32(_dt.Rows[0][0].ToString() == string.Empty ? "0" : _dt.Rows[0][0].ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Id;
        //}

        //public bool UpdateStatus(string query)
        //{
        //    bool Saved = true;
        //    try
        //    {
        //        OracleConnection objConn = new OracleConnection(_connectionString);
        //        OracleCommand objCmd = new OracleCommand(query, objConn);
        //        objCmd.Connection.Open();
        //        objCmd.ExecuteNonQuery();
        //        objCmd.Connection.Close();
        //    }
        //    catch (Exception ex)
        //    {

        //        Saved = false;
        //    }
        //    return Saved;
        //}

        public string ProductionScheduleCRUD(ProductionSchedule cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                //if (cy.ID == null)
                //{
                //    DateTime theDate = DateTime.Now;
                //    DateTime todate; DateTime fromdate;
                //    string t; string f;
                //    if (DateTime.Now.Month >= 4)
                //    {
                //        todate = theDate.AddYears(1);
                //    }
                //    else
                //    {
                //        todate = theDate;
                //    }
                //    if (DateTime.Now.Month >= 4)
                //    {
                //        fromdate = theDate;
                //    }
                //    else
                //    {
                //        fromdate = theDate.AddYears(-1);
                //    }
                //    t = todate.ToString("yy");
                //    f = fromdate.ToString("yy");
                //    string disp = string.Format("{0}-{1}", f, t);

                //    int idc = GetDataId(" SELECT COMMON_TEXT FROM COMMON_MASTER WHERE COMMON_TYPE = 'PSch' AND IS_ACTIVE = 'Y'");
                //    cy.DocId = string.Format("{0}/{3}/{1} - {2} ", "TAAI", "PSch", (idc + 1).ToString(), disp);

                //    string updateCMd = " UPDATE COMMON_MASTER SET COMMON_TEXT ='" + (idc + 1).ToString() + "' WHERE COMMON_TYPE ='PSch' AND IS_ACTIVE ='Y'";
                //    try
                //    {
                //        UpdateStatus(updateCMd);
                //    }
                //    catch (Exception ex)
                //    {
                //        throw ex;
                //    }
                //}

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
                    objCmd.Parameters.Add("PDUEDATE", OracleDbType.Date).Value = DateTime.Parse(cy.Proddt);
                    objCmd.Parameters.Add("OPITEMID", OracleDbType.NVarchar2).Value = cy.Itemid;
                    objCmd.Parameters.Add("OPUNIT", OracleDbType.NVarchar2).Value = cy.Unit;
                    objCmd.Parameters.Add("OPQTY", OracleDbType.NVarchar2).Value = cy.Qty;
                    objCmd.Parameters.Add("EXPRUNHRS", OracleDbType.NVarchar2).Value = cy.Exprunhrs;
                    objCmd.Parameters.Add("REFSCHNO", OracleDbType.NVarchar2).Value = cy.Refno;
                    objCmd.Parameters.Add("AMDSCHNO", OracleDbType.NVarchar2).Value = cy.Amdno;
                    objCmd.Parameters.Add("ENTEREDBY", OracleDbType.NVarchar2).Value = cy.Entered;
                    objCmd.Parameters.Add("PRODQTY", OracleDbType.NVarchar2).Value = cy.ProdQty;
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
                            if (cp.Isvalid == "Y" && cp.Itemd != null)
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
                                    objCmds.Parameters.Add("ODDATE", OracleDbType.Date).Value = DateTime.Parse(cp.SchDate);
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
                            if (cp.Isvalid == "Y" && cp.Pack != null)
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
            SvSql = "select PSBASICID,to_char(ODDATE,'dd-MON-yyyy')ODDATE,ODRUNHRS,ODITEMID,ODQTY,NOOFCHARGE from PSOUTDAYDETAIL  where PSBASICID='" + id + "' ";
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
                            DocId = rdr["DOCID"].ToString(),


                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public DataTable EditProSche(string PROID)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHMAST.BRANCHID,SCHPLANTYPE,DOCID,to_char(PSBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,WCBASIC.WCID,PROCESSMAST.PROCESSID,to_char(PSBASIC.SCHDATE,'dd-MON-yyyy') SCHDATE,OPQTY,PRODQTY,FORMULA,to_char(PSBASIC.PDOCDT,'dd-MON-yyyy') PDOCDT,ITEMMASTER.ITEMID,OPUNIT,EXPRUNHRS,REFSCHNO,AMDSCHNO,ENTEREDBY from PSBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=PSBASIC.BRANCHID LEFT OUTER JOIN WCBASIC ON PSBASIC.WCID=WCBASIC.WCBASICID LEFT OUTER JOIN PROCESSMAST ON PROCESSMAST.PROCESSMASTID=PSBASIC.PROCESSID LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=PSBASIC.OPITEMID Where PSBASICID='" + PROID + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable ProPackDetail(string PROID)
        {
            string SvSql = string.Empty;
            SvSql = "select PSBASICID,PKITEMID ,PKQTY from PSPACKDETAIL   where PSBASICID='" + PROID + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable ProDailyDatDetail(string PROID)
        {
            string SvSql = string.Empty;
            SvSql = "select PSBASICID,to_char(PSOUTDAYDETAIL.ODDATE,'dd-MON-yyyy')ODDATE,ODRUNHRS,ITEMMASTER.ITEMID ,ODQTY,NOOFCHARGE from PSOUTDAYDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=PSOUTDAYDETAIL.ODITEMID  where PSBASICID='" + PROID + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable ProParmDetail(string PROID)
        {
            string SvSql = string.Empty;
            SvSql = "select PSBASICID,PARAMETERS,UNIT,IPARAMVALUE,FPARAMVALUE,REMARKS from PSPARAMDETAIL  where PSBASICID='" + PROID + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable ProOutDetail(string PROID)
        {
            string SvSql = string.Empty;
            SvSql = "select PSBASICID,ITEMMASTER.ITEMID ,OITEMDESC,OUNIT,OPER,ALPER,OTYPE,SCHQTY,PQTY from PSOUTDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=PSOUTDETAIL.OITEMID where PSBASICID='" + PROID + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable ProIndetail(string PROID)
        {
            string SvSql = string.Empty;
            SvSql = "select PSBASICID,ITEMMASTER.ITEMID ,RUNIT,RITEMDESC,IPER,RQTY from PSINPDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=PSINPDETAIL.RITEMID  where PSBASICID='" + PROID + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        //public string StatusChange(string tag, int id)
        //{

        //    try
        //    {

        //        string StatementType = string.Empty; string svSQL = "";
        //        using (OracleConnection objConn = new OracleConnection(_connectionString))
        //        {
        //            string Sql = string.Empty;
        //            //Sql = "UPDATE DRUM_STOCK SET BALANCE_QTY ='0' WHERE DRUM_STOCK_ID='" + dt.Rows[i]["DRUM_STOCK_ID"].ToString() + "'";
        //            OracleCommand objCmds = new OracleCommand(Sql, objConn);
        //            objConn.Open();
        //            objCmds.ExecuteNonQuery();
        //            objConn.Close();
        //        }
              

                
        //        //dbcommand = new SqlCommand(svSQL, conn);
        //        //dbcommand.CommandType = CommandType.StoredProcedure;
        //        //dbcommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
        //        //dbcommand.Parameters.Add("@isactive", SqlDbType.Char, 1).Value = tag == "Del" ? "N" : "Y";
        //        //dbcommand.Parameters.Add("@StatementType", SqlDbType.NVarChar, 50).Value = "Delete";
        //        //if (dbcommand.Connection.State == ConnectionState.Open)
        //        //{
        //        //    dbcommand.Connection.Close();
        //        //}
        //        //if (_dtransactions.InsUpateCMD(dbcommand) == true)
        //        //{

        //        //}

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return "";

        //}
    }
}
