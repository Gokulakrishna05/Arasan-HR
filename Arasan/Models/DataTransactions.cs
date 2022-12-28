using System.Data;
using Oracle.ManagedDataAccess.Client;
using MimeKit;
using System.IO;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using Arasan.Interface;
using Arasan.Models;
using Org.BouncyCastle.Crypto.Macs;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Arasan.Models
{
    public class DataTransactions
    {
        private readonly string _connectionString;
        public DataTransactions(string connectionString)
        {
            //_connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            _connectionString = connectionString;// _configuratio.GetConnectionString("OracleDBConnection");
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

      
        public int GetFinancialYear(DateTime Date1)
        {
            if (Date1.Year > 2000)
            {
                if (Date1.Month > 3)
                {
                    return new DateTime(Date1.Year, 4, 1).Year;
                }
                else
                {
                    return new DateTime(Date1.Year - 1, 4, 1).Year;
                }
            }
            return Date1.Year;
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
        public DataTable GetBranch()
        {
            string SvSql = string.Empty;
            SvSql = "select BRANCHMASTID,BRANCHID from BRANCHMAST order by BRANCHMASTID asc";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetLocation()
        {
            string SvSql = string.Empty;
            SvSql = "Select LOCID,LOCDETAILSID from LOCDETAILS ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEmailConfig()
        {
            string SvSql = "Select ID,SMTP_HOST,PORT_NO,SIGNATURE,SSL,EMAIL_ID,PASSWORD from EMAIL_CONFIG where IS_ACTIVE = 'Y'";

            DataTable dtCity = new DataTable();
            dtCity = GetData(SvSql);
            return dtCity;
        }
        public DataTable GetSupplier()
        {
            string SvSql = string.Empty;
            SvSql = "Select PARTYMAST.PARTYMASTID,PARTYRCODE.PARTY from PARTYMAST LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND PARTYRCODE.PARTY IS NOT NULL";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);

            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetCurency()
        {
            string SvSql = string.Empty;
            SvSql = "Select MAINCURR || ' - ' || SYMBOL  as Cur,CURRENCYID from CURRENCY";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEmp()
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPID,EMPNAME,EMPMASTID from EMPMAST";
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
        public DataTable GetItemDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "select UNITMAST.UNITID,ITEMID,ITEMDESC,UNITMAST.UNITMASTID,LATPURPRICE,ITEMMASTERPUNIT.CF,ITEMMASTER.LATPURPRICE,QCCOMPFLAG from ITEMMASTER LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID LEFT OUTER JOIN ITEMMASTERPUNIT ON  ITEMMASTER.ITEMMASTERID=ITEMMASTERPUNIT.ITEMMASTERID Where ITEMMASTER.ITEMMASTERID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string GetDataString(String sql)
        {
            DataTable _dt = new DataTable();
            string str = string.Empty;
            try
            {
                _dt = GetData(sql);
                if (_dt.Rows.Count > 0)
                {
                    str = _dt.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return str;
        }
        public void sendemail(string Subject, string Message, string EmailID, string SenderID, string SenderPassword, string CompanySMTPPort, string SmtpEnableSsl, string sSmtpServer, string CompanyName)
        {
            string strerr = "";
            if (EmailID != "" && EmailID != null)
            {
                try
                {
                    MailMessage mail = new MailMessage();
                    System.Net.Mail.SmtpClient SmtpServer = new System.Net.Mail.SmtpClient(sSmtpServer);
                    mail.From = new MailAddress(SenderID, CompanyName);
                    mail.To.Add(EmailID);
                    mail.Subject = Subject;
                    StringBuilder sb3 = new StringBuilder();
                    sb3.Append(Message);
                    mail.Body = sb3.ToString();
                    AlternateView avHtml = AlternateView.CreateAlternateViewFromString(sb3.ToString(), null, MediaTypeNames.Text.Html);
                    mail.AlternateViews.Add(avHtml);
                    mail.IsBodyHtml = true;
                    SmtpServer.Port = Convert.ToInt32(CompanySMTPPort);
                    SmtpServer.Credentials = new System.Net.NetworkCredential(SenderID, SenderPassword);
                    SmtpServer.EnableSsl = Convert.ToBoolean(SmtpEnableSsl);
                    //SmtpServer.Timeout = 10000;
                    SmtpServer.Send(mail);
                    mail.Dispose();
                }
                catch (Exception ex)
                {
                    strerr = ex.Message;
                }
            }
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

       
    }
}
