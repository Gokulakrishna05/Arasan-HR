using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace Arasan.Services.Master
{
    public class EmployeeService : IEmployee
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        private static readonly int KeySize = 256; // You can choose 128, 192, or 256 bits
        private static readonly int BlockSize = 128; // Block size for AES
        private static readonly string EncryptionKey = "Arasan"; // Use a strong key
        private static readonly string IV = "TaaiErp"; // Use a strong IV

        public EmployeeService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);

        }




        public long GetMregion(string regionid, string id)
        {
            string SvSql = "SELECT LOCID from EMPLOYEELOCATION where LOCID=" + regionid + " and EMPID=" + id + "";
            DataTable dtCity = new DataTable();
            long user_id = datatrans.GetDataIdlong(SvSql);
            return user_id;
        }
        public string EmployeeCRUD(Employee cy, List<IFormFile> files1)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                if (cy.ID == null)
                {
                    svSQL = " SELECT Count(EMPID) as cnt FROM EMPMAST WHERE EMPNAME = LTRIM(RTRIM('" + cy.EmpName + "')) ";

                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "EmployeeID Already Existed";
                        return msg;
                    }
                }
                string encpass = Encrypt(cy.Password);
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("EMPLOYEEPROC", objConn);
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
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branchs;
                    objCmd.Parameters.Add("EMPNAME", OracleDbType.NVarchar2).Value = cy.EmpName;
                    objCmd.Parameters.Add("EMPID", OracleDbType.NVarchar2).Value = cy.EmpNo;
                    objCmd.Parameters.Add("EMPSEX", OracleDbType.NVarchar2).Value = cy.Gender;
                    objCmd.Parameters.Add("EMPDOB", OracleDbType.NVarchar2).Value = cy.DOB;
                    objCmd.Parameters.Add("ECCITY", OracleDbType.NVarchar2).Value = cy.CityId;
                    objCmd.Parameters.Add("ECSTATE", OracleDbType.NVarchar2).Value = cy.StateId;
                    objCmd.Parameters.Add("ECMAILID", OracleDbType.NVarchar2).Value = cy.EmailId;
                    objCmd.Parameters.Add("ECPHNO", OracleDbType.NVarchar2).Value = cy.PhoneNo;
                    objCmd.Parameters.Add("FATHERNAME", OracleDbType.NVarchar2).Value = cy.FatherName;
                    objCmd.Parameters.Add("MOTHERNAME", OracleDbType.NVarchar2).Value = cy.MotherName;
                    objCmd.Parameters.Add("GAURDNAME", OracleDbType.NVarchar2).Value = cy.GaurdName;
                    objCmd.Parameters.Add("ECADD1", OracleDbType.NVarchar2).Value = cy.Address1;
                    objCmd.Parameters.Add("ECADD2", OracleDbType.NVarchar2).Value = cy.Address2;
                    objCmd.Parameters.Add("ECPCODE", OracleDbType.NVarchar2).Value = cy.PinCode;
                    objCmd.Parameters.Add("EMPPAYCAT", OracleDbType.NVarchar2).Value = cy.PayCate;
                    objCmd.Parameters.Add("EMPBASIC", OracleDbType.NVarchar2).Value = cy.EMPBasic;
                    objCmd.Parameters.Add("PFNO", OracleDbType.NVarchar2).Value = cy.PFNo;
                    objCmd.Parameters.Add("ESINO", OracleDbType.NVarchar2).Value = cy.ESINo;
                    objCmd.Parameters.Add("EMPCOST", OracleDbType.NVarchar2).Value = cy.Cost;
                    objCmd.Parameters.Add("PFDT", OracleDbType.NVarchar2).Value = cy.PFdate;
                    objCmd.Parameters.Add("ESIDT", OracleDbType.NVarchar2).Value = cy.ESIDate;
                    objCmd.Parameters.Add("USERNAME", OracleDbType.NVarchar2).Value = cy.UserName;
                    objCmd.Parameters.Add("PASSWORD", OracleDbType.NVarchar2).Value = encpass;
                    objCmd.Parameters.Add("EMPDEPT", OracleDbType.NVarchar2).Value = cy.EMPDeptment;
                    objCmd.Parameters.Add("EMPDESIGN", OracleDbType.NVarchar2).Value = cy.EMPDesign;
                    objCmd.Parameters.Add("EMPDEPTCODE", OracleDbType.NVarchar2).Value = cy.Dept;
                    objCmd.Parameters.Add("JOINDATE", OracleDbType.NVarchar2).Value = cy.JoinDate;
                    objCmd.Parameters.Add("RESIGNDATE", OracleDbType.NVarchar2).Value = cy.ResignDate;
                    objCmd.Parameters.Add("IS_ACTIVE", OracleDbType.NVarchar2).Value = "Y";



                    if (cy.ID == null)
                    {
                        objCmd.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = cy.createby;
                        objCmd.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    }
                    else
                    {
                        objCmd.Parameters.Add("UPDATED_BY", OracleDbType.NVarchar2).Value = cy.createby;
                        objCmd.Parameters.Add("UPDATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    }
                    objCmd.Parameters.Add("PAYMODE", OracleDbType.NVarchar2).Value = cy.Payment;
                    objCmd.Parameters.Add("BANK", OracleDbType.NVarchar2).Value = cy.Bank;
                    objCmd.Parameters.Add("SHIFTCATEGORY", OracleDbType.NVarchar2).Value = cy.Shift;
                    objCmd.Parameters.Add("EACTIVE", OracleDbType.NVarchar2).Value ="Yes";
                    objCmd.Parameters.Add("BONAPP", OracleDbType.NVarchar2).Value = cy.Bonus;
                    objCmd.Parameters.Add("CLAPP", OracleDbType.NVarchar2).Value = cy.CL;
                    objCmd.Parameters.Add("PFCLOSE", OracleDbType.NVarchar2).Value = cy.pfclose;
                    objCmd.Parameters.Add("OTYN", OracleDbType.NVarchar2).Value = cy.OT;
                    objCmd.Parameters.Add("BANKACCNO", OracleDbType.NVarchar2).Value = cy.BAccount;
                    objCmd.Parameters.Add("MEALSYN", OracleDbType.NVarchar2).Value = cy.Meals;
                    objCmd.Parameters.Add("APPRENTICE", OracleDbType.NVarchar2).Value = cy.Appren;
                    objCmd.Parameters.Add("LOPYN", OracleDbType.NVarchar2).Value = cy.LOP;
                    //objCmd.Parameters.Add("BindBranch", OracleDbType.NVarchar2).Value = cy.createby;
                    //objCmd.Parameters.Add("EMPSEX", OracleDbType.NVarchar2).Value = cy.Gender;
                    objCmd.Parameters.Add("HANDICAPPED", OracleDbType.NVarchar2).Value = cy.Phychal;

                    objCmd.Parameters.Add("PFAPP", OracleDbType.NVarchar2).Value = cy.PF;
                    objCmd.Parameters.Add("ESIAPP", OracleDbType.NVarchar2).Value = cy.ESI;
                    objCmd.Parameters.Add("WOFF", OracleDbType.NVarchar2).Value = cy.Week;

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



                        if (cy.Phlst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (Prhs cp in cy.Phlst)
                                {
                                    if (cp.Isvalid == "Y" && cp.emp != "")
                                    {
                                        svSQL = "Insert into EMPMPH (EMPMASTID,EMPLOYER,CITY,EDESIG,LSALARYDRAWN,WM) VALUES ('" + Pid + "','" + cp.emp + "','" + cp.city + "','" + cp.wc + "','" + cp.lsd + "','" + cp.wrm + "')";
                                        OracleCommand objCmdss = new OracleCommand(svSQL, objConn);
                                        objCmdss.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                svSQL = "Delete EMPMPH WHERE EMPMASTID='" + cy.ID + "'";
                                OracleCommand objCmddss = new OracleCommand(svSQL, objConn);
                                objCmddss.ExecuteNonQuery();
                                foreach (Prhs cp in cy.Phlst)
                                {
                                    if (cp.Isvalid == "Y" && cp.emp != "")
                                    {
                                        svSQL = "Insert into EMPMPH (EMPMASTID,EMPLOYER,CITY,EDESIG,LSALARYDRAWN,WM) VALUES ('" + Pid + "','" + cp.emp + "','" + cp.city + "','" + cp.wc + "','" + cp.lsd + "','" + cp.wrm + "')";
                                        objCmddss = new OracleCommand(svSQL, objConn);
                                        objCmddss.ExecuteNonQuery();
                                    }
                                }
                            }
                        }

                        if (cy.Inlst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (Eins cp in cy.Inlst)
                                {
                                    if (cp.Isvalid == "Y" && cp.pno != "")
                                    {
                                        svSQL = "Insert into EMPMINS (EMPMASTID,POLICYNO,NATUREOFPOLICY,BADD,ACTPREMIUM,PREMIUM,PSTDT,PEDDT) VALUES ('" + Pid + "','" + cp.pno + "','" + cp.nop + "','" + cp.bad + "','" + cp.apr + "','" + cp.dpr + "','" + cp.psd + "','" + cp.ped + "')";
                                        OracleCommand objCmdss = new OracleCommand(svSQL, objConn);
                                        objCmdss.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                svSQL = "Delete EMPMINS WHERE EMPMASTID='" + cy.ID + "'";
                                OracleCommand objCmddss = new OracleCommand(svSQL, objConn);
                                objCmddss.ExecuteNonQuery();
                                foreach (Eins cp in cy.Inlst)
                                {
                                    if (cp.Isvalid == "Y" && cp.pno != "")
                                    {
                                        svSQL = "Insert into EMPMINS (EMPMASTID,POLICYNO,NATUREOFPOLICY,BADD,ACTPREMIUM,PREMIUM,PSTDT,PEDDT) VALUES ('" + Pid + "','" + cp.pno + "','" + cp.nop + "','" + cp.bad + "','" + cp.apr + "','" + cp.dpr + "','" + cp.psd + "','" + cp.ped + "')";
                                        objCmddss = new OracleCommand(svSQL, objConn);
                                        objCmddss.ExecuteNonQuery();
                                    }
                                }
                            }
                        }


                        if (cy.Emlst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (Emrc cp in cy.Emlst)
                                {
                                    if (cp.Isvalid == "Y" && cp.cna != "")
                                    {

                                        svSQL = "Insert into EMPMEC (EMPMASTID,ECNAME,NREL,ECPHONE,ECMOBILE,ECFAX) VALUES ('" + Pid + "','" + cp.cna + "','" + cp.nor + "','" + cp.pho + "','" + cp.mob + "','" + cp.fax + "')";
                                        OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                        objCmdd.ExecuteNonQuery();

                                    }
                                }

                            }
                            else
                            {
                                svSQL = "Delete EMPMEC WHERE EMPMASTID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (Emrc cp in cy.Emlst)
                                {
                                    if (cp.Isvalid == "Y" && cp.cna != "")
                                    {
                                        svSQL = "Insert into EMPMEC (EMPMASTID,ECNAME,NREL,ECPHONE,ECMOBILE,ECFAX) VALUES ('" + Pid + "','" + cp.cna + "','" + cp.nor + "','" + cp.pho + "','" + cp.mob + "','" + cp.fax + "')";
                                        objCmdd = new OracleCommand(svSQL, objConn);
                                        objCmdd.ExecuteNonQuery();

                                    }
                                }
                            }
                        }


                        if (cy.pelst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (Perf cp in cy.pelst)
                                {
                                    if (cp.Isvalid == "Y" && cp.pps != "")
                                    {

                                        svSQL = "Insert into EMPMREWARDS (EMPMASTID,PERFDESC,RESULT,RATING,AWDGN) VALUES ('" + Pid + "','" + cp.pps + "','" + cp.res + "','" + cp.rat + "','" + cp.awd + "')";
                                        OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                        objCmdd.ExecuteNonQuery();

                                    }
                                }

                            }
                            else
                            {
                                svSQL = "Delete EMPMREWARDS WHERE EMPMASTID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (Perf cp in cy.pelst)
                                {
                                    if (cp.Isvalid == "Y" && cp.pps != "")
                                    {
                                        svSQL = "Insert into EMPMREWARDS (EMPMASTID,PERFDESC,RESULT,RATING,AWDGN) VALUES ('" + Pid + "','" + cp.pps + "','" + cp.res + "','" + cp.rat + "','" + cp.awd + "')";
                                        objCmdd = new OracleCommand(svSQL, objConn);
                                        objCmdd.ExecuteNonQuery();

                                    }
                                }
                            }
                        }

                        if (cy.Edlst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (Edet cp in cy.Edlst)
                                {
                                    if (cp.Isvalid == "Y" && cp.qua != "")
                                    {
                                        svSQL = "Insert into EMPMEDU (EMPMASTID,EDUCATION,UC,ECPLACE,MPER,YRPASSING) VALUES ('" + Pid + "','" + cp.qua + "','" + cp.clg + "','" + cp.plc + "','" + cp.peo + "','" + cp.yop + "')";
                                        OracleCommand objCmdss = new OracleCommand(svSQL, objConn);
                                        objCmdss.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                svSQL = "Delete EMPMEDU WHERE EMPMASTID='" + cy.ID + "'";
                                OracleCommand objCmddss = new OracleCommand(svSQL, objConn);
                                objCmddss.ExecuteNonQuery();
                                foreach (Edet cp in cy.Edlst)
                                {
                                    if (cp.Isvalid == "Y" && cp.qua != "")
                                    {
                                        svSQL = "Insert into EMPMEDU (EMPMASTID,EDUCATION,UC,ECPLACE,MPER,YRPASSING) VALUES ('" + Pid + "','" + cp.qua + "','" + cp.clg + "','" + cp.plc + "','" + cp.peo + "','" + cp.yop + "')";
                                        objCmddss = new OracleCommand(svSQL, objConn);
                                        objCmddss.ExecuteNonQuery();
                                    }
                                }
                            }
                        }


                        if (cy.Dclst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (Dcod cp in cy.Dclst)
                                {
                                    if (cp.Isvalid == "Y" && cp.wc != "")
                                    {
                                        svSQL = "Insert into EMPDCDETAIL (EMPMASTID,DEPTCODE) VALUES ('" + Pid + "','" + cp.wc + "')";
                                        OracleCommand objCmdsr = new OracleCommand(svSQL, objConn);
                                        objCmdsr.ExecuteNonQuery();

                                    }


                                }
                            }
                            else
                            {
                                svSQL = "Delete EMPDCDETAIL WHERE EMPMASTID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (Dcod cp in cy.Dclst)
                                {
                                    if (cp.Isvalid == "Y" && cp.wc != "")
                                    {
                                        svSQL = "Insert into EMPDCDETAIL (EMPMASTID,DEPTCODE) VALUES ('" + Pid + "','" + cp.wc + "')";
                                        objCmdd = new OracleCommand(svSQL, objConn);
                                        objCmdd.ExecuteNonQuery();

                                    }
                                }
                            }
                        }

                        if (cy.Brlst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (Brch cp in cy.Brlst)
                                {
                                    if (cp.Isvalid == "Y" && cp.wc != "")
                                    {
                                        svSQL = "Insert into EMPBCDETAIL (EMPMASTID,BRANCH) VALUES ('" + Pid + "','" + cp.wc + "')";
                                        OracleCommand objCmdsr = new OracleCommand(svSQL, objConn);
                                        objCmdsr.ExecuteNonQuery();

                                    }


                                }
                            }
                            else
                            {
                                svSQL = "Delete EMPBCDETAIL WHERE EMPMASTID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (Brch cp in cy.Brlst)
                                {
                                    if (cp.Isvalid == "Y" && cp.wc != "")
                                    {
                                        svSQL = "Insert into EMPBCDETAIL (EMPMASTID,BRANCH) VALUES ('" + Pid + "','" + cp.wc + "')";
                                        objCmdd = new OracleCommand(svSQL, objConn);
                                        objCmdd.ExecuteNonQuery();

                                    }
                                }
                            }
                        }

                        if (cy.Atlst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (Eatt cp in cy.Atlst)
                                {
                                    if (cp.Isvalid == "Y" && cp.mon != "")
                                    {
                                        svSQL = "Insert into EMPATT (EMPMASTID,AMONTH,PDAYS,ADAYS,LDAYS,NHD,NHW,WO,WDAYS,HDAYS) VALUES ('" + Pid + "','" + cp.mon + "','" + cp.pre + "','" + cp.abs + "','" + cp.lea + "','" + cp.nhd + "','" + cp.nhw + "','" + cp.wo + "','" + cp.wds + "','" + cp.hds + "')";
                                        OracleCommand objCmdss = new OracleCommand(svSQL, objConn);
                                        objCmdss.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                svSQL = "Delete EMPATT WHERE EMPMASTID='" + cy.ID + "'";
                                OracleCommand objCmddss = new OracleCommand(svSQL, objConn);
                                objCmddss.ExecuteNonQuery();
                                foreach (Eatt cp in cy.Atlst)
                                {
                                    if (cp.Isvalid == "Y" && cp.mon != "")
                                    {
                                        svSQL = "Insert into EMPATT (EMPMASTID,AMONTH,PDAYS,ADAYS,LDAYS,NHD,NHW,WO,WDAYS,HDAYS) VALUES ('" + Pid + "','" + cp.mon + "','" + cp.pre + "','" + cp.abs + "','" + cp.lea + "','" + cp.nhd + "','" + cp.nhw + "','" + cp.wo + "','" + cp.wds + "','" + cp.hds + "')";
                                        objCmddss = new OracleCommand(svSQL, objConn);
                                        objCmddss.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                        if (cy.Leavelst != null)
                        {
                            if (cy.ID == null)
                            {

                                foreach (LeaveDet cp in cy.Leavelst)
                                {
                                    if (cp.Isvalid == "Y" && cp.leavecode != "")
                                    {
                                        svSQL = "Insert into EMPMLEAVE (EMPMASTID,EMPMLEAVEROW,LEAVECODE,LEAVESALLOWED,LCDATE) VALUES ('" + Pid + "','1','" + cp.leavecode + "','" + cp.allwoleaves + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "')";
                                        OracleCommand objCmdss = new OracleCommand(svSQL, objConn);
                                        objCmdss.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                svSQL = "Delete EMPMLEAVE WHERE EMPMASTID='" + cy.ID + "'";
                                OracleCommand objCmddss = new OracleCommand(svSQL, objConn);
                                objCmddss.ExecuteNonQuery();
                                foreach (LeaveDet cp in cy.Leavelst)
                                {
                                    if (cp.Isvalid == "Y" && cp.leavecode != "")
                                    {
                                        svSQL = "Insert into EMPMLEAVE (EMPMASTID,EMPMLEAVEROW,LEAVECODE,LEAVESALLOWED,LCDATE) VALUES ('" + Pid + "','1','" + cp.leavecode + "','" + cp.allwoleaves + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "')";
                                        objCmddss = new OracleCommand(svSQL, objConn);
                                        objCmddss.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                        if (cy.Pclst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (Pcod cp in cy.Pclst)
                                {
                                    if (cp.Isvalid == "Y" && cp.pc != "")
                                    {

                                        svSQL = "Insert into EMPPAYCODE (EMPMASTID,PAYCODE,FORMULA,EID) VALUES ('" + Pid + "','" + cp.pc + "','" + cp.pf + "','" + cy.EmpNo + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }
                                }

                            }
                            else
                            {
                                svSQL = "Delete EMPPAYCODE WHERE EMPMASTID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (Pcod cp in cy.Pclst)
                                {
                                    if (cp.Isvalid == "Y" && cp.pc != "")
                                    {
                                        svSQL = "Insert into EMPPAYCODE (EMPMASTID,PAYCODE,FORMULA,EID) VALUES ('" + Pid + "','" + cp.pc + "','" + cp.pf + "','" + cy.EmpNo + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }
                                }
                            }
                        }

                        OracleCommand objCmdsa = new OracleCommand("EMPOTHERINFOPROC", objConn);
                        if (cy.ID == null)
                        {
                            StatementType = "Insert";
                            objCmdsa.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;

                        }
                        else
                        {
                            StatementType = "Update";
                            objCmdsa.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;

                        }
                        objCmdsa.CommandType = CommandType.StoredProcedure;
                        objCmdsa.Parameters.Add("EMPMASTID", OracleDbType.NVarchar2).Value = Pid;
                        objCmdsa.Parameters.Add("MARITALSTATUS", OracleDbType.NVarchar2).Value = cy.MaterialStatus;
                        objCmdsa.Parameters.Add("BLOODGROUP", OracleDbType.NVarchar2).Value = cy.BloodGroup;
                        objCmdsa.Parameters.Add("COMMUNITY", OracleDbType.NVarchar2).Value = cy.Community;
                        objCmdsa.Parameters.Add("PAYTYPE", OracleDbType.NVarchar2).Value = cy.PayType;
                        objCmdsa.Parameters.Add("EMPTYPE", OracleDbType.NVarchar2).Value = cy.EmpType;
                        objCmdsa.Parameters.Add("DISP", OracleDbType.NVarchar2).Value = cy.Disp;

                        objCmdsa.Parameters.Add("OPFNO", OracleDbType.NVarchar2).Value = cy.oldpf;
                        objCmdsa.Parameters.Add("NOOFDEP", OracleDbType.NVarchar2).Value = cy.dependantes;
                        objCmdsa.Parameters.Add("OPFFDT", OracleDbType.NVarchar2).Value = cy.Pffrom;
                        objCmdsa.Parameters.Add("OPFTODT", OracleDbType.NVarchar2).Value = cy.Pfto;
                        objCmdsa.Parameters.Add("ADVACC", OracleDbType.NVarchar2).Value = cy.AdAccount;

                        objCmdsa.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;


                        objCmdsa.ExecuteNonQuery();



                        OracleCommand objCmds1 = new OracleCommand("EMPSKILLPROC", objConn);
                        if (cy.ID == null)
                        {
                            StatementType = "Insert";
                            objCmds1.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;

                        }
                        else
                        {
                            StatementType = "Update";
                            objCmds1.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;

                        }
                        objCmds1.CommandType = CommandType.StoredProcedure;
                        objCmds1.Parameters.Add("EMPMASTID", OracleDbType.NVarchar2).Value = Pid;
                        objCmds1.Parameters.Add("SKILL", OracleDbType.NVarchar2).Value = cy.SkillSet;
                        objCmds1.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;


                        objCmds1.ExecuteNonQuery();

                        if (files1 != null && files1.Count > 0)
                        {
                            int r = 1;
                            foreach (var file in files1)
                            {
                                if (file.Length > 0)
                                {
                                    // Get the file name and combine it with the target folder path
                                    String strLongFilePath1 = file.FileName;
                                    String sFileType1 = "";
                                    sFileType1 = System.IO.Path.GetExtension(file.FileName);
                                    sFileType1 = sFileType1.ToLower();

                                    String strFleName = strLongFilePath1.Replace(sFileType1, "") + String.Format("{0:ddMMMyyyy-hhmmsstt}", DateTime.Now) + sFileType1;
                                    var fileName = Path.Combine("wwwroot/itemdoc", strFleName);
                                    var fileName1 = "../itemdoc/" + strFleName;
                                    var name = file.FileName;
                                    // Save the file to the target folder

                                    using (var fileStream = new FileStream(fileName, FileMode.Create))
                                    {
                                        file.CopyTo(fileStream);



                                        svSQL = "UPDATE EMPMAST SET IMGPATH='" + fileName1 + "' WHERE EMPMASTID='" + Pid + "'";
                                        OracleCommand objCmdss = new OracleCommand(svSQL, objConn);
                                        objCmdss.ExecuteNonQuery();

                                        r++;
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
        public DataTable GetState()
        {
            string SvSql = string.Empty;
            SvSql = "select STATE,STATEMASTID from STATEMAST ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;

        }
        public DataTable GetCity(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select CITYNAME,CITYID from CITYMASTER where IS_ACTIVE = 'Y' "; /*where STATEID ='"  + id +"'";*/
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetCityst(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select CITYNAME,CITYID from CITYMASTER where STATEID =  '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEmployee(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPMAST.EMPNAME,EMPMAST.EMPID,IMGPATH,EMPMAST.EMPSEX,to_char(EMPMAST.EMPDOB,'dd-MON-yyyy')EMPDOB,WOFF,EMPMAST.ECADD1,EMPMAST.ECCITY,ESIAPP,PFAPP,EMPMAST.ECSTATE,EMPMAST.ECMAILID,EMPMAST.ECPHNO,EMPMAST.FATHERNAME,EMPMAST.MOTHERNAME,EMPMAST.GAURDNAME,EMPMAST.ECADD1,ECPCODE,EMPMAST.ECADD2,EMPMAST.ECPCODE,EMPMAST.EMPPAYCAT,EMPMAST.EMPBASIC,EMPMAST.PFNO,EMPMAST.ESINO,EMPMAST.EMPCOST,to_char(EMPMAST.PFDT,'dd-MON-yyyy')PFDT,to_char(EMPMAST.ESIDT,'dd-MON-yyyy')ESIDT,USERNAME,PASSWORD,EMPDEPT,EMPDESIGN,EMPDEPTCODE,to_char(EMPMAST.JOINDATE,'dd-MON-yyyy')JOINDATE,to_char(EMPMAST.RESIGNDATE,'dd-MON-yyyy')RESIGNDATE,EMPMASTID,BRANCHID,EMPDEPTCODE,EMPPAYCAT,PAYMODE,BANK,SHIFTCATEGORY,ESINO,EACTIVE,BONAPP,CLAPP,PFCLOSE,EMPCOST,OTYN,BANKACCNO,MEALSYN,APPRENTICE,LOPYN,HANDICAPPED  from EMPMAST where EMPMAST.EMPMASTID= '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEmpEduDeatils(string data)
        {
            string SvSql = string.Empty;

            //SvSql = "Select EMPMEDU.EDUCATION,UC,EMPMEDU.ECPLACE,to_char(EMPMEDU.YRPASSING,'dd-MON-yyyy')YRPASSING,EMPMEDU.MPER,EMPMEDUID  from EMPMEDU where EMPMEDU.EMPMASTID=" + data + "";

            SvSql = "Select EMPMEDU.EDUCATION, EMPMEDU.UC,EMPMEDU.ECPLACE,to_char(EMPMEDU.YRPASSING,'dd-MON-yyyy')YRPASSING,EMPMEDU.MPER,EMPMEDUID  from EMPMEDU where EMPMEDU.EMPMASTID=" + data + "";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEmpPersonalDeatils(string data)
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPMOI.MARITALSTATUS,EMPMOI.BLOODGROUP,EMPMOI.COMMUNITY,EMPMOI.PAYTYPE,EMPMOI.EMPTYPE,EMPMOI.DISP,EMPMOIID,OPFNO,NOOFDEP,OPFFDT,OPFTODT,ADVACC  from EMPMOI where EMPMOI.EMPMASTID=" + data + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEmpSkillDeatils(string data)
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPMSS.SKILL,EMPMSSID  from EMPMSS where EMPMSS.EMPMASTID =" + data + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;

        }
        public DataTable GetPayCodeDeatils(string data)
        {
            string SvSql = string.Empty;
            SvSql = "Select PAYCODE,FORMULA  from EMPPAYCODE where EMPPAYCODE.EMPMASTID=" + data + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetPrvHisDeatils(string data)
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPLOYER,CITY,EDESIG,LSALARYDRAWN,WM  from EMPMPH where EMPMPH.EMPMASTID=" + data + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetInsuranceDeatils(string data)
        {
            string SvSql = string.Empty;
            SvSql = "Select POLICYNO,NATUREOFPOLICY,BADD,ACTPREMIUM,PREMIUM,PSTDT,PEDDT  from EMPMINS where EMPMINS.EMPMASTID=" + data + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEmrgConDeatils(string data)
        {
            string SvSql = string.Empty;
            SvSql = "Select ECNAME,NREL,ECPHONE,ECMOBILE,ECFAX  from EMPMEC where EMPMEC.EMPMASTID=" + data + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPeforDeatils(string data)
        {
            string SvSql = string.Empty;
            SvSql = "Select PERFDESC,RESULT,RATING,AWDGN  from EMPMREWARDS where EMPMREWARDS.EMPMASTID=" + data + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDCode()
        {
            string SvSql = string.Empty;
            SvSql = "select DDBASICID,DEPTCODE from DDBASIC  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetDepCodeDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select DEPTCODE from EMPDCDETAIL where EMPMASTID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetBr()
        {
            string SvSql = string.Empty;
            SvSql = "select BRANCHMASTID,BRANCHID from BRANCHMAST  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetBranchDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select BRANCH from EMPBCDETAIL where EMPMASTID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEmpAtted(string data)
        {
            string SvSql = string.Empty;
            SvSql = "Select AMONTH,PDAYS,ADAYS,LDAYS,NHD,NHW,WO,WDAYS,HDAYS  from EMPATT where EMPATT.EMPMASTID=" + data + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }


        public DataTable GetCurrentUser(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPNAME  from EMPMAST where  EMPMASTID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEMPDept()
        {
            string SvSql = string.Empty;
            SvSql = "select DEPTCODE,DDBASICID from DDBASIC    ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetDesign()
        {
            string SvSql = string.Empty;
            SvSql = "select DESIGNATION,PDESGID from PDESG where  IS_ACTIVE= 'Y'  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public async Task<IEnumerable<EmployeeDetails>> GetEmployeeDetails(string id)
        {
            using (OracleConnection db = new OracleConnection(_connectionString))
            {
                return await db.QueryAsync<EmployeeDetails>("SELECT EMPMAST.EMPMASTID, EMPMAST.APPROVAL, EMPMAST.PROFITDOC, EMPMAST.APPROVALSTATUS, EMPMAST.MAXAPPROVED, EMPMAST.CANCEL,EMPMAST.T1SOURCEID, EMPMAST.LATEMPLATEID, EMPMAST.EMPID, EMPMAST.EMPNAME, EMPMAST.EMPSEX, EMPMAST.EMPDOB, to_char(EMPMAST.DOB,'dd-MON-yyyy')DOB,to_char(EMPMAST.JOINDATE,'dd-MON-yyyy')JOINDATE, EMPMAST.EACTIVE, EMPMAST.APPRENTICE, EMPMAST.EMPDESIGN, EMPMAST.EMPDEPTCODE, PCBASIC.PAYCATEGORY AS EMPPAYCAT,EMPMAST.EMPBASIC, EMPMAST.PFNO, EMPMAST.ESINO, DDBASIC.DEPTNAME as EMPDEPT, EMPMAST.EMPCOST, EMPMAST.USERID, EMPMAST.BRANCHID, EMPMAST.ECADD1 ||','|| EMPMAST.ECADD2 as ADDRESS, EMPMAST.ECCITY, EMPMAST.ECSTATE, EMPMAST.ECMAILID, EMPMAST.ECPHNO, EMPMAST.ECPCODE, EMPMAST.EMPADD1, EMPMAST.EMPADD2, EMPMAST.EPCITY, EMPMAST.EPSTATE, EMPMAST.EPINCODE, EMPMAST.EPPHNO, EMPMAST.OTPERHR,EMPMAST.PFDT, EMPMAST.ESIDT, EMPMAST.RESIGNDATE, EMPMAST.FATHERNAME, EMPMAST.MOTHERNAME, EMPMAST.HANDICAPPED, EMPMAST.PFAPP, EMPMAST.ESIAPP, EMPMAST.LOPYN, EMPMAST.OTYN, EMPMAST.NEMPID, EMPMAST.USERNAME, EMPMAST.SHIFTCATEGORY, EMPMAST.WOFF, EMPMAST.BUSFYN, EMPMAST.BUSFPERDAY, EMPMAST.MEALSYN, EMPMAST.BANK ||' & '|| EMPMAST.BANKACCNO as BANK, EMPMAST.PEMPID,EMPMAST.GAURDNAME, EMPMAST.PFCLOSE, EMPMAST.EXTNNO, EMPMAST.BASAMT, EMPMAST.INTRMAILID, EMPMAST.PAYMODE, EMPMAST.UDAINO,EMPMAST.BONAPP, EMPMAST.CLAPP, EMPMAST.PASSWORD, EMPMAST.IS_ACTIVE, EMPMAST.CREATED_BY, EMPMAST.CREATED_ON,EMPMAST.UPDATED_BY, EMPMAST.UPDATED_ON, EMPMAST.PDESGID, EMPMAST.PDEPTID, EMPMAST.IMGPATH FROM EMPMAST LEFT OUTER JOIN DDBASIC ON DDBASIC.DDBASICID=EMPMAST.EMPDEPT LEFT OUTER JOIN PCBASIC ON PCBASIC.PCBASICID=EMPMAST.EMPPAYCAT WHERE EMPMAST.EMPMASTID ='" + id + "'", commandType: CommandType.Text);
            }
        }
        public async Task<IEnumerable<EmpEduDetails>> GetEmpEduDetails(string id)
        {
            using (OracleConnection db = new OracleConnection(_connectionString))
            {
                return await db.QueryAsync<EmpEduDetails>("SELECT EMPMEDUID, EMPMASTID, EMPMEDUROW, EDUCATION, UC, ECPLACE, to_char(YRPASSING,'dd-MON-yyyy')YRPASSING, MPER FROM EMPMEDU  WHERE EMPMEDU.EMPMASTID ='" + id + "'", commandType: CommandType.Text);
            }
        }
        public async Task<IEnumerable<EmpOthDetails>> GetEmpOthDetails(string id)
        {
            using (OracleConnection db = new OracleConnection(_connectionString))
            {
                return await db.QueryAsync<EmpOthDetails>("SELECT EMPMOIID, EMPMASTID, MARITALSTATUS, NOOFDEP, BLOODGROUP, COMMUNITY, VCHBASIC, PAYTYPE, EMPTYPE, DISP, OPFNO, OPFFDT, OPFTODT, ADVACC FROM EMPMOI  WHERE EMPMOI.EMPMASTID ='" + id + "'", commandType: CommandType.Text);
            }
        }
        public string GetMultipleLocation(MultipleLocation mp)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    objConn.Open();
                    if (mp.Location != null)
                    {
                        string EmpID = mp.ID;//datatrans.GetDataString("Select EMPMASTID from EMPMAST where EMPNAME='" + mp.EmpName + "' AND EACTIVE='Yes' ");
                        string dt = datatrans.GetDataString("Select EMPID from EMPLOYEELOCATION WHERE EMPID='" + mp.ID + "'");
                        //string loc = dt.Rows[0]["LOCID"].ToString();
                        if (EmpID == dt)
                        {
                            string Sql = string.Empty;
                            Sql = "DELETE FROM employeelocation WHERE empid = '" + EmpID + "'";
                            OracleCommand objCmds = new OracleCommand(Sql, objConn);

                            objCmds.ExecuteNonQuery();

                        }
                        for (int i = 0; i < mp.Location.Length; i++)
                        {
                            OracleCommand objCmd = new OracleCommand("EMPLOCATIONPROC", objConn);
                            /*objCmd.Connection = objConn;
                            objCmd.CommandText = "MULTIPLELOCATIONPROC";*/

                            objCmd.CommandType = CommandType.StoredProcedure;

                            StatementType = "Insert";
                            objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                            objCmd.Parameters.Add("EMPID", OracleDbType.NVarchar2).Value = EmpID;
                            objCmd.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = mp.Location[i];

                            objCmd.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                            objCmd.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = mp.CreadtedBy;




                            objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;


                            try
                            {

                                objCmd.ExecuteNonQuery();
                                //System.Console.WriteLine("Number of employees in department 20 is {0}", objCmd.Parameters["pout_count"].Value);
                            }
                            catch (Exception ex)
                            {
                                //System.Console.WriteLine("Exception: {0}", ex.ToString());
                            }

                        }
                        objConn.Close();
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

        public string StatusChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE EMPMAST SET IS_ACTIVE ='N' WHERE EMPMASTID='" + id + "'";
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
        public string RemoveChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE EMPMAST SET IS_ACTIVE ='Y' WHERE EMPMASTID='" + id + "'";
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

        public DataTable GetAllEmployee(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select EMPMAST.IS_ACTIVE,EMPMAST.EMPNAME,EMPMAST.EMPID,EMPMAST.EMPSEX,to_char(EMPMAST.EMPDOB,'dd-MON-yyyy')EMPDOB,EMPMAST.ECMAILID,EMPMAST.ECPHNO,EMPMASTID,GAURDNAME,EMPADD1,EMPADD2,EPINCODE,ECMAILID,DDBASIC.DEPTNAME  from EMPMAST LEFT OUTER JOIN DDBASIC ON DDBASIC.DDBASICID=EMPMAST.EMPDEPT WHERE EMPMAST.IS_ACTIVE = 'Y' AND EACTIVE='Yes'  ORDER BY EMPMASTID DESC ";

            }
            else
            {
                SvSql = "Select EMPMAST.IS_ACTIVE,EMPMAST.EMPNAME,EMPMAST.EMPID,EMPMAST.EMPSEX,to_char(EMPMAST.EMPDOB,'dd-MON-yyyy')EMPDOB,EMPMAST.ECMAILID,EMPMAST.ECPHNO,EMPMASTID  from EMPMAST WHERE EMPMAST.IS_ACTIVE = 'N' ORDER BY EMPMASTID DESC ";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string AddBloodGroupCRUD(string category)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                svSQL = " SELECT Count(COMMON_VALUE) as cnt FROM COMMONMASTER WHERE COMMON_VALUE =LTRIM(RTRIM('" + category + "')) AND COMMON_TEXT='BLOODGROUP'";
                if (datatrans.GetDataId(svSQL) > 0)
                {
                    msg = "BLOODGROUP Already Existed";
                    return msg;
                }
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    svSQL = "Insert into COMMONMASTER (COMMON_TEXT,COMMON_VALUE) VALUES ('BLOODGROUP','" + category + "')";

                    OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                    objConn.Open();
                    objCmds.ExecuteNonQuery();
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
        public string AddCommunityCRUD(string category)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                svSQL = " SELECT Count(COMMON_VALUE) as cnt FROM COMMONMASTER WHERE COMMON_VALUE =LTRIM(RTRIM('" + category + "')) AND COMMON_TEXT='COMMUNITY'";
                if (datatrans.GetDataId(svSQL) > 0)
                {
                    msg = " COMMUNITY Already Existed";
                    return msg;
                }
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    svSQL = "Insert into COMMONMASTER (COMMON_TEXT,COMMON_VALUE) VALUES ('COMMUNITY','" + category + "')";

                    OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                    objConn.Open();
                    objCmds.ExecuteNonQuery();
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
        public string AddDispCRUD(string category)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                svSQL = " SELECT Count(COMMON_VALUE) as cnt FROM COMMONMASTER WHERE COMMON_VALUE =LTRIM(RTRIM('" + category + "')) AND COMMON_TEXT='CITY'";
                if (datatrans.GetDataId(svSQL) > 0)
                {
                    msg = "CITY Already Existed";
                    return msg;
                }
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    svSQL = "Insert into COMMONMASTER (COMMON_TEXT,COMMON_VALUE) VALUES ('CITY','" + category + "')";

                    OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                    objConn.Open();
                    objCmds.ExecuteNonQuery();
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
        public string AddBankCRUD(string category)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                svSQL = " SELECT Count(COMMON_VALUE) as cnt FROM COMMONMASTER WHERE COMMON_VALUE =LTRIM(RTRIM('" + category + "')) AND COMMON_TEXT='BANKNAME'";
                if (datatrans.GetDataId(svSQL) > 0)
                {
                    msg = "BANKNAME Already Existed";
                    return msg;
                }
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    svSQL = "Insert into COMMONMASTER (COMMON_TEXT,COMMON_VALUE) VALUES ('BANKNAME','" + category + "')";

                    OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                    objConn.Open();
                    objCmds.ExecuteNonQuery();
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
        public static string Encrypt(string plainText)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = GetKey(EncryptionKey);  // Ensure correct key size
                aes.IV = GetIV(IV);               // Ensure IV is 16 bytes

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (var sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                    }
                    // Return Base64-encoded string
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }
        public string Changepass(Employee ss)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";

                string encpass = Encrypt(ss.newpass);
                string pass =datatrans.GetDataString("SELECT PASSWORD FROM EMPMAST WHERE EMPMASTID='"+ss.createby +"' ");
               string oldpass = Decrypt(pass);
                if (oldpass != ss.oldpass)
                {

                     
                        msg = "Old Password is not match";
                        return msg;
                    
               }
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    objConn.Open();
                    svSQL = "UPDATE  EMPMAST SET USERNAME='" + ss.UserName + "',PASSWORD='" + encpass + "' WHERE EMPMASTID='" + ss.createby + "'";
                    OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                    objCmdd.ExecuteNonQuery();
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
        private static byte[] GetKey(string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            // If the key is too long, truncate it
            if (keyBytes.Length > 32)
            {
                Array.Resize(ref keyBytes, 32); // For AES-256
            }
            // If the key is too short, pad it with zeros
            else if (keyBytes.Length < 32)
            {
                Array.Resize(ref keyBytes, 32); // For AES-256
            }

            return keyBytes;
        }
        // Function to adjust the IV to 16 bytes (128-bit block size for AES)
        private static byte[] GetIV(string iv)
        {
            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);

            // If the IV is too long, truncate it
            if (ivBytes.Length > 16)
            {
                Array.Resize(ref ivBytes, 16); // IV must be 16 bytes for AES
            }
            // If the IV is too short, pad it with zeros
            else if (ivBytes.Length < 16)
            {
                Array.Resize(ref ivBytes, 16); // IV must be 16 bytes for AES
            }

            return ivBytes;
        }

        public static string Decrypt(string cipherText)
        {
            try
            {
                // Ensure the cipher text is Base64 encoded
                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                using (var aes = Aes.Create())
                {
                    aes.Key = GetKey(EncryptionKey);  // Ensure correct key size
                    aes.IV = GetIV(IV);               // Ensure IV is 16 bytes

                    var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                    using (var ms = new MemoryStream(cipherBytes))
                    {
                        using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (var sr = new StreamReader(cs))
                            {
                                return sr.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (FormatException)
            {
                // Handle invalid Base64 string case
                throw new ArgumentException("Input is not a valid Base64 string.");
            }
            catch (CryptographicException ex)
            {
                // Handle encryption-specific exceptions
                throw new CryptographicException("Decryption failed. The input may have been tampered with or the wrong key/IV was used.", ex);
            }
        }
    }
}
