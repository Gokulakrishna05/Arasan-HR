using Arasan.Models.Transaction;
using System.Data;

namespace Arasan.Interface.Transaction
{
    public interface IMissingPunchEntry
    {
        DataTable GetAlLMissingPunchEntry(string strStatus);
        DataTable GetEmpName();
        string MissingPunchEntryCRUD(MissingPunchEntry by);
    }
}
