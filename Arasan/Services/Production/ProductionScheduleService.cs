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
        public DataTable GetProdSche(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PRODFCBASICID,WCBASIC.WCID,PRODFCPYID,PYWCID,ITEMMASTER.ITEMID,PYITEMID,PYREQQTY,PYTARQTY,UNITMAST.UNITID, PYPRODCAPD,PYPRODQTY,PREBALQTY,PYREQAP,PROCESSMAST.PROCESSID,WCBASIC.PROCESSID as process from PRODFCPY INNER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=PRODFCPY.PYITEMID  INNER JOIN WCBASIC on WCBASIC.WCBASICID=PRODFCPY.PYWCID LEFT OUTER JOIN PROCESSMAST  on PROCESSMAST.PROCESSMASTID=WCBASIC.PROCESSID LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID where PRODFCPYID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProdSchePolish(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PRODFCBASICID,WCBASIC.WCID,PRODFCPIGID,PIGWCID,PIGPRODD,ITEMMASTER.ITEMID,PIGITEMID,PIGRAWREQPY,UNITMAST.UNITID,PROCESSMAST.PROCESSID,WCBASIC.PROCESSID as process from PRODFCPIG INNER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=PRODFCPIG.PIGITEMID  INNER JOIN WCBASIC on WCBASIC.WCBASICID=PRODFCPIG.PIGWCID LEFT OUTER JOIN PROCESSMAST  on PROCESSMAST.PROCESSMASTID=WCBASIC.PROCESSID LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID where PRODFCPIGID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPolishInputDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PRODFCBASICID,PRODFCPIGID,ITEMMASTER.ITEMID,PIGADDIT,PIGRAWMAT,it.ITEMID as item,PIGRAWREQPY,UNITMAST.UNITID,PIGADDPER,ITEMMASTER.ITEMDESC from PRODFCPIG INNER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=PRODFCPIG.PIGRAWMAT LEFT OUTER JOIN ITEMMASTER it ON it.ITEMMASTERID=PRODFCPIG.PIGADDIT LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID where PRODFCPIGID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPolishOutputDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PRODFCBASICID,PRODFCPIGID,ITEMMASTER.ITEMID,PIGITEMID,PIGRAWREQPY,UNITMAST.UNITID,PIGADDPER,ITEMMASTER.ITEMDESC from PRODFCPIG INNER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=PRODFCPIG.PIGITEMID LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID where PRODFCPIGID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProdScheInputDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMMASTER.ITEMID,PYITEMID,UNITMAST.UNITID, PYPRODCAPD,PYPRODQTY,PREBALQTY,it.ITEMID as item,PYADD1,PYADDPER,PYREQAP,ITEMMASTER.ITEMDESC from PRODFCPY INNER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=PRODFCPY.PYITEMID LEFT OUTER JOIN ITEMMASTER it ON it.ITEMMASTERID=PRODFCPY.PYADD1 LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID where PRODFCPYID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProdScheOutputDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMMASTER.ITEMID,PYITEMID,UNITMAST.UNITID, PYPRODCAPD,PYPRODQTY,PREBALQTY,it.ITEMID as item,PYADD1,PYADDPER,PYREQAP,ITEMMASTER.ITEMDESC from PRODFCPY INNER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=PRODFCPY.PYITEMID LEFT OUTER JOIN ITEMMASTER it ON it.ITEMMASTERID=PRODFCPY.PYADD1 LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID where PRODFCPYID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProdScheRVD(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PRODFCBASICID,WCBASIC.WCID,PRODFCRVDID,RVDWCID,RVDPRODD,ITEMMASTER.ITEMID,RVDITEMID,RVDRAWQTY,UNITMAST.UNITID,PROCESSMAST.PROCESSID,WCBASIC.PROCESSID as process from PRODFCRVD INNER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=PRODFCRVD.RVDITEMID  INNER JOIN WCBASIC on WCBASIC.WCBASICID=PRODFCRVD.RVDWCID LEFT OUTER JOIN PROCESSMAST  on PROCESSMAST.PROCESSMASTID=WCBASIC.PROCESSID LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID where PRODFCRVDID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetRVDInputDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PRODFCBASICID,ITEMMASTER.ITEMID,RVDRAWMATit.ITEMID as item,RVDITEMID,RVDRAWQTY,UNITMAST.UNITID,ITEMMASTER.ITEMDESC from PRODFCRVD INNER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=PRODFCRVD.RVDRAWMAT LEFT OUTER JOIN ITEMMASTER it ON it.ITEMMASTERID=PRODFCRVD.RVDCONS LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID where PRODFCRVDID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetRVDOutputDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PRODFCBASICID,ITEMMASTER.ITEMID,RVDITEMID,RVDRAWQTY,UNITMAST.UNITID,ITEMMASTER.ITEMDESC from PRODFCRVD INNER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=PRODFCRVD.RVDITEMID LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID where PRODFCRVDID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProdSchePaste(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PRODFCBASICID,WCBASIC.WCID,PAWCID,ITEMMASTER.ITEMID,PAITEMID,PAPRODD,PAAPPOW,UNITMAST.UNITID,PROCESSMAST.PROCESSID,WCBASIC.PROCESSID as process from PRODFCPA INNER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=PRODFCPA.PAITEMID  INNER JOIN WCBASIC on WCBASIC.WCBASICID=PRODFCPA.PAWCID LEFT OUTER JOIN PROCESSMAST  on PROCESSMAST.PROCESSMASTID=WCBASIC.PROCESSID LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID where PRODFCPAID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPasteInputDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PRODFCBASICID,ITEMMASTER.ITEMID,PAADD1,PAITEMID,PAALLADDIT,PAAPPOW,UNITMAST.UNITID,ITEMMASTER.ITEMDESC from PRODFCPA INNER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=PRODFCPA.PAADD1 LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID where PRODFCPAID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPasteOutputDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PRODFCBASICID,ITEMMASTER.ITEMID,PAITEMID,PAALLADDIT,PAAPPOW,UNITMAST.UNITID,ITEMMASTER.ITEMDESC from PRODFCPA INNER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=PRODFCPA.PAITEMID LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID where PRODFCPAID='" + id + "' ";
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
                if(cy.ID!=null)
                {
                    cy.ID = null;
                }
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);


                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'PSch' AND ACTIVESEQUENCE = 'T'");
                string docid = string.Format("{0}{1}", "PSch", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='PSch' AND ACTIVESEQUENCE ='T'";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                cy.DocId = docid;
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
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.Docdate;
                    objCmd.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = cy.WorkCenterid;
                    objCmd.Parameters.Add("PROCESSID", OracleDbType.NVarchar2).Value = cy.Processid;
                    objCmd.Parameters.Add("SCHDATE", OracleDbType.NVarchar2).Value = cy.Schdate;
                    objCmd.Parameters.Add("FORMULA", OracleDbType.NVarchar2).Value = cy.Formula;
                    objCmd.Parameters.Add("PDUEDATE", OracleDbType.NVarchar2).Value = cy.Proddt;
                    objCmd.Parameters.Add("OPITEMID", OracleDbType.NVarchar2).Value = cy.saveitemid;
                    objCmd.Parameters.Add("OPUNIT", OracleDbType.NVarchar2).Value = cy.Unit;
                    objCmd.Parameters.Add("OPQTY", OracleDbType.NVarchar2).Value = cy.Qty;
                    objCmd.Parameters.Add("EXPRUNHRS", OracleDbType.NVarchar2).Value = cy.Exprunhrs;
                     
                    objCmd.Parameters.Add("ENTEREDBY", OracleDbType.NVarchar2).Value = cy.Enterd;
                    objCmd.Parameters.Add("PRODQTY", OracleDbType.NVarchar2).Value = cy.ProdQty;
                    objCmd.Parameters.Add("FROMDATE", OracleDbType.NVarchar2).Value = cy.startdate;
                    objCmd.Parameters.Add("TODATE", OracleDbType.NVarchar2).Value = cy.enddate;
                    objCmd.Parameters.Add("FORDETID", OracleDbType.NVarchar2).Value = cy.detid;
                    objCmd.Parameters.Add("FORTYPE", OracleDbType.NVarchar2).Value = cy.ttype;
                  
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
                        if (cy.PrsLst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (ProductionScheduleItem cp in cy.PrsLst)
                                {
                                    if (cp.Isvalid == "Y" && cp.saveItemId != null)
                                    {
                                        svSQL = "Insert into PSINPDETAIL (PSBASICID,RITEMID,RITEMDESC,RUNIT,IPER,RQTY,FORDETID) VALUES ('" + Pid + "','" + cp.saveItemId + "','" + cp.Desc + "','" + cp.Unit + "','" + cp.Input + "','" + cp.Qty + "','"+cy.detid+"')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                svSQL = "Delete PSINPDETAIL WHERE PSBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (ProductionScheduleItem cp in cy.PrsLst)
                                {
                                    if (cp.Isvalid == "Y" && cp.saveItemId != null)
                                    {
                                        svSQL = "Insert into PSINPDETAIL (PSBASICID,RITEMID,RITEMDESC,RUNIT,IPER,RQTY) VALUES ('" + Pid + "','" + cp.saveItemId + "','" + cp.Desc + "','" + cp.Unit + "','" + cp.Input + "','" + cp.Qty + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }



                            }

                        if (cy.ProLst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (ProductionItem cp in cy.ProLst)
                                {
                                    if (cp.Isvalid == "Y" && cp.saveItemId != null)
                                    {
                                        svSQL = "Insert into PSOUTDETAIL (PSBASICID,OITEMID,OITEMDESC,OUNIT,OPER,ALPER,OTYPE,SCHQTY,PQTY) VALUES ('" + Pid + "','" + cp.saveItemId + "','" + cp.Des + "','" + cp.Unit + "','" + cp.Output + "','" + cp.Alam + "','" + cp.OutputType + "','" + cp.Sch + "','" + cp.Produced + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                svSQL = "Delete PSOUTDETAIL WHERE PSBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (ProductionItem cp in cy.ProLst)
                                {
                                    if (cp.Isvalid == "Y" && cp.saveItemId != null)
                                    {
                                        svSQL = "Insert into PSOUTDETAIL (PSBASICID,OITEMID,OITEMDESC,OUNIT,OPER,ALPER,OTYPE,SCHQTY,PQTY) VALUES ('" + Pid + "','" + cp.saveItemId + "','" + cp.Des + "','" + cp.Unit + "','" + cp.Output + "','" + cp.Alam + "','" + cp.OutputType + "','" + cp.Sch + "','" + cp.Produced + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();




                                    }
                                }
                            }
                        }
                        //if (cy.Prlst != null)
                        //{
                        //    if (cy.ID == null)
                        //    {
                        //        foreach (ProItem cp in cy.Prlst)
                        //        {
                        //            if (cp.Isvalid == "Y" && cp.Parameters != "0")
                        //            {
                        //                svSQL = "Insert into PSPARAMDETAIL (PSBASICID,PARAMETERS,UNIT,IPARAMVALUE,FPARAMVALUE,REMARKS) VALUES ('" + Pid + "','" + cp.Parameters + "','" + cp.Unit + "','" + cp.Initial + "','" + cp.Final + "','" + cp.Remarks + "')";
                        //                OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        //                objCmds.ExecuteNonQuery();

                        //            }
                        //        }


                        //    }
                        //    else
                        //    {
                        //        svSQL = "Delete PSPARAMDETAIL WHERE PSBASICID='" + cy.ID + "'";
                        //        OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                        //        objCmdd.ExecuteNonQuery();
                        //        foreach (ProItem cp in cy.Prlst)
                        //        {
                        //            if (cp.Isvalid == "Y" && cp.Parameters != "0")
                        //            {
                        //                svSQL = "Insert into PSPARAMDETAIL (PSBASICID,PARAMETERS,UNIT,IPARAMVALUE,FPARAMVALUE,REMARKS) VALUES ('" + Pid + "','" + cp.Parameters + "','" + cp.Unit + "','" + cp.Initial + "','" + cp.Final + "','" + cp.Remarks + "')";
                        //                OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        //                objCmds.ExecuteNonQuery();

                        //            }
                        //        }
                        //    }
                        //}
                        if (cy.ProscLst != null)
                        {
                            if (cy.ID == null)
                            {
                                int ro = 1;
                                foreach (ProScItem cp in cy.ProscLst)
                                {
                                    if (cp.isvalid == "Y" && cp.schdate != null)
                                    {
                                        string item = cp.itemd;
                                        DateTime da =DateTime.Parse(cp.schdate);
                                      
                                        string uniq = string.Format("{0}----{1}",  da.ToString("dd/MM/yyyy"),item);
                                        svSQL = "Insert into PSOUTDAYDETAIL (PSBASICID,ODDATE,ODRUNHRS,ODITEMID,ODQTY,UNIQUEDO,PSOUTDAYDETAILROW,SOFARRUNHRS,BALHRS,NOOFCHARGE) VALUES ('" + Pid + "','" + cp.schdate + "','" + cp.hrs + "','" + cy.saveitemid + "','" + cp.qty + "','"+ uniq+"','"+ ro +"','0','0','0')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                    ro++;
                                }

                            }
                            
                        }
                        if (cy.ProschedLst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (ProSchItem cp in cy.ProschedLst)
                                {
                                    if (cp.Isvalid == "Y" && cp.item != null)
                                    {
                                        svSQL = "Insert into PSPACKDETAIL (PSBASICID,PKITEMID,PKQTY) VALUES ('" + Pid + "','" + cp.item + "','" + cp.Qty + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }
                                }
                            }
                            else
                            {
                                svSQL = "Delete PSPACKDETAIL WHERE PSBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (ProSchItem cp in cy.ProschedLst)
                                {
                                    if (cp.Isvalid == "Y" && cp.Pack != null)
                                    {
                                        svSQL = "Insert into PSPACKDETAIL (PSBASICID,PKITEMID,PKQTY) VALUES ('" + Pid + "','" + cp.Pack + "','" + cp.Qty + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }
                                }
                            }
                            if (cy.ttype == "Pyro")
                            {
                                DateTime start = DateTime.Parse(cy.startdate);
                                DateTime end = DateTime.Parse(cy.enddate);

                                TimeSpan difference = end - start;
                                int daysAgo = (int)difference.TotalDays;
                                string ago = daysAgo.ToString();
                                if (cy.Days == ago)
                                {
                                    svSQL = "UPDATE PRODFCPY SET SCHEDULE='Y',STATUS='Complete' WHERE PRODFCPYID='" + cy.detid + "'";
                                    OracleCommand objCmdss = new OracleCommand(svSQL, objConn);
                                    objCmdss.ExecuteNonQuery();
                                }
                                else
                                {
                                    svSQL = "UPDATE PRODFCPY SET  STATUS='Pending' WHERE PRODFCPYID='" + cy.detid + "'";
                                    OracleCommand objCmdss = new OracleCommand(svSQL, objConn);
                                    objCmdss.ExecuteNonQuery();
                                }
                            }
                            if (cy.ttype == "Polish")
                            {
                                DateTime start = DateTime.Parse(cy.startdate);
                                DateTime end = DateTime.Parse(cy.enddate);

                                TimeSpan difference = end - start;
                                int daysAgo = (int)difference.TotalDays;
                                string ago = daysAgo.ToString();
                                if (cy.Days == ago)
                                {
                                    svSQL = "UPDATE PRODFCPIG SET SCHEDULE='Y',STATUS='Complete' WHERE PRODFCPIGID='" + cy.detid + "'";
                                    OracleCommand objCmdss = new OracleCommand(svSQL, objConn);
                                    objCmdss.ExecuteNonQuery();
                                }
                                else
                                {
                                    svSQL = "UPDATE PRODFCPIG SET  STATUS='Pending' WHERE PRODFCPIGID='" + cy.detid + "'";
                                    OracleCommand objCmdss = new OracleCommand(svSQL, objConn);
                                    objCmdss.ExecuteNonQuery();
                                }
                            }
                            if (cy.ttype == "RVD")
                            {
                                DateTime start = DateTime.Parse(cy.startdate);
                                DateTime end = DateTime.Parse(cy.enddate);

                                TimeSpan difference = end - start;
                                int daysAgo = (int)difference.TotalDays;
                                string ago = daysAgo.ToString();
                                if (cy.Days == ago)
                                {
                                    svSQL = "UPDATE PRODFCRVD SET SCHEDULE='Y',STATUS='Complete' WHERE PRODFCRVDID='" + cy.detid + "'";
                                    OracleCommand objCmdss = new OracleCommand(svSQL, objConn);
                                    objCmdss.ExecuteNonQuery();
                                }
                                else
                                {
                                    svSQL = "UPDATE PRODFCRVD SET STATUS='Pending' WHERE PRODFCRVDID='" + cy.detid + "'";
                                    OracleCommand objCmdss = new OracleCommand(svSQL, objConn);
                                    objCmdss.ExecuteNonQuery();
                                }
                            }
                            if (cy.ttype == "Paste")
                            {
                                DateTime start = DateTime.Parse(cy.startdate);
                                DateTime end = DateTime.Parse(cy.enddate);

                                TimeSpan difference = end - start;
                                int daysAgo = (int)difference.TotalDays;
                                string ago = daysAgo.ToString();
                                if (cy.Days == ago)
                                {
                                    svSQL = "UPDATE PRODFCPA SET SCHEDULE='Y',STATUS='Complete' WHERE PRODFCPAID='" + cy.detid + "'";
                                    OracleCommand objCmdss = new OracleCommand(svSQL, objConn);
                                    objCmdss.ExecuteNonQuery();
                                }
                                else
                                {
                                    svSQL = "UPDATE PRODFCPA SET  STATUS='Pending' WHERE PRODFCPAID='" + cy.detid + "'";
                                    OracleCommand objCmdss = new OracleCommand(svSQL, objConn);
                                    objCmdss.ExecuteNonQuery();
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
        public DataTable GetAllProdSch(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select BRANCHMAST.BRANCHID,ITEMMASTER.ITEMID,DOCID,to_char(PSBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PROCESSMAST.PROCESSID,WCBASIC.WCID,PSBASICID,PSBASIC.STATUS from PSBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=PSBASIC.BRANCHID LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=PSBASIC.OPITEMID  LEFT OUTER JOIN PROCESSMAST ON PROCESSMASTID=PSBASIC.PROCESSID  LEFT OUTER JOIN WCBASIC ON WCBASICID=PSBASIC.WCID WHERE PSBASIC.IS_ACTIVE='Y' ORDER BY PSBASICID DESC ";
            }
            else
            {
                SvSql = "Select BRANCHMAST.BRANCHID,DOCID,to_char(PSBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PROCESSMAST.PROCESSID,WCBASIC.WCID,PSBASICID,PSBASIC.STATUS from PSBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=PSBASIC.BRANCHID  LEFT OUTER JOIN PROCESSMAST ON PROCESSMASTID=PSBASIC.PROCESSID  LEFT OUTER JOIN WCBASIC ON WCBASICID=PSBASIC.WCID WHERE PSBASIC.IS_ACTIVE='N' ORDER BY PSBASICID DESC ";
            }
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
                    cmd.CommandText = "Select BRANCHMAST.BRANCHID,DOCID,to_char(PSBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PROCESSMAST.PROCESSID,WCBASIC.WCID,PSBASICID,PSBASIC.STATUS from PSBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=PSBASIC.BRANCHID  LEFT OUTER JOIN PROCESSMAST ON PROCESSMASTID=PSBASIC.PROCESSID  LEFT OUTER JOIN WCBASIC ON WCBASICID=PSBASIC.WCID WHERE PSBASIC.STATUS='ACTIVE'";
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
        public DataTable GetPackItem(string value)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT distinct I.ITEMID,pb.OITEMID PACKMAT FROM itemmaster im,wcbasic wc, packconsdetail pc,packbasic pb,ITEMMASTER I WHERE pb.WCID=wc.WCBASICID AND pb.OITEMID=im.ITEMMASTERID AND PC.CITEMID=I.ITEMMASTERID AND im.ITEMID='" + value +"' AND pb.PACKBASICID=pc.PACKBASICID   ";
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
        public string StatusChange(string tag )
        {

            try
            {

                string StatementType = string.Empty; string svSQL = "";
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    string Sql = string.Empty;
                    Sql = "UPDATE PSBASIC SET PSCHSTATUS ='INACTIVE' WHERE PSBASICID='" + tag + "'";
                    OracleCommand objCmds = new OracleCommand(Sql, objConn);
                    objConn.Open();
                    objCmds.ExecuteNonQuery();
                    objConn.Close();
                }



                //dbcommand = new SqlCommand(svSQL, conn);
                //dbcommand.CommandType = CommandType.StoredProcedure;
                //dbcommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
                //dbcommand.Parameters.Add("@isactive", SqlDbType.Char, 1).Value = tag == "Del" ? "N" : "Y";
                //dbcommand.Parameters.Add("@StatementType", SqlDbType.NVarChar, 50).Value = "Delete";
                //if (dbcommand.Connection.State == ConnectionState.Open)
                //{
                //    dbcommand.Connection.Close();
                //}
                //if (_dtransactions.InsUpateCMD(dbcommand) == true)
                //{

                //}

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "";

        }

        public string StatusChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE PSBASIC SET STATUS ='ISACTIVE' WHERE PSBASICID='" + id + "'";
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
