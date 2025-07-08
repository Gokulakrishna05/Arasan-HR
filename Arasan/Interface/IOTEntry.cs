using Arasan.Models;
using System.Data;

namespace Arasan.Interface
{
    public interface IOTEntry
    {
        DataTable GetOTEntryEdit(string id);
        string OTEntryCRUD(OTEntry Cy);
        DataTable GetEmpName();
        string StatusChange(string tag, string id);
        DataTable GetAllOTEntry(string strStatus);
        string ViewOTEntry(OTEntry Cy);
    }
}
